using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnixTimestampConvertTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DateTime to Linux Timestamp(in millisecond, 13 digit)
            var current = dateTimePicker1.Value;
            var diff = current.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime());

            Int64 seconds = Convert.ToInt64(diff.TotalMilliseconds);
            textBox1.Text = seconds.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Linux Timestamp(in millisecond, 13 digit) to Datetime
            var linux_base = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var csharp_time = linux_base.AddMilliseconds(Convert.ToInt64(textBox1.Text));
            dateTimePicker1.Value = csharp_time.ToLocalTime();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
