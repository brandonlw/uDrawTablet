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
      this.label7 = new System.Windows.Forms.Label();
      this.trbPrecision = new System.Windows.Forms.TrackBar();
      this.label8 = new System.Windows.Forms.Label();
      this.lblPrecision = new System.Windows.Forms.Label();
      this.lblPenClickSensitivity = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.trbPenClick = new System.Windows.Forms.TrackBar();
      this.trbSpeed = new System.Windows.Forms.TrackBar();
      this.label3 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.grpMovementType = new System.Windows.Forms.GroupBox();
      this.chkAllowFingerMovement = new System.Windows.Forms.CheckBox();
      this.rdoAbsolute = new System.Windows.Forms.RadioButton();
      this.rdoRelative = new System.Windows.Forms.RadioButton();
      this.tbpDisplays = new System.Windows.Forms.TabPage();
      this.flpDisplays = new System.Windows.Forms.FlowLayoutPanel();
      this.chkAllowAllDisplays = new System.Windows.Forms.CheckBox();
      this.pnlDisplays = new System.Windows.Forms.Panel();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.tbpBounds = new System.Windows.Forms.TabPage();
      this.flpBounds = new System.Windows.Forms.FlowLayoutPanel();
      this.chkRestrictToWindow = new System.Windows.Forms.CheckBox();
      this.chkMaintainAspectRatio = new System.Windows.Forms.CheckBox();
      this.flpCursorBounds = new System.Windows.Forms.FlowLayoutPanel();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.cboJoystickNumber = new System.Windows.Forms.ComboBox();
      this.lblPPJoyInstructions = new System.Windows.Forms.Label();
      this.tbcMain.SuspendLayout();
      this.tbpButtons.SuspendLayout();
      this.tbpMovement.SuspendLayout();
      this.grpMovementSettings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trbPrecision)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trbPenClick)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
      this.grpMovementType.SuspendLayout();
      this.tbpDisplays.SuspendLayout();
      this.flpDisplays.SuspendLayout();
      this.tbpBounds.SuspendLayout();
      this.flpBounds.SuspendLayout();
      this.SuspendLayout();
      // 
      // tbcMain
      // 
      this.tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbcMain.Controls.Add(this.tbpButtons);
      this.tbcMain.Controls.Add(this.tbpMovement);
      this.tbcMain.Controls.Add(this.tbpDisplays);
      this.tbcMain.Controls.Add(this.tbpBounds);
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
      this.grpMovementSettings.Controls.Add(this.lblPenClickSensitivity);
      this.grpMovementSettings.Controls.Add(this.label2);
      this.grpMovementSettings.Controls.Add(this.label5);
      this.grpMovementSettings.Controls.Add(this.trbPenClick);
      this.grpMovementSettings.Controls.Add(this.trbSpeed);
      this.grpMovementSettings.Controls.Add(this.label3);
      this.grpMovementSettings.Controls.Add(this.label6);
      this.grpMovementSettings.Controls.Add(this.label4);
      this.grpMovementSettings.Location = new System.Drawing.Point(9, 133);
      this.grpMovementSettings.Name = "grpMovementSettings";
      this.grpMovementSettings.Size = new System.Drawing.Size(397, 259);
      this.grpMovementSettings.TabIndex = 11;
      this.grpMovementSettings.TabStop = false;
      this.grpMovementSettings.Text = "Movement Settings";
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
      // lblPenClickSensitivity
      // 
      this.lblPenClickSensitivity.AutoSize = true;
      this.lblPenClickSensitivity.Location = new System.Drawing.Point(6, 17);
      this.lblPenClickSensitivity.Name = "lblPenClickSensitivity";
      this.lblPenClickSensitivity.Size = new System.Drawing.Size(102, 13);
      this.lblPenClickSensitivity.TabIndex = 1;
      this.lblPenClickSensitivity.Text = "Pen click sensitivity:";
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
      this.grpMovementType.Controls.Add(this.lblPPJoyInstructions);
      this.grpMovementType.Controls.Add(this.cboJoystickNumber);
      this.grpMovementType.Controls.Add(this.chkAllowFingerMovement);
      this.grpMovementType.Controls.Add(this.rdoAbsolute);
      this.grpMovementType.Controls.Add(this.rdoRelative);
      this.grpMovementType.Location = new System.Drawing.Point(9, 6);
      this.grpMovementType.Name = "grpMovementType";
      this.grpMovementType.Size = new System.Drawing.Size(397, 121);
      this.grpMovementType.TabIndex = 10;
      this.grpMovementType.TabStop = false;
      this.grpMovementType.Text = "Movement Type";
      // 
      // chkAllowFingerMovement
      // 
      this.chkAllowFingerMovement.AutoSize = true;
      this.chkAllowFingerMovement.Location = new System.Drawing.Point(9, 89);
      this.chkAllowFingerMovement.Name = "chkAllowFingerMovement";
      this.chkAllowFingerMovement.Size = new System.Drawing.Size(271, 17);
      this.chkAllowFingerMovement.TabIndex = 2;
      this.chkAllowFingerMovement.Text = "Detect movement with fingers (instead of just pen)";
      this.chkAllowFingerMovement.UseVisualStyleBackColor = true;
      // 
      // rdoAbsolute
      // 
      this.rdoAbsolute.AutoSize = true;
      this.rdoAbsolute.Location = new System.Drawing.Point(9, 66);
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
      this.rdoRelative.Location = new System.Drawing.Point(9, 43);
      this.rdoRelative.Name = "rdoRelative";
      this.rdoRelative.Size = new System.Drawing.Size(304, 17);
      this.rdoRelative.TabIndex = 0;
      this.rdoRelative.TabStop = true;
      this.rdoRelative.Text = "Relative (swipes move mouse cursor in indicated direction)";
      this.rdoRelative.UseVisualStyleBackColor = true;
      // 
      // tbpDisplays
      // 
      this.tbpDisplays.Controls.Add(this.flpDisplays);
      this.tbpDisplays.Location = new System.Drawing.Point(4, 22);
      this.tbpDisplays.Name = "tbpDisplays";
      this.tbpDisplays.Size = new System.Drawing.Size(441, 398);
      this.tbpDisplays.TabIndex = 3;
      this.tbpDisplays.Text = "Displays";
      this.tbpDisplays.UseVisualStyleBackColor = true;
      // 
      // flpDisplays
      // 
      this.flpDisplays.Controls.Add(this.chkAllowAllDisplays);
      this.flpDisplays.Controls.Add(this.pnlDisplays);
      this.flpDisplays.Controls.Add(this.lblInstructions);
      this.flpDisplays.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flpDisplays.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpDisplays.Location = new System.Drawing.Point(0, 0);
      this.flpDisplays.Name = "flpDisplays";
      this.flpDisplays.Size = new System.Drawing.Size(441, 398);
      this.flpDisplays.TabIndex = 3;
      // 
      // chkAllowAllDisplays
      // 
      this.chkAllowAllDisplays.AutoSize = true;
      this.chkAllowAllDisplays.Location = new System.Drawing.Point(3, 3);
      this.chkAllowAllDisplays.Name = "chkAllowAllDisplays";
      this.chkAllowAllDisplays.Size = new System.Drawing.Size(272, 17);
      this.chkAllowAllDisplays.TabIndex = 2;
      this.chkAllowAllDisplays.Text = "Allow absolute movement across all displays at once";
      this.chkAllowAllDisplays.UseVisualStyleBackColor = true;
      this.chkAllowAllDisplays.CheckedChanged += new System.EventHandler(this.chkAllowAllDisplays_CheckedChanged);
      // 
      // pnlDisplays
      // 
      this.pnlDisplays.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlDisplays.Location = new System.Drawing.Point(3, 26);
      this.pnlDisplays.Name = "pnlDisplays";
      this.pnlDisplays.Size = new System.Drawing.Size(272, 165);
      this.pnlDisplays.TabIndex = 0;
      this.pnlDisplays.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDisplays_Paint);
      this.pnlDisplays.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlDisplays_MouseClick);
      // 
      // lblInstructions
      // 
      this.lblInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblInstructions.AutoSize = true;
      this.lblInstructions.Location = new System.Drawing.Point(3, 194);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(269, 39);
      this.lblInstructions.TabIndex = 1;
      this.lblInstructions.Text = "Highlight which display the tablet will use.\r\nYou can assign a \"Switch Tablet Dis" +
    "play\" action to one\r\nof the tablet\'s buttons via the \"Buttons\" tab.";
      // 
      // tbpBounds
      // 
      this.tbpBounds.Controls.Add(this.flpBounds);
      this.tbpBounds.Location = new System.Drawing.Point(4, 22);
      this.tbpBounds.Name = "tbpBounds";
      this.tbpBounds.Size = new System.Drawing.Size(441, 398);
      this.tbpBounds.TabIndex = 4;
      this.tbpBounds.Text = "Cursor Bounds";
      this.tbpBounds.UseVisualStyleBackColor = true;
      // 
      // flpBounds
      // 
      this.flpBounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flpBounds.Controls.Add(this.chkRestrictToWindow);
      this.flpBounds.Controls.Add(this.chkMaintainAspectRatio);
      this.flpBounds.Controls.Add(this.flpCursorBounds);
      this.flpBounds.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpBounds.Location = new System.Drawing.Point(0, 0);
      this.flpBounds.Name = "flpBounds";
      this.flpBounds.Size = new System.Drawing.Size(441, 398);
      this.flpBounds.TabIndex = 4;
      // 
      // chkRestrictToWindow
      // 
      this.chkRestrictToWindow.AutoSize = true;
      this.chkRestrictToWindow.Location = new System.Drawing.Point(3, 3);
      this.chkRestrictToWindow.Name = "chkRestrictToWindow";
      this.chkRestrictToWindow.Size = new System.Drawing.Size(244, 17);
      this.chkRestrictToWindow.TabIndex = 3;
      this.chkRestrictToWindow.Text = "Restrict absolute movement to current window";
      this.chkRestrictToWindow.UseVisualStyleBackColor = true;
      // 
      // chkMaintainAspectRatio
      // 
      this.chkMaintainAspectRatio.AutoSize = true;
      this.chkMaintainAspectRatio.Location = new System.Drawing.Point(3, 26);
      this.chkMaintainAspectRatio.Name = "chkMaintainAspectRatio";
      this.chkMaintainAspectRatio.Size = new System.Drawing.Size(379, 17);
      this.chkMaintainAspectRatio.TabIndex = 2;
      this.chkMaintainAspectRatio.Text = "Maintain absolute aspect ratio (some areas of tablet surface will go unused)";
      this.chkMaintainAspectRatio.UseVisualStyleBackColor = true;
      this.chkMaintainAspectRatio.CheckedChanged += new System.EventHandler(this.chkMaintainAspectRatio_CheckedChanged);
      // 
      // flpCursorBounds
      // 
      this.flpCursorBounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flpCursorBounds.AutoScroll = true;
      this.flpCursorBounds.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.flpCursorBounds.BackColor = System.Drawing.SystemColors.Control;
      this.flpCursorBounds.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpCursorBounds.Location = new System.Drawing.Point(3, 49);
      this.flpCursorBounds.Name = "flpCursorBounds";
      this.flpCursorBounds.Size = new System.Drawing.Size(379, 327);
      this.flpCursorBounds.TabIndex = 4;
      this.flpCursorBounds.WrapContents = false;
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
      // cboJoystickNumber
      // 
      this.cboJoystickNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboJoystickNumber.FormattingEnabled = true;
      this.cboJoystickNumber.Items.AddRange(new object[] {
            "None",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
      this.cboJoystickNumber.Location = new System.Drawing.Point(182, 14);
      this.cboJoystickNumber.Name = "cboJoystickNumber";
      this.cboJoystickNumber.Size = new System.Drawing.Size(98, 21);
      this.cboJoystickNumber.TabIndex = 3;
      // 
      // lblPPJoyInstructions
      // 
      this.lblPPJoyInstructions.AutoSize = true;
      this.lblPPJoyInstructions.Location = new System.Drawing.Point(6, 17);
      this.lblPPJoyInstructions.Name = "lblPPJoyInstructions";
      this.lblPPJoyInstructions.Size = new System.Drawing.Size(163, 13);
      this.lblPPJoyInstructions.TabIndex = 14;
      this.lblPPJoyInstructions.Text = "PPJoy Virtual Joystick (optional):";
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
      this.MinimumSize = new System.Drawing.Size(469, 495);
      this.Name = "TabletOptions";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Tablet Options";
      this.tbcMain.ResumeLayout(false);
      this.tbpButtons.ResumeLayout(false);
      this.tbpMovement.ResumeLayout(false);
      this.grpMovementSettings.ResumeLayout(false);
      this.grpMovementSettings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trbPrecision)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trbPenClick)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
      this.grpMovementType.ResumeLayout(false);
      this.grpMovementType.PerformLayout();
      this.tbpDisplays.ResumeLayout(false);
      this.flpDisplays.ResumeLayout(false);
      this.flpDisplays.PerformLayout();
      this.tbpBounds.ResumeLayout(false);
      this.flpBounds.ResumeLayout(false);
      this.flpBounds.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tbcMain;
    private System.Windows.Forms.TabPage tbpButtons;
    private System.Windows.Forms.TabPage tbpMovement;
    private System.Windows.Forms.Label lblPenClickSensitivity;
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
    private System.Windows.Forms.CheckBox chkAllowFingerMovement;
    private System.Windows.Forms.TabPage tbpDisplays;
    private System.Windows.Forms.Panel pnlDisplays;
    private System.Windows.Forms.Label lblInstructions;
    private System.Windows.Forms.FlowLayoutPanel flpDisplays;
    private System.Windows.Forms.CheckBox chkAllowAllDisplays;
    private System.Windows.Forms.TabPage tbpBounds;
    private System.Windows.Forms.FlowLayoutPanel flpBounds;
    private System.Windows.Forms.CheckBox chkMaintainAspectRatio;
    private System.Windows.Forms.CheckBox chkRestrictToWindow;
    private System.Windows.Forms.FlowLayoutPanel flpCursorBounds;
    private System.Windows.Forms.Label lblPPJoyInstructions;
    private System.Windows.Forms.ComboBox cboJoystickNumber;
  }
}