using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace uDrawLib
{
  public class HIDDevice : IDisposable
  {
    #region P/Invoke Crud

    [Flags]
    public enum EFileAttributes : uint
    {
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDD_ATTRIBUTES
    {
      public int Size;
      public short VendorID;
      public short ProductID;
      public short VersionNumber;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GUID
    {
      public int Data1;
      public System.UInt16 Data2;
      public System.UInt16 Data3;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
      public byte[] data4;
    }

    [DllImport("hid.dll", SetLastError = true)]
    static extern void HidD_GetHidGuid(
      ref GUID lpHidGuid);

    [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetupDiGetClassDevs(
      ref GUID ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags
      );

    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DEVICE_INTERFACE_DATA
    {
      public int cbSize;
      public GUID InterfaceClassGuid;
      public int Flags;
      public int Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
      public UInt32 cbSize;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PSP_DEVICE_INTERFACE_DETAIL_DATA
    {
      public int cbSize;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    [DllImport(@"setupapi.dll", SetLastError = true)]
    public static extern Boolean SetupDiGetDeviceInterfaceDetail(
      IntPtr hDevInfo,
      ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
      IntPtr deviceInterfaceDetailData,
      UInt32 deviceInterfaceDetailDataSize,
      out UInt32 requiredSize,
      IntPtr deviceInfoData
    );

    [DllImport(@"setupapi.dll", SetLastError = true)]
    public static extern Boolean SetupDiGetDeviceInterfaceDetail(
      IntPtr hDevInfo,
      ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
      ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
      UInt32 deviceInterfaceDetailDataSize,
      out UInt32 requiredSize,
      IntPtr deviceInfoData
    );

    [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern Boolean SetupDiEnumDeviceInterfaces(
      IntPtr hDevInfo,
      //ref SP_DEVINFO_DATA devInfo,
      IntPtr devInvo,
      ref GUID interfaceClassGuid,
      Int32 memberIndex,
      ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
    );

    [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern UInt16 SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern SafeFileHandle CreateFile(
      string fileName,
      [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
      [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
      [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
      IntPtr template);

    [DllImport("kernel32.dll")]
    static public extern int CloseHandle(SafeFileHandle hObject);

    [DllImport("hid.dll", SetLastError = true)]
    private static extern int HidD_GetPreparsedData(
      SafeFileHandle hObject,
      ref int pPHIDP_PREPARSED_DATA);

    [DllImport("hid.dll", SetLastError = true)]
    private static extern int HidP_GetCaps(
      int pPHIDP_PREPARSED_DATA,
      ref HIDP_CAPS myPHIDP_CAPS);

    [DllImport("hid.dll")]
    internal extern static bool HidD_SetOutputReport(
      IntPtr HidDeviceObject,
      byte[] lpReportBuffer,
      uint ReportBufferLength);

    [DllImport("hid.dll")]
    public static extern Boolean HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

    [DllImport("kernel32.dll")]
    static public extern int WriteFile(SafeFileHandle hFile, ref byte lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, int lpOverlapped);

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_CAPS
    {
      public System.UInt16 Usage;
      public System.UInt16 UsagePage;
      public System.UInt16 InputReportByteLength;
      public System.UInt16 OutputReportByteLength;
      public System.UInt16 FeatureReportByteLength;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
      public System.UInt16[] Reserved;
      public System.UInt16 NumberLinkCollectionNodes;
      public System.UInt16 NumberInputButtonCaps;
      public System.UInt16 NumberInputValueCaps;
      public System.UInt16 NumberInputDataIndices;
      public System.UInt16 NumberOutputButtonCaps;
      public System.UInt16 NumberOutputValueCaps;
      public System.UInt16 NumberOutputDataIndices;
      public System.UInt16 NumberFeatureButtonCaps;
      public System.UInt16 NumberFeatureValueCaps;
      public System.UInt16 NumberFeatureDataIndices;
    }

    private const int DIGCF_PRESENT = 0x00000002;
    private const int DIGCF_DEVICEINTERFACE = 0x00000010;
    private const int DIGCF_INTERFACEDEVICE = 0x00000010;
    private const uint GENERIC_READ = 0x80000000;
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint FILE_SHARE_READ = 0x00000001;
    private const uint FILE_SHARE_WRITE = 0x00000002;
    private const int OPEN_EXISTING = 3;

    #endregion

    #region Declarations

    private bool _found = false;
    private GUID _guid;
    private IntPtr _hDeviceInfo = IntPtr.Zero;
    private SP_DEVICE_INTERFACE_DATA _SP_DEVICE_INTERFACE_DATA;
    private string _devicePath;
    private SafeFileHandle _hidHandle;
    private FileStream _stream;

    #endregion

    #region Public Events

    public event EventHandler<DataReceivedEventArgs> DataReceived;

    #endregion

    #region Constructors / Teardown

    public HIDDevice(string devicePath)
    {
      _Init(devicePath, false);
    }

    public HIDDevice(int vendorId, int productId)
    {
      _Init(vendorId, productId, false);
    }

    public HIDDevice(int vendorId, int productId, bool throwNotFoundError)
    {
      _Init(vendorId, productId, throwNotFoundError);
    }

    public HIDDevice(string devicePath, bool throwNotFoundError)
    {
      _Init(devicePath, throwNotFoundError);
    }

    #endregion

    #region Public Properties

    public int InputReportLength { get; private set; }
    public int OutputReportLength { get; private set; }

    public bool Found
    {
      get
      {
        return _found;
      }
    }

    #endregion

    #region Public Methods

    public static List<string> GetAllDevices(int? vendorId, int? productId)
    {
      int index = 0;
      GUID guid = new GUID();
      var ret = new List<string>();

      HidD_GetHidGuid(ref guid);
      IntPtr devicesHandle = SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);
      var diData = new SP_DEVICE_INTERFACE_DATA();
      diData.cbSize = Marshal.SizeOf(diData);

      while (SetupDiEnumDeviceInterfaces(devicesHandle, IntPtr.Zero, ref guid, index, ref diData))
      {
        //Get the buffer size
        UInt32 size;
        SetupDiGetDeviceInterfaceDetail(devicesHandle, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);

        // Uh...yeah.
        var diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
        diDetail.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);

        //Get detailed information
        if (SetupDiGetDeviceInterfaceDetail(devicesHandle, ref diData, ref diDetail, size, out size, IntPtr.Zero))
        {
          //Get a handle to this device
          var handle = CreateFile(diDetail.DevicePath, FileAccess.ReadWrite, FileShare.ReadWrite,
            IntPtr.Zero, FileMode.Open, EFileAttributes.Overlapped, IntPtr.Zero);

          //Get this device's attributes
          var attrib = new HIDD_ATTRIBUTES();
          attrib.Size = Marshal.SizeOf(attrib);
          if (HidD_GetAttributes(handle.DangerousGetHandle(), ref attrib))
          {
            //See if this is one we care about
            if ((!vendorId.HasValue || ((attrib.VendorID & 0xFFFF) == vendorId.Value)) &&
              (!productId.HasValue || ((attrib.ProductID & 0xFFFF) == productId.Value)))
            {
              ret.Add(diDetail.DevicePath);
              break;
            }
          }

          //Close the handle
          handle.Close();
        }

        //Move on
        index++;
      }

      SetupDiDestroyDeviceInfoList(devicesHandle);

      return ret;
    }

    public static bool IsDetected(int vendorId, int productId)
    {
      var device = new HIDDevice(vendorId, productId, false);
      bool ret = device.Found;

      device.Dispose();

      return ret;
    }

    public void Write(byte reportType, byte[] data)
    {
      int bytesSent = 0;

      do
      {
        byte[] buffer = new byte[OutputReportLength];
        buffer[0] = reportType;
        for (int i = 1; i < buffer.Length; i++)
        {
          if (bytesSent < data.Length)
          {
            buffer[i] = data[bytesSent];
            bytesSent++;
          }
          else
          {
            buffer[i] = 0;
          }
        }

        HidD_SetOutputReport(_hidHandle.DangerousGetHandle(), buffer, (uint)buffer.Length);
      } while (bytesSent < data.Length) ;
    }

    public void Disconnect()
    {
      try
      {
        _stream.Close();
      }
      catch
      {
      }

      try
      {
        CloseHandle(_hidHandle);
      }
      catch
      {
      }

      SetupDiDestroyDeviceInfoList(_hDeviceInfo);
    }

    public void Dispose()
    {
      try
      {
        Disconnect();
      }
      catch
      {
        //Don't care...
      }
    }

    #endregion

    #region Private Methods

    private void _Init(int vendorId, int productId, bool throwNotFoundError)
    {
      var devices = HIDDevice.GetAllDevices(vendorId, productId);

      if (devices != null && devices.Count > 0)
      {
        _Init(devices[0], throwNotFoundError);
      }
      else
      {
        if (throwNotFoundError)
          throw new InvalidOperationException("Device not found");
      }
    }

    private void _Init(string devicePath, bool throwNotFoundError)
    {
      bool result;
      int deviceCount = 0;
      uint size;
      uint requiredSize;

      _guid = new GUID();
      HidD_GetHidGuid(ref _guid);

      _hDeviceInfo = SetupDiGetClassDevs(ref _guid, null, IntPtr.Zero, DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);

      do
      {
        _SP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
        _SP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(_SP_DEVICE_INTERFACE_DATA);
        result = SetupDiEnumDeviceInterfaces(_hDeviceInfo, IntPtr.Zero, ref _guid, deviceCount, ref _SP_DEVICE_INTERFACE_DATA);
        SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, IntPtr.Zero, 0, out requiredSize, IntPtr.Zero);
        size = requiredSize;
        var diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
        diDetail.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);
        SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, ref diDetail,
          size, out requiredSize, IntPtr.Zero);
        _devicePath = diDetail.DevicePath;

        if (_devicePath == devicePath)
        {
          _found = true;
          _SP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
          _SP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(_SP_DEVICE_INTERFACE_DATA);
          SetupDiEnumDeviceInterfaces(_hDeviceInfo, IntPtr.Zero, ref _guid, deviceCount, ref _SP_DEVICE_INTERFACE_DATA);
          size = 0;
          requiredSize = 0;
          SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, IntPtr.Zero, size, out requiredSize, IntPtr.Zero);
          SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, IntPtr.Zero, size, out requiredSize, IntPtr.Zero);
          _hidHandle = CreateFile(_devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, EFileAttributes.Overlapped, IntPtr.Zero);

          //Get report lengths
          int preparsedDataPtr = -1;
          HidD_GetPreparsedData(_hidHandle, ref preparsedDataPtr);
          var caps = new HIDP_CAPS();
          HidP_GetCaps(preparsedDataPtr, ref caps);
          OutputReportLength = caps.OutputReportByteLength;
          InputReportLength = caps.InputReportByteLength;

          _stream = new FileStream(_hidHandle, FileAccess.ReadWrite, InputReportLength, true);
          var buffer = new byte[InputReportLength];
          _stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnReadData), buffer);

          break;
        }

        deviceCount++;
      } while (result);

      if (!_found)
      {
        if (throwNotFoundError)
          throw new InvalidOperationException("Device not found");
      }
    }

    private void OnReadData(IAsyncResult result)
    {
      var buffer = (byte[])result.AsyncState;

      try
      {
        _stream.EndRead(result);
        var receivedData = new byte[InputReportLength - 1];
        Array.Copy(buffer, 1, receivedData, 0, receivedData.Length);

        if (receivedData != null)
        {
          if (DataReceived != null)
            DataReceived(this, new DataReceivedEventArgs(buffer[0], receivedData));
        }

        var buf = new byte[buffer.Length];
        _stream.BeginRead(buf, 0, buffer.Length, new AsyncCallback(OnReadData), buf);
      }
      catch
      {
      }
    }

    #endregion
  }
}
