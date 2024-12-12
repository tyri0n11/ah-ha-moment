namespace ah_ha_moment
{
    partial class Monitor
    {
        private Button btnStartScan;
        private Button btnStopScan;
        private ListBox lstDevices;
        private Label lblStatus;
        private TextBox txtDeviceInfo;
        private TextBox txtBattery;
        private TextBox txtSignal;
        private TextBox txtLog;
        private Button btnRecord;
        private Button btnStopRecord;

        private void InitializeComponent()
        {
            btnStartScan = new Button();
            btnStopScan = new Button();
            lstDevices = new ListBox();
            lblStatus = new Label();
            txtDeviceInfo = new TextBox();
            txtBattery = new TextBox();
            txtSignal = new TextBox();
            txtLog = new TextBox();
            btnRecord = new Button();
            btnStopRecord = new Button();
            SuspendLayout();
            // 
            // btnStartScan
            // 
            btnStartScan.Location = new Point(12, 116);
            btnStartScan.Name = "btnStartScan";
            btnStartScan.Size = new Size(100, 30);
            btnStartScan.TabIndex = 1;
            btnStartScan.Text = "Start Scan";
            btnStartScan.Click += btnStartScan_Click;
            // 
            // btnStopScan
            // 
            btnStopScan.Enabled = false;
            btnStopScan.Location = new Point(122, 116);
            btnStopScan.Name = "btnStopScan";
            btnStopScan.Size = new Size(100, 30);
            btnStopScan.TabIndex = 2;
            btnStopScan.Text = "Stop Scan";
            btnStopScan.Click += btnStopScan_Click;
            // 
            // lstDevices
            // 
            lstDevices.Location = new Point(12, 189);
            lstDevices.Name = "lstDevices";
            lstDevices.Size = new Size(357, 84);
            lstDevices.TabIndex = 3;
            lstDevices.DoubleClick += lstDevices_DoubleClick;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 156);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(272, 30);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status: Ready";
            // 
            // txtDeviceInfo
            // 
            txtDeviceInfo.Location = new Point(12, 279);
            txtDeviceInfo.Multiline = true;
            txtDeviceInfo.Name = "txtDeviceInfo";
            txtDeviceInfo.ReadOnly = true;
            txtDeviceInfo.ScrollBars = ScrollBars.Vertical;
            txtDeviceInfo.Size = new Size(357, 100);
            txtDeviceInfo.TabIndex = 5;
            // 
            // txtBattery
            // 
            txtBattery.Location = new Point(14, 385);
            txtBattery.Name = "txtBattery";
            txtBattery.ReadOnly = true;
            txtBattery.Size = new Size(355, 27);
            txtBattery.TabIndex = 6;
            // 
            // txtSignal
            // 
            txtSignal.Location = new Point(401, 279);
            txtSignal.Multiline = true;
            txtSignal.Name = "txtSignal";
            txtSignal.ReadOnly = true;
            txtSignal.ScrollBars = ScrollBars.Vertical;
            txtSignal.Size = new Size(692, 270);
            txtSignal.TabIndex = 8;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(401, 50);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(692, 223);
            txtLog.TabIndex = 0;
            // 
            // btnRecord
            // 
            btnRecord.Location = new Point(605, 555);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(100, 30);
            btnRecord.TabIndex = 9;
            btnRecord.Text = "Record";
            btnRecord.UseVisualStyleBackColor = true;
            btnRecord.Click += btnRecord_Click;
            // 
            // btnStopRecord
            // 
            btnStopRecord.Location = new Point(840, 555);
            btnStopRecord.Name = "btnStopRecord";
            btnStopRecord.Size = new Size(100, 30);
            btnStopRecord.TabIndex = 10;
            btnStopRecord.Text = "Stop record";
            btnStopRecord.UseVisualStyleBackColor = true;
            btnStopRecord.Click += btnStopRecord_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(1106, 595);
            Controls.Add(btnStopRecord);
            Controls.Add(btnRecord);
            Controls.Add(btnStartScan);
            Controls.Add(btnStopScan);
            Controls.Add(lstDevices);
            Controls.Add(lblStatus);
            Controls.Add(txtDeviceInfo);
            Controls.Add(txtBattery);
            Controls.Add(txtSignal);
            Controls.Add(txtLog);
            Name = "MainForm";
            Text = "Brainbit Monitor";
            FormClosed += MainForm_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}