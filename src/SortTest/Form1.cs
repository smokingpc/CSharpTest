using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<CMyData> list = new List<CMyData>()
            { 
             new CMyData(2, "A"),
             new CMyData(7, "B"),
             new CMyData(5, "C"),
            };

            //Acending sort
            list.Sort((prev, next) => prev.ID.CompareTo(next.ID));
            //Decending sort
            list.Sort((prev, next) => next.ID.CompareTo(prev.ID));
        }
    }

    internal class CMyData
    {
        public int ID = 0;
        public string Name = "";

        public CMyData(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
