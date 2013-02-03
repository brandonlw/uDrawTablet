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
      this.pnlKeyboard = new System.Windows.Forms.Panel();
      this.lblButtonName = new System.Windows.Forms.Label();
      this.cboValue = new System.Windows.Forms.ComboBox();
      this.pnlMain = new System.Windows.Forms.Panel();
      this.cboKey = new System.Windows.Forms.ComboBox();
      this.chkCtrl = new System.Windows.Forms.CheckBox();
      this.chkShift = new System.Windows.Forms.CheckBox();
      this.chkAlt = new System.Windows.Forms.CheckBox();
      this.chkWin = new System.Windows.Forms.CheckBox();
      this.grpMain = new System.Windows.Forms.GroupBox();
      this.chkSendKeysOnce = new System.Windows.Forms.CheckBox();
      this.pnlKeyboard.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.grpMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlKeyboard
      // 
      this.pnlKeyboard.BackColor = System.Drawing.SystemColors.Control;
      this.pnlKeyboard.Controls.Add(this.chkSendKeysOnce);
      this.pnlKeyboard.Controls.Add(this.chkWin);
      this.pnlKeyboard.Controls.Add(this.chkAlt);
      this.pnlKeyboard.Controls.Add(this.chkShift);
      this.pnlKeyboard.Controls.Add(this.chkCtrl);
      this.pnlKeyboard.Controls.Add(this.cboKey);
      this.pnlKeyboard.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlKeyboard.Location = new System.Drawing.Point(3, 41);
      this.pnlKeyboard.Name = "pnlKeyboard";
      this.pnlKeyboard.Size = new System.Drawing.Size(413, 52);
      this.pnlKeyboard.TabIndex = 2;
      // 
      // lblButtonName
      // 
      this.lblButtonName.AutoSize = true;
      this.lblButtonName.Location = new System.Drawing.Point(3, 3);
      this.lblButtonName.Name = "lblButtonName";
      this.lblButtonName.Size = new System.Drawing.Size(73, 13);
      this.lblButtonName.TabIndex = 0;
      this.lblButtonName.Text = "Button Name:";
      // 
      // cboValue
      // 
      this.cboValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cboValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboValue.FormattingEnabled = true;
      this.cboValue.Location = new System.Drawing.Point(178, 0);
      this.cboValue.Name = "cboValue";
      this.cboValue.Size = new System.Drawing.Size(232, 21);
      this.cboValue.TabIndex = 1;
      // 
      // pnlMain
      // 
      this.pnlMain.AutoSize = true;
      this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
      this.pnlMain.Controls.Add(this.cboValue);
      this.pnlMain.Controls.Add(this.lblButtonName);
      this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlMain.Location = new System.Drawing.Point(3, 17);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new System.Drawing.Size(413, 24);
      this.pnlMain.TabIndex = 3;
      // 
      // cboKey
      // 
      this.cboKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cboKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboKey.FormattingEnabled = true;
      this.cboKey.Location = new System.Drawing.Point(253, 28);
      this.cboKey.Name = "cboKey";
      this.cboKey.Size = new System.Drawing.Size(157, 21);
      this.cboKey.TabIndex = 2;
      // 
      // chkCtrl
      // 
      this.chkCtrl.AutoSize = true;
      this.chkCtrl.Location = new System.Drawing.Point(0, 30);
      this.chkCtrl.Name = "chkCtrl";
      this.chkCtrl.Size = new System.Drawing.Size(51, 17);
      this.chkCtrl.TabIndex = 3;
      this.chkCtrl.Text = "CTRL";
      this.chkCtrl.UseVisualStyleBackColor = true;
      // 
      // chkShift
      // 
      this.chkShift.AutoSize = true;
      this.chkShift.Location = new System.Drawing.Point(57, 30);
      this.chkShift.Name = "chkShift";
      this.chkShift.Size = new System.Drawing.Size(55, 17);
      this.chkShift.TabIndex = 4;
      this.chkShift.Text = "SHIFT";
      this.chkShift.UseVisualStyleBackColor = true;
      // 
      // chkAlt
      // 
      this.chkAlt.AutoSize = true;
      this.chkAlt.Location = new System.Drawing.Point(118, 30);
      this.chkAlt.Name = "chkAlt";
      this.chkAlt.Size = new System.Drawing.Size(44, 17);
      this.chkAlt.TabIndex = 5;
      this.chkAlt.Text = "ALT";
      this.chkAlt.UseVisualStyleBackColor = true;
      // 
      // chkWin
      // 
      this.chkWin.AutoSize = true;
      this.chkWin.Location = new System.Drawing.Point(168, 30);
      this.chkWin.Name = "chkWin";
      this.chkWin.Size = new System.Drawing.Size(69, 17);
      this.chkWin.TabIndex = 6;
      this.chkWin.Text = "Windows";
      this.chkWin.UseVisualStyleBackColor = true;
      // 
      // grpMain
      // 
      this.grpMain.AutoSize = true;
      this.grpMain.BackColor = System.Drawing.SystemColors.Control;
      this.grpMain.Controls.Add(this.pnlMain);
      this.grpMain.Controls.Add(this.pnlKeyboard);
      this.grpMain.Dock = System.Windows.Forms.DockStyle.Top;
      this.grpMain.Location = new System.Drawing.Point(0, 0);
      this.grpMain.Name = "grpMain";
      this.grpMain.Size = new System.Drawing.Size(419, 96);
      this.grpMain.TabIndex = 4;
      this.grpMain.TabStop = false;
      // 
      // chkSendKeysOnce
      // 
      this.chkSendKeysOnce.AutoSize = true;
      this.chkSendKeysOnce.Location = new System.Drawing.Point(0, 7);
      this.chkSendKeysOnce.Name = "chkSendKeysOnce";
      this.chkSendKeysOnce.Size = new System.Drawing.Size(132, 17);
      this.chkSendKeysOnce.TabIndex = 7;
      this.chkSendKeysOnce.Text = "Send key(s) only once";
      this.chkSendKeysOnce.UseVisualStyleBackColor = true;
      // 
      // TabletOptionButton
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.grpMain);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "TabletOptionButton";
      this.Size = new System.Drawing.Size(419, 103);
      this.pnlKeyboard.ResumeLayout(false);
      this.pnlKeyboard.PerformLayout();
      this.pnlMain.ResumeLayout(false);
      this.pnlMain.PerformLayout();
      this.grpMain.ResumeLayout(false);
      this.grpMain.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel pnlKeyboard;
    private System.Windows.Forms.Label lblButtonName;
    private System.Windows.Forms.ComboBox cboValue;
    private System.Windows.Forms.Panel pnlMain;
    private System.Windows.Forms.ComboBox cboKey;
    private System.Windows.Forms.CheckBox chkWin;
    private System.Windows.Forms.CheckBox chkAlt;
    private System.Windows.Forms.CheckBox chkShift;
    private System.Windows.Forms.CheckBox chkCtrl;
    private System.Windows.Forms.GroupBox grpMain;
    private System.Windows.Forms.CheckBox chkSendKeysOnce;
  }
}
