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
    public class DatabaseProvider : IDatabaseProvider
    {
        #region Fields
        private SqlConnection _conn;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public DatabaseProvider(SqlConnection conn)
        {
            this._conn = conn;
        }
        #endregion

        #region Methods
        public Boolean BackupDatabase(String fileName)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "BackupDatabase";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@FileName", SqlDbType.VarChar, 512);
            myParam1.Value = fileName;
            myCommand.Parameters.Add(myParam1);
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
            return true;
        }
        #endregion
    }
}
