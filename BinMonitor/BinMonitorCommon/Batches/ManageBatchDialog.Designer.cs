namespace BinMonitor.Common
{
    partial class ManageBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageBatchDialog));
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider();
            this.BinLookupControl = new BinMonitor.Common.BinLookupControl();
            this.UserAuthenticationControl = new BinMonitor.Common.UserAuthenticationControl();
            this.button1 = new System.Windows.Forms.Button();
            this.grpCredentials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCredentials
            // 
            this.grpCredentials.Controls.Add(this.UserAuthenticationControl);
            this.grpCredentials.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCredentials.Location = new System.Drawing.Point(0, 0);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(619, 44);
            this.grpCredentials.TabIndex = 2;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Credentials";
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // BinLookupControl
            // 
            this.BinLookupControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.BinLookupControl.Location = new System.Drawing.Point(0, 44);
            this.BinLookupControl.Name = "BinLookupControl";
            this.BinLookupControl.Size = new System.Drawing.Size(619, 510);
            this.BinLookupControl.TabIndex = 4;
            // 
            // UserAuthenticationControl
            // 
            this.UserAuthenticationControl.AdminOverrideVisible = false;
            this.UserAuthenticationControl.AutoSize = true;
            this.UserAuthenticationControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.UserAuthenticationControl.Location = new System.Drawing.Point(9, 15);
            this.UserAuthenticationControl.Margin = new System.Windows.Forms.Padding(0);
            this.UserAuthenticationControl.Name = "UserAuthenticationControl";
            this.UserAuthenticationControl.Size = new System.Drawing.Size(290, 29);
            this.UserAuthenticationControl.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(272, 560);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 75);
            this.button1.TabIndex = 5;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ManageBatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 653);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BinLookupControl);
            this.Controls.Add(this.grpCredentials);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManageBatchDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Batch";
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCredentials;
        private UserAuthenticationControl UserAuthenticationControl;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private BinLookupControl BinLookupControl;
        private System.Windows.Forms.Button button1;
    }
}