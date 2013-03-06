using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public class ChatpadKeyStateEventArgs : EventArgs
  {
    public int Index { get; set; }
    public byte KeyCode { get; set; }
    public bool Held { get; set; }

    public ChatpadKeyStateEventArgs(int index, byte keyCode, bool held)
    {
      Index = index;
      KeyCode = keyCode;
      Held = held;
    }
  }
}
