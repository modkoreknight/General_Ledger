using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interact.BusinessLogic;
using Interact.Common;

namespace Interact.DataAccess
{
    public class SalesProvider : ISalesProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SalesProvider(SqlConnection conn)
        {
            this._conn = conn;
            String[] str = conn.Database.Split('_');
            if (str.Count() > 1)
            {
                if (str[1] == "Audit")
                {
                    this._branch = Branch.Audit_;
                }
                else
                {
                    this._branch = (Branch)Enum.Parse(typeof(Branch), str[1]);
                }
            }

            Int32 branchKey = 0;
            Boolean result = Int32.TryParse(this._conn.Database.Substring(this._conn.Database.Length - 2, 2), out branchKey);
            if (result)
            {
                this._isLocal = false;
                this._branch = (Branch)Enum.Parse(typeof(Branch), branchKey.ToString());
            }
            else
            {
                this._isLocal = true;
            }
        }
        #endregion

        #region Methods
        public Int32 GetSalesPageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesGetPageCount";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@PageSize", SqlDbType.TinyInt);
            myParam1.Value = Utility.PageSize;
            SqlParameter myParam2 = new SqlParameter("@Output", SqlDbType.TinyInt);
            myParam2.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                }
                catch
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                this._conn.Close();
            }
            return Convert.ToInt32(myParam2.Value);
        }

        public Sales GetSales(Int32 id)
        {
            Sales sales = Sales.CreateSales();
            ICustomerProvider customerProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    customerProvider = new CustomerProvider(Database.AuditConnection());
                }
                else
                {
                    customerProvider = new CustomerProvider(Database.GeneralLedger);
                }
            }
            else
            {
                customerProvider = new CustomerProvider(Database.BranchConnection(this._branch));
            }
            CustomerManager customerManager = new CustomerManager(customerProvider);
            IVehicleProvider vehicleProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    vehicleProvider = new VehicleProvider(Database.AuditConnection());
                }
                else
                {
                    vehicleProvider = new VehicleProvider(Database.GeneralLedger);
                }
            }
            else
            {
                vehicleProvider = new VehicleProvider(Database.BranchConnection(this._branch));
            }
            VehicleManager vehicleManager = new VehicleManager(vehicleProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = id;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            myReader.Read();
                            sales.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                sales.SaleCode = String.Empty;
                            }
                            else
                            {
                                sales.SaleCode = myReader.GetString(1);
                            }
                            if (myReader.IsDBNull(2))
                            {
                                sales.SaleDate = DateTime.Today;
                            }
                            else
                            {
                                sales.SaleDate = myReader.GetDateTime(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                sales.SaleAmount = 0;
                            }
                            else
                            {
                                sales.SaleAmount = myReader.GetDecimal(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                sales.Customer = null;
                            }
                            else
                            {
                                sales.Customer = customerManager.GetCustomer(myReader.GetInt32(4));
                            }
                            if (myReader.IsDBNull(5))
                            {
                                sales.Vehicle = null;
                            }
                            else
                            {
                                sales.Vehicle = vehicleManager.GetVehicle(myReader.GetInt32(5));
                            }
                            if (myReader.IsDBNull(6))
                            {
                                sales.Term = 0;
                            }
                            else
                            {
                                sales.Term = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), myReader.GetInt32(6).ToString());
                            }
                            if (myReader.IsDBNull(7))
                            {
                                sales.TermTotal = 0;
                            }
                            else
                            {
                                sales.TermTotal = myReader.GetInt32(7);
                            }
                            if (myReader.IsDBNull(8))
                            {
                                sales.AmortStartDate = DateTime.Today;
                            }
                            else
                            {
                                sales.AmortStartDate = myReader.GetDateTime(8);
                            }
                            if (myReader.IsDBNull(9))
                            {
                                sales.AmortAmount = 0;
                            }
                            else
                            {
                                sales.AmortAmount = myReader.GetDecimal(9);
                            }
                            if (myReader.IsDBNull(10))
                            {
                                sales.AmortRebate = 0;
                            }
                            else
                            {
                                sales.AmortRebate = myReader.GetDecimal(10);
                            }
                            if (myReader.IsDBNull(11))
                            {
                                sales.Status = 0;
                            }
                            else
                            {
                                sales.Status = (SalesStatus)Enum.Parse(typeof(SalesStatus), myReader.GetInt32(11).ToString());
                            }
                            if (myReader.IsDBNull(12))
                            {
                                sales.CashPrice = 0;
                            }
                            else
                            {
                                sales.CashPrice = myReader.GetDecimal(12);
                            }
                            if (myReader.IsDBNull(13))
                            {
                                sales.LCP = 0;
                            }
                            else
                            {
                                sales.LCP = myReader.GetDecimal(13);
                            }
                            if (myReader.IsDBNull(14))
                            {
                                sales.InvoiceNo = String.Empty;
                            }
                            else
                            {
                                sales.InvoiceNo = myReader.GetString(14);
                            }
                            if (myReader.IsDBNull(15))
                            {
                                sales.InvoiceDate = DateTime.Today;
                            }
                            else
                            {
                                sales.InvoiceDate = myReader.GetDateTime(15);
                            }
                            if (myReader.IsDBNull(16))
                            {
                                sales.InterestRate = 0;
                            }
                            else
                            {
                                sales.InterestRate = myReader.GetDecimal(16);
                            }
                            if (myReader.IsDBNull(17))
                            {
                                sales.Remarks = String.Empty;
                            }
                            else
                            {
                                sales.Remarks = myReader.GetString(17);
                            }
                            if (myReader.IsDBNull(18))
                            {
                                sales.BalanceFwd = 0;
                            }
                            else
                            {
                                sales.BalanceFwd = myReader.GetDecimal(18);
                            }
                            if (myReader.IsDBNull(19))
                            {
                                sales.DueDate01 = DateTime.Today;
                            }
                            else
                            {
                                sales.DueDate01 = myReader.GetDateTime(19);
                            }
                            if (myReader.IsDBNull(20))
                            {
                                sales.DueDate02 = DateTime.Today;
                            }
                            else
                            {
                                sales.DueDate02 = myReader.GetDateTime(20);
                            }
                            if (myReader.FieldCount > 21)
                            {
                                if (myReader.IsDBNull(21))
                                {
                                    sales.Branch = 0;
                                }
                                else
                                {
                                    sales.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(21).ToString());
                                }
                            }
                            if (myReader.FieldCount > 22)
                            {
                                if (myReader.IsDBNull(22))
                                {
                                    sales.AuditID = 0;
                                }
                                else
                                {
                                    sales.AuditID = myReader.GetInt32(22);
                                }
                            }
                            sales.Branch = this._branch;
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sales;
        }

        public GenericList<Sales> GetAllSales()
        {
            GenericList<Sales> allSales = new GenericList<Sales>();
            //ICustomerProvider customerProvider;
            //if (this._isLocal)
            //{
            //    customerProvider = new CustomerProvider(Database.GeneralLedger);
            //}
            //else
            //{
            //    customerProvider = new CustomerProvider(Database.BranchConnection(this._branch));
            //}
            //CustomerManager customerManager = new CustomerManager(customerProvider);
            //IVehicleProvider vehicleProvider;
            //if (this._isLocal)
            //{
            //    vehicleProvider = new VehicleProvider(Database.GeneralLedger);
            //}
            //else
            //{
            //    vehicleProvider = new VehicleProvider(Database.BranchConnection(this._branch));
            //}
            //VehicleManager vehicleManager = new VehicleManager(vehicleProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesSelect";
            myCommand.Connection = this._conn;
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Sales sales = Sales.CreateSales();
                                sales.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    sales.SaleCode = String.Empty;
                                }
                                else
                                {
                                    sales.SaleCode = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    sales.SaleDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.SaleDate = myReader.GetDateTime(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    sales.SaleAmount = 0;
                                }
                                else
                                {
                                    sales.SaleAmount = myReader.GetDecimal(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    sales.Customer = null;
                                }
                                else
                                {
                                    //sales.Customer = customerManager.GetCustomer(myReader.GetInt32(4));
                                    sales.Customer = Customer.CreateCustomer();
                                    sales.Customer.ID = myReader.GetInt32(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    sales.Vehicle = null;
                                }
                                else
                                {
                                    //sales.Vehicle = vehicleManager.GetVehicle(myReader.GetInt32(5));
                                    sales.Vehicle = Vehicle.CreateVehicle();
                                    sales.Vehicle.ID = myReader.GetInt32(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    sales.Term = 0;
                                }
                                else
                                {
                                    sales.Term = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    sales.TermTotal = 0;
                                }
                                else
                                {
                                    sales.TermTotal = myReader.GetInt32(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    sales.AmortStartDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.AmortStartDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    sales.AmortAmount = 0;
                                }
                                else
                                {
                                    sales.AmortAmount = myReader.GetDecimal(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    sales.AmortRebate = 0;
                                }
                                else
                                {
                                    sales.AmortRebate = myReader.GetDecimal(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    sales.Status = 0;
                                }
                                else
                                {
                                    sales.Status = (SalesStatus)Enum.Parse(typeof(SalesStatus), myReader.GetInt32(11).ToString());
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    sales.CashPrice = 0;
                                }
                                else
                                {
                                    sales.CashPrice = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    sales.LCP = 0;
                                }
                                else
                                {
                                    sales.LCP = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    sales.InvoiceNo = String.Empty;
                                }
                                else
                                {
                                    sales.InvoiceNo = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    sales.InvoiceDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.InvoiceDate = myReader.GetDateTime(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    sales.InterestRate = 0;
                                }
                                else
                                {
                                    sales.InterestRate = myReader.GetDecimal(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    sales.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Remarks = myReader.GetString(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    sales.BalanceFwd = 0;
                                }
                                else
                                {
                                    sales.BalanceFwd = myReader.GetDecimal(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    sales.DueDate01 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate01 = myReader.GetDateTime(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    sales.DueDate02 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate02 = myReader.GetDateTime(20);
                                }
                                if (myReader.IsDBNull(21))
                                {
                                    sales.Vehicle.Status = 0;
                                }
                                else
                                {
                                    sales.Vehicle.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(21).ToString());
                                }
                                if (myReader.IsDBNull(22))
                                {
                                    sales.Vehicle.Code = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Code = myReader.GetString(22);
                                }
                                if (myReader.IsDBNull(23))
                                {
                                    sales.Vehicle.Brand = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Brand = myReader.GetString(23);
                                }
                                if (myReader.IsDBNull(24))
                                {
                                    sales.Vehicle.Model = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Model = myReader.GetString(24);
                                }
                                if (myReader.IsDBNull(25))
                                {
                                    sales.Vehicle.Color = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Color = myReader.GetString(25);
                                }
                                if (myReader.IsDBNull(26))
                                {
                                    sales.Vehicle.EngineNo = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.EngineNo = myReader.GetString(26);
                                }
                                if (myReader.IsDBNull(27))
                                {
                                    sales.Vehicle.ChassisNo = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.ChassisNo = myReader.GetString(27);
                                }
                                if (myReader.IsDBNull(28))
                                {
                                    sales.Vehicle.PlateNo = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.PlateNo = myReader.GetString(28);
                                }
                                if (myReader.IsDBNull(29))
                                {
                                    sales.Vehicle.CertReg = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.CertReg = myReader.GetString(29);
                                }
                                if (myReader.IsDBNull(30))
                                {
                                    sales.Vehicle.OwnerReg = null;
                                }
                                else
                                {
                                    sales.Vehicle.OwnerReg = Customer.CreateCustomer();
                                    sales.Vehicle.OwnerReg.ID = myReader.GetInt32(30);
                                }
                                if (myReader.IsDBNull(31))
                                {
                                    sales.Vehicle.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Remarks = myReader.GetString(31);
                                }
                                if (myReader.IsDBNull(32))
                                {
                                    sales.Customer.LastName = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.LastName = myReader.GetString(32);
                                }
                                if (myReader.IsDBNull(33))
                                {
                                    sales.Customer.FirstName = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.FirstName = myReader.GetString(33);
                                }
                                if (myReader.IsDBNull(34))
                                {
                                    sales.Customer.MiddleName = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.MiddleName = myReader.GetString(34);
                                }
                                if (myReader.IsDBNull(35))
                                {
                                    sales.Customer.Address = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.Address = myReader.GetString(35);
                                }
                                if (myReader.IsDBNull(36))
                                {
                                    sales.Customer.Zone = null;
                                }
                                else
                                {
                                    sales.Customer.Zone = Zone.CreateZone();
                                    sales.Customer.Zone.ID = myReader.GetInt32(36);
                                }
                                if (myReader.IsDBNull(37))
                                {
                                    sales.Customer.Phone = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.Phone = myReader.GetString(37);
                                }
                                if (myReader.IsDBNull(38))
                                {
                                    sales.Customer.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.Remarks = myReader.GetString(38);
                                }
                                if (myReader.FieldCount > 39)
                                {
                                    if (myReader.IsDBNull(39))
                                    {
                                        sales.Branch = 0;
                                    }
                                    else
                                    {
                                        sales.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(39).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 40)
                                {
                                    if (myReader.IsDBNull(40))
                                    {
                                        sales.AuditID = 0;
                                    }
                                    else
                                    {
                                        sales.AuditID = myReader.GetInt32(40);
                                    }
                                }
                                allSales.Add(sales);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allSales;
        }

        public GenericList<Sales> GetAllSales(Customer customer)
        {
            GenericList<Sales> allSales = new GenericList<Sales>();
            IVehicleProvider vehicleProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    vehicleProvider = new VehicleProvider(Database.AuditConnection());
                }
                else
                {
                    vehicleProvider = new VehicleProvider(Database.GeneralLedger);
                }
            }
            else
            {
                vehicleProvider = new VehicleProvider(Database.BranchConnection(this._branch));
            }
            VehicleManager vehicleManager = new VehicleManager(vehicleProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@CustomerID", SqlDbType.Int);
            myParam1.Value = customer.ID;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Sales sales = Sales.CreateSales();
                                sales.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    sales.SaleCode = String.Empty;
                                }
                                else
                                {
                                    sales.SaleCode = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    sales.SaleDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.SaleDate = myReader.GetDateTime(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    sales.SaleAmount = 0;
                                }
                                else
                                {
                                    sales.SaleAmount = myReader.GetDecimal(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    sales.Customer = null;
                                }
                                else
                                {
                                    sales.Customer = customer;
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    sales.Vehicle = null;
                                }
                                else
                                {
                                    sales.Vehicle = vehicleManager.GetVehicle(myReader.GetInt32(5));
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    sales.Term = 0;
                                }
                                else
                                {
                                    sales.Term = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    sales.TermTotal = 0;
                                }
                                else
                                {
                                    sales.TermTotal = myReader.GetInt32(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    sales.AmortStartDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.AmortStartDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    sales.AmortAmount = 0;
                                }
                                else
                                {
                                    sales.AmortAmount = myReader.GetDecimal(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    sales.AmortRebate = 0;
                                }
                                else
                                {
                                    sales.AmortRebate = myReader.GetDecimal(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    sales.Status = 0;
                                }
                                else
                                {
                                    sales.Status = (SalesStatus)Enum.Parse(typeof(SalesStatus), myReader.GetInt32(11).ToString());
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    sales.CashPrice = 0;
                                }
                                else
                                {
                                    sales.CashPrice = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    sales.LCP = 0;
                                }
                                else
                                {
                                    sales.LCP = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    sales.InvoiceNo = String.Empty;
                                }
                                else
                                {
                                    sales.InvoiceNo = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    sales.InvoiceDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.InvoiceDate = myReader.GetDateTime(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    sales.InterestRate = 0;
                                }
                                else
                                {
                                    sales.InterestRate = myReader.GetDecimal(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    sales.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Remarks = myReader.GetString(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    sales.BalanceFwd = 0;
                                }
                                else
                                {
                                    sales.BalanceFwd = myReader.GetDecimal(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    sales.DueDate01 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate01 = myReader.GetDateTime(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    sales.DueDate02 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate02 = myReader.GetDateTime(20);
                                }
                                if (myReader.FieldCount > 21)
                                {
                                    if (myReader.IsDBNull(21))
                                    {
                                        sales.Branch = 0;
                                    }
                                    else
                                    {
                                        sales.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(21).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 22)
                                {
                                    if (myReader.IsDBNull(22))
                                    {
                                        sales.AuditID = 0;
                                    }
                                    else
                                    {
                                        sales.AuditID = myReader.GetInt32(22);
                                    }
                                }
                                allSales.Add(sales);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allSales;
        }

        public GenericList<Sales> GetAllSales(SalesStatus status)
        {
            GenericList<Sales> allSales = new GenericList<Sales>();
            //ICustomerProvider customerProvider;
            //if (this._isLocal)
            //{
            //    customerProvider = new CustomerProvider(Database.GeneralLedger);
            //}
            //else
            //{
            //    customerProvider = new CustomerProvider(Database.BranchConnection(this._branch));
            //}
            //CustomerManager customerManager = new CustomerManager(customerProvider);
            //IVehicleProvider vehicleProvider;
            //if (this._isLocal)
            //{
            //    vehicleProvider = new VehicleProvider(Database.GeneralLedger);
            //}
            //else
            //{
            //    vehicleProvider = new VehicleProvider(Database.BranchConnection(this._branch));
            //}
            //VehicleManager vehicleManager = new VehicleManager(vehicleProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@Status", SqlDbType.Int);
            myParam1.Value = status;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Sales sales = Sales.CreateSales();
                                sales.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    sales.SaleCode = String.Empty;
                                }
                                else
                                {
                                    sales.SaleCode = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    sales.SaleDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.SaleDate = myReader.GetDateTime(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    sales.SaleAmount = 0;
                                }
                                else
                                {
                                    sales.SaleAmount = myReader.GetDecimal(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    sales.Customer = null;
                                }
                                else
                                {
                                    //sales.Customer = customerManager.GetCustomer(myReader.GetInt32(4));
                                    sales.Customer = Customer.CreateCustomer();
                                    sales.Customer.ID = myReader.GetInt32(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    sales.Vehicle = null;
                                }
                                else
                                {
                                    //sales.Vehicle = vehicleManager.GetVehicle(myReader.GetInt32(5));
                                    sales.Vehicle = Vehicle.CreateVehicle();
                                    sales.Vehicle.ID = myReader.GetInt32(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    sales.Term = 0;
                                }
                                else
                                {
                                    sales.Term = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    sales.TermTotal = 0;
                                }
                                else
                                {
                                    sales.TermTotal = myReader.GetInt32(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    sales.AmortStartDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.AmortStartDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    sales.AmortAmount = 0;
                                }
                                else
                                {
                                    sales.AmortAmount = myReader.GetDecimal(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    sales.AmortRebate = 0;
                                }
                                else
                                {
                                    sales.AmortRebate = myReader.GetDecimal(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    sales.Status = 0;
                                }
                                else
                                {
                                    sales.Status = (SalesStatus)Enum.Parse(typeof(SalesStatus), myReader.GetInt32(11).ToString());
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    sales.CashPrice = 0;
                                }
                                else
                                {
                                    sales.CashPrice = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    sales.LCP = 0;
                                }
                                else
                                {
                                    sales.LCP = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    sales.InvoiceNo = String.Empty;
                                }
                                else
                                {
                                    sales.InvoiceNo = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    sales.InvoiceDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.InvoiceDate = myReader.GetDateTime(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    sales.InterestRate = 0;
                                }
                                else
                                {
                                    sales.InterestRate = myReader.GetDecimal(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    sales.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Remarks = myReader.GetString(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    sales.BalanceFwd = 0;
                                }
                                else
                                {
                                    sales.BalanceFwd = myReader.GetDecimal(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    sales.DueDate01 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate01 = myReader.GetDateTime(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    sales.DueDate02 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate02 = myReader.GetDateTime(20);
                                }
                                if (myReader.IsDBNull(21))
                                {
                                    sales.Vehicle.Status = 0;
                                }
                                else
                                {
                                    sales.Vehicle.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(21).ToString());
                                }
                                if (myReader.IsDBNull(22))
                                {
                                    sales.Vehicle.Code = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Code = myReader.GetString(22);
                                }
                                    if (myReader.IsDBNull(23))
                                {
                                    sales.Vehicle.Brand = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Brand = myReader.GetString(23);
                                }
                                    if (myReader.IsDBNull(24))
                                {
                                    sales.Vehicle.Model = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Model = myReader.GetString(24);
                                }
                                if (myReader.IsDBNull(25))
                                {
                                    sales.Vehicle.Color = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Color = myReader.GetString(25);
                                }
                                if (myReader.IsDBNull(26))
                                {
                                    sales.Vehicle.EngineNo = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.EngineNo = myReader.GetString(26);
                                }
                                if (myReader.IsDBNull(27))
                                {
                                    sales.Vehicle.ChassisNo = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.ChassisNo = myReader.GetString(27);
                                }
                                if (myReader.IsDBNull(28))
                                {
                                    sales.Vehicle.PlateNo = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.PlateNo = myReader.GetString(28);
                                }
                                if (myReader.IsDBNull(29))
                                {
                                    sales.Vehicle.CertReg = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.CertReg = myReader.GetString(29);
                                }
                                if (myReader.IsDBNull(30))
                                {
                                    sales.Vehicle.OwnerReg = null;
                                }
                                else
                                {
                                    sales.Vehicle.OwnerReg = Customer.CreateCustomer();
                                    sales.Vehicle.OwnerReg.ID = myReader.GetInt32(30);
                                }
                                if (myReader.IsDBNull(31))
                                {
                                    sales.Vehicle.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Vehicle.Remarks = myReader.GetString(31);
                                }
                                if (myReader.IsDBNull(32))
                                {
                                    sales.Customer.LastName = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.LastName = myReader.GetString(32);
                                }
                                if (myReader.IsDBNull(33))
                                {
                                    sales.Customer.FirstName = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.FirstName = myReader.GetString(33);
                                }
                                if (myReader.IsDBNull(34))
                                {
                                    sales.Customer.MiddleName = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.MiddleName = myReader.GetString(34);
                                }
                                if (myReader.IsDBNull(35))
                                {
                                    sales.Customer.Address = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.Address = myReader.GetString(35);
                                }
                                if (myReader.IsDBNull(36))
                                {
                                    sales.Customer.Zone = null;
                                }
                                else
                                {
                                    sales.Customer.Zone = Zone.CreateZone();
                                    sales.Customer.Zone.ID = myReader.GetInt32(36);
                                }
                                if (myReader.IsDBNull(37))
                                {
                                    sales.Customer.Phone = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.Phone = myReader.GetString(37);
                                }
                                if (myReader.IsDBNull(38))
                                {
                                    sales.Customer.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Customer.Remarks = myReader.GetString(38);
                                }
                                if (myReader.FieldCount > 39)
                                {
                                    if (myReader.IsDBNull(39))
                                    {
                                        sales.Branch = 0;
                                    }
                                    else
                                    {
                                        sales.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(39).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 40)
                                {
                                    if (myReader.IsDBNull(40))
                                    {
                                        sales.AuditID = 0;
                                    }
                                    else
                                    {
                                        sales.AuditID = myReader.GetInt32(40);
                                    }
                                }
                                allSales.Add(sales);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allSales;
        }

        public GenericList<Sales> GetAllSales(Vehicle vehicle)
        {
            GenericList<Sales> allSales = new GenericList<Sales>();
            ICustomerProvider customerProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    customerProvider = new CustomerProvider(Database.AuditConnection());
                }
                else
                {
                    customerProvider = new CustomerProvider(Database.GeneralLedger);
                }
            }
            else
            {
                customerProvider = new CustomerProvider(Database.BranchConnection(this._branch));
            }
            CustomerManager customerManager = new CustomerManager(customerProvider);
            
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@VehicleID", SqlDbType.Int);
            myParam1.Value = vehicle.ID;
            myCommand.Parameters.Add(myParam1);
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Sales sales = Sales.CreateSales();
                                sales.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    sales.SaleCode = String.Empty;
                                }
                                else
                                {
                                    sales.SaleCode = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    sales.SaleDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.SaleDate = myReader.GetDateTime(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    sales.SaleAmount = 0;
                                }
                                else
                                {
                                    sales.SaleAmount = myReader.GetDecimal(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    sales.Customer = null;
                                }
                                else
                                {
                                    sales.Customer = customerManager.GetCustomer(myReader.GetInt32(4)); ;
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    sales.Vehicle = null;
                                }
                                else
                                {
                                    sales.Vehicle = vehicle;
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    sales.Term = 0;
                                }
                                else
                                {
                                    sales.Term = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    sales.TermTotal = 0;
                                }
                                else
                                {
                                    sales.TermTotal = myReader.GetInt32(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    sales.AmortStartDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.AmortStartDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    sales.AmortAmount = 0;
                                }
                                else
                                {
                                    sales.AmortAmount = myReader.GetDecimal(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    sales.AmortRebate = 0;
                                }
                                else
                                {
                                    sales.AmortRebate = myReader.GetDecimal(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    sales.Status = 0;
                                }
                                else
                                {
                                    sales.Status = (SalesStatus)Enum.Parse(typeof(SalesStatus), myReader.GetInt32(11).ToString());
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    sales.CashPrice = 0;
                                }
                                else
                                {
                                    sales.CashPrice = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    sales.LCP = 0;
                                }
                                else
                                {
                                    sales.LCP = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    sales.InvoiceNo = String.Empty;
                                }
                                else
                                {
                                    sales.InvoiceNo = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    sales.InvoiceDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.InvoiceDate = myReader.GetDateTime(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    sales.InterestRate = 0;
                                }
                                else
                                {
                                    sales.InterestRate = myReader.GetDecimal(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    sales.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Remarks = myReader.GetString(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    sales.BalanceFwd = 0;
                                }
                                else
                                {
                                    sales.BalanceFwd = myReader.GetDecimal(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    sales.DueDate01 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate01 = myReader.GetDateTime(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    sales.DueDate02 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate02 = myReader.GetDateTime(20);
                                }
                                if (myReader.FieldCount > 21)
                                {
                                    if (myReader.IsDBNull(21))
                                    {
                                        sales.Branch = 0;
                                    }
                                    else
                                    {
                                        sales.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(21).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 22)
                                {
                                    if (myReader.IsDBNull(22))
                                    {
                                        sales.AuditID = 0;
                                    }
                                    else
                                    {
                                        sales.AuditID = myReader.GetInt32(22);
                                    }
                                }
                                allSales.Add(sales);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allSales;
        }

        public GenericList<Sales> GetAllSales(Int32 pageNo, SortBySales sortBy, SortingOrder sortOrder)
        {
            GenericList<Sales> allSales = new GenericList<Sales>();
            ICustomerProvider customerProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    customerProvider = new CustomerProvider(Database.AuditConnection());
                }
                else
                {
                    customerProvider = new CustomerProvider(Database.GeneralLedger);
                }
            }
            else
            {
                customerProvider = new CustomerProvider(Database.BranchConnection(this._branch));
            }
            CustomerManager customerManager = new CustomerManager(customerProvider);
            IVehicleProvider vehicleProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    vehicleProvider = new VehicleProvider(Database.AuditConnection());
                }
                else
                {
                    vehicleProvider = new VehicleProvider(Database.GeneralLedger);
                }
            }
            else
            {
                vehicleProvider = new VehicleProvider(Database.BranchConnection(this._branch));
            }
            VehicleManager vehicleManager = new VehicleManager(vehicleProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesSelect";
            myCommand.Connection = this._conn;

            if (pageNo >= 0)
            {
                SqlParameter myParam1 = new SqlParameter("@PageNo", SqlDbType.TinyInt);
                myParam1.Value = pageNo;
                SqlParameter myParam2 = new SqlParameter("@PageSize", SqlDbType.TinyInt);
                myParam2.Value = Utility.PageSize;
                SqlParameter myParam3 = new SqlParameter("@SortBy", SqlDbType.TinyInt);
                myParam3.Value = (Byte)sortBy;
                SqlParameter myParam4 = new SqlParameter("@SortOrder", SqlDbType.Bit);
                myParam4.Value = (Byte)sortOrder;
                myCommand.Parameters.Add(myParam1);
                myCommand.Parameters.Add(myParam2);
                myCommand.Parameters.Add(myParam3);
                myCommand.Parameters.Add(myParam4);
            }
            try
            {
                this._conn.Open();
                try
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                            {
                                Sales sales = Sales.CreateSales();
                                sales.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    sales.SaleCode = String.Empty;
                                }
                                else
                                {
                                    sales.SaleCode = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    sales.SaleDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.SaleDate = myReader.GetDateTime(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    sales.SaleAmount = 0;
                                }
                                else
                                {
                                    sales.SaleAmount = myReader.GetDecimal(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    sales.Customer = null;
                                }
                                else
                                {
                                    sales.Customer = customerManager.GetCustomer(myReader.GetInt32(4));
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    sales.Vehicle = null;
                                }
                                else
                                {
                                    sales.Vehicle = vehicleManager.GetVehicle(myReader.GetInt32(5));
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    sales.Term = 0;
                                }
                                else
                                {
                                    sales.Term = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    sales.TermTotal = 0;
                                }
                                else
                                {
                                    sales.TermTotal = myReader.GetInt32(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    sales.AmortStartDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.AmortStartDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    sales.AmortAmount = 0;
                                }
                                else
                                {
                                    sales.AmortAmount = myReader.GetDecimal(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    sales.AmortRebate = 0;
                                }
                                else
                                {
                                    sales.AmortRebate = myReader.GetDecimal(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    sales.Status = 0;
                                }
                                else
                                {
                                    sales.Status = (SalesStatus)Enum.Parse(typeof(SalesStatus), myReader.GetInt32(11).ToString());
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    sales.CashPrice = 0;
                                }
                                else
                                {
                                    sales.CashPrice = myReader.GetDecimal(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    sales.LCP = 0;
                                }
                                else
                                {
                                    sales.LCP = myReader.GetDecimal(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    sales.InvoiceNo = String.Empty;
                                }
                                else
                                {
                                    sales.InvoiceNo = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    sales.InvoiceDate = DateTime.Today;
                                }
                                else
                                {
                                    sales.InvoiceDate = myReader.GetDateTime(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    sales.InterestRate = 0;
                                }
                                else
                                {
                                    sales.InterestRate = myReader.GetDecimal(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    sales.Remarks = String.Empty;
                                }
                                else
                                {
                                    sales.Remarks = myReader.GetString(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    sales.BalanceFwd = 0;
                                }
                                else
                                {
                                    sales.BalanceFwd = myReader.GetDecimal(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    sales.DueDate01 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate01 = myReader.GetDateTime(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    sales.DueDate02 = DateTime.Today;
                                }
                                else
                                {
                                    sales.DueDate02 = myReader.GetDateTime(20);
                                }
                                if (myReader.FieldCount > 21)
                                {
                                    if (myReader.IsDBNull(21))
                                    {
                                        sales.Branch = 0;
                                    }
                                    else
                                    {
                                        sales.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(21).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 22)
                                {
                                    if (myReader.IsDBNull(22))
                                    {
                                        sales.AuditID = 0;
                                    }
                                    else
                                    {
                                        sales.AuditID = myReader.GetInt32(22);
                                    }
                                }
                                allSales.Add(sales);
                            }
                        }
                    }
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allSales;
        }

        public Sales InsertSales(Sales sales)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@SaleCode", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(sales.SaleCode))
            {
                myParam1.Value = DBNull.Value;
            }
            else
            {
                myParam1.Value = sales.SaleCode;
            }
            SqlParameter myParam2 = new SqlParameter("@SaleDate", SqlDbType.DateTime);
            myParam2.Value = sales.SaleDate;
            SqlParameter myParam3 = new SqlParameter("@SaleAmount", SqlDbType.Decimal);
            if (sales.SaleAmount == 0)
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = sales.SaleAmount;
            }
            SqlParameter myParam4 = new SqlParameter("@CustomerID", SqlDbType.Int);
            if (sales.Customer == null)
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = sales.Customer.ID;
            }
            SqlParameter myParam5 = new SqlParameter("@VehicleID", SqlDbType.Int);
            if (sales.Vehicle == null)
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = sales.Vehicle.ID;
            }
            SqlParameter myParam6 = new SqlParameter("@PaymentTerm", SqlDbType.Int);
            myParam6.Value = sales.Term;
            SqlParameter myParam7 = new SqlParameter("@TermTotal", SqlDbType.Int);
            if (sales.TermTotal == 0)
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = sales.TermTotal;
            }
            SqlParameter myParam8 = new SqlParameter("@AmortStartDate", SqlDbType.SmallDateTime);
            myParam8.Value = sales.AmortStartDate;
            SqlParameter myParam9 = new SqlParameter("@AmortAmount", SqlDbType.Decimal);
            if (sales.AmortAmount == 0)
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = sales.AmortAmount;
            }
            SqlParameter myParam10 = new SqlParameter("@AmortRebate", SqlDbType.Decimal);
            if (sales.AmortRebate == 0)
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = sales.AmortRebate;
            }
            SqlParameter myParam11 = new SqlParameter("@Status", SqlDbType.Int);
            myParam11.Value = sales.Status;
            SqlParameter myParam12 = new SqlParameter("@CashPrice", SqlDbType.Decimal);
            if (sales.CashPrice == 0)
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = sales.CashPrice;
            }
            SqlParameter myParam13 = new SqlParameter("@LCPrice", SqlDbType.Decimal);
            if (sales.LCP == 0)
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = sales.LCP;
            }
            SqlParameter myParam14 = new SqlParameter("@SINo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(sales.InvoiceNo))
            {
                myParam14.Value = DBNull.Value;
            }
            else
            {
                myParam14.Value = sales.InvoiceNo;
            }
            SqlParameter myParam15 = new SqlParameter("@SIDate", SqlDbType.SmallDateTime);
            myParam15.Value = sales.InvoiceDate;
            SqlParameter myParam16 = new SqlParameter("@InterestRate", SqlDbType.Decimal);
            if (sales.InterestRate == 0)
            {
                myParam16.Value = DBNull.Value;
            }
            else
            {
                myParam16.Value = sales.InterestRate;
            }
            SqlParameter myParam17 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(sales.Remarks))
            {
                myParam17.Value = DBNull.Value;
            }
            else
            {
                myParam17.Value = sales.Remarks;
            }
            SqlParameter myParam18 = new SqlParameter("@BalanceFwd", SqlDbType.Decimal);
            if (sales.BalanceFwd == 0)
            {
                myParam18.Value = DBNull.Value;
            }
            else
            {
                myParam18.Value = sales.BalanceFwd;
            }
            SqlParameter myParam19 = new SqlParameter("@DueDate01", SqlDbType.SmallDateTime);
            myParam19.Value = sales.DueDate01;
            SqlParameter myParam20 = new SqlParameter("@DueDate02", SqlDbType.SmallDateTime);
            myParam20.Value = sales.DueDate02;
            SqlParameter myParam21 = new SqlParameter("@BranchID", SqlDbType.Int);
            myParam21.Value = (Int32)sales.Branch;
            SqlParameter myParam22 = new SqlParameter("@AuditID", SqlDbType.Int);
            myParam22.Value = sales.AuditID;
            SqlParameter myParam23 = new SqlParameter("@Output", SqlDbType.Int);
            myParam23.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
            myCommand.Parameters.Add(myParam6);
            myCommand.Parameters.Add(myParam7);
            myCommand.Parameters.Add(myParam8);
            myCommand.Parameters.Add(myParam9);
            myCommand.Parameters.Add(myParam10);
            myCommand.Parameters.Add(myParam11);
            myCommand.Parameters.Add(myParam12);
            myCommand.Parameters.Add(myParam13);
            myCommand.Parameters.Add(myParam14);
            myCommand.Parameters.Add(myParam15);
            myCommand.Parameters.Add(myParam16);
            myCommand.Parameters.Add(myParam17);
            myCommand.Parameters.Add(myParam18);
            myCommand.Parameters.Add(myParam19);
            myCommand.Parameters.Add(myParam20);
            myCommand.Parameters.Add(myParam21);
            myCommand.Parameters.Add(myParam22);
            myCommand.Parameters.Add(myParam23);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    sales.ID = Convert.ToInt32(myParam23.Value);
                }
                finally
                {
                    this._conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sales;
        }

        public Boolean UpdateSales(Sales sales)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = sales.ID;
            SqlParameter myParam2 = new SqlParameter("@SaleCode", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(sales.SaleCode))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = sales.SaleCode;
            }
            SqlParameter myParam3 = new SqlParameter("@SaleDate", SqlDbType.DateTime);
            myParam3.Value = sales.SaleDate;
            SqlParameter myParam4 = new SqlParameter("@SaleAmount", SqlDbType.Decimal);
            if (sales.SaleAmount == 0)
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = sales.SaleAmount;
            }
            SqlParameter myParam5 = new SqlParameter("@CustomerID", SqlDbType.Int);
            if (sales.Customer == null)
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = sales.Customer.ID;
            }
            SqlParameter myParam6 = new SqlParameter("@VehicleID", SqlDbType.Int);
            if (sales.Vehicle == null)
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = sales.Vehicle.ID;
            }
            SqlParameter myParam7 = new SqlParameter("@PaymentTerm", SqlDbType.Int);
            myParam7.Value = sales.Term;
            SqlParameter myParam8 = new SqlParameter("@TermTotal", SqlDbType.Int);
            if (sales.TermTotal == 0)
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = sales.TermTotal;
            }
            SqlParameter myParam9 = new SqlParameter("@AmortStartDate", SqlDbType.SmallDateTime);
            myParam9.Value = sales.AmortStartDate;
            SqlParameter myParam10 = new SqlParameter("@AmortAmount", SqlDbType.Decimal);
            if (sales.AmortAmount == 0)
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = sales.AmortAmount;
            }
            SqlParameter myParam11 = new SqlParameter("@AmortRebate", SqlDbType.Decimal);
            if (sales.AmortRebate == 0)
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = sales.AmortRebate;
            }
            SqlParameter myParam12 = new SqlParameter("@Status", SqlDbType.Int);
            myParam12.Value = sales.Status;
            SqlParameter myParam13 = new SqlParameter("@CashPrice", SqlDbType.Decimal);
            if (sales.CashPrice == 0)
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = sales.CashPrice;
            }
            SqlParameter myParam14 = new SqlParameter("@LCPrice", SqlDbType.Decimal);
            if (sales.LCP == 0)
            {
                myParam14.Value = DBNull.Value;
            }
            else
            {
                myParam14.Value = sales.LCP;
            }
            SqlParameter myParam15 = new SqlParameter("@SINo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(sales.InvoiceNo))
            {
                myParam15.Value = DBNull.Value;
            }
            else
            {
                myParam15.Value = sales.InvoiceNo;
            }
            SqlParameter myParam16 = new SqlParameter("@SIDate", SqlDbType.SmallDateTime);
            myParam16.Value = sales.InvoiceDate;
            SqlParameter myParam17 = new SqlParameter("@InterestRate", SqlDbType.Decimal);
            if (sales.InterestRate == 0)
            {
                myParam17.Value = DBNull.Value;
            }
            else
            {
                myParam17.Value = sales.InterestRate;
            }
            SqlParameter myParam18 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(sales.Remarks))
            {
                myParam18.Value = DBNull.Value;
            }
            else
            {
                myParam18.Value = sales.Remarks;
            }
            SqlParameter myParam19 = new SqlParameter("@BalanceFwd", SqlDbType.Decimal);
            if (sales.BalanceFwd == 0)
            {
                myParam19.Value = DBNull.Value;
            }
            else
            {
                myParam19.Value = sales.BalanceFwd;
            }
            SqlParameter myParam20 = new SqlParameter("@DueDate01", SqlDbType.SmallDateTime);
            myParam20.Value = sales.DueDate01;
            SqlParameter myParam21 = new SqlParameter("@DueDate02", SqlDbType.SmallDateTime);
            myParam21.Value = sales.DueDate02;
            SqlParameter myParam22 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam22.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
            myCommand.Parameters.Add(myParam6);
            myCommand.Parameters.Add(myParam7);
            myCommand.Parameters.Add(myParam8);
            myCommand.Parameters.Add(myParam9);
            myCommand.Parameters.Add(myParam10);
            myCommand.Parameters.Add(myParam11);
            myCommand.Parameters.Add(myParam12);
            myCommand.Parameters.Add(myParam13);
            myCommand.Parameters.Add(myParam14);
            myCommand.Parameters.Add(myParam15);
            myCommand.Parameters.Add(myParam16);
            myCommand.Parameters.Add(myParam17);
            myCommand.Parameters.Add(myParam18);
            myCommand.Parameters.Add(myParam19);
            myCommand.Parameters.Add(myParam20);
            myCommand.Parameters.Add(myParam21);
            myCommand.Parameters.Add(myParam22);
            try
            {
                this._conn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                this._conn.Close();
            }
            if (Convert.ToInt32(myParam22.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteSales(Sales sales)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SalesDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = sales.ID;
            SqlParameter myParam2 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam2.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            try
            {
                this._conn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                this._conn.Close();
            }
            if (Convert.ToInt32(myParam2.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
