using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace uDrawLib
{
  public class WiiInputDevice : IInputDevice, IDisposable
  {
    #region Declarations

    private const int _VENDOR_ID = 0x057E;
    private const int _PRODUCT_ID1 = 0x0306;
    private const int _PRODUCT_ID2 = 0x0330;
    private HIDDevice _device;
    private byte _lastReportingMode;
    private ulong? _extensionType;
    private Dictionary<byte, int> _acknowledgements;
    private int _bytesRemaining;
    private byte[] _bytesRead;
    private int _readError;

    /// <summary>
    /// Indicates the pressed state of every non-directional button on the tablet.
    /// </summary>
    public TabletButtonState ButtonState { get; set; }

    /// <summary>
    /// Indicates the pressed state of each direction on the directional pad.
    /// </summary>
    public TabletDPadState DPadState { get; set; }

    /// <summary>
    /// Indicates whether the nib/stylus, fingers, or nothing are currently pressing on the tablet.
    /// </summary>
    public TabletPressureType PressureType { get; set; }

    /// <summary>
    /// The raw pressure being applied by the pen. Only valid with TabletPressureType.PenPressed.
    /// </summary>
    public ushort PenPressure { get; set; }

    /// <summary>
    /// Indicates the distance between fingers. Only valid with TabletPressureType.Multitouch.
    /// </summary>
    public ushort MultitouchDistance { get; set; }

    /// <summary>
    /// Indicates the absolute coordinates (from top-left corner) of the current pressure point.
    /// Only valid with TabletPressureType.PenPressed.
    /// </summary>
    public Point PressurePoint { get; set; }

    /// <summary>
    /// Raw X-, Y-, and Z-axis data from the attached Wii remote's accelerometer.
    /// </summary>
    public TabletAccelerometerData AccelerometerData { get; set; }

    public ulong? ExtensionType
    {
      get
      {
        return _extensionType;
      }
    }

    public int? Index { get; set; }

    #endregion

    #region Constructors / Teardown

    public WiiInputDevice(string devicePath, int? index)
    {
      ButtonState = new TabletButtonState();
      DPadState = new TabletDPadState();
      AccelerometerData = new TabletAccelerometerData();
      _acknowledgements = new Dictionary<byte, int>();

      _device = new HIDDevice(devicePath);

      //Initialize the device
      Index = index;
      _device.DataReceived += _device_DataReceived;
      if (index.HasValue)
      {
        EnableLEDs(index.Value == 0, index.Value == 1, index.Value == 2, index.Value == 3);
      }

      //Get the initial status
      _lastReportingMode = 0x37;
      RefreshStatus();
    }

    #endregion

    #region Public Events

    public event EventHandler<EventArgs> ButtonStateChanged;
    public event EventHandler<EventArgs> DPadStateChanged;

    #endregion

    #region Public Properties

    /// <summary>
    /// Indicates whether pressure is being applied to the tablet.
    /// </summary>
    public bool IsPressed
    {
      get
      {
        bool ret = false;

        if (PressureType != TabletPressureType.NotPressed)
          ret = true;

        return ret;
      }
    }

    /// <summary>
    /// Indicates whether the user is currently zooming out by multitouch.
    /// </summary>
    public bool IsZooming
    {
      get
      {
        //No multitouch supported
        return false;
      }
    }

    /// <summary>
    /// Indicates whether the user is currently pinching by multitouch.
    /// </summary>
    public bool IsPinching
    {
      get
      {
        //No multitouch supported
        return false;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Detects whether the Wii uDraw tablet device is currently attached to the PC.
    /// This is detecting the presence of a Wii remote with uDraw tablet attached.
    /// </summary>
    /// <returns></returns>
    public static bool IsDetected()
    {
      return GetAllDevices().Count > 0;
    }

    public static List<string> GetAllDevices()
    {
      var ret = new List<string>();
      
      var candidates = HIDDevice.GetAllDevices(_VENDOR_ID, _PRODUCT_ID1);
      foreach (var c in HIDDevice.GetAllDevices(_VENDOR_ID, _PRODUCT_ID2))
      {
        candidates.Add(c);
      }

      foreach (var device in candidates)
      {
        var d = new WiiInputDevice(device, null);
        Thread.Sleep(1000);
        bool found = d.ExtensionType.HasValue && d.ExtensionType.Value == 0xFF00A4200112;
        d.Disconnect();
        d.Dispose();
        d = null;

        if (found)
        {
          ret.Add(device);
        }
      }

      return ret;
    }

    public void EnableLEDs(bool led1, bool led2, bool led3, bool led4)
    {
      _SendCommand(0x11, new byte[] {(byte)((led1 ? 0x10 : 0x00) | (led2 ? 0x20 : 0x00) |
        (led3 ? 0x40 : 0x00) | (led4 ? 0x80 : 0x00))}, true);
    }

    public void RefreshStatus()
    {
      _SendCommand(0x15, null, true);
    }

    public void SetReportingMode(byte reportingMode)
    {
      _lastReportingMode = reportingMode;

      _SendCommand(0x12, new byte[] { 0x02, reportingMode }, true);

      _WaitForAcknowledgement(0x12, true);
    }

    public void InitializeExtension()
    {
      WriteMemory(0xA400F0, true, new byte[] { 0x55 }, false);
      WriteMemory(0xA400FB, true, new byte[] { 0x00 }, false);
    }

    public void WriteMemory(int address, bool controlRegisterSpace, byte[] data, bool throwExceptionOnTimeout)
    {
      var buffer = new byte[(data != null ? data.Length : 0) + 6];
      buffer[0] = (byte)(controlRegisterSpace ? 0x04 : 0x00);
      buffer[1] = (byte)((address >> 16) & 0xFF);
      buffer[2] = (byte)((address >> 8) & 0xFF);
      buffer[3] = (byte)(address & 0xFF);
      buffer[4] = (byte)(data.Length & 0xFF);
      if (data != null)
      {
        Array.Copy(data, 0, buffer, 5, data.Length);
      }

      _SendCommand(0x16, buffer, true);

      var ret = _WaitForAcknowledgement(0x16, throwExceptionOnTimeout);
      if (ret != 0)
      {
        throw new InvalidOperationException("Error writing to address: " + ret.ToString("X02"));
      }
    }

    public byte[] ReadMemory(int address, bool controlRegisterSpace, int numberOfBytes)
    {
      var buffer = new byte[6];
      buffer[0] = (byte)(controlRegisterSpace ? 0x04 : 0x00);
      buffer[1] = (byte)((address >> 16) & 0xFF);
      buffer[2] = (byte)((address >> 8) & 0xFF);
      buffer[3] = (byte)(address & 0xFF);
      buffer[4] = (byte)((numberOfBytes >> 8) & 0xFF);
      buffer[5] = (byte)(numberOfBytes & 0xFF);

      _bytesRead = new byte[numberOfBytes];
      _bytesRemaining = numberOfBytes;
      _readError = 0;
      _SendCommand(0x17, buffer, true);

      return _WaitForDataRead(false);
    }

    /// <summary>
    /// Disconnects from the Wii remote.
    /// </summary>
    public void Disconnect()
    {
      _device.Disconnect();
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

    private void _GetExtensionType()
    {
      var data = ReadMemory(0xA400FA, true, 6);

      _extensionType = null;
      if (_readError == 0 && data != null && data.Length == 6)
      {
        _extensionType = (ulong)data[5] & (ulong)0x00000000FF;
        _extensionType |= (ulong)((ulong)data[4] << 8) & (ulong)0x00000000FF00;
        _extensionType |= (ulong)((ulong)data[3] << 16) & (ulong)0x000000FF0000;
        _extensionType |= (ulong)((ulong)data[2] << 24) & (ulong)0x0000FF000000;
        _extensionType |= (ulong)((ulong)data[1] << 32) & (ulong)0x00FF00000000;
        _extensionType |= (ulong)((ulong)data[0] << 40) & (ulong)0xFF0000000000;
      }
    }

    private void _SendCommand(byte reportType, byte[] data, bool pauseBriefly)
    {
      var buffer = new byte[data != null ? data.Length : 0];
      if (data != null)
      {
        Array.Copy(data, 0, buffer, 0, Math.Min(data.Length, buffer.Length));
      }

      _device.Write(reportType, buffer);

      if (pauseBriefly)
      {
        Thread.Sleep(100);
      }
    }

    private byte[] _WaitForDataRead(bool throwExceptionOnError)
    {
      const int TIMEOUT_MSECS = 2000;
      var start = DateTime.Now;

      while (_bytesRemaining > 0)
      {
        if (_readError != 0)
        {
          if (throwExceptionOnError)
          {
            throw new InvalidOperationException("Error while reading data: " + _readError.ToString("X02"));
          }
          else
          {
            break;
          }
        }

        if (DateTime.Now > start.AddMilliseconds(TIMEOUT_MSECS))
        {
          if (throwExceptionOnError)
          {
            throw new TimeoutException("Timed out waiting for data read");
          }
          else
          {
            break;
          }
        }

        Thread.Sleep(100);
      }

      return _bytesRead;
    }

    private int _WaitForAcknowledgement(byte reportType, bool throwExceptionOnTimeout)
    {
      const int TIMEOUT_MSECS = 2000;
      int? ret = null;
      var start = DateTime.Now;

      while (!ret.HasValue)
      {
        if (DateTime.Now > start.AddMilliseconds(TIMEOUT_MSECS))
        {
          if (throwExceptionOnTimeout)
          {
            throw new TimeoutException("Timed out waiting for response to output report " + reportType.ToString("X02"));
          }
          else
          {
            break;
          }
        }

        lock (_acknowledgements)
        {
          if (_acknowledgements.ContainsKey(reportType))
          {
            ret = _acknowledgements[reportType];
            _acknowledgements.Remove(reportType);

            break;
          }
        }

        Thread.Sleep(100);
      }

      return ret.GetValueOrDefault();
    }

    private byte[] _GetData(byte[] data, int offset, int length)
    {
      var ret = new byte[length];

      Array.Copy(data, offset, ret, 0, length);

      return ret;
    }

    private bool _ParseButtonState(byte[] data)
    {
      bool changed = false;
      bool raw;

      raw = (data[1] & 0x02) > 0;
      changed |= ButtonState.SquareHeld != raw; ButtonState.SquareHeld = raw;
      raw = (data[1] & 0x08) > 0;
      changed |= ButtonState.CrossHeld != raw; ButtonState.CrossHeld = raw;
      raw = (data[1] & 0x04) > 0;
      changed |= ButtonState.CircleHeld != raw; ButtonState.CircleHeld = raw;
      raw = (data[1] & 0x01) > 0;
      changed |= ButtonState.TriangleHeld != raw; ButtonState.TriangleHeld = raw;
      raw = (data[1] & 0x10) > 0;
      changed |= ButtonState.SelectHeld != raw; ButtonState.SelectHeld = raw;
      raw = (data[0] & 0x10) > 0;
      changed |= ButtonState.StartHeld != raw; ButtonState.StartHeld = raw;
      raw = (data[1] & 0x80) > 0;
      changed |= ButtonState.PSHeld != raw; ButtonState.PSHeld = raw;

      return changed;
    }

    private bool _ParseDPadState(byte[] data)
    {
      bool changed = false;
      bool raw;

      raw = (data[0] & 0x08) > 0;
      changed |= DPadState.UpHeld != raw; DPadState.UpHeld = raw;
      raw = (data[0] & 0x04) > 0;
      changed |= DPadState.DownHeld != raw; DPadState.DownHeld = raw;
      raw = (data[0] & 0x01) > 0;
      changed |= DPadState.LeftHeld != raw; DPadState.LeftHeld = raw;
      raw = (data[0] & 0x02) > 0;
      changed |= DPadState.RightHeld != raw; DPadState.RightHeld = raw;

      return changed;
    }

    private void _HandleExtensionConnected()
    {
      //Apparently it doesn't like it if we do this too fast
      Thread.Sleep(500);

      //Initialize it
      InitializeExtension();

      //Get the extension type
      _GetExtensionType();
    }

    private void _ParseTabletState(byte[] data)
    {
      const int HORIZONTAL_PRESSURE_DATA_OFFSET = 0;
      const int VERTICAL_PRESSURE_DATA_OFFSET = 1;
      const int POSITION_BOX_OFFSET = 2;
      const int PEN_PRESSURE_OFFSET = 3;
      //const int BUTTON_DATA_OFFSET = 5;
      const int FIRST_BOX_START = 0x62;

      //Ge(t the pressure state
      if (data[POSITION_BOX_OFFSET] == 0xFF)
        PressureType = TabletPressureType.NotPressed;
      else
        PressureType = TabletPressureType.PenPressed;

      //Get the pen pressure
      PenPressure = (ushort)(data[PEN_PRESSURE_OFFSET]);

      //Get the (singular) pressure point
      var xBox = (data[POSITION_BOX_OFFSET] & 0x0F);
      var yBox = ((data[POSITION_BOX_OFFSET] >> 4) & 0x0F);
      var xPosition = data[HORIZONTAL_PRESSURE_DATA_OFFSET];
      var yPosition = data[VERTICAL_PRESSURE_DATA_OFFSET];
      var x = xBox == 0 ? xPosition - FIRST_BOX_START : (0x100 - FIRST_BOX_START) + ((xBox - 1) * 0x100) + xPosition;
      var y = yBox == 0 ? yPosition - FIRST_BOX_START : (0x100 - FIRST_BOX_START) + ((yBox - 1) * 0x100) + yPosition;
      PressurePoint = new Point(x, 1340 - y);
    }

    #endregion

    #region Event Handlers

    private void _device_DataReceived(object sender, DataReceivedEventArgs e)
    {
      bool dpadChanged = false;
      bool buttonsChanged = false;

      switch (e.ReportType)
      {
        case 0x20:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);
            var isExtensionConnected = (e.Data[2] & 0x02) > 0;
            bool changed = isExtensionConnected != _extensionType.HasValue;

            var th = new Thread(new ThreadStart(() => { try { SetReportingMode(_lastReportingMode); } catch { } }));
            th.IsBackground = true;
            th.Start();

            if (changed)
            {
              if (isExtensionConnected)
              {
                try
                {
                  th = new Thread(new ThreadStart(_HandleExtensionConnected));
                  th.IsBackground = true;
                  th.Start();
                }
                catch
                {
                }
              }
              else
              {
                _extensionType = null;
              }
            }

            break;
          }
        case 0x21:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            int numberOfBytes = (e.Data[2] >> 4) + 1;
            _readError = e.Data[2] & 0x0F;
            if (_bytesRemaining >= numberOfBytes)
            {
              Array.Copy(e.Data, 5, _bytesRead, _bytesRead.Length - _bytesRemaining, numberOfBytes);
            }

            _bytesRemaining -= numberOfBytes;

            break;
          }
        case 0x22:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            lock (_acknowledgements)
            {
              var reportType = e.Data[2];
              var errorCode = e.Data[3];

              if (_acknowledgements.ContainsKey(reportType))
              {
                _acknowledgements.Remove(reportType);
              }

              _acknowledgements.Add(reportType, errorCode);
            }
            break;
          }
        case 0x30:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);
            break;
          }
        case 0x31:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);
            break;
          }
        case 0x32:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            _ParseTabletState(_GetData(e.Data, 2, 6));
            break;
          }
        case 0x33:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);
            break;
          }
        case 0x34:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            _ParseTabletState(_GetData(e.Data, 2, 6));
            break;
          }
        case 0x35:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            _ParseTabletState(_GetData(e.Data, 5, 6));
            break;
          }
        case 0x36:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            _ParseTabletState(_GetData(e.Data, 12, 6));
            break;
          }
        case 0x37:
          {
            var data = _GetData(e.Data, 0, 2);
            dpadChanged |= _ParseDPadState(data);
            buttonsChanged |= _ParseButtonState(data);

            _ParseTabletState(_GetData(e.Data, 15, 6));
            break;
          }
        default:
          {
            break;
          }
      }

      if (buttonsChanged && ButtonStateChanged != null)
        ButtonStateChanged(this, EventArgs.Empty);

      if (dpadChanged && DPadStateChanged != null)
        DPadStateChanged(this, EventArgs.Empty);
    }

    #endregion
  }
}
