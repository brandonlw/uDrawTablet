using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xbox360USB;

namespace uDrawLib
{
  public class Xbox360uDrawTabletDevice : ITabletDevice
  {
    #region Declarations

    private const int _MULTITOUCH_SENSITIVITY = 30;
    private WirelessReceiver _receiver;
    private int _index;

    private enum RawPressureType
    {
      NotPressed = 0x00,
      PenPressed = 0x40,
      FingerPressed = 0x80
    };

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
    /// Only valid with TabletPressureType.PenPressed and TabletPressureType.FingerPressed.
    /// </summary>
    public Point PressurePoint { get; set; }

    /// <summary>
    /// Raw X-, Y-, and Z-axis data from the accelerometer.
    /// </summary>
    public TabletAccelerometerData AccelerometerData { get; set; }

    private int _multitouchTimer;
    private ushort _previousMultitouchDistance;

    #endregion

    #region Constructors / Teardown

    public Xbox360uDrawTabletDevice(WirelessReceiver receiver, int index)
    {
      ButtonState = new TabletButtonState();
      DPadState = new TabletDPadState();
      AccelerometerData = new TabletAccelerometerData();

      _index = index;
      _receiver = receiver;
      _receiver.EventDataReceived += _receiver_EventDataReceived;
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
        bool ret = false;

        if (MultitouchDistance > _previousMultitouchDistance)
          ret = true;

        return ret;
      }
    }

    /// <summary>
    /// Indicates whether the user is currently pinching by multitouch.
    /// </summary>
    public bool IsPinching
    {
      get
      {
        bool ret = false;

        if (MultitouchDistance < _previousMultitouchDistance)
          ret = true;

        return ret;
      }
    }

    #endregion

    #region Event Handlers

    private void _receiver_EventDataReceived(object sender, Xbox360USB.DataReceivedEventArgs e)
    {
      const int MULTITOUCH_DISTANCE_OFFSET = 15;
      const int PEN_PRESSURE_OFFSET = 8;
      const int PRESSURE_DATA_OFFSET = 10;
      const int ACCELEROMETER_X_OFFSET = 16;
      const int ACCELEROMETER_Y_OFFSET = 17;
      const int ACCELEROMETER_Z_OFFSET = 9;
      const int PRESSURE_STATE = 14;

      //Get the pressure state
      if (e.Data[PRESSURE_STATE] == (byte)RawPressureType.NotPressed)
        PressureType = TabletPressureType.NotPressed;
      else if (e.Data[PRESSURE_STATE] == (byte)RawPressureType.PenPressed)
        PressureType = TabletPressureType.PenPressed;
      else if (e.Data[PRESSURE_STATE] == (byte)RawPressureType.FingerPressed)
        PressureType = TabletPressureType.FingerPressed;
      else
        PressureType = TabletPressureType.Multitouch;

      //Get the pen pressure
      PenPressure = (ushort)(e.Data[PEN_PRESSURE_OFFSET]);

      //Get the multitouch distance
      _multitouchTimer++;
      if (_multitouchTimer > _MULTITOUCH_SENSITIVITY)
      {
        _multitouchTimer = 0;
        _previousMultitouchDistance = MultitouchDistance;
      }
      MultitouchDistance = e.Data[MULTITOUCH_DISTANCE_OFFSET];

      //Get the (singular) pressure point
      PressurePoint = new Point(e.Data[PRESSURE_DATA_OFFSET+1] * 0x100 + e.Data[PRESSURE_DATA_OFFSET],
        e.Data[PRESSURE_DATA_OFFSET+3] * 0x100 + e.Data[PRESSURE_DATA_OFFSET+2]);

      //Get the accelerometer data
      AccelerometerData.XAxis = (ushort)(e.Data[ACCELEROMETER_X_OFFSET]);
      AccelerometerData.YAxis = (ushort)(e.Data[ACCELEROMETER_Y_OFFSET]);
      AccelerometerData.ZAxis = (ushort)(e.Data[ACCELEROMETER_Z_OFFSET]);

      //Parse raw data for buttons
      bool changed = false;
      bool raw = (e.Data[7] & 0x40) > 0;
      changed |= ButtonState.SquareHeld != raw; ButtonState.SquareHeld = raw;
      raw = (e.Data[7] & 0x10) > 0;
      changed |= ButtonState.CrossHeld != raw; ButtonState.CrossHeld = raw;
      raw = (e.Data[7] & 0x20) > 0;
      changed |= ButtonState.CircleHeld != raw; ButtonState.CircleHeld = raw;
      raw = (e.Data[7] & 0x80) > 0;
      changed |= ButtonState.TriangleHeld != raw; ButtonState.TriangleHeld = raw;
      raw = (e.Data[6] & 0x20) > 0;
      changed |= ButtonState.SelectHeld != raw; ButtonState.SelectHeld = raw;
      raw = (e.Data[6] & 0x10) > 0;
      changed |= ButtonState.StartHeld != raw; ButtonState.StartHeld = raw;
      raw = (e.Data[7] & 0x04) > 0;
      changed |= ButtonState.PSHeld != raw; ButtonState.PSHeld = raw;

      if (changed && ButtonStateChanged != null)
        ButtonStateChanged(this, EventArgs.Empty);

      //Now parse raw data for D-pad changes
      changed = false;
      raw = (e.Data[6] == 0x1) || (e.Data[6] == 0x5) || (e.Data[6] == 0x9);
      changed |= DPadState.UpHeld != raw; DPadState.UpHeld = raw;
      raw = (e.Data[6] == 0x2) || (e.Data[6] == 0x6) || (e.Data[6] == 0xA);
      changed |= DPadState.DownHeld != raw; DPadState.DownHeld = raw;
      raw = (e.Data[6] == 0x4) || (e.Data[6] == 0x6) || (e.Data[6] == 0x5);
      changed |= DPadState.LeftHeld != raw; DPadState.LeftHeld = raw;
      raw = (e.Data[6] == 0x8) || (e.Data[6] == 0x9) || (e.Data[6] == 0xA);
      changed |= DPadState.RightHeld != raw; DPadState.RightHeld = raw;

      if (changed && DPadStateChanged != null)
        DPadStateChanged(this, EventArgs.Empty);
    }

    #endregion
  }
}
