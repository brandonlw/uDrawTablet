using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace uDrawTablet
{
  public class TabletSettings
  {
    #region Declarations

    private const string _DEFAULT_SECTION = "Default";
    private const string _KEY_PEN_THRESHOLD = "PenThreshold";
    private const int _DEFAULT_PEN_THRESHOLD = 5;
    private const string _KEY_MOVEMENT_TYPE = "MovementType";
    private const string _KEY_MOVEMENT_SPEED = "MovementSpeed";
    private const int _DEFAULT_MOVEMENT_SPEED = 10;
    private const string _KEY_PRECISION = "Precision";
    private const int _DEFAULT_PRECISION = 3;
    private const string _KEY_A_ACTION = "AAction";
    private const string _KEY_A_FILE = "AFile";
    private const string _DEFAULT_A_ACTION = "LeftClick";
    private const string _KEY_B_ACTION = "BAction";
    private const string _KEY_B_FILE = "BFile";
    private const string _DEFAULT_B_ACTION = "None";
    private const string _KEY_X_ACTION = "XAction";
    private const string _KEY_X_FILE = "XFile";
    private const string _DEFAULT_X_ACTION = "RightClick";
    private const string _KEY_Y_ACTION = "YAction";
    private const string _KEY_Y_FILE = "YFile";
    private const string _DEFAULT_Y_ACTION = "ShowOptions";
    private const string _KEY_UP_ACTION = "UpAction";
    private const string _KEY_UP_FILE = "UpFile";
    private const string _DEFAULT_UP_ACTION = "MoveUp";
    private const string _KEY_DOWN_ACTION = "DownAction";
    private const string _KEY_DOWN_FILE = "DownFile";
    private const string _DEFAULT_DOWN_ACTION = "MoveDown";
    private const string _KEY_LEFT_ACTION = "LeftAction";
    private const string _KEY_LEFT_FILE = "LeftFile";
    private const string _DEFAULT_LEFT_ACTION = "MoveLeft";
    private const string _KEY_RIGHT_ACTION = "RightAction";
    private const string _KEY_RIGHT_FILE = "RightFile";
    private const string _DEFAULT_RIGHT_ACTION = "MoveRight";
    private const string _KEY_START_ACTION = "StartAction";
    private const string _KEY_START_FILE = "StartFile";
    private const string _DEFAULT_START_ACTION = "None";
    private const string _KEY_BACK_ACTION = "BackAction";
    private const string _KEY_BACK_FILE = "BackFile";
    private const string _DEFAULT_BACK_ACTION = "None";
    private const string _KEY_GUIDE_ACTION = "GuideAction";
    private const string _KEY_GUIDE_FILE = "GuideFile";
    private const string _DEFAULT_GUIDE_ACTION = "TurnOffTablet";
    private const string _KEY_CLICK_ACTION = "ClickAction";
    private const string _KEY_CLICK_FILE = "ClickFile";
    private const string _DEFAULT_CLICK_ACTION = "LeftClick";
    private const string _KEY_PEOPLE_ACTION = "PeopleAction";
    private const string _KEY_PEOPLE_FILE = "PeopleFile";
    private const string _DEFAULT_PEOPLE_ACTION = "None";
    private const string _KEY_ALLOW_FINGER_MOVEMENT = "AllowFingerMovement";
    private const bool _DEFAULT_ALLOW_FINGER_MOVEMENT = false;
    private const string _KEY_CURRENT_DISPLAY = "CurrentDisplay";
    private const string _KEY_ALLOW_ALL_DISPLAYS = "AllowAllDisplays";
    private const bool _DEFAULT_ALLOW_ALL_DISPLAYS = false;
    private const string _KEY_MAINTAIN_ASPECT_RATIO = "MaintainAspectRatio";
    private const bool _DEFAULT_MAINTAIN_ASPECT_RATIO = false;
    private const string _KEY_RESTRICT_TO_CURRENT_WINDOW = "RestrictToCurrentWindow";
    private const bool _DEFAULT_RESTRICT_TO_CURRENT_WINDOW = false;
    private const string _KEY_HORIZONTAL_DOCK = "HorizontalDock";
    private const DockOption.DockOptionValue _DEFAULT_HORIZONTAL_DOCK = DockOption.DockOptionValue.Left;
    private const string _KEY_VERTICAL_DOCK = "VerticalDock";
    private const DockOption.DockOptionValue _DEFAULT_VERTICAL_DOCK = DockOption.DockOptionValue.Top;
    private const string _KEY_PPJOY_NUMBER = "PPJoyNumber";
    private const int _DEFAULT_PPJOY_NUMBER = 0;

    public enum TabletMovementType
    {
      Absolute,
      Relative
    };

    public int PenPressureThreshold { get; set; }
    public TabletMovementType MovementType { get; set; }
    public int MovementSpeed { get; set; }
    public int Precision { get; set; }
    public bool AllowFingerMovement { get; set; }
    public TabletOptionButton.ButtonAction AAction { get; set; }
    public string AFile { get; set; }
    public TabletOptionButton.ButtonAction BAction { get; set; }
    public string BFile { get; set; }
    public TabletOptionButton.ButtonAction XAction { get; set; }
    public string XFile { get; set; }
    public TabletOptionButton.ButtonAction YAction { get; set; }
    public string YFile { get; set; }
    public TabletOptionButton.ButtonAction UpAction { get; set; }
    public string UpFile { get; set; }
    public TabletOptionButton.ButtonAction DownAction { get; set; }
    public string DownFile { get; set; }
    public TabletOptionButton.ButtonAction LeftAction { get; set; }
    public string LeftFile { get; set; }
    public TabletOptionButton.ButtonAction RightAction { get; set; }
    public string RightFile { get; set; }
    public TabletOptionButton.ButtonAction StartAction { get; set; }
    public string StartFile { get; set; }
    public TabletOptionButton.ButtonAction BackAction { get; set; }
    public string BackFile { get; set; }
    public TabletOptionButton.ButtonAction GuideAction { get; set; }
    public string GuideFile { get; set; }
    public TabletOptionButton.ButtonAction ClickAction { get; set; }
    public string ClickFile { get; set; }
    public TabletOptionButton.ButtonAction PeopleAction { get; set; }
    public string PeopleFile { get; set; }
    public string CurrentDisplay { get; set; }
    public bool AllowAllDisplays { get; set; }
    public bool MaintainAspectRatio { get; set; }
    public bool RestrictToCurrentWindow { get; set; }
    public DockOption.DockOptionValue HorizontalDock { get; set; }
    public DockOption.DockOptionValue VerticalDock { get; set; }
    public int PPJoyNumber { get; set; }

    #endregion

    #region P/Invoke Crud

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string lPAppName, string lpKeyName, string lpString, string lpFileName);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFilePath);

    #endregion

    #region Constructors/Teardown

    public TabletSettings()
    {
      PenPressureThreshold = _DEFAULT_PEN_THRESHOLD;
      MovementType = TabletMovementType.Absolute;
      MovementSpeed = _DEFAULT_MOVEMENT_SPEED;
      Precision = _DEFAULT_PRECISION;
      AllowFingerMovement = _DEFAULT_ALLOW_FINGER_MOVEMENT;
      AAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_A_ACTION);
      BAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_B_ACTION);
      XAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_X_ACTION);
      YAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_Y_ACTION);
      UpAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_UP_ACTION);
      DownAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_DOWN_ACTION);
      LeftAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_LEFT_ACTION);
      RightAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_RIGHT_ACTION);
      StartAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_START_ACTION);
      BackAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_BACK_ACTION);
      GuideAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_GUIDE_ACTION);
      ClickAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_CLICK_ACTION);
      PeopleAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_PEOPLE_ACTION);
      CurrentDisplay = TabletOptions.GetDeviceName(Screen.PrimaryScreen.DeviceName);
      AllowAllDisplays = _DEFAULT_ALLOW_ALL_DISPLAYS;
      MaintainAspectRatio = _DEFAULT_MAINTAIN_ASPECT_RATIO;
      RestrictToCurrentWindow = _DEFAULT_RESTRICT_TO_CURRENT_WINDOW;
      HorizontalDock = _DEFAULT_HORIZONTAL_DOCK;
      VerticalDock = _DEFAULT_VERTICAL_DOCK;
      PPJoyNumber = _DEFAULT_PPJOY_NUMBER;
    }

    #endregion

    #region Public Methods

    public static TabletSettings LoadSettings(string iniFileName)
    {
      var ret = new TabletSettings();

      //Pen pressure threshold
      var sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_PEN_THRESHOLD, ret.PenPressureThreshold.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int threshold = ret.PenPressureThreshold; int.TryParse(sb.ToString(), out threshold);
      ret.PenPressureThreshold = threshold;

      //Allow finger movement
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_ALLOW_FINGER_MOVEMENT, ret.AllowFingerMovement.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      bool allowFingerMovement = ret.AllowFingerMovement; bool.TryParse(sb.ToString(), out allowFingerMovement);
      ret.AllowFingerMovement = allowFingerMovement;

      //Movement type
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_TYPE, ret.MovementType.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      var type = ret.MovementType; type = (TabletMovementType)Enum.Parse(typeof(TabletMovementType), sb.ToString());
      ret.MovementType = type;

      //Movement speed
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_SPEED, ret.MovementSpeed.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int speed = ret.MovementSpeed; int.TryParse(sb.ToString(), out speed);
      ret.MovementSpeed = speed;

      //Movement precision
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_PRECISION, ret.Precision.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int precision = ret.Precision; int.TryParse(sb.ToString(), out precision);
      ret.Precision = precision;

      //Actions
      ret.AAction = _GetAction(iniFileName, _KEY_A_ACTION, _DEFAULT_A_ACTION);
      ret.AFile = _GetString(iniFileName, _KEY_A_FILE);
      ret.BAction = _GetAction(iniFileName, _KEY_B_ACTION, _DEFAULT_B_ACTION);
      ret.BFile = _GetString(iniFileName, _KEY_B_FILE);
      ret.XAction = _GetAction(iniFileName, _KEY_X_ACTION, _DEFAULT_X_ACTION);
      ret.XFile = _GetString(iniFileName, _KEY_X_FILE);
      ret.YAction = _GetAction(iniFileName, _KEY_Y_ACTION, _DEFAULT_Y_ACTION);
      ret.YFile = _GetString(iniFileName, _KEY_Y_FILE);
      ret.UpAction = _GetAction(iniFileName, _KEY_UP_ACTION, _DEFAULT_UP_ACTION);
      ret.UpFile = _GetString(iniFileName, _KEY_UP_FILE);
      ret.DownAction = _GetAction(iniFileName, _KEY_DOWN_ACTION, _DEFAULT_DOWN_ACTION);
      ret.DownFile = _GetString(iniFileName, _KEY_DOWN_FILE);
      ret.LeftAction = _GetAction(iniFileName, _KEY_LEFT_ACTION, _DEFAULT_LEFT_ACTION);
      ret.LeftFile = _GetString(iniFileName, _KEY_LEFT_FILE);
      ret.RightAction = _GetAction(iniFileName, _KEY_RIGHT_ACTION, _DEFAULT_RIGHT_ACTION);
      ret.RightFile = _GetString(iniFileName, _KEY_RIGHT_FILE);
      ret.StartAction = _GetAction(iniFileName, _KEY_START_ACTION, _DEFAULT_START_ACTION);
      ret.StartFile = _GetString(iniFileName, _KEY_START_FILE);
      ret.BackAction = _GetAction(iniFileName, _KEY_BACK_ACTION, _DEFAULT_BACK_ACTION);
      ret.BackFile = _GetString(iniFileName, _KEY_BACK_FILE);
      ret.GuideAction = _GetAction(iniFileName, _KEY_GUIDE_ACTION, _DEFAULT_GUIDE_ACTION);
      ret.GuideFile = _GetString(iniFileName, _KEY_GUIDE_FILE);
      ret.ClickAction = _GetAction(iniFileName, _KEY_CLICK_ACTION, _DEFAULT_CLICK_ACTION);
      ret.ClickFile = _GetString(iniFileName, _KEY_CLICK_FILE);
      ret.PeopleAction = _GetAction(iniFileName, _KEY_PEOPLE_ACTION, _DEFAULT_PEOPLE_ACTION);
      ret.PeopleFile = _GetString(iniFileName, _KEY_PEOPLE_FILE);

      //Display name
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_CURRENT_DISPLAY, ret.CurrentDisplay.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      ret.CurrentDisplay = sb.ToString();

      //Allow all displays
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_ALLOW_ALL_DISPLAYS, ret.AllowAllDisplays.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      bool allowAllDisplays = ret.AllowAllDisplays; bool.TryParse(sb.ToString(), out allowAllDisplays);
      ret.AllowAllDisplays = allowAllDisplays;

      //Maintain aspect ratio
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_MAINTAIN_ASPECT_RATIO, ret.MaintainAspectRatio.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      bool maintainAspectRatio = ret.MaintainAspectRatio; bool.TryParse(sb.ToString(), out maintainAspectRatio);
      ret.MaintainAspectRatio = maintainAspectRatio;

      //Restrict to current window
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_RESTRICT_TO_CURRENT_WINDOW, ret.RestrictToCurrentWindow.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      bool restrictToCurrentWindow = ret.RestrictToCurrentWindow; bool.TryParse(sb.ToString(), out restrictToCurrentWindow);
      ret.RestrictToCurrentWindow = restrictToCurrentWindow;

      //Horizontal dock
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_HORIZONTAL_DOCK, ret.HorizontalDock.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      var hdock = ret.HorizontalDock; hdock = (DockOption.DockOptionValue)Enum.Parse(typeof(DockOption.DockOptionValue), sb.ToString());
      ret.HorizontalDock = hdock;

      //Vertical dock
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_VERTICAL_DOCK, ret.VerticalDock.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      var vdock = ret.VerticalDock; vdock = (DockOption.DockOptionValue)Enum.Parse(typeof(DockOption.DockOptionValue), sb.ToString());
      ret.VerticalDock = vdock;

      //PPJoy virtual joystick number
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_PPJOY_NUMBER, ret.PPJoyNumber.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int joyNumber = ret.PPJoyNumber; int.TryParse(sb.ToString(), out joyNumber);
      ret.PPJoyNumber = joyNumber;

      return ret;
    }

    public void SaveSettings(string iniFileName)
    {
      //Pen pressure threshold
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_PEN_THRESHOLD, this.PenPressureThreshold.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Allow finger movement
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_ALLOW_FINGER_MOVEMENT, this.AllowFingerMovement.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Movement type
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_TYPE, this.MovementType.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Movement speed
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_SPEED, this.MovementSpeed.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Movement precision
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_PRECISION, this.Precision.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Actions
      _SetAction(iniFileName, _KEY_A_ACTION, AAction);
      _SetString(iniFileName, _KEY_A_FILE, AFile);
      _SetAction(iniFileName, _KEY_B_ACTION, BAction);
      _SetString(iniFileName, _KEY_B_FILE, BFile);
      _SetAction(iniFileName, _KEY_X_ACTION, XAction);
      _SetString(iniFileName, _KEY_X_FILE, XFile);
      _SetAction(iniFileName, _KEY_Y_ACTION, YAction);
      _SetString(iniFileName, _KEY_Y_FILE, YFile);
      _SetAction(iniFileName, _KEY_UP_ACTION, UpAction);
      _SetString(iniFileName, _KEY_UP_FILE, UpFile);
      _SetAction(iniFileName, _KEY_DOWN_ACTION, DownAction);
      _SetString(iniFileName, _KEY_DOWN_FILE, DownFile);
      _SetAction(iniFileName, _KEY_LEFT_ACTION, LeftAction);
      _SetString(iniFileName, _KEY_LEFT_FILE, LeftFile);
      _SetAction(iniFileName, _KEY_RIGHT_ACTION, RightAction);
      _SetString(iniFileName, _KEY_RIGHT_FILE, RightFile);
      _SetAction(iniFileName, _KEY_START_ACTION, StartAction);
      _SetString(iniFileName, _KEY_START_FILE, StartFile);
      _SetAction(iniFileName, _KEY_BACK_ACTION, BackAction);
      _SetString(iniFileName, _KEY_BACK_FILE, BackFile);
      _SetAction(iniFileName, _KEY_GUIDE_ACTION, GuideAction);
      _SetString(iniFileName, _KEY_GUIDE_FILE, GuideFile);
      _SetAction(iniFileName, _KEY_CLICK_ACTION, ClickAction);
      _SetString(iniFileName, _KEY_CLICK_FILE, ClickFile);
      _SetAction(iniFileName, _KEY_PEOPLE_ACTION, PeopleAction);
      _SetString(iniFileName, _KEY_PEOPLE_FILE, PeopleFile);

      //Display name
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_CURRENT_DISPLAY, this.CurrentDisplay.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Allow all displays
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_ALLOW_ALL_DISPLAYS, this.AllowAllDisplays.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Maintain aspect ratio
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_MAINTAIN_ASPECT_RATIO, this.MaintainAspectRatio.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Restrict to current window
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_RESTRICT_TO_CURRENT_WINDOW, this.RestrictToCurrentWindow.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Horizontal dock
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_HORIZONTAL_DOCK, this.HorizontalDock.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Vertical dock
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_VERTICAL_DOCK, this.VerticalDock.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //PPJoy virtual joystick number
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_PPJOY_NUMBER, this.PPJoyNumber.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
    }

    #endregion

    #region Local Methods

    private static TabletOptionButton.ButtonAction _GetAction(string iniFileName, string keyName, string defaultValue)
    {
      var sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, keyName, defaultValue, sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      return (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), sb.ToString());
    }

    private static string _GetString(string iniFileName, string keyName)
    {
      return _GetString(iniFileName, keyName, String.Empty);
    }

    private static string _GetString(string iniFileName, string keyName, string defaultValue)
    {
      var sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, keyName, defaultValue, sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      return sb.ToString();
    }

    private static void _SetAction(string iniFileName, string keyName, TabletOptionButton.ButtonAction action)
    {
      WritePrivateProfileString(_DEFAULT_SECTION, keyName, action.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
    }

    private static void _SetString(string iniFileName, string keyName, string value)
    {
      WritePrivateProfileString(_DEFAULT_SECTION, keyName, value,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
    }

    #endregion
  }
}
