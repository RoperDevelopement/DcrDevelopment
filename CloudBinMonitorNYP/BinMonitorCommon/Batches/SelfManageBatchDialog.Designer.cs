namespace BinMonitor.Common
{
    partial class SelfManageBatchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelfManageBatchDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnAddComment = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlDialogControls = new System.Windows.Forms.Panel();
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.UserAuthenticationControl = new BinMonitor.Common.UserAuthenticationControl();
            this.grpBatchLookup = new System.Windows.Forms.GroupBox();
            this.BatchLookup = new BinMonitor.Common.BatchLookupControl();
            this.grpComments = new System.Windows.Forms.GroupBox();
            this.txtNewComment = new System.Windows.Forms.TextBox();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.BatchStatusManager = new BinMonitor.Common.Batches.BatchStatusManager();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlDialogControls.SuspendLayout();
            this.grpCredentials.SuspendLayout();
            this.grpBatchLookup.SuspendLayout();
            this.grpComments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(233, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 67);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnAddComment
            // 
            this.btnAddComment.Location = new System.Drawing.Point(555, 77);
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(55, 23);
            this.btnAddComment.TabIndex = 3;
            this.btnAddComment.Text = "Add";
            this.btnAddComment.UseVisualStyleBackColor = true;
            this.btnAddComment.Click += new System.EventHandler(this.btnAddComment_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(314, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 67);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlDialogControls
            // 
            this.pnlDialogControls.Controls.Add(this.btnCancel);
            this.pnlDialogControls.Controls.Add(this.btnOk);
            this.pnlDialogControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDialogControls.Location = new System.Drawing.Point(0, 525);
            this.pnlDialogControls.Name = "pnlDialogControls";
            this.pnlDialogControls.Size = new System.Drawing.Size(622, 78);
            this.pnlDialogControls.TabIndex = 49;
            // 
            // grpCredentials
            // 
            this.grpCredentials.Controls.Add(this.UserAuthenticationControl);
            this.grpCredentials.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCredentials.Location = new System.Drawing.Point(0, 0);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(622, 44);
            this.grpCredentials.TabIndex = 0;
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
            // grpBatchLookup
            // 
            this.grpBatchLookup.Controls.Add(this.BatchLookup);
            this.grpBatchLookup.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBatchLookup.Location = new System.Drawing.Point(0, 44);
            this.grpBatchLookup.Name = "grpBatchLookup";
            this.grpBatchLookup.Size = new System.Drawing.Size(622, 100);
            this.grpBatchLookup.TabIndex = 51;
            this.grpBatchLookup.TabStop = false;
            this.grpBatchLookup.Text = "Lookup";
            // 
            // BatchLookup
            // 
            this.BatchLookup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BatchLookup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BatchLookup.Location = new System.Drawing.Point(3, 16);
            this.BatchLookup.Name = "BatchLookup";
            this.BatchLookup.SelectedBatch = null;
            this.BatchLookup.Size = new System.Drawing.Size(616, 81);
            this.BatchLookup.TabIndex = 1;
            this.BatchLookup.SelectedBatchChanged += new System.EventHandler<BinMonitor.Common.SelectedBatchChangedEventArgs>(this.BatchLookup_SelectedBatchChanged);
            // 
            // grpComments
            // 
            this.grpComments.Controls.Add(this.txtNewComment);
            this.grpComments.Controls.Add(this.btnAddComment);
            this.grpComments.Controls.Add(this.txtComments);
            this.grpComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpComments.Location = new System.Drawing.Point(0, 144);
            this.grpComments.Name = "grpComments";
            this.grpComments.Size = new System.Drawing.Size(622, 107);
            this.grpComments.TabIndex = 2;
            this.grpComments.TabStop = false;
            this.grpComments.Text = "Comments";
            // 
            // txtNewComment
            // 
            this.txtNewComment.Location = new System.Drawing.Point(12, 80);
            this.txtNewComment.Name = "txtNewComment";
            this.txtNewComment.Size = new System.Drawing.Size(537, 20);
            this.txtNewComment.TabIndex = 2;
            this.txtNewComment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewComment_KeyPress);
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(12, 19);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.ReadOnly = true;
            this.txtComments.Size = new System.Drawing.Size(598, 59);
            this.txtComments.TabIndex = 35;
            // 
            // BatchStatusManager
            // 
            this.BatchStatusManager.ActiveBatch = null;
            this.BatchStatusManager.Dock = System.Windows.Forms.DockStyle.Top;
            this.BatchStatusManager.Location = new System.Drawing.Point(0, 251);
            this.BatchStatusManager.Name = "BatchStatusManager";
            this.BatchStatusManager.Size = new System.Drawing.Size(622, 274);
            this.BatchStatusManager.TabIndex = 4;
            this.BatchStatusManager.WorkflowAssignmentMode = BinMonitor.Common.Batches.WorkflowAssignmentMode.Self;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // SelfManageBatchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 627);
            this.Controls.Add(this.pnlDialogControls);
            this.Controls.Add(this.BatchStatusManager);
            this.Controls.Add(this.grpComments);
            this.Controls.Add(this.grpBatchLookup);
            this.Controls.Add(this.grpCredentials);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelfManageBatchDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Self Manage Batch";
            this.Shown += new System.EventHandler(this.SelfManageBatchDialog_Shown);
            this.pnlDialogControls.ResumeLayout(false);
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            this.grpBatchLookup.ResumeLayout(false);
            this.grpComments.ResumeLayout(false);
            this.grpComments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnAddComment;
        private System.Windows.Forms.Button btnCancel;
        private BatchLookupControl BatchLookup;
        private Batches.BatchStatusManager BatchStatusManager;
        private System.Windows.Forms.Panel pnlDialogControls;
        private System.Windows.Forms.GroupBox grpCredentials;
        private System.Windows.Forms.GroupBox grpBatchLookup;
        private System.Windows.Forms.GroupBox grpComments;
        private System.Windows.Forms.TextBox txtNewComment;
        private System.Windows.Forms.TextBox txtComments;
        private UserAuthenticationControl UserAuthenticationControl;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}