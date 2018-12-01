using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnumGridTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            repositoryItemImageComboBox1.Items.AddEnum(typeof(LockStatus), true);
            sqlDataSource1.Fill();
        }
    }

    public enum LockStatus
    {
        [Description("------")]
        NoDataForDisplay = -1,
        [Description("鎖定")]
        Locked = 0,
        [Description("未鎖定")]
        Unlock = 1,
    }
    public enum GENERIC_STATE
    {
        [Description("------")]
        UNKNOWN = -1,
        [Description("正常")]
        NORMAL = 0,
        [Description("警告")]
        WARNING = 1,
        [Description("異常")]
        ERROR = 2,
    }
}
