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
      _ResizeControls();

      _Load();
    }

    private void _ResizeControls()
    {
      int width = flpMain.Width - 25;
      foreach (Control ctrl in flpMain.Controls)
        ctrl.Width = width;
      grpMovementType.Width = tbpButtons.Width - 20;
      grpMovementSettings.Width = tbpMovement.Width - 20;
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

    #endregion
  }
}
