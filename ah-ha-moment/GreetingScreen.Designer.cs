namespace ah_ha_moment
{
    partial class GreetingScreen
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
            components = new System.ComponentModel.Container();
            btnStart = new Button();
            countStartTimer = new System.Windows.Forms.Timer(components);
            timeRemainStart = new TextBox();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(593, 408);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(368, 324);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += button1_Click;
            // 
            // countStartTimer
            // 
            countStartTimer.Interval = 1000;
            countStartTimer.Tick += timer2_Tick;
            // 
            // timeRemainStart
            // 
            timeRemainStart.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            timeRemainStart.Location = new Point(1332, 173);
            timeRemainStart.Name = "timeRemainStart";
            timeRemainStart.Size = new Size(178, 38);
            timeRemainStart.TabIndex = 1;
            // 
            // GreetingScreen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1577, 1061);
            Controls.Add(timeRemainStart);
            Controls.Add(btnStart);
            Name = "GreetingScreen";
            Text = "AhaMoment";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer countStartTimer;
        private System.Windows.Forms.TextBox timeRemainStart;
    }
}
