using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputMaskTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var data = 1.0f;
            var msg = data.ToString("F1");
            textEdit2.Text = msg;
            data = Convert.ToSingle(textEdit2.Text);
        }
    }
}
