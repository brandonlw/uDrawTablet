using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawLib
{
  public class DataReceivedEventArgs : EventArgs
  {
    #region Declarations

    private byte _reportType;
    private byte[] _data;

    #endregion

    #region Constructors / Teardown

    public DataReceivedEventArgs(byte reportType, byte[] data)
    {
      _reportType = reportType;
      _data = data;
    }

    #endregion

    #region Public Properties

    public byte ReportType
    {
      get
      {
        return _reportType;
      }
    }

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
