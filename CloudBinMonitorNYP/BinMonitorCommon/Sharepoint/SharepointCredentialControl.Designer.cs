namespace BinMonitor.Common.Sharepoint
{
    partial class SharepointCredentialControl
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
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblDomain = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider();
            this.txtPassword = new BinMonitor.Common.ValidatingTextBox();
            this.txtUserName = new BinMonitor.Common.ValidatingTextBox();
            this.txtDomain = new BinMonitor.Common.ValidatingTextBox();
            this.txtHost = new BinMonitor.Common.ValidatingTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(16, 85);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Password";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(9, 60);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 13);
            this.lblUserName.TabIndex = 10;
            this.lblUserName.Text = "User Name";
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(26, 35);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(43, 13);
            this.lblDomain.TabIndex = 9;
            this.lblDomain.Text = "Domain";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(40, 10);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(29, 13);
            this.lblHost.TabIndex = 8;
            this.lblHost.Text = "Host";
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(75, 82);
            this.txtPassword.MaxValue = null;
            this.txtPassword.MinLength = 1;
            this.txtPassword.MinValue = null;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(143, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.ValidatingRegex = null;
            this.txtPassword.ValidatingTypeName = "";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(75, 57);
            this.txtUserName.MaxValue = null;
            this.txtUserName.MinLength = 1;
            this.txtUserName.MinValue = null;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(143, 20);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.ValidatingRegex = null;
            this.txtUserName.ValidatingTypeName = "";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(75, 32);
            this.txtDomain.MaxValue = null;
            this.txtDomain.MinLength = 0;
            this.txtDomain.MinValue = null;
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(143, 20);
            this.txtDomain.TabIndex = 1;
            this.txtDomain.ValidatingRegex = null;
            this.txtDomain.ValidatingTypeName = "";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(75, 7);
            this.txtHost.MaxValue = null;
            this.txtHost.MinLength = 1;
            this.txtHost.MinValue = null;
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(143, 20);
            this.txtHost.TabIndex = 0;
            this.txtHost.ValidatingRegex = null;
            this.txtHost.ValidatingTypeName = "";
            // 
            // SharepointCredentialControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblDomain);
            this.Controls.Add(this.lblHost);
            this.Name = "SharepointCredentialControl";
            this.Size = new System.Drawing.Size(235, 109);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private ValidatingTextBox txtPassword;
        private ValidatingTextBox txtUserName;
        private ValidatingTextBox txtDomain;
        private ValidatingTextBox txtHost;
    }
}
