namespace Scanquire.Public.UserControls
{
    partial class ProgressMonitor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgressBar = new EdocsUSA.Utilities.Controls.ProgressBarEx();
            this.CancelTaskButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.BarColor = System.Drawing.SystemColors.ControlDark;
            this.ProgressBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ProgressBar.Caption = "";
            this.ProgressBar.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressBar.Location = new System.Drawing.Point(0, 0);
            this.ProgressBar.MinimumSize = new System.Drawing.Size(100, 25);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(126, 25);
            this.ProgressBar.TabIndex = 0;
            this.ProgressBar.ToolTipText = "";
            this.ProgressBar.Value = 0;
            // 
            // CancelTaskButton
            // 
            this.CancelTaskButton.AutoSize = true;
            this.CancelTaskButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelTaskButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CancelTaskButton.Location = new System.Drawing.Point(126, 0);
            this.CancelTaskButton.Name = "CancelTaskButton";
            this.CancelTaskButton.Size = new System.Drawing.Size(24, 25);
            this.CancelTaskButton.TabIndex = 1;
            this.CancelTaskButton.Text = "X";
            this.CancelTaskButton.UseVisualStyleBackColor = true;
            
            // 
            // ProgressMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.CancelTaskButton);
            this.Name = "ProgressMonitor";
            this.Size = new System.Drawing.Size(150, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EdocsUSA.Utilities.Controls.ProgressBarEx ProgressBar;
        private System.Windows.Forms.Button CancelTaskButton;
    }
}
