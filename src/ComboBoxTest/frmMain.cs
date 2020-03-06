using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComboBoxTest
{
    public partial class frmMain : Form
    {
        public CBindData MyBindData = new CBindData();

        public frmMain()
        {
            InitializeComponent();
        }
        private void SetupComboBox()
        {
            List<CMyData> list = new List<CMyData>();
            list.Add(new CMyData(1,"Kevin", "Addr1"));
            list.Add(new CMyData(3, "Roy", "2addr"));
            BindingList<CMyData> src = new BindingList<CMyData>(list);
            comboBox1.DataSource = src;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "SN";
        }

        private void SetupBinding()
        {
            //one-way binding
            var bind = new Binding("SelectedValue", MyBindData, "SN")
                {
                    DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged,
                    ControlUpdateMode = ControlUpdateMode.Never,        //don't update control
                };
            comboBox1.DataBindings.Add(bind);

            //one-way binding
            bind = new Binding("Text", MyBindData, "Name")
                {
                    DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged,
                    ControlUpdateMode = ControlUpdateMode.Never,        //don't update control
                };
            comboBox1.DataBindings.Add(bind);

            //one-way binding
            bind = new Binding("SelectedItem", MyBindData, "Data")
            {
                DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged,
                ControlUpdateMode = ControlUpdateMode.Never,        //don't update control
            };
            comboBox1.DataBindings.Add(bind);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SetupComboBox();
            SetupBinding();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = MyBindData.SN.ToString();
            textBox2.Text = MyBindData.Name;
            textBox3.Text = MyBindData.Data.ToString();
        }
    }

    public class CBindData
    { 
        public int SN { get; set; }
        public string Name { get; set; }
        public CMyData Data { get; set; }
    }

    public class CMyData
    { 
        public int SN { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public CMyData()
        { }
        public CMyData(int sn, string name, string addr)
        {
            this.SN = sn;
            this.Name = name;
            this.Address = addr;
        }
    }
}
