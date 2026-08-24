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
    public class ZoneProvider : IZoneProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ZoneProvider(SqlConnection conn)
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
        }
        #endregion

        #region Methods
        public Int32 GetZonePageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGetPageCount";
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

        public Zone GetZone(Int32 id)
        {
            Zone zone = Zone.CreateZone();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneSelect";
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
                            zone.ID = myReader.GetInt32(0);
                            zone.Name = myReader.GetString(1);
                            if (myReader.IsDBNull(2))
                            {
                                zone.Abbreviation = String.Empty;
                            }
                            else
                            {
                                zone.Abbreviation = myReader.GetString(2);
                            }
                            if (myReader.IsDBNull(3))
                            {
                                zone.Description = String.Empty;
                            }
                            else
                            {
                                zone.Description = myReader.GetString(3);
                            }
                            if (myReader.FieldCount > 4)
                            {
                                if (myReader.IsDBNull(4))
                                {
                                    zone.Branch = 0;
                                }
                                else
                                {
                                    zone.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(4).ToString());
                                }
                            }
                            if (myReader.FieldCount > 5)
                            {
                                if (myReader.IsDBNull(5))
                                {
                                    zone.AuditID = 0;
                                }
                                else
                                {
                                    zone.AuditID = myReader.GetInt32(5);
                                }
                            }
                            zone.Branch = this._branch;
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
            return zone;
        }

        public GenericList<Zone> GetAllZone()
        {
            GenericList<Zone> allZone = new GenericList<Zone>();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneSelect";
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
                                Zone zone = Zone.CreateZone();
                                zone.ID = myReader.GetInt32(0);
                                zone.Name = myReader.GetString(1);
                                if (myReader.IsDBNull(2))
                                {
                                    zone.Abbreviation = String.Empty;
                                }
                                else
                                {
                                    zone.Abbreviation = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    zone.Description = String.Empty;
                                }
                                else
                                {
                                    zone.Description = myReader.GetString(3);
                                }
                                if (myReader.FieldCount > 4)
                                {
                                    if (myReader.IsDBNull(4))
                                    {
                                        zone.Branch = this._branch;
                                    }
                                    else
                                    {
                                        zone.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(4).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 5)
                                {
                                    if (myReader.IsDBNull(5))
                                    {
                                        zone.AuditID = 0;
                                    }
                                    else
                                    {
                                        zone.AuditID = myReader.GetInt32(5);
                                    }
                                }
                                allZone.Add(zone);
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
            return allZone;
        }

        public GenericList<Zone> GetAllZone(Int32 pageNo, SortByZone sortBy, SortingOrder sortOrder)
        {
            GenericList<Zone> allZone = new GenericList<Zone>();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneSelect";
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
                                Zone zone = Zone.CreateZone();
                                zone.ID = myReader.GetInt32(0);
                                zone.Name = myReader.GetString(1);
                                if (myReader.IsDBNull(2))
                                {
                                    zone.Abbreviation = String.Empty;
                                }
                                else
                                {
                                    zone.Abbreviation = myReader.GetString(2);
                                }
                                if (myReader.IsDBNull(3))
                                {
                                    zone.Description = String.Empty;
                                }
                                else
                                {
                                    zone.Description = myReader.GetString(3);
                                }
                                if (myReader.IsDBNull(4))
                                {
                                    zone.Branch = 0;
                                }
                                else
                                {
                                    zone.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(4).ToString());
                                }
                                if (myReader.IsDBNull(5))
                                {
                                    zone.AuditID = 0;
                                }
                                else
                                {
                                    zone.AuditID = myReader.GetInt32(5);
                                }
                                zone.Branch = this._branch;
                                allZone.Add(zone);
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
            return allZone;
        }

        public Zone InsertZone(Zone zone)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@Name", SqlDbType.VarChar, 50);
            myParam1.Value = zone.Name;
            SqlParameter myParam2 = new SqlParameter("@Abbreviation", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(zone.Abbreviation))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = zone.Abbreviation;
            }
            SqlParameter myParam3 = new SqlParameter("@Description", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(zone.Description))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = zone.Description;
            }
            SqlParameter myParam4 = new SqlParameter("@BranchID", SqlDbType.Int);
            myParam4.Value = (Int32)zone.Branch;
            SqlParameter myParam5 = new SqlParameter("@AuditID", SqlDbType.Int);
            myParam5.Value = zone.AuditID;
            SqlParameter myParam6 = new SqlParameter("@Output", SqlDbType.Int);
            myParam6.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
            myCommand.Parameters.Add(myParam6);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    zone.ID = Convert.ToInt32(myParam6.Value);
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
            return zone;
        }

        public Boolean UpdateZone(Zone zone)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = zone.ID;
            SqlParameter myParam2 = new SqlParameter("@Name", SqlDbType.VarChar, 50);
            myParam2.Value = zone.Name;
            SqlParameter myParam3 = new SqlParameter("@Abbreviation", SqlDbType.VarChar, 10);
            if (String.IsNullOrEmpty(zone.Abbreviation))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = zone.Abbreviation;
            }
            SqlParameter myParam4 = new SqlParameter("@Description", SqlDbType.VarChar, 100);
            if (String.IsNullOrEmpty(zone.Description))
            {
                myParam4.Value = DBNull.Value;
            }
            else
            {
                myParam4.Value = zone.Description;
            }
            SqlParameter myParam5 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam5.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
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
            if (Convert.ToInt32(myParam5.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteZone(Zone zone)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = zone.ID;
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
