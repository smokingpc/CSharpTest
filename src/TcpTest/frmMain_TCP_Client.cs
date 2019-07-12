using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpTest
{
    public partial class frmMain
    {
        private TcpClient SockClient = null;
        private ManualResetEventSlim EventClientStop = new ManualResetEventSlim(false);

        private void StartClient()
        {
            StopClient();

            SockClient = new TcpClient();
            SockClient.Connect("127.0.0.1", this.TcpPort);
            EventClientStop.Reset();
            Task.Run(() => { ThreadRecvMessage(SockClient, textBox3); });
        }

        private void SendMessage(TcpClient socket, string msg)
        {
            //傳binary data
            //如果要用來傳字串，記得用正確的encoding轉換成byte array
            byte[] buffer = Encoding.Unicode.GetBytes("send msg => " + msg);
            int sent_size = socket.Client.Send(buffer);
        }

        private void StopClient()
        {
            EventClientStop.Set();
            if (SockClient != null)
            {
                SockClient.Close();
                SockClient = null;
            }
        }
    }
}
