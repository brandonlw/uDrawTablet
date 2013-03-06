using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xbox360USB;

namespace uDrawLib
{
  public class Xbox360InputDevice : IInputDevice
  {
    #region Declarations

    private const int _MULTITOUCH_SENSITIVITY = 30;
    private WirelessReceiver _receiver;
    private int _index;
    public WirelessReceiver.DeviceInformation Info { get; set; }

    //Chatpad statuses
    public bool PeopleLit { get; set; }
    public bool ShiftLit { get; set; }
    public bool OrangeLit { get; set; }
    public bool GreenLit { get; set; }
    public bool BacklightLit { get; set; }
    public bool PeopleHeld { get; set; }
    public bool ShiftHeld { get; set; }
    public bool OrangeHeld { get; set; }
    public bool GreenHeld { get; set; }

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

    public Xbox360InputDevice(WirelessReceiver receiver, int index, WirelessReceiver.DeviceInformation info)
    {
      ButtonState = new TabletButtonState();
      DPadState = new TabletDPadState();
      AccelerometerData = new TabletAccelerometerData();

      _index = index;
      _receiver = receiver;
      Info = info;
      _receiver.EventDataReceived += _receiver_EventDataReceived;
    }

    #endregion

    #region Public Events

    public event EventHandler<EventArgs> ButtonStateChanged;
    public event EventHandler<EventArgs> DPadStateChanged;
    public event EventHandler<ChatpadKeyStateEventArgs> ChatpadKeyStateChanged;
    public event EventHandler<Xbox360DeviceEventArgs> ShiftPressed;
    public event EventHandler<Xbox360DeviceEventArgs> PeoplePressed;

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

    private void _HandleKeyDownData(int index, byte key)
    {
      if (key > 0)
      {
        if (!_chatpadKeysHeld.Contains(key))
        {
          _chatpadKeysHeld.Add(key);

          //Let others know this key is now being held down
          if (ChatpadKeyStateChanged != null)
            ChatpadKeyStateChanged(this, new ChatpadKeyStateEventArgs(index, key, true));
        }
      }
    }

    private byte[] _oldChatpadData = new byte[29];
    private List<byte> _chatpadKeysHeld = new List<byte>();
    private void _receiver_EventDataReceived(object sender, Xbox360USB.DataReceivedEventArgs e)
    {
      if (Info.Subtype == WirelessReceiver.DeviceSubtype.Controller)
      {
        if (e.Data[0] == 0x00 && e.Data[2] == 0x00 && e.Data[3] == 0xF0)
        {
          if (e.Data[1] == 0x01)
          {
            //Parse raw data for buttons
            Console.WriteLine("Controller data: " + BitConverter.ToString(e.Data));

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
          else if (e.Data[1] == 0x02)
          {
            if (e.Data[24] == 0xF0 && e.Data[25] == 0x04)
            {
              //LED statuses
              this.PeopleLit = (e.Data[26] & 0x01) > 0;
              this.GreenLit = (e.Data[26] & 0x08) > 0;
              this.OrangeLit = (e.Data[26] & 0x10) > 0;
              this.ShiftLit = (e.Data[26] & 0x20) > 0;
              this.BacklightLit = (e.Data[26] & 0x80) > 0;
            }
            else if (e.Data[24] == 0x00)
            {
              //See if anything at all has changed
              bool changed = false;
              if (_oldChatpadData != null && _oldChatpadData.Length == e.Data.Length)
              {
                for (int i = 0; i < e.Data.Length; i++)
                {
                  if (_oldChatpadData[i] != e.Data[i])
                  {
                    changed = true;
                    break;
                  }
                }
              }
              else
                changed = true;

              if (changed)
              {
                //Get modifier statuses
                this.GreenHeld = (e.Data[25] & 0x02) > 0;
                this.OrangeHeld = (e.Data[25] & 0x04) > 0;
                bool raw = (e.Data[25] & 0x01) > 0;
                changed = this.ShiftHeld != raw;
                this.ShiftHeld = raw;
                if (changed && this.ShiftHeld && ShiftPressed != null)
                  ShiftPressed(this, new Xbox360DeviceEventArgs(e.Index));
                raw = (e.Data[25] & 0x08) > 0;
                changed = this.PeopleHeld != raw;
                this.PeopleHeld = raw;
                if (changed && this.PeopleHeld && PeoplePressed != null)
                  PeoplePressed(this, new Xbox360DeviceEventArgs(e.Index));

                //Raise scan code events
                var key1 = e.Data[26];
                var key2 = e.Data[27];
                _HandleKeyDownData(e.Index, key1);
                _HandleKeyDownData(e.Index, key2);
                var keysToRemove = new List<byte>();
                foreach (var key in _chatpadKeysHeld)
                  if (key != key1 && key != key2)
                    keysToRemove.Add(key);
                foreach (var key in keysToRemove)
                {
                  _chatpadKeysHeld.Remove(key);

                  //Let others know this key is no longer being held down
                  if (ChatpadKeyStateChanged != null)
                    ChatpadKeyStateChanged(this, new ChatpadKeyStateEventArgs(e.Index, key, false));
                }
              }

              _oldChatpadData = e.Data;
            }
          }
        }
      }
      else if (Info.Subtype == WirelessReceiver.DeviceSubtype.uDrawTablet)
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
        PressurePoint = new Point(e.Data[PRESSURE_DATA_OFFSET + 1] * 0x100 + e.Data[PRESSURE_DATA_OFFSET],
          e.Data[PRESSURE_DATA_OFFSET + 3] * 0x100 + e.Data[PRESSURE_DATA_OFFSET + 2]);

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
    }

    #endregion
  }
}
