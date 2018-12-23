using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatasetDesignerTest
{
    public partial class frmMain : Form
    {
        private string MyConnStr1 = "";
        private string MyConnStr2 = "";
        private static readonly string ExePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database2DataSet.TEST' table. You can move, or remove it, as needed.
            //this.tESTTableAdapter1.Fill(this.database2DataSet.TEST);

            string ip = "192.168.20.13";
            string db = "TEST";
            int timeout = 5;
            string uid = "test";
            string pwd = "5678";
            MyConnStr1 = string.Format("Data Source={0};Initial Catalog={1};Connection Timeout={2};User ID={3};Password={4}",
                        ip, db, timeout, uid, pwd);

            MyConnStr2 = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\{1}",
                        ExePath, "Database2.accdb");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'tESTDataSet1.TEST' table. You can move, or remove it, as needed.
            //竄改DataDesigner connection string可以直接在這邊改....
            TestTableAdapter.Connection.ConnectionString = MyConnStr1;

            TestBindingSource.DataSource = TestDataSet1;
            TestBindingSource.DataMember = "TEST";  //把連結綁回去要記得填這欄，指定你要看哪個table的資料
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(
                new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "ID",
                    HeaderText = "ID",
                    Name = "ColumnID",
                    ReadOnly = true,
                }
                );
            dataGridView1.Columns.Add(
                new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "MYNAME",
                    HeaderText = "MyName",
                    Name = "ColumnMyName",
                    ReadOnly = true,
                }
                );

            dataGridView1.DataSource = TestBindingSource;
            this.TestTableAdapter.Fill(this.TestDataSet1.TEST);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //把 binding source 的連結斷開，Fill Table的行為就不會因為連不上server而exception
            TestTableAdapter.Connection.ConnectionString = "";
            TestBindingSource.DataSource = null;
            dataGridView1.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //竄改DataDesigner connection string可以直接在這邊改....
            tESTTableAdapter1.Connection.ConnectionString = MyConnStr2;

            tESTBindingSource1.DataSource = database2DataSet;
            tESTBindingSource1.DataMember = "TEST";  //把連結綁回去要記得填這欄，指定你要看哪個table的資料
            dataGridView1.DataSource = tESTBindingSource1;

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(
                new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "識別碼",
                    HeaderText = "識別碼",
                    Name = "Column識別碼",
                    ReadOnly = true,
                }
                );
            dataGridView1.Columns.Add(
                new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "MY_NAME",
                    HeaderText = "MyName",
                    Name = "ColumnMyName",
                    ReadOnly = true,
                }
                );

            this.tESTTableAdapter1.Fill(database2DataSet.TEST);
        }
    }
}
