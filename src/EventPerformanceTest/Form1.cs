using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        private ConcurrentQueue<string> TestQueue = new ConcurrentQueue<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventStop.Reset();
            Thread thread = new Thread(TestFunction);
            thread.Start();
            thread = new Thread(TestFunction2);
            thread.Start();
        }

        private void TestFunction()
        {
            textBox3.AppendLine($"===========[Test Begin]==========");
            textBox3.AppendLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            int count = 0;
            int limit = int.Parse(textBox1.Text);
            int interval = int.Parse(textBox2.Text);
            UInt64 loop_run_count = 0;

            List<string> ret = new List<string>();
            while (false == EventStop.Wait(interval)&& (count < limit))
            {
                bool ok = TestQueue.TryDequeue(out string data);
                if (ok)
                {
                    ret.Add(data);
                    count++;
                }
                loop_run_count++;
            }

            textBox3.AppendLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} Test END\r\n");
        }
        private void TestFunction2()
        {
            int count = int.Parse(textBox1.Text);
            int interval = int.Parse(textBox2.Text);
            Random rnd = new Random();

            while (false == EventStop.Wait(interval) && count > 0)
            {
                if (rnd.Next() % 2 == 0)
                {
                    count--;
                    TestQueue.Enqueue(DateTime.Now.ToLongDateString());
                }
                else
                    Thread.Sleep(1);
            }
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
