namespace Scanquire.Public.UserControls
{
    partial class ProgressDialog
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
            this.CaptionLabel = new System.Windows.Forms.Label();
            this.ProgressMonitor = new Scanquire.Public.UserControls.ProgressMonitor();
            this.SuspendLayout();
            // 
            // CaptionLabel
            // 
            this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.CaptionLabel.Margin = new System.Windows.Forms.Padding(5);
            this.CaptionLabel.Name = "CaptionLabel";
            this.CaptionLabel.Size = new System.Drawing.Size(284, 13);
            this.CaptionLabel.TabIndex = 1;
            this.CaptionLabel.Text = "Please Wait ...";
            // 
            // ProgressMonitor
            // 
            this.ProgressMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressMonitor.Location = new System.Drawing.Point(0, 13);
            this.ProgressMonitor.MinimumSize = new System.Drawing.Size(250, 25);
            this.ProgressMonitor.Name = "ProgressMonitor";
            this.ProgressMonitor.Size = new System.Drawing.Size(284, 61);
            this.ProgressMonitor.TabIndex = 0;
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 74);
            this.Controls.Add(this.ProgressMonitor);
            this.Controls.Add(this.CaptionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProgressDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing";
            this.ResumeLayout(false);

        }

        #endregion

        public ProgressMonitor ProgressMonitor;
        public System.Windows.Forms.Label CaptionLabel;

    }
}