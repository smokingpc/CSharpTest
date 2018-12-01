using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleDataSourceTest
{
    public partial class frmMain : Form
    {
        //public BindingList<CTestData> DataSource = new BindingList<CTestData>();
        public List<CTestData> GridData = new List<CTestData>();
        public List<CTestData> DBData = new List<CTestData>();
        public List<CTestData> FileData = new List<CTestData>();

        //public BindingList<CTestData> DBBinding = new BindingList<CTestData>();
        //public BindingList<CTestData> FileBinding = new BindingList<CTestData>();
        public frmMain()
        {
            InitializeComponent();
            //BindingSource src = new BindingSource();

            gridControl1.DataSource = this.GridData;

            
            //gridControl1.DataSource = new BindingList<CTestData>[2] { DBBinding, FileBinding };
            //DataSource.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadFileData();
            RefreshGridControl();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadDBData();
            ReloadFileData();
            RefreshGridControl();
        }

        private void LoadDBData()
        {
            string[] splitor = { "\r\n" };
            string[] lines = textBox1.Text.Split(splitor, StringSplitOptions.RemoveEmptyEntries);
            DBData.Clear();
            foreach (var item in lines)
            {
                DBData.Add(new CTestData(item));
            }
        }

        private void ReloadFileData()
        {
            string[] splitor = {"\r\n"};
            string[] lines = textBox2.Text.Split(splitor, StringSplitOptions.RemoveEmptyEntries);
            FileData.Clear();
            foreach (var item in lines)
            {
                FileData.Add(new CTestData(item));
            }
        }

        private void RefreshGridControl()
        {
            GridData.Clear();
            GridData.AddRange(DBData);
            GridData.AddRange(FileData);
            gridControl1.RefreshDataSource();
        }
    }



    public class CTestData
    {
        public string Phone { get; set; }
        public string Name { get; set; }

        public CTestData(string data)
        {
            var result = data.Split(',');
            Phone = result[0].Trim();
            Name = result[1].Trim();
        }
    }
}
