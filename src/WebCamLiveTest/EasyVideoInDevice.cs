using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;

namespace WebCamLiveTest
{
    public enum MEDIA_STATE
    { 
        STOP = 0,
        RUNNING = 1,
    }

    internal class CEasyVideoIn : IDisposable
    {
        private DsDevice CurrentDevice = null;
        private IFilterGraph2 GraphBuilder = null;
        private IBaseFilter VmrRenderer = null;
        private bool IsDisposed = false;
        private MEDIA_STATE State = MEDIA_STATE.STOP;

        public static DsDevice[] GetVideoInDevices()
        {
            return DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        }
        public static string[] GetVideoInDeviceNames()
        {
            List<string> ret = new List<string>();
            var devlist = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            foreach (var device in devlist)
            {
                ret.Add(device.Name);
            }

            return ret.ToArray();
        }

        public CEasyVideoIn() { }
        public CEasyVideoIn(DsDevice dev, IntPtr render_handle, Rectangle render_rect) 
            :this()
        { Setup(dev, render_handle, render_rect); }

        ~CEasyVideoIn() { Dispose(false); }

        private void Dispose(bool disposing)
        {
            //dispose的原則：
            //1.不論從誰呼叫進來都要 release unmanaged resource
            //2.要檢查是否已經dispose過
            //3.如果是從public interface進來，就表示caller希望明確
            //  釋放 managed resource，要在這邊做release

            if (IsDisposed)
                return;

            Teardown();
            IsDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //DirectShow整個 Rendering Pipe 被稱為 Graph
        //觀念類似於RF電路圖，要串起整個電路才能正確render video stream
        //https://www.twblogs.net/a/5d04e5f9bd9eee487be9b034
        public void Setup(DsDevice dev, IntPtr render_handle, Rectangle render_rect)
        {
            IBaseFilter cap_filter = null;
            GraphBuilder = new FilterGraph() as IFilterGraph2;
            //將Capture Device封裝成Filter，塞進整個電路
            GraphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out cap_filter);
            VmrRenderer = (IBaseFilter)new VideoMixingRenderer9();
            SetupVideoStream(GraphBuilder, cap_filter, VmrRenderer, render_handle, render_rect);
            CurrentDevice = dev;
        }
        public void Teardown()
        {
            Stop();
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

        public bool Start()
        {
            if (null != CurrentDevice)
                Stop();
            if (null == GraphBuilder)
                return false;

            IMediaControl ctrl = (IMediaControl)GraphBuilder;
            ctrl.Run();
            State = MEDIA_STATE.RUNNING;
            return true;
        }
        public void Stop()
        {
            if (null == CurrentDevice || null == GraphBuilder)
                return;

            IMediaControl ctrl = (IMediaControl)GraphBuilder;
            ctrl.Stop();
            State = MEDIA_STATE.STOP;
        }

        public void ResetRenderWindow(IntPtr render_handle, Rectangle render_rect)
        {
            IMediaControl ctrl = (IMediaControl)GraphBuilder;
            MEDIA_STATE old_state = this.State;
            if (null != GraphBuilder && old_state == MEDIA_STATE.RUNNING)
                ctrl.Stop();

            SetupRenderWindow((IVMRFilterConfig9)VmrRenderer, render_handle, render_rect);

            if (null != GraphBuilder && old_state == MEDIA_STATE.RUNNING)
                ctrl.Run();
        }

        private void SetupRenderWindow(IVMRFilterConfig9 renderer, IntPtr render_handle, Rectangle render_rect)
        {
            IVMRFilterConfig9 config = renderer;

            // Not really needed for VMR9 but don't forget calling it with VMR7
            config.SetNumberOfStreams(1);
            // Change VMR9 mode to Windowless
            config.SetRenderingMode(VMR9Mode.Windowless);
            IVMRWindowlessControl9 winless_ctrl = (IVMRWindowlessControl9)renderer;
            // Set "Parent" window
            winless_ctrl.SetVideoClippingWindow(render_handle);
            // Set Aspect-Ratio
            winless_ctrl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
            winless_ctrl.SetVideoPosition(null, DsRect.FromRectangle(render_rect));
            winless_ctrl = null;
        }
        private void SetupVideoStream(IFilterGraph2 builder, IBaseFilter capfiler, IBaseFilter renderer, IntPtr render_handle, Rectangle render_rect)
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
                SetupRenderWindow((IVMRFilterConfig9)renderer, render_handle, render_rect);
                builder.AddFilter(renderer, "Video Mixing Renderer 9");
                pin_in = DsFindPin.ByDirection(renderer, PinDirection.Input, 0);

                //裝好Filter以後，在Capture Device找出對應的pin腳然後接上去...
                //就像焊接電路一樣的觀念
                pin_out = DsFindPin.ByCategory(capfiler, PinCategory.Capture, 0);
                
                //todo: setup capture format
                //IAMStreamConfig pin_cfg = (IAMStreamConfig) pin_out;
                //AMMediaType type = null;
                //int count = 0, size = 0;
                //pin_cfg.GetNumberOfCapabilities(out count, out size);
                //for (int i = 0; i < count; i++)
                //{
                //    //IntPtr scc = Marshal.AllocHGlobal(Marshal.SizeOf<VideoStreamConfigCaps>());
                //    VideoStreamConfigCaps cap = new VideoStreamConfigCaps();
                //    GCHandle handle = GCHandle.Alloc(cap, GCHandleType.Pinned);
                //    IntPtr scc = handle.AddrOfPinnedObject();
                //    //Marshal.StructureToPtr<VideoStreamConfigCaps>(cap, scc, false);
                //    pin_cfg.GetStreamCaps(i, out type, scc);
                //    handle.Free();
                //}

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

    }
}
