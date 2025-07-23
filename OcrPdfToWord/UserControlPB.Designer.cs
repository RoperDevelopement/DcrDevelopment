namespace OcrPdfToWord
{
    partial class UserControlPB
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
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // proBar
            // 
            this.proBar.Location = new System.Drawing.Point(51, 38);
            this.proBar.Minimum = 10;
            this.proBar.Name = "proBar";
            this.proBar.Size = new System.Drawing.Size(245, 46);
            this.proBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.proBar.TabIndex = 0;
            this.proBar.Value = 10;
            // 
            // UserControlPB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.proBar);
            this.Name = "UserControlPB";
            this.Size = new System.Drawing.Size(343, 150);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ProgressBar proBar;
    }
}
