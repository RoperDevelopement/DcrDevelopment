namespace BinMonitor.Common
{
    partial class MasterCategoryPermissionControl
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
            this.components = new System.ComponentModel.Container();
            this.chkCanCreate = new System.Windows.Forms.CheckBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.chkCanAddComments = new System.Windows.Forms.CheckBox();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkCanAssign = new System.Windows.Forms.CheckBox();
            this.chkCanCheckOut = new System.Windows.Forms.CheckBox();
            this.chkCanCheckIn = new System.Windows.Forms.CheckBox();
            this.chkCanClose = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // chkCanCreate
            // 
            this.chkCanCreate.AutoSize = true;
            this.chkCanCreate.Location = new System.Drawing.Point(12, 37);
            this.chkCanCreate.Name = "chkCanCreate";
            this.chkCanCreate.Size = new System.Drawing.Size(57, 17);
            this.chkCanCreate.TabIndex = 0;
            this.chkCanCreate.Text = "Create";
            this.chkCanCreate.UseVisualStyleBackColor = true;
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(121, 23);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "Caption";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chkCanAddComments
            // 
            this.chkCanAddComments.AutoSize = true;
            this.chkCanAddComments.Location = new System.Drawing.Point(12, 59);
            this.chkCanAddComments.Name = "chkCanAddComments";
            this.chkCanAddComments.Size = new System.Drawing.Size(97, 17);
            this.chkCanAddComments.TabIndex = 1;
            this.chkCanAddComments.Text = "Add Comments";
            this.chkCanAddComments.UseVisualStyleBackColor = true;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.ContainerControl = this;
            // 
            // chkCanAssign
            // 
            this.chkCanAssign.AutoSize = true;
            this.chkCanAssign.Location = new System.Drawing.Point(12, 81);
            this.chkCanAssign.Name = "chkCanAssign";
            this.chkCanAssign.Size = new System.Drawing.Size(57, 17);
            this.chkCanAssign.TabIndex = 3;
            this.chkCanAssign.Text = "Assign";
            this.chkCanAssign.UseVisualStyleBackColor = true;
            // 
            // chkCanCheckOut
            // 
            this.chkCanCheckOut.AutoSize = true;
            this.chkCanCheckOut.Location = new System.Drawing.Point(12, 103);
            this.chkCanCheckOut.Name = "chkCanCheckOut";
            this.chkCanCheckOut.Size = new System.Drawing.Size(77, 17);
            this.chkCanCheckOut.TabIndex = 4;
            this.chkCanCheckOut.Text = "Check Out";
            this.chkCanCheckOut.UseVisualStyleBackColor = true;
            // 
            // chkCanCheckIn
            // 
            this.chkCanCheckIn.AutoSize = true;
            this.chkCanCheckIn.Location = new System.Drawing.Point(12, 125);
            this.chkCanCheckIn.Name = "chkCanCheckIn";
            this.chkCanCheckIn.Size = new System.Drawing.Size(69, 17);
            this.chkCanCheckIn.TabIndex = 5;
            this.chkCanCheckIn.Text = "Check In";
            this.chkCanCheckIn.UseVisualStyleBackColor = true;
            // 
            // chkCanClose
            // 
            this.chkCanClose.AutoSize = true;
            this.chkCanClose.Location = new System.Drawing.Point(12, 147);
            this.chkCanClose.Name = "chkCanClose";
            this.chkCanClose.Size = new System.Drawing.Size(52, 17);
            this.chkCanClose.TabIndex = 6;
            this.chkCanClose.Text = "Close";
            this.chkCanClose.UseVisualStyleBackColor = true;
            // 
            // MasterCategoryPermissionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkCanClose);
            this.Controls.Add(this.chkCanCheckIn);
            this.Controls.Add(this.chkCanCheckOut);
            this.Controls.Add(this.chkCanAssign);
            this.Controls.Add(this.chkCanAddComments);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.chkCanCreate);
            this.Name = "MasterCategoryPermissionControl";
            this.Size = new System.Drawing.Size(121, 175);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCanCreate;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.CheckBox chkCanAddComments;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.Windows.Forms.CheckBox chkCanClose;
        private System.Windows.Forms.CheckBox chkCanCheckIn;
        private System.Windows.Forms.CheckBox chkCanCheckOut;
        private System.Windows.Forms.CheckBox chkCanAssign;
    }
}
