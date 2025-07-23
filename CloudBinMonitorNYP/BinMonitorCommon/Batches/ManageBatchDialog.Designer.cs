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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageBatchDialog));
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.UserAuthenticationControl = new BinMonitor.Common.UserAuthenticationControl();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.BinLookupControl = new BinMonitor.Common.BinLookupControl();
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
            this.grpCredentials.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpCredentials.Size = new System.Drawing.Size(845, 54);
            this.grpCredentials.TabIndex = 2;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Credentials";
            // 
            // UserAuthenticationControl
            // 
            this.UserAuthenticationControl.AdminOverrideVisible = false;
            this.UserAuthenticationControl.AutoSize = true;
            this.UserAuthenticationControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.UserAuthenticationControl.Location = new System.Drawing.Point(12, 18);
            this.UserAuthenticationControl.Margin = new System.Windows.Forms.Padding(0);
            this.UserAuthenticationControl.Name = "UserAuthenticationControl";
            this.UserAuthenticationControl.Size = new System.Drawing.Size(377, 35);
            this.UserAuthenticationControl.TabIndex = 0;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // BinLookupControl
            // 
            this.BinLookupControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.BinLookupControl.Location = new System.Drawing.Point(0, 54);
            this.BinLookupControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.BinLookupControl.Name = "BinLookupControl";
            this.BinLookupControl.Size = new System.Drawing.Size(845, 628);
            this.BinLookupControl.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(363, 689);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 92);
            this.button1.TabIndex = 5;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ManageBatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 804);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BinLookupControl);
            this.Controls.Add(this.grpCredentials);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        public BinLookupControl BinLookupControl;
        private System.Windows.Forms.Button button1;
    }
}