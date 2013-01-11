using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public class TabletDPadState : ICloneable
  {
    public bool UpHeld;
    public bool DownHeld;
    public bool LeftHeld;
    public bool RightHeld;

    public Object Clone()
    {
      var ret = new TabletDPadState();

      ret.UpHeld = this.UpHeld;
      ret.DownHeld = this.DownHeld;
      ret.LeftHeld = this.LeftHeld;
      ret.RightHeld = this.RightHeld;

      return ret;
    }
  };
}
