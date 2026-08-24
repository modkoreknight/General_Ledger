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
    public class EmployeeProvider : IEmployeeProvider
    {
        #region Fields
        private SqlConnection _conn;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public EmployeeProvider(SqlConnection conn)
        {
            this._conn = conn;
        }
        #endregion

        #region Methods
        public Int32 GetEmployeePageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeGetPageCount";
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

        public Employee GetEmployee(Int32 id)
        {
            Employee employee = Employee.CreateEmployee();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeSelect";
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
                            employee.ID = myReader.GetInt32(0);
                            if (myReader.IsDBNull(1))
                            {
                                employee.EmployeeNo = String.Empty;
                            }
                            else
                            {
                                employee.EmployeeNo = myReader.GetString(1);
                            }
                            if (myReader.IsDBNull(2))
                            {
                                employee.LastName = String.Empty;
                            }
                            else
                            {
                                employee.LastName = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                employee.FirstName = String.Empty;
                            }
                            else
                            {
                                employee.FirstName = myReader.GetString(3);
                            }
                            if (myReader.IsDBNull(4))
                            {
                                employee.MiddleName = String.Empty;
                            }
                            else
                            {
                                employee.MiddleName = myReader.GetString(4);
                            }
                            if (myReader.IsDBNull(5))
                            {
                                employee.Address = String.Empty;
                            }
                            else
                            {
                                employee.Address = myReader.GetString(5);
                            }
                            if (myReader.IsDBNull(6))
                            {
                                employee.Phone = String.Empty;
                            }
                            else
                            {
                                employee.Phone = myReader.GetString(6);
                            }
                            if (myReader.IsDBNull(7))
                            {
                                employee.BirthDate = DateTime.MinValue;
                            }
                            else
                            {
                                employee.BirthDate = myReader.GetDateTime(7);
                            }
                            if (myReader.IsDBNull(8))
                            {
                                employee.UserName = String.Empty;
                            }
                            else
                            {
                                employee.UserName = myReader.GetString(8);
                            }
                            if (myReader.IsDBNull(9))
                            {
                                employee.Password = String.Empty;
                            }
                            else
                            {
                                employee.Password = myReader.GetString(9);
                            }
                            if (myReader.IsDBNull(10))
                            {
                                employee.Salt = String.Empty;
                            }
                            else
                            {
                                employee.Salt = myReader.GetString(10);
                            }
                            if (myReader.IsDBNull(11))
                            {
                                employee.PictureFile = String.Empty;
                            }
                            else
                            {
                                employee.PictureFile = myReader.GetString(11);
                            }
                            if (myReader.IsDBNull(12))
                            {
                                employee.Remarks = String.Empty;
                            }
                            else
                            {
                                employee.Remarks = myReader.GetString(12);
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
            return employee;
        }

        public GenericList<Employee> GetAllEmployee()
        {
            GenericList<Employee> allEmployee = new GenericList<Employee>();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeSelect";
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
                                Employee employee = Employee.CreateEmployee();
                                employee.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    employee.EmployeeNo = String.Empty;
                                }
                                else
                                {
                                    employee.EmployeeNo = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    employee.LastName = String.Empty;
                                }
                                else
                                {
                                    employee.LastName = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    employee.FirstName = String.Empty;
                                }
                                else
                                {
                                    employee.FirstName = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    employee.MiddleName = String.Empty;
                                }
                                else
                                {
                                    employee.MiddleName = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    employee.Address = String.Empty;
                                }
                                else
                                {
                                    employee.Address = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    employee.Phone = String.Empty;
                                }
                                else
                                {
                                    employee.Phone = myReader.GetString(6);
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    employee.BirthDate = DateTime.MinValue;
                                }
                                else
                                {
                                    employee.BirthDate = myReader.GetDateTime(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    employee.UserName = String.Empty;
                                }
                                else
                                {
                                    employee.UserName = myReader.GetString(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    employee.Password = String.Empty;
                                }
                                else
                                {
                                    employee.Password = myReader.GetString(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    employee.Salt = String.Empty;
                                }
                                else
                                {
                                    employee.Salt = myReader.GetString(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    employee.PictureFile = String.Empty;
                                }
                                else
                                {
                                    employee.PictureFile = myReader.GetString(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    employee.Remarks = String.Empty;
                                }
                                else
                                {
                                    employee.Remarks = myReader.GetString(12);
                                }
                                allEmployee.Add(employee);
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
            return allEmployee;
        }

        public GenericList<Employee> GetAllEmployee(Int32 pageNo, SortByEmployee sortBy, SortingOrder sortOrder)
        {
            GenericList<Employee> allEmployee = new GenericList<Employee>();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeSelect";
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
                                Employee employee = Employee.CreateEmployee();
                                employee.ID = myReader.GetInt32(0);
                                if (myReader.IsDBNull(1))
                                {
                                    employee.EmployeeNo = String.Empty;
                                }
                                else
                                {
                                    employee.EmployeeNo = myReader.GetString(1);
                                }
                                if (myReader.IsDBNull(2))
                                {
                                    employee.LastName = String.Empty;
                                }
                                else
                                {
                                    employee.LastName = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    employee.FirstName = String.Empty;
                                }
                                else
                                {
                                    employee.FirstName = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    employee.MiddleName = String.Empty;
                                }
                                else
                                {
                                    employee.MiddleName = myReader.GetString(4);
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    employee.Address = String.Empty;
                                }
                                else
                                {
                                    employee.Address = myReader.GetString(5);
                                }
                                if (myReader.IsDBNull(6))
                                {
                                    employee.Phone = String.Empty;
                                }
                                else
                                {
                                    employee.Phone = myReader.GetString(6);
                                }
                                if (myReader.IsDBNull(7))
                                {
                                    employee.BirthDate = DateTime.MinValue;
                                }
                                else
                                {
                                    employee.BirthDate = myReader.GetDateTime(7);
                                }
                                if (myReader.IsDBNull(8))
                                {
                                    employee.UserName = String.Empty;
                                }
                                else
                                {
                                    employee.UserName = myReader.GetString(8);
                                }
                                if (myReader.IsDBNull(9))
                                {
                                    employee.Password = String.Empty;
                                }
                                else
                                {
                                    employee.Password = myReader.GetString(9);
                                }
                                if (myReader.IsDBNull(10))
                                {
                                    employee.Salt = String.Empty;
                                }
                                else
                                {
                                    employee.Salt = myReader.GetString(10);
                                }
                                if (myReader.IsDBNull(11))
                                {
                                    employee.PictureFile = String.Empty;
                                }
                                else
                                {
                                    employee.PictureFile = myReader.GetString(11);
                                }
                                if (myReader.IsDBNull(12))
                                {
                                    employee.Remarks = String.Empty;
                                }
                                else
                                {
                                    employee.Remarks = myReader.GetString(12);
                                }
                                allEmployee.Add(employee);
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
            return allEmployee;
        }

        public Employee InsertEmployee(Employee employee)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@EmployeeNo", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(employee.EmployeeNo))
            {
                myParam1.Value = DBNull.Value;
            }
            else
            {
                myParam1.Value = employee.EmployeeNo;
            }
            SqlParameter myParam2 = new SqlParameter("@LastName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(employee.LastName))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = employee.LastName;
            }
            SqlParameter myParam3 = new SqlParameter("@FirstName", SqlDbType.VarChar, 35);
            if (String.IsNullOrEmpty(employee.FirstName))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = employee.FirstName;
            }
            SqlParameter myParam4 = new SqlParameter("@MiddleName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(employee.MiddleName))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = employee.MiddleName;
            }
            SqlParameter myParam5 = new SqlParameter("@Address", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(employee.Address))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = employee.Address;
            }
            SqlParameter myParam6 = new SqlParameter("@Phone", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(employee.Phone))
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = employee.Phone;
            }
            SqlParameter myParam7 = new SqlParameter("@BirthDate", SqlDbType.SmallDateTime);
            if (employee.BirthDate == DateTime.MinValue)
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = employee.BirthDate;
            }
            SqlParameter myParam8 = new SqlParameter("@UserName", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(employee.UserName))
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = employee.UserName;
            }
            SqlParameter myParam9 = new SqlParameter("@Password", SqlDbType.VarChar, 64);
            if (String.IsNullOrEmpty(employee.Password))
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = employee.Password;
            }
            SqlParameter myParam10 = new SqlParameter("@Salt", SqlDbType.VarChar, 64);
            if (String.IsNullOrEmpty(employee.Salt))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = employee.Salt;
            }
            SqlParameter myParam11 = new SqlParameter("@PictureFile", SqlDbType.VarChar, 128);
            if (String.IsNullOrEmpty(employee.PictureFile))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = employee.PictureFile;
            }
            SqlParameter myParam12 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(employee.Remarks))
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = employee.Remarks;
            }
            SqlParameter myParam13 = new SqlParameter("@Output", SqlDbType.Int);
            myParam11.Direction = ParameterDirection.Output;
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
                    employee.ID = Convert.ToInt32(myParam13.Value);
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
            return employee;
        }

        public Boolean UpdateEmployee(Employee employee)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = employee.ID;
            SqlParameter myParam2 = new SqlParameter("@EmployeeNo", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(employee.EmployeeNo))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = employee.EmployeeNo;
            }
            SqlParameter myParam3 = new SqlParameter("@LastName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(employee.LastName))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = employee.LastName;
            }
            SqlParameter myParam4 = new SqlParameter("@FirstName", SqlDbType.VarChar, 35);
            if (String.IsNullOrEmpty(employee.FirstName))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = employee.FirstName;
            }
            SqlParameter myParam5 = new SqlParameter("@MiddleName", SqlDbType.VarChar, 25);
            if (String.IsNullOrEmpty(employee.MiddleName))
            {
                myParam5.Value = DBNull.Value;
            }
            else
            {
                myParam5.Value = employee.MiddleName;
            }
            SqlParameter myParam6 = new SqlParameter("@Address", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(employee.Address))
            {
                myParam6.Value = DBNull.Value;
            }
            else
            {
                myParam6.Value = employee.Address;
            }
            SqlParameter myParam7 = new SqlParameter("@Phone", SqlDbType.VarChar, 16);
            if (String.IsNullOrEmpty(employee.Phone))
            {
                myParam7.Value = DBNull.Value;
            }
            else
            {
                myParam7.Value = employee.Phone;
            }
            SqlParameter myParam8 = new SqlParameter("@BirthDate", SqlDbType.SmallDateTime);
            if (employee.BirthDate == DateTime.MinValue)
            {
                myParam8.Value = DBNull.Value;
            }
            else
            {
                myParam8.Value = employee.BirthDate;
            }
            SqlParameter myParam9 = new SqlParameter("@UserName", SqlDbType.VarChar, 32);
            if (String.IsNullOrEmpty(employee.UserName))
            {
                myParam9.Value = DBNull.Value;
            }
            else
            {
                myParam9.Value = employee.UserName;
            }
            SqlParameter myParam10 = new SqlParameter("@Password", SqlDbType.VarChar, 64);
            if (String.IsNullOrEmpty(employee.Password))
            {
                myParam10.Value = DBNull.Value;
            }
            else
            {
                myParam10.Value = employee.Password;
            }
            SqlParameter myParam11 = new SqlParameter("@Salt", SqlDbType.VarChar, 64);
            if (String.IsNullOrEmpty(employee.Salt))
            {
                myParam11.Value = DBNull.Value;
            }
            else
            {
                myParam11.Value = employee.Salt;
            }
            SqlParameter myParam12 = new SqlParameter("@PictureFile", SqlDbType.VarChar, 128);
            if (String.IsNullOrEmpty(employee.PictureFile))
            {
                myParam12.Value = DBNull.Value;
            }
            else
            {
                myParam12.Value = employee.PictureFile;
            }
            SqlParameter myParam13 = new SqlParameter("@Remarks", SqlDbType.VarChar, 1024);
            if (String.IsNullOrEmpty(employee.Remarks))
            {
                myParam13.Value = DBNull.Value;
            }
            else
            {
                myParam13.Value = employee.Remarks;
            }
            SqlParameter myParam14 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam14.Direction = ParameterDirection.ReturnValue;
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
            if (Convert.ToInt32(myParam14.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteEmployee(Employee employee)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "EmployeeDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = employee.ID;
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
