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
    public class VehicleRegistrationProvider : IVehicleRegistrationProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public VehicleRegistrationProvider(SqlConnection conn)
        {
            this._conn = conn;

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
        public Int32 GetVehicleRegistrationPageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleGetPageCount";
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

        public VehicleRegistration GetVehicleRegistration(Int32 id)
        {
            VehicleRegistration vehicleRegistration = VehicleRegistration.CreateVehicleRegistration();
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
            myCommand.CommandText = "VehicleSelect";
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
                            vehicleRegistration.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                vehicleRegistration.Brand = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Brand = myReader.GetString(1);
                            }
                            if (myReader.IsDBNull(2))
                            {
                                vehicleRegistration.Model = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Model = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                vehicleRegistration.Color = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Color = myReader.GetString(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                vehicleRegistration.EngineNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.EngineNo = myReader.GetString(4);
                            }
                            if (myReader.IsDBNull(5))
                            {
                                vehicleRegistration.ChassisNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.ChassisNo = myReader.GetString(5);
                            }
                            if (myReader.IsDBNull(6))
                            {
                                vehicleRegistration.Status = 0;
                            }
                            else
                            {
                                vehicleRegistration.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(6).ToString());
                            }
                            if (myReader.IsDBNull(7))
                            {
                                vehicleRegistration.PlateNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.PlateNo = myReader.GetString(7);
                            }
                            if (myReader.IsDBNull(8))
                            {
                                vehicleRegistration.CertReg = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.CertReg = myReader.GetString(8);
                            }
                            if (myReader.IsDBNull(9))
                            {
                                vehicleRegistration.OwnerReg = null;
                            }
                            else
                            {
                                vehicleRegistration.OwnerReg = customerManager.GetCustomer(myReader.GetInt32(9));
                            }
                            if (myReader.IsDBNull(10))
                            {
                                vehicleRegistration.Remarks = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Remarks = myReader.GetString(10);
                            }
                            if (myReader.IsDBNull(11))
                            {
                                vehicleRegistration.Code = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Code = myReader.GetString(11);
                            }
                            if (myReader.IsDBNull(12))
                            {
                                vehicleRegistration.ReferenceNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.ReferenceNo = myReader.GetString(12);
                            }
                            if (myReader.IsDBNull(13))
                            {
                                vehicleRegistration.ConfirmLTO = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.ConfirmLTO = myReader.GetString(13);
                            }
                            if (myReader.IsDBNull(14))
                            {
                                vehicleRegistration.ConfirmationFieldOffice = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.ConfirmationFieldOffice = myReader.GetString(14);
                            }
                            if (myReader.IsDBNull(15))
                            {
                                vehicleRegistration.ConfirmationAmount = 0;
                            }
                            else
                            {
                                vehicleRegistration.ConfirmationAmount = myReader.GetDecimal(15);
                            }
                            if (myReader.IsDBNull(16))
                            {
                                vehicleRegistration.SOP = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.SOP = myReader.GetString(16);
                            }
                            if (myReader.IsDBNull(17))
                            {
                                vehicleRegistration.SOPAmount = 0;
                            }
                            else
                            {
                                vehicleRegistration.SOPAmount = myReader.GetDecimal(17);
                            }
                            if (myReader.IsDBNull(18))
                            {
                                vehicleRegistration.Registered = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Registered = myReader.GetString(18);
                            }
                            if (myReader.IsDBNull(19))
                            {
                                vehicleRegistration.Insured = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Insured = myReader.GetString(19);
                            }
                            if (myReader.IsDBNull(20))
                            {
                                vehicleRegistration.ORNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.ORNo = myReader.GetString(20);
                            }
                            if (myReader.IsDBNull(21))
                            {
                                vehicleRegistration.CRNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.CRNo = myReader.GetString(21);
                            }
                            if (myReader.IsDBNull(22))
                            {
                                vehicleRegistration.RegistrationAmount = 0;
                            }
                            else
                            {
                                vehicleRegistration.RegistrationAmount = myReader.GetDecimal(22);
                            }
                            if (myReader.IsDBNull(23))
                            {
                                vehicleRegistration.FileNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.FileNo = myReader.GetString(23);
                            }
                            if (myReader.IsDBNull(24))
                            {
                                vehicleRegistration.SINo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.SINo = myReader.GetString(24);
                            }
                            if (myReader.IsDBNull(25))
                            {
                                vehicleRegistration.DistrictOffice = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.DistrictOffice = myReader.GetString(25);
                            }
                            if (myReader.IsDBNull(26))
                            {
                                vehicleRegistration.Clearance = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.Clearance = myReader.GetString(26);
                            }
                            if (myReader.IsDBNull(27))
                            {
                                vehicleRegistration.ClearanceNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.ClearanceNo = myReader.GetString(27);
                            }
                            if (myReader.IsDBNull(28))
                            {
                                vehicleRegistration.DateRegistration = DateTime.Today;
                            }
                            else
                            {
                                vehicleRegistration.DateRegistration = myReader.GetDateTime(28);
                            }
                            if (myReader.IsDBNull(29))
                            {
                                vehicleRegistration.RegistrationFieldOffice = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.RegistrationFieldOffice = myReader.GetString(29);
                            }
                            if (myReader.IsDBNull(30))
                            {
                                vehicleRegistration.InsuranceNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.InsuranceNo = myReader.GetString(30);
                            }
                            if (myReader.IsDBNull(31))
                            {
                                vehicleRegistration.InsuranceAmount = 0;
                            }
                            else
                            {
                                vehicleRegistration.InsuranceAmount = myReader.GetDecimal(31);
                            }
                            if (myReader.IsDBNull(32))
                            {
                                vehicleRegistration.DateRegistered = DateTime.Today;
                            }
                            else
                            {
                                vehicleRegistration.DateRegistered = myReader.GetDateTime(32);
                            }
                            if (myReader.IsDBNull(33))
                            {
                                vehicleRegistration.FieldOffice = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.FieldOffice = myReader.GetString(33);
                            }
                            if (myReader.IsDBNull(34))
                            {
                                vehicleRegistration.MVFileNo = String.Empty;
                            }
                            else
                            {
                                vehicleRegistration.MVFileNo = myReader.GetString(34);
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
            return vehicleRegistration;
        }

        public GenericList<VehicleRegistration> GetAllVehicleRegistration()
        {
            GenericList<VehicleRegistration> allVehicle = new GenericList<VehicleRegistration>();
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

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleSelect";
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
                                VehicleRegistration vehicleRegistration = VehicleRegistration.CreateVehicleRegistration();
                                vehicleRegistration.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    vehicleRegistration.Brand = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Brand = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    vehicleRegistration.Model = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Model = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    vehicleRegistration.Color = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Color = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    vehicleRegistration.EngineNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.EngineNo = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    vehicleRegistration.ChassisNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ChassisNo = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    vehicleRegistration.Status = 0;
                                }
                                else
                                {
                                    vehicleRegistration.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    vehicleRegistration.PlateNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.PlateNo = myReader.GetString(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    vehicleRegistration.CertReg = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.CertReg = myReader.GetString(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    vehicleRegistration.OwnerReg = null;
                                }
                                else
                                {
                                    //vehicleRegistration.OwnerReg = customerManager.GetCustomer(myReader.GetInt32(9));
                                    vehicleRegistration.OwnerReg = Customer.CreateCustomer();
                                    vehicleRegistration.OwnerReg.ID = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    vehicleRegistration.Remarks = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Remarks = myReader.GetString(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    vehicleRegistration.Code = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Code = myReader.GetString(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    vehicleRegistration.ReferenceNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ReferenceNo = myReader.GetString(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    vehicleRegistration.ConfirmLTO = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ConfirmLTO = myReader.GetString(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    vehicleRegistration.ConfirmationFieldOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ConfirmationFieldOffice = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    vehicleRegistration.ConfirmationAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.ConfirmationAmount = myReader.GetDecimal(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    vehicleRegistration.SOP = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.SOP = myReader.GetString(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    vehicleRegistration.SOPAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.SOPAmount = myReader.GetDecimal(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    vehicleRegistration.Registered = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Registered = myReader.GetString(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    vehicleRegistration.Insured = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Insured = myReader.GetString(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    vehicleRegistration.ORNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ORNo = myReader.GetString(20);
                                }
                                if (myReader.IsDBNull(21))
                                {
                                    vehicleRegistration.CRNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.CRNo = myReader.GetString(21);
                                }
                                if (myReader.IsDBNull(22))
                                {
                                    vehicleRegistration.RegistrationAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.RegistrationAmount = myReader.GetDecimal(22);
                                }
                                if (myReader.IsDBNull(23))
                                {
                                    vehicleRegistration.FileNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.FileNo = myReader.GetString(23);
                                }
                                if (myReader.IsDBNull(24))
                                {
                                    vehicleRegistration.SINo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.SINo = myReader.GetString(24);
                                }
                                if (myReader.IsDBNull(25))
                                {
                                    vehicleRegistration.DistrictOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.DistrictOffice = myReader.GetString(25);
                                }
                                if (myReader.IsDBNull(26))
                                {
                                    vehicleRegistration.Clearance = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Clearance = myReader.GetString(26);
                                }
                                if (myReader.IsDBNull(27))
                                {
                                    vehicleRegistration.ClearanceNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ClearanceNo = myReader.GetString(27);
                                }
                                if (myReader.IsDBNull(28))
                                {
                                    vehicleRegistration.DateRegistration = DateTime.Today;
                                }
                                else
                                {
                                    vehicleRegistration.DateRegistration = myReader.GetDateTime(28);
                                }
                                if (myReader.IsDBNull(29))
                                {
                                    vehicleRegistration.RegistrationFieldOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.RegistrationFieldOffice = myReader.GetString(29);
                                }
                                if (myReader.IsDBNull(30))
                                {
                                    vehicleRegistration.InsuranceNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.InsuranceNo = myReader.GetString(30);
                                }
                                if (myReader.IsDBNull(31))
                                {
                                    vehicleRegistration.InsuranceAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.InsuranceAmount = myReader.GetDecimal(31);
                                }
                                if (myReader.IsDBNull(32))
                                {
                                    vehicleRegistration.DateRegistered = DateTime.Today;
                                }
                                else
                                {
                                    vehicleRegistration.DateRegistered = myReader.GetDateTime(32);
                                }
                                if (myReader.IsDBNull(33))
                                {
                                    vehicleRegistration.FieldOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.FieldOffice = myReader.GetString(33);
                                }
                                if (myReader.IsDBNull(34))
                                {
                                    vehicleRegistration.MVFileNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.MVFileNo = myReader.GetString(34);
                                }
                                allVehicle.Add(vehicleRegistration);
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
            return allVehicle;
        }

        public GenericList<VehicleRegistration> GetAllVehicleRegistration(Int32 pageNo, SortByVehicle sortBy, SortingOrder sortOrder)
        {
            GenericList<VehicleRegistration> allVehicle = new GenericList<VehicleRegistration>();
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
            myCommand.CommandText = "VehicleSelect";
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
                                VehicleRegistration vehicleRegistration = VehicleRegistration.CreateVehicleRegistration();
                                vehicleRegistration.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    vehicleRegistration.Brand = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Brand = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    vehicleRegistration.Model = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Model = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    vehicleRegistration.Color = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Color = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    vehicleRegistration.EngineNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.EngineNo = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    vehicleRegistration.ChassisNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ChassisNo = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    vehicleRegistration.Status = 0;
                                }
                                else
                                {
                                    vehicleRegistration.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    vehicleRegistration.PlateNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.PlateNo = myReader.GetString(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    vehicleRegistration.CertReg = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.CertReg = myReader.GetString(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    vehicleRegistration.OwnerReg = null;
                                }
                                else
                                {
                                    vehicleRegistration.OwnerReg = customerManager.GetCustomer(myReader.GetInt32(9));
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    vehicleRegistration.Remarks = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Remarks = myReader.GetString(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    vehicleRegistration.Code = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Code = myReader.GetString(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    vehicleRegistration.ReferenceNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ReferenceNo = myReader.GetString(12);
                                }
                                if (myReader.IsDBNull(13))
                                {
                                    vehicleRegistration.ConfirmLTO = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ConfirmLTO = myReader.GetString(13);
                                }
                                if (myReader.IsDBNull(14))
                                {
                                    vehicleRegistration.ConfirmationFieldOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ConfirmationFieldOffice = myReader.GetString(14);
                                }
                                if (myReader.IsDBNull(15))
                                {
                                    vehicleRegistration.ConfirmationAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.ConfirmationAmount = myReader.GetDecimal(15);
                                }
                                if (myReader.IsDBNull(16))
                                {
                                    vehicleRegistration.SOP = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.SOP = myReader.GetString(16);
                                }
                                if (myReader.IsDBNull(17))
                                {
                                    vehicleRegistration.SOPAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.SOPAmount = myReader.GetDecimal(17);
                                }
                                if (myReader.IsDBNull(18))
                                {
                                    vehicleRegistration.Registered = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Registered = myReader.GetString(18);
                                }
                                if (myReader.IsDBNull(19))
                                {
                                    vehicleRegistration.Insured = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Insured = myReader.GetString(19);
                                }
                                if (myReader.IsDBNull(20))
                                {
                                    vehicleRegistration.ORNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ORNo = myReader.GetString(20);
                                }
                                if (myReader.IsDBNull(21))
                                {
                                    vehicleRegistration.CRNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.CRNo = myReader.GetString(21);
                                }
                                if (myReader.IsDBNull(22))
                                {
                                    vehicleRegistration.RegistrationAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.RegistrationAmount = myReader.GetDecimal(22);
                                }
                                if (myReader.IsDBNull(23))
                                {
                                    vehicleRegistration.FileNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.FileNo = myReader.GetString(23);
                                }
                                if (myReader.IsDBNull(24))
                                {
                                    vehicleRegistration.SINo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.SINo = myReader.GetString(24);
                                }
                                if (myReader.IsDBNull(25))
                                {
                                    vehicleRegistration.DistrictOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.DistrictOffice = myReader.GetString(25);
                                }
                                if (myReader.IsDBNull(26))
                                {
                                    vehicleRegistration.Clearance = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.Clearance = myReader.GetString(26);
                                }
                                if (myReader.IsDBNull(27))
                                {
                                    vehicleRegistration.ClearanceNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.ClearanceNo = myReader.GetString(27);
                                }
                                if (myReader.IsDBNull(28))
                                {
                                    vehicleRegistration.DateRegistration = DateTime.Today;
                                }
                                else
                                {
                                    vehicleRegistration.DateRegistration = myReader.GetDateTime(28);
                                }
                                if (myReader.IsDBNull(29))
                                {
                                    vehicleRegistration.RegistrationFieldOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.RegistrationFieldOffice = myReader.GetString(29);
                                }
                                if (myReader.IsDBNull(30))
                                {
                                    vehicleRegistration.InsuranceNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.InsuranceNo = myReader.GetString(30);
                                }
                                if (myReader.IsDBNull(31))
                                {
                                    vehicleRegistration.InsuranceAmount = 0;
                                }
                                else
                                {
                                    vehicleRegistration.InsuranceAmount = myReader.GetDecimal(31);
                                }
                                if (myReader.IsDBNull(32))
                                {
                                    vehicleRegistration.DateRegistered = DateTime.Today;
                                }
                                else
                                {
                                    vehicleRegistration.DateRegistered = myReader.GetDateTime(32);
                                }
                                if (myReader.IsDBNull(33))
                                {
                                    vehicleRegistration.FieldOffice = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.FieldOffice = myReader.GetString(33);
                                }
                                if (myReader.IsDBNull(34))
                                {
                                    vehicleRegistration.MVFileNo = String.Empty;
                                }
                                else
                                {
                                    vehicleRegistration.MVFileNo = myReader.GetString(34);
                                }
                                allVehicle.Add(vehicleRegistration);
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
            return allVehicle;
        }

        public VehicleRegistration InsertVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@Brand", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.Brand))
            {
                myParam1.Value = DBNull.Value;
            }
            else
            {
                myParam1.Value = vehicleRegistration.Brand;
            }
            SqlParameter myParam2 = new SqlParameter("@Model", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.Model))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = vehicleRegistration.Model;
            }
            SqlParameter myParam3 = new SqlParameter("@Color", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.Color))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = vehicleRegistration.Color;
            }
            SqlParameter myParam4 = new SqlParameter("@EngineNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.EngineNo))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = vehicleRegistration.EngineNo;
            }
            SqlParameter myParam5 = new SqlParameter("@ChassisNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.ChassisNo))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = vehicleRegistration.ChassisNo;
            }
            SqlParameter myParam6 = new SqlParameter("@Status", SqlDbType.Int);
            //if (vehicleRegistration.Status == null)
            //{
            //    myParam6.Value = DBNull.Value;
            //}
            //else
            //{
                myParam6.Value = vehicleRegistration.Status;
            //}
            SqlParameter myParam7 = new SqlParameter("@PlateNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.PlateNo))
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = vehicleRegistration.PlateNo;
            }
            SqlParameter myParam8 = new SqlParameter("@CertReg", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.CertReg))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = vehicleRegistration.CertReg;
            }
            SqlParameter myParam9 = new SqlParameter("@OwnerRegID", SqlDbType.Int);
            if (vehicleRegistration.OwnerReg == null)
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = vehicleRegistration.OwnerReg.ID;
            }
            SqlParameter myParam10 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(vehicleRegistration.Remarks))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = vehicleRegistration.Remarks;
            }
            SqlParameter myParam11 = new SqlParameter("@VehicleCode", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(vehicleRegistration.Code))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = vehicleRegistration.Code;
            }
            SqlParameter myParam12 = new SqlParameter("@ReferenceNo", SqlDbType.Int);
            if (vehicleRegistration.ReferenceNo == null)
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = vehicleRegistration.ReferenceNo;
            }
            SqlParameter myParam13 = new SqlParameter("@ConfirmLTO", SqlDbType.Int);
            if (vehicleRegistration.ConfirmLTO == null)
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = vehicleRegistration.ConfirmLTO;
            }
            SqlParameter myParam14 = new SqlParameter("@ConfirmationFieldOffice", SqlDbType.VarChar, 255);
            if (String.IsNullOrEmpty(vehicleRegistration.ConfirmationFieldOffice))
            {
                myParam14.Value = DBNull.Value;
            }
            else
            {
                myParam14.Value = vehicleRegistration.ConfirmationFieldOffice;
            }
            SqlParameter myParam15 = new SqlParameter("@ConfirmationAmount", SqlDbType.Decimal);
            if (vehicleRegistration.ConfirmationAmount == 0)
            {
                myParam15.Value = DBNull.Value;
            }
            else
            {
                myParam15.Value = vehicleRegistration.ConfirmationAmount;
            }
            SqlParameter myParam16 = new SqlParameter("@SOP", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.SOP))
            {
                myParam16.Value = DBNull.Value;
            }
            else
            {
                myParam16.Value = vehicleRegistration.SOP;
            }
            SqlParameter myParam17 = new SqlParameter("@SOPAmount", SqlDbType.Decimal);
            if (vehicleRegistration.SOPAmount == 0)
            {
                myParam17.Value = DBNull.Value;
            }
            else
            {
                myParam17.Value = vehicleRegistration.SOPAmount;
            }
            SqlParameter myParam18 = new SqlParameter("@Registered", SqlDbType.Int);
            if (vehicleRegistration.Registered == null)
            {
                myParam18.Value = DBNull.Value;
            }
            else
            {
                myParam18.Value = vehicleRegistration.Registered;
            }
            SqlParameter myParam19 = new SqlParameter("@Insured", SqlDbType.Int);
            if (vehicleRegistration.Insured == null)
            {
                myParam19.Value = DBNull.Value;
            }
            else
            {
                myParam19.Value = vehicleRegistration.Insured;
            }
            SqlParameter myParam20 = new SqlParameter("@ORNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.ORNo))
            {
                myParam20.Value = DBNull.Value;
            }
            else
            {
                myParam20.Value = vehicleRegistration.ORNo;
            }
            SqlParameter myParam21 = new SqlParameter("@CRNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.CRNo))
            {
                myParam21.Value = DBNull.Value;
            }
            else
            {
                myParam21.Value = vehicleRegistration.CRNo;
            }
            SqlParameter myParam22 = new SqlParameter("@RegistrationAmount", SqlDbType.Decimal);
            if (vehicleRegistration.RegistrationAmount == 0)
            {
                myParam22.Value = DBNull.Value;
            }
            else
            {
                myParam22.Value = vehicleRegistration.RegistrationAmount;
            }
            SqlParameter myParam23 = new SqlParameter("@FileNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.FileNo))
            {
                myParam23.Value = DBNull.Value;
            }
            else
            {
                myParam23.Value = vehicleRegistration.FileNo;
            }
            SqlParameter myParam24 = new SqlParameter("@SINo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.SINo))
            {
                myParam24.Value = DBNull.Value;
            }
            else
            {
                myParam24.Value = vehicleRegistration.SINo;
            }
            SqlParameter myParam25 = new SqlParameter("@DistrictOffice", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.DistrictOffice))
            {
                myParam25.Value = DBNull.Value;
            }
            else
            {
                myParam25.Value = vehicleRegistration.DistrictOffice;
            }
            SqlParameter myParam26 = new SqlParameter("@Clearance", SqlDbType.Int);
            if (vehicleRegistration.Clearance == null)
            {
                myParam26.Value = DBNull.Value;
            }
            else
            {
                myParam26.Value = vehicleRegistration.Clearance;
            }
            SqlParameter myParam27 = new SqlParameter("@ClearanceNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.ClearanceNo))
            {
                myParam27.Value = DBNull.Value;
            }
            else
            {
                myParam27.Value = vehicleRegistration.ClearanceNo;
            }
            SqlParameter myParam28 = new SqlParameter("@DateRegistration", SqlDbType.DateTime);
            myParam28.Value = vehicleRegistration.DateRegistration;
            SqlParameter myParam29 = new SqlParameter("@RegistrationFieldOffice", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.RegistrationFieldOffice))
            {
                myParam29.Value = DBNull.Value;
            }
            else
            {
                myParam29.Value = vehicleRegistration.RegistrationFieldOffice;
            }
            SqlParameter myParam30 = new SqlParameter("@InsuranceNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.InsuranceNo))
            {
                myParam30.Value = DBNull.Value;
            }
            else
            {
                myParam30.Value = vehicleRegistration.InsuranceNo;
            }
            SqlParameter myParam31 = new SqlParameter("@InsuranceAmount", SqlDbType.Decimal);
            if (vehicleRegistration.InsuranceAmount == 0)
            {
                myParam31.Value = DBNull.Value;
            }
            else
            {
                myParam31.Value = vehicleRegistration.InsuranceAmount;
            }
            SqlParameter myParam32 = new SqlParameter("@DateRegistered", SqlDbType.DateTime);
            myParam32.Value = vehicleRegistration.DateRegistered;
            SqlParameter myParam33 = new SqlParameter("@FieldOffice", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.FieldOffice))
            {
                myParam33.Value = DBNull.Value;
            }
            else
            {
                myParam33.Value = vehicleRegistration.FieldOffice;
            }
            SqlParameter myParam34 = new SqlParameter("@MVFileNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.MVFileNo))
            {
                myParam34.Value = DBNull.Value;
            }
            else
            {
                myParam34.Value = vehicleRegistration.MVFileNo;
            }
            SqlParameter myParam35 = new SqlParameter("@Output", SqlDbType.Int);
            myParam35.Direction = ParameterDirection.Output;
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
            myCommand.Parameters.Add(myParam24);
            myCommand.Parameters.Add(myParam25);
            myCommand.Parameters.Add(myParam26);
            myCommand.Parameters.Add(myParam27);
            myCommand.Parameters.Add(myParam28);
            myCommand.Parameters.Add(myParam29);
            myCommand.Parameters.Add(myParam30);
            myCommand.Parameters.Add(myParam31);
            myCommand.Parameters.Add(myParam32);
            myCommand.Parameters.Add(myParam33);
            myCommand.Parameters.Add(myParam34);
            myCommand.Parameters.Add(myParam35);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    vehicleRegistration.ID = Convert.ToInt32(myParam35.Value);
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
            return vehicleRegistration;
        }

        public Boolean UpdateVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = vehicleRegistration.ID;
            SqlParameter myParam2 = new SqlParameter("@Brand", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(vehicleRegistration.Brand))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = vehicleRegistration.Brand;
            }
            SqlParameter myParam3 = new SqlParameter("@Model", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(vehicleRegistration.Model))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = vehicleRegistration.Model;
            }
            SqlParameter myParam4 = new SqlParameter("@Color", SqlDbType.VarChar, 35);
            if (String.IsNullOrEmpty(vehicleRegistration.Color))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = vehicleRegistration.Color;
            }
            SqlParameter myParam5 = new SqlParameter("@EngineNo", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(vehicleRegistration.EngineNo))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = vehicleRegistration.EngineNo;
            }
            SqlParameter myParam6 = new SqlParameter("@ChassisNo", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(vehicleRegistration.ChassisNo))
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = vehicleRegistration.ChassisNo;
            }
            SqlParameter myParam7 = new SqlParameter("@Status", SqlDbType.Int);
            //if (vehicleRegistration.Status == 0)
            //{
            //    myParam7.Value = DBNull.Value;
            //}
            //else
            //{
                myParam7.Value = vehicleRegistration.Status;
            //}
            SqlParameter myParam8 = new SqlParameter("@PlateNo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(vehicleRegistration.PlateNo))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = vehicleRegistration.PlateNo;
            }
            SqlParameter myParam9 = new SqlParameter("@CertReg", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(vehicleRegistration.CertReg))
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = vehicleRegistration.CertReg;
            }
            SqlParameter myParam10 = new SqlParameter("@OwnerRegID", SqlDbType.Int);
            if (vehicleRegistration.OwnerReg == null)
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = vehicleRegistration.OwnerReg.ID;
            }
            SqlParameter myParam11 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(vehicleRegistration.Remarks))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = vehicleRegistration.Remarks;
            }
            SqlParameter myParam12 = new SqlParameter("@VehicleCode", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(vehicleRegistration.Code))
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = vehicleRegistration.Code;
            }
            SqlParameter myParam13 = new SqlParameter("@ReferenceNo", SqlDbType.Int);
            if (vehicleRegistration.ReferenceNo == null)
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = vehicleRegistration.ReferenceNo;
            }
            SqlParameter myParam14 = new SqlParameter("@ConfirmLTO", SqlDbType.Int);
            if (vehicleRegistration.ConfirmLTO == null)
            {
                myParam14.Value = DBNull.Value;
            }
            else
            {
                myParam14.Value = vehicleRegistration.ConfirmLTO;
            }
            SqlParameter myParam15 = new SqlParameter("@ConfirmationFieldOffice", SqlDbType.VarChar, 255);
            if (String.IsNullOrEmpty(vehicleRegistration.ConfirmationFieldOffice))
            {
                myParam15.Value = DBNull.Value;
            }
            else
            {
                myParam15.Value = vehicleRegistration.ConfirmationFieldOffice;
            }
            SqlParameter myParam16 = new SqlParameter("@ConfirmationAmount", SqlDbType.Decimal);
            if (vehicleRegistration.ConfirmationAmount == 0)
            {
                myParam16.Value = DBNull.Value;
            }
            else
            {
                myParam16.Value = vehicleRegistration.ConfirmationAmount;
            }
            SqlParameter myParam17 = new SqlParameter("@SOP", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.SOP))
            {
                myParam17.Value = DBNull.Value;
            }
            else
            {
                myParam17.Value = vehicleRegistration.SOP;
            }
            SqlParameter myParam18 = new SqlParameter("@SOPAmount", SqlDbType.Decimal);
            if (vehicleRegistration.SOPAmount == 0)
            {
                myParam18.Value = DBNull.Value;
            }
            else
            {
                myParam18.Value = vehicleRegistration.SOPAmount;
            }
            SqlParameter myParam19 = new SqlParameter("@Registered", SqlDbType.Int);
            if (vehicleRegistration.Registered == null)
            {
                myParam19.Value = DBNull.Value;
            }
            else
            {
                myParam19.Value = vehicleRegistration.Registered;
            }
            SqlParameter myParam20 = new SqlParameter("@Insured", SqlDbType.Int);
            if (vehicleRegistration.Insured == null)
            {
                myParam20.Value = DBNull.Value;
            }
            else
            {
                myParam20.Value = vehicleRegistration.Insured;
            }
            SqlParameter myParam21 = new SqlParameter("@ORNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.ORNo))
            {
                myParam21.Value = DBNull.Value;
            }
            else
            {
                myParam21.Value = vehicleRegistration.ORNo;
            }
            SqlParameter myParam22 = new SqlParameter("@CRNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.CRNo))
            {
                myParam22.Value = DBNull.Value;
            }
            else
            {
                myParam22.Value = vehicleRegistration.CRNo;
            }
            SqlParameter myParam23 = new SqlParameter("@RegistrationAmount", SqlDbType.Decimal);
            if (vehicleRegistration.RegistrationAmount == 0)
            {
                myParam23.Value = DBNull.Value;
            }
            else
            {
                myParam23.Value = vehicleRegistration.RegistrationAmount;
            }
            SqlParameter myParam24 = new SqlParameter("@FileNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.FileNo))
            {
                myParam24.Value = DBNull.Value;
            }
            else
            {
                myParam24.Value = vehicleRegistration.FileNo;
            }
            SqlParameter myParam25 = new SqlParameter("@SINo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.SINo))
            {
                myParam25.Value = DBNull.Value;
            }
            else
            {
                myParam25.Value = vehicleRegistration.SINo;
            }
            SqlParameter myParam26 = new SqlParameter("@DistrictOffice", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.DistrictOffice))
            {
                myParam26.Value = DBNull.Value;
            }
            else
            {
                myParam26.Value = vehicleRegistration.DistrictOffice;
            }
            SqlParameter myParam27 = new SqlParameter("@Clearance", SqlDbType.Int);
            if (vehicleRegistration.Clearance == null)
            {
                myParam27.Value = DBNull.Value;
            }
            else
            {
                myParam27.Value = vehicleRegistration.Clearance;
            }
            SqlParameter myParam28 = new SqlParameter("@ClearanceNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.ClearanceNo))
            {
                myParam28.Value = DBNull.Value;
            }
            else
            {
                myParam28.Value = vehicleRegistration.ClearanceNo;
            }
            SqlParameter myParam29 = new SqlParameter("@DateRegistration", SqlDbType.DateTime);
            myParam29.Value = vehicleRegistration.DateRegistration;
            SqlParameter myParam30 = new SqlParameter("@RegistrationFieldOffice", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.RegistrationFieldOffice))
            {
                myParam30.Value = DBNull.Value;
            }
            else
            {
                myParam30.Value = vehicleRegistration.RegistrationFieldOffice;
            }
            SqlParameter myParam31 = new SqlParameter("@InsuranceNo", SqlDbType.VarChar, 50);
            if (String.IsNullOrEmpty(vehicleRegistration.InsuranceNo))
            {
                myParam31.Value = DBNull.Value;
            }
            else
            {
                myParam31.Value = vehicleRegistration.InsuranceNo;
            }
            SqlParameter myParam32 = new SqlParameter("@InsuranceAmount", SqlDbType.Decimal);
            if (vehicleRegistration.InsuranceAmount == 0)
            {
                myParam32.Value = DBNull.Value;
            }
            else
            {
                myParam32.Value = vehicleRegistration.InsuranceAmount;
            }
            SqlParameter myParam33 = new SqlParameter("@DateRegistered", SqlDbType.DateTime);
            myParam33.Value = vehicleRegistration.DateRegistered;
            SqlParameter myParam34 = new SqlParameter("@FieldOffice", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.FieldOffice))
            {
                myParam34.Value = DBNull.Value;
            }
            else
            {
                myParam34.Value = vehicleRegistration.FieldOffice;
            }
            SqlParameter myParam35 = new SqlParameter("@MVFileNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicleRegistration.MVFileNo))
            {
                myParam35.Value = DBNull.Value;
            }
            else
            {
                myParam35.Value = vehicleRegistration.MVFileNo;
            }
            SqlParameter myParam36 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam36.Direction = ParameterDirection.ReturnValue;
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
            myCommand.Parameters.Add(myParam24);
            myCommand.Parameters.Add(myParam25);
            myCommand.Parameters.Add(myParam26);
            myCommand.Parameters.Add(myParam27);
            myCommand.Parameters.Add(myParam28);
            myCommand.Parameters.Add(myParam29);
            myCommand.Parameters.Add(myParam30);
            myCommand.Parameters.Add(myParam31);
            myCommand.Parameters.Add(myParam32);
            myCommand.Parameters.Add(myParam33);
            myCommand.Parameters.Add(myParam34);
            myCommand.Parameters.Add(myParam35);
            myCommand.Parameters.Add(myParam36);
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
            if (Convert.ToInt32(myParam36.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = vehicleRegistration.ID;
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
