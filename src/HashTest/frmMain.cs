using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Security.Cryptography;

namespace HashTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add(HASH_TYPE.MD5);
            comboBox1.Items.Add(HASH_TYPE.SHA1);
            comboBox1.Items.Add(HASH_TYPE.SHA256);
            comboBox1.Items.Add(HASH_TYPE.SHA384);
            comboBox1.Items.Add(HASH_TYPE.SHA512);
            comboBox1.SelectedIndex = 2;
        }

        private byte[] DoHash(HASH_TYPE type, byte[] source)
        {
            byte[] result = null;

            HashAlgorithm algorithm = null;

            switch (type)
            { 
                case HASH_TYPE.MD5:
                    algorithm = MD5.Create();
                    break;
                case HASH_TYPE.SHA1:
                    algorithm = SHA1.Create();
                    break;
                case HASH_TYPE.SHA256:
                    algorithm = SHA256.Create();
                    break;
                case HASH_TYPE.SHA384:
                    algorithm = SHA384.Create();
                    break;
                case HASH_TYPE.SHA512:
                    algorithm = SHA512.Create();
                    break;
            }

            //hash是針對byte作處理，所以要先弄成byte[]
            if (source != null && source.Length > 0)
            {
                //byte[] src_buf = Encoding.UTF8.GetBytes(source);
                result = algorithm.ComputeHash(source, 0, source.Length);
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HASH_TYPE type = (HASH_TYPE)Enum.Parse(typeof(HASH_TYPE), comboBox1.SelectedItem.ToString());
            byte[] source = Encoding.ASCII.GetBytes(textBox1.Text);
            byte[] result = DoHash(type, source);

            textBox2.Text = Convert.ToBase64String(result);
            textBox3.Text = result.ToHexString();
        }
    }

    public static class Utils
    {
        public static String ToHexString(this byte[] value, bool space_delimiter = false)
        {
            string result = "";

            foreach (var item in value)
            {
                int int_val = Convert.ToInt32(item);
                result = result + string.Format("{0:X2} ", int_val);
            }

            result = result.TrimEnd(' ');

            if (!space_delimiter)   //如果不需要空格隔開=>把所有空格拿掉，顯示出來就會是 8A5BFC62 這樣
                result = result.Replace(" ", "");

            return result;
        }
    }

    public enum HASH_TYPE
    { 
        MD5 = 0,
        SHA1=1,
        SHA256=2,
        SHA384=3,
        SHA512=4,
    }
}
