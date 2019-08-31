using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLogTest
{
    public partial class frmMain : Form
    {
        //init NLog
        private NLog.Logger Log1 = NLog.LogManager.GetLogger("mytest1");
        private NLog.Logger Log2 = NLog.LogManager.GetLogger("abc");
        public frmMain()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = "Who is calling a fleet ?";
            Log1.Error(msg + "1");
            Log2.Error(msg + "2");

            MessageBox.Show("Done!");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Done!");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
