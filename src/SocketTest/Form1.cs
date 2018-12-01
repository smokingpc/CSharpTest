using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace SocketTest
{
    public partial class Form1 : Form
    {
        public delegate void DelegateUpdateTextbox(TextBox textbox, string newstr);

        public TcpListener SockListen = null;
        public TcpClient SockIn = null;
        public byte[] RecvBuf = new byte[1024];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(null != SockListen)
            {
                //SockListen.Close();
                SockIn.Close();
                SockIn = null;
                Array.Clear(RecvBuf, 0, 1024);
            }

            IPEndPoint ListenEP = new IPEndPoint(IPAddress.Any, int.Parse(textBox1.Text));
            SockListen = new TcpListener(ListenEP);
            SockListen.Start();
            SockIn = SockListen.AcceptTcpClient();
            SockListen.Stop();
            try
            {
                SockIn.Client.BeginReceive(RecvBuf, 0, 1024, SocketFlags.None, new AsyncCallback(AsyncRecv), null);
            }
            catch (Exception)
            {}
        }

        private void AsyncRecv(IAsyncResult ar)
        {
            int size = SockIn.Client.EndReceive(ar);

            if (size > 0)
            {
                string newstr = Encoding.ASCII.GetString(RecvBuf);
                UpdateTextBox(textBox2, newstr);
                Array.Clear(RecvBuf, 0, 1024);
                try
                {
                    SockIn.Client.BeginReceive(RecvBuf, 0, 1024, SocketFlags.None, new AsyncCallback(AsyncRecv), null);
                }
                catch (Exception)
                {}
            }

        }


        private void UpdateTextBox(TextBox textbox, string newstr)
        {
            if(textbox.InvokeRequired)
            {
                textbox.Invoke(new DelegateUpdateTextbox(UpdateTextBox), textbox, newstr);
            }
            else
            {
                textbox.Text = newstr;
            }
        }
    }
}
