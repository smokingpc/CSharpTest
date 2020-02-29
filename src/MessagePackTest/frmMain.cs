using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using MessagePack.Internal;

namespace MessagePackTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.SetText("");
            textBox2.SetLine("[Test1]");

            CMyData data = new CMyData() { Age = 77 };
            var blob = MessagePackSerializer.Serialize(data);

            string msg = string.Format("Blob Converted, binary size={0}", blob.Length);
            textBox2.SetLine(msg);

            CMyData new_data = MessagePackSerializer.Deserialize<CMyData>(blob);
            msg = string.Format("Deserialize data done! Name = {0}", new_data.Name);
            textBox2.SetLine(msg);

            msg = string.Format("Convert from blob to JSON=>\r\n{0}", MessagePackSerializer.ConvertToJson(blob));
            textBox2.SetLine(msg);
            textBox2.SetLine("");

            textBox2.SetLine("[Test2]");
            CMyData2 data2 = new CMyData2() { Age = 22 };
            var blob2 = MessagePackSerializer.Serialize(data2);
            msg = string.Format("Blob2 Converted, binary size={0}", blob2.Length);
            textBox2.SetLine(msg);
            msg = string.Format("Convert from blob2 to JSON=>\r\n{0}", MessagePackSerializer.ConvertToJson(blob2));
            textBox2.SetLine(msg);

            textBox2.SetLine("[Test Inheritance]");
            CMyData3 data3 = new CMyData3() { Age = 44 };
            var blob3 = MessagePackSerializer.Serialize(data3);
            msg = string.Format("Blob3 Converted, binary size={0}", blob3.Length);
            textBox2.SetLine(msg);
            msg = string.Format("Convert from blob3 to JSON=>\r\n{0}", MessagePackSerializer.ConvertToJson(blob3));
            textBox2.SetLine(msg);
        }
    }

    //注意：安裝MessagePack前要先裝 NETStandard.Library 套件，有這套件就能讓
    //     .Net Framework程式吃 .NetCore 的 DLL

    //1. Key只對 public member有效，不論property或field
    //2. Key編號如有跳號，中間跳過的號會在JSON裡補null
    //3. 轉成JSON時會省去 KeyName
    //4. 如果 class 以 [MessagePackObject(keyAsPropertyName:true)] 修飾，
    //   則不需要[Key()] 修飾member，但這樣轉出的Blob比較肥
    //5. 如果有繼承，child class的欄位 Key() 號碼不能跟parent class欄位號碼相同，要接著排下去
    [MessagePackObject]
    public class CMyData
    {
        [Key(0)]
        public string Name = "TestName";

        [Key(1)]
        public int Age { get; set; }

        [IgnoreMember]
        public DateTime Timestamp { get; set; }

        [Key(2)]
        public byte[] Payload = new byte[] { 0x30, 0x31, 0x32, 0x33 };
    }

    [MessagePackObject(true)]
    public class CMyData2
    {
        public string Name = "TestName2";

        public int Age { get; set; }

        public byte[] Payload = new byte[] { 0x35, 0x36, 0x37, 0x38 };
    }

    [MessagePackObject]
    public class CMyData3 : CMyData
    {
        [Key(3)]
        public string NickAlias = "Alias";
    }
}