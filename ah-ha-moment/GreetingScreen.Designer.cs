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
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Microsoft Sans Serif", 40F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(599, 493);
            btnStart.Margin = new Padding(4);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(744, 228);
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
            timeRemainStart.Location = new Point(861, 221);
            timeRemainStart.Margin = new Padding(4);
            timeRemainStart.Name = "timeRemainStart";
            timeRemainStart.Size = new Size(222, 53);
            timeRemainStart.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 20F);
            textBox1.Location = new Point(369, 57);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(1264, 61);
            textBox1.TabIndex = 2;
            textBox1.Text = "WELCOME TO THE RESEARCH APP";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // GreetingScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1971, 1326);
            Controls.Add(textBox1);
            Controls.Add(timeRemainStart);
            Controls.Add(btnStart);
            Margin = new Padding(4);
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
        private TextBox textBox1;
    }
}
