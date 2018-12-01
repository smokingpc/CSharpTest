using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace EnumTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ANTENNA antenna = (ANTENNA)Enum.Parse(typeof(ANTENNA), "天線");
            ANTENNA antenna = GetValueByDescription<ANTENNA>("假負載");
            textBox1.Text = string.Format("{0}", (int)antenna);
        }

        public T GetValueByDescription<T>(string desc)
        {
            var type = typeof(T);
            //FieldInfo[] fields = type.GetFields();
            //T value;

            foreach(FieldInfo field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                                typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == desc)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("description not found");
        }
    }

    public enum ANTENNA : int
    {
        [Description("假負載")]
        ANT_DUMMY = 0,
        [Description("天線")]
        ANT_REAL = 1
    }
}
