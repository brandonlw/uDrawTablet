using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using uDrawLib;

namespace uDrawTablet
{
  public partial class Options : Form
  {
    #region Declarations

    private NotifyIcon _icon;
    private ContextMenu _menu;
    private bool _inOptions;
    private Slot[] _slots;

    public struct Slot
    {
      public int Index;
      public Control Label;
      public Control Button;

      public Slot(int index, Control label, Control button)
      {
        Index = index;
        Label = label;
        Button = button;
      }
    };

    #endregion

    #region Constructors / Teardown

    public Options()
    {
      InitializeComponent();

      _slots = new Slot[] { new Slot(1, lbl360_1, btnSlot1Settings),
          new Slot(2, lbl360_2, btnSlot2Settings), new Slot(3, lbl360_3, btnSlot3Settings),
          new Slot(4, lbl360_4, btnSlot4Settings) };

      _menu = new ContextMenu();
      _menu.MenuItems.Add("Options...", OnOptionsClick);
      _menu.MenuItems.Add("Exit", OnExit);

      _icon = new NotifyIcon();
      _icon.Text = Assembly.GetExecutingAssembly().GetName().Name;
      _icon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
      _icon.ContextMenu = _menu;
      _icon.Visible = true;
      _icon.DoubleClick += _icon_DoubleClick;

      lbl360_1.DoubleClick += Xbox360Label_DoubleClick;
      lbl360_2.DoubleClick += Xbox360Label_DoubleClick;
      lbl360_3.DoubleClick += Xbox360Label_DoubleClick;
      lbl360_4.DoubleClick += Xbox360Label_DoubleClick;

      _StartInterface();
    }

    #endregion

    #region Public Methods

    public void ShowOptions()
    {
      OnOptionsClick(this, EventArgs.Empty);
    }

    #endregion

    #region Overrides

    protected override void SetVisibleCore(bool value)
    {
      if (!this.IsHandleCreated)
      {
        value = false;
        CreateHandle();
      }

      base.SetVisibleCore(value);
    }

    protected override void Dispose(bool isDisposing)
    {
      if (isDisposing)
      {
        if (components != null)
          components.Dispose();

        //Get rid of the icon
        _icon.Dispose();
      }

      base.Dispose(isDisposing);
    }

    #endregion

    #region Event Handlers

    private void Receiver_DeviceChanged(object sender, Xbox360USB.DeviceEventArgs e)
    {
      this.Invoke(new MethodInvoker(_SetStatuses));
    }

    private void Xbox360Label_DoubleClick(object sender, EventArgs e)
    {
      foreach (var slot in _slots)
      {
        if (slot.Label == sender)
        {
          foreach (var t in MouseInterface.Tablets)
          {
            if (slot.Index == t.ReceiverIndex)
            {
              t.Receiver.TurnOffDevice(t.ReceiverIndex);
              break;
            }
          }

          break;
        }
      }
    }

    private void _icon_DoubleClick(object sender, EventArgs e)
    {
      ShowOptions();
    }

    private void btnRedetect_Click(object sender, EventArgs e)
    {
      _StartInterface();

      _SetStatuses();
    }

    private void OnOptionsClick(object sender, EventArgs e)
    {
      _inOptions = true;

      _ShowOptions();
    }

    private void OnExit(object sender, EventArgs e)
    {
      try
      {
        _inOptions = false;

        //Turn off all 360 devices
        var devicesToTurnOff = new List<TabletConnection>();
        foreach (var t in MouseInterface.Tablets)
        {
          if (t.Tablet as Xbox360uDrawTabletDevice != null)
            devicesToTurnOff.Add(t);
        }
        foreach (var t in devicesToTurnOff)
          t.Receiver.TurnOffDevice(t.ReceiverIndex);

        _StopInterface();
      }
      finally
      {
        Application.Exit();
      }
    }

    private void btnSlotSettings_Click(object sender, EventArgs e)
    {
      //Find out which one we're dealing with
      bool isPS3 = (sender == btnPS3Settings);
      int? index = null;
      if (!isPS3)
      {
        //Find which 360 slot
        foreach (var slot in _slots)
        {
          if (sender == slot.Button)
          {
            index = slot.Index;
            break;
          }
        }
      }

      string fileName = MouseInterface.GetSettingsFileName(isPS3, index);
      if (!String.IsNullOrEmpty(fileName))
      {
        var frm = new TabletOptions(fileName, TabletSettings.LoadSettings(fileName));
        frm.ShowDialog();
        frm.Dispose();

        MouseInterface.ReloadSettings();
      }
    }

    private void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_inOptions)
      {
        _inOptions = false;

        e.Cancel = true;
        this.Hide();
      }
    }

    #endregion

    #region Local Methods

    private void _SetStatuses()
    {
      //Determine if PS3 tablet exists
      if (PS3uDrawTabletDevice.IsDetected())
      {
        grpPS3.Enabled = true;
        lblPS3Tablet.ForeColor = Color.Green;
      }
      else
      {
        grpPS3.Enabled = false;
      }

      //Determine if 360 wireless receiver is connected
      if (MouseInterface.Receiver != null && MouseInterface.Receiver.IsReceiverConnected)
      {
        grp360.Enabled = true;
        lbl360Receiver.ForeColor = Color.Green;

        foreach (var slot in _slots)
        {
          if (MouseInterface.Receiver.IsDeviceConnected(slot.Index))
          {
            var description = "N/A";
            var info = MouseInterface.Receiver.GetDeviceInformation(slot.Index);
            if (info != null)
            {
              switch (info.Subtype)
              {
                case Xbox360USB.WirelessReceiver.DeviceSubtype.Controller:
                  {
                    description = "Controller: " + info.ToString();
                    break;
                  }
                case Xbox360USB.WirelessReceiver.DeviceSubtype.uDrawTablet:
                  {
                    description = "uDraw GameTablet: " + info.ToString();
                    break;
                  }
                default:
                  description = "Unknown Device";
                  break;
              }
            }

            slot.Label.ForeColor = Color.Green;
            slot.Label.Text = String.Format("Slot {0}: {1}", slot.Index, description);
            slot.Button.Enabled = true;
          }
          else
          {
            slot.Label.ForeColor = Color.Red;
            slot.Label.Text = String.Format("Slot {0}: N/A", slot.Index);
            slot.Button.Enabled = false;
          }
        }
      }
      else
      {
        grp360.Enabled = false;
      }
    }

    private void _StartInterface()
    {
      MouseInterface.Start(this);

      if (MouseInterface.Receiver != null)
      {
        MouseInterface.Receiver.DeviceConnected += Receiver_DeviceChanged;
        MouseInterface.Receiver.DeviceDisconnected += Receiver_DeviceChanged;
      }
    }

    private void _StopInterface()
    {
      MouseInterface.Stop();

      if (MouseInterface.Receiver != null)
      {
        MouseInterface.Receiver.DeviceConnected -= Receiver_DeviceChanged;
        MouseInterface.Receiver.DeviceDisconnected -= Receiver_DeviceChanged;
      }
    }

    private void _ShowOptions()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new MethodInvoker(_ShowOptions));
      }
      else
      {
        _SetStatuses();

        this.Show();
      }
    }

    #endregion
  }
}
