namespace uDrawTablet
{
  partial class TabletOptionButton
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
      this.lblButtonName = new System.Windows.Forms.Label();
      this.cboValue = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // lblButtonName
      // 
      this.lblButtonName.AutoSize = true;
      this.lblButtonName.Location = new System.Drawing.Point(9, 6);
      this.lblButtonName.Name = "lblButtonName";
      this.lblButtonName.Size = new System.Drawing.Size(72, 13);
      this.lblButtonName.TabIndex = 0;
      this.lblButtonName.Text = "Button Name:";
      // 
      // cboValue
      // 
      this.cboValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cboValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboValue.FormattingEnabled = true;
      this.cboValue.Location = new System.Drawing.Point(111, 3);
      this.cboValue.Name = "cboValue";
      this.cboValue.Size = new System.Drawing.Size(232, 21);
      this.cboValue.TabIndex = 1;
      // 
      // TabletOptionButton
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.cboValue);
      this.Controls.Add(this.lblButtonName);
      this.Name = "TabletOptionButton";
      this.Size = new System.Drawing.Size(346, 27);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblButtonName;
    private System.Windows.Forms.ComboBox cboValue;
  }
}
