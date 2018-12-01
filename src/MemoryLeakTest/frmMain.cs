using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using NAudio;
using NAudio.Wave;

namespace MemoryLeakTest
{
    //這個實驗主要是因為跨thread傳buffer時發生了memeryleak 的詭異現象，
    //只好實驗一下GC的隱藏機制到底是怎樣？

    public partial class frmMain : Form
    {
        private string TestDir = "D:\\MemTest";
        System.Threading.Thread WorkerThread = null;
        int Counter;
        ConcurrentQueue<CMyTask> TaskQueue = new ConcurrentQueue<CMyTask>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateTextBox(textBox1, "", false);
            if (!Directory.Exists(TestDir))
                Directory.CreateDirectory(TestDir);
            
            WorkerThread = new System.Threading.Thread(DoTest);
            WorkerThread.Start();
            
        }
        private void DoTest()
        {
            DoTaskRunTest(1000 * 10);

            while (Counter > 0)
            {
                System.Threading.Thread.Sleep(500);
                float size = Convert.ToSingle(GC.GetTotalMemory(false)) / 1048576;
                string msg = string.Format("Counter={0}, GC total mem ={1}MB \r\n", Counter, size);
                UpdateTextBox(textBox1, msg);
            }

            UpdateTextBox(textBox1, "============\r\n");
            DoProducerTest(1000 * 10);

            while (Counter > 0)
            {
                System.Threading.Thread.Sleep(500);
                float size = Convert.ToSingle(GC.GetTotalMemory(false)) / 1048576;
                string msg = string.Format("Counter={0}, GC total mem ={1}MB \r\n", Counter, size);
                UpdateTextBox(textBox1, msg);
            }
        }

        private void DoProducerTest(int times)
        {

            for (int i = 0; i < times; i++)
            {
                CMyTask task = new CMyTask();
                for (int j = 0; j < 50 * 3; j++)
                {
                    var data = new byte[720];
                    task.BufferList.Add(data);
                }

                Interlocked.Increment(ref Counter);
                TaskQueue.Enqueue(task);
            }

            System.Threading.Thread consumer = new Thread(ThreadConsumer);
            consumer.Start();
        }
        private void ThreadConsumer()
        {
            while (Counter > 0)
            {
                //同一條thread的記憶體似乎都是同一代generation....
                CMyTask task = null;
                if (TaskQueue.TryDequeue(out task))
                {
                    WriteFile2(task);
                }
            }
        }
        private void DoTaskRunTest(int times)
        {
            List<byte[]> raw_buffer = new List<byte[]>();

            for (int i = 0; i < times; i++)
            {
                for (int j = 0; j < 50 * 3; j++)
                {
                    var data = new byte[720];
                    raw_buffer.Add(data);
                }

                List<byte[]> output_data = new List<byte[]>();
                output_data.AddRange(raw_buffer);
                Interlocked.Increment(ref Counter);
                System.Threading.Tasks.Task.Run(
                    (() => { WriteFile(output_data); }));
                raw_buffer.Clear();
                //output_data = null;
            }
        }
        private void WriteFile2(CMyTask task)
        {
            string filename = string.Format("{0}\\{1}", TestDir, task.Name);
            WaveFileWriter writer = new WaveFileWriter(filename, new WaveFormat(8000, 16, 1));
            foreach (var item in task.BufferList)
                writer.Write(item, 0, item.Length);

            //偷看一下task與其property的generation : 通通都是generation 2 (包括task自己)
            //writer是generation 0
            //if (Counter % 1000 == 0)
            //{
            //    ShowGeneration(task);
            //    ShowGeneration(task.Name);
            //    ShowGeneration(task.BufferList);
            //    ShowGeneration(writer);
            //}
            writer.Close();
            writer.Dispose();
            writer = null;
            //GC似乎只會自動回收 generation 最新的那層
            //所以這函式內要是有做new，傳入的那個List就不會被快速回收到，要等很久
            //傳入的參數跨thread，測驗過它的generation==2
            if (Counter % 1000 == 0)
                GC.Collect(2);
            Interlocked.Decrement(ref Counter);
        }
        private void WriteFile(List<byte[]> buffers)
        {
            string filename = string.Format("{0}\\{1}.wav", TestDir, Guid.NewGuid());
            WaveFileWriter writer = new WaveFileWriter(filename, new WaveFormat(8000, 16, 1));
            foreach (var item in buffers)
                writer.Write(item, 0, item.Length);

<<<<<<< HEAD
            //for (int i = 0; i < buffers.Count; i++)
            //    buffers[i] = null;
            buffers.Clear();
            buffers = null;
=======
            writer.Close();
            writer.Dispose();
>>>>>>> 89cb229e4f66cffa2c24ab590a348bd45e266d78
            writer = null;

            //GC似乎只會自動回收 generation 最新的那層
            //所以這函式內要是有做new，傳入的那個List就不會被快速回收到，要等很久
            //傳入的參數跨thread，測驗過它的generation==2
            if (Counter % 1000 == 0)
                GC.Collect(2);
            Interlocked.Decrement(ref Counter);
        }

        private void ShowGeneration(object target)
        {
            string msg = string.Format("Object {0} generation = {1} \r\n", target.GetType(), GC.GetGeneration(target));
            UpdateTextBox(textBox1, msg);
        }

        private void UpdateTextBox(TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke(
                           (Action)(() => { UpdateTextBox(tb, msg, append); })
                        );
            }
            else
            {
                if (append)
                    tb.AppendText(msg);
                else
                    tb.Text = msg;
            }
        }
    }

    public class CMyTask
    {
        public string Name = Guid.NewGuid().ToString() + ".wav";
        public List<byte[]> BufferList = new List<byte[]>(); 
    }
}
