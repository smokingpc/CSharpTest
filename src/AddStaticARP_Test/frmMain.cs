using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AddStaticARP_Test
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private string ShowArpTable ()
        {
            string ret = "";
            Process proc = new Process();
            proc.StartInfo.FileName = "arp";
            proc.StartInfo.Arguments = " -a "; 
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;//no popup window of command... I don't want to see it! :(

            proc.Start();
            ret = proc.StandardOutput.ReadToEnd();//read all response text of command from STDOUT...
            return ret;
        }
        private string AddStaticArp(string myip, string target_ip, string target_mac)
        {
        //Command Line call arp.exe to add arp entry:
        // arp.exe -s <%target ip%> <%target mac%> <%my NIC ip to add this arp entry%>
        //      -s ==> static
        //example : arp -s  192.168.20.199  aa-bb-cc-dd-ee-ff  192.168.20.10
        //NOTE: spliter of MAC address should be "-", not ":"

            Process proc = new Process();
            proc.StartInfo.FileName = "arp";
            proc.StartInfo.Arguments = $" -s {target_ip} {target_mac.Replace(":", "-")} {myip}";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            
            //no popup window of command... I don't want to see it! :(
            proc.StartInfo.CreateNoWindow = true;
            
            proc.Start();

            return proc.StandardOutput.ReadToEnd();//read all response text of command from STDOUT...
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            string msg = ShowArpTable();
            textBox4.AppendText(msg + "\r\n");

            textBox4.AppendText("==============================================\r\n");
            msg = $"[Add Static ARP] {textBox2.Text} to {textBox3.Text}";
            textBox4.AppendText(msg + "\r\n");

            // if response message is empty string, it means "SUCCEED" ....
            msg = AddStaticArp(textBox1.Text, textBox2.Text, textBox3.Text);
            textBox4.AppendText(msg + "\r\n");

            textBox4.AppendText("==============================================\r\n");
            msg = ShowArpTable();
            textBox4.AppendText(msg + "\r\n");
        }
    }
}
