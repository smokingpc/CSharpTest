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
        private UdpClient SockServer = null;

        private void StartServer()
        {
            SockServer = new UdpClient(ServerPort);
            SockServer.BeginReceive(_ServerRecv, null);
        }

        private void _ServerRecv(IAsyncResult ar)
        {
            //.Net的網路元件非常討厭的一件事情是：不管同步或非同步的動作，
            //都沒有任何方法可以cancel。目前只有把這些Tcp / udp元件的任何動作用try catch
            //包起來，想要cancel operation就直接close元件，讓它觸發Exception強制離開

            try 
            {
                IPEndPoint remote = null;   //用來取得  "這封包從哪裡發來的"
                byte[] buffer = SockServer.EndReceive(ar, ref remote);
                string msg = Encoding.UTF8.GetString(buffer);
                msg = msg.Replace("\0", "");
                textBox5.SetLine(msg);
            }
            catch (Exception ex) 
            {            }
        }

        private void StopServer()
        {
            if (null != SockServer)
            {
                SockServer.Close();
                SockServer = null;
            }
        }
    }
}
