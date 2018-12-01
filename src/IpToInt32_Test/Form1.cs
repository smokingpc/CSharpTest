using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace IpToInt32_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(textBox1.Text);
            byte[] ip_bytes = ip.GetAddressBytes();
            
            //ip_bytes.
            textBox2.Text = string.Format("0x{0:X8}",
                IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ip_bytes, 0)));

            textBox3.Text = string.Format("0x{0:X2}", ip_bytes[0]);
            textBox4.Text = string.Format("0x{0:X2}", ip_bytes[1]);
            textBox5.Text = string.Format("0x{0:X2}", ip_bytes[2]);
            textBox6.Text = string.Format("0x{0:X2}", ip_bytes[3]);
        }
    }
}
