using System;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RegexTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(IsInputDataValid())
                MatchingRegex();
        }

        private bool IsInputDataValid()
        { 
            if ((textBox1.Text.Length > 0) && (textBox2.Text.Length > 0))
                return true;

            return false;
        }

        private void MatchingRegex()
        {
            string input = textBox1.Text.Trim();
            string regexp = textBox2.Text.Trim();
            Match match = Regex.Match(input, regexp);

            if(match.Success)
            {
                textBox3.AppendText("Regexp matched!\r\n");
                if(match.Groups.Count > 0)
                { 
                    for(var i=0; i< match.Groups.Count; i++)
                    {
                        textBox3.AppendText(string.Format("Group[{0}]={1}\r\n", i, match.Groups[i]));
                    }
                }
            }
            else
                textBox3.AppendText("Regexp NOT match!\r\n");
        }
    }
}
