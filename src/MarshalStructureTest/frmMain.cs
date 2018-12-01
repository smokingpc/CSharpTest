using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace MarshalStructureTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000000; i++)
            {
                //var buffer = new byte[TestData.Metadata.Length];
                //Array.Copy(TestData.Metadata, buffer, TestData.Metadata.Length);
                CIztUdpHeader header = CIztUdpHeader.Load(TestData.Header, 0);
                CIztMetadata metadata = CIztMetadata.Load(TestData.Header, 64);
                CIztPacket packet = CIztPacket.Load(TestData.Header, 0);
            }
        }
    }

    public enum GAIN_CTRL_MODE : byte
    { 
        MGC = 0,
        ONEOFF_AGC = 1,
        SFAST_AGC = 2,
        FAST_AGC = 3,
        MED_AGC = 4,
        SLOW_AGC = 5
    }

    public enum FFT_LENGTH : byte
    { 
        FFT_1024 = 1,
        FFT_4096 = 2,
    }

    //StructLayout屬性直接控制structure與class在記憶體中的field layout
    //並可以強迫指定struct / class 的總size (不包含function)
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable")]
    public struct CIztUdpHeader 
    {
        public static readonly int LayoutSize = 64;

        //FieldOffset指定變數在記憶體中是從哪個byte開始算，通常都配合StructLayout使用
        //這個屬性不能用在Auto Property身上，static不受其影響
        [FieldOffset(0)]
        private UInt32 _SyncPattern;
        public UInt32 SyncPattern { get { return _SyncPattern; } }

        [FieldOffset(4)]
        private UInt32 _DataType;
        public UInt32 DataType { get { return _DataType; } }

        [FieldOffset(8)]
        private Int64 _ToA;    //in MicroSecond.
        public Int64 ToA { get { return _ToA; } }
        public DateTime ToADateTime 
        { 
            get 
            { 
                //Linux timestamp is from seconds.....
                DateTime linux_base = new DateTime(1970,1,1,0,0,0);
                var timestamp = linux_base.Add(TimeSpan.FromTicks(_ToA * 10));
                return timestamp.ToLocalTime();
            } 
        }

        [FieldOffset(16)]
        private UInt64 _FrameCount;
        public UInt64 FrameCount { get { return _FrameCount; } }

        [FieldOffset(24)]
        private UInt64 _InJobFrameCount;
        public UInt64 InJobFrameCount { get { return _InJobFrameCount; } }

        [FieldOffset(32)]
        private UInt64 _ConfigCount;
        public UInt64 ConfigCount { get { return _ConfigCount; } }

        [FieldOffset(40)]
        private UInt32 _Unused1;
        [FieldOffset(44)]
        private UInt32 _Unused2;
        [FieldOffset(48)]
        private UInt32 _Unused3;

        [FieldOffset(52)]
        private UInt32 _ClientPostProcess;
        public UInt32 ClientPostProcess { get { return _ClientPostProcess; } }

        [FieldOffset(56)]
        private UInt32 _ExtHeaderLen;
        public UInt32 ExtHeaderLen { get { return _ExtHeaderLen; } }

        [FieldOffset(60)]
        private UInt32 _FullLen;
        public UInt32 FullLen { get { return _FullLen; } }

        public static CIztUdpHeader Load(byte[] buffer, int offset)
        {
            CIztUdpHeader header;

            if (buffer != null && (offset + LayoutSize) <= buffer.Length)
            {

                byte[] copydata = new byte[LayoutSize];
                Array.Copy(buffer, offset, copydata, 0, LayoutSize);

                //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
                //相當於允許直接使用底層的C指標
                GCHandle handle = GCHandle.Alloc(copydata, GCHandleType.Pinned);
                header = (CIztUdpHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CIztUdpHeader));
                handle.Free();
            }
            else
                header = new CIztUdpHeader();

            return header;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 184)]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable")]
    public struct CIztMetadata
    {
        public static readonly byte ChannelMask = 0x0F;
        public static readonly byte FFTMask = 0x30;
        public static readonly byte JobFirstItemMask = 0x01;
        public static readonly byte JobLastItemMask = 0x02;
        public static readonly byte GapMask = 0x04;
        public static readonly byte AdcOverloadMask = 0x08;
        public static readonly byte PreAmpMask = 0x10;
        public static readonly byte BiasTMask = 0xE0;
        public static readonly byte WidthViolationMask = 0x01;
        public static readonly byte dataImpairedMask = 0x02;
        public static readonly byte PreAttenuatorMask = 0x04;
        public static readonly byte HFFilterBypassedMask = 0x08;
        public static readonly byte DataVideoFilteredMask = 0x10;
        public static readonly byte GainControlModeMask = 0xE0;

        public static readonly int LayoutSize = 184;

        [FieldOffset(0)]
        private byte _Version;
        public byte Version { get { return _Version; } }

        [FieldOffset(1)]
        private byte _ChnAndFFT;
        public byte ChannelID { get { return (byte)(_ChnAndFFT & ChannelMask); } }
        public FFT_LENGTH FFTLength { get {return (FFT_LENGTH) (_ChnAndFFT & FFTMask); } }
        public int FFTSize 
        { 
            get 
            {
                switch (FFTLength)
                { 
                    case FFT_LENGTH.FFT_1024:
                        return 1024;
                    case FFT_LENGTH.FFT_4096:
                        return 4096;
                }
                return 0;
            } 
        }

        [FieldOffset(2)]
        private UInt16 _Unused1;

        [FieldOffset(4)]
        private unsafe fixed byte _JobID[28];   //固定的char buffer要用fixed byte, unsafe指的是直接存取pointer
        public unsafe string JobID
        {
            get
            {
                fixed (byte* buffer = _JobID)
                {
                    var data = new byte[28];
                    //宣告 unsafe fixed 的陣列必須視為 C 語言裏面new出來的東西
                    //這樣的陣列必須透過Marshal去操作，不能再用C#裏面的語法
                    Marshal.Copy(new IntPtr(buffer), data, 0, 28);
                    //用 "找尋array裏面第一個 '\0' " 的方法去決定整個array的size
                    //透過這方法直接把字串尾的 '\0' 排除掉
                    var size = Array.IndexOf<byte>(data, 0);
                    return Encoding.UTF8.GetString(data, 0, size);
                }
            }
        }

        [FieldOffset(32)]
        private double _XOversampling;
        public double XOverSampling { get { return _XOversampling; } }

        [FieldOffset(40)]
        private double _XStart;
        public double XStart { get { return _XStart; } }

        [FieldOffset(48)]
        private double _YDelta;
        public double YDelta { get { return _YDelta; } }

        [FieldOffset(56)]
        private double _YRange;
        public double YRange { get { return _YRange; } }

        [FieldOffset(64)]
        private double _ProcGain;
        public double ProcGain { get { return _ProcGain; } }

        [FieldOffset(72)]
        private double _Unused2;

        [FieldOffset(80)]
        private byte _Flags1;

        [FieldOffset(81)]
        private byte _Flags2;

        public bool JobFirstItem { get { return Convert.ToBoolean(_Flags1 & JobFirstItemMask); } }
        public bool JobLastItem { get { return Convert.ToBoolean(_Flags1 & JobLastItemMask); } }
        public bool Gap { get { return Convert.ToBoolean(_Flags1 & GapMask); } }
        public bool AdcOverload { get { return Convert.ToBoolean(_Flags1 & AdcOverloadMask); } }
        public bool PreAmp { get { return Convert.ToBoolean(_Flags1 & PreAmpMask); } }
        public bool BiasT { get { return Convert.ToBoolean(_Flags1 & BiasTMask); } }
        public bool BandwidthViolation { get { return Convert.ToBoolean(_Flags2 & WidthViolationMask); } }
        public bool DataImpaired { get { return Convert.ToBoolean(_Flags2 & dataImpairedMask); } }
        public bool PreAttenuator { get { return Convert.ToBoolean(_Flags2 & PreAttenuatorMask); } }
        public bool HFFilterBypassed { get { return Convert.ToBoolean(_Flags2 & HFFilterBypassedMask); } }
        public bool DataVideoFiltered { get { return Convert.ToBoolean(_Flags2 & DataVideoFilteredMask); } }
        public GAIN_CTRL_MODE GainControlMode 
        { 
            get 
            {
                byte value = (byte)(_Flags2 & GainControlModeMask);
                return (GAIN_CTRL_MODE)value;
            } 
        }

        [FieldOffset(82)]
        private UInt16 _Unused3;

        [FieldOffset(84)]
        private Int32 _Antenna;
        public Int32 Antenna { get { return _Antenna; } }

        [FieldOffset(88)]
        private double _FreqCenter;
        public double FreqCenter { get { return _FreqCenter; } }

        [FieldOffset(96)]
        private double _XDelta;
        public double XDelta { get { return _XDelta; } }

        [FieldOffset(104)]
        private double _Duration;
        public double Duration { get { return _Duration; } }

        [FieldOffset(112)]
        private double _Atenuation;
        public double Atenuation { get { return _Atenuation; } }

        [FieldOffset(120)]
        private double _GainCorrection;
        public double GainCorrection { get { return _GainCorrection; } }

        [FieldOffset(128)]
        private double _HwFrequency;
        public double HwFrequency { get { return _HwFrequency; } }

        [FieldOffset(136)]
        private Int32 _FilterLimitLower;
        public Int32 FilterLimitLower { get { return _FilterLimitLower; } }

        [FieldOffset(140)]
        private Int32 _FilterLimitUpper;
        public Int32 FilterLimitUpper { get { return _FilterLimitUpper; } }

        [FieldOffset(144)]
        private unsafe fixed byte _Unused4[24];

        [FieldOffset(168)]
        private unsafe fixed byte _Unused5[16];

        public int ValidNodes
        {
            get 
            {
                var result = Convert.ToInt32(Math.Round(Math.Floor(Convert.ToDouble(FFTSize) / XOverSampling)));
                if (result % 2 == 1)
                    result++;

                return result;
            }
        }

        public static CIztMetadata Load(byte[] buffer, int offset)
        {
            CIztMetadata metadata;

            if (buffer != null && (offset + LayoutSize) <= buffer.Length)
            {

                byte[] copydata = new byte[LayoutSize];
                Array.Copy(buffer, offset, copydata, 0, LayoutSize);

                //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
                //相當於允許直接使用底層的C指標
                GCHandle handle = GCHandle.Alloc(copydata, GCHandleType.Pinned);
                metadata = (CIztMetadata)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CIztMetadata));
                handle.Free();
            }
            else
                metadata = new CIztMetadata();

            return metadata;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 248)]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable")]
    public struct CIztPacket
    {
        public static readonly int LayoutSize = 248;

        [FieldOffset(0)]
        public CIztUdpHeader Header;
        [FieldOffset(64)]
        public CIztMetadata Metadata;

        public static CIztPacket Load(byte[] buffer, int offset)
        {
            CIztPacket header;

            if (buffer != null && (offset + LayoutSize) <= buffer.Length)
            {

                byte[] copydata = new byte[LayoutSize];
                Array.Copy(buffer, offset, copydata, 0, LayoutSize);

                //GCHandleType.Pinned 允許這個buffer做出來的handle直接取用底下的實體記憶體位置
                //相當於允許直接使用底層的C指標
                GCHandle handle = GCHandle.Alloc(copydata, GCHandleType.Pinned);
                header = (CIztPacket)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CIztPacket));
                handle.Free();
            }
            else
                header = new CIztPacket();

            return header;
        }
    }

    public static class TestData
    {
        public static byte[] Header = new byte[]
        {
            //用R3000的封包檔頭來實驗
            //前64byte : header
            0xfe, 0xff, 0xff, 0x7f, 0x00, 0x00, 0x01, 0x10,
            0x26, 0x4e, 0x58, 0xcf, 0x2d, 0x50, 0x05, 0x00,
            0x01, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xb8, 0x00, 0x00, 0x00, 0xb8, 0x20, 0x00, 0x00,
            
            //接下來184byte是 metadata
            0x04, 0x20, 0x00, 0x00, 0x4c, 0x69, 0x76, 0x65,
            0x54, 0x75, 0x6e, 0x69, 0x6e, 0x67, 0x20, 0x39,
            0xb8, 0x00, 0x00, 0x00, 0xb8, 0x20, 0x00, 0x00,
            0xb8, 0x00, 0x00, 0x00, 0xb8, 0x20, 0x00, 0x00,
            0xb8, 0x00, 0x00, 0x00, 0xb8, 0x20, 0xf4, 0x3f,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x3f,
            0x00, 0x00, 0x00, 0x00, 0xe0, 0xff, 0x6f, 0x40,
            0xa5, 0xbd, 0xc1, 0x17, 0x26, 0x33, 0x60, 0x40,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0xa0, 0x14, 0x0a, 0x00, 0x00, 0x00, 0x00,
            0x3c, 0x85, 0xfd, 0x7f, 0xdf, 0x17, 0xa0, 0x41,
            0x00, 0x00, 0x00, 0x00, 0x38, 0x9c, 0xbc, 0x40,
            0xdb, 0x74, 0x28, 0x67, 0x4c, 0xe5, 0x91, 0x3f,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0xc0, 0xcc, 0x4c, 0x36, 0x40,
            0x00, 0x00, 0x00, 0x80, 0xdf, 0x17, 0xa0, 0x41,
            0xc8, 0xa9, 0x01, 0x00, 0xf8, 0x9b, 0x02, 0x00, 
            0x00, 0x06, 0x00, 0x00, 0x11, 0x9e, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x40, 0x65, 0x12, 0x20, 0xc4, 0x9b, 0xa5, 0xe3,
            0x04, 0x01, 0x00, 0x00, 0x4c, 0x69, 0x76, 0x65,
        };

        public static byte[] Payload = new byte[]
        {
            0x0b, 0x2a, 0xe8, 0x28, 0x54, 0x2a, 0xa1, 0x29,
            0xa6, 0x28, 0xe2, 0x29, 0xa3, 0x29, 0xf7, 0x28,
        };
    }
}
