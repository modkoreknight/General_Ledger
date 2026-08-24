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
    public class CustomerProvider : ICustomerProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public CustomerProvider(SqlConnection conn)
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
        public Int32 GetCustomerPageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerGetPageCount";
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

        public Customer GetCustomer(Int32 id)
        {
            Customer customer = Customer.CreateCustomer();
            IZoneProvider zoneProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    zoneProvider = new ZoneProvider(Database.AuditConnection());
                }
                else
                {
                    zoneProvider = new ZoneProvider(Database.GeneralLedger);
                }
            }
            else
            {
                zoneProvider = new ZoneProvider(Database.BranchConnection(this._branch));
            }
            ZoneManager zoneManager = new ZoneManager(zoneProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerSelect";
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
                            customer.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                customer.CustomerNo = String.Empty;
                            }
                            else
                            {
                                customer.CustomerNo = myReader.GetString(1);
                            }
                            if (myReader.IsDBNull(2))
                            {
                                customer.LastName = String.Empty;
                            }
                            else
                            {
                                customer.LastName = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                customer.FirstName = String.Empty;
                            }
                            else
                            {
                                customer.FirstName = myReader.GetString(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                customer.MiddleName = String.Empty;
                            }
                            else
                            {
                                customer.MiddleName = myReader.GetString(4);
                            }
                            if (myReader.IsDBNull(5))
                            {
                                customer.Address = String.Empty;
                            }
                            else
                            {
                                customer.Address = myReader.GetString(5);
                            }
                            if (myReader.IsDBNull(6))
                            {
                                customer.Zone = null;
                            }
                            else
                            {
                                customer.Zone = zoneManager.GetZone(myReader.GetInt32(6));
                            }
                            if (myReader.IsDBNull(7))
                            {
                                customer.Phone = String.Empty;
                            }
                            else
                            {
                                customer.Phone = myReader.GetString(7);
                            }
                            if (myReader.IsDBNull(8))
                            {
                                customer.BirthDate = DateTime.MinValue;
                            }
                            else
                            {
                                customer.BirthDate = myReader.GetDateTime(8);
                            }
                            if (myReader.IsDBNull(9))
                            {
                                customer.PictureFile = String.Empty;
                            }
                            else
                            {
                                customer.PictureFile = myReader.GetString(9);
                            }
                            if (myReader.IsDBNull(10))
                            {
                                customer.Remarks = String.Empty;
                            }
                            else
                            {
                                customer.Remarks = myReader.GetString(10);
                            }
                            if (myReader.FieldCount > 11)
                            {
                                if (myReader.IsDBNull(11))
                                {
                                    customer.Branch = 0;
                                }
                                else
                                {
                                    customer.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(11).ToString());
                                }
                            }
                            if (myReader.FieldCount > 12)
                            {
                                if (myReader.IsDBNull(12))
                                {
                                    customer.AuditID = 0;
                                }
                                else
                                {
                                    customer.AuditID = myReader.GetInt32(12);
                                }
                            }
                            customer.Branch = this._branch;
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
            return customer;
        }

        public GenericList<Customer> GetAllCustomer()
        {
            GenericList<Customer> allCustomer = new GenericList<Customer>();
            //IZoneProvider zoneProvider;
            //if (this._isLocal)
            //{
            //    zoneProvider = new ZoneProvider(Database.GeneralLedger);
            //}
            //else
            //{
            //    zoneProvider = new ZoneProvider(Database.BranchConnection(this._branch));
            //}
            //ZoneManager zoneManager = new ZoneManager(zoneProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerSelect";
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
                                Customer customer = Customer.CreateCustomer();
                                customer.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    customer.CustomerNo = String.Empty;
                                }
                                else
                                {
                                    customer.CustomerNo = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    customer.LastName = String.Empty;
                                }
                                else
                                {
                                    customer.LastName = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    customer.FirstName = String.Empty;
                                }
                                else
                                {
                                    customer.FirstName = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    customer.MiddleName = String.Empty;
                                }
                                else
                                {
                                    customer.MiddleName = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    customer.Address = String.Empty;
                                }
                                else
                                {
                                    customer.Address = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    customer.Zone = null;
                                }
                                else
                                {
                                    //customer.Zone = zoneManager.GetZone(myReader.GetInt32(6));
                                    customer.Zone = Zone.CreateZone();
                                    customer.Zone.ID = myReader.GetInt32(6);
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    customer.Phone = String.Empty;
                                }
                                else
                                {
                                    customer.Phone = myReader.GetString(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    customer.BirthDate = DateTime.MinValue;
                                }
                                else
                                {
                                    customer.BirthDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    customer.PictureFile = String.Empty;
                                }
                                else
                                {
                                    customer.PictureFile = myReader.GetString(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    customer.Remarks = String.Empty;
                                }
                                else
                                {
                                    customer.Remarks = myReader.GetString(10);
                                }
                                if (myReader.FieldCount > 11)
                                {
                                    if (myReader.IsDBNull(11))
                                    {
                                        customer.Branch = 0;
                                    }
                                    else
                                    {
                                        customer.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(11).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 12)
                                {
                                    if (myReader.IsDBNull(12))
                                    {
                                        customer.AuditID = 0;
                                    }
                                    else
                                    {
                                        customer.AuditID = myReader.GetInt32(12);
                                    }
                                }
                                allCustomer.Add(customer);
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
            return allCustomer;
        }

        public GenericList<Customer> GetAllCustomer(Int32 pageNo, SortByCustomer sortBy, SortingOrder sortOrder)
        {
            GenericList<Customer> allCustomer = new GenericList<Customer>();
            IZoneProvider zoneProvider;
            if (this._isLocal)
            {
                if (this._branch == Branch.Audit_)
                {
                    zoneProvider = new ZoneProvider(Database.AuditConnection());
                }
                else
                {
                    zoneProvider = new ZoneProvider(Database.GeneralLedger);
                }
            }
            else
            {
                zoneProvider = new ZoneProvider(Database.BranchConnection(this._branch));
            }
            ZoneManager zoneManager = new ZoneManager(zoneProvider);

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerSelect";
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
                                Customer customer = Customer.CreateCustomer();
                                customer.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    customer.CustomerNo = String.Empty;
                                }
                                else
                                {
                                    customer.CustomerNo = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    customer.LastName = String.Empty;
                                }
                                else
                                {
                                    customer.LastName = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    customer.FirstName = String.Empty;
                                }
                                else
                                {
                                    customer.FirstName = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    customer.MiddleName = String.Empty;
                                }
                                else
                                {
                                    customer.MiddleName = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    customer.Address = String.Empty;
                                }
                                else
                                {
                                    customer.Address = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    customer.Zone = null;
                                }
                                else
                                {
                                    customer.Zone = zoneManager.GetZone(myReader.GetInt32(6));
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    customer.Phone = String.Empty;
                                }
                                else
                                {
                                    customer.Phone = myReader.GetString(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    customer.BirthDate = DateTime.MinValue;
                                }
                                else
                                {
                                    customer.BirthDate = myReader.GetDateTime(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    customer.PictureFile = String.Empty;
                                }
                                else
                                {
                                    customer.PictureFile = myReader.GetString(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    customer.Remarks = String.Empty;
                                }
                                else
                                {
                                    customer.Remarks = myReader.GetString(10);
                                }
                                if (myReader.FieldCount > 11)
                                {
                                    if (myReader.IsDBNull(11))
                                    {
                                        customer.Branch = 0;
                                    }
                                    else
                                    {
                                        customer.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(11).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 12)
                                {
                                    if (myReader.IsDBNull(12))
                                    {
                                        customer.AuditID = 0;
                                    }
                                    else
                                    {
                                        customer.AuditID = myReader.GetInt32(12);
                                    }
                                }
                                allCustomer.Add(customer);
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
            return allCustomer;
        }

        public Customer InsertCustomer(Customer customer)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@CustomerNo", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(customer.CustomerNo))
            {
                myParam1.Value = DBNull.Value;
            }
            else
            {
                myParam1.Value = customer.CustomerNo;
            }
            SqlParameter myParam2 = new SqlParameter("@LastName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(customer.LastName))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = customer.LastName;
            }
            SqlParameter myParam3 = new SqlParameter("@FirstName", SqlDbType.VarChar, 35);
            if (String.IsNullOrEmpty(customer.FirstName))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = customer.FirstName;
            }
            SqlParameter myParam4 = new SqlParameter("@MiddleName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(customer.MiddleName))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = customer.MiddleName;
            }
            SqlParameter myParam5 = new SqlParameter("@Address", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(customer.Address))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = customer.Address;
            }
            SqlParameter myParam6 = new SqlParameter("@ZoneID", SqlDbType.Int);
            if (customer.Zone == null)
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = customer.Zone.ID;
            }
            SqlParameter myParam7 = new SqlParameter("@Phone", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(customer.Phone))
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = customer.Phone;
            }
            SqlParameter myParam8 = new SqlParameter("@BirthDate", SqlDbType.SmallDateTime);
            if (customer.BirthDate == DateTime.MinValue)
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = customer.BirthDate;
            }
            SqlParameter myParam9 = new SqlParameter("@PictureFile", SqlDbType.VarChar, 128);
            if (String.IsNullOrEmpty(customer.PictureFile))
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = customer.PictureFile;
            }
            SqlParameter myParam10 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(customer.Remarks))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = customer.Remarks;
            }
            SqlParameter myParam11 = new SqlParameter("@BranchID", SqlDbType.Int);
            myParam11.Value = (Int32)customer.Branch;
            SqlParameter myParam12 = new SqlParameter("@AuditID", SqlDbType.Int);
            myParam12.Value = customer.AuditID;
            SqlParameter myParam13 = new SqlParameter("@Output", SqlDbType.Int);
            myParam13.Direction = ParameterDirection.Output;
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
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    customer.ID = Convert.ToInt32(myParam13.Value);
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
            return customer;
        }

        public Boolean UpdateCustomer(Customer customer)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = customer.ID;
            SqlParameter myParam2 = new SqlParameter("@CustomerNo", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(customer.CustomerNo))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = customer.CustomerNo;
            }
            SqlParameter myParam3 = new SqlParameter("@LastName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(customer.LastName))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = customer.LastName;
            }
            SqlParameter myParam4 = new SqlParameter("@FirstName", SqlDbType.VarChar, 35);
            if (String.IsNullOrEmpty(customer.FirstName))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = customer.FirstName;
            }
            SqlParameter myParam5 = new SqlParameter("@MiddleName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(customer.MiddleName))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = customer.MiddleName;
            }
            SqlParameter myParam6 = new SqlParameter("@Address", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(customer.Address))
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = customer.Address;
            }
            SqlParameter myParam7 = new SqlParameter("@ZoneID", SqlDbType.Int);
            if (customer.Zone == null)
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = customer.Zone.ID;
            }
            SqlParameter myParam8 = new SqlParameter("@Phone", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(customer.Phone))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = customer.Phone;
            }
            SqlParameter myParam9 = new SqlParameter("@BirthDate", SqlDbType.SmallDateTime);
            if (customer.BirthDate == DateTime.MinValue)
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = customer.BirthDate;
            }
            SqlParameter myParam10 = new SqlParameter("@PictureFile", SqlDbType.VarChar, 128);
            if (String.IsNullOrEmpty(customer.PictureFile))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = customer.PictureFile;
            }
            SqlParameter myParam11 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(customer.Remarks))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = customer.Remarks;
            }
            SqlParameter myParam12 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam12.Direction = ParameterDirection.ReturnValue;
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
            if (Convert.ToInt32(myParam12.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteCustomer(Customer customer)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "CustomerDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = customer.ID;
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
