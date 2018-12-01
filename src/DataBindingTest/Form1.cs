using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBindingTest
{
    public partial class Form1 : Form
    {
        //BindingList<CTestData> DataSource = new BindingList<CTestData>();
        CTestData Item = new CTestData()
        {
            Name = "財田明神",
            Phone = "02-945794狂",
        };
        Binding ctx = null;

        public Form1()
        {
            InitializeComponent();
            ctx = new Binding("Text", Item, "Phone", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //DataSource.Add(Item);

            //Binding()  三參數：
            //第一個是 Target Object 要bind的property，例如把資料綁到畫面UI的TextBox來顯示，
            //  那就指定 TextBox裡面的 Text => 所以要寫 "Text"
            //第二個是 source object
            //第三個是 source object 的 property name。
            //本例想把 CTestData::Name欄位去跟UI上的TextBox綁定
            // =>所以參數1 是"Text", 參數2 是資料物件, 參數3 是"Name"
            //DataSourceUpdateMode.OnPropertyChanged 指定TextBox每次變更時都會直接變更binding source object
            textBox1.DataBindings.Add(new Binding("Text", Item, "Name", false, DataSourceUpdateMode.OnPropertyChanged));
            textBox3.DataBindings.Add(ctx);

            //dataGridView1.DataSource = DataSource;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Item.Phone = textBox2.Text;
            var test = textBox3.BindingContext;
            //DataBinding的行為控制靠 Binding 這個class的物件
            //ctx.ReadValue();
        }
    }

    public class CTestData
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
