namespace AhhaMoment
{
    partial class AhhaMoment1
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
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.countStartTimer = new System.Windows.Forms.Timer(this.components);
            this.timeRemainStart = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(667, 408);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(414, 324);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // countStartTimer
            // 
            this.countStartTimer.Interval = 1000;
            this.countStartTimer.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timeRemainStart
            // 
            this.timeRemainStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeRemainStart.Location = new System.Drawing.Point(1498, 173);
            this.timeRemainStart.Name = "timeRemainStart";
            this.timeRemainStart.Size = new System.Drawing.Size(200, 53);
            this.timeRemainStart.TabIndex = 1;
            this.timeRemainStart.TextChanged += new System.EventHandler(this.timeRemainStart_TextChanged);
            // 
            // AhhaMoment1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1774, 1162);
            this.Controls.Add(this.timeRemainStart);
            this.Controls.Add(this.btnStart);
            this.Name = "AhhaMoment1";
            this.Text = "AhaMoment";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer countStartTimer;
        private System.Windows.Forms.TextBox timeRemainStart;
    }
}

