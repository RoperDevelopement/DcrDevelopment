namespace Edocs.Clients.NYP.L8SpecimenBatchUploader
{
    partial class frmBatchUploader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatchUploader));
            this.lblLastUpdated = new System.Windows.Forms.Label();
            this.txtLastUpdated = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblNextUpdate = new System.Windows.Forms.Label();
            this.txtNextUpdate = new System.Windows.Forms.TextBox();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.AutoSize = true;
            this.lblLastUpdated.Location = new System.Drawing.Point(11, 13);
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(71, 13);
            this.lblLastUpdated.TabIndex = 0;
            this.lblLastUpdated.Text = "Last Updated";
            // 
            // txtLastUpdated
            // 
            this.txtLastUpdated.Location = new System.Drawing.Point(88, 10);
            this.txtLastUpdated.Name = "txtLastUpdated";
            this.txtLastUpdated.ReadOnly = true;
            this.txtLastUpdated.Size = new System.Drawing.Size(140, 20);
            this.txtLastUpdated.TabIndex = 1;
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(0, 34);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(565, 156);
            this.txtMessage.TabIndex = 2;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnUpdate);
            this.pnlControls.Controls.Add(this.lblNextUpdate);
            this.pnlControls.Controls.Add(this.txtNextUpdate);
            this.pnlControls.Controls.Add(this.lblLastUpdated);
            this.pnlControls.Controls.Add(this.txtLastUpdated);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(565, 34);
            this.pnlControls.TabIndex = 3;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(478, 8);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblNextUpdate
            // 
            this.lblNextUpdate.AutoSize = true;
            this.lblNextUpdate.Location = new System.Drawing.Point(235, 13);
            this.lblNextUpdate.Name = "lblNextUpdate";
            this.lblNextUpdate.Size = new System.Drawing.Size(67, 13);
            this.lblNextUpdate.TabIndex = 2;
            this.lblNextUpdate.Text = "Next Update";
            // 
            // txtNextUpdate
            // 
            this.txtNextUpdate.Location = new System.Drawing.Point(312, 10);
            this.txtNextUpdate.Name = "txtNextUpdate";
            this.txtNextUpdate.ReadOnly = true;
            this.txtNextUpdate.Size = new System.Drawing.Size(156, 20);
            this.txtNextUpdate.TabIndex = 3;
            // 
            // frmBatchUploader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 190);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pnlControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBatchUploader";
            this.Text = "L8 Batch Uploader";
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLastUpdated;
        private System.Windows.Forms.TextBox txtLastUpdated;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Label lblNextUpdate;
        private System.Windows.Forms.TextBox txtNextUpdate;
        private System.Windows.Forms.Button btnUpdate;

    }
}

