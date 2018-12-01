using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void ShowInstalledPrinters()
        {
            comboBox1.Items.Clear();
            
            //installed printers不知道是每次呼叫時會自動更新？還是以app啟動時偵測的印表機清單為準？
            foreach (string name in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox1.Items.Add(name);
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowInstalledPrinters();
            this.reportViewer1.RefreshReport();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowInstalledPrinters();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PreivewReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintWithPreview(comboBox1.SelectedItem.ToString());
        }
    }
}
