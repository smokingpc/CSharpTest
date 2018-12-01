using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net.Appender;

namespace Log4NetTest
{
    public partial class frmMain : Form
    {
        private readonly ILog Log1 = LogManager.GetLogger("WindSeeker");
        private readonly ILog Log2 = LogManager.GetLogger("test");
        private readonly static string ExePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public frmMain()
        {
            InitializeComponent();

            string file = "logconfig.xml";
            XmlConfigurator.Configure(new FileInfo(string.Format("{0}\\{1}", ExePath, file)));
            var repository = (Hierarchy)log4net.LogManager.GetRepository();
            FileAppender appender = repository.Root.GetAppender("RollingFileLog") as FileAppender;

            appender.File = string.Format("{0}\\{1}", ExePath, "test.log") ;
            appender.ActivateOptions(); //apply new filename
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Log1.Error("Test Log1");
            Log2.Error("Test Log2");
        }
    }
}
