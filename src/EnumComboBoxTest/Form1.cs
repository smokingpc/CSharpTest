using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnumComboBoxTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new List<TEST_ENUM>(Enum.getv(typeof(TEST_ENUM)));
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public enum TEST_ENUM
    { 
        [Description("第一項")]
        ITEM1 = 1,
        [Description("第二項")]
        ITEM2 = 2,
    }
}
