using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using Interact.BusinessLogic;

namespace Interact.Common
{
    public static class Database
    {
        #region Fields
        private static SecureString _secureString = new SecureString();
        #endregion

        #region Properties
        public static SqlConnection GeneralLedger
        {
            get
            {
                return Connection("GeneralLedger");
            }
        }
        #endregion

        #region Constructors
        static Database()
        {
            Database._secureString.AppendChar('!');
            Database._secureString.AppendChar('@');
            Database._secureString.AppendChar('#');
            Database._secureString.AppendChar('$');
            Database._secureString.AppendChar('%');
            Database._secureString.AppendChar('^');
            Database._secureString.AppendChar('&');
            Database._secureString.AppendChar('*');
            Database._secureString.AppendChar('(');
            Database._secureString.AppendChar(')');
            Database._secureString.MakeReadOnly();
        }
        #endregion

        #region Methods
        public static SqlConnection Connection(String key)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[key].ConnectionString);
        }

        public static SqlConnection BranchConnection(Branch branch)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + Database.GeneralLedger.DataSource + "; Initial Catalog=LSMJ_" + ((Int32)branch).ToString("D3") + "; User Id=LSMJ_Admin; Password=123456;");
            Server server = new Server(new ServerConnection(Database.GeneralLedger.DataSource, "LSMJ_Admin", "123456"));
            if (server.Databases.Contains("AccountLedger_" + ((Int32)branch).ToString("D3")))
            {
                return conn;
            }
            else
            {
                return null;
            }
        }

        public static SqlConnection AuditConnection()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + Database.GeneralLedger.DataSource + "; Initial Catalog=LSMJ_Audit; User Id=LSMJ_Admin; Password=123456;");
            Server server = new Server(new ServerConnection(Database.GeneralLedger.DataSource, "LSMJ_Admin", "123456"));
            if (server.Databases.Contains("GeneralLedger_Audit"))
            {
                return conn;
            }
            else
            {
                return null;
            }
        }

        public static List<SqlConnection> AllConnection()
        {
            List<SqlConnection> allConn = new List<SqlConnection>();
            Server server = new Server(new ServerConnection(Database.GeneralLedger.DataSource, "LSMJ_Admin", "123456"));
            foreach (Microsoft.SqlServer.Management.Smo.Database database in server.Databases)
            {
                if (database.Name.Contains("AccountLedger_"))
                {
                    String[] strArray = database.Name.Split('_');
                    Branch branch;
                    try
                    {
                        branch = (Branch)Enum.Parse(typeof(Branch), strArray[1]);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    allConn.Add(BranchConnection(branch));
                }
            }
            return allConn;
        }

        public static void CreateDatabaseAudit()
        {
            //Connect to server using default database
            Server server = new Server(new ServerConnection(Database.GeneralLedger.DataSource, "LSMJ_Admin", "123456"));

            //Close connections to Audit database
            server.KillAllProcesses("LSMJ_Audit");

            //Rename existing Audit database
            String suffix = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
            if (server.Databases.Contains("LSMJ_Audit"))
            {
                server.Databases["LSMJ_Audit"].Rename("LSMJ_Audit_" + suffix);
            }

            //Create new database
            Microsoft.SqlServer.Management.Smo.Database database = new Microsoft.SqlServer.Management.Smo.Database(server ,"LSMJ_Audit");
            FileGroup fileGroup = new FileGroup(database, "PRIMARY");
            //SQL 2005
            //DataFile dataFile = new DataFile(fileGroup, "GeneralLedger_Audit_" + suffix + "_dat", "C:\\Program Files\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".mdf");
            //SQL 2008
            // DataFile dataFile = new DataFile(fileGroup, "GeneralLedger_Audit_" + suffix + "_dat", "C:\\Program Files\\Microsoft SQL Server\\MSSQL10.MSSQLSERVER\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".mdf");
            //SQL 2008 R2
            DataFile dataFile = new DataFile(fileGroup, "LSMJ_Audit_" + suffix + "_dat", "C:\\Program Files\\Microsoft SQL Server\\MSSQL10_50.SQL2008R2\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".mdf");
            //SQL 2008 R2 for Audit
            //DataFile dataFile = new DataFile(fileGroup, "GeneralLedger_Audit_" + suffix + "_dat", "F:\\Program Files\\Microsoft SQL Server\\MSSQL10_50.SQL2008R2\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".mdf");            
            dataFile.Size = 10000;  //1000000
            dataFile.GrowthType = FileGrowthType.KB;
            dataFile.Growth = 1000;  //100000
            fileGroup.Files.Add(dataFile);
            database.FileGroups.Add(fileGroup);
            //SQL 2005
            //LogFile logFile = new LogFile(database, "GeneralLedger_Audit_" + suffix + "_log", "C:\\Program Files\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".ldf");
            //SQL 2008
            //LogFile logFile = new LogFile(database, "GeneralLedger_Audit_" + suffix + "_log", "C:\\Program Files\\Microsoft SQL Server\\MSSQL10.MSSQLSERVER\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".ldf");
            //SQL 2008 R2
            LogFile logFile = new LogFile(database, "LSMJ_Audit_" + suffix + "_log", "C:\\Program Files\\Microsoft SQL Server\\MSSQL10_50.SQL2008R2\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".ldf");
            //SQL 2008 R2
            //LogFile logFile = new LogFile(database, "GeneralLedger_Audit_" + suffix + "_log", "G:\\Program Files\\Microsoft SQL Server\\MSSQL10_50.SQL2008R2\\MSSQL\\Data\\GeneralLedger_Audit_" + suffix + ".ldf");
            logFile.Size = 1000;  //100000
            logFile.GrowthType = FileGrowthType.KB;
            logFile.Growth = 500;  //10000
            database.LogFiles.Add(logFile);
            database.Create();

            //Create database objects
            //StreamReader sr = new StreamReader("C:\\Interact Technologies\\Projects\\GeneralLedger\\SqlScript\\Tables\\Zone.sql");
            StreamReader sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Tables\\Zone.sql");
            String script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery("ALTER TABLE dbo.Zone DROP CONSTRAINT UK_Zone;");

            //sr = new StreamReader("C:\\Interact Technologies\\Projects\\GeneralLedger\\SqlScript\\Tables\\ZoneGroup.sql");
            sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Tables\\ZoneGroup.sql");
            script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery("ALTER TABLE dbo.ZoneGroup DROP CONSTRAINT UK_ZoneGroup;");

            //sr = new StreamReader("C:\\Interact Technologies\\Projects\\GeneralLedger\\SqlScript\\Tables\\Customer.sql");
            sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Tables\\Customer.sql");
            script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery("ALTER TABLE dbo.Customer DROP CONSTRAINT UK_Customer;");

            //sr = new StreamReader("C:\\Interact Technologies\\Projects\\GeneralLedger\\SqlScript\\Tables\\Vehicle.sql");
            sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Tables\\Vehicle.sql");
            script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery("ALTER TABLE dbo.Vehicle DROP CONSTRAINT UK_Vehicle;");

            //sr = new StreamReader("C:\\Interact Technologies\\Projects\\GeneralLedger\\SqlScript\\Tables\\Sales.sql");
            sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Tables\\Sales.sql");
            script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
            //server.Databases["GeneralLedger_Audit"].ExecuteNonQuery("ALTER TABLE dbo.Sales DROP CONSTRAINT UK_Sales;");

            //sr = new StreamReader("C:\\Interact Technologies\\Projects\\GeneralLedger\\SqlScript\\Tables\\Payment.sql");
            sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Tables\\Payment.sql");
            script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
            //server.Databases["GeneralLedger_Audit"].ExecuteNonQuery("ALTER TABLE dbo.Payment DROP CONSTRAINT UK_Payment;");

            sr = new StreamReader("C:\\Temp\\GeneralLedger\\SqlScript\\Functions.sql");
            script = sr.ReadToEnd();
            server.Databases["GeneralLedger_Audit"].ExecuteNonQuery(script);
        }

        public static Boolean Backup(String databaseName, String fileName)
        {
            Boolean result = false;

            ServerConnection connection = new ServerConnection(Database.GeneralLedger);
            //connection.LoginSecure = true;
            //connection.Connect();
            try
            {
                Server server = new Server(connection);
                Backup backup = new Backup();
                backup.Action = BackupActionType.Database;
                //backup.Database = "GeneralLedger_" + ((Int32)branch).ToString("D3");
                backup.Database = databaseName;
                //destinationPath = System.IO.Path.Combine(destinationPath, databaseName + ".bak");
                backup.Devices.Add(new BackupDeviceItem(fileName, DeviceType.File));
                backup.Initialize = true;
                backup.Checksum = true;
                backup.ContinueAfterError = true;
                backup.Incremental = false;
                backup.LogTruncation = BackupTruncateLogType.Truncate;
                //backup.SetPassword("!@#$%^&*()");
                //backup.SetPassword(Database._secureString);
                //backup.PercentComplete += new PercentCompleteEventHandler(backup_PercentComplete);
                //backup.Complete += new Microsoft.SqlServer.Management.Common.ServerMessageEventHandler(backup_Complete);
                //Perform backup.
                backup.SqlBackup(server);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.IsOpen)
                    {
                        connection.Disconnect();
                    }
                    connection = null;
                }
            }

            return result;
        }

        public static Boolean Restore(String fileName, String databaseName, String dataFile, String logFile)
        {
            Boolean result = false;

            ServerConnection connection = new ServerConnection(Database.GeneralLedger);
            //connection.LoginSecure = true;
            //connection.Connect();
            try
            {
                Server server = new Server(connection);
                Restore restore = new Restore();
                restore.Database = databaseName;
                Microsoft.SqlServer.Management.Smo.Database currentDb = server.Databases[databaseName];
                if (currentDb != null)
                {
                    server.KillAllProcesses(databaseName);
                }
                restore.Devices.AddDevice(fileName, DeviceType.File);
                
                //restore.SetPassword("!@#$%^&*()");
                restore.SetPassword(Database._secureString);

                DataTable files = restore.ReadFileList(server);
                String oldDataFile = String.Empty;
                String oldLogFile = String.Empty;
                if (files != null)
                {
                    if (files.Rows[0][2].ToString() == "D")
                    {
                        oldDataFile = files.Rows[0][0].ToString();
                        oldLogFile = files.Rows[1][0].ToString();
                    }
                    else
                    {
                        oldDataFile = files.Rows[1][0].ToString();
                        oldLogFile = files.Rows[0][0].ToString();
                    }
                }
                restore.RelocateFiles.Add(new RelocateFile(oldDataFile, dataFile));
                restore.RelocateFiles.Add(new RelocateFile(oldLogFile, logFile));
                restore.ReplaceDatabase = true;
                
                //IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secString);
                //string sDecrypString = System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);
                //secString.Dispose();
                
                //restore.PercentCompleteNotification = 10;
                //restore.PercentComplete += new PercentCompleteEventHandler(restore_PercentComplete);
                //restore.Complete += new ServerMessageEventHandler(restore_Complete);
                restore.SqlRestore(server);
                currentDb = server.Databases[databaseName];
                currentDb.SetOnline();
                result = true;
            }
            catch (Exception ex)
            {
                //result = false;
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.IsOpen)
                    {
                        connection.Disconnect();
                    }
                    connection = null;
                }
            }

            return result;
        }

        public static Boolean InstallZoneGroupPatch()
        {
            Boolean result = false;

            String sqlCommand = @"USE {0};
                GO

                CREATE TABLE dbo.ZoneGroup (
	                ID INT NOT NULL IDENTITY(1, 1),
	                [Name] VARCHAR(64) NOT NULL,
	                Zones VARCHAR(512) NULL,
	                CreateDate SMALLDATETIME NOT NULL,
	                CreateUser VARCHAR(32) NOT NULL,
	                UpdateDate SMALLDATETIME NULL,
	                UpdateUser VARCHAR(32) NULL);
                GO

                ---------------------------------------------------------------------------------------------------

                ALTER TABLE dbo.ZoneGroup
                ADD
	                CONSTRAINT PK_ZoneGroup PRIMARY KEY (ID),
	                CONSTRAINT UK_ZoneGroup UNIQUE ([Name]),
	                CONSTRAINT DF_ZoneGroup_CreateDate DEFAULT GETDATE() FOR CreateDate,
	                CONSTRAINT DF_ZoneGroup_CreateUser DEFAULT SYSTEM_USER FOR CreateUser;
                GO

                ---------------------------------------------------------------------------------------------------

                IF EXISTS (SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID('dbo.ZoneGroupGetPageCount') AND type in ('P', 'PC'))
                BEGIN
	                DROP PROCEDURE dbo.ZoneGroupGetPageCount;
                END
                GO

                CREATE PROCEDURE dbo.ZoneGroupGetPageCount
	                (@PageSize TINYINT,
	                 @Output TINYINT OUTPUT)
                WITH ENCRYPTION
                AS
                BEGIN
	                SET NOCOUNT ON;
	                DECLARE @PageCount DECIMAL(5, 2);
	                DECLARE @PageSizeDecimal DECIMAL(5, 2);
	                SET @PageSizeDecimal = CAST(@PageSize AS DECIMAL(5, 2));
	                SELECT
		                @PageCount = COUNT(*) / @PageSizeDecimal
	                FROM
		                ZoneGroup WITH (NOLOCK);
	                SET @Output = CEILING(@PageCount);
	                SET NOCOUNT OFF;
                END
                GO

                ---------------------------------------------------------------------------------------------------

                IF EXISTS (SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID('dbo.ZoneGroupSelect') AND type in ('P', 'PC'))
                BEGIN
	                DROP PROCEDURE dbo.ZoneGroupSelect;
                END
                GO

                CREATE PROCEDURE dbo.ZoneGroupSelect (
	                @ID INT = NULL)
                WITH ENCRYPTION
                AS
                BEGIN
	                SET NOCOUNT ON;
	                IF @ID IS NULL
	                BEGIN
		                SELECT
			                ID,
			                [Name],
			                Zones
		                FROM
			                dbo.ZoneGroup WITH (NOLOCK);
	                END
	                ELSE
	                BEGIN
		                SELECT
			                ID,
			                [Name],
			                Zones
		                FROM
			                dbo.ZoneGroup WITH (NOLOCK)
		                WHERE
			                ID = @ID;
	                END
	                SET NOCOUNT OFF;
                END
                GO

                ---------------------------------------------------------------------------------------------------

                IF EXISTS (SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID('dbo.ZoneGroupInsert') AND type in ('P', 'PC'))
                BEGIN
	                DROP PROCEDURE dbo.ZoneGroupInsert;
                END
                GO

                CREATE PROCEDURE dbo.ZoneGroupInsert (
	                @Name VARCHAR(64),
	                @Zones VARCHAR(512),
	                @Output INT OUTPUT)
                WITH ENCRYPTION
                AS
                BEGIN
	                SET NOCOUNT ON;
	                INSERT INTO dbo.ZoneGroup (
		                [Name],
		                Zones)
	                VALUES (
		                @Name,
		                @Zones);
	                SET @Output = SCOPE_IDENTITY();
	                SET NOCOUNT OFF;
                END
                GO

                ---------------------------------------------------------------------------------------------------

                IF EXISTS (SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID('dbo.ZoneGroupUpdate') AND type in ('P', 'PC'))
                BEGIN
	                DROP PROCEDURE dbo.ZoneGroupUpdate;
                END
                GO

                CREATE PROCEDURE dbo.ZoneGroupUpdate (
	                @ID INT,
	                @Name VARCHAR(64),
	                @Zones VARCHAR(512))
                WITH ENCRYPTION
                AS
                BEGIN
	                SET NOCOUNT ON;
	                UPDATE dbo.ZoneGroup
	                SET
		                [Name] = @Name,
		                Zones = @Zones,
		                UpdateDate = GETDATE(),
		                UpdateUser = SYSTEM_USER
	                WHERE
		                ID = @ID;
	                RETURN @@ROWCOUNT;
	                SET NOCOUNT OFF;
                END
                GO

                ---------------------------------------------------------------------------------------------------

                IF EXISTS (SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID('dbo.ZoneGroupDelete') AND type in ('P', 'PC'))
                BEGIN
	                DROP PROCEDURE dbo.ZoneGroupDelete;
                END
                GO

                CREATE PROCEDURE dbo.ZoneGroupDelete (
	                @ID INT)
                WITH ENCRYPTION
                AS
                BEGIN
	                SET NOCOUNT ON;
	                DELETE FROM dbo.ZoneGroup
	                WHERE
		                ID = @ID;
	                RETURN @@ROWCOUNT;
	                SET NOCOUNT OFF;
                END
                GO";

            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
            String[] instances = (String[])rk.GetValue("InstalledInstances");
            if (instances.Length > 0)
            {
                foreach (String element in instances)
                {
                    SqlConnectionInfo conn;
                    if (element == "MSSQLSERVER")
                    {
                        conn = new SqlConnectionInfo(System.Environment.MachineName, "InteractTech", "password123");
                    }
                    else
                    {
                        conn = new SqlConnectionInfo(System.Environment.MachineName + @"\" + element, "InteractTech", "password123");
                    }
                    ServerConnection connection = new ServerConnection(conn);
                    Server server = new Server(connection);
                    for (Int32 i = 0; i < server.Databases.Count; i++)
                    {
                        if (server.Databases[i].Name.Contains("GeneralLedger"))
                        {
                            try
                            {
                                Int32 x = connection.ExecuteNonQuery(String.Format(sqlCommand, server.Databases[i].Name));
                                result = true;
                            }
                            catch
                            {
                                result = false;
                            }
                        }
                    }
                }
            }

            return result;
        }

        //static void backup_Complete(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        //{
        //    WriteToLogAndConsole(e.ToString() + "% Complete");
        //}

        //static void backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        //{
        //    WriteToLogAndConsole(e.Percent.ToString() + "% Complete");
        //}

        //static void restore_Complete(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        //{
        //    WriteToLogAndConsole(e.ToString() + " Complete");
        //}

        //static void restore_PercentComplete(object sender, PercentCompleteEventArgs e)
        //{
        //    WriteToLogAndConsole(e.Percent.ToString() + "% Complete");
        //} 
        #endregion
    }
}
