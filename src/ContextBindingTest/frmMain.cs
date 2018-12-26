using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContextBindingTest
{
    public partial class frmMain : Form
    {
        private string MyConnStr = "";
        private static readonly string ExePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        private object NewObj = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void InitDB()
        {
            MyConnStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\{1}",
                            ExePath, "Database2.accdb");
            
            MyData_TableAdapter.Connection.ConnectionString = MyConnStr;
        }

        private void BindContext()
        { }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitDB();

            this.MyData_TableAdapter.Fill(this.database2DataSet.MY_DATA);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                MyData_BindingSource.Position = e.RowIndex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyData_BindingSource.AddNew();

            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = checkBox1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyData_BindingSource.EndEdit();
            //endedit後 dataset.Table 就會更新，要把更新後的東西sync回DB
            this.MyData_TableAdapter.Update(this.database2DataSet.MY_DATA);
            this.MyData_TableAdapter.Fill(this.database2DataSet.MY_DATA);
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = checkBox1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MyData_BindingSource.Position >= 0)
            {
                textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = checkBox1.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MyData_BindingSource.CancelEdit();
            this.MyData_TableAdapter.Update(this.database2DataSet.MY_DATA);
            this.MyData_TableAdapter.Fill(this.database2DataSet.MY_DATA);
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = checkBox1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.SelectedRows;

            foreach (DataGridViewRow row in rows)
            {
                dataGridView1.Rows.Remove(row);
            }

            this.MyData_TableAdapter.Update(this.database2DataSet.MY_DATA);
            this.MyData_TableAdapter.Fill(this.database2DataSet.MY_DATA);
        }
    }
}
