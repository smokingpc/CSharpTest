using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringFormatTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //float data = float.Parse(textBox1.Text);
            int data = int.Parse(textBox1.Text);
            textBox3.Text = data.ToString();
            //textBox3.Text = data.ToString(textBox2.Text);
            //textBox3.Text = string.Format(textBox2.Text, data);
        }
    }
}
