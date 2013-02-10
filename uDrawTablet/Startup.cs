using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using uDrawLib;

namespace uDrawTablet
{
  public class Startup
  {
    [STAThread]
    static void Main()
    {
      //Application.EnableVisualStyles();
      //Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Options());
    }
  }
}
