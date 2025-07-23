namespace BinMonitor.Common
{
    partial class CheckpointConfigurationControl
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
            this.lblDuration = new System.Windows.Forms.Label();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.chkFlash = new System.Windows.Forms.CheckBox();
            this.chkEmailImmediately = new System.Windows.Forms.CheckBox();
            this.chkEmailUntilComplete = new System.Windows.Forms.CheckBox();
            this.txtEmailFrequency = new System.Windows.Forms.TextBox();
            this.lblEmailFrequency = new System.Windows.Forms.Label();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblCaption = new System.Windows.Forms.Label();
            this.txtEmailRecipients = new System.Windows.Forms.TextBox();
            this.lblEmailRecipients = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(9, 28);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(47, 13);
            this.lblDuration.TabIndex = 0;
            this.lblDuration.Text = "Duration";
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(72, 25);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(62, 20);
            this.txtDuration.TabIndex = 1;
            // 
            // chkFlash
            // 
            this.chkFlash.AutoSize = true;
            this.chkFlash.Location = new System.Drawing.Point(12, 51);
            this.chkFlash.Name = "chkFlash";
            this.chkFlash.Size = new System.Drawing.Size(51, 17);
            this.chkFlash.TabIndex = 3;
            this.chkFlash.Text = "Flash";
            this.chkFlash.UseVisualStyleBackColor = true;
            // 
            // chkEmailImmediately
            // 
            this.chkEmailImmediately.AutoSize = true;
            this.chkEmailImmediately.Location = new System.Drawing.Point(12, 74);
            this.chkEmailImmediately.Name = "chkEmailImmediately";
            this.chkEmailImmediately.Size = new System.Drawing.Size(109, 17);
            this.chkEmailImmediately.TabIndex = 4;
            this.chkEmailImmediately.Text = "Email Immediately";
            this.chkEmailImmediately.UseVisualStyleBackColor = true;
            this.chkEmailImmediately.Visible = false;
            // 
            // chkEmailUntilComplete
            // 
            this.chkEmailUntilComplete.AutoSize = true;
            this.chkEmailUntilComplete.Location = new System.Drawing.Point(12, 97);
            this.chkEmailUntilComplete.Name = "chkEmailUntilComplete";
            this.chkEmailUntilComplete.Size = new System.Drawing.Size(122, 17);
            this.chkEmailUntilComplete.TabIndex = 5;
            this.chkEmailUntilComplete.Text = "Email Until Complete";
            this.chkEmailUntilComplete.UseVisualStyleBackColor = true;
            this.chkEmailUntilComplete.Visible = false;
            // 
            // txtEmailFrequency
            // 
            this.txtEmailFrequency.Location = new System.Drawing.Point(72, 114);
            this.txtEmailFrequency.Name = "txtEmailFrequency";
            this.txtEmailFrequency.Size = new System.Drawing.Size(62, 20);
            this.txtEmailFrequency.TabIndex = 7;
            this.txtEmailFrequency.Visible = false;
            // 
            // lblEmailFrequency
            // 
            this.lblEmailFrequency.AutoSize = true;
            this.lblEmailFrequency.Location = new System.Drawing.Point(9, 117);
            this.lblEmailFrequency.Name = "lblEmailFrequency";
            this.lblEmailFrequency.Size = new System.Drawing.Size(57, 13);
            this.lblEmailFrequency.TabIndex = 6;
            this.lblEmailFrequency.Text = "Frequency";
            this.lblEmailFrequency.Visible = false;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.ContainerControl = this;
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(150, 22);
            this.lblCaption.TabIndex = 9;
            this.lblCaption.Text = "...";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEmailRecipients
            // 
            this.txtEmailRecipients.Location = new System.Drawing.Point(12, 153);
            this.txtEmailRecipients.Name = "txtEmailRecipients";
            this.txtEmailRecipients.Size = new System.Drawing.Size(122, 20);
            this.txtEmailRecipients.TabIndex = 17;
            this.txtEmailRecipients.Visible = false;
            // 
            // lblEmailRecipients
            // 
            this.lblEmailRecipients.AutoSize = true;
            this.lblEmailRecipients.Location = new System.Drawing.Point(9, 137);
            this.lblEmailRecipients.Name = "lblEmailRecipients";
            this.lblEmailRecipients.Size = new System.Drawing.Size(85, 13);
            this.lblEmailRecipients.TabIndex = 16;
            this.lblEmailRecipients.Text = "Email Recipients";
            this.lblEmailRecipients.Visible = false;
            // 
            // CheckpointConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtEmailRecipients);
            this.Controls.Add(this.lblEmailRecipients);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.txtEmailFrequency);
            this.Controls.Add(this.lblEmailFrequency);
            this.Controls.Add(this.chkEmailUntilComplete);
            this.Controls.Add(this.chkEmailImmediately);
            this.Controls.Add(this.chkFlash);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.lblDuration);
            this.Name = "CheckpointConfigurationControl";
            this.Size = new System.Drawing.Size(150, 185);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.CheckBox chkFlash;
        private System.Windows.Forms.CheckBox chkEmailImmediately;
        private System.Windows.Forms.CheckBox chkEmailUntilComplete;
        private System.Windows.Forms.TextBox txtEmailFrequency;
        private System.Windows.Forms.Label lblEmailFrequency;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.TextBox txtEmailRecipients;
        private System.Windows.Forms.Label lblEmailRecipients;
    }
}
