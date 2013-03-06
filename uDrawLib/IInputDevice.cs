using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace uDrawLib
{
  public interface IInputDevice
  {
    /// <summary>
    /// Indicates non-directional button press states have changed.
    /// </summary>
    event EventHandler<EventArgs> ButtonStateChanged;

    /// <summary>
    /// Indicates directional button press states have changed.
    /// </summary>
    event EventHandler<EventArgs> DPadStateChanged;

    /// <summary>
    /// Indicates the pressed state of every non-directional button on the tablet.
    /// </summary>
    TabletButtonState ButtonState { get; set; }

    /// <summary>
    /// Indicates the pressed state of each direction on the directional pad.
    /// </summary>
    TabletDPadState DPadState { get; set; }

    /// <summary>
    /// Indicates whether the nib/stylus, fingers, or nothing are currently pressing on the tablet.
    /// </summary>
    TabletPressureType PressureType { get; set; }

    /// <summary>
    /// The raw pressure being applied by the pen. Only valid with TabletPressureType.PenPressed.
    /// </summary>
    ushort PenPressure { get; set; }

    /// <summary>
    /// Indicates the distance between fingers. Only valid with TabletPressureType.Multitouch.
    /// </summary>
    ushort MultitouchDistance { get; set; }

    /// <summary>
    /// Indicates the absolute coordinates (from top-left corner) of the current pressure point.
    /// Only valid with TabletPressureType.PenPressed and TabletPressureType.FingerPressed.
    /// </summary>
    Point PressurePoint { get; set; }

    /// <summary>
    /// Raw X-, Y-, and Z-axis data from the accelerometer.
    /// </summary>
    TabletAccelerometerData AccelerometerData { get; set; }
  }
}
