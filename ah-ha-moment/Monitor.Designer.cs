using System.Windows.Forms;

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
        private Button btnRecord;
        private TableLayoutPanel tableLayoutPanel1;

        private void InitializeComponent()
        {
            btnStartScan = new Button();
            btnStopScan = new Button();
            lstDevices = new ListBox();
            lblStatus = new Label();
            txtDeviceInfo = new TextBox();
            txtBattery = new TextBox();
            txtSignal = new TextBox();
            btnRecord = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            txtResult = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartScan
            // 
            btnStartScan.BackColor = Color.FromArgb(50, 100, 180);
            btnStartScan.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 130, 210);
            btnStartScan.FlatStyle = FlatStyle.Flat;
            btnStartScan.Font = new Font("Segoe UI", 10F);
            btnStartScan.ForeColor = Color.White;
            btnStartScan.Location = new Point(15, 15);
            btnStartScan.Margin = new Padding(5);
            btnStartScan.Name = "btnStartScan";
            btnStartScan.Size = new Size(120, 40);
            btnStartScan.TabIndex = 1;
            btnStartScan.Text = "Start Scan";
            btnStartScan.UseVisualStyleBackColor = false;
            btnStartScan.Click += btnStartScan_Click;
            // 
            // btnStopScan
            // 
            btnStopScan.BackColor = Color.FromArgb(50, 100, 180);
            btnStopScan.Enabled = false;
            btnStopScan.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 130, 210);
            btnStopScan.FlatStyle = FlatStyle.Flat;
            btnStopScan.Font = new Font("Segoe UI", 10F);
            btnStopScan.ForeColor = Color.White;
            btnStopScan.Location = new Point(496, 15);
            btnStopScan.Margin = new Padding(5);
            btnStopScan.Name = "btnStopScan";
            btnStopScan.Size = new Size(120, 40);
            btnStopScan.TabIndex = 2;
            btnStopScan.Text = "Stop Scan";
            btnStopScan.UseVisualStyleBackColor = false;
            btnStopScan.Click += btnStopScan_Click;
            // 
            // lstDevices
            // 
            lstDevices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lstDevices.BackColor = Color.White;
            lstDevices.Font = new Font("Segoe UI", 10F);
            lstDevices.FormattingEnabled = true;
            lstDevices.ItemHeight = 17;
            lstDevices.Location = new Point(13, 73);
            lstDevices.Name = "lstDevices";
            lstDevices.Size = new Size(475, 174);
            lstDevices.TabIndex = 3;
            lstDevices.DoubleClick += lstDevices_DoubleClick;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.BackColor = Color.FromArgb(180, 200, 230);
            lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Black;
            lblStatus.Location = new Point(13, 300);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(5);
            lblStatus.Size = new Size(475, 40);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status: Ready";
            // 
            // txtDeviceInfo
            // 
            txtDeviceInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDeviceInfo.BackColor = Color.White;
            txtDeviceInfo.Font = new Font("Segoe UI", 10F);
            txtDeviceInfo.Location = new Point(13, 343);
            txtDeviceInfo.Multiline = true;
            txtDeviceInfo.Name = "txtDeviceInfo";
            txtDeviceInfo.ReadOnly = true;
            txtDeviceInfo.ScrollBars = ScrollBars.Vertical;
            txtDeviceInfo.Size = new Size(475, 150);
            txtDeviceInfo.TabIndex = 5;
            // 
            // txtBattery
            // 
            txtBattery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBattery.BackColor = Color.White;
            txtBattery.Font = new Font("Segoe UI", 10F);
            txtBattery.Location = new Point(13, 573);
            txtBattery.Name = "txtBattery";
            txtBattery.ReadOnly = true;
            txtBattery.Size = new Size(475, 25);
            txtBattery.TabIndex = 6;
            // 
            // txtSignal
            // 
            txtSignal.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSignal.BackColor = Color.White;
            txtSignal.Font = new Font("Segoe UI", 10F);
            txtSignal.Location = new Point(494, 73);
            txtSignal.Multiline = true;
            txtSignal.Name = "txtSignal";
            txtSignal.ReadOnly = true;
            txtSignal.ScrollBars = ScrollBars.Vertical;
            txtSignal.Size = new Size(888, 224);
            txtSignal.TabIndex = 8;
            // 
            // btnRecord
            // 
            btnRecord.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRecord.BackColor = Color.FromArgb(50, 100, 180);
            btnRecord.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 130, 210);
            btnRecord.FlatStyle = FlatStyle.Flat;
            btnRecord.Font = new Font("Segoe UI", 10F);
            btnRecord.ForeColor = Color.White;
            btnRecord.Location = new Point(1245, 640);
            btnRecord.Margin = new Padding(5, 5, 20, 5);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(120, 40);
            btnRecord.TabIndex = 9;
            btnRecord.Text = "SHOW GAME";
            btnRecord.UseVisualStyleBackColor = false;
            btnRecord.Click += btnShowGame_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel1.Controls.Add(btnStartScan, 0, 0);
            tableLayoutPanel1.Controls.Add(btnStopScan, 1, 0);
            tableLayoutPanel1.Controls.Add(lstDevices, 0, 1);
            tableLayoutPanel1.Controls.Add(lblStatus, 0, 2);
            tableLayoutPanel1.Controls.Add(txtDeviceInfo, 0, 3);
            tableLayoutPanel1.Controls.Add(txtBattery, 0, 4);
            tableLayoutPanel1.Controls.Add(txtSignal, 1, 1);
            tableLayoutPanel1.Controls.Add(txtResult, 1, 3);
            tableLayoutPanel1.Controls.Add(btnRecord, 1, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel1.Size = new Size(1395, 811);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // txtResult
            // 
            txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtResult.BackColor = Color.White;
            txtResult.Font = new Font("Segoe UI", 10F);
            txtResult.Location = new Point(494, 343);
            txtResult.Multiline = true;
            txtResult.Name = "txtResult";
            txtResult.ReadOnly = true;
            txtResult.ScrollBars = ScrollBars.Vertical;
            txtResult.Size = new Size(888, 224);
            txtResult.TabIndex = 11;
            // 
            // Monitor
            // 
            BackColor = Color.FromArgb(230, 240, 250);
            ClientSize = new Size(1395, 811);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Segoe UI", 10F);
            Name = "Monitor";
            Text = "Brainbit Monitor";
            FormClosed += MainForm_FormClosed;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        private TextBox txtResult;
    }
}