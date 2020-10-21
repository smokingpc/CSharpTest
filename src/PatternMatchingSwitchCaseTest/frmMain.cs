using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatternMatchingSwitchCaseTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text;

            string pattern = "This is a book.";

            switch (input)
            {
                case string data when (pattern.IndexOf(input)>=0):
                    MessageBox.Show(data.ToString());
                    return;

                case "wwwwww" when (pattern.IndexOf(input) < 0):
                    MessageBox.Show("!?!?!?!?!?!?");
                    return;
                case "x" when (pattern.IndexOf(input) < 0):
                    MessageBox.Show("xxxx");
                    return;
            }

            MessageBox.Show("errrrrr");
        }
    }
}
