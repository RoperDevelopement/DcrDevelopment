namespace BinMonitor.Common
{
    partial class BatchTransferDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchTransferDialog));
            this.lblOriginSpecimens = new System.Windows.Forms.Label();
            this.lbOriginSpecimens = new System.Windows.Forms.ListBox();
            this.txtBatchId = new System.Windows.Forms.TextBox();
            this.lblOriginalBatchId = new System.Windows.Forms.Label();
            this.btnLookupBinById = new System.Windows.Forms.Button();
            this.lblLookupBinId = new System.Windows.Forms.Label();
            this.txtLookupBinId = new System.Windows.Forms.TextBox();
            this.lbSpecimensToTransfer = new System.Windows.Forms.ListBox();
            this.btnAddSpecimen = new System.Windows.Forms.Button();
            this.btnRemoveSpecimen = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grpBatchDetails = new System.Windows.Forms.GroupBox();
            this.grpBatchDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOriginSpecimens
            // 
            this.lblOriginSpecimens.AutoSize = true;
            this.lblOriginSpecimens.Location = new System.Drawing.Point(40, 87);
            this.lblOriginSpecimens.Name = "lblOriginSpecimens";
            this.lblOriginSpecimens.Size = new System.Drawing.Size(59, 13);
            this.lblOriginSpecimens.TabIndex = 35;
            this.lblOriginSpecimens.Text = "Specimens";
            // 
            // lbOriginSpecimens
            // 
            this.lbOriginSpecimens.FormattingEnabled = true;
            this.lbOriginSpecimens.Location = new System.Drawing.Point(105, 87);
            this.lbOriginSpecimens.Name = "lbOriginSpecimens";
            this.lbOriginSpecimens.Size = new System.Drawing.Size(120, 212);
            this.lbOriginSpecimens.TabIndex = 3;
            this.lbOriginSpecimens.DoubleClick += new System.EventHandler(this.lbOriginSpecimens_DoubleClick);
            // 
            // txtBatchId
            // 
            this.txtBatchId.Location = new System.Drawing.Point(203, 61);
            this.txtBatchId.Name = "txtBatchId";
            this.txtBatchId.ReadOnly = true;
            this.txtBatchId.Size = new System.Drawing.Size(171, 20);
            this.txtBatchId.TabIndex = 33;
            this.txtBatchId.TabStop = false;
            // 
            // lblOriginalBatchId
            // 
            this.lblOriginalBatchId.AutoSize = true;
            this.lblOriginalBatchId.Location = new System.Drawing.Point(105, 64);
            this.lblOriginalBatchId.Name = "lblOriginalBatchId";
            this.lblOriginalBatchId.Size = new System.Drawing.Size(87, 13);
            this.lblOriginalBatchId.TabIndex = 32;
            this.lblOriginalBatchId.Text = "Original Batch ID";
            this.lblOriginalBatchId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLookupBinById
            // 
            this.btnLookupBinById.Location = new System.Drawing.Point(309, 33);
            this.btnLookupBinById.Name = "btnLookupBinById";
            this.btnLookupBinById.Size = new System.Drawing.Size(75, 23);
            this.btnLookupBinById.TabIndex = 2;
            this.btnLookupBinById.Text = "Lookup";
            this.btnLookupBinById.UseVisualStyleBackColor = true;
            this.btnLookupBinById.Click += new System.EventHandler(this.btnLookupBinById_Click);
            // 
            // lblLookupBinId
            // 
            this.lblLookupBinId.AutoSize = true;
            this.lblLookupBinId.Location = new System.Drawing.Point(105, 38);
            this.lblLookupBinId.Name = "lblLookupBinId";
            this.lblLookupBinId.Size = new System.Drawing.Size(74, 13);
            this.lblLookupBinId.TabIndex = 29;
            this.lblLookupBinId.Text = "Original Bin ID";
            this.lblLookupBinId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtLookupBinId
            // 
            this.txtLookupBinId.Location = new System.Drawing.Point(203, 35);
            this.txtLookupBinId.Name = "txtLookupBinId";
            this.txtLookupBinId.Size = new System.Drawing.Size(100, 20);
            this.txtLookupBinId.TabIndex = 0;
            this.txtLookupBinId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLookupBinId_KeyPress);
            // 
            // lbSpecimensToTransfer
            // 
            this.lbSpecimensToTransfer.FormattingEnabled = true;
            this.lbSpecimensToTransfer.Location = new System.Drawing.Point(268, 87);
            this.lbSpecimensToTransfer.Name = "lbSpecimensToTransfer";
            this.lbSpecimensToTransfer.Size = new System.Drawing.Size(120, 212);
            this.lbSpecimensToTransfer.TabIndex = 6;
            this.lbSpecimensToTransfer.DoubleClick += new System.EventHandler(this.lbSpecimensToTransfer_DoubleClick);
            // 
            // btnAddSpecimen
            // 
            this.btnAddSpecimen.AutoSize = true;
            this.btnAddSpecimen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddSpecimen.Location = new System.Drawing.Point(233, 87);
            this.btnAddSpecimen.Name = "btnAddSpecimen";
            this.btnAddSpecimen.Size = new System.Drawing.Size(29, 23);
            this.btnAddSpecimen.TabIndex = 4;
            this.btnAddSpecimen.Text = ">>";
            this.btnAddSpecimen.UseVisualStyleBackColor = true;
            this.btnAddSpecimen.Click += new System.EventHandler(this.btnAddSpecimen_Click);
            // 
            // btnRemoveSpecimen
            // 
            this.btnRemoveSpecimen.AutoSize = true;
            this.btnRemoveSpecimen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRemoveSpecimen.Location = new System.Drawing.Point(233, 116);
            this.btnRemoveSpecimen.Name = "btnRemoveSpecimen";
            this.btnRemoveSpecimen.Size = new System.Drawing.Size(29, 23);
            this.btnRemoveSpecimen.TabIndex = 5;
            this.btnRemoveSpecimen.Text = "<<";
            this.btnRemoveSpecimen.UseVisualStyleBackColor = true;
            this.btnRemoveSpecimen.Click += new System.EventHandler(this.btnRemoveSpecimen_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(232, 349);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 67);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(151, 349);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 67);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grpBatchDetails
            // 
            this.grpBatchDetails.Controls.Add(this.lbSpecimensToTransfer);
            this.grpBatchDetails.Controls.Add(this.txtLookupBinId);
            this.grpBatchDetails.Controls.Add(this.lblLookupBinId);
            this.grpBatchDetails.Controls.Add(this.btnRemoveSpecimen);
            this.grpBatchDetails.Controls.Add(this.btnLookupBinById);
            this.grpBatchDetails.Controls.Add(this.btnAddSpecimen);
            this.grpBatchDetails.Controls.Add(this.lblOriginalBatchId);
            this.grpBatchDetails.Controls.Add(this.txtBatchId);
            this.grpBatchDetails.Controls.Add(this.lblOriginSpecimens);
            this.grpBatchDetails.Controls.Add(this.lbOriginSpecimens);
            this.grpBatchDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBatchDetails.Location = new System.Drawing.Point(0, 0);
            this.grpBatchDetails.Name = "grpBatchDetails";
            this.grpBatchDetails.Size = new System.Drawing.Size(458, 324);
            this.grpBatchDetails.TabIndex = 41;
            this.grpBatchDetails.TabStop = false;
            this.grpBatchDetails.Text = "Batch Details";
            // 
            // BatchTransferDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 431);
            this.Controls.Add(this.grpBatchDetails);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BatchTransferDialog";
            this.Text = "Transfer Batch";
            this.Load += new System.EventHandler(this.BatchTransferDialog_Load);
            this.grpBatchDetails.ResumeLayout(false);
            this.grpBatchDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblOriginSpecimens;
        private System.Windows.Forms.ListBox lbOriginSpecimens;
        private System.Windows.Forms.TextBox txtBatchId;
        private System.Windows.Forms.Label lblOriginalBatchId;
        private System.Windows.Forms.Button btnLookupBinById;
        private System.Windows.Forms.Label lblLookupBinId;
        private System.Windows.Forms.TextBox txtLookupBinId;
        private System.Windows.Forms.ListBox lbSpecimensToTransfer;
        private System.Windows.Forms.Button btnAddSpecimen;
        private System.Windows.Forms.Button btnRemoveSpecimen;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grpBatchDetails;
    }
}