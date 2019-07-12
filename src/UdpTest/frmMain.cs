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

namespace UdpTest
{
    public partial class frmMain : Form
    {
        public int ClientPort { get { return int.Parse(textBox1.Text); } }
        public int ServerPort { get { return int.Parse(textBox6.Text); } }

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartServer();
            System.Threading.Thread.Sleep(500);
            StartClient();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopClient();
            StopServer();
            ClearUI();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendMessage(SockClient, textBox2.Text, new IPEndPoint(IPAddress.Loopback, ServerPort));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendMessage(SockServer, textBox4.Text, new IPEndPoint(IPAddress.Loopback, ClientPort));
        }

        private void ClearUI()
        {
            textBox1.Text = "55688";
            textBox6.Text = "12345";
            textBox2.Text = textBox3.Text = textBox5.Text = textBox4.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearUI();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            button2.PerformClick();
        }
    }
}
