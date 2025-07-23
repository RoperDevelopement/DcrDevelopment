namespace BinMonitor.Common
{
    partial class WorkflowStepViewer
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
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblStartedAt = new System.Windows.Forms.Label();
            this.lblAssignedBy = new System.Windows.Forms.Label();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.lblCompletedAt = new System.Windows.Forms.Label();
            this.lblCompletedBy = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.txtStartedAt = new System.Windows.Forms.TextBox();
            this.txtAssignedBy = new System.Windows.Forms.TextBox();
            this.txtAssignedTo = new System.Windows.Forms.TextBox();
            this.txtCompletedAt = new System.Windows.Forms.TextBox();
            this.txtCompletedBy = new System.Windows.Forms.TextBox();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(206, 22);
            this.lblCaption.TabIndex = 10;
            this.lblCaption.Text = "...";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStartedAt
            // 
            this.lblStartedAt.AutoSize = true;
            this.lblStartedAt.Location = new System.Drawing.Point(30, 86);
            this.lblStartedAt.Name = "lblStartedAt";
            this.lblStartedAt.Size = new System.Drawing.Size(54, 13);
            this.lblStartedAt.TabIndex = 11;
            this.lblStartedAt.Text = "Started At";
            // 
            // lblAssignedBy
            // 
            this.lblAssignedBy.AutoSize = true;
            this.lblAssignedBy.Location = new System.Drawing.Point(22, 34);
            this.lblAssignedBy.Name = "lblAssignedBy";
            this.lblAssignedBy.Size = new System.Drawing.Size(62, 13);
            this.lblAssignedBy.TabIndex = 12;
            this.lblAssignedBy.Text = "AssignedBy";
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.AutoSize = true;
            this.lblAssignedTo.Location = new System.Drawing.Point(18, 60);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(66, 13);
            this.lblAssignedTo.TabIndex = 13;
            this.lblAssignedTo.Text = "Assigned To";
            // 
            // lblCompletedAt
            // 
            this.lblCompletedAt.AutoSize = true;
            this.lblCompletedAt.Location = new System.Drawing.Point(14, 137);
            this.lblCompletedAt.Name = "lblCompletedAt";
            this.lblCompletedAt.Size = new System.Drawing.Size(70, 13);
            this.lblCompletedAt.TabIndex = 14;
            this.lblCompletedAt.Text = "Completed At";
            // 
            // lblCompletedBy
            // 
            this.lblCompletedBy.AutoSize = true;
            this.lblCompletedBy.Location = new System.Drawing.Point(12, 111);
            this.lblCompletedBy.Name = "lblCompletedBy";
            this.lblCompletedBy.Size = new System.Drawing.Size(72, 13);
            this.lblCompletedBy.TabIndex = 15;
            this.lblCompletedBy.Text = "Completed By";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(37, 163);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(47, 13);
            this.lblDuration.TabIndex = 16;
            this.lblDuration.Text = "Duration";
            // 
            // txtStartedAt
            // 
            this.txtStartedAt.Location = new System.Drawing.Point(92, 83);
            this.txtStartedAt.Name = "txtStartedAt";
            this.txtStartedAt.ReadOnly = true;
            this.txtStartedAt.Size = new System.Drawing.Size(100, 20);
            this.txtStartedAt.TabIndex = 17;
            this.txtStartedAt.TabStop = false;
            // 
            // txtAssignedBy
            // 
            this.txtAssignedBy.Location = new System.Drawing.Point(94, 31);
            this.txtAssignedBy.Name = "txtAssignedBy";
            this.txtAssignedBy.ReadOnly = true;
            this.txtAssignedBy.Size = new System.Drawing.Size(100, 20);
            this.txtAssignedBy.TabIndex = 18;
            this.txtAssignedBy.TabStop = false;
            // 
            // txtAssignedTo
            // 
            this.txtAssignedTo.Location = new System.Drawing.Point(94, 57);
            this.txtAssignedTo.Name = "txtAssignedTo";
            this.txtAssignedTo.ReadOnly = true;
            this.txtAssignedTo.Size = new System.Drawing.Size(100, 20);
            this.txtAssignedTo.TabIndex = 19;
            this.txtAssignedTo.TabStop = false;
            // 
            // txtCompletedAt
            // 
            this.txtCompletedAt.Location = new System.Drawing.Point(92, 134);
            this.txtCompletedAt.Name = "txtCompletedAt";
            this.txtCompletedAt.ReadOnly = true;
            this.txtCompletedAt.Size = new System.Drawing.Size(100, 20);
            this.txtCompletedAt.TabIndex = 20;
            this.txtCompletedAt.TabStop = false;
            // 
            // txtCompletedBy
            // 
            this.txtCompletedBy.Location = new System.Drawing.Point(92, 108);
            this.txtCompletedBy.Name = "txtCompletedBy";
            this.txtCompletedBy.ReadOnly = true;
            this.txtCompletedBy.Size = new System.Drawing.Size(100, 20);
            this.txtCompletedBy.TabIndex = 21;
            this.txtCompletedBy.TabStop = false;
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(92, 160);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.ReadOnly = true;
            this.txtDuration.Size = new System.Drawing.Size(100, 20);
            this.txtDuration.TabIndex = 22;
            this.txtDuration.TabStop = false;
            // 
            // WorkflowStepViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.txtCompletedBy);
            this.Controls.Add(this.txtCompletedAt);
            this.Controls.Add(this.txtAssignedTo);
            this.Controls.Add(this.txtAssignedBy);
            this.Controls.Add(this.txtStartedAt);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblCompletedBy);
            this.Controls.Add(this.lblCompletedAt);
            this.Controls.Add(this.lblAssignedTo);
            this.Controls.Add(this.lblAssignedBy);
            this.Controls.Add(this.lblStartedAt);
            this.Controls.Add(this.lblCaption);
            this.Name = "WorkflowStepViewer";
            this.Size = new System.Drawing.Size(206, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblStartedAt;
        private System.Windows.Forms.Label lblAssignedBy;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.Label lblCompletedAt;
        private System.Windows.Forms.Label lblCompletedBy;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.TextBox txtStartedAt;
        private System.Windows.Forms.TextBox txtAssignedBy;
        private System.Windows.Forms.TextBox txtAssignedTo;
        private System.Windows.Forms.TextBox txtCompletedAt;
        private System.Windows.Forms.TextBox txtCompletedBy;
        private System.Windows.Forms.TextBox txtDuration;
    }
}
