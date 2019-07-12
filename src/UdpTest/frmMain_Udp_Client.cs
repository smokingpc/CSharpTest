using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace UdpTest
{
    public partial class frmMain
    {
        private UdpClient SockClient = null;

        private void StartClient()
        {
            SockClient = new UdpClient(ClientPort);
            SockClient.BeginReceive(_ClientRecv, null);
        }

        private void StopClient()
        {
            if (null != SockClient)
            {
                SockClient.Close();
                SockClient = null;
            }
        }

        private void SendMessage(UdpClient sock, string msg, IPEndPoint target)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            SockClient.Send(buffer, buffer.Length, target);
        }

        private void _ClientRecv(IAsyncResult ar)
        {
            //.Net的網路元件非常討厭的一件事情是：不管同步或非同步的動作，
            //都沒有任何方法可以cancel。目前只有把這些Tcp / udp元件的任何動作用try catch
            //包起來，想要cancel operation就直接close元件，讓它觸發Exception強制離開

            try
            {
                IPEndPoint remote = null;   //用來取得  "這封包從哪裡發來的"
                byte[] buffer = SockClient.EndReceive(ar, ref remote);
                string msg = Encoding.UTF8.GetString(buffer);
                msg = msg.Replace("\0", "");
                textBox3.SetLine(msg);
            }
            catch (Exception ex)
            { }
        }

    }
}
