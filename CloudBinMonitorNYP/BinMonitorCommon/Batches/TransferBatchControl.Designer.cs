namespace BinMonitor.Common.Batches
{
    partial class TransferBatchControl
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
            this.grpBinLookup = new System.Windows.Forms.GroupBox();
            this.btnPerformTransfer = new System.Windows.Forms.Button();
            this.lblSpecimensToTransfer = new System.Windows.Forms.Label();
            this.lbSpecimensToTransfer = new System.Windows.Forms.ListBox();
            this.txtBatchId = new System.Windows.Forms.TextBox();
            this.lblOriginalBatchId = new System.Windows.Forms.Label();
            this.btnLookupBinById = new System.Windows.Forms.Button();
            this.lblLookupBinId = new System.Windows.Forms.Label();
            this.txtLookupBinId = new System.Windows.Forms.TextBox();
            this.createBatchControl1 = new BinMonitor.Common.CreateBatchControl();
            this.grpBinLookup.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBinLookup
            // 
            this.grpBinLookup.Controls.Add(this.btnPerformTransfer);
            this.grpBinLookup.Controls.Add(this.lblSpecimensToTransfer);
            this.grpBinLookup.Controls.Add(this.lbSpecimensToTransfer);
            this.grpBinLookup.Controls.Add(this.txtBatchId);
            this.grpBinLookup.Controls.Add(this.lblOriginalBatchId);
            this.grpBinLookup.Controls.Add(this.btnLookupBinById);
            this.grpBinLookup.Controls.Add(this.lblLookupBinId);
            this.grpBinLookup.Controls.Add(this.txtLookupBinId);
            this.grpBinLookup.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBinLookup.Location = new System.Drawing.Point(0, 0);
            this.grpBinLookup.Name = "grpBinLookup";
            this.grpBinLookup.Size = new System.Drawing.Size(665, 184);
            this.grpBinLookup.TabIndex = 0;
            this.grpBinLookup.TabStop = false;
            this.grpBinLookup.Text = "Transfer Details";
            // 
            // btnPerformTransfer
            // 
            this.btnPerformTransfer.Location = new System.Drawing.Point(422, 74);
            this.btnPerformTransfer.Name = "btnPerformTransfer";
            this.btnPerformTransfer.Size = new System.Drawing.Size(120, 23);
            this.btnPerformTransfer.TabIndex = 28;
            this.btnPerformTransfer.Text = "Transfer";
            this.btnPerformTransfer.UseVisualStyleBackColor = true;
            // 
            // lblSpecimensToTransfer
            // 
            this.lblSpecimensToTransfer.AutoSize = true;
            this.lblSpecimensToTransfer.Location = new System.Drawing.Point(175, 74);
            this.lblSpecimensToTransfer.Name = "lblSpecimensToTransfer";
            this.lblSpecimensToTransfer.Size = new System.Drawing.Size(117, 13);
            this.lblSpecimensToTransfer.TabIndex = 27;
            this.lblSpecimensToTransfer.Text = "Specimens To Transfer";
            // 
            // lbSpecimensToTransfer
            // 
            this.lbSpecimensToTransfer.FormattingEnabled = true;
            this.lbSpecimensToTransfer.Location = new System.Drawing.Point(296, 74);
            this.lbSpecimensToTransfer.Name = "lbSpecimensToTransfer";
            this.lbSpecimensToTransfer.Size = new System.Drawing.Size(120, 95);
            this.lbSpecimensToTransfer.TabIndex = 26;
            // 
            // txtBatchId
            // 
            this.txtBatchId.Location = new System.Drawing.Point(296, 48);
            this.txtBatchId.Name = "txtBatchId";
            this.txtBatchId.ReadOnly = true;
            this.txtBatchId.Size = new System.Drawing.Size(171, 20);
            this.txtBatchId.TabIndex = 23;
            this.txtBatchId.TabStop = false;
            // 
            // lblOriginalBatchId
            // 
            this.lblOriginalBatchId.AutoSize = true;
            this.lblOriginalBatchId.Location = new System.Drawing.Point(203, 51);
            this.lblOriginalBatchId.Name = "lblOriginalBatchId";
            this.lblOriginalBatchId.Size = new System.Drawing.Size(87, 13);
            this.lblOriginalBatchId.TabIndex = 22;
            this.lblOriginalBatchId.Text = "Original Batch ID";
            this.lblOriginalBatchId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLookupBinById
            // 
            this.btnLookupBinById.Location = new System.Drawing.Point(404, 19);
            this.btnLookupBinById.Name = "btnLookupBinById";
            this.btnLookupBinById.Size = new System.Drawing.Size(75, 23);
            this.btnLookupBinById.TabIndex = 21;
            this.btnLookupBinById.Text = "Lookup";
            this.btnLookupBinById.UseVisualStyleBackColor = true;
            // 
            // lblLookupBinId
            // 
            this.lblLookupBinId.AutoSize = true;
            this.lblLookupBinId.Location = new System.Drawing.Point(218, 24);
            this.lblLookupBinId.Name = "lblLookupBinId";
            this.lblLookupBinId.Size = new System.Drawing.Size(74, 13);
            this.lblLookupBinId.TabIndex = 19;
            this.lblLookupBinId.Text = "Original Bin ID";
            this.lblLookupBinId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtLookupBinId
            // 
            this.txtLookupBinId.Location = new System.Drawing.Point(296, 21);
            this.txtLookupBinId.Name = "txtLookupBinId";
            this.txtLookupBinId.Size = new System.Drawing.Size(100, 20);
            this.txtLookupBinId.TabIndex = 20;
            this.txtLookupBinId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLookupBinId_KeyPress);
            // 
            // createBatchControl1
            // 
            this.createBatchControl1.CredentialHost = null;
            this.createBatchControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.createBatchControl1.Location = new System.Drawing.Point(0, 184);
            this.createBatchControl1.Name = "createBatchControl1";
            this.createBatchControl1.Size = new System.Drawing.Size(665, 710);
            this.createBatchControl1.TabIndex = 1;
            // 
            // TransferBatchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createBatchControl1);
            this.Controls.Add(this.grpBinLookup);
            this.Name = "TransferBatchControl";
            this.Size = new System.Drawing.Size(665, 925);
            this.grpBinLookup.ResumeLayout(false);
            this.grpBinLookup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBinLookup;
        private System.Windows.Forms.Button btnLookupBinById;
        private System.Windows.Forms.Label lblLookupBinId;
        private System.Windows.Forms.TextBox txtLookupBinId;
        private System.Windows.Forms.TextBox txtBatchId;
        private System.Windows.Forms.Label lblOriginalBatchId;
        private System.Windows.Forms.Button btnPerformTransfer;
        private System.Windows.Forms.Label lblSpecimensToTransfer;
        private System.Windows.Forms.ListBox lbSpecimensToTransfer;
        private CreateBatchControl createBatchControl1;
    }
}
