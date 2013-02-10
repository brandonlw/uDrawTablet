using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace uDrawTablet
{
  public partial class TabletOptions : Form
  {
    #region Declarations

    private string _iniFileName;
    private TabletSettings _settings;
    private string _currentDisplay;

    #endregion

    #region Constructors/Teardown

    public TabletOptions()
    {
      InitializeComponent();

      _Init();
    }

    public TabletOptions(string iniFileName, TabletSettings settings)
    {
      InitializeComponent();

      _iniFileName = iniFileName;
      _settings = settings;

      _Init();
    }

    #endregion

    #region Local Methods

    private void _Init()
    {
      this.Text = "Tablet Options - " + _iniFileName;

      foreach (TabletOptionButton.TabletButton button in Enum.GetValues(typeof(TabletOptionButton.TabletButton)))
      {
        var ctrl = new TabletOptionButton(button);
        flpMain.Controls.Add(ctrl);
      }
      this.Resize += Me_Resize;

      _Load();

      _ResizeControls();

      this.Invalidate();
    }

    private void _ResizeControls()
    {
      int width = flpMain.Width - 25;
      foreach (Control ctrl in flpMain.Controls)
        ctrl.Width = width;
      grpMovementType.Width = tbpButtons.Width - 20;
      grpMovementSettings.Width = tbpMovement.Width - 20;

      float ratio = ((float)SystemInformation.VirtualScreen.Height / (float)SystemInformation.VirtualScreen.Width);
      pnlDisplays.Height = (int)Math.Round(pnlDisplays.Width * ratio, 0);
      pnlDisplays.Location = new Point((int)Math.Round(((float)tbpDisplays.Width - pnlDisplays.Width) / 2.0, 0), pnlDisplays.Location.Y);
    }

    private void _Load()
    {
      //Load settings from settings object into UI controls
      trbPenClick.Value = _settings.PenPressureThreshold;
      if (_settings.MovementType == TabletSettings.TabletMovementType.Absolute)
      {
        rdoAbsolute.Checked = true;
        rdoRelative.Checked = false;
      }
      else
      {
        rdoAbsolute.Checked = false;
        rdoRelative.Checked = true;
      }
      trbSpeed.Value = _settings.MovementSpeed;
      trbPrecision.Value = _settings.Precision;
      chkAllowFingerMovement.Checked = _settings.AllowFingerMovement;
      foreach (TabletOptionButton option in flpMain.Controls)
      {
        switch (option.Button)
        {
          case TabletOptionButton.TabletButton.ACross:
            option.Action = _settings.AAction;
            break;
          case TabletOptionButton.TabletButton.BCircle:
            option.Action = _settings.BAction;
            break;
          case TabletOptionButton.TabletButton.XSquare:
            option.Action = _settings.XAction;
            break;
          case TabletOptionButton.TabletButton.YTriangle:
            option.Action = _settings.YAction;
            break;
          case TabletOptionButton.TabletButton.Up:
            option.Action = _settings.UpAction;
            break;
          case TabletOptionButton.TabletButton.Down:
            option.Action = _settings.DownAction;
            break;
          case TabletOptionButton.TabletButton.Left:
            option.Action = _settings.LeftAction;
            break;
          case TabletOptionButton.TabletButton.Right:
            option.Action = _settings.RightAction;
            break;
          case TabletOptionButton.TabletButton.Start:
            option.Action = _settings.StartAction;
            break;
          case TabletOptionButton.TabletButton.BackSelect:
            option.Action = _settings.BackAction;
            break;
          case TabletOptionButton.TabletButton.PSXboxGuide:
            option.Action = _settings.GuideAction;
            break;
          default:
            break;
        }
      }
      _currentDisplay = _settings.CurrentDisplay;
      chkAllowAllDisplays.Checked = _settings.AllowAllDisplays;
    }

    private void _Validate()
    {
      //Flush values from UI controls to settings object
      _settings.PenPressureThreshold = trbPenClick.Value;
      if (rdoAbsolute.Checked)
        _settings.MovementType = TabletSettings.TabletMovementType.Absolute;
      else if (rdoRelative.Checked)
        _settings.MovementType = TabletSettings.TabletMovementType.Relative;
      _settings.MovementSpeed = trbSpeed.Value;
      _settings.Precision = trbPrecision.Value;
      _settings.AllowFingerMovement = chkAllowFingerMovement.Checked;
      foreach (TabletOptionButton option in flpMain.Controls)
      {
        switch (option.Button)
        {
          case TabletOptionButton.TabletButton.ACross:
            _settings.AAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.BCircle:
            _settings.BAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.XSquare:
            _settings.XAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.YTriangle:
            _settings.YAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.Up:
            _settings.UpAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.Down:
            _settings.DownAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.Left:
            _settings.LeftAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.Right:
            _settings.RightAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.Start:
            _settings.StartAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.BackSelect:
            _settings.BackAction = option.Action;
            break;
          case TabletOptionButton.TabletButton.PSXboxGuide:
            _settings.GuideAction = option.Action;
            break;
          default:
            break;
        }
      }
      _settings.CurrentDisplay = _currentDisplay;
      _settings.AllowAllDisplays = chkAllowAllDisplays.Checked;
    }

    public static string GetDeviceName(string deviceName)
    {
      string ret = String.Empty;

      foreach (var c in deviceName)
      {
        ret += c;

        if (c == '\0')
          break;
      }

      return ret;
    }

    #endregion

    #region Event Handlers

    private void Me_Resize(object sender, EventArgs e)
    {
      _ResizeControls();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      //Save preferences and close
      _Validate();
      _settings.SaveSettings(_iniFileName);
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      //Just close
      this.Close();
    }

    private struct FloatRectangle
    {
      public float X;
      public float Y;
      public float Width;
      public float Height;

      public FloatRectangle(float x, float y, float width, float height)
      {
        X = x;
        Y = y;
        Width = width;
        Height = height;
      }
    };

    private void pnlDisplays_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      var pen = new Pen(Color.Black, 1);
      var selectedPen = new Pen(Color.Black, 5);
      var dimensions = new Rectangle(5, 5, pnlDisplays.Width - 15, pnlDisplays.Height - 15);

      g.DrawRectangle(pen, dimensions);

      //Set default display if we don't have one
      bool found = false;
      foreach (var screen in Screen.AllScreens)
      {
        if (GetDeviceName(screen.DeviceName) == _currentDisplay)
        {
          found = true;
          break;
        }
      }
      if (!found) _currentDisplay = GetDeviceName(Screen.PrimaryScreen.DeviceName);

      var v = SystemInformation.VirtualScreen;
      var colors = new Color[] { Color.LightBlue, Color.LightGreen, Color.Pink,
        Color.LightYellow, Color.PowderBlue, Color.LightSalmon };
      FloatRectangle? selected = null;
      for (int i = 0; i < Screen.AllScreens.Length; i++)
      {
        float x = (float)dimensions.X + ((((float)Screen.AllScreens[i].Bounds.X -
          (float)v.X) / (float)v.Width) * (float)dimensions.Width);
        float y = (float)dimensions.Y + ((((float)Screen.AllScreens[i].Bounds.Y -
          (float)v.Y) / (float)v.Height) * (float)dimensions.Height);
        float width = (float)Math.Round((float)((float)Screen.AllScreens[i].Bounds.Width /
          (float)v.Width) * (float)dimensions.Width, 0);
        float height = (float)Math.Round((float)((float)Screen.AllScreens[i].Bounds.Height /
          (float)v.Height) * (float)dimensions.Height, 0);

        g.FillRectangle(new SolidBrush(colors[i % colors.Length]), x, y, width, height);

        if (_currentDisplay == GetDeviceName(Screen.AllScreens[i].DeviceName))
          selected = new FloatRectangle(x, y, width, height);
        g.DrawRectangle(pen, x, y, width, height);
      }

      if (selected != null)
        g.DrawRectangle(selectedPen, selected.Value.X, selected.Value.Y,
          selected.Value.Width, selected.Value.Height);
    }

    private void pnlDisplays_MouseClick(object sender, MouseEventArgs e)
    {
      var dimensions = new Rectangle(5, 5, pnlDisplays.Width - 15, pnlDisplays.Height - 15);
      var v = SystemInformation.VirtualScreen;

      for (int i = 0; i < Screen.AllScreens.Length; i++)
      {
        float x = (float)dimensions.X + ((((float)Screen.AllScreens[i].Bounds.X -
          (float)v.X) / (float)v.Width) * (float)dimensions.Width);
        float y = (float)dimensions.Y + ((((float)Screen.AllScreens[i].Bounds.Y -
          (float)v.Y) / (float)v.Height) * (float)dimensions.Height);
        float width = (float)Math.Round((float)((float)Screen.AllScreens[i].Bounds.Width /
          (float)v.Width) * (float)dimensions.Width, 0);
        float height = (float)Math.Round((float)((float)Screen.AllScreens[i].Bounds.Height /
          (float)v.Height) * (float)dimensions.Height, 0);

        if ((e.X >= x) && (e.X < (x + width)) && (e.Y >= y) && (e.Y < (y + height)))
          _currentDisplay = GetDeviceName(Screen.AllScreens[i].DeviceName);
      }

      pnlDisplays.Invalidate();
      pnlDisplays.Update();
    }

    private void chkAllowAllDisplays_CheckedChanged(object sender, EventArgs e)
    {
      if (chkAllowAllDisplays.Checked)
      {
        pnlDisplays.Visible = false;
        lblInstructions.Visible = false;
      }
      else
      {
        pnlDisplays.Visible = true;
        lblInstructions.Visible = true;
      }
    }

    #endregion
  }
}
