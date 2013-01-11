using System;
using System.Collections.Generic;
using System.Text;

namespace Xbox360USB
{
  public class DataReceivedEventArgs : EventArgs
  {
    #region Declarations

    public int Index { get; internal set; }
    public byte[] Data { get; internal set; }

    #endregion

    #region Constructors/Teardown

    public DataReceivedEventArgs(int index, byte[] data)
    {
      Index = index;
      Data = data;
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
      return BitConverter.ToString(Data);
    }

    #endregion
  }
}
