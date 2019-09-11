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

namespace DetectTxtFileEncodingTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void DetectEncoding(string filename)
        { 
            //Limitation : this function can only detect TXT file with BOM.
            //If no BOM, detection could be wrong...

            //因為ASCII file如果用UTF8 init encoding去偵測會誤判，所以先用系統預設 encoding
            using (StreamReader reader = new StreamReader(filename,Encoding.Default, true))
            {
                string msg =string.Format( "Encoding of specified file is [{0}]\r\nFirst Line is [{1}]",reader.CurrentEncoding.ToString(), reader.ReadLine());
                MessageBox.Show(msg);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
                {
                    Filter = "Text File (*.txt)|*.txt"
                };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dlg.FileName;
                DetectEncoding(dlg.FileName);
            }
        }
    }
}
