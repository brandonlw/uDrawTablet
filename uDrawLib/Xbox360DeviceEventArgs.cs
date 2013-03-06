using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public class Xbox360DeviceEventArgs : EventArgs
  {
    public int Index { get; set; }

    public Xbox360DeviceEventArgs(int index)
    {
      Index = index;
    }
  }
}
