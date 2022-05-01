using System;
using System.Windows.Forms;
using DirectShowLib;


namespace WebCamViewAndCapture
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void InitComboList(DsDevice[] devlist)
        {
            comboBox1.Items.Clear();
            comboBox1.DisplayMember = "Name";
            
            comboBox1.Items.AddRange(devlist);
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var devlist = EnumerateCaptureDevices();
            InitComboList(devlist);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnStop.PerformClick();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartVideoStream((DsDevice)comboBox1.SelectedItem);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopVideoStream();
        }
    }
}
