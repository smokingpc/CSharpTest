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

using System.Net;
using NetMQ;
using NetMQ.Sockets;

namespace NetMQ_MultiConnectTest
{
    public partial class frmMain : Form
    {
        bool Running = false;
        int WaitInterval = 20;
        int SleepTime = 2000;
        ManualResetEventSlim EventStop = new ManualResetEventSlim(false);
        List<int> PortList = new List<int>() { 55688, 55689, 55690 };

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Running)
            {
                EventStop.Set();
                button1.Text = "GO";
            }
            else
            {
                EventStop.Reset();
                Task.Run(ThreadPub);
                Task.Run(ThreadSub);
                button1.Text = "STOP";
            }
            Running ^= true;
        }

        private void ThreadPub()
        {
            List<PublisherSocket> socklist = new List<PublisherSocket>();
            foreach (var port in PortList)
            {
                string addr = $"tcp://{IPAddress.Any}:{port}";
                var sock = new PublisherSocket();
                sock.Options.IPv4Only = true;
                sock.Bind(addr);
                socklist.Add(sock);
            }

            while (EventStop.Wait(SleepTime) == false)
            {
                DateTime timestamp = DateTime.Now;
                for (int i = 0; i < socklist.Count; i++)
                {
                    string prefix = "";
                    switch (i)
                    {
                        case 0:
                            prefix = "I am first!";
                            break;
                        case 1:
                            prefix = " second one";
                            break;
                        case 2:
                            prefix = "I want to go home";
                            break;
                    }
                    string data = $"[from {prefix}] timestamp={timestamp.ToString("HH:mm:ss.fff")}";
                    var buffer = Encoding.Unicode.GetBytes(data);
                    var sock = socklist[i];
                    sock.SendFrame(buffer);
                }
            }

            foreach (var sock in socklist)
            {
                sock.Close();
            }
        }

        private void ThreadSub()
        {
            List<SubscriberSocket> socklist = new List<SubscriberSocket>();
            var sock = new SubscriberSocket();
            sock.Options.IPv4Only = true;
            sock.SubscribeToAnyTopic();

            foreach (var port in PortList)
            {
                string addr = $"tcp://{IPAddress.Loopback}:{port}";
                sock.Connect(addr);
            }

            Thread.Sleep(2000);

            while (EventStop.Wait(WaitInterval) == false)
            {
                var buffer = sock.ReceiveFrameBytes();
                string msg = Encoding.Unicode.GetString(buffer);
                textBox1.AppendLine(msg);
            }

            sock.Close();
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
