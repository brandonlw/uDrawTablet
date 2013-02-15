using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace uDrawTablet
{
  public partial class DockOption : UserControl
  {
    public DockStyle Style { get; set; }

    public DockOptionValue Value
    {
      get
      {
        return (DockOptionValue)cboDockOptions.SelectedItem;
      }
      set
      {
        cboDockOptions.SelectedItem = value;
      }
    }

    public enum DockStyle
    {
      Vertical = 1,
      Horizontal = 2
    };

    public enum DockOptionValue
    {
      Top = 1,
      Center = 2,
      Bottom = 3,
      Left = 4,
      Right = 5
    };

    public DockOption()
    {
      _Init(DockStyle.Vertical); //have to default to something...
    }

    public DockOption(DockStyle style)
    {
      _Init(style);
    }

    private void _Init(DockStyle style)
    {
      InitializeComponent();

      Style = style;

      cboDockOptions.Items.Clear();
      switch (style)
      {
        case DockStyle.Vertical:
          {
            lblInstructions.Text = "When tablet is higher than screen/window, dock to:";
            cboDockOptions.Items.Add(DockOptionValue.Top);
            cboDockOptions.Items.Add(DockOptionValue.Center);
            cboDockOptions.Items.Add(DockOptionValue.Bottom);
            break;
          }
        case DockStyle.Horizontal:
          {
            lblInstructions.Text = "When tablet is wider than screen/window, dock to:";
            cboDockOptions.Items.Add(DockOptionValue.Left);
            cboDockOptions.Items.Add(DockOptionValue.Center);
            cboDockOptions.Items.Add(DockOptionValue.Right);
            break;
          }
        default:
          break;
      }

      cboDockOptions.SelectedIndex = 0;
    }

    private void pnlBounds_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      var pen = new Pen(Color.Black, 1);

      float x = 0;
      float y = 0;
      float width = 0;
      float height = 0;
      bool ignore = false;
      var value = (DockOptionValue)cboDockOptions.SelectedItem;
      switch (Style)
      {
        case DockStyle.Vertical:
          {
            width = pnlBounds.Width - 3;
            height = (float)(pnlBounds.Height * 0.75);

            if (value == DockOptionValue.Center)
              y = (pnlBounds.Height - height) / 2;
            else if (value == DockOptionValue.Top)
              y = 0;
            else if (value == DockOptionValue.Bottom)
              y = pnlBounds.Height - height;

            break;
          }
        case DockStyle.Horizontal:
          {
            height = pnlBounds.Height - 3;
            width = (float)(pnlBounds.Width * 0.75);

            if (value == DockOptionValue.Center)
              x = (pnlBounds.Width - width) / 2;
            else if (value == DockOptionValue.Left)
              x = 0;
            else if (value == DockOptionValue.Right)
              x = pnlBounds.Width - width;

            break;
          }
        default:
          ignore = true;
          break;
      }

      if (!ignore)
      {
        g.FillRectangle(new SolidBrush(Color.LightBlue), x, y, width, height);
        g.DrawRectangle(pen, x, y, width, height);
      }
    }

    private void cboDockOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      pnlBounds.Invalidate();
      pnlBounds.Update();
    }
  }
}
