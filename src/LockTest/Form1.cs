using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LockTest
{
    public partial class Form1 : Form
    {
        private Object Lock = new Object();

        public Form1()
        {
            InitializeComponent();
        }

        private void Test()
        {
            if (!Monitor.IsEntered(this.Lock) && Monitor.TryEnter(this.Lock))
            {
                Test();
                Monitor.Exit(this.Lock);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Test();
        }
    }
}
