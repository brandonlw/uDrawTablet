using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace uDrawLib
{
  public class HIDDevice : IDisposable
  {
    #region P/Invoke Crud

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GUID
    {
      public int Data1;
      public System.UInt16 Data2;
      public System.UInt16 Data3;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
      public byte[] data4;
    }

    [DllImport("hid.dll", SetLastError = true)]
    static extern unsafe void HidD_GetHidGuid(
      ref GUID lpHidGuid);

    [DllImport("setupapi.dll", SetLastError = true)]
    static extern unsafe int SetupDiGetClassDevs(
      ref GUID lpHidGuid,
      int* Enumerator,
      int* hwndParent,
      int Flags);

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SP_DEVICE_INTERFACE_DATA
    {
      public int cbSize;
      public GUID InterfaceClassGuid;
      public int Flags;
      public int Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct PSP_DEVICE_INTERFACE_DETAIL_DATA
    {
      public int cbSize;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    [DllImport("setupapi.dll", SetLastError = true)]
    static extern unsafe int SetupDiGetDeviceInterfaceDetail(
      int DeviceInfoSet,
      ref SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData,
      int* aPtr,
      int detailSize,
      ref int requiredSize,
      int* bPtr);

    [DllImport("setupapi.dll", SetLastError = true)]
    static extern unsafe int SetupDiGetDeviceInterfaceDetail(
      int DeviceInfoSet,
      ref SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData,
      ref PSP_DEVICE_INTERFACE_DETAIL_DATA myPSP_DEVICE_INTERFACE_DETAIL_DATA,
      int detailSize,
      ref int requiredSize,
      int* bPtr);

    [DllImport("setupapi.dll", SetLastError = true)]
    static extern unsafe int SetupDiEnumDeviceInterfaces(
      int DeviceInfoSet,
      int DeviceInfoData,
      ref  GUID lpHidGuid,
      int MemberIndex,
      ref  SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData);

    [DllImport("setupapi.dll", SetLastError = true)]
    static extern unsafe int SetupDiDestroyDeviceInfoList(
      int DeviceInfoSet
      );

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern int CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        uint lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        uint hTemplateFile
        );

    [DllImport("kernel32.dll")]
    static public extern int CloseHandle(int hObject);

    [DllImport("hid.dll", SetLastError = true)]
    private unsafe static extern int HidD_GetPreparsedData(
      int hObject,
      ref int pPHIDP_PREPARSED_DATA);

    [DllImport("hid.dll", SetLastError = true)]
    private unsafe static extern int HidP_GetCaps(
      int pPHIDP_PREPARSED_DATA,
      ref HIDP_CAPS myPHIDP_CAPS);

    [DllImport("kernel32.dll", SetLastError = true)]
    private unsafe static extern bool ReadFile(
      int hFile,
      byte[] lpBuffer,
      int nNumberOfBytesToRead,
      ref int lpNumberOfBytesRead,
      int* ptr
      );

    [DllImport("kernel32.dll")]
    static public extern int WriteFile(int hFile, ref byte lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, int lpOverlapped);

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct HIDP_CAPS
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
    private int _hDeviceInfo = -1;
    private SP_DEVICE_INTERFACE_DATA _SP_DEVICE_INTERFACE_DATA;
    private PSP_DEVICE_INTERFACE_DETAIL_DATA _PSP_DEVICE_INTERFACE_DETAIL_DATA;
    private string _devicePath;
    private int _hidHandle;
    private Thread _dataReadingThread;

    #endregion

    #region Public Events

    public event EventHandler<DataReceivedEventArgs> DataReceived;

    #endregion

    #region Constructors / Teardown

    public unsafe HIDDevice(string devicePath)
    {
      _Init(devicePath, false);
    }

    public unsafe HIDDevice(int vendorId, int productId)
    {
      _Init(vendorId, productId, false);
    }

    private unsafe HIDDevice(int vendorId, int productId, bool throwNotFoundError)
    {
      _Init(vendorId, productId, throwNotFoundError);
    }

    #endregion

    #region Public Properties

    public bool Found
    {
      get
      {
        return _found;
      }
    }

    #endregion

    #region Public Methods

    public static unsafe List<string> GetAllDevices(int? vendorId, int? productId)
    {
      int result;
      int deviceCount = 0;
      int size = 0;
      int requiredSize = 0;
      var ret = new List<string>();

      var guid = new GUID();
      HidD_GetHidGuid(ref guid);

      var hDeviceInfo = SetupDiGetClassDevs(ref guid, null, null, DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);

      do
      {
        var SP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
        SP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(SP_DEVICE_INTERFACE_DATA);
        result = SetupDiEnumDeviceInterfaces(hDeviceInfo, 0, ref guid, deviceCount, ref SP_DEVICE_INTERFACE_DATA);
        SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref SP_DEVICE_INTERFACE_DATA, null, 0, ref requiredSize, null);
        size = requiredSize;
        var PSP_DEVICE_INTERFACE_DETAIL_DATA = new PSP_DEVICE_INTERFACE_DETAIL_DATA();
        PSP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = 5;
        SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref SP_DEVICE_INTERFACE_DATA, ref PSP_DEVICE_INTERFACE_DETAIL_DATA,
          size, ref requiredSize, null);
        var devicePath = PSP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;

        string deviceID = String.Empty;
        if (vendorId.HasValue)
          deviceID += "vid_" + vendorId.Value.ToString("x4");
        if (productId.HasValue)
        {
          if (!String.IsNullOrEmpty(deviceID)) deviceID += "&";
          deviceID += "pid_" + productId.Value.ToString("x4");
        }
        if (String.IsNullOrEmpty(deviceID) || devicePath.IndexOf(deviceID) > 0)
        {
          SP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
          SP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(SP_DEVICE_INTERFACE_DATA);
          SetupDiEnumDeviceInterfaces(hDeviceInfo, 0, ref guid, deviceCount, ref SP_DEVICE_INTERFACE_DATA);
          size = 0;
          requiredSize = 0;
          SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref SP_DEVICE_INTERFACE_DATA, null, size, ref requiredSize, null);
          SetupDiGetDeviceInterfaceDetail(hDeviceInfo, ref SP_DEVICE_INTERFACE_DATA, null, size, ref requiredSize, null);
          //var hidHandle = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
          ret.Add(devicePath);
          //CloseHandle(hidHandle);
        }

        deviceCount++;
      } while (result != 0);

      SetupDiDestroyDeviceInfoList(hDeviceInfo);

      return ret;
    }

    public static bool IsDetected(int vendorId, int productId)
    {
      var device = new HIDDevice(vendorId, productId, false);
      bool ret = device.Found;

      device.Dispose();

      return ret;
    }

    public void Write(byte[] data)
    {
      /*
      int preparsedDataPtr = -1;
      HidD_GetPreparsedData(_hidHandle, ref preparsedDataPtr);
      var caps = new HIDP_CAPS();
      HidP_GetCaps(preparsedDataPtr, ref caps);
       */
      int outputReportByteLength = data.Length + 1; // caps.OutputReportByteLength;
      int bytesSent = 0;

      while (bytesSent < data.Length)
      {
        byte[] buffer = new byte[outputReportByteLength];
        buffer[0] = 0;
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

        int bytesWritten = 0;
        WriteFile(_hidHandle, ref buffer[0], buffer.Length, ref bytesWritten, 0);
      }
    }

    public void Disconnect()
    {
      if (_dataReadingThread != null)
      {
        _dataReadingThread.Abort();
        _dataReadingThread = null;
      }

      CloseHandle(_hidHandle);

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

    private unsafe void _Init(int vendorId, int productId, bool throwNotFoundError)
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

    private unsafe void _Init(string devicePath, bool throwNotFoundError)
    {
      int result;
      int deviceCount = 0;
      int size = 0;
      int requiredSize = 0;

      _guid = new GUID();
      HidD_GetHidGuid(ref _guid);

      _hDeviceInfo = SetupDiGetClassDevs(ref _guid, null, null, DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);

      do
      {
        _SP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
        _SP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(_SP_DEVICE_INTERFACE_DATA);
        result = SetupDiEnumDeviceInterfaces(_hDeviceInfo, 0, ref _guid, deviceCount, ref _SP_DEVICE_INTERFACE_DATA);
        SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, null, 0, ref requiredSize, null);
        size = requiredSize;
        _PSP_DEVICE_INTERFACE_DETAIL_DATA = new PSP_DEVICE_INTERFACE_DETAIL_DATA();
        _PSP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = 5;
        SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, ref _PSP_DEVICE_INTERFACE_DETAIL_DATA,
          size, ref requiredSize, null);
        _devicePath = _PSP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;

        if (_devicePath == devicePath)
        {
          _found = true;
          _SP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
          _SP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(_SP_DEVICE_INTERFACE_DATA);
          SetupDiEnumDeviceInterfaces(_hDeviceInfo, 0, ref _guid, deviceCount, ref _SP_DEVICE_INTERFACE_DATA);
          size = 0;
          requiredSize = 0;
          SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, null, size, ref requiredSize, null);
          SetupDiGetDeviceInterfaceDetail(_hDeviceInfo, ref _SP_DEVICE_INTERFACE_DATA, null, size, ref requiredSize, null);
          _hidHandle = CreateFile(_devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);

          break;
        }

        deviceCount++;
      } while (result != 0);

      if (!_found)
      {
        if (throwNotFoundError)
          throw new InvalidOperationException("Device not found");
      }
      else
      {
        _dataReadingThread = new Thread(new ThreadStart(_ReadDataThread));
        _dataReadingThread.Start();
      }
    }

    private unsafe void _ReadDataThread()
    {
      while (true)
      {
        int preparsedDataPtr = -1;
        if (HidD_GetPreparsedData(_hidHandle, ref preparsedDataPtr) != 0)
        {
          var caps = new HIDP_CAPS();
          HidP_GetCaps(preparsedDataPtr, ref caps);
          int reportLength = caps.InputReportByteLength;

          while (true)
          {
            byte[] receivedData = null;
            int bytesRead = 0;
            byte[] buffer = new byte[reportLength];

            if (ReadFile(_hidHandle, buffer, reportLength, ref bytesRead, null))
            {
              receivedData = new byte[bytesRead - 1];
              Array.Copy(buffer, 1, receivedData, 0, bytesRead - 1);
            }

            if (receivedData != null)
            {
              if (DataReceived != null)
                DataReceived(this, new DataReceivedEventArgs(receivedData));
            }

            //Don't do this...
            //Thread.Sleep(1);
          }
        }
      }
    }

    #endregion
  }
}
