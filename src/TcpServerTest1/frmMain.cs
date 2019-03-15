using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace TcpServerTest1
{
    public partial class frmMain : Form
    {
        private TcpListener Listener = null;
        private ManualResetEvent EventStop = new ManualResetEvent(false);
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StopSocket();
            StartSocket();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopSocket();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void StartSocket()
        {
            int port = int.Parse(textBox2.Text);
            Listener = new TcpListener(IPAddress.Any, port);
            EventStop.Reset();
            Listener.Start();
            Listener.BeginAcceptTcpClient(_AcceptTcpClient, null);
        }

        private void StopSocket()
        {
            EventStop.Set();
            if (null != Listener)
            {
                Listener.Stop();
                Listener = null;
            }
        }

        private void _AcceptTcpClient(IAsyncResult ar)
        {
            try
            {
                TcpClient client = Listener.EndAcceptTcpClient(ar);

                //把新的accepted socket丟到threadpool去負責收資料
                ThreadPool.QueueUserWorkItem(_ReceiveTcp, client);
                Listener.BeginAcceptTcpClient(_AcceptTcpClient, null);
            }
            catch (Exception ex)
            { }
        }

        private void _ReceiveTcp(object arg)
        {
            TcpClient client = (TcpClient)arg;
            NetworkStream stream = client.GetStream();
            //stream.Seek(0, System.IO.SeekOrigin.End);

            while (false == EventStop.WaitOne(1) && client.Connected)
            {
                byte[] buffer = new byte[256];
                int read_size = stream.Read(buffer, 0, buffer.Length);
                if (read_size > 0)
                {
                    //byte buffer轉 unicode 時要這樣處理，不然buffer裡的空字元 0x00 會在unicode顯示 \0\0\0\0
                    string msg = Encoding.ASCII.GetString(buffer).TrimEnd('\0');
                    UpdateTextbox(textBox1, msg);
                }
            }
            stream.Close();
            client.Close();
        }

        private void UpdateTextbox(TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { UpdateTextbox(tb, msg, append); }));
            }
            else
            {
                if (append)
                    tb.AppendText(msg + "\r\n");
                else
                    tb.Text = msg;
            }
        }
    }
}
