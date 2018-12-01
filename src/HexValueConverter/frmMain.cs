using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexValueConverter
{
    public partial class frmMain : Form
    {
        public static readonly string HEX_MASK = "";        //16進位數字的INPUT MASK
        public static readonly string FLOAT_MASK = "";      //數字帶小數的INPUT MASK
        public static readonly string INTEGER_MASK = "";    //整數數字的INPUT MASK

        public frmMain()
        {
            InitializeComponent();
        }

        private void InitUI()
        {
            radioButton1.Checked = true;
        }

        private void UpdateUIEffect()
        {
            comboBox1.DataSource = Enum.GetValues(typeof(VALUE_TYPE));
            comboBox1.SelectedIndex = 1;
        }

        private void ClearInputs() 
        {
            textBox1.Text = textBox2.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitUI();
            UpdateUIEffect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox2.Text = ConvertValues(textBox1.Text, (VALUE_TYPE)comboBox1.SelectedValue);
            }
            else
            {
                if(VALUE_TYPE.INT32_INT64 == (VALUE_TYPE)comboBox1.SelectedValue)
                {
                    var input = Int64.Parse(textBox1.Text);
                    textBox2.Text = ConvertValues(input);
                }
                else if (VALUE_TYPE.FLOAT_DOUBLE == (VALUE_TYPE)comboBox1.SelectedValue)
                {
                    var input = Double.Parse(textBox1.Text);
                    textBox2.Text = ConvertValues(input);
                }
            }
        }
    }

    public enum VALUE_TYPE 
    { 
        [Description("INT32 / INT64")]
        INT32_INT64 = 1,
        [Description("FLOAT / DOUBLE")]
        FLOAT_DOUBLE = 2,
    }
}
