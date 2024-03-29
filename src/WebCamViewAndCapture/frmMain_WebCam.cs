﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
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
        private static readonly string BinPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        private DsDevice CurrentDevice = null;
        private IFilterGraph2 GraphBuilder = null;
        private IBaseFilter VmrRenderer = null;

        private void TeardownDirectShowGraph()
        {
            StopVideoStream();
            if (VmrRenderer != null)
            {
                Marshal.ReleaseComObject(VmrRenderer);
                VmrRenderer = null;
            }
            if (GraphBuilder != null)
            {
                Marshal.ReleaseComObject(GraphBuilder);
                GraphBuilder = null;
            }
            CurrentDevice = null;
        }

        public void VideoWindowSnapshot(IVMRFilterConfig9 renderer)
        {
            if (null == VmrRenderer)
                return;

            IVMRWindowlessControl9 ctrl = (IVMRWindowlessControl9)renderer;
            IntPtr imgptr = IntPtr.Zero;
            Bitmap bmp = null;
            try
            {
                ctrl.GetCurrentImage(out imgptr);
                if (IntPtr.Zero != imgptr)
                {
                    BitmapInfoHeader header = new BitmapInfoHeader();
                    Marshal.PtrToStructure(imgptr, header);
                    //stride formula comes from MSDN
                    //https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-bitmapinfoheader
                    var stride = ((((header.Width * header.BitCount) + 31) & ~31) >> 3);
                    
                    //暫時假定輸出圖像是純DIB沒有Color Mask等額外資訊
                    bmp = new Bitmap(header.Width, header.Height, stride, 
                                PixelFormat.Format32bppArgb, imgptr+header.Size);
                    //注意這邊撈出的CurrentImage，是上下顛倒狀態(沿著X軸鏡射)
                    //要把它再鏡射回來才能存檔
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "Bitmap File (*.bmp)|*.bmp";
                    dlg.InitialDirectory = BinPath;
                    if(dlg.ShowDialog() == DialogResult.OK)
                        bmp.Save(dlg.FileName, ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save snapshot bmp file : " + ex.Message);
            }
            finally
            {
                if (bmp != null)
                    bmp.Dispose();
                if (IntPtr.Zero != imgptr)
                    Marshal.FreeCoTaskMem(imgptr);
            }
        }

        public DsDevice[] EnumerateCaptureDevices()
        {
            return DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        }

        public void StartVideoStream(DsDevice dev, Control render_target) 
        {
            if (null != CurrentDevice)
                StopVideoStream();

            SetupDirectShowGraph(dev, render_target);
            IMediaControl ctrl = (IMediaControl)GraphBuilder;
            ctrl.Run();
        }

        public void StopVideoStream() 
        {
            if (null == CurrentDevice || null == GraphBuilder)
                return;

            IMediaControl ctrl = (IMediaControl)GraphBuilder;
            ctrl.Stop();
        }

        private void SetupRenderWindow(IVMRFilterConfig9 renderer, Control target)
        {
            IVMRFilterConfig9 config = renderer;

            // Not really needed for VMR9 but don't forget calling it with VMR7
            config.SetNumberOfStreams(1);
            // Change VMR9 mode to Windowless
            config.SetRenderingMode(VMR9Mode.Windowless);
            IVMRWindowlessControl9 winless_ctrl = (IVMRWindowlessControl9)renderer;
            // Set "Parent" window
            winless_ctrl.SetVideoClippingWindow(target.Handle);
            // Set Aspect-Ratio
            winless_ctrl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
            winless_ctrl.SetVideoPosition(null, DsRect.FromRectangle(target.ClientRectangle));
            winless_ctrl = null;
        }

        public void SetupVideoStream(IFilterGraph2 builder, IBaseFilter capfiler, IBaseFilter renderer, Control target)
        {
            //串接後面的Filters 的 Pins
            //目前不做畫面擷取所以也不需要用SmartTee來分流
            IPin pin_out = null;
            IPin pin_in = null;

            //==== Streaming Circuit ====
            try
            {
                //Win7開始預設最好是使用 VideoMixingRenderer7，當然能用VideoMixingRenderer9更好
                //原始的VideoRenderer吃不到顯卡的特殊能力
                //renderer = (IBaseFilter)new VideoMixingRenderer9();
                SetupRenderWindow((IVMRFilterConfig9)renderer, target);
                builder.AddFilter(renderer, "Video Mixing Renderer 9");
                pin_in = DsFindPin.ByDirection(renderer, PinDirection.Input, 0);

                //裝好Filter以後，在Capture Device找出對應的pin腳然後接上去...
                //就像焊接電路一樣的觀念
                pin_out = DsFindPin.ByCategory(capfiler, PinCategory.Capture, 0);
                builder.Connect(pin_out, pin_in);
            }
            finally 
            {
                //todo: 這邊應該弄Dispose不應該直接叫 Marshal
                if (null != pin_out)
                    Marshal.ReleaseComObject(pin_out);
                if (null != pin_in)
                    Marshal.ReleaseComObject(pin_in);
            }
        }

        //DirectShow整個 Rendering Pipe 被稱為 Graph
        //觀念類似於RF電路圖，要串起整個電路才能正確render video stream
        //https://www.twblogs.net/a/5d04e5f9bd9eee487be9b034
        public void SetupDirectShowGraph(DsDevice dev, Control render_target)
        {
            IBaseFilter cap_filter = null;
            GraphBuilder = new FilterGraph() as IFilterGraph2;
            //將Capture Device封裝成Filter，塞進整個電路
            GraphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out cap_filter);
            VmrRenderer = (IBaseFilter)new VideoMixingRenderer9();
            SetupVideoStream(GraphBuilder, cap_filter, VmrRenderer, render_target);
            CurrentDevice = dev;
        }
    }
}
