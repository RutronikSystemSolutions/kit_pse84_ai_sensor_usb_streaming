namespace ov7675
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ov7675PictureBox = new PictureBox();
            rawRadarSignalsView = new Views.RawRadarSignalsView();
            comPortLabel = new Label();
            comPortComboBox = new ComboBox();
            connectButton = new Button();
            disconnectButton = new Button();
            statusTextBox = new TextBox();
            dataLoggerTabPage = new TabPage();
            storedSizeTextBox = new TextBox();
            storedSizeLabel = new Label();
            dataLoggerStopButton = new Button();
            dataLoggerStartButton = new Button();
            dataLoggerOpenPathButton = new Button();
            dataLoggerPathTextBox = new TextBox();
            label1 = new Label();
            dataLoggerTabControl = new TabControl();
            mainMenuStrip = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            flipVerticalyToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)ov7675PictureBox).BeginInit();
            dataLoggerTabPage.SuspendLayout();
            dataLoggerTabControl.SuspendLayout();
            mainMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // ov7675PictureBox
            // 
            ov7675PictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            ov7675PictureBox.BorderStyle = BorderStyle.FixedSingle;
            ov7675PictureBox.Location = new Point(12, 65);
            ov7675PictureBox.Name = "ov7675PictureBox";
            ov7675PictureBox.Size = new Size(320, 492);
            ov7675PictureBox.TabIndex = 2;
            ov7675PictureBox.TabStop = false;
            // 
            // rawRadarSignalsView
            // 
            rawRadarSignalsView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rawRadarSignalsView.BorderStyle = BorderStyle.FixedSingle;
            rawRadarSignalsView.Location = new Point(338, 65);
            rawRadarSignalsView.Name = "rawRadarSignalsView";
            rawRadarSignalsView.Size = new Size(680, 492);
            rawRadarSignalsView.TabIndex = 4;
            // 
            // comPortLabel
            // 
            comPortLabel.AutoSize = true;
            comPortLabel.Location = new Point(12, 34);
            comPortLabel.Name = "comPortLabel";
            comPortLabel.Size = new Size(77, 20);
            comPortLabel.TabIndex = 5;
            comPortLabel.Text = "COM port:";
            // 
            // comPortComboBox
            // 
            comPortComboBox.FormattingEnabled = true;
            comPortComboBox.Location = new Point(95, 31);
            comPortComboBox.Name = "comPortComboBox";
            comPortComboBox.Size = new Size(151, 28);
            comPortComboBox.TabIndex = 6;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(252, 30);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(94, 29);
            connectButton.TabIndex = 7;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // disconnectButton
            // 
            disconnectButton.Enabled = false;
            disconnectButton.Location = new Point(352, 30);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(94, 29);
            disconnectButton.TabIndex = 8;
            disconnectButton.Text = "Disconnect";
            disconnectButton.UseVisualStyleBackColor = true;
            disconnectButton.Click += disconnectButton_Click;
            // 
            // statusTextBox
            // 
            statusTextBox.Location = new Point(452, 31);
            statusTextBox.Name = "statusTextBox";
            statusTextBox.ReadOnly = true;
            statusTextBox.Size = new Size(125, 27);
            statusTextBox.TabIndex = 9;
            // 
            // dataLoggerTabPage
            // 
            dataLoggerTabPage.Controls.Add(storedSizeTextBox);
            dataLoggerTabPage.Controls.Add(storedSizeLabel);
            dataLoggerTabPage.Controls.Add(dataLoggerStopButton);
            dataLoggerTabPage.Controls.Add(dataLoggerStartButton);
            dataLoggerTabPage.Controls.Add(dataLoggerOpenPathButton);
            dataLoggerTabPage.Controls.Add(dataLoggerPathTextBox);
            dataLoggerTabPage.Controls.Add(label1);
            dataLoggerTabPage.Location = new Point(4, 29);
            dataLoggerTabPage.Name = "dataLoggerTabPage";
            dataLoggerTabPage.Padding = new Padding(3);
            dataLoggerTabPage.Size = new Size(994, 122);
            dataLoggerTabPage.TabIndex = 0;
            dataLoggerTabPage.Text = "Data Logger";
            dataLoggerTabPage.UseVisualStyleBackColor = true;
            // 
            // storedSizeTextBox
            // 
            storedSizeTextBox.Location = new Point(383, 64);
            storedSizeTextBox.Name = "storedSizeTextBox";
            storedSizeTextBox.ReadOnly = true;
            storedSizeTextBox.Size = new Size(125, 27);
            storedSizeTextBox.TabIndex = 6;
            // 
            // storedSizeLabel
            // 
            storedSizeLabel.AutoSize = true;
            storedSizeLabel.Location = new Point(280, 67);
            storedSizeLabel.Name = "storedSizeLabel";
            storedSizeLabel.Size = new Size(97, 20);
            storedSizeLabel.TabIndex = 5;
            storedSizeLabel.Text = "Stored so far:";
            // 
            // dataLoggerStopButton
            // 
            dataLoggerStopButton.Enabled = false;
            dataLoggerStopButton.Location = new Point(111, 63);
            dataLoggerStopButton.Name = "dataLoggerStopButton";
            dataLoggerStopButton.Size = new Size(94, 29);
            dataLoggerStopButton.TabIndex = 4;
            dataLoggerStopButton.Text = "Stop";
            dataLoggerStopButton.UseVisualStyleBackColor = true;
            dataLoggerStopButton.Click += dataLoggerStopButton_Click;
            // 
            // dataLoggerStartButton
            // 
            dataLoggerStartButton.Enabled = false;
            dataLoggerStartButton.Location = new Point(11, 63);
            dataLoggerStartButton.Name = "dataLoggerStartButton";
            dataLoggerStartButton.Size = new Size(94, 29);
            dataLoggerStartButton.TabIndex = 3;
            dataLoggerStartButton.Text = "Start";
            dataLoggerStartButton.UseVisualStyleBackColor = true;
            dataLoggerStartButton.Click += dataLoggerStartButton_Click;
            // 
            // dataLoggerOpenPathButton
            // 
            dataLoggerOpenPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataLoggerOpenPathButton.Location = new Point(923, 14);
            dataLoggerOpenPathButton.Name = "dataLoggerOpenPathButton";
            dataLoggerOpenPathButton.Size = new Size(65, 29);
            dataLoggerOpenPathButton.TabIndex = 2;
            dataLoggerOpenPathButton.Text = "...";
            dataLoggerOpenPathButton.UseVisualStyleBackColor = true;
            dataLoggerOpenPathButton.Click += dataLoggerOpenPathButton_Click;
            // 
            // dataLoggerPathTextBox
            // 
            dataLoggerPathTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataLoggerPathTextBox.Location = new Point(107, 15);
            dataLoggerPathTextBox.Name = "dataLoggerPathTextBox";
            dataLoggerPathTextBox.ReadOnly = true;
            dataLoggerPathTextBox.Size = new Size(810, 27);
            dataLoggerPathTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 18);
            label1.Name = "label1";
            label1.Size = new Size(90, 20);
            label1.TabIndex = 0;
            label1.Text = "Store to file:";
            // 
            // dataLoggerTabControl
            // 
            dataLoggerTabControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataLoggerTabControl.Controls.Add(dataLoggerTabPage);
            dataLoggerTabControl.Location = new Point(12, 563);
            dataLoggerTabControl.Name = "dataLoggerTabControl";
            dataLoggerTabControl.SelectedIndex = 0;
            dataLoggerTabControl.Size = new Size(1002, 155);
            dataLoggerTabControl.TabIndex = 10;
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.BackColor = Color.FromArgb(255, 128, 255);
            mainMenuStrip.ImageScalingSize = new Size(20, 20);
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            mainMenuStrip.Location = new Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Size = new Size(1026, 28);
            mainMenuStrip.TabIndex = 11;
            mainMenuStrip.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { flipVerticalyToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 24);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // flipVerticalyToolStripMenuItem
            // 
            flipVerticalyToolStripMenuItem.Name = "flipVerticalyToolStripMenuItem";
            flipVerticalyToolStripMenuItem.Size = new Size(224, 26);
            flipVerticalyToolStripMenuItem.Text = "Flip vertically";
            flipVerticalyToolStripMenuItem.Click += flipVerticalyToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1026, 730);
            Controls.Add(dataLoggerTabControl);
            Controls.Add(statusTextBox);
            Controls.Add(disconnectButton);
            Controls.Add(connectButton);
            Controls.Add(comPortComboBox);
            Controls.Add(comPortLabel);
            Controls.Add(rawRadarSignalsView);
            Controls.Add(ov7675PictureBox);
            Controls.Add(mainMenuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mainMenuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Rutronik - KIT_PSE84_AI streaming";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)ov7675PictureBox).EndInit();
            dataLoggerTabPage.ResumeLayout(false);
            dataLoggerTabPage.PerformLayout();
            dataLoggerTabControl.ResumeLayout(false);
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox ov7675PictureBox;
        private Views.RawRadarSignalsView rawRadarSignalsView;
        private Label comPortLabel;
        private ComboBox comPortComboBox;
        private Button connectButton;
        private Button disconnectButton;
        private TextBox statusTextBox;
        private TabPage dataLoggerTabPage;
        private TabControl dataLoggerTabControl;
        private Button dataLoggerOpenPathButton;
        private TextBox dataLoggerPathTextBox;
        private Label label1;
        private Button dataLoggerStopButton;
        private Button dataLoggerStartButton;
        private Label storedSizeLabel;
        private TextBox storedSizeTextBox;
        private MenuStrip mainMenuStrip;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem flipVerticalyToolStripMenuItem;
    }
}
