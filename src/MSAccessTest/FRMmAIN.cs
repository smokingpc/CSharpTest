using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MSAccessTest
{
    public partial class frmMain : Form
    {
        private string ExePath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        private string DsnFormat = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};";
        private string DSN = "";

        private OleDbConnection Connection;
        private OleDbCommand Command;
        private List<CTestUser> UserList = new List<CTestUser>();
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void InitDatabase()
        {
            string filename = ExePath + "\\testdb.accdb";
            DSN = string.Format(DsnFormat, filename);
            string SQL = "";

            int ret = 0;
            //SQL = "INSERT INTO USER([USERNAME], [ADDRESS], [PHONE]) VALUES('海綿寶寶', '蟹堡王', '0800-092000'); SELECT SCOPE_IDENTITY();";
            var SQL1 = "INSERT INTO [USER] (USERNAME, ADDRESS, PHONE) VALUES ('海綿寶寶', '蟹堡王', '0800-092000')";
            var SQL2 = "SELECT @@Identity";
            using (OleDbCommand cmd = new OleDbCommand(SQL1, new OleDbConnection(DSN)))
            {
                //cmd不像adapter，不會自己開啟connection....
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                //MSAccess一條statement不能執行兩個命令
                //所以要拆開做，另外 SELECT @@Identity 只能取得 CurrentConnection 內的 identity
                //如果 connection closed 這值就會重算.....
                cmd.CommandText = SQL2;
                ret = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
            }

            SQL = "SELECT * FROM [USER]";
            using (DataTable table = new DataTable())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(SQL, DSN))
                {
                    adapter.Fill(table);
                }

                foreach (DataRow row in table.Rows)
                {
                    //CJamCmd item = new CJamCmd(row);
                    CTestUser item = new CTestUser();
                    item.Id = Convert.ToInt32(row["ID"].ToString());
                    item.Name = row["UserName"].ToString();
                    item.Address = row["Address"].ToString();
                    item.Phone = row["Phone"].ToString();
                    UserList.Add(item);
                }
            }
            gridControl1.DataSource = UserList;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitDatabase();
            gridControl1.RefreshDataSource();
        }
    }

    public class CTestUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
