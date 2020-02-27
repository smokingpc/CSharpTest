using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Google.Protobuf;
using System.Threading;

namespace GoogleProtoBufferTest
{
    //ProtoBuf撰寫步驟
    //1.寫proto檔，以中繼語言來定義資料格式
    //2.用protoc-3.11.4-win64.zip裡面的 protoc.exe去編譯 proto檔
    //  cmdline 範例 =>  protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/MyData.proto
    //  -I是增加 include dir
    //3.編譯後就會幫你生出CS檔，當做標準class來用即可

    //注意：proto編譯器那包裡面的include目錄擺的是 protobuf 預設的一些資料定義

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(new Action(WorkThread));

            CUser data = new CUser();

            using (MemoryStream ms = new MemoryStream())
            {
                data.Name = "Roy";
                data.Age = 44;
                data.Address = "Taipei";
                data.WriteTo(ms);
                queue.Enqueue(ms.ToArray());
                Thread.Sleep(1000);
                
                data.Name = "Kevin";
                data.Age = 28;
                data.Address = "TaiChung";
                data.WriteTo(ms);
                queue.Enqueue(ms.ToArray());
                Thread.Sleep(1000);

                data.Name = "Leo";
                data.Age = 50;
                data.Address = "KaoHsung";
                data.WriteTo(ms);
                queue.Enqueue(ms.ToArray());
            }
        }
    }
}
