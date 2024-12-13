using System;
using System.Drawing;
using System.Windows.Forms;
using NeuroSDK;
namespace ah_ha_moment
{
    partial class GamingScreen
    {
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.TextBox answerBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Panel questionPanel;
        private void InitializeComponent()
        {
            timerLabel = new Label();
            questionPanel = new Panel();
            hintLabel = new Label();
            questionLabel = new Label();
            answerBox = new TextBox();
            submitButton = new Button();
            mainLayout = new TableLayoutPanel();
            questionPanel.SuspendLayout();
            mainLayout.SuspendLayout();
            SuspendLayout();
            // 
            // timerLabel
            // 
            timerLabel.Dock = DockStyle.Fill;
            timerLabel.Font = new Font("Arial", 16F, FontStyle.Bold);
            timerLabel.ForeColor = Color.Black;
            timerLabel.Location = new Point(3, 0);
            timerLabel.Name = "timerLabel";
            timerLabel.Size = new Size(794, 120);
            timerLabel.TabIndex = 0;
            timerLabel.Text = "00:53.800";
            timerLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // questionPanel
            // 
            questionPanel.BackColor = Color.FromArgb(245, 245, 245);
            questionPanel.Controls.Add(hintLabel);
            questionPanel.Controls.Add(questionLabel);
            questionPanel.Dock = DockStyle.Fill;
            questionPanel.Location = new Point(3, 123);
            questionPanel.Name = "questionPanel";
            questionPanel.Padding = new Padding(20);
            questionPanel.Size = new Size(794, 234);
            questionPanel.TabIndex = 1;
            // 
            // hintLabel
            // 
            hintLabel.Dock = DockStyle.Bottom;
            hintLabel.Font = new Font("Arial", 12F);
            hintLabel.ForeColor = Color.Red;
            hintLabel.Location = new Point(20, 191);
            hintLabel.Name = "hintLabel";
            hintLabel.Size = new Size(754, 23);
            hintLabel.TabIndex = 0;
            hintLabel.Text = "Think of an equation where the number is multiplied by 3 to get 24.";
            hintLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // questionLabel
            // 
            questionLabel.Dock = DockStyle.Top;
            questionLabel.Font = new Font("Arial", 18F);
            questionLabel.ForeColor = Color.Black;
            questionLabel.Location = new Point(20, 20);
            questionLabel.Name = "questionLabel";
            questionLabel.Size = new Size(754, 23);
            questionLabel.TabIndex = 1;
            questionLabel.Text = "If you multiply me by 3, I become 24. What number am I?";
            questionLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // answerBox
            // 
            answerBox.Dock = DockStyle.Fill;
            answerBox.Font = new Font("Arial", 14F);
            answerBox.Location = new Point(20, 365);
            answerBox.Margin = new Padding(20, 5, 20, 5);
            answerBox.Name = "answerBox";
            answerBox.Size = new Size(760, 29);
            answerBox.TabIndex = 2;
            // 
            // submitButton
            // 
            submitButton.Anchor = AnchorStyles.None;
            submitButton.BackColor = Color.FromArgb(220, 220, 220);
            submitButton.Font = new Font("Arial", 14F, FontStyle.Bold);
            submitButton.ForeColor = Color.Black;
            submitButton.Location = new Point(325, 515);
            submitButton.Margin = new Padding(20, 5, 20, 5);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(150, 50);
            submitButton.TabIndex = 3;
            submitButton.Text = "Submit";
            submitButton.UseVisualStyleBackColor = false;
            submitButton.Click += SubmitButton_Click;
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            mainLayout.Controls.Add(timerLabel, 0, 0);
            mainLayout.Controls.Add(questionPanel, 0, 1);
            mainLayout.Controls.Add(answerBox, 0, 2);
            mainLayout.Controls.Add(submitButton, 0, 3);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 4;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainLayout.Size = new Size(800, 600);
            mainLayout.TabIndex = 0;
            // 
            // GamingScreen
            // 
            BackColor = Color.White;
            ClientSize = new Size(800, 600);
            Controls.Add(mainLayout);
            Name = "GamingScreen";
            Text = "Gaming Screen";
            WindowState = FormWindowState.Maximized;
            questionPanel.ResumeLayout(false);
            mainLayout.ResumeLayout(false);
            mainLayout.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel mainLayout;
    }
}