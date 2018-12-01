using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZipFromTempFolderTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            var temp_path = CreateTempPath();
            CreateTestFile(temp_path);
            ZipTestPath(temp_path);
        }
    }
}
