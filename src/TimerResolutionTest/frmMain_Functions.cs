using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace TimerResolutionTest
{
    public partial class frmMain
    {
        private System.Threading.Timer ThreadTimer = null;
        private System.Timers.Timer SystemTimer = null;
        private Stopwatch Watcher = new Stopwatch();
        private List<string> LogList = new List<string>();
        private int Loop = -1;
        private int OldResolution = -1;

        private void TestThreadTimer(int loop, int interval,  bool high_precision)
        {
            int nt_state = -1;
            if (high_precision)
            {
                nt_state = NtSetTimerResolution(3900, true, ref OldResolution);
                LogList.Add("NTState of SetTimerResolution=" + nt_state.ToString());
            }

            Watcher.Reset();
            Watcher.Start();
            this.Loop = loop;
            ThreadTimer = new System.Threading.Timer(ThreadTimerCallback, null, 0, interval);
        }
        
        private void ThreadTimerCallback(object state)
        {
            Watcher.Stop();
            string msg = string.Format("Thread Timer: elapsed time = {0}ms", Watcher.ElapsedMilliseconds);
            Watcher.Reset();
            LogList.Add(msg);
            Loop--;

            //loop跑完了，可以停下timer了
            if (Loop <= 0)
            {
                //Watcher.Stop();
                //string msg = string.Format("Thread Timer: elapsed time = {0}ms", Watcher.ElapsedMilliseconds);
                //LogList.Add(msg);

                ThreadTimer.Change(0, Timeout.Infinite);
                ThreadTimer.Dispose();
                ThreadTimer = null;
                int res = -1;
                NtSetTimerResolution(OldResolution, true, ref res);

                DumpLog();
            }
            else
                Watcher.Start();
        }

        private void TestSystemTimer(int loop, int interval, bool high_precision)
        {
            int nt_state = -1;
            if (high_precision)
            {
                nt_state = NtSetTimerResolution(3900, true, ref OldResolution);
                LogList.Add("NTState of SetTimerResolution=" + nt_state.ToString());
            }

            Watcher.Reset(); 
            this.Loop = loop;
            SystemTimer = new System.Timers.Timer(interval);
            SystemTimer.Elapsed += SystemTimerCallback;
            Watcher.Start();
            SystemTimer.Enabled = true;
        }
        private void SystemTimerCallback(object sender, ElapsedEventArgs e)
        {
            Watcher.Stop();
            string msg = string.Format("System Timer: elapsed time = {0}ms", Watcher.ElapsedMilliseconds);
            LogList.Add(msg);
            Watcher.Reset();
            Loop--;

            //loop跑完了，可以停下timer了
            if (Loop <= 0)
            {
                //Watcher.Stop();
                //string msg = string.Format("System Timer: elapsed time = {0}ms", Watcher.ElapsedMilliseconds);
                //LogList.Add(msg);

                SystemTimer.Enabled = false;
                SystemTimer.Dispose();
                SystemTimer = null;

                int res = -1;
                NtSetTimerResolution(OldResolution, true, ref res);
                
                DumpLog();
            }
            else
                Watcher.Start();
        }

        private void DumpLog()
        {
            foreach (var log in LogList)
            {
                SetText(textBox2, log);
            }
            LogList.Clear();
        }
    }
}
