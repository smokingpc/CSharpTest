using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtensionTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime time = dateTimePicker1.Value;
            textBox1.AppendText(time.ToDBString());
            textBox1.AppendText("\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time = dateTimePicker1.Value;
            textBox1.AppendText(time.ToMyString());
            textBox1.AppendText("\r\n");
        }
    }


    public static class DateTimeExtension
    {
        
        public static string ToDBString(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        public static string ToMyString(this DateTime obj)
        {
            return obj.ToString("yyyy_MM_dd HH:mm");
        }
    }
}
