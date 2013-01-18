using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using uDrawLib;
using Xbox360USB;

namespace uDrawTablet
{
  public static class MouseInterface
  {
    #region P/Invoke Crud

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;

      public static implicit operator Point(POINT point)
      {
        return new Point(point.X, point.Y);
      }
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

    private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    private const int MOUSEEVENTF_MOVE = 0x0001;
    private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const int MOUSEEVENTF_LEFTUP = 0x0004;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const int MOUSEEVENTF_RIGHTUP = 0x0010;

    #endregion

    #region Declarations

    private const int _MIN_PEN_PRESSURE_THRESHOLD = 0xD0;
    private const int _MAX_PEN_PRESSURE_THRESHOLD = 0xFF;
    private static Options _frmOptions;
    private static System.Threading.Timer _timer = null;
    public static WirelessReceiver Receiver { get; set; }
    public static List<TabletConnection> Tablets;

    #endregion

    #region Constructors/Teardown

    static MouseInterface()
    {
      Tablets = new List<TabletConnection>();
    }

    #endregion

    #region Public Methods

    public static string GetSettingsFileName(bool isPS3, int? index)
    {
      string ret = String.Empty;

      if (isPS3)
        ret = "PS3.ini";
      else if (index.HasValue)
      {
        //Find the tablet with this index
        foreach (var t in Tablets)
        {
          if (t.ReceiverIndex == index.Value)
          {
            //Use its serial number
            var info = Receiver.GetDeviceInformation(t.ReceiverIndex);

            if (info != null)
              ret = String.Format("X360_{0}.ini", info.ToString());
            break;
          }
        }
      }

      return ret;
    }

    public static void ReloadSettings()
    {
      foreach (var t in Tablets)
        t.Settings = TabletSettings.LoadSettings(GetSettingsFileName(
          t.Tablet as PS3uDrawTabletDevice != null, t.ReceiverIndex));
    }

    public static void Start(Options options)
    {
      Stop();

      _frmOptions = options;

      //Set up Xbox 360 USB wireless receiver
      if (Receiver == null || !Receiver.IsReceiverConnected)
      {
        Receiver = new Xbox360USB.WirelessReceiver();
        Receiver.DeviceConnected += Receiver_DeviceConnected;
        Receiver.DeviceDisconnected += Receiver_DeviceDisconnected;
        Receiver.Start();
      }

      //Set up the PS3 tablet dongle
      var conn = new TabletConnection((new PS3uDrawTabletDevice()) as ITabletDevice);
      conn.ButtonStateChanged += _ButtonStateChanged;
      conn.DPadStateChanged += _DPadStateChanged;
      conn.Settings = TabletSettings.LoadSettings(GetSettingsFileName(true, null));
      Tablets.Add(conn);

      //Set up the event timer
      _timer = new System.Threading.Timer(new TimerCallback(_HandleTabletEvents), null, 0, 1);
    }

    public static void Stop()
    {
      //Dispose of the PS3 tablet
      foreach (var t in Tablets)
      {
        var ps3 = t.Tablet as PS3uDrawTabletDevice;

        if (ps3 != null)
          ps3.Dispose();
      }

      //Dispose of the Xbox 360 wireless USB receiver
      if (Receiver != null)
      {
        Receiver.Dispose();
        Receiver = null;
      }

      Tablets.Clear();
    }

    #endregion

    #region Event Handlers

    private static void _ButtonStateChanged(object sender, EventArgs e)
    {
      var conn = (TabletConnection)sender;

      if (conn.Tablet.ButtonState.CrossHeld != conn.LastButtonState.CrossHeld)
        _PerformAction(conn, conn.Settings.AAction, conn.Tablet.ButtonState.CrossHeld);
      if (conn.Tablet.ButtonState.CircleHeld != conn.LastButtonState.CircleHeld)
        _PerformAction(conn, conn.Settings.BAction, conn.Tablet.ButtonState.CircleHeld);
      if (conn.Tablet.ButtonState.SquareHeld != conn.LastButtonState.SquareHeld)
        _PerformAction(conn, conn.Settings.XAction, conn.Tablet.ButtonState.SquareHeld);
      if (conn.Tablet.ButtonState.TriangleHeld != conn.LastButtonState.TriangleHeld)
        _PerformAction(conn, conn.Settings.YAction, conn.Tablet.ButtonState.TriangleHeld);
      if (conn.Tablet.ButtonState.StartHeld != conn.LastButtonState.StartHeld)
        _PerformAction(conn, conn.Settings.StartAction, conn.Tablet.ButtonState.StartHeld);
      if (conn.Tablet.ButtonState.SelectHeld != conn.LastButtonState.SelectHeld)
        _PerformAction(conn, conn.Settings.BackAction, conn.Tablet.ButtonState.SelectHeld);
      if (conn.Tablet.ButtonState.PSHeld != conn.LastButtonState.PSHeld)
        _PerformAction(conn, conn.Settings.GuideAction, conn.Tablet.ButtonState.PSHeld);
    }

    private static void _DPadStateChanged(object sender, EventArgs e)
    {
      var conn = (TabletConnection)sender;

      if (conn.Tablet.DPadState.UpHeld != conn.LastDPadState.UpHeld)
        _PerformAction(conn, conn.Settings.UpAction, conn.Tablet.DPadState.UpHeld);
      if (conn.Tablet.DPadState.DownHeld != conn.LastDPadState.DownHeld)
        _PerformAction(conn, conn.Settings.DownAction, conn.Tablet.DPadState.DownHeld);
      if (conn.Tablet.DPadState.LeftHeld != conn.LastDPadState.LeftHeld)
        _PerformAction(conn, conn.Settings.LeftAction, conn.Tablet.DPadState.LeftHeld);
      if (conn.Tablet.DPadState.RightHeld != conn.LastDPadState.RightHeld)
        _PerformAction(conn, conn.Settings.RightAction, conn.Tablet.DPadState.RightHeld);
    }

    private static void _PerformAction(TabletConnection conn, TabletOptionButton.ButtonAction action, bool held)
    {
      switch (action)
      {
        case TabletOptionButton.ButtonAction.LeftClick:
          mouse_event(held ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.RightClick:
          mouse_event(held ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.ShowOptions:
          if (held) _frmOptions.ShowOptions();
          break;
        case TabletOptionButton.ButtonAction.TurnOffTablet:
          if (conn.Receiver != null && held) conn.Receiver.TurnOffDevice(conn.ReceiverIndex);
          break;
        default:
          break;
      }
    }

    #endregion

    #region Local Methods

    //Thread off 360 connected handler.
    private static void Receiver_DeviceConnected(object sender, DeviceEventArgs e)
    {
      var th = new Thread(new ParameterizedThreadStart(_HandleConnected));
      th.IsBackground = true;
      th.Start(e.Index);
    }

    //Xbox 360 device connection handler
    private static void _HandleConnected(object i)
    {
      int index = (int)i;
      bool shouldHandle = false;

      if (Receiver.IsDeviceConnected(index))
      {
        var info = Receiver.GetDeviceInformation(index);

        if (info != null && info.Subtype == WirelessReceiver.DeviceSubtype.uDrawTablet)
          shouldHandle = true;

        foreach (var t in Tablets)
        {
          if (t.ReceiverIndex == index)
          {
            shouldHandle = false;
            break;
          }
        }
      }

      if (shouldHandle)
        _Handle360TabletConnect(index);
    }

    //Xbox 360 device disconnection handler
    private static void Receiver_DeviceDisconnected(object sender, DeviceEventArgs e)
    {
      _Handle360TabletDisconnect(e.Index);
    }

    private static void _Handle360TabletConnect(int index)
    {
      _Handle360TabletDisconnect(index);

      var connection = new TabletConnection((new Xbox360uDrawTabletDevice(Receiver, index)) as ITabletDevice, Receiver, index);
      connection.ButtonStateChanged += _ButtonStateChanged;
      connection.DPadStateChanged += _DPadStateChanged;
      Tablets.Add(connection);
      connection.Settings = TabletSettings.LoadSettings(GetSettingsFileName(false, index));
    }

    private static void _Handle360TabletDisconnect(int index)
    {
      TabletConnection conn = null;

      foreach (var t in Tablets)
      {
        if (t.ReceiverIndex == index)
        {
          t.ButtonStateChanged -= _ButtonStateChanged;
          t.DPadStateChanged -= _DPadStateChanged;
          conn = t;
          break;
        }
      }

      if (conn != null)
        Tablets.Remove(conn);
    }

    private static void _HandleTabletEvents(object target)
    {
      foreach (var t in Tablets)
      {
        _HandleTabletEvents(t);
      }
    }

    private static bool _IsActionRequested(TabletConnection conn, TabletOptionButton.ButtonAction action)
    {
      bool ret = false;

      if (conn.Settings.AAction == action)
        ret |= conn.Tablet.ButtonState.CrossHeld;
      if (conn.Settings.BAction == action)
        ret |= conn.Tablet.ButtonState.CircleHeld;
      if (conn.Settings.XAction == action)
        ret |= conn.Tablet.ButtonState.SquareHeld;
      if (conn.Settings.YAction == action)
        ret |= conn.Tablet.ButtonState.TriangleHeld;
      if (conn.Settings.UpAction == action)
        ret |= conn.Tablet.DPadState.UpHeld;
      if (conn.Settings.DownAction == action)
        ret |= conn.Tablet.DPadState.DownHeld;
      if (conn.Settings.LeftAction == action)
        ret |= conn.Tablet.DPadState.LeftHeld;
      if (conn.Settings.RightAction == action)
        ret |= conn.Tablet.DPadState.RightHeld;
      if (conn.Settings.StartAction == action)
        ret |= conn.Tablet.ButtonState.StartHeld;
      if (conn.Settings.BackAction == action)
        ret |= conn.Tablet.ButtonState.SelectHeld;
      if (conn.Settings.GuideAction == action)
        ret |= conn.Tablet.ButtonState.PSHeld;

      return ret;
    }

    private static int _accel = 1;
    private static void _HandleTabletEvents(TabletConnection conn)
    {
      if (conn != null)
      {
        const double TABLET_PAD_WIDTH = 1920;
        const double TABLET_PAD_HEIGHT = 1080;

        double threshold = ((conn.Settings.PenPressureThreshold / 10.0) *
          (_MAX_PEN_PRESSURE_THRESHOLD - _MIN_PEN_PRESSURE_THRESHOLD)) + _MIN_PEN_PRESSURE_THRESHOLD;
        if (conn.LastPressure != (conn.Tablet.PenPressure >= threshold))
          mouse_event(conn.Tablet.PenPressure >= threshold ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        conn.LastPressure = (conn.Tablet.PenPressure >= threshold);

        bool doUp = _IsActionRequested(conn, TabletOptionButton.ButtonAction.MoveUp);
        bool doDown = _IsActionRequested(conn, TabletOptionButton.ButtonAction.MoveDown);
        bool doLeft = _IsActionRequested(conn, TabletOptionButton.ButtonAction.MoveLeft);
        bool doRight = _IsActionRequested(conn, TabletOptionButton.ButtonAction.MoveRight);

        if ((conn.Tablet.PressureType == TabletPressureType.PenPressed) ||
          (conn.Settings.AllowFingerMovement && conn.Tablet.PressureType == TabletPressureType.FingerPressed))
        {
          if (conn.Settings.MovementType == TabletSettings.TabletMovementType.Absolute)
          {
            //Calculate the absolute coordinates of the new mouse position
            double x = (conn.Tablet.PressurePoint.X / TABLET_PAD_WIDTH) * 65536;
            double y = (conn.Tablet.PressurePoint.Y / TABLET_PAD_HEIGHT) * 65536;

            if (Cursor.Position.X != x && Cursor.Position.Y != y)
              mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, (int)x, (int)y, 0, UIntPtr.Zero);
          }
          else if (conn.Settings.MovementType == TabletSettings.TabletMovementType.Relative)
          {
            //Based on last position, determine whether to move in a certain direction or not
            if (conn.LastPressurePoint.HasValue)
            {
              const int DELTA = 1;
              int precision = conn.Settings.Precision;
              if ((conn.Tablet.PressurePoint.X - conn.LastPressurePoint.Value.X) >= precision)
                conn.HorizontalDelta++;
              if ((conn.Tablet.PressurePoint.Y - conn.LastPressurePoint.Value.Y) >= precision)
                conn.VerticalDelta++;
              if ((conn.Tablet.PressurePoint.X - conn.LastPressurePoint.Value.X) <= -precision)
                conn.HorizontalDelta--;
              if ((conn.Tablet.PressurePoint.Y - conn.LastPressurePoint.Value.Y) <= -precision)
                conn.VerticalDelta--;

              if (conn.VerticalDelta <= -DELTA) doUp = true;
              if (conn.VerticalDelta >= DELTA) doDown = true;
              if (conn.HorizontalDelta <= -DELTA) doLeft = true;
              if (conn.HorizontalDelta >= DELTA) doRight = true;
            }

            if (!conn.LastPressurePoint.HasValue)
              conn.LastPressurePoint = new Point(conn.Tablet.PressurePoint.X, conn.Tablet.PressurePoint.Y);
            if (doUp || doDown)
              conn.LastPressurePoint = new Point(conn.LastPressurePoint.Value.X, conn.Tablet.PressurePoint.Y);
            if (doLeft || doRight)
              conn.LastPressurePoint = new Point(conn.Tablet.PressurePoint.X, conn.LastPressurePoint.Value.Y);
          }
        }
        else
          conn.LastPressurePoint = null;

        if (doUp || doDown) conn.VerticalDelta = 0;
        if (doLeft || doRight) conn.HorizontalDelta = 0;

        if (doUp || doDown || doLeft || doRight)
          _accel = Math.Min(conn.Settings.MovementSpeed, _accel + 2);
        else
          _accel = Math.Max(1, _accel - 1);

        if (doDown)
          mouse_event(MOUSEEVENTF_MOVE, 0, _accel, 0, UIntPtr.Zero);
        if (doUp)
          mouse_event(MOUSEEVENTF_MOVE, 0, 0 - _accel, 0, UIntPtr.Zero);
        if (doLeft)
          mouse_event(MOUSEEVENTF_MOVE, 0 - _accel, 0, 0, UIntPtr.Zero);
        if (doRight)
          mouse_event(MOUSEEVENTF_MOVE, _accel, 0, 0, UIntPtr.Zero);
      }
    }

    #endregion
  }
}
