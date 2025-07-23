namespace BinMonitor.Common
{
    partial class CreateBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateBatchDialog));
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.UserAuthenticationControl = new BinMonitor.Common.UserAuthenticationControl();
            this.createBatchControl = new BinMonitor.Common.CreateBatchControl();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
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
            this.grpCredentials.Size = new System.Drawing.Size(673, 44);
            this.grpCredentials.TabIndex = 1;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Credentials";
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
            // createBatchControl
            // 
            this.createBatchControl.AdvancedOptionsVisible = false;
            this.createBatchControl.CredentialHost = null;
            this.createBatchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createBatchControl.Location = new System.Drawing.Point(0, 44);
            this.createBatchControl.Name = "createBatchControl";
            this.createBatchControl.Size = new System.Drawing.Size(673, 300);
            this.createBatchControl.TabIndex = 0;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // CreateBatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 344);
            this.Controls.Add(this.createBatchControl);
            this.Controls.Add(this.grpCredentials);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreateBatchDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Batch";
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CreateBatchControl createBatchControl;
        private System.Windows.Forms.GroupBox grpCredentials;
        private UserAuthenticationControl UserAuthenticationControl;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}