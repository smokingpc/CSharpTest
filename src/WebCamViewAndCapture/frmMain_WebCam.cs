using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectShowLib;

namespace WebCamViewAndCapture
{
    public partial class frmMain : Form
    {
        private DsDevice CurrentDevice = null;
        IFilterGraph2 GraphBuilder = null;
        IBaseFilter CaptureFilter = null;

        IPin PinPreviewOut = null;
        IPin PinCaptureOut = null;
        IPin PinPreviewStillOut = null;
        IPin PinCaptureStillOut = null;
        private void CloseInterfaces()
        {
            if (mediaControl != null)
                mediaControl.Stop();

            if (vmr9 != null)
            {
                Marshal.ReleaseComObject(vmr9);
                vmr9 = null;
                windowlessCtrl = null;
            }
            if (vmr9_2 != null)
            {
                Marshal.ReleaseComObject(vmr9_2);
                vmr9_2 = null;
                windowlessCtrl_2 = null;
            }

            if (graphBuilder != null)
            {
                Marshal.ReleaseComObject(graphBuilder);
                graphBuilder = null;
                mediaControl = null;
            }

        }

        public DsDevice[] EnumerateCaptureDevices()
        {
            return DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        }

        public void StartVideoStream(DsDevice dev) 
        {
            if (null != CurrentDevice)
                StopVideoStream();

            SetupDirectShowGraph(dev);
        }
        
        public void StopVideoStream() 
        {
            if (null == CurrentDevice)
                return;
        }

        private void SetupSmartTee(IFilterGraph2 builder, IBaseFilter capfiler)
        {
            IPin PinStreamOut = null;
            IPin PinSmartIn = null;
            IBaseFilter SmartTee = null;

            //==== Streaming Circuit ====
            try
            {
                SmartTee = new SmartTee() as IBaseFilter;
                GraphBuilder.AddFilter(SmartTee, "SmartTee");
                //裝好Filter以後，在Capture Device找出對應的pin腳然後接上去...
                //就像焊接電路一樣的觀念
                PinStreamOut = DsFindPin.ByCategory(capfiler, PinCategory.Capture, 0);
                PinSmartIn = DsFindPin.ByDirection(SmartTee, PinDirection.Input, 0);
                GraphBuilder.Connect(PinStreamOut, PinSmartIn);

                PinPreviewOut = DsFindPin.ByName(SmartTee, "Preview");
                PinCaptureOut = DsFindPin.ByName(SmartTee, "Capture");
            }
            finally 
            {
            //todo: 這邊應該弄Dispose不應該直接叫 Marshal
                if(null != PinStreamOut)
                    Marshal.ReleaseComObject(PinStreamOut);
                if (null != PinSmartIn)
                    Marshal.ReleaseComObject(PinSmartIn);
                if (null != SmartTee)
                    Marshal.ReleaseComObject(SmartTee);
            }
        }
        private void SetupSmartTeeStill(IFilterGraph2 builder, IBaseFilter capfiler)
        {
            //Still指 "靜止畫面"，也就是兩個SmartTee分別處理steaming與snapshot
            IPin PinStillOut = null;
            IPin PinSmartStillIn = null;
            IBaseFilter SmartTeeStill = null; 

            //==== Streaming Circuit ====
            try
            {
                SmartTeeStill = new SmartTee() as IBaseFilter;
                GraphBuilder.AddFilter(SmartTeeStill, "SmartTee");
                //裝好Filter以後，在Capture Device找出對應的pin腳然後接上去...
                //就像焊接電路一樣的觀念
                PinStillOut = DsFindPin.ByCategory(capfiler, PinCategory.Capture, 0);
                PinSmartStillIn = DsFindPin.ByDirection(SmartTeeStill, PinDirection.Input, 0);
                GraphBuilder.Connect(PinStillOut, PinSmartStillIn);

                PinPreviewStillOut = DsFindPin.ByName(SmartTeeStill, "Preview");
                PinCaptureStillOut = DsFindPin.ByName(SmartTeeStill, "Capture");
            }
            finally
            {
                //todo: 這邊應該弄Dispose不應該直接叫 Marshal
                if (null != PinStillOut)
                    Marshal.ReleaseComObject(PinStillOut);
                if (null != PinSmartStillIn)
                    Marshal.ReleaseComObject(PinSmartStillIn);
                if (null != SmartTeeStill)
                    Marshal.ReleaseComObject(SmartTeeStill);
            }
        }

        //DirectShow整個 Rendering Pipe 被稱為 Graph
        //觀念類似於RF電路圖，要串起整個電路才能正確render video stream
        //https://www.twblogs.net/a/5d04e5f9bd9eee487be9b034
        public void SetupDirectShowGraph(DsDevice dev)
        {
            GraphBuilder = new FilterGraph() as IFilterGraph2;
            //將Capture Device封裝成Filter，塞進整個電路
            GraphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out CaptureFilter);
            //串接後面的Filters 的 Pins
            SetupSmartTee(GraphBuilder, CaptureFilter);
            SetupSmartTeeStill(GraphBuilder, CaptureFilter);
        }
    }
}
