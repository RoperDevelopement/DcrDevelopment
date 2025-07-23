namespace Scanquire.Clients.NYP
{
    partial class PunchFormsRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PunchFormsRecordDialog));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpRecordSettings = new System.Windows.Forms.GroupBox();
            this.dpLogDate = new EdocsUSA.Utilities.Controls.DatePicker();
            this.lblLogDate = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
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
            this.grpRecordSettings.Controls.Add(this.dpLogDate);
            this.grpRecordSettings.Controls.Add(this.lblLogDate);
            this.grpRecordSettings.Controls.Add(this.lblLocation);
            this.grpRecordSettings.Controls.Add(this.cmbLocation);
            this.grpRecordSettings.Location = new System.Drawing.Point(18, 85);
            this.grpRecordSettings.Name = "grpRecordSettings";
            this.grpRecordSettings.Size = new System.Drawing.Size(414, 128);
            this.grpRecordSettings.TabIndex = 26;
            this.grpRecordSettings.TabStop = false;
            this.grpRecordSettings.Text = "Record Settings";
            // 
            // dpLogDate
            // 
            this.dpLogDate.DisplayFormat = "MM/dd/yyyy";
            this.dpLogDate.Location = new System.Drawing.Point(112, 55);
            this.dpLogDate.Name = "dpLogDate";
            this.dpLogDate.Size = new System.Drawing.Size(95, 20);
            this.dpLogDate.TabIndex = 13;
            this.dpLogDate.Value = null;
            // 
            // lblLogDate
            // 
            this.lblLogDate.AutoSize = true;
            this.lblLogDate.Location = new System.Drawing.Point(55, 58);
            this.lblLogDate.Name = "lblLogDate";
            this.lblLogDate.Size = new System.Drawing.Size(51, 13);
            this.lblLogDate.TabIndex = 12;
            this.lblLogDate.Text = "Log Date";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(58, 32);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(48, 13);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location";
            // 
            // cmbLocation
            // 
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Items.AddRange(new object[] {
            "",
            "Client Services",
            "Patient Service Center - K09",
            "Logistics",
            "Specimen Processing"});
            this.cmbLocation.Location = new System.Drawing.Point(112, 29);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(245, 21);
            this.cmbLocation.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(67, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(618, 51);
            this.lblTitle.TabIndex = 28;
            this.lblTitle.Text = "Missed Punch Forms By DOS";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(228, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(147, 233);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 24;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // PunchFormsRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(450, 336);
            this.Controls.Add(this.grpRecordSettings);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PunchFormsRecordDialog";
            this.Text = "Punch Forms Record Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpRecordSettings.ResumeLayout(false);
            this.grpRecordSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox grpRecordSettings;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private EdocsUSA.Utilities.Controls.DatePicker dpLogDate;
        private System.Windows.Forms.Label lblLogDate;
    }
}