using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsEventLogTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string source = textBox1.Text;
            string logger = comboBox1.Text;
            string msg = textBox3.Text;

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logger);
            }

            var log = new EventLog(logger, ".", source);
            
            log.WriteEntry(msg, EventLogEntryType.Error, 1234, 5678);
        }
    }
}
