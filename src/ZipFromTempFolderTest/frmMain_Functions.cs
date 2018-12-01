using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipFromTempFolderTest
{
    public partial class frmMain
    {
        private static readonly string ExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        private void InitUI()
        { 
            //更新 RadioGroup 的item清單
            radioGroup1.Properties.Items.Clear();
            radioGroup1.Properties.Items.AddEnum<PATH_METHOD>();
            radioGroup1.SelectedIndex = 0;
            
            radioGroup2.Properties.Items.Clear();
            radioGroup2.Properties.Items.AddEnum<NAME_ENCODING>();
            radioGroup2.SelectedIndex = 0;
        }

        private string CreateTempPath()
        {
            //用API抓目前的temp folder在哪 (傳回的路徑，結尾會帶反斜線 "\")
            string os_temp_path = Path.GetTempPath();
            string temp_name = "";
            var type = (PATH_METHOD)radioGroup1.EditValue;

            if (type == PATH_METHOD.NATIVE)
            {
                //隨便取得一個亂數名稱，雖然它帶附檔名但也可以幹掉附檔名後用在Path
                temp_name = Path.GetRandomFileName();
                temp_name = Path.GetFileNameWithoutExtension(temp_name);
            }
            else if (type == PATH_METHOD.GUID)
            {
                temp_name = Guid.NewGuid().ToString();
            }

            string my_temp_path = os_temp_path + temp_name;
            Directory.CreateDirectory(my_temp_path);

            string msg = string.Format("Create TempFolder : {0}", my_temp_path);
            WriteLogLine(msg);
            
            return my_temp_path;
        }
        private void CreateTestFile(string path)
        {
            string name = Path.GetRandomFileName();
            name = Path.GetFileNameWithoutExtension(name) + ".txt";
            string filepath = path + "\\" + name;
            var writer = System.IO.File.CreateText(filepath);
            writer.WriteLine("我是測試檔案");
            writer.Close();

            string msg = string.Format("Create TempFile : {0}", filepath);
            WriteLogLine(msg);
        }
        private void ZipTestPath(string path)
        {
            string filename = Path.GetRandomFileName();
            filename = Path.GetFileNameWithoutExtension(filename);
            string zip_path = string.Format("{0}\\{1}.zip", ExePath, filename);
            var type = (NAME_ENCODING)radioGroup1.EditValue;
            var encode = Encoding.Default;

            //不指定檔名編碼，用作業系統的預設值
            if (type == NAME_ENCODING.NATIVE)
                encode = Encoding.Default;
            else
                encode = Encoding.UTF8;

            //把指定目錄壓成ZIP檔，並指定壓縮等級與檔名編碼
            System.IO.Compression.ZipFile.CreateFromDirectory
                            (path, zip_path, System.IO.Compression.CompressionLevel.Optimal, 
                                false, encode);

            string msg = string.Format("Create ZipFile : {0}", zip_path);
            WriteLogLine(msg);
        }

        private void WriteLogLine(string msg)
        {
            if (textBox1.InvokeRequired)
            {
                //如果textBox1.InvokeRequired == true 表示受ui thread protect限制
                //此時需要用Invoke，用委派再呼叫一次自己，這樣才能更新畫面資料
                textBox1.Invoke((Action)(
                                    () => { WriteLogLine(msg); }
                                ));
            }
            else
            {
                textBox1.AppendText(msg + "\r\n");
            }
        }
    }

    public enum PATH_METHOD
    { 
        [Description("作業系統亂數名稱")]
        NATIVE = 0,
        [Description("GUID名稱")]
        GUID = 1,
    }

    public enum NAME_ENCODING
    {
        [Description("預設編碼(by OS)")]
        NATIVE = 0,
        [Description("UTF8")]
        UTF8 = 1,
    }
}
