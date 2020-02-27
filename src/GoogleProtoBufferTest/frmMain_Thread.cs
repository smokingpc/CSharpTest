using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Google.Protobuf;

namespace GoogleProtoBufferTest
{
    public partial class frmMain
    {
        Queue<byte[]> queue = new Queue<byte[]>();
        private void WorkThread()
        {
            int counter = 0;

            while (counter < 3)
            { 
                if(queue.Count > 0)
                {
                    byte[] block = queue.Dequeue();
                    var data  = CUser.Parser.ParseFrom(block);

                    string msg = string.Format("Name={0}, Age={1}, Address={2}", data.Name, data.Age, data.Address);
                    textBox1.SetLine(msg);

                    counter++;
                }
            }
        }
    }
}
