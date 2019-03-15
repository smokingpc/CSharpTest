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

namespace TcpServerTest2
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

                TcpRecvObj obj = new TcpRecvObj()
                {
                    Client = client,
                    Buffer = new byte[256],
                };
                //不走TcpClient，用更底層的Socket直接讀
                client.Client.BeginReceive(obj.Buffer, 0, obj.Buffer.Length, SocketFlags.None, _ReceiveTcp, obj);
                Listener.BeginAcceptTcpClient(_AcceptTcpClient, null);
            }
            catch (Exception ex)
            { }
        }

        private void _ReceiveTcp(IAsyncResult ar)
        {
            TcpRecvObj obj = (TcpRecvObj)ar.AsyncState;
            SocketError err;
            int read_size = obj.Client.Client.EndReceive(ar, out err);
            if (err == SocketError.Success)
            {
                if (read_size > 0)
                {
                    //byte buffer轉 unicode 時要這樣處理，
                    //不然buffer裡的空字元 0x00 會在unicode顯示 \0\0\0\0
                    string msg = Encoding.ASCII.GetString(obj.Buffer).TrimEnd('\0');
                    UpdateTextbox(textBox1, msg);

                    //讀取完資料記得把buffer清一清
                    Array.Clear(obj.Buffer, 0, obj.Buffer.Length);
                }

                //如果連線沒斷掉就繼續呼叫AsyncReceive讀下一筆
                if (obj.Client.Client.Connected)
                {
                    obj.Client.Client.BeginReceive(obj.Buffer, 0, obj.Buffer.Length, SocketFlags.None, _ReceiveTcp, obj);
                }
            }
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

    class TcpRecvObj
    {
        public TcpClient Client = null;
        public byte[] Buffer = null;
    }
}
