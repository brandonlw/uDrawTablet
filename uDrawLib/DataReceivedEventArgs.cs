using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public class DataReceivedEventArgs : EventArgs
  {
    #region Declarations

    private byte[] _data;

    #endregion

    #region Constructors / Teardown

    public DataReceivedEventArgs(byte[] data)
    {
      _data = data;
    }

    #endregion

    #region Public Properties

    public byte[] Data
    {
      get
      {
        return _data;
      }
    }

    #endregion
  }
}
