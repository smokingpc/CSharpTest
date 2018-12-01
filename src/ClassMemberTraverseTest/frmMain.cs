using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMemberTraverseTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Traverse()
        {
            PropertyInfo[] properties = typeof(CSite).GetProperties();
            
            WriteLog("======== PropertyInfo ========");
            foreach (var item in properties)
            {
                string scope = "Public";
                if (!item.PropertyType.IsPublic)
                    scope = "NON public";

                string description = "";
                var attribute = item.GetCustomAttribute<DescriptionAttribute>();
                if(attribute != null)
                    description = item.GetCustomAttribute<DescriptionAttribute>().Description;
                
                string msg = string.Format("[{0}] => Type={1}, MemberType={2}, Scope={3}, Description={4}", 
                            item.Name, item.MemberType, item.MemberType.ToString(), scope, description);
                
                WriteLog(msg);
            }

            WriteLog("\r\n");
            WriteLog("======== FieldInfo ========");
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.ExactBinding;
            FieldInfo[] fields = typeof(CSite).GetFields(flags);
            foreach (var item in fields)
            {
                string scope = "Public";
                if (item.IsPrivate)
                    scope = "Private";

                string description = "";
                var attribute = item.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                    description = item.GetCustomAttribute<DescriptionAttribute>().Description;

                string msg = string.Format("[{0}] => MemberType={1}, Scope={2}, Description={3}",
                            item.Name, item.MemberType, scope, description);

                WriteLog(msg);
            }

            WriteLog("\r\n");
            WriteLog("======== MemberInfo ========");
            flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.ExactBinding;
            MemberInfo[] members = typeof(CSite).GetMembers(flags);
            foreach (var item in members)
            {
                string scope = "Public";
                if (!item.ReflectedType.IsPublic)
                    scope = "NON public";

                string description = "";
                var attribute = item.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                    description = item.GetCustomAttribute<DescriptionAttribute>().Description;

                string msg = string.Format("[{0}] => MemberType={1}, RefType={2} Scope={3}, Description={4}",
                            item.Name, item.MemberType, item.ReflectedType.GetType(), scope, description);

                WriteLog(msg);
            }
        }

        private void WriteLog(string msg)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke((Action)(() => { WriteLog(msg); }));
            }
            else
            {
                textBox1.AppendText(msg + "\r\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            Traverse();
        }
    }

    public class CSite
    {
        [Description("資料庫序號")]
        public Int64 SN { get; set; }

        public string TestProperty1 { get { return "1"; } }
        public string TestProperty2 { set { PrivateData = value; } }
        
        public string PublicData = "";
        private string PrivateData = "";

        [Description("SN")]
        private Int64 _SN = 0;
    }
}
