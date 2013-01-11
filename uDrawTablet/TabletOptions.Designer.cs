namespace uDrawTablet
{
  partial class TabletOptions
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabletOptions));
      this.tbcMain = new System.Windows.Forms.TabControl();
      this.tbpButtons = new System.Windows.Forms.TabPage();
      this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
      this.tbpMovement = new System.Windows.Forms.TabPage();
      this.grpMovementSettings = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.trbPenClick = new System.Windows.Forms.TrackBar();
      this.trbSpeed = new System.Windows.Forms.TrackBar();
      this.label3 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.grpMovementType = new System.Windows.Forms.GroupBox();
      this.rdoAbsolute = new System.Windows.Forms.RadioButton();
      this.rdoRelative = new System.Windows.Forms.RadioButton();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this.trbPrecision = new System.Windows.Forms.TrackBar();
      this.label8 = new System.Windows.Forms.Label();
      this.lblPrecision = new System.Windows.Forms.Label();
      this.tbcMain.SuspendLayout();
      this.tbpButtons.SuspendLayout();
      this.tbpMovement.SuspendLayout();
      this.grpMovementSettings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trbPenClick)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
      this.grpMovementType.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trbPrecision)).BeginInit();
      this.SuspendLayout();
      // 
      // tbcMain
      // 
      this.tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbcMain.Controls.Add(this.tbpButtons);
      this.tbcMain.Controls.Add(this.tbpMovement);
      this.tbcMain.Location = new System.Drawing.Point(7, 7);
      this.tbcMain.Name = "tbcMain";
      this.tbcMain.SelectedIndex = 0;
      this.tbcMain.Size = new System.Drawing.Size(449, 424);
      this.tbcMain.TabIndex = 7;
      // 
      // tbpButtons
      // 
      this.tbpButtons.Controls.Add(this.flpMain);
      this.tbpButtons.Location = new System.Drawing.Point(4, 22);
      this.tbpButtons.Name = "tbpButtons";
      this.tbpButtons.Size = new System.Drawing.Size(441, 398);
      this.tbpButtons.TabIndex = 2;
      this.tbpButtons.Text = "Buttons";
      this.tbpButtons.UseVisualStyleBackColor = true;
      // 
      // flpMain
      // 
      this.flpMain.AutoScroll = true;
      this.flpMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flpMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpMain.Location = new System.Drawing.Point(0, 0);
      this.flpMain.Name = "flpMain";
      this.flpMain.Size = new System.Drawing.Size(441, 398);
      this.flpMain.TabIndex = 0;
      this.flpMain.WrapContents = false;
      // 
      // tbpMovement
      // 
      this.tbpMovement.Controls.Add(this.grpMovementSettings);
      this.tbpMovement.Controls.Add(this.grpMovementType);
      this.tbpMovement.Location = new System.Drawing.Point(4, 22);
      this.tbpMovement.Name = "tbpMovement";
      this.tbpMovement.Padding = new System.Windows.Forms.Padding(3);
      this.tbpMovement.Size = new System.Drawing.Size(441, 398);
      this.tbpMovement.TabIndex = 0;
      this.tbpMovement.Text = "Movement";
      this.tbpMovement.UseVisualStyleBackColor = true;
      // 
      // grpMovementSettings
      // 
      this.grpMovementSettings.Controls.Add(this.label7);
      this.grpMovementSettings.Controls.Add(this.trbPrecision);
      this.grpMovementSettings.Controls.Add(this.label8);
      this.grpMovementSettings.Controls.Add(this.lblPrecision);
      this.grpMovementSettings.Controls.Add(this.label1);
      this.grpMovementSettings.Controls.Add(this.label2);
      this.grpMovementSettings.Controls.Add(this.label5);
      this.grpMovementSettings.Controls.Add(this.trbPenClick);
      this.grpMovementSettings.Controls.Add(this.trbSpeed);
      this.grpMovementSettings.Controls.Add(this.label3);
      this.grpMovementSettings.Controls.Add(this.label6);
      this.grpMovementSettings.Controls.Add(this.label4);
      this.grpMovementSettings.Location = new System.Drawing.Point(9, 87);
      this.grpMovementSettings.Name = "grpMovementSettings";
      this.grpMovementSettings.Size = new System.Drawing.Size(397, 305);
      this.grpMovementSettings.TabIndex = 11;
      this.grpMovementSettings.TabStop = false;
      this.grpMovementSettings.Text = "Movement Settings";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 17);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(102, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Pen click sensitivity:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(121, 49);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(66, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Press Softer";
      // 
      // label5
      // 
      this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(366, 129);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(28, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Fast";
      // 
      // trbPenClick
      // 
      this.trbPenClick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.trbPenClick.Location = new System.Drawing.Point(114, 8);
      this.trbPenClick.Name = "trbPenClick";
      this.trbPenClick.Size = new System.Drawing.Size(277, 42);
      this.trbPenClick.TabIndex = 0;
      // 
      // trbSpeed
      // 
      this.trbSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.trbSpeed.Location = new System.Drawing.Point(114, 88);
      this.trbSpeed.Maximum = 25;
      this.trbSpeed.Minimum = 1;
      this.trbSpeed.Name = "trbSpeed";
      this.trbSpeed.Size = new System.Drawing.Size(277, 42);
      this.trbSpeed.TabIndex = 7;
      this.trbSpeed.Value = 1;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(325, 49);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(69, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Press Harder";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(121, 129);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(29, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Slow";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(15, 88);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(93, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Movement speed:";
      // 
      // grpMovementType
      // 
      this.grpMovementType.Controls.Add(this.rdoAbsolute);
      this.grpMovementType.Controls.Add(this.rdoRelative);
      this.grpMovementType.Location = new System.Drawing.Point(9, 6);
      this.grpMovementType.Name = "grpMovementType";
      this.grpMovementType.Size = new System.Drawing.Size(397, 75);
      this.grpMovementType.TabIndex = 10;
      this.grpMovementType.TabStop = false;
      this.grpMovementType.Text = "Movement Type";
      // 
      // rdoAbsolute
      // 
      this.rdoAbsolute.AutoSize = true;
      this.rdoAbsolute.Location = new System.Drawing.Point(9, 43);
      this.rdoAbsolute.Name = "rdoAbsolute";
      this.rdoAbsolute.Size = new System.Drawing.Size(329, 17);
      this.rdoAbsolute.TabIndex = 1;
      this.rdoAbsolute.TabStop = true;
      this.rdoAbsolute.Text = "Absolute (taps on tablet surface correspond to screen location)";
      this.rdoAbsolute.UseVisualStyleBackColor = true;
      // 
      // rdoRelative
      // 
      this.rdoRelative.AutoSize = true;
      this.rdoRelative.Location = new System.Drawing.Point(9, 20);
      this.rdoRelative.Name = "rdoRelative";
      this.rdoRelative.Size = new System.Drawing.Size(304, 17);
      this.rdoRelative.TabIndex = 0;
      this.rdoRelative.TabStop = true;
      this.rdoRelative.Text = "Relative (swipes move mouse cursor in indicated direction)";
      this.rdoRelative.UseVisualStyleBackColor = true;
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(294, 437);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(78, 25);
      this.btnSave.TabIndex = 9;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(378, 437);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(78, 25);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // label7
      // 
      this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(363, 196);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(31, 13);
      this.label7.TabIndex = 13;
      this.label7.Text = "More";
      // 
      // trbPrecision
      // 
      this.trbPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.trbPrecision.Location = new System.Drawing.Point(114, 155);
      this.trbPrecision.Minimum = 1;
      this.trbPrecision.Name = "trbPrecision";
      this.trbPrecision.Size = new System.Drawing.Size(277, 42);
      this.trbPrecision.TabIndex = 11;
      this.trbPrecision.Value = 1;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(121, 196);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(28, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "Less";
      // 
      // lblPrecision
      // 
      this.lblPrecision.AutoSize = true;
      this.lblPrecision.Location = new System.Drawing.Point(55, 155);
      this.lblPrecision.Name = "lblPrecision";
      this.lblPrecision.Size = new System.Drawing.Size(53, 13);
      this.lblPrecision.TabIndex = 10;
      this.lblPrecision.Text = "Precision:";
      // 
      // TabletOptions
      // 
      this.AcceptButton = this.btnSave;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(461, 468);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.tbcMain);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "TabletOptions";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Tablet Options";
      this.tbcMain.ResumeLayout(false);
      this.tbpButtons.ResumeLayout(false);
      this.tbpMovement.ResumeLayout(false);
      this.grpMovementSettings.ResumeLayout(false);
      this.grpMovementSettings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trbPenClick)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
      this.grpMovementType.ResumeLayout(false);
      this.grpMovementType.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trbPrecision)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tbcMain;
    private System.Windows.Forms.TabPage tbpButtons;
    private System.Windows.Forms.TabPage tbpMovement;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TrackBar trbPenClick;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TrackBar trbSpeed;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.GroupBox grpMovementType;
    private System.Windows.Forms.RadioButton rdoAbsolute;
    private System.Windows.Forms.RadioButton rdoRelative;
    private System.Windows.Forms.GroupBox grpMovementSettings;
    private System.Windows.Forms.FlowLayoutPanel flpMain;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TrackBar trbPrecision;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label lblPrecision;
  }
}