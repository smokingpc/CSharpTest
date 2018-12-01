using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSystemWatcherTest
{
    public partial class frmMain : Form
    {
        private string ExePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private string FileToWatch = "*.txt";
        FileSystemWatcher Watcher = new FileSystemWatcher();

        public frmMain()
        {
            InitializeComponent();
        }

        private void StartWatch()
        {
            Watcher.Filter = FileToWatch;
            Watcher.Path = ExePath;
            Watcher.Created += this.OnNewFileCreated;
            Watcher.Changed += this.OnFileChanged;
            Watcher.Renamed += this.OnFileRenamed;
            Watcher.Deleted += this.OnFileDeleted;
            Watcher.EnableRaisingEvents = true;
        }

        private void StopWatch()
        {
            Watcher.EnableRaisingEvents = false;
        }


        #region ======== EventHandler of FileWatcher ========
        private void OnNewFileCreated(object sender, System.IO.FileSystemEventArgs e)
        { 
            string msg = string.Format("發現新檔案：{0}", e.FullPath);
            MessageBox.Show(msg);
        }
        private void OnFileChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            string msg = string.Format("檔案：{0} 被改變了！\r\nChange Type ={1}", e.FullPath, e.ChangeType);
            MessageBox.Show(msg);
        }
        private void OnFileRenamed(object sender, System.IO.RenamedEventArgs e)
        {
            string msg = string.Format("{0} \r\n被改名為 \r\n{1}", e.OldFullPath, e.FullPath);
            MessageBox.Show(msg);
        }
        private void OnFileDeleted(object sender, System.IO.FileSystemEventArgs e)
        {
            string msg = string.Format("[{0}] \r\n被砍掉了", e.FullPath);
            MessageBox.Show(msg);
        }

        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            StartWatch();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopWatch();
        }
    }
}
