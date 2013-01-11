using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public class TabletButtonState : ICloneable
  {
    public bool CircleHeld;
    public bool CrossHeld;
    public bool SquareHeld;
    public bool TriangleHeld;
    public bool PSHeld;
    public bool SelectHeld;
    public bool StartHeld;

    public Object Clone()
    {
      var ret = new TabletButtonState();

      ret.CircleHeld = this.CircleHeld;
      ret.CrossHeld = this.CrossHeld;
      ret.PSHeld = this.PSHeld;
      ret.SelectHeld = this.SelectHeld;
      ret.SquareHeld = this.SquareHeld;
      ret.StartHeld = this.StartHeld;
      ret.TriangleHeld = this.TriangleHeld;

      return ret;
    }
  };
}
