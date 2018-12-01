using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadWaitTest
{
    public partial class Form1 : Form
    {
        private ManualResetEvent EventStopRun = new ManualResetEvent(false);
        private Thread TestThread = null;
        private DateTime BeginTime;
        private DateTime EndTime;


        private Int64 Counter = 0;
        public Form1()
        {
            InitializeComponent();

            int worker = 0;
            int completion = 0;
            ThreadPool.GetMaxThreads(out worker, out completion);
            ThreadPool.GetAvailableThreads(out worker, out completion);

            TestThread = new Thread(WaitThread);
            TestThread.Start();
        }

        private void WaitThread()
        {
            BeginTime = DateTime.Now;
            while(!EventStopRun.WaitOne(1))
            {
                Counter++;
            }
            EndTime = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventStopRun.Set();
            TimeSpan interval = EndTime.Subtract(BeginTime);
            string msg = string.Format("Counter={0}, Elapsed Time={1}", Counter, interval.ToString("g"));
            MessageBox.Show(msg);
        }
    }
}
