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

        return ret;
      }
      set
      {
        foreach (var action in _GetActionNames())
          if (action.Key == value)
            cboValue.SelectedItem = action.Value;
      }
    }

    public enum TabletButton
    {
      ACross = 1,
      BCircle = 2,
      XSquare = 3,
      YTriangle = 4,
      Up = 5,
      Down = 6,
      Left = 7,
      Right = 8,
      Start = 9,
      BackSelect = 10,
      PSXboxGuide = 11
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
      TurnOffTablet = 12
    };

    public TabletOptionButton(TabletButton button)
    {
      InitializeComponent();

      Button = button;
      lblButtonName.Text = _GetButtonName(button) + ": ";
      foreach (var action in _GetActionNames())
        cboValue.Items.Add(action.Value);
      cboValue.SelectedItem = ButtonAction.None.ToString();
    }

    public TabletOptionButton()
    {
      InitializeComponent();
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
      ret.Add(ButtonAction.TurnOffTablet, "Turn Off Tablet (Xbox 360 Only)");

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
        case TabletButton.BackSelect:
          {
            ret = "Back / Select";
            break;
          }
        case TabletButton.BCircle:
          {
            ret = "B / Circle";
            break;
          }
        case TabletButton.PSXboxGuide:
          {
            ret = "PS / Xbox Guide Button";
            break;
          }
        case TabletButton.XSquare:
          {
            ret = "X / Square";
            break;
          }
        case TabletButton.YTriangle:
          {
            ret = "Y / Triangle";
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
