using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectShowLib;

namespace WebCamLiveTest
{
    public partial class frmMain : Form
    {
        private CEasyVideoIn VideoIn = null;// new CEasyVideoIn();

        public frmMain()
        {
            InitializeComponent();
        }

        public void InitDeviceList()
        {
            cbDeviceList.Items.Clear();
            cbDeviceList.DisplayMember = "Name";
            var devlist = CEasyVideoIn.GetVideoInDevices();
            cbDeviceList.Items.AddRange(devlist);

            if (cbDeviceList.Items.Count > 0)
                cbDeviceList.SelectedIndex = 0;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitDeviceList();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != VideoIn)
                VideoIn.Dispose();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (null != VideoIn)
            {
                VideoIn.Stop();
                VideoIn.Dispose();
                VideoIn = null;
            }

            var device = (DsDevice)cbDeviceList.SelectedItem;
            VideoIn = new CEasyVideoIn(device, VideoPanel.Handle, VideoPanel.ClientRectangle);
            VideoIn.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (null == VideoIn)
                return;
            
            VideoIn.Stop();
            VideoIn.Dispose();
            VideoIn = null;
        }

        private void VideoPanel_SizeChanged(object sender, EventArgs e)
        {
            if (null == VideoIn)
                return;

            VideoIn.ResetRenderWindow(VideoPanel.Handle, VideoPanel.ClientRectangle);
        }
    }
}
