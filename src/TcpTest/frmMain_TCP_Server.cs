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
        //TCP Server最特別的地方在於需要先建一個listener接聽連線
        //一旦有連線進來，Listener要生一個TcpClient來代表這個連入的連線。
        //要對這個連進來的連線做讀寫，要對這個生出來的TCPClient讀寫，
        //而不是對listen port的Listener做
        private TcpListener SockListener = null;
        private TcpClient SockIncoming = null;
        private ManualResetEventSlim EventServerStop = new ManualResetEventSlim(false);

        private void StartServer()
        {
            StopServer();
            //listen可以指定要在哪個IP上Listen。如果要在所有網卡的上所有IP Listen，
            //就指定IPAddress.Any (0.0.0.0)
            SockListener = new TcpListener(IPAddress.Any, TcpPort);
            SockListener.Start(1);     //最多只接受一個connection排隊，超過的都會被拒絕
            EventServerStop.Reset();
            Task.Run(new Action(WaitIncomingConnection));
        }

        private void WaitIncomingConnection()
        {
            //如果只收一個connection可以不必用迴圈
            //如果是標準server要收N條connection in，就要用迴圈一直Accept
            while (EventServerStop.Wait(1) == false)
            {
                //Listen for incoming connection，其實用非同步比較方便...
                //.Net的網路元件非常討厭的一件事情是：不管同步或非同步的動作，
                //都沒有任何方法可以cancel。目前只有把這些Tcp / udp元件的任何動作用try catch
                //包起來，想要cancel operation就直接close元件，讓它觸發Exception強制離開
                try
                {
                    SockIncoming = SockListener.AcceptTcpClient();
                    Task.Run(() => { ThreadRecvMessage(SockIncoming, textBox5); });
                }
                catch (Exception ex)
                { }
            }
        }

        private void ThreadRecvMessage(TcpClient socket, System.Windows.Forms.TextBox tb)
        {
            string data = "";
            while (EventServerStop.Wait(1) == false)
            {
                //收binary data
                byte[] buffer = new byte[1024];
                socket.Client.Receive(buffer);
                data = Encoding.Unicode.GetString(buffer);
                //用binary方式收string要特別注意這點：buffer如果比較大，後面會有一堆'\0'
                //在顯示時就會跟著一堆奇怪字元，轉換encoding完取得字串後要記得做下面這行處理
                data = data.Replace("\0", "");
                tb.SetLine(data);
            }
        }

        private void StopServer()
        {
            EventServerStop.Set();
            if (null != SockListener)
            {
                SockListener.Stop();
                SockListener = null;
            }
            if (null != SockIncoming)
            {
                SockIncoming.Close();
                SockIncoming = null;
            }
        }
    }
}
