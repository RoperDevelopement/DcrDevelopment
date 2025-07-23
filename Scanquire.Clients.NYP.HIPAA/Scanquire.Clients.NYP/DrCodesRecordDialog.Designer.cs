namespace Scanquire.Clients.NYP
{
    partial class DrCodesRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrCodesRecordDialog));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpRecordSettings = new System.Windows.Forms.GroupBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtDrCode = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblDrCode = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkUsePatchCards = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.grpRecordSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // grpRecordSettings
            // 
            this.grpRecordSettings.Controls.Add(this.txtFirstName);
            this.grpRecordSettings.Controls.Add(this.txtLastName);
            this.grpRecordSettings.Controls.Add(this.txtDrCode);
            this.grpRecordSettings.Controls.Add(this.lblFirstName);
            this.grpRecordSettings.Controls.Add(this.lblLastName);
            this.grpRecordSettings.Controls.Add(this.lblDrCode);
            this.grpRecordSettings.Location = new System.Drawing.Point(35, 107);
            this.grpRecordSettings.Name = "grpRecordSettings";
            this.grpRecordSettings.Size = new System.Drawing.Size(414, 138);
            this.grpRecordSettings.TabIndex = 31;
            this.grpRecordSettings.TabStop = false;
            this.grpRecordSettings.Text = "Record Settings";
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(140, 96);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(198, 20);
            this.txtFirstName.TabIndex = 5;
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(140, 67);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(198, 20);
            this.txtLastName.TabIndex = 4;
            // 
            // txtDrCode
            // 
            this.txtDrCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDrCode.Location = new System.Drawing.Point(140, 38);
            this.txtDrCode.Name = "txtDrCode";
            this.txtDrCode.Size = new System.Drawing.Size(100, 20);
            this.txtDrCode.TabIndex = 3;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(77, 99);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 2;
            this.lblFirstName.Text = "First Name";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(76, 70);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 1;
            this.lblLastName.Text = "Last Name";
            // 
            // lblDrCode
            // 
            this.lblDrCode.AutoSize = true;
            this.lblDrCode.Location = new System.Drawing.Point(88, 41);
            this.lblDrCode.Name = "lblDrCode";
            this.lblDrCode.Size = new System.Drawing.Size(46, 13);
            this.lblDrCode.TabIndex = 0;
            this.lblDrCode.Text = "Dr Code";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(114, 31);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(256, 51);
            this.lblTitle.TabIndex = 32;
            this.lblTitle.Text = "DR CODES";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(245, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(164, 285);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 29;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkUsePatchCards
            // 
            this.chkUsePatchCards.AutoSize = true;
            this.chkUsePatchCards.Location = new System.Drawing.Point(189, 257);
            this.chkUsePatchCards.Name = "chkUsePatchCards";
            this.chkUsePatchCards.Size = new System.Drawing.Size(106, 17);
            this.chkUsePatchCards.TabIndex = 33;
            this.chkUsePatchCards.Text = "Use Patch Cards";
            this.chkUsePatchCards.UseVisualStyleBackColor = true;
            this.chkUsePatchCards.CheckedChanged += new System.EventHandler(this.chkUsePatchCards_CheckedChanged);
            // 
            // DrCodesRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 372);
            this.Controls.Add(this.chkUsePatchCards);
            this.Controls.Add(this.grpRecordSettings);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DrCodesRecordDialog";
            this.Text = "Dr Codes Record Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpRecordSettings.ResumeLayout(false);
            this.grpRecordSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox grpRecordSettings;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtDrCode;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblDrCode;
        private System.Windows.Forms.CheckBox chkUsePatchCards;
    }
}