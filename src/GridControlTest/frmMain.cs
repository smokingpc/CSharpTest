using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridControlTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void InitUI()
        {
            BindingList<CMyData> DataList = new BindingList<CMyData>();
            //List<CMyData> DataList = new List<CMyData>();

            var data = new CMyData() { Name = "AA", Age = 11, ID = "A123456789" };
            DataList.Add(data);

            data = new CMyData() { Name = "XXXXXXXXXX", Age = 22, ID = "SB" };
            DataList.Add(data);

            data = new CMyData() { Name = "CCCCCC", Age = 33, ID = "Z987654321" };
            DataList.Add(data);
            DataList.AllowNew = DataList.AllowRemove = true;

            gridControl1.DataSource = DataList;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //gridControl1.MainView = gridView1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //gridControl1.MainView = cardView1;
            
        }
    }

    public class CMyData
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public int Age { get; set; }

        public CMyData() { }
    }
}
