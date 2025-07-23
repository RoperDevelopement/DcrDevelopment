namespace BinMonitor.Common
{
    partial class ArchiveManagerControl
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
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.BatchStatusManager = new BinMonitor.Common.Batches.BatchStatusManager();
            this.gpbBatchDetails = new System.Windows.Forms.GroupBox();
            this.btnChangeCategory = new System.Windows.Forms.Button();
            this.txtNewComment = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.txtTransferredFromBatchId = new System.Windows.Forms.TextBox();
            this.lblTransferredFrom = new System.Windows.Forms.Label();
            this.btnAddComment = new System.Windows.Forms.Button();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.lbBatchContents = new System.Windows.Forms.ListBox();
            this.lblBatchContents = new System.Windows.Forms.Label();
            this.lblBatchCategory = new System.Windows.Forms.Label();
            this.pnlScroll.SuspendLayout();
            this.gpbBatchDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlScroll
            // 
            this.pnlScroll.Controls.Add(this.BatchStatusManager);
            this.pnlScroll.Controls.Add(this.gpbBatchDetails);
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Margin = new System.Windows.Forms.Padding(4);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(829, 553);
            this.pnlScroll.TabIndex = 0;
            // 
            // BatchStatusManager
            // 
            this.BatchStatusManager.ActiveBatch = null;
            this.BatchStatusManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BatchStatusManager.IsArchive = false;
            this.BatchStatusManager.Location = new System.Drawing.Point(0, 215);
            this.BatchStatusManager.Margin = new System.Windows.Forms.Padding(5);
            this.BatchStatusManager.Name = "BatchStatusManager";
            this.BatchStatusManager.Size = new System.Drawing.Size(829, 338);
            this.BatchStatusManager.TabIndex = 2;
            this.BatchStatusManager.WorkflowAssignmentMode = BinMonitor.Common.Batches.WorkflowAssignmentMode.Self;
            // 
            // gpbBatchDetails
            // 
            this.gpbBatchDetails.Controls.Add(this.btnChangeCategory);
            this.gpbBatchDetails.Controls.Add(this.txtNewComment);
            this.gpbBatchDetails.Controls.Add(this.txtCategory);
            this.gpbBatchDetails.Controls.Add(this.txtTransferredFromBatchId);
            this.gpbBatchDetails.Controls.Add(this.lblTransferredFrom);
            this.gpbBatchDetails.Controls.Add(this.btnAddComment);
            this.gpbBatchDetails.Controls.Add(this.txtComments);
            this.gpbBatchDetails.Controls.Add(this.lblComments);
            this.gpbBatchDetails.Controls.Add(this.lbBatchContents);
            this.gpbBatchDetails.Controls.Add(this.lblBatchContents);
            this.gpbBatchDetails.Controls.Add(this.lblBatchCategory);
            this.gpbBatchDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpbBatchDetails.Location = new System.Drawing.Point(0, 0);
            this.gpbBatchDetails.Margin = new System.Windows.Forms.Padding(4);
            this.gpbBatchDetails.Name = "gpbBatchDetails";
            this.gpbBatchDetails.Padding = new System.Windows.Forms.Padding(4);
            this.gpbBatchDetails.Size = new System.Drawing.Size(829, 215);
            this.gpbBatchDetails.TabIndex = 24;
            this.gpbBatchDetails.TabStop = false;
            this.gpbBatchDetails.Text = "Batch Details";
            // 
            // btnChangeCategory
            // 
            this.btnChangeCategory.AutoSize = true;
            this.btnChangeCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnChangeCategory.Location = new System.Drawing.Point(352, 53);
            this.btnChangeCategory.Margin = new System.Windows.Forms.Padding(4);
            this.btnChangeCategory.Name = "btnChangeCategory";
            this.btnChangeCategory.Size = new System.Drawing.Size(30, 27);
            this.btnChangeCategory.TabIndex = 20;
            this.btnChangeCategory.TabStop = false;
            this.btnChangeCategory.Text = "...";
            this.btnChangeCategory.UseVisualStyleBackColor = true;
            this.btnChangeCategory.Click += new System.EventHandler(this.btnChangeCategory_Click);
            // 
            // txtNewComment
            // 
            this.txtNewComment.Location = new System.Drawing.Point(161, 169);
            this.txtNewComment.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewComment.Name = "txtNewComment";
            this.txtNewComment.Size = new System.Drawing.Size(299, 22);
            this.txtNewComment.TabIndex = 0;
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(161, 55);
            this.txtCategory.Margin = new System.Windows.Forms.Padding(4);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(181, 22);
            this.txtCategory.TabIndex = 18;
            // 
            // txtTransferredFromBatchId
            // 
            this.txtTransferredFromBatchId.Location = new System.Drawing.Point(161, 23);
            this.txtTransferredFromBatchId.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransferredFromBatchId.Name = "txtTransferredFromBatchId";
            this.txtTransferredFromBatchId.ReadOnly = true;
            this.txtTransferredFromBatchId.Size = new System.Drawing.Size(307, 22);
            this.txtTransferredFromBatchId.TabIndex = 17;
            this.txtTransferredFromBatchId.TabStop = false;
            // 
            // lblTransferredFrom
            // 
            this.lblTransferredFrom.AutoSize = true;
            this.lblTransferredFrom.Location = new System.Drawing.Point(37, 27);
            this.lblTransferredFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTransferredFrom.Name = "lblTransferredFrom";
            this.lblTransferredFrom.Size = new System.Drawing.Size(119, 17);
            this.lblTransferredFrom.TabIndex = 16;
            this.lblTransferredFrom.Text = "Transferred From";
            this.lblTransferredFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnAddComment
            // 
            this.btnAddComment.Location = new System.Drawing.Point(469, 166);
            this.btnAddComment.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(73, 28);
            this.btnAddComment.TabIndex = 1;
            this.btnAddComment.Text = "Add";
            this.btnAddComment.UseVisualStyleBackColor = true;
            this.btnAddComment.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(161, 90);
            this.txtComments.Margin = new System.Windows.Forms.Padding(4);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.ReadOnly = true;
            this.txtComments.Size = new System.Drawing.Size(380, 72);
            this.txtComments.TabIndex = 14;
            // 
            // lblComments
            // 
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(79, 94);
            this.lblComments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(74, 17);
            this.lblComments.TabIndex = 13;
            this.lblComments.Text = "Comments";
            this.lblComments.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbBatchContents
            // 
            this.lbBatchContents.FormattingEnabled = true;
            this.lbBatchContents.ItemHeight = 16;
            this.lbBatchContents.Location = new System.Drawing.Point(572, 46);
            this.lbBatchContents.Margin = new System.Windows.Forms.Padding(4);
            this.lbBatchContents.Name = "lbBatchContents";
            this.lbBatchContents.Size = new System.Drawing.Size(292, 148);
            this.lbBatchContents.TabIndex = 5;
            // 
            // lblBatchContents
            // 
            this.lblBatchContents.AutoSize = true;
            this.lblBatchContents.Location = new System.Drawing.Point(568, 27);
            this.lblBatchContents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchContents.Name = "lblBatchContents";
            this.lblBatchContents.Size = new System.Drawing.Size(64, 17);
            this.lblBatchContents.TabIndex = 4;
            this.lblBatchContents.Text = "Contents";
            this.lblBatchContents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBatchCategory
            // 
            this.lblBatchCategory.AutoSize = true;
            this.lblBatchCategory.Location = new System.Drawing.Point(88, 59);
            this.lblBatchCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchCategory.Name = "lblBatchCategory";
            this.lblBatchCategory.Size = new System.Drawing.Size(65, 17);
            this.lblBatchCategory.TabIndex = 2;
            this.lblBatchCategory.Text = "Category";
            this.lblBatchCategory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ArchiveManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScroll);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ArchiveManagerControl";
            this.Size = new System.Drawing.Size(829, 553);
            this.pnlScroll.ResumeLayout(false);
            this.gpbBatchDetails.ResumeLayout(false);
            this.gpbBatchDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.GroupBox gpbBatchDetails; 
        private System.Windows.Forms.Button btnAddComment;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.ListBox lbBatchContents;
        private System.Windows.Forms.Label lblBatchContents;
        private System.Windows.Forms.Label lblBatchCategory;
        private System.Windows.Forms.TextBox txtTransferredFromBatchId;
        private System.Windows.Forms.Label lblTransferredFrom;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.TextBox txtNewComment;
        private Batches.BatchStatusManager BatchStatusManager;
        private System.Windows.Forms.Button btnChangeCategory;
    }
}
