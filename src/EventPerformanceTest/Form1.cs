using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EventPerformanceTest
{
    public partial class Form1 : Form
    {
        ManualResetEventSlim EventStop = new ManualResetEventSlim(false);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventStop.Reset();
            Thread thread = new Thread(TestFunction);
            thread.Start();
        }

        private void TestFunction()
        {
            textBox3.AppendLine($"===========[Test Begin]==========");
            textBox3.AppendLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            int count = int.Parse(textBox1.Text);
            int interval = int.Parse(textBox2.Text);

            while (false == EventStop.Wait(interval) && count > 0)
            {
                count--;
            }

            textBox3.AppendLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} Test END\r\n");
        }
    }

    public static class Extension
    {
        public static void AppendLine(this TextBox tb, string msg)
        {
            if (tb.InvokeRequired)
            {
                tb.BeginInvoke((Action)(() => { AppendLine(tb, msg); }));
            }
            else
            {
                tb.AppendText(msg + "\r\n");
            }
        }
    }
}
