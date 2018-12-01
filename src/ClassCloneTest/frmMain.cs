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

namespace ClassCloneTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CTest obj1 = new CTest(1);
            //obj1.BaseData1 = 1111;
            //obj1.BaseData2 = "obj1 BaseData2";
            //obj1.BaseData4 = 4444;
            //obj1.ChildData1 = 1111;
            //obj1.ChildData2 = "obj1 Child2";
            //obj1.ChildData4 = "obj1 child4";

            CTest obj2 = new CTest(2);
            //obj2.BaseData1 = 2222;
            //obj1.BaseData2 = "obj1 BaseData2";
            //obj2.BaseData4 = 8888;
            //obj2.ChildData1 = 2222;
            //obj2.ChildData4 = "obj2 child4";
            ObjTools.CopyMembers(obj1, obj2);
        }
    }


    public abstract class CBaseClass
    {
        public int BaseData1 { get; set; }
        public string BaseData2 = "Base2";
        protected string BaseData3 = "Base3";
        
        public int BaseData4 { get { return _BaseData4; } }
        protected int _BaseData4 = 11;

        public abstract CBaseClass Clone();
        public abstract void CopyFrom(CBaseClass old);
    }

    public class CTest : CBaseClass
    {
        public int ChildData1 { get; set; }
        public string ChildData2 = "Child2";
        private string ChildData3 = "Child3";

        public string ChildData4 { set { _ChildData4=value; } }
        private string _ChildData4 = "Child4";
        public CTest(int init_val) 
        {
            BaseData1 = init_val * 10;
            BaseData2 = "BaseData2 " + init_val.ToString();
            BaseData3 = "BaseData3 " + init_val.ToString();
            _BaseData4 = init_val * 10;

            ChildData1 = init_val * 10;
            ChildData2 = "ChildData2 " + init_val.ToString();
            ChildData3 = "ChildData3 " + init_val.ToString();
            _ChildData4 = "ChildData4 " + init_val.ToString();
        }
        public override CBaseClass Clone()
        {
            return (CTest)this.MemberwiseClone();
        }
        public override void CopyFrom(CBaseClass old)
        {
            ObjTools.CopyMembers(old, this);
        }
    }

    public class ObjTools
    {
        public static string ToHexString(byte[] value, bool space_delimiter = true)
        {
            string result = "";

            foreach (var item in value)
            {
                int int_val = Convert.ToInt32(item);
                result = result + string.Format("{0:X2} ", int_val);
            }

            result = result.TrimEnd(' ');

            if (!space_delimiter)   //如果不需要空格隔開=>把所有空格拿掉，顯示出來就會是 8A5BFC62 這樣
                result = result.Replace(" ", "");

            return result;
        }

        public static void CopyMembers(object srcobj, object dstobj)
        {
            CopyProperties(srcobj, dstobj);
            CopyFields(srcobj, dstobj);
        }
        public static void CopyProperties(object srcobj, object dstobj)
        {
            List<PropertyInfo> src_list = new List<PropertyInfo>();
            List<PropertyInfo> dst_list = new List<PropertyInfo>();
            //BindingFlags src_flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty;
            //BindingFlags dst_flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.ExactBinding;//| BindingFlags.SetProperty;

            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic;
            //srcobj.GetType().GetProperties
            src_list.AddRange(srcobj.GetType().GetProperties());
            dst_list.AddRange(dstobj.GetType().GetProperties());

            //把兩個物件裡的property翻出來比對，同名的property就copy value
            //(同一個class裏不會有兩個同名的data property)
            foreach (var copy_dest in dst_list)
            {
                var copy_source = src_list.First(o => o.Name == copy_dest.Name);
                if (copy_source != null)
                {
                    //檢查一下read / write狀況，例如只有get沒有set的property，其CanWrite欄位就是 false
                    if(copy_source.CanRead && copy_dest.CanWrite)
                        copy_dest.SetValue(dstobj, copy_source.GetValue(srcobj));
                }
            }
        }
        public static void CopyFields(object srcobj, object dstobj)
        {
            List<FieldInfo> src_list = new List<FieldInfo>();
            List<FieldInfo> dst_list = new List<FieldInfo>();
            BindingFlags src_flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.ExactBinding;
            BindingFlags dst_flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.ExactBinding;
            //BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic;
            src_list.AddRange(srcobj.GetType().GetFields(src_flag));
            dst_list.AddRange(dstobj.GetType().GetFields(dst_flag));

            //把兩個物件裡的property翻出來比對，同名的property就copy value
            //(同一個class裏不會有兩個同名的data field)
            foreach (var copy_dest in dst_list)
            {
                var copy_source = src_list.First(o => o.Name == copy_dest.Name);
                if (copy_source != null)
                {
                    //if (copy_source.CanRead && copy_dest.CanWrite)
                        copy_dest.SetValue(dstobj, copy_source.GetValue(srcobj));
                }
            }
        }
    }
}
