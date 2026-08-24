using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Interact.BusinessLogic;
using Interact.Common;

namespace Interact.UserInterface
{
    static class Program
    {
        public static String UserName;

        //Data comes from SerialNo in App.config
        public static Settings Settings;

        //User settings only
        public static Branch Branch;

        public static Boolean IsReadOnly;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Program.Settings = Settings.CreateSettings(Properties.Settings.Default.ApplicationKey);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
        }
    }
}
