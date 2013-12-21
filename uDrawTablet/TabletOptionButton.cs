using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace uDrawTablet
{
  public partial class TabletOptionButton : UserControl
  {
    public TabletButton Button;

    public ButtonAction Action
    {
      get
      {
        ButtonAction ret = ButtonAction.None;

        foreach (var action in _GetActionNames())
          if (action.Value == cboValue.SelectedItem.ToString())
            ret = action.Key;

        return (ButtonAction)((int)ret | ((int)this.FullKeyCode << 16));
      }
      set
      {
        foreach (var action in _GetActionNames())
          if (action.Key == (ButtonAction)(((int)value & 0xFFFF)))
            cboValue.SelectedItem = action.Value;

        this.FullKeyCode = (Keypress.KeyCode)(((int)value >> 16));
      }
    }

    public Keypress.KeyCode FullKeyCode
    {
      get
      {
        return Keypress.GetFullKeyCode(((Keypress.KeyCode)cboKey.SelectedItem),
          chkCtrl.Checked, chkShift.Checked, chkAlt.Checked, chkWin.Checked, chkSendKeysOnce.Checked);
      }
      set
      {
        cboKey.SelectedItem = (value & (Keypress.KeyCode)0xFF);
        chkCtrl.Checked = ((value & Keypress.CTRL_MASK) > 0);
        chkShift.Checked = ((value & Keypress.SHIFT_MASK) > 0);
        chkAlt.Checked = ((value & Keypress.ALT_MASK) > 0);
        chkWin.Checked = ((value & Keypress.WIN_MASK) > 0);
        chkSendKeysOnce.Checked = ((value & Keypress.SEND_ONCE_MASK) > 0);
      }
    }

    public string FileToExecute
    {
      get
      {
        return txtFile.Text;
      }
      set
      {
        txtFile.Text = value;
      }
    }

    public enum TabletButton
    {
      ACross = 1,
      BCircle = 2,
      XSquare1 = 3,
      YTriangle2 = 4,
      Up = 5,
      Down = 6,
      Left = 7,
      Right = 8,
      StartPlus = 9,
      BackSelectMinus = 10,
      PSXboxGuideHome = 11,
      PenClick = 12,
      People = 13,
      Hidden = 9999
    };

    public enum ButtonAction
    {
      None = 0,
      LeftClick = 1,
      RightClick = 2,
      ShowOptions = 7,
      MoveUp = 8,
      MoveDown = 9,
      MoveLeft = 10,
      MoveRight = 11,
      TurnOffTablet = 12,
      KeyboardKeypress = 13,
      SwitchTabletDisplay = 14,
      ExecuteFile = 15,
      ScrollUp = 16,
      ScrollDown = 17,
      MiddleClick = 18
    };

    public TabletOptionButton(TabletButton button)
    {
      InitializeComponent();

      pnlKeyboard.Visible = false;
      pnlExecute.Visible = false;
      cboValue.SelectedIndexChanged += cboValue_SelectedIndexChanged;

      _Resize();

      Button = button;
      lblButtonName.Text = _GetButtonName(button) + ": ";
      foreach (var action in _GetActionNames())
        cboValue.Items.Add(action.Value);
      cboValue.SelectedItem = ButtonAction.None.ToString();

      foreach (var key in Enum.GetValues(typeof(Keypress.KeyCode)))
        cboKey.Items.Add(key);
      cboKey.SelectedItem = Keypress.KeyCode.None;
    }

    public TabletOptionButton()
    {
      InitializeComponent();
    }

    private void cboValue_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cboValue.SelectedItem != null)
      {
        if (Convert.ToString(cboValue.SelectedItem) == _GetActionNames()[ButtonAction.KeyboardKeypress])
          pnlKeyboard.Visible = true;
        else
          pnlKeyboard.Visible = false;
        if (Convert.ToString(cboValue.SelectedItem) == _GetActionNames()[ButtonAction.ExecuteFile])
          pnlExecute.Visible = true;
        else
          pnlExecute.Visible = false;

        _Resize();
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      var ofd = new OpenFileDialog();
      ofd.AddExtension = false;
      ofd.CheckFileExists = false;
      ofd.CheckPathExists = false;
      ofd.Multiselect = false;
      ofd.RestoreDirectory = true;
      ofd.SupportMultiDottedExtensions = true;
      ofd.Title = "Select File to Execute";

      if (ofd.ShowDialog() == DialogResult.OK)
        txtFile.Text = ofd.FileName;
    }

    private void _Resize()
    {
      this.Height = grpMain.Height + grpMain.Margin.Vertical;
    }

    private Dictionary<ButtonAction, string> _GetActionNames()
    {
      var ret = new Dictionary<ButtonAction, string>();

      ret.Add(ButtonAction.None, "None");
      ret.Add(ButtonAction.LeftClick, "Left Mouse Click");
      ret.Add(ButtonAction.MoveDown, "Move Down");
      ret.Add(ButtonAction.MoveLeft, "Move Left");
      ret.Add(ButtonAction.MoveRight, "Move Right");
      ret.Add(ButtonAction.MoveUp, "Move Up");
      ret.Add(ButtonAction.RightClick, "Right Mouse Click");
      ret.Add(ButtonAction.ShowOptions, "Show Options");
      ret.Add(ButtonAction.TurnOffTablet, "Turn Off Device (Xbox 360 Only)");
      ret.Add(ButtonAction.KeyboardKeypress, "Press Keyboard Key(s)");
      ret.Add(ButtonAction.SwitchTabletDisplay, "Switch Display");
      ret.Add(ButtonAction.ExecuteFile, "Execute File");
      ret.Add(ButtonAction.ScrollUp, "Scroll Up");
      ret.Add(ButtonAction.ScrollDown, "Scroll Down");
      ret.Add(ButtonAction.MiddleClick, "Middle Mouse Click");

      return ret;
    }

    private string _GetButtonName(TabletButton button)
    {
      string ret = String.Empty;

      switch (button)
      {
        case TabletButton.ACross:
          {
            ret = "A / Cross";
            break;
          }
        case TabletButton.BackSelectMinus:
          {
            ret = "Back / Select / Minus";
            break;
          }
        case TabletButton.BCircle:
          {
            ret = "B / Circle";
            break;
          }
        case TabletButton.PSXboxGuideHome:
          {
            ret = "Home / PS / Xbox Guide Button";
            break;
          }
        case TabletButton.XSquare1:
          {
            ret = "X / Square / One";
            break;
          }
        case TabletButton.YTriangle2:
          {
            ret = "Y / Triangle / Two";
            break;
          }
        case TabletButton.PenClick:
          {
            ret = "Pen Click";
            break;
          }
        case TabletButton.StartPlus:
          {
            ret = "Start / Plus";
            break;
          }
        default:
          {
            ret = button.ToString();
            break;
          }
      }

      return ret;
    }
  }
}
