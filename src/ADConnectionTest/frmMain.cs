using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.DirectoryServices.AccountManagement;

namespace ADConnectionTest
{
    //用來測試AD Server或LDAP Server連線，原理是給定一個OP帳號，用它去查詢另一個帳號或物件。
    //其實在 IsADUserExist() 裡面，只要 domainContext 正確取得也沒有Exception，這樣就代表有連上Server了。

    //會這樣測試是因為很多AD或LDAP方面的應用都是這個模式：
    //利用一個帳號去查詢AD或LDAP裡面其他的物件，取得資訊等等`.

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string op_id = textBox1.Text;
            string op_pwd = textBox2.Text;
            string ad_server = textBox3.Text;
            string target_uid = textBox4.Text;
            IdentityType type = (IdentityType)Enum.Parse(typeof(IdentityType), comboBox1.SelectedItem.ToString());
            bool exist = IsADUserExist(op_id, op_pwd, ad_server, type, target_uid);
            string msg = string.Format("目標 [{0}] ({1}) ", target_uid, type.ToString());

            if (exist)
                MessageBox.Show(msg + "存在");
            else
                MessageBox.Show(msg + string.Format("在 AD Server [{0}] 找不到", ad_server));
        }

        private bool IsADUserExist(string op_id, string op_pwd, string ad_server, IdentityType account_type, string account)
        {
            bool result = false;

            try
            {
                using (var domainContext = new PrincipalContext(ContextType.Domain, ad_server, op_id, op_pwd))//需要有一組在網域內的帳號密碼才能查詢
                {
                    using (var foundUser = UserPrincipal.FindByIdentity(domainContext, account_type, account))
                    {
                        if (null != foundUser && foundUser.Enabled.HasValue)
                            result = true;
                    }
                }
            }
            catch (PrincipalServerDownException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }


            return result;
        }

        private void InitUI()
        {
            comboBox1.Items.Add(IdentityType.DistinguishedName.ToString());
            comboBox1.Items.Add(IdentityType.Guid.ToString());
            comboBox1.Items.Add(IdentityType.Name.ToString());
            comboBox1.Items.Add(IdentityType.SamAccountName.ToString());
            comboBox1.Items.Add(IdentityType.Sid.ToString());
            comboBox1.Items.Add(IdentityType.UserPrincipalName.ToString());
            comboBox1.SelectedIndex = 3;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitUI();
        }
    }
}
