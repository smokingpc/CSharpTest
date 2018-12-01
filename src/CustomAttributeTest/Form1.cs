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

namespace CustomAttributeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            var obj = new CTest();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.ExactBinding;
            var fields = typeof(CTest).GetFields();
            foreach (var item in fields)
            {
                var att1 = item.GetCustomAttribute<DBFieldAttribute>();
                if (att1 != null)
                {
                    //MessageBox.Show(att1.ColumnName);
                }
                var att2 = item.GetCustomAttribute<DescriptionAttribute>();
                if (att2 != null)
                {
                    //MessageBox.Show(att2.Description);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DBFieldAttribute : Attribute
    {
        public string ColumnName { get { return _ColumnName; } }
        private string _ColumnName = string.Empty;

        public DBFieldAttribute(string name)
        {
            _ColumnName = name;
        }
    }

    public class CTest
    {
        [DBField("SN1")]
        [Description("TestDesc1")]
        public string Data1 = string.Empty;
        [DBField("SN2")]
        [Description("TestDesc2")]
        public string Data2 = string.Empty;
        [DBField("SN3")]
        [Description("TestDesc3")]
        public string Data3 = string.Empty;
        [DBField("SN4")]
        [Description("TestDesc4")]
        public string Data4 = string.Empty;
        [DBField("SN5")]
        [Description("TestDesc5")]
        public string Data5 = string.Empty;
        [DBField("SN6")]
        [Description("TestDesc6")]
        public string Data6 = string.Empty;
        [DBField("SN7")]
        [Description("TestDesc7")]
        public string Data7 = string.Empty;
        [DBField("SN8")]
        [Description("TestDesc8")]
        public string Data8 = string.Empty;
        [DBField("SN9")]
        [Description("TestDesc9")]
        public string Data9 = string.Empty;
        [DBField("SN10")]
        [Description("TestDesc10")]
        public string Data10 = string.Empty;

        public static List<MemberInfo> Fields = new List<MemberInfo>();
        public static List<PropertyInfo> Properties = new List<PropertyInfo>();

        public CTest() 
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.ExactBinding;
            var fields = typeof(CTest).GetFields(flag);
            foreach (var item in fields)
            {
                var att1 = item.GetCustomAttribute<DBFieldAttribute>();
                if (att1 != null)
                {
                    Fields.Add(item);
                    //MessageBox.Show(att1.ColumnName);
                }
            }

            flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetProperty | BindingFlags.ExactBinding;
            var properties = typeof(CTest).GetProperties(flag);
            foreach (var item in properties)
            {
                var att1 = item.GetCustomAttribute<DBFieldAttribute>();
                if (att1 != null)
                {
                    Properties.Add(item);
                    //MessageBox.Show(att1.ColumnName);
                }
            }
        }
    }
}
