using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace uDrawTablet
{
  public static class PPJoyInterface
  {
    #region P/Invoke Crud

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct JoystickState
    {
      [MarshalAs(UnmanagedType.U4, SizeConst = 1)]
      internal uint Signature;
      [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
      internal char NumAnalog;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
      internal int[] Analog;
      [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
      internal char NumDigital;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
      internal char[] Digital;
    };

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr SecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("Kernel32.dll", EntryPoint = "DeviceIoControl", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeviceIoControl(Microsoft.Win32.SafeHandles.SafeFileHandle hDevice,
        uint IoControlCode,
        [MarshalAs(UnmanagedType.AsAny)]
        [In] object InBuffer,
        uint nInBufferSize,
        [MarshalAs(UnmanagedType.AsAny)]
        [Out] object OutBuffer,
        uint nOutBufferSize,
        ref uint pBytesReturned,
        [In] ref System.Threading.NativeOverlapped Overlapped);

    #endregion

    #region Declarations

    private static Dictionary<int, SafeFileHandle> _handles;

    #endregion

    #region Constructors / Teardown

    static PPJoyInterface()
    {
      _handles = new Dictionary<int, SafeFileHandle>();
    }

    #endregion

    #region Public Methods

    public static unsafe void Update(int index, int[] analogData, byte[] digitalData)
    {
      //Create the structure
      var state = new JoystickState();
      state.Signature = (uint)0x53544143;
      state.NumAnalog = (char)63;
      state.NumDigital = (char)128;
      state.Analog = new int[state.NumAnalog];
      state.Digital = new char[state.NumDigital];

      //Fill it with our data
      if (analogData != null)
        Array.Copy(analogData, 0, state.Analog, 0, Math.Min(analogData.Length, state.Analog.Length));
      if (digitalData != null)
        Array.Copy(digitalData, 0, state.Digital, 0, Math.Min(digitalData.Length, state.Digital.Length));

      //Send the data
      uint bytesReturned = 0;
      NativeOverlapped overlapped = new NativeOverlapped();
      var h = _GetHandle(index);
      bool ret = DeviceIoControl(h, 0x00220000,
         state, (uint)Marshal.SizeOf(state),
         IntPtr.Zero, 0, ref bytesReturned, ref overlapped);

      if (!ret)
      {
        //TODO: Do something with this?
        int lastError = Marshal.GetLastWin32Error();

        //Invalidate the handle
        _CloseHandle(h);
        _handles[index] = null;
      }
    }

    public static void CloseAllHandles()
    {
      foreach (var h in _handles.Values)
        _CloseHandle(h);
    }

    #endregion

    #region Private Methods

    private static SafeFileHandle _CreateHandle(int index)
    {
      return CreateFile("\\\\.\\PPJoyIOCTL" + index.ToString(), 0x00120116, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
    }

    private static SafeFileHandle _GetHandle(int index)
    {
      SafeFileHandle ret = new SafeFileHandle(IntPtr.Zero, true);

      lock (_handles)
      {
        if (_handles != null)
        {
          if (!_handles.ContainsKey(index))
          {
            _handles.Add(index, _CreateHandle(index));
          }
          else
            ret = _handles[index];

          if (ret == null || ret.IsClosed || ret.IsInvalid)
          {
            //Close old handle
            _CloseHandle(ret);

            //Try again
            ret = _CreateHandle(index);
            _handles[index] = ret;
          }
        }
      }

      return ret;
    }

    private static void _CloseHandle(SafeFileHandle h)
    {
      try
      {
        if (h != null)
        {
          h.Close();
          h.Dispose();
        }
      }
      catch
      {
        //Eat...
      }
    }

    #endregion
  }
}
