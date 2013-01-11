using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using uDrawLib;
using Xbox360USB;

namespace uDrawTablet
{
  public class TabletConnection
  {
    #region Declarations

    private ITabletDevice _tablet;

    public WirelessReceiver Receiver { get; set; }
    public int ReceiverIndex { get; set; }
    public TabletSettings Settings { get; set; }
    public TabletButtonState LastButtonState { get; set; }
    public TabletDPadState LastDPadState { get; set; }
    public bool LastPressure { get; set; }
    public Point? LastPressurePoint { get; set; }
    public int VerticalDelta { get; set; }
    public int HorizontalDelta { get; set; }

    #endregion

    #region Public Properties

    public ITabletDevice Tablet
    {
      get
      {
        return _tablet;
      }
      set
      {
        //Unwire old events
        if (_tablet != null)
        {
          _tablet.ButtonStateChanged -= _tablet_ButtonStateChanged;
        }

        //Set value
        _tablet = value;

        //Wire new events
        if (_tablet != null)
        {
          LastButtonState = (TabletButtonState)_tablet.ButtonState.Clone();
          LastDPadState = (TabletDPadState)_tablet.DPadState.Clone();
          _tablet.ButtonStateChanged += _tablet_ButtonStateChanged;
          _tablet.DPadStateChanged += _tablet_DPadStateChanged;
        }
      }
    }

    #endregion

    #region Events

    public event EventHandler<EventArgs> ButtonStateChanged;
    public event EventHandler<EventArgs> DPadStateChanged;

    #endregion

    #region Constructors/Teardown

    public TabletConnection(ITabletDevice tablet)
    {
      Tablet = tablet;

      Settings = new TabletSettings();
    }

    public TabletConnection(ITabletDevice tablet, WirelessReceiver receiver, int index)
    {
      Tablet = tablet;
      Receiver = receiver;
      ReceiverIndex = index;

      Settings = new TabletSettings();
    }

    #endregion

    #region Event Handlers

    private void _tablet_ButtonStateChanged(object sender, EventArgs e)
    {
      if (ButtonStateChanged != null)
        ButtonStateChanged(this, e);

      LastButtonState = (TabletButtonState)((ITabletDevice)sender).ButtonState.Clone();
    }

    private void _tablet_DPadStateChanged(object sender, EventArgs e)
    {
      if (DPadStateChanged != null)
        DPadStateChanged(this, e);

      LastDPadState = (TabletDPadState)((ITabletDevice)sender).DPadState.Clone();
    }

    #endregion
  }
}
