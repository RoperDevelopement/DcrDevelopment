namespace BinMonitor.Common
{
    partial class WorkflowStepConfigurationControl
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
            this.lblCaption = new System.Windows.Forms.Label();
            this.chkEmailOnStart = new System.Windows.Forms.CheckBox();
            this.chkEmailOnComplete = new System.Windows.Forms.CheckBox();
            this.chkIncludeContentsInEmail = new System.Windows.Forms.CheckBox();
            this.lblEmailRecipients = new System.Windows.Forms.Label();
            this.txtEmailRecipients = new System.Windows.Forms.TextBox();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(134, 22);
            this.lblCaption.TabIndex = 10;
            this.lblCaption.Text = "...";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkEmailOnStart
            // 
            this.chkEmailOnStart.AutoSize = true;
            this.chkEmailOnStart.Location = new System.Drawing.Point(10, 29);
            this.chkEmailOnStart.Name = "chkEmailOnStart";
            this.chkEmailOnStart.Size = new System.Drawing.Size(93, 17);
            this.chkEmailOnStart.TabIndex = 0;
            this.chkEmailOnStart.Text = "Email On Start";
            this.chkEmailOnStart.UseVisualStyleBackColor = true;
            // 
            // chkEmailOnComplete
            // 
            this.chkEmailOnComplete.AutoSize = true;
            this.chkEmailOnComplete.Location = new System.Drawing.Point(10, 52);
            this.chkEmailOnComplete.Name = "chkEmailOnComplete";
            this.chkEmailOnComplete.Size = new System.Drawing.Size(115, 17);
            this.chkEmailOnComplete.TabIndex = 1;
            this.chkEmailOnComplete.Text = "Email On Complete";
            this.chkEmailOnComplete.UseVisualStyleBackColor = true;
            // 
            // chkIncludeContentsInEmail
            // 
            this.chkIncludeContentsInEmail.AutoSize = true;
            this.chkIncludeContentsInEmail.Location = new System.Drawing.Point(10, 75);
            this.chkIncludeContentsInEmail.Name = "chkIncludeContentsInEmail";
            this.chkIncludeContentsInEmail.Size = new System.Drawing.Size(96, 17);
            this.chkIncludeContentsInEmail.TabIndex = 2;
            this.chkIncludeContentsInEmail.Text = "Email Contents";
            this.chkIncludeContentsInEmail.UseVisualStyleBackColor = true;
            // 
            // lblEmailRecipients
            // 
            this.lblEmailRecipients.AutoSize = true;
            this.lblEmailRecipients.Location = new System.Drawing.Point(7, 95);
            this.lblEmailRecipients.Name = "lblEmailRecipients";
            this.lblEmailRecipients.Size = new System.Drawing.Size(85, 13);
            this.lblEmailRecipients.TabIndex = 14;
            this.lblEmailRecipients.Text = "Email Recipients";
            // 
            // txtEmailRecipients
            // 
            this.txtEmailRecipients.Location = new System.Drawing.Point(10, 111);
            this.txtEmailRecipients.Name = "txtEmailRecipients";
            this.txtEmailRecipients.Size = new System.Drawing.Size(115, 20);
            this.txtEmailRecipients.TabIndex = 3;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.ContainerControl = this;
            // 
            // WorkflowStepConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtEmailRecipients);
            this.Controls.Add(this.lblEmailRecipients);
            this.Controls.Add(this.chkIncludeContentsInEmail);
            this.Controls.Add(this.chkEmailOnComplete);
            this.Controls.Add(this.chkEmailOnStart);
            this.Controls.Add(this.lblCaption);
            this.Name = "WorkflowStepConfigurationControl";
            this.Size = new System.Drawing.Size(134, 147);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.CheckBox chkEmailOnStart;
        private System.Windows.Forms.CheckBox chkEmailOnComplete;
        private System.Windows.Forms.CheckBox chkIncludeContentsInEmail;
        private System.Windows.Forms.Label lblEmailRecipients;
        private System.Windows.Forms.TextBox txtEmailRecipients;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
    }
}
