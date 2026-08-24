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
    public class ZoneGroupProvider : IZoneGroupProvider
    {
        #region Fields
        private SqlConnection _conn;
        private Branch _branch;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ZoneGroupProvider(SqlConnection conn)
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
        public Int32 GetZoneGroupPageCount()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupGetPageCount";
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

        public ZoneGroup GetZoneGroup(Int32 id)
        {
            ZoneGroup zoneGroup = ZoneGroup.CreateZoneGroup();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupSelect";
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
                            zoneGroup.ID = myReader.GetInt32(0);
                            zoneGroup.Name = myReader.GetString(1);
                            if (myReader.IsDBNull(2))
                            {
                                zoneGroup.Zones = String.Empty;
                            }
                            else
                            {
                                zoneGroup.Zones = myReader.GetString(2);
                            }
                            if (myReader.FieldCount > 3)
                            {
                                if (myReader.IsDBNull(3))
                                {
                                    zoneGroup.Branch = this._branch;
                                }
                                else
                                {
                                    zoneGroup.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(3).ToString());
                                }
                            }
                            if (myReader.FieldCount > 4)
                            {
                                if (myReader.IsDBNull(4))
                                {
                                    zoneGroup.AuditID = 0;
                                }
                                else
                                {
                                    zoneGroup.AuditID = myReader.GetInt32(4);
                                }
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
            return zoneGroup;
        }

        public GenericList<ZoneGroup> GetAllZoneGroup()
        {
            GenericList<ZoneGroup> allZoneGroup = new GenericList<ZoneGroup>();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupSelect";
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
                                ZoneGroup zoneGroup = ZoneGroup.CreateZoneGroup();
                                zoneGroup.ID = myReader.GetInt32(0);
                                zoneGroup.Name = myReader.GetString(1);
                                if (myReader.IsDBNull(2))
                                {
                                    zoneGroup.Zones = String.Empty;
                                }
                                else
                                {
                                    zoneGroup.Zones = myReader.GetString(2);
                                }
                                if (myReader.FieldCount > 3)
                                {
                                    if (myReader.IsDBNull(3))
                                    {
                                        zoneGroup.Branch = this._branch;
                                    }
                                    else
                                    {
                                        zoneGroup.Branch = (Branch)Enum.Parse(typeof(Branch), myReader.GetInt32(3).ToString());
                                    }
                                }
                                if (myReader.FieldCount > 4)
                                {
                                    if (myReader.IsDBNull(4))
                                    {
                                        zoneGroup.AuditID = 0;
                                    }
                                    else
                                    {
                                        zoneGroup.AuditID = myReader.GetInt32(4);
                                    }
                                }
                                allZoneGroup.Add(zoneGroup);
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
                //throw ex;
                return allZoneGroup;
            }
            return allZoneGroup;
        }

        public GenericList<ZoneGroup> GetAllZoneGroup(Int32 pageNo, SortByZoneGroup sortBy, SortingOrder sortOrder)
        {
            GenericList<ZoneGroup> allZoneGroup = new GenericList<ZoneGroup>();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupSelect";
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
                                ZoneGroup zoneGroup = ZoneGroup.CreateZoneGroup();
                                zoneGroup.ID = myReader.GetInt32(0);
                                zoneGroup.Name = myReader.GetString(1);
                                if (myReader.IsDBNull(2))
                                {
                                    zoneGroup.Zones = String.Empty;
                                }
                                else
                                {
                                    zoneGroup.Zones = myReader.GetString(2);
                                }
                                allZoneGroup.Add(zoneGroup);
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
            return allZoneGroup;
        }

        public ZoneGroup InsertZoneGroup(ZoneGroup zoneGroup)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupInsert";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@Name", SqlDbType.VarChar, 64);
            myParam1.Value = zoneGroup.Name;
            SqlParameter myParam2 = new SqlParameter("@Zones", SqlDbType.VarChar, 512);
            if (String.IsNullOrEmpty(zoneGroup.Zones))
            {
                myParam2.Value = DBNull.Value;
            }
            else
            {
                myParam2.Value = zoneGroup.Zones;
            }
            SqlParameter myParam3 = new SqlParameter("@BranchID", SqlDbType.Int);
            myParam3.Value = (Int32)zoneGroup.Branch;
            SqlParameter myParam4 = new SqlParameter("@AuditID", SqlDbType.Int);
            myParam4.Value = zoneGroup.AuditID;
            SqlParameter myParam5 = new SqlParameter("@Output", SqlDbType.Int);
            myParam5.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
            myCommand.Parameters.Add(myParam5);
            try
            {
                this._conn.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                    zoneGroup.ID = Convert.ToInt32(myParam5.Value);
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
            return zoneGroup;
        }

        public Boolean UpdateZoneGroup(ZoneGroup zoneGroup)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupUpdate";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = zoneGroup.ID;
            SqlParameter myParam2 = new SqlParameter("@Name", SqlDbType.VarChar, 64);
            myParam2.Value = zoneGroup.Name;
            SqlParameter myParam3 = new SqlParameter("@Zones", SqlDbType.VarChar, 512);
            if (String.IsNullOrEmpty(zoneGroup.Zones))
            {
                myParam3.Value = DBNull.Value;
            }
            else
            {
                myParam3.Value = zoneGroup.Zones;
            }
            SqlParameter myParam4 = new SqlParameter("@ReturnValue", SqlDbType.Int);
            myParam4.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(myParam1);
            myCommand.Parameters.Add(myParam2);
            myCommand.Parameters.Add(myParam3);
            myCommand.Parameters.Add(myParam4);
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
            if (Convert.ToInt32(myParam4.Value) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteZoneGroup(ZoneGroup zoneGroup)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "ZoneGroupDelete";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@ID", SqlDbType.Int);
            myParam1.Value = zoneGroup.ID;
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
