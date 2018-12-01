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
using System.Net.NetworkInformation;

namespace NICInformationTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseAllNIC();
        }

        private void ParseAllNIC()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet)
                {
                    var property = nic.GetIPProperties();
                    textBox1.AppendText(string.Format("==NIC = {0}\r\n", nic.Name));

                    if (property.AnycastAddresses.Count > 0)
                        ShowAnycast(property);
                    if (property.MulticastAddresses.Count > 0)
                        ShowMulticast(property);
                    if (property.UnicastAddresses.Count > 0)
                        ShowUnicast(property);
                }
            }
        }

        private void ShowAnycast(IPInterfaceProperties prop)
        {
            textBox1.AppendText("anycast address=");
            foreach (var addr in prop.AnycastAddresses)
            {
                textBox1.AppendText(addr.Address.ToString());
                textBox1.AppendText(", ");
            }
            textBox1.AppendText("\r\n");
        }
        private void ShowUnicast(IPInterfaceProperties prop)
        {
            textBox1.AppendText("unicast address=");
            foreach (var addr in prop.UnicastAddresses)
            {
                string msg = string.Format("{0}/{1}", addr.Address.ToString(), addr.IPv4Mask.ToString());
                textBox1.AppendText(msg);
                textBox1.AppendText(", ");
            }
            textBox1.AppendText("\r\n");
        }
        private void ShowMulticast(IPInterfaceProperties prop)
        {
            textBox1.AppendText("multicast address=");
            foreach (var addr in prop.MulticastAddresses)
            {
                textBox1.AppendText(addr.Address.ToString());
                textBox1.AppendText(", ");
            }
            textBox1.AppendText("\r\n");
        }
        public string GetLocalIP(string target)
        {
            //string result = "";
            //nic 是常用名詞， Network Interface Card的縮寫
            foreach(NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                var property = nic.GetIPProperties();
                foreach (var addr in property.UnicastAddresses)
                { 
                    if(IsSameSubnet(target, addr.Address.ToString(), addr.IPv4Mask.ToString()))
                    {
                        return addr.Address.ToString();
                    }   
                }
            }

            return "";
        }

        public bool IsSameSubnet(string ip1, string ip2, string netmask)
        {
            bool result = false;

            IPAddress temp = null;
            byte[] addr1 = null;
            byte[] addr2 = null;
            byte[] mask = null;

            bool[] compare = new bool[4] { false, false, false, false };

            if (IPAddress.TryParse(ip1, out temp))
                addr1 = temp.GetAddressBytes();
            if (IPAddress.TryParse(ip2, out temp))
                addr2 = temp.GetAddressBytes();
            if (IPAddress.TryParse(netmask, out temp))
                mask = temp.GetAddressBytes();

            if (addr1 != null && addr2 != null && mask != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if ((addr1[i] & mask[i]) == (addr2[i] & mask[i]))
                        compare[i] = true;
                }
            }

            //四個欄位都符合才算通過
            result = compare[0] & compare[1] & compare[2] & compare[3];
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ip1 = "192.168.0.95";
            string[] tokens = textBox2.Text.Split('/');
            string ip2 = tokens[0].Trim();
            string netmask = tokens[1].Trim();
            if (IsSameSubnet(ip1, ip2, netmask))
                MessageBox.Show(string.Format("{0} and {1} are same subnet under netmask {2}",
                                    ip1, ip2, netmask));
            else
                MessageBox.Show("Test failed");
        }
    }
}
