using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLServerTest
{
    public partial class Form1 : Form
    {
        string SQL = "select name from sys.tables";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //BindingSource source = new BindingSource();
            string connstr = string.Format("Data Source={0}:{1}; Initial Catalog={2};User ID={3};Password={4}", 
                                    textBox1.Text, 
                                    textBox2.Text,
                                    textBox5.Text,
                                    textBox3.Text,
                                    textBox4.Text);
            string SQL = "select name from sys.tables";
            SqlConnection connection = new SqlConnection(connstr);
            SqlDataAdapter adapter = new SqlDataAdapter(SQL, connection);
            DataSet ds = new DataSet();

            connection.Open();
            adapter.Fill(ds);
            connection.Close();

            dataGridView1.DataSource = ds;
            dataGridView1.Refresh();

            connection = null;
            adapter = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }
    }
}
