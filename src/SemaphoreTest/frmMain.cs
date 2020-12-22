using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemaphoreTest
{
    public partial class frmMain : Form
    {
        private int ThreadCount = 0;
        private int WaitCount = 0;
        private int LockCount = 0;
        private Semaphore SemaphoreLock = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = int.Parse(textBox1.Text);
            ThreadCount = WaitCount = LockCount = 0;
            int max_semaphore = int.Parse(textBox2.Text);
            SemaphoreLock = new Semaphore(max_semaphore, max_semaphore, "MyTestSemaphore");

            for (int i = 0; i < count; i++)
                Task.Run(DoJob);
        }

        private void DoJob()
        {
            int count1 = Interlocked.Increment(ref ThreadCount);
            int count2 = Interlocked.Increment(ref WaitCount);
            textBox3.AppendLine($"[EnterThread] Thread Count ={count1}, WaitCount={count2}");

            //Accquire Semaphore. If max count reached , this method will block until
            //semaphore count < max semaphore.
            SemaphoreLock.WaitOne();

            count2 = Interlocked.Decrement(ref WaitCount);
            int count3 = Interlocked.Increment(ref LockCount);
            textBox3.AppendLine($"[EnterSemaphore] Wait Count={count2}, Locked Count ={count3}");
            Thread.Sleep(3000);

            //Release accquired Semaphore.
            SemaphoreLock.Release();
            count1 = Interlocked.Decrement(ref ThreadCount);
            count3 = Interlocked.Decrement(ref LockCount);
            textBox3.AppendLine($"[LeaveThread] Thread Count ={count1}, Locked Count ={count3}");
        }
    }

    public static class Extension 
    {
        public static void AppendLine(this TextBox tb, string msg)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { tb.AppendLine(msg); }));
            }
            else
            {
                tb.AppendText(msg + "\r\n");
            }
        }
    }
}
