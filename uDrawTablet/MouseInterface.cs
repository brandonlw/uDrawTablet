using Microsoft.Win32.SafeHandles;
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

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

    [StructLayout(LayoutKind.Sequential)]
    public struct SHELLEXECUTEINFO
    {
      public int cbSize;
      public uint fMask;
      public IntPtr hwnd;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string lpVerb;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string lpFile;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string lpParameters;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string lpDirectory;
      public int nShow;
      public IntPtr hInstApp;
      public IntPtr lpIDList;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string lpClass;
      public IntPtr hkeyClass;
      public uint dwHotKey;
      public IntPtr hIcon;
      public IntPtr hProcess;
    }

    public enum ShowCommands : int
    {
      SW_HIDE = 0,
      SW_SHOWNORMAL = 1,
      SW_NORMAL = 1,
      SW_SHOWMINIMIZED = 2,
      SW_SHOWMAXIMIZED = 3,
      SW_MAXIMIZE = 3,
      SW_SHOWNOACTIVATE = 4,
      SW_SHOW = 5,
      SW_MINIMIZE = 6,
      SW_SHOWMINNOACTIVE = 7,
      SW_SHOWNA = 8,
      SW_RESTORE = 9,
      SW_SHOWDEFAULT = 10,
      SW_FORCEMINIMIZE = 11,
      SW_MAX = 11
    }

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

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    static extern uint MapVirtualKey(uint uCode, uint uMapType);

    private const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
    private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    private const int MOUSEEVENTF_MOVE = 0x0001;
    private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const int MOUSEEVENTF_LEFTUP = 0x0004;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const int MOUSEEVENTF_RIGHTUP = 0x0010;
    private const int MOUSEEVENTF_WHEEL = 0x0800;
    private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const int MOUSEEVENTF_MIDDLEUP = 0x0040;

    #endregion

    #region Declarations

    private const int _MIN_PEN_PRESSURE_THRESHOLD = 0xD0;
    private const int _MAX_PEN_PRESSURE_THRESHOLD = 0xFF;
    private static Options _frmOptions;
    private static System.Threading.Timer _timer = null;
    private static Dictionary<TabletOptionButton.TabletButton, Dictionary<int, int>> _keyCounters;
    private static Dictionary<Keypress.ModifierKeyCode, int> _modifierCounters;
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

    public static string GetSettingsFileName(bool isWii, bool isPS3, int? index)
    {
      string ret = String.Empty;

      if (isWii)
      {
        ret = "Wii.ini";
      }
      else if (isPS3)
      {
        ret = "PS3.ini";
      }
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
        t.Settings = TabletSettings.LoadSettings(GetSettingsFileName(t.Tablet as WiiInputDevice != null,
          t.Tablet as PS3InputDevice != null, t.ReceiverIndex));
    }

    public static void Start(Options options)
    {
      Stop();

      _frmOptions = options;
      _keyCounters = new Dictionary<TabletOptionButton.TabletButton, Dictionary<int, int>>();
      foreach (TabletOptionButton.TabletButton button in Enum.GetValues(typeof(TabletOptionButton.TabletButton)))
        _keyCounters.Add(button, new Dictionary<int, int>());
      _modifierCounters = new Dictionary<Keypress.ModifierKeyCode, int>();
      foreach (Keypress.ModifierKeyCode code in Enum.GetValues(typeof(Keypress.ModifierKeyCode)))
        _modifierCounters.Add(code, 0);

      //Set up Xbox 360 USB wireless receiver
      if (Receiver == null || !Receiver.IsReceiverConnected)
      {
        Receiver = new Xbox360USB.WirelessReceiver();
        Receiver.DeviceConnected += Receiver_DeviceConnected;
        Receiver.DeviceDisconnected += Receiver_DeviceDisconnected;
        Receiver.Start();
      }

      //Set up the PS3 tablet dongle
      var conn = new TabletConnection((new PS3InputDevice()) as IInputDevice);
      conn.ButtonStateChanged += _ButtonStateChanged;
      conn.DPadStateChanged += _DPadStateChanged;
      conn.Settings = TabletSettings.LoadSettings(GetSettingsFileName(false, true, null));
      Tablets.Add(conn);

      //Set up the Wii tablet(s)
      var devices = WiiInputDevice.GetAllDevices();
      int i = 0;
      foreach (var dev in devices)
      {
        conn = new TabletConnection((new WiiInputDevice(dev, i)));
        conn.ButtonStateChanged += _ButtonStateChanged;
        conn.DPadStateChanged += _DPadStateChanged;
        conn.Settings = TabletSettings.LoadSettings(GetSettingsFileName(true, false, null));
        Tablets.Add(conn);
        i++;
      }

      //Set up the event timer
      _timer = new System.Threading.Timer(new TimerCallback(_HandleTabletEvents), null, 0, 1);
    }

    public static void Stop()
    {
      //Dispose of the PS3 tablet
      foreach (var t in Tablets)
      {
        var ps3 = t.Tablet as PS3InputDevice;

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

      PPJoyInterface.CloseAllHandles();
    }

    #endregion

    #region Event Handlers

    private static void _ButtonStateChanged(object sender, EventArgs e)
    {
      var conn = (TabletConnection)sender;

      if (conn.Tablet.ButtonState.CrossHeld != conn.LastButtonState.CrossHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.ACross, conn.Settings.AAction, conn.Tablet.ButtonState.CrossHeld);
      if (conn.Tablet.ButtonState.CircleHeld != conn.LastButtonState.CircleHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.BCircle, conn.Settings.BAction, conn.Tablet.ButtonState.CircleHeld);
      if (conn.Tablet.ButtonState.SquareHeld != conn.LastButtonState.SquareHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.XSquare1, conn.Settings.XAction, conn.Tablet.ButtonState.SquareHeld);
      if (conn.Tablet.ButtonState.TriangleHeld != conn.LastButtonState.TriangleHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.YTriangle2, conn.Settings.YAction, conn.Tablet.ButtonState.TriangleHeld);
      if (conn.Tablet.ButtonState.StartHeld != conn.LastButtonState.StartHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.StartPlus, conn.Settings.StartAction, conn.Tablet.ButtonState.StartHeld);
      if (conn.Tablet.ButtonState.SelectHeld != conn.LastButtonState.SelectHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.BackSelectMinus, conn.Settings.BackAction, conn.Tablet.ButtonState.SelectHeld);
      if (conn.Tablet.ButtonState.PSHeld != conn.LastButtonState.PSHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.PSXboxGuideHome, conn.Settings.GuideAction, conn.Tablet.ButtonState.PSHeld);
    }

    private static void _ChatpadKeyStateChanged(object sender, ChatpadKeyStateEventArgs e)
    {
      Xbox360InputDevice dev = null;
      foreach (var d in Tablets)
      {
        if (d.Receiver != null && d.ReceiverIndex == e.Index)
        {
          dev = d.Tablet as Xbox360InputDevice;
          break;
        }
      }

      if (dev != null)
      {
        var k = Keypress.GetChatpadKeyCode(new Keypress.ChatpadKeyCode(e.KeyCode, dev.OrangeHeld, dev.GreenHeld, dev.ShiftHeld));

        if (k.HasValue)
          _CheckKeys(TabletOptionButton.TabletButton.Hidden, (int)k.Value, e.Held);
      }
    }

    private static void _ShiftPressed(object sender, Xbox360DeviceEventArgs e)
    {
      Xbox360InputDevice dev = null;
      foreach (var d in Tablets)
      {
        if (d.Receiver != null && d.ReceiverIndex == e.Index)
        {
          dev = d.Tablet as Xbox360InputDevice;
          break;
        }
      }

      if (dev != null && dev.OrangeHeld)
      {
        _CheckKeys(TabletOptionButton.TabletButton.Hidden, (int)Keypress.GetFullKeyCode(Keypress.KeyCode.CapsLock), true);
        _CheckKeys(TabletOptionButton.TabletButton.Hidden, (int)Keypress.GetFullKeyCode(Keypress.KeyCode.CapsLock), false);
      }
    }

    private static void _PeoplePressed(object sender, Xbox360DeviceEventArgs e)
    {
      TabletConnection conn = null;
      foreach (var d in Tablets)
      {
        if (d.Receiver != null && d.ReceiverIndex == e.Index)
        {
          conn = d;
          break;
        }
      }

      if (conn != null)
      {
        _PerformAction(conn, TabletOptionButton.TabletButton.People, conn.Settings.PeopleAction, true);
        _PerformAction(conn, TabletOptionButton.TabletButton.People, conn.Settings.PeopleAction, false);
      }
    }

    private static void _DPadStateChanged(object sender, EventArgs e)
    {
      var conn = (TabletConnection)sender;

      if (conn.Tablet.DPadState.UpHeld != conn.LastDPadState.UpHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.Up, conn.Settings.UpAction, conn.Tablet.DPadState.UpHeld);
      if (conn.Tablet.DPadState.DownHeld != conn.LastDPadState.DownHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.Down, conn.Settings.DownAction, conn.Tablet.DPadState.DownHeld);
      if (conn.Tablet.DPadState.LeftHeld != conn.LastDPadState.LeftHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.Left, conn.Settings.LeftAction, conn.Tablet.DPadState.LeftHeld);
      if (conn.Tablet.DPadState.RightHeld != conn.LastDPadState.RightHeld)
        _PerformAction(conn, TabletOptionButton.TabletButton.Right, conn.Settings.RightAction, conn.Tablet.DPadState.RightHeld);
    }

    private static void _PerformAction(TabletConnection conn, TabletOptionButton.TabletButton button,
      TabletOptionButton.ButtonAction action, bool held)
    {
      switch ((TabletOptionButton.ButtonAction)((int)action & 0xFFFF))
      {
        case TabletOptionButton.ButtonAction.LeftClick:
          mouse_event(held ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.MiddleClick:
          mouse_event(held ? MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.RightClick:
          mouse_event(held ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.ScrollDown:
          if (held) mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -120, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.ScrollUp:
          if (held) mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 120, UIntPtr.Zero);
          break;
        case TabletOptionButton.ButtonAction.ShowOptions:
          if (held) _frmOptions.ShowOptions();
          break;
        case TabletOptionButton.ButtonAction.TurnOffTablet:
          if (conn.Receiver != null && held) conn.Receiver.TurnOffDevice(conn.ReceiverIndex);
          break;
        case TabletOptionButton.ButtonAction.SwitchTabletDisplay:
          if (held)
          {
            //Find our current display in the AllScreens list, and then switch to the next one
            int? index = null;
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
              if (conn.CurrentDisplay == Screen.AllScreens[i])
              {
                index = i;
                break;
              }
            }
            if (!index.HasValue)
              conn.CurrentDisplay = Screen.PrimaryScreen;
            else
              conn.CurrentDisplay = Screen.AllScreens[(index.Value + 1) % Screen.AllScreens.Length];
          }

          break;
        case TabletOptionButton.ButtonAction.ExecuteFile:
          if (held)
          {
            try
            {
              bool ignore = false;
              var p = new Process();
              
              p.StartInfo.ErrorDialog = true;
              switch (button)
              {
                case TabletOptionButton.TabletButton.ACross:
                  p.StartInfo.FileName = conn.Settings.AFile;
                  break;
                case TabletOptionButton.TabletButton.BCircle:
                  p.StartInfo.FileName = conn.Settings.BFile;
                  break;
                case TabletOptionButton.TabletButton.XSquare1:
                  p.StartInfo.FileName = conn.Settings.XFile;
                  break;
                case TabletOptionButton.TabletButton.YTriangle2:
                  p.StartInfo.FileName = conn.Settings.YFile;
                  break;
                case TabletOptionButton.TabletButton.Up:
                  p.StartInfo.FileName = conn.Settings.UpFile;
                  break;
                case TabletOptionButton.TabletButton.Down:
                  p.StartInfo.FileName = conn.Settings.DownFile;
                  break;
                case TabletOptionButton.TabletButton.Left:
                  p.StartInfo.FileName = conn.Settings.LeftFile;
                  break;
                case TabletOptionButton.TabletButton.Right:
                  p.StartInfo.FileName = conn.Settings.RightFile;
                  break;
                case TabletOptionButton.TabletButton.StartPlus:
                  p.StartInfo.FileName = conn.Settings.StartFile;
                  break;
                case TabletOptionButton.TabletButton.BackSelectMinus:
                  p.StartInfo.FileName = conn.Settings.BackFile;
                  break;
                case TabletOptionButton.TabletButton.PSXboxGuideHome:
                  p.StartInfo.FileName = conn.Settings.GuideFile;
                  break;
                case TabletOptionButton.TabletButton.PenClick:
                  p.StartInfo.FileName = conn.Settings.ClickFile;
                  break;
                default:
                  ignore = true;
                  break;
              }

              if (!ignore) p.Start();
            }
            catch
            {
              //Whatever...
            }
          }

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

        if (info != null)
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

      var connection = new TabletConnection((new Xbox360InputDevice(Receiver, index,
        Receiver.GetDeviceInformation(index))) as IInputDevice, Receiver, index);
      connection.ButtonStateChanged += _ButtonStateChanged;
      connection.DPadStateChanged += _DPadStateChanged;
      connection.ChatpadKeyStateChanged += _ChatpadKeyStateChanged;
      connection.ShiftPressed += _ShiftPressed;
      connection.PeoplePressed += _PeoplePressed;
      Tablets.Add(connection);
      connection.Settings = TabletSettings.LoadSettings(GetSettingsFileName(false, false, index));
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
          t.ChatpadKeyStateChanged -= _ChatpadKeyStateChanged;
          t.ShiftPressed -= _ShiftPressed;
          t.PeoplePressed -= _PeoplePressed;
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
        float tabletPadWidth = conn.Tablet is WiiInputDevice ? 1852 : 1920;
        float tabletPadHeight = conn.Tablet is WiiInputDevice ? 1340 : 1080;
        
        double threshold = ((conn.Settings.PenPressureThreshold / 10.0) *
          (_MAX_PEN_PRESSURE_THRESHOLD - _MIN_PEN_PRESSURE_THRESHOLD)) + _MIN_PEN_PRESSURE_THRESHOLD;
        bool penClicked = (conn.Tablet.PenPressure >= threshold);
        if (conn.LastPressure != (conn.Tablet.PenPressure >= threshold))
          _PerformAction(conn, TabletOptionButton.TabletButton.PenClick, conn.Settings.ClickAction, penClicked);
        conn.LastPressure = (conn.Tablet.PenPressure >= threshold);

        //Send PPJoy data, if necessary
        if (conn.Settings.PPJoyNumber > 0)
        {
          var buttons = new byte[16];
          buttons[0] = (byte)(conn.Tablet.ButtonState.CrossHeld ? 1 : 0);
          buttons[1] = (byte)(conn.Tablet.ButtonState.CircleHeld ? 1 : 0);
          buttons[2] = (byte)(conn.Tablet.ButtonState.TriangleHeld ? 1 : 0);
          buttons[3] = (byte)(conn.Tablet.ButtonState.SquareHeld ? 1 : 0);
          buttons[4] = (byte)(conn.Tablet.DPadState.UpHeld ? 1 : 0);
          buttons[5] = (byte)(conn.Tablet.DPadState.DownHeld ? 1 : 0);
          buttons[6] = (byte)(conn.Tablet.DPadState.LeftHeld ? 1 : 0);
          buttons[7] = (byte)(conn.Tablet.DPadState.RightHeld ? 1 : 0);
          buttons[8] = (byte)(conn.Tablet.ButtonState.PSHeld ? 1 : 0);
          buttons[9] = (byte)(conn.Tablet.ButtonState.SelectHeld ? 1 : 0);
          buttons[10] = (byte)(conn.Tablet.ButtonState.StartHeld ? 1 : 0);
          buttons[11] = (byte)(penClicked ? 1 : 0);
          buttons[12] = (byte)(conn.Tablet.ButtonState.LeftStickHeld ? 1 : 0);
          buttons[13] = (byte)(conn.Tablet.ButtonState.RightStickHeld ? 1 : 0);
          buttons[14] = (byte)(conn.Tablet.ButtonState.LeftButtonHeld ? 1 : 0);
          buttons[15] = (byte)(conn.Tablet.ButtonState.RightButtonHeld ? 1 : 0);
          var axes = new int[11];
          axes[0] = conn.Tablet.AccelerometerData.XAxis * 32767;
          axes[1] = conn.Tablet.AccelerometerData.YAxis * 32767;
          axes[2] = conn.Tablet.AccelerometerData.ZAxis * 32767;
          if (conn.Tablet.PressureType == TabletPressureType.FingerPressed ||
            conn.Tablet.PressureType == TabletPressureType.PenPressed)
          {
            axes[3] = Convert.ToInt32(32767 * (conn.Tablet.PressurePoint.X / tabletPadWidth));
            axes[4] = Convert.ToInt32(32767 * (conn.Tablet.PressurePoint.Y / tabletPadHeight));
          }
          else
          {
            //Centered
            axes[3] = Convert.ToInt32(32767 * (tabletPadWidth / 2.0));
            axes[4] = Convert.ToInt32(32767 * (tabletPadHeight / 2.0));
          }
          var controller = conn.Tablet as Xbox360InputDevice;
          if (controller != null)
          {
            axes[5] = 32767 * (controller.LeftTrigger / 255);
            axes[6] = 32767 * (controller.RightTrigger / 255);
            axes[7] = 32767 * ((controller.X1 + 32768) / 65535);
            axes[8] = 32767 * ((controller.Y1 + 32768) / 65535);
            axes[9] = 32767 * ((controller.X2 + 32768) / 65535);
            axes[10] = 32767 * ((controller.Y2 + 32768) / 65535);
          }
          PPJoyInterface.Update(conn.Settings.PPJoyNumber, axes, buttons);
        }

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
            float actualWidth = conn.Settings.AllowAllDisplays ? SystemInformation.VirtualScreen.Width : conn.CurrentDisplay.Bounds.Width;
            float actualHeight = conn.Settings.AllowAllDisplays ? SystemInformation.VirtualScreen.Height : conn.CurrentDisplay.Bounds.Height;
            float width = 65536;
            float height = 65536;
            float xStart = 0;
            float yStart = 0;
            float x = 0;
            float y = 0;

            if (conn.Settings.RestrictToCurrentWindow)
            {
              //We're only looking at the current window, which could be on any display
              var v = SystemInformation.VirtualScreen;
              RECT rect;
              var hWnd = GetForegroundWindow(); GetWindowRect(hWnd, out rect);
              int windowWidth = rect.Right - rect.Left, windowHeight = rect.Bottom - rect.Top, windowX = rect.Left, windowY = rect.Top;
              actualWidth = windowWidth;
              actualHeight = windowHeight;

              width = (float)Math.Round((float)((float)windowWidth / (float)v.Width) * 65536.0, 0);
              height = (float)Math.Round((float)((float)windowHeight / (float)v.Height) * 65536.0, 0);
              xStart = ((((float)windowX - (float)v.X) / (float)v.Width) * (float)65536.0);
              yStart = ((((float)windowY - (float)v.Y) / (float)v.Height) * (float)65536.0);
            }
            else if (!conn.Settings.AllowAllDisplays)
            {
              //Find the absolute coordinates for start of current display (which will be X and Y starting offsets)
              var v = SystemInformation.VirtualScreen;
              width = (float)Math.Round((float)((float)conn.CurrentDisplay.Bounds.Width /
                (float)v.Width) * 65536.0, 0);
              height = (float)Math.Round((float)((float)conn.CurrentDisplay.Bounds.Height /
                (float)v.Height) * 65536.0, 0);
              xStart = ((((float)conn.CurrentDisplay.Bounds.X -
                (float)v.X) / (float)v.Width) * (float)65536.0);
              yStart = ((((float)conn.CurrentDisplay.Bounds.Y -
                (float)v.Y) / (float)v.Height) * (float)65536.0);
            }

            if (conn.Settings.MaintainAspectRatio)
            {
              //Get the current style based on which is higher, width or height
              var style = ((tabletPadHeight / tabletPadWidth) >= (actualHeight / actualWidth)) ?
                DockOption.DockStyle.Vertical : DockOption.DockStyle.Horizontal;

              //Translate the width and height to tablet proportions
              float tabletWidth = (style == DockOption.DockStyle.Vertical ? tabletPadWidth :
                tabletPadWidth * ((tabletPadHeight / tabletPadWidth) / (actualHeight / actualWidth)));
              float tabletHeight = (style == DockOption.DockStyle.Horizontal ? tabletPadHeight :
                tabletPadHeight * ((actualHeight / actualWidth) / (tabletPadHeight / tabletPadWidth)));
              float tabletX = 0;
              float tabletY = 0;
              if (style == DockOption.DockStyle.Horizontal)
              {
                switch (conn.Settings.HorizontalDock)
                {
                  case DockOption.DockOptionValue.Center:
                    tabletX = (tabletPadWidth - tabletWidth) / 2;
                    break;
                  case DockOption.DockOptionValue.Right:
                    tabletX = tabletPadWidth - tabletWidth;
                    break;
                  default:
                    break;
                }
              }
              else
              {
                switch (conn.Settings.VerticalDock)
                {
                  case DockOption.DockOptionValue.Center:
                    tabletY = (tabletPadHeight - tabletHeight) / 2;
                    break;
                  case DockOption.DockOptionValue.Right:
                    tabletY = tabletPadHeight - tabletHeight;
                    break;
                  default:
                    break;
                }
              }

              //Determine whether our current pressure point is within the box
              x = conn.Tablet.PressurePoint.X;
              y = conn.Tablet.PressurePoint.Y;
              if (x < tabletX) x = tabletX;
              if (x > (tabletX + tabletWidth)) x = tabletX + tabletWidth;
              if (y < tabletY) y = tabletY;
              if (y > (tabletY + tabletHeight)) y = tabletY + tabletHeight;

              //It is, so set the coordinates appropriately
              x = (((x - tabletX) / tabletWidth) * width) + xStart;
              y = (((y - tabletY) / tabletHeight) * height) + yStart;
            }
            else
            {
              x = ((conn.Tablet.PressurePoint.X / tabletPadWidth) * width) + xStart;
              y = (conn.Tablet.PressurePoint.Y / tabletPadHeight * height) + yStart;
            }

            if (Cursor.Position.X != x && Cursor.Position.Y != y) //not sure if this respects virtual desktop coordinates
              mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_VIRTUALDESK,
                (int)x, (int)y, 0, UIntPtr.Zero);
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

        _CheckKeys(conn.Settings.AAction, TabletOptionButton.TabletButton.ACross, conn.Tablet.ButtonState.CrossHeld);
        _CheckKeys(conn.Settings.BAction, TabletOptionButton.TabletButton.BCircle, conn.Tablet.ButtonState.CircleHeld);
        _CheckKeys(conn.Settings.XAction, TabletOptionButton.TabletButton.XSquare1, conn.Tablet.ButtonState.SquareHeld);
        _CheckKeys(conn.Settings.YAction, TabletOptionButton.TabletButton.YTriangle2, conn.Tablet.ButtonState.TriangleHeld);
        _CheckKeys(conn.Settings.UpAction, TabletOptionButton.TabletButton.Up, conn.Tablet.DPadState.UpHeld);
        _CheckKeys(conn.Settings.DownAction, TabletOptionButton.TabletButton.Down, conn.Tablet.DPadState.DownHeld);
        _CheckKeys(conn.Settings.LeftAction, TabletOptionButton.TabletButton.Left, conn.Tablet.DPadState.LeftHeld);
        _CheckKeys(conn.Settings.RightAction, TabletOptionButton.TabletButton.Right, conn.Tablet.DPadState.RightHeld);
        _CheckKeys(conn.Settings.BackAction, TabletOptionButton.TabletButton.BackSelectMinus, conn.Tablet.ButtonState.SelectHeld);
        _CheckKeys(conn.Settings.StartAction, TabletOptionButton.TabletButton.StartPlus, conn.Tablet.ButtonState.StartHeld);
        _CheckKeys(conn.Settings.GuideAction, TabletOptionButton.TabletButton.PSXboxGuideHome, conn.Tablet.ButtonState.PSHeld);
      }
    }

    private static void _CheckKeys(TabletOptionButton.ButtonAction action, TabletOptionButton.TabletButton button, bool held)
    {
      if (_IsKeypressAction(action))
      {
        _CheckKeys(button, ((int)action >> 16), held);
      }
    }

    private static void _CheckKeys(TabletOptionButton.TabletButton button, int a, bool held)
    {
      byte p = (byte)(a & 0xFF);
      bool ctrl = (a & (int)Keypress.CTRL_MASK) > 0;
      bool shift = (a & (int)Keypress.SHIFT_MASK) > 0;
      bool alt = (a & (int)Keypress.ALT_MASK) > 0;
      bool win = (a & (int)Keypress.WIN_MASK) > 0;
      bool sendOnce = (a & (int)Keypress.SEND_ONCE_MASK) > 0;

      _CheckKeys(button, p, ctrl, shift, alt, win, held, sendOnce);
    }

    private static void _CheckKeys(TabletOptionButton.TabletButton button, byte p,
      bool ctrl, bool shift, bool alt, bool win, bool held, bool sendOnce)
    {
      try
      {
        if (held)
        {
          if (!_keyCounters[button].ContainsKey(p))
            _keyCounters[button].Add(p, 0);

          if (_keyCounters[button][p] == 0)
          {
            if (ctrl && _modifierCounters[Keypress.ModifierKeyCode.Control] == 0)
            {
              _modifierCounters[Keypress.ModifierKeyCode.Control]++;
              keybd_event((byte)Keypress.ModifierKeyCode.Control, 0, 0, UIntPtr.Zero);
            }
            if (shift && _modifierCounters[Keypress.ModifierKeyCode.Shift] == 0)
            {
              _modifierCounters[Keypress.ModifierKeyCode.Shift]++;
              keybd_event((byte)Keypress.ModifierKeyCode.Shift, 0, 0, UIntPtr.Zero);
            }
            if (alt && _modifierCounters[Keypress.ModifierKeyCode.Alt] == 0)
            {
              _modifierCounters[Keypress.ModifierKeyCode.Alt]++;
              keybd_event((byte)Keypress.ModifierKeyCode.Alt, 0, 0, UIntPtr.Zero);
            }
            if (win && _modifierCounters[Keypress.ModifierKeyCode.Windows] == 0)
            {
              _modifierCounters[Keypress.ModifierKeyCode.Windows]++;
              keybd_event((byte)Keypress.ModifierKeyCode.Windows, 0, 0, UIntPtr.Zero);
            }
          }

          //Send the keydown event
          if (sendOnce && _keyCounters[button][p] == 1)
          {
            //Already sent it, do nothing
          }
          else
          {
            _keyCounters[button][p]++;
            keybd_event(p, 0, 0, UIntPtr.Zero);
          }
        }
        else
        {
          //Send the key up event for the key and each modifier
          if (_keyCounters[button].ContainsKey(p) &&
            _keyCounters[button][p] > 0)
          {
            _keyCounters[button][p]--;
            keybd_event(p, 0, 2, UIntPtr.Zero);
          }

          if (ctrl && _modifierCounters[Keypress.ModifierKeyCode.Control] > 0)
          {
            _modifierCounters[Keypress.ModifierKeyCode.Control]--;
            if (_modifierCounters[Keypress.ModifierKeyCode.Control] == 0)
              keybd_event((byte)Keypress.ModifierKeyCode.Control, 0, 2, UIntPtr.Zero);
          }
          if (shift && _modifierCounters[Keypress.ModifierKeyCode.Shift] > 0)
          {
            _modifierCounters[Keypress.ModifierKeyCode.Shift]--;
            if (_modifierCounters[Keypress.ModifierKeyCode.Shift] == 0)
            {
              keybd_event((byte)Keypress.ModifierKeyCode.Shift, 0, 2, UIntPtr.Zero);
            }
          }
          if (alt && _modifierCounters[Keypress.ModifierKeyCode.Alt] > 0)
          {
            _modifierCounters[Keypress.ModifierKeyCode.Alt]--;
            if (_modifierCounters[Keypress.ModifierKeyCode.Alt] == 0)
              keybd_event((byte)Keypress.ModifierKeyCode.Alt, 0, 2, UIntPtr.Zero);
          }
          if (win && _modifierCounters[Keypress.ModifierKeyCode.Windows] > 0)
          {
            _modifierCounters[Keypress.ModifierKeyCode.Windows]--;
            if (_modifierCounters[Keypress.ModifierKeyCode.Windows] == 0)
              keybd_event((byte)Keypress.ModifierKeyCode.Windows, 0, 2, UIntPtr.Zero);
          }
        }
      }
      catch
      {
      }
    }

    private static bool _IsKeypressAction(TabletOptionButton.ButtonAction action)
    {
      return (((int)action & 0xFFFF) == (int)TabletOptionButton.ButtonAction.KeyboardKeypress);
    }

    #endregion
  }
}
