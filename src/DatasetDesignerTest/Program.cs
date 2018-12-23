using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;

namespace DatasetDesignerTest
{
    static class Program
    {
        private static readonly string ExeLocation = Application.ExecutablePath;
        private static readonly string ExePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ChangeConnectionString();
            ChangeConnectionString2();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static void ChangeConnectionString()
        {
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ExeLocation);
            //ConnectionStringsSection section = config.ConnectionStrings;

            //var current = section.ConnectionStrings.CurrentConfiguration;

            //string mystr = current.ConnectionStrings.ConnectionStrings["DatasetDesignerTest.Properties.Settings.MyConnStr"].ConnectionString;
            string ip = "192.168.20.13";
            string db = "TEST";
            int timeout = 5;
            string uid = "test";
            string pwd = "5678";
            string mystr = string.Format("Data Source={0};Initial Catalog={1};Connection Timeout={2};User ID={3};Password={4}",
                        ip, db, timeout, uid, pwd);

            //current.ConnectionStrings.ConnectionStrings["DatasetDesignerTest.Properties.Settings.MyConnStr"].ConnectionString = mystr;
            DatasetDesignerTest.Properties.Settings.Default["MyConnStr"] = mystr;
        }
        static void ChangeConnectionString2()
        {
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ExeLocation);
            //ConnectionStringsSection section = config.ConnectionStrings;

            //var current = section.ConnectionStrings.CurrentConfiguration;

            //string mystr = current.ConnectionStrings.ConnectionStrings["DatasetDesignerTest.Properties.Settings.MyConnStr2"].ConnectionString;
            string mystr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\{1}",
                        ExePath, "Database2.accdb");

            //current.ConnectionStrings.ConnectionStrings["DatasetDesignerTest.Properties.Settings.MyConnStr2"].ConnectionString = mystr;
            DatasetDesignerTest.Properties.Settings.Default["MyConnStr2"] = mystr;
        }
    }
}
