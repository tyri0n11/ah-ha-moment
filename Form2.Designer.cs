using System;
using System.Drawing;
using System.Windows.Forms;

namespace AhhaMoment
{
    partial class AhhaMoment2
    {
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.TextBox answerBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Panel questionPanel;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AhhaMoment2));
            this.timerLabel = new System.Windows.Forms.Label();
            this.questionPanel = new System.Windows.Forms.Panel();
            this.hintLabel = new System.Windows.Forms.Label();
            this.questionLabel = new System.Windows.Forms.Label();
            this.answerBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.questionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerLabel
            // 
            resources.ApplyResources(this.timerLabel, "timerLabel");
            this.timerLabel.ForeColor = System.Drawing.Color.Black;
            this.timerLabel.Name = "timerLabel";
            // 
            // questionPanel
            // 
            this.questionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.questionPanel.Controls.Add(this.hintLabel);
            this.questionPanel.Controls.Add(this.questionLabel);
            resources.ApplyResources(this.questionPanel, "questionPanel");
            this.questionPanel.Name = "questionPanel";
            // 
            // hintLabel
            // 
            resources.ApplyResources(this.hintLabel, "hintLabel");
            this.hintLabel.ForeColor = System.Drawing.Color.Red;
            this.hintLabel.Name = "hintLabel";
            // 
            // questionLabel
            // 
            resources.ApplyResources(this.questionLabel, "questionLabel");
            this.questionLabel.ForeColor = System.Drawing.Color.Black;
            this.questionLabel.Name = "questionLabel";
            // 
            // answerBox
            // 
            resources.ApplyResources(this.answerBox, "answerBox");
            this.answerBox.Name = "answerBox";
            // 
            // submitButton
            // 
            this.submitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.submitButton, "submitButton");
            this.submitButton.ForeColor = System.Drawing.Color.Black;
            this.submitButton.Name = "submitButton";
            this.submitButton.UseVisualStyleBackColor = false;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // AhhaMoment2
            // 
            this.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.answerBox);
            this.Controls.Add(this.questionPanel);
            this.Controls.Add(this.timerLabel);
            this.Name = "AhhaMoment2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.questionPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}