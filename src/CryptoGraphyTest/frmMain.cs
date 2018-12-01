using System;
using System.Windows.Forms;

namespace CryptoGraphyTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AESTest();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AESCngTest();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SHA1Test();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SHA256Test();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DESTest();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
    }
}
