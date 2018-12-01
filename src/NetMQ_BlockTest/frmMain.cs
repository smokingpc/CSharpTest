using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NetMQ.Sockets;
using NetMQ;

namespace NetMQ_BlockTest
{
    public partial class frmMain : Form
    {
        PublisherSocket Sock = null;// new PublisherSocket("tcp://192.168.0.117:5567");
        //PushSocket Sock = new PushSocket("tcp://192.168.0.117:5567");

        private ManualResetEvent EventStop = new ManualResetEvent(false);
        private Thread WorkThread = null;
        private int SleepInterval = 10;     //每10ms送一筆
        private Int32 SentCounter = 0;
        private Int32 WorkItemCounter = 0;

        public frmMain()
        {
            InitializeComponent();

        }
        private void SendPushMessage()
        {
            while (!EventStop.WaitOne(SleepInterval))
            {
                //if (WorkItemCounter <= 1200)
                {
                    ThreadPool.QueueUserWorkItem(WorkFunction);
                    int result = Interlocked.Increment(ref WorkItemCounter);
                    if (result % 200 == 0)
                    {
                        string msg = string.Format("{0} workitem created\r\n", result);
                        UpdateTextBox(textBox1, msg);
                    }
                }
            }
        }

        private void WorkFunction(object state)
        {
            string data = Guid.NewGuid().ToString();
            Sock.SendFrame(data);

            int result = Interlocked.Increment(ref SentCounter);
            if (result % 200 == 0)
            {
                string msg = string.Format("{0} msg sent\r\n", result);
                UpdateTextBox(textBox1, msg);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Sock = new PushSocket("tcp://192.168.0.117:5567");
            //Sock = new PushSocket();
            //Sock.Bind("tcp://0.0.0.0:5567");
            Sock = new PublisherSocket("tcp://0.0.0.0:5567");
            //Sock = new PublisherSocket();
            //Sock.Connect("tcp://192.168.0.117:5567");
            //Sock.Bind("tcp://0.0.0.0:5567");
            
            EventStop.Reset();
            WorkThread = new Thread(SendPushMessage);
            WorkThread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (null != WorkThread && WorkThread.IsAlive)
            {
                EventStop.Set();
                WorkThread.Join();
                WorkThread = null;
            }

            Sock.Close();
            Sock = null;
        }
        private void UpdateTextBox(TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke(
                           (Action)(() => { UpdateTextBox(tb, msg, append); })
                        );
            }
            else
            {
                if (append)
                    tb.AppendText(msg);
                else
                    tb.Text = msg;
            }
        }
    }
}
