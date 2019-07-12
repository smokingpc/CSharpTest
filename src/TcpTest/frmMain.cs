using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpTest
{
    public partial class frmMain : Form
    {
        public int TcpPort
        { get { return int.Parse(textBox1.Text); } }

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartServer();
            //這測試是在同一台電腦上，要睡一下免得Server還沒起來，client會連不上去...
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
            //client send msg to server
            SendMessage(SockClient, textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //server send to client
            SendMessage(SockIncoming, textBox4.Text);
        }

        private void ClearUI()
        {
            textBox1.Text = "55688";
            textBox2.Text = textBox3.Text = textBox5.Text = textBox4.Text = "";
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ClearUI();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            button2.PerformClick();
        }

    }
}
