namespace uDrawTablet
{
  partial class DockOption
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblInstructions = new System.Windows.Forms.Label();
      this.pnlBounds = new System.Windows.Forms.Panel();
      this.cboDockOptions = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // lblInstructions
      // 
      this.lblInstructions.AutoSize = true;
      this.lblInstructions.Location = new System.Drawing.Point(3, 13);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(68, 13);
      this.lblInstructions.TabIndex = 0;
      this.lblInstructions.Text = "Instructions:";
      // 
      // pnlBounds
      // 
      this.pnlBounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlBounds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlBounds.Location = new System.Drawing.Point(3, 56);
      this.pnlBounds.Name = "pnlBounds";
      this.pnlBounds.Size = new System.Drawing.Size(271, 154);
      this.pnlBounds.TabIndex = 1;
      this.pnlBounds.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBounds_Paint);
      // 
      // cboDockOptions
      // 
      this.cboDockOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboDockOptions.FormattingEnabled = true;
      this.cboDockOptions.Location = new System.Drawing.Point(6, 29);
      this.cboDockOptions.Name = "cboDockOptions";
      this.cboDockOptions.Size = new System.Drawing.Size(165, 21);
      this.cboDockOptions.TabIndex = 2;
      this.cboDockOptions.SelectedIndexChanged += new System.EventHandler(this.cboDockOptions_SelectedIndexChanged);
      // 
      // DockOption
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.cboDockOptions);
      this.Controls.Add(this.pnlBounds);
      this.Controls.Add(this.lblInstructions);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "DockOption";
      this.Size = new System.Drawing.Size(277, 213);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblInstructions;
    private System.Windows.Forms.Panel pnlBounds;
    private System.Windows.Forms.ComboBox cboDockOptions;
  }
}
