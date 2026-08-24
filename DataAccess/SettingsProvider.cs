using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Security;
using Interact.BusinessLogic;
using Interact.Common;

namespace Interact.DataAccess
{
    public class SettingsProvider :ISettingsProvider
    {
        #region Fields
        private SqlConnection _conn;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SettingsProvider(SqlConnection conn)
        {
            this._conn = conn;
        }
        #endregion

        #region Methods
        public Settings GetSettings(Settings settings)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SettingsSelect";
            myCommand.Connection = this._conn;
            SqlParameter myParam1 = new SqlParameter("@BranchKey", SqlDbType.Int);
            myParam1.Value = (Int32)settings.Branch;
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
                            if (myReader.IsDBNull(0))
                            {
                                settings.Salt = String.Empty;
                            }
                            else
                            {
                                settings.Salt = myReader.GetString(0);
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
            String branchName = settings.Branch.ToString() + "InteractTech10";
            String saltedBranchName = FormsAuthentication.HashPasswordForStoringInConfigFile(String.Concat(branchName, settings.Salt), "sha1");
            Int32 result = String.Compare(settings.Code, saltedBranchName, false);
            if (result == 0)
            {
                settings.IsAuthorized = true;
            }
            return settings;
        }
        #endregion
    }
}
