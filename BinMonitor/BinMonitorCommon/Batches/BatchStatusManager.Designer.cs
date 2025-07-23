namespace BinMonitor.Common.Batches
{
    partial class BatchStatusManager
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCompleteProcessing = new System.Windows.Forms.Button();
            this.btnBeginProcessing = new System.Windows.Forms.Button();
            this.btnCompleteRegistration = new System.Windows.Forms.Button();
            this.btnBeginRegistration = new System.Windows.Forms.Button();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtClosedBy = new System.Windows.Forms.TextBox();
            this.lblCreatedAt = new System.Windows.Forms.Label();
            this.lblClosedBy = new System.Windows.Forms.Label();
            this.txtCreatedAt = new System.Windows.Forms.TextBox();
            this.txtClosedAt = new System.Windows.Forms.TextBox();
            this.CreatedBy = new System.Windows.Forms.Label();
            this.lblClosedAt = new System.Windows.Forms.Label();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.wfsRegister = new BinMonitor.Common.WorkflowStepViewer();
            this.wfsProcess = new BinMonitor.Common.WorkflowStepViewer();
            this.pnlControls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(540, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 64);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close\r\nBatch";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCompleteProcessing
            // 
            this.btnCompleteProcessing.Enabled = false;
            this.btnCompleteProcessing.Location = new System.Drawing.Point(350, 2);
            this.btnCompleteProcessing.Name = "btnCompleteProcessing";
            this.btnCompleteProcessing.Size = new System.Drawing.Size(75, 64);
            this.btnCompleteProcessing.TabIndex = 3;
            this.btnCompleteProcessing.Text = "Complete Processing";
            this.btnCompleteProcessing.UseVisualStyleBackColor = true;
            this.btnCompleteProcessing.Click += new System.EventHandler(this.btnCompleteProcessing_Click);
            // 
            // btnBeginProcessing
            // 
            this.btnBeginProcessing.Enabled = false;
            this.btnBeginProcessing.Location = new System.Drawing.Point(217, 2);
            this.btnBeginProcessing.Name = "btnBeginProcessing";
            this.btnBeginProcessing.Size = new System.Drawing.Size(75, 64);
            this.btnBeginProcessing.TabIndex = 2;
            this.btnBeginProcessing.Text = "Begin Processing";
            this.btnBeginProcessing.UseVisualStyleBackColor = true;
            this.btnBeginProcessing.Click += new System.EventHandler(this.btnBeginProcessing_Click);
            // 
            // btnCompleteRegistration
            // 
            this.btnCompleteRegistration.Enabled = false;
            this.btnCompleteRegistration.Location = new System.Drawing.Point(136, 2);
            this.btnCompleteRegistration.Name = "btnCompleteRegistration";
            this.btnCompleteRegistration.Size = new System.Drawing.Size(75, 64);
            this.btnCompleteRegistration.TabIndex = 1;
            this.btnCompleteRegistration.Text = "Complete Registration";
            this.btnCompleteRegistration.UseVisualStyleBackColor = true;
            this.btnCompleteRegistration.Click += new System.EventHandler(this.btnCompleteRegistration_Click);
            // 
            // btnBeginRegistration
            // 
            this.btnBeginRegistration.Enabled = false;
            this.btnBeginRegistration.Location = new System.Drawing.Point(3, 2);
            this.btnBeginRegistration.Name = "btnBeginRegistration";
            this.btnBeginRegistration.Size = new System.Drawing.Size(75, 64);
            this.btnBeginRegistration.TabIndex = 0;
            this.btnBeginRegistration.Text = "Begin Registration";
            this.btnBeginRegistration.UseVisualStyleBackColor = true;
            this.btnBeginRegistration.Click += new System.EventHandler(this.btnBeginRegistration_Click);
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnBeginRegistration);
            this.pnlControls.Controls.Add(this.btnClose);
            this.pnlControls.Controls.Add(this.btnCompleteRegistration);
            this.pnlControls.Controls.Add(this.btnCompleteProcessing);
            this.pnlControls.Controls.Add(this.btnBeginProcessing);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 198);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(618, 72);
            this.pnlControls.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.txtClosedBy);
            this.panel1.Controls.Add(this.lblCreatedAt);
            this.panel1.Controls.Add(this.lblClosedBy);
            this.panel1.Controls.Add(this.txtCreatedAt);
            this.panel1.Controls.Add(this.txtClosedAt);
            this.panel1.Controls.Add(this.CreatedBy);
            this.panel1.Controls.Add(this.lblClosedAt);
            this.panel1.Controls.Add(this.txtCreatedBy);
            this.panel1.Location = new System.Drawing.Point(431, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(182, 192);
            this.panel1.TabIndex = 35;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(180, 20);
            this.lblStatus.TabIndex = 26;
            this.lblStatus.Text = "Overall Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtClosedBy
            // 
            this.txtClosedBy.Location = new System.Drawing.Point(73, 101);
            this.txtClosedBy.Name = "txtClosedBy";
            this.txtClosedBy.ReadOnly = true;
            this.txtClosedBy.Size = new System.Drawing.Size(100, 20);
            this.txtClosedBy.TabIndex = 25;
            this.txtClosedBy.TabStop = false;
            // 
            // lblCreatedAt
            // 
            this.lblCreatedAt.AutoSize = true;
            this.lblCreatedAt.Location = new System.Drawing.Point(10, 34);
            this.lblCreatedAt.Name = "lblCreatedAt";
            this.lblCreatedAt.Size = new System.Drawing.Size(57, 13);
            this.lblCreatedAt.TabIndex = 18;
            this.lblCreatedAt.Text = "Created At";
            // 
            // lblClosedBy
            // 
            this.lblClosedBy.AutoSize = true;
            this.lblClosedBy.Location = new System.Drawing.Point(13, 104);
            this.lblClosedBy.Name = "lblClosedBy";
            this.lblClosedBy.Size = new System.Drawing.Size(54, 13);
            this.lblClosedBy.TabIndex = 24;
            this.lblClosedBy.Text = "Closed By";
            // 
            // txtCreatedAt
            // 
            this.txtCreatedAt.Location = new System.Drawing.Point(73, 31);
            this.txtCreatedAt.Name = "txtCreatedAt";
            this.txtCreatedAt.ReadOnly = true;
            this.txtCreatedAt.Size = new System.Drawing.Size(100, 20);
            this.txtCreatedAt.TabIndex = 19;
            this.txtCreatedAt.TabStop = false;
            // 
            // txtClosedAt
            // 
            this.txtClosedAt.Location = new System.Drawing.Point(73, 78);
            this.txtClosedAt.Name = "txtClosedAt";
            this.txtClosedAt.ReadOnly = true;
            this.txtClosedAt.Size = new System.Drawing.Size(100, 20);
            this.txtClosedAt.TabIndex = 23;
            this.txtClosedAt.TabStop = false;
            // 
            // CreatedBy
            // 
            this.CreatedBy.AutoSize = true;
            this.CreatedBy.Location = new System.Drawing.Point(8, 57);
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.Size = new System.Drawing.Size(59, 13);
            this.CreatedBy.TabIndex = 20;
            this.CreatedBy.Text = "Created By";
            // 
            // lblClosedAt
            // 
            this.lblClosedAt.AutoSize = true;
            this.lblClosedAt.Location = new System.Drawing.Point(15, 81);
            this.lblClosedAt.Name = "lblClosedAt";
            this.lblClosedAt.Size = new System.Drawing.Size(52, 13);
            this.lblClosedAt.TabIndex = 22;
            this.lblClosedAt.Text = "Closed At";
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.Location = new System.Drawing.Point(73, 54);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(100, 20);
            this.txtCreatedBy.TabIndex = 21;
            this.txtCreatedBy.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.wfsRegister);
            this.flowLayoutPanel1.Controls.Add(this.wfsProcess);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(618, 198);
            this.flowLayoutPanel1.TabIndex = 37;
            // 
            // wfsRegister
            // 
            this.wfsRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfsRegister.Caption = "Registration";
            this.wfsRegister.Location = new System.Drawing.Point(3, 3);
            this.wfsRegister.Name = "wfsRegister";
            this.wfsRegister.Size = new System.Drawing.Size(208, 192);
            this.wfsRegister.TabIndex = 32;
            this.wfsRegister.TabStop = false;
            // 
            // wfsProcess
            // 
            this.wfsProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wfsProcess.Caption = "Processing";
            this.wfsProcess.Location = new System.Drawing.Point(217, 3);
            this.wfsProcess.Name = "wfsProcess";
            this.wfsProcess.Size = new System.Drawing.Size(208, 192);
            this.wfsProcess.TabIndex = 33;
            this.wfsProcess.TabStop = false;
            // 
            // BatchStatusManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "BatchStatusManager";
            this.Size = new System.Drawing.Size(618, 274);
            this.pnlControls.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCompleteProcessing;
        private System.Windows.Forms.Button btnBeginProcessing;
        private System.Windows.Forms.Button btnCompleteRegistration;
        private System.Windows.Forms.Button btnBeginRegistration;
        private System.Windows.Forms.Panel pnlControls;
        private WorkflowStepViewer wfsRegister;
        private WorkflowStepViewer wfsProcess;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtClosedBy;
        private System.Windows.Forms.Label lblCreatedAt;
        private System.Windows.Forms.Label lblClosedBy;
        private System.Windows.Forms.TextBox txtCreatedAt;
        private System.Windows.Forms.TextBox txtClosedAt;
        private System.Windows.Forms.Label CreatedBy;
        private System.Windows.Forms.Label lblClosedAt;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
