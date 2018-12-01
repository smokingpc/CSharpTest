using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DateTimePickerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string now = DateTime.Now.ToLongTimeString();
            timeEdit1.EditValue = DateTime.Parse(now);
            dateEdit1.EditValue = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = dateTimePicker1.Value.ToString();

        }
    }
}
