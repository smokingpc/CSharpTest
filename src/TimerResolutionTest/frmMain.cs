using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimerResolutionTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int loop = int.Parse(textBox1.Text);
            bool high_precision = checkBox1.Checked;
            int interval = int.Parse(textBox3.Text);

            textBox2.Text = "";
            TestThreadTimer(loop, interval, high_precision);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int loop = int.Parse(textBox1.Text);
            bool high_precision = checkBox1.Checked;
            int interval = int.Parse(textBox3.Text);

            textBox2.Text = "";
            TestSystemTimer(loop, interval, high_precision);
        }


        public static void SetText(TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { SetText(tb, msg, append); }));
            }
            else
            {
                if (append)
                    tb.AppendText(msg + "\r\n");
                else
                    tb.Text = msg + "\r\n";
            }
        }

        //timer resolution 單位是 tick (100ns)
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution", SetLastError = true)]
        public static extern int NtSetTimerResolution(int DesiredResolution, bool SetResolution, ref int CurrentResolution);

    }
}
