namespace BinMonitor.Common.Batches
{
    partial class BatchQuickStepDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchQuickStepDialog));
            this.pnlDialogControls = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblBinId = new System.Windows.Forms.Label();
            this.txtBinId = new System.Windows.Forms.TextBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.UserAuthenticationControl = new BinMonitor.Common.UserAuthenticationControl();
            this.pnlDialogControls.SuspendLayout();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogControls
            // 
            this.pnlDialogControls.Controls.Add(this.btnCancel);
            this.pnlDialogControls.Controls.Add(this.btnOk);
            this.pnlDialogControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDialogControls.Location = new System.Drawing.Point(0, 98);
            this.pnlDialogControls.Name = "pnlDialogControls";
            this.pnlDialogControls.Size = new System.Drawing.Size(319, 78);
            this.pnlDialogControls.TabIndex = 50;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(157, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 67);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(76, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 67);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblBinId
            // 
            this.lblBinId.AutoSize = true;
            this.lblBinId.Location = new System.Drawing.Point(89, 63);
            this.lblBinId.Name = "lblBinId";
            this.lblBinId.Size = new System.Drawing.Size(34, 13);
            this.lblBinId.TabIndex = 51;
            this.lblBinId.Text = "Bin Id";
            // 
            // txtBinId
            // 
            this.txtBinId.Location = new System.Drawing.Point(129, 60);
            this.txtBinId.Name = "txtBinId";
            this.txtBinId.Size = new System.Drawing.Size(100, 20);
            this.txtBinId.TabIndex = 52;
            this.txtBinId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBinId_KeyPress);
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.txtBinId);
            this.pnlInput.Controls.Add(this.lblBinId);
            this.pnlInput.Controls.Add(this.UserAuthenticationControl);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(319, 100);
            this.pnlInput.TabIndex = 53;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // UserAuthenticationControl
            // 
            this.UserAuthenticationControl.AdminOverrideVisible = false;
            this.UserAuthenticationControl.AutoSize = true;
            this.UserAuthenticationControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.UserAuthenticationControl.Location = new System.Drawing.Point(14, 13);
            this.UserAuthenticationControl.Margin = new System.Windows.Forms.Padding(0);
            this.UserAuthenticationControl.Name = "UserAuthenticationControl";
            this.UserAuthenticationControl.Size = new System.Drawing.Size(290, 29);
            this.UserAuthenticationControl.TabIndex = 0;
            // 
            // BatchQuickStepDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 176);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.pnlDialogControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BatchQuickStepDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Quick Step";
            this.Shown += new System.EventHandler(this.BatchQuickStepDialog_Shown);
            this.pnlDialogControls.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UserAuthenticationControl UserAuthenticationControl;
        private System.Windows.Forms.Panel pnlDialogControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblBinId;
        private System.Windows.Forms.TextBox txtBinId;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}