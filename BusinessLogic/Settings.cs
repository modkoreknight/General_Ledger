using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Interact.BusinessLogic
{
    public class Settings
    {
        #region Fields
        private Branch _branch;
        private Boolean _isMain;
        private String _code;
        private String _salt;
        private String _applicationKey;
        private Boolean _isAuthorized;
        private SqlConnection _connection;
        #endregion

        #region Properties
        [Description("Branch")]
        public Branch Branch
        {
            get
            {
                return this._branch;
            }
            set
            {
                if (this._branch != value)
                {
                    this._branch = value;
                }
            }
        }

        [Description("IsMain")]
        public Boolean IsMain
        {
            get
            {
                return this._isMain;
            }
            set
            {
                if (this._isMain != value)
                {
                    this._isMain = value;
                }
            }
        }

        [Description("Code")]
        public String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                if (this._code != value)
                {
                    this._code = value;
                }
            }
        }

        [Description("Salt")]
        public String Salt
        {
            get
            {
                return this._salt;
            }
            set
            {
                if (this._salt != value)
                {
                    this._salt = value;
                }
            }
        }

        [Description("ApplicationKey")]
        public String ApplicationKey
        {
            get
            {
                return this._applicationKey;
            }
            set
            {
                if (this._applicationKey != value)
                {
                    this._applicationKey = value;
                }
            }
        }

        [Description("IsAuthorized")]
        public Boolean IsAuthorized
        {
            get
            {
                return this._isAuthorized;
            }
            set
            {
                if (this._isAuthorized != value)
                {
                    this._isAuthorized = value;
                }
            }
        }

        [Description("Connection")]
        public SqlConnection Connection
        {
            get
            {
                return this._connection;
            }
            set
            {
                if (this._connection != value)
                {
                    this._connection = value;
                }
            }
        }
        #endregion

        #region Constructors
        private Settings()
        {
        }

        public static Settings CreateSettings(String applicationKey)
        {
            Settings settings = new Settings();
            //String applicationKey = ConfigurationManager.AppSettings["SerialNo"].ToString();
            //String applicationKey = Properties.Settings.Default.ApplicationKey;
            try
            {
                settings.ApplicationKey = applicationKey;
                settings.Branch = (Branch)Enum.Parse(typeof(Branch), applicationKey.Substring(10, 3));
                if (applicationKey.Substring(23, 1) == "X")
                {
                    settings.IsMain = true;
                }
                else
                {
                    settings.IsMain = false;
                }
                settings.Code = applicationKey.Remove(10, 3).Remove(20, 1);

                String machineCode = ServerInfo.GetMachineCode();
                String salt = settings.Branch.ToString() + "1n+3r@c+" + machineCode.Substring((machineCode.Length / 2) - 4, 8);
                String storedMachineCode = FormsAuthentication.HashPasswordForStoringInConfigFile(String.Concat(machineCode, salt), "sha1");
                Int32 result = String.Compare(settings.Code, storedMachineCode, false);
                if (result == 0)
                {
                    settings.IsAuthorized = true;
                }
                else
                {
                    settings.IsAuthorized = false;
                }
            }
            catch
            {
                settings.IsAuthorized = false;
            }
            return settings;
        }
        #endregion

        #region Methods
        #endregion

        #region Overrides
        public override String ToString()
        {
            return this._branch.ToString();
        }
        #endregion
    }
}
