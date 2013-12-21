using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace uDrawTablet
{
  public partial class WiiTabletDevice : UserControl
  {
    public event EventHandler<EventArgs> ButtonClicked;

    public WiiTabletDevice(int index)
    {
      InitializeComponent();

      label1.Text = "Wii Tablet Device " + (index + 1).ToString();
    }

    private void WiiTabletDevice_Resize(object sender, EventArgs e)
    {
      this.Width = this.Parent.Width - 5;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (ButtonClicked != null)
      {
        ButtonClicked(this, EventArgs.Empty);
      }
    }
  }
}
