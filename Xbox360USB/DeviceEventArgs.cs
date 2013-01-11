using System;
using System.Collections.Generic;
using System.Text;

namespace Xbox360USB
{
  public class DeviceEventArgs : EventArgs
  {
    #region Declarations

    public int Index { get; set; }

    #endregion

    #region Constructors/Teardown

    public DeviceEventArgs(int index)
    {
      Index = index;
    }

    #endregion
  }
}
