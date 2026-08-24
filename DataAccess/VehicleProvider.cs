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
    public class VehicleProvider : IVehicleProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Boolean _isLocal;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public VehicleProvider(SqlConnection conn)
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
        public Int32 GetVehiclePageCount()
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

        public Vehicle GetVehicle(Int32 id)
        {
            Vehicle vehicle = Vehicle.CreateVehicle();
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
                            vehicle.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                vehicle.Brand = String.Empty;
                            }
                            else
                            {
                                vehicle.Brand = myReader.GetString(1);
                            }
                            if (myReader.IsDBNull(2))
                            {
                                vehicle.Model = String.Empty;
                            }
                            else
                            {
                                vehicle.Model = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                vehicle.Color = String.Empty;
                            }
                            else
                            {
                                vehicle.Color = myReader.GetString(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                vehicle.EngineNo = String.Empty;
                            }
                            else
                            {
                                vehicle.EngineNo = myReader.GetString(4);
                            }
                            if (myReader.IsDBNull(5))
                            {
                                vehicle.ChassisNo = String.Empty;
                            }
                            else
                            {
                                vehicle.ChassisNo = myReader.GetString(5);
                            }
                            if (myReader.IsDBNull(6))
                            {
                                vehicle.Status = 0;
                            }
                            else
                            {
                                vehicle.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(6).ToString());
                            }
                            if (myReader.IsDBNull(7))
                            {
                                vehicle.PlateNo = String.Empty;
                            }
                            else
                            {
                                vehicle.PlateNo = myReader.GetString(7);
                            }
                            if (myReader.IsDBNull(8))
                            {
                                vehicle.CertReg = String.Empty;
                            }
                            else
                            {
                                vehicle.CertReg = myReader.GetString(8);
                            }
                            if (myReader.IsDBNull(9))
                            {
                                vehicle.OwnerReg = null;
                            }
                            else
                            {
                                vehicle.OwnerReg = customerManager.GetCustomer(myReader.GetInt32(9));
                            }
                            if (myReader.IsDBNull(10))
                            {
                                vehicle.Remarks = String.Empty;
                            }
                            else
                            {
                                vehicle.Remarks = myReader.GetString(10);
                            }
                            if (myReader.IsDBNull(11))
                            {
                                vehicle.Code = String.Empty;
                            }
                            else
                            {
                                vehicle.Code = myReader.GetString(11);
                            }
                            if (myReader.FieldCount > 12)
                            {
                                if (myReader.IsDBNull(12))
                                {
                                    vehicle.Branch = 0;
                                }
                                else
                                {
                                    vehicle.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(12).ToString());
                                }
                            }
                            if (myReader.FieldCount > 13)
                            {
                                if (myReader.IsDBNull(13))
                                {
                                    vehicle.AuditID = 0;
                                }
                                else
                                {
                                    vehicle.AuditID = myReader.GetInt32(13);
                                }
                            }
                            vehicle.Branch = this._branch;
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
            return vehicle;
        }

        public GenericList<Vehicle> GetAllVehicle()
        {
            GenericList<Vehicle> allVehicle = new GenericList<Vehicle>();
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
                                Vehicle vehicle = Vehicle.CreateVehicle();
                                vehicle.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    vehicle.Brand = String.Empty;
                                }
                                else
                                {
                                    vehicle.Brand = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    vehicle.Model = String.Empty;
                                }
                                else
                                {
                                    vehicle.Model = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    vehicle.Color = String.Empty;
                                }
                                else
                                {
                                    vehicle.Color = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    vehicle.EngineNo = String.Empty;
                                }
                                else
                                {
                                    vehicle.EngineNo = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    vehicle.ChassisNo = String.Empty;
                                }
                                else
                                {
                                    vehicle.ChassisNo = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    vehicle.Status = 0;
                                }
                                else
                                {
                                    vehicle.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    vehicle.PlateNo = String.Empty;
                                }
                                else
                                {
                                    vehicle.PlateNo = myReader.GetString(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    vehicle.CertReg = String.Empty;
                                }
                                else
                                {
                                    vehicle.CertReg = myReader.GetString(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    vehicle.OwnerReg = null;
                                }
                                else
                                {
                                    //vehicle.OwnerReg = customerManager.GetCustomer(myReader.GetInt32(9));
                                    vehicle.OwnerReg = Customer.CreateCustomer();
                                    vehicle.OwnerReg.ID = myReader.GetInt32(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    vehicle.Remarks = String.Empty;
                                }
                                else
                                {
                                    vehicle.Remarks = myReader.GetString(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    vehicle.Code = String.Empty;
                                }
                                else
                                {
                                    vehicle.Code = myReader.GetString(11);
                                }
                                if (myReader.FieldCount > 12)
                                {
                                    if (myReader.IsDBNull(12))
                                    {
                                        vehicle.Branch = 0;
                                    }
                                    else
                                    {
                                        vehicle.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(12).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 13)
                                {
                                    if (myReader.IsDBNull(13))
                                    {
                                        vehicle.AuditID = 0;
                                    }
                                    else
                                    {
                                        vehicle.AuditID = myReader.GetInt32(13);
                                    }
                                }
                                allVehicle.Add(vehicle);
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

        public GenericList<Vehicle> GetAllVehicle(Int32 pageNo, SortByVehicle sortBy, SortingOrder sortOrder)
        {
            GenericList<Vehicle> allVehicle = new GenericList<Vehicle>();
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
                                Vehicle vehicle = Vehicle.CreateVehicle();
                                vehicle.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    vehicle.Brand = String.Empty;
                                }
                                else
                                {
                                    vehicle.Brand = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    vehicle.Model = String.Empty;
                                }
                                else
                                {
                                    vehicle.Model = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    vehicle.Color = String.Empty;
                                }
                                else
                                {
                                    vehicle.Color = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    vehicle.EngineNo = String.Empty;
                                }
                                else
                                {
                                    vehicle.EngineNo = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    vehicle.ChassisNo = String.Empty;
                                }
                                else
                                {
                                    vehicle.ChassisNo = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    vehicle.Status = 0;
                                }
                                else
                                {
                                    vehicle.Status = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), myReader.GetInt32(6).ToString());
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    vehicle.PlateNo = String.Empty;
                                }
                                else
                                {
                                    vehicle.PlateNo = myReader.GetString(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    vehicle.CertReg = String.Empty;
                                }
                                else
                                {
                                    vehicle.CertReg = myReader.GetString(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    vehicle.OwnerReg = null;
                                }
                                else
                                {
                                    vehicle.OwnerReg = customerManager.GetCustomer(myReader.GetInt32(9));
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    vehicle.Remarks = String.Empty;
                                }
                                else
                                {
                                    vehicle.Remarks = myReader.GetString(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    vehicle.Code = String.Empty;
                                }
                                else
                                {
                                    vehicle.Code = myReader.GetString(11);
                                }
                                if (myReader.FieldCount > 12)
                                {
                                    if (myReader.IsDBNull(12))
                                    {
                                        vehicle.Branch = 0;
                                    }
                                    else
                                    {
                                        vehicle.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(12).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 13)
                                {
                                    if (myReader.IsDBNull(13))
                                    {
                                        vehicle.AuditID = 0;
                                    }
                                    else
                                    {
                                        vehicle.AuditID = myReader.GetInt32(13);
                                    }
                                }
                                allVehicle.Add(vehicle);
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

        public Vehicle InsertVehicle(Vehicle vehicle)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@Brand", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.Brand))
            {
                myParam1.Value = DBNull.Value;
            }
            else
            {
                myParam1.Value = vehicle.Brand;
            }
            SqlParameter myParam2 = new SqlParameter("@Model", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.Model))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = vehicle.Model;
            }
            SqlParameter myParam3 = new SqlParameter("@Color", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.Color))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = vehicle.Color;
            }
            SqlParameter myParam4 = new SqlParameter("@EngineNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.EngineNo))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = vehicle.EngineNo;
            }
            SqlParameter myParam5 = new SqlParameter("@ChassisNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.ChassisNo))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = vehicle.ChassisNo;
            }
            SqlParameter myParam6 = new SqlParameter("@Status", SqlDbType.Int);
            //if (vehicle.Status == null)
            //{
            //    myParam6.Value = DBNull.Value;
            //}
            //else
            //{
                myParam6.Value = vehicle.Status;
            //}
            SqlParameter myParam7 = new SqlParameter("@PlateNo", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.PlateNo))
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = vehicle.PlateNo;
            }
            SqlParameter myParam8 = new SqlParameter("@CertReg", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(vehicle.CertReg))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = vehicle.CertReg;
            }
            SqlParameter myParam9 = new SqlParameter("@OwnerRegID", SqlDbType.Int);
            if (vehicle.OwnerReg == null)
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = vehicle.OwnerReg.ID;
            }
            SqlParameter myParam10 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(vehicle.Remarks))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = vehicle.Remarks;
            }
            SqlParameter myParam11 = new SqlParameter("@VehicleCode", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(vehicle.Code))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = vehicle.Code;
            }
            SqlParameter myParam12 = new SqlParameter("@BranchID", SqlDbType.Int);
            myParam12.Value = (Int32)vehicle.Branch;
            SqlParameter myParam13 = new SqlParameter("@AuditID", SqlDbType.Int);
            myParam13.Value = vehicle.AuditID;
            SqlParameter myParam14 = new SqlParameter("@Output", SqlDbType.Int);
            myParam14.Direction = ParameterDirection.Output;
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
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    vehicle.ID = Convert.ToInt32(myParam14.Value);
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
            return vehicle;
        }

        public Boolean UpdateVehicle(Vehicle vehicle)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = vehicle.ID;
            SqlParameter myParam2 = new SqlParameter("@Brand", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(vehicle.Brand))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = vehicle.Brand;
            }
            SqlParameter myParam3 = new SqlParameter("@Model", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(vehicle.Model))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = vehicle.Model;
            }
            SqlParameter myParam4 = new SqlParameter("@Color", SqlDbType.VarChar, 35);
            if (String.IsNullOrEmpty(vehicle.Color))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = vehicle.Color;
            }
            SqlParameter myParam5 = new SqlParameter("@EngineNo", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(vehicle.EngineNo))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = vehicle.EngineNo;
            }
            SqlParameter myParam6 = new SqlParameter("@ChassisNo", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(vehicle.ChassisNo))
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = vehicle.ChassisNo;
            }
            SqlParameter myParam7 = new SqlParameter("@Status", SqlDbType.Int);
            //if (vehicle.Status == 0)
            //{
            //    myParam7.Value = DBNull.Value;
            //}
            //else
            //{
                myParam7.Value = vehicle.Status;
            //}
            SqlParameter myParam8 = new SqlParameter("@PlateNo", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(vehicle.PlateNo))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = vehicle.PlateNo;
            }
            SqlParameter myParam9 = new SqlParameter("@CertReg", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(vehicle.CertReg))
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = vehicle.CertReg;
            }
            SqlParameter myParam10 = new SqlParameter("@OwnerRegID", SqlDbType.Int);
            if (vehicle.OwnerReg == null)
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = vehicle.OwnerReg.ID;
            }
            SqlParameter myParam11 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(vehicle.Remarks))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = vehicle.Remarks;
            }
            SqlParameter myParam12 = new SqlParameter("@VehicleCode", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(vehicle.Code))
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = vehicle.Code;
            }
            SqlParameter myParam13 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam13.Direction = ParameterDirection.ReturnValue;
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
            if (Convert.ToInt32(myParam13.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteVehicle(Vehicle vehicle)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "VehicleDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = vehicle.ID;
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
