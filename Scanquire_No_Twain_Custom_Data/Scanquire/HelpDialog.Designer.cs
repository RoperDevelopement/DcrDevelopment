namespace Scanquire
{
    partial class HelpDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpDialog));
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.NavigateHomeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WebBrowser
            // 
            this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowser.Location = new System.Drawing.Point(0, 23);
            this.WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.ScriptErrorsSuppressed = true;
            this.WebBrowser.Size = new System.Drawing.Size(624, 419);
            this.WebBrowser.TabIndex = 0;
            // 
            // NavigateHomeButton
            // 
            this.NavigateHomeButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.NavigateHomeButton.Location = new System.Drawing.Point(0, 0);
            this.NavigateHomeButton.Name = "NavigateHomeButton";
            this.NavigateHomeButton.Size = new System.Drawing.Size(624, 23);
            this.NavigateHomeButton.TabIndex = 1;
            this.NavigateHomeButton.Text = "Home";
            this.NavigateHomeButton.UseVisualStyleBackColor = true;
            this.NavigateHomeButton.Click += new System.EventHandler(this.NavigateHomeButton_Click);
            // 
            // HelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.WebBrowser);
            this.Controls.Add(this.NavigateHomeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelpDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scanquire Help";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WebBrowser;
        private System.Windows.Forms.Button NavigateHomeButton;
    }
}