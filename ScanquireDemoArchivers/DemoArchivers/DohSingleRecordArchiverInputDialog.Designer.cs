namespace DemoArchivers
{
    partial class DohSingleRecordArchiverInputDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DohSingleRecordArchiverInputDialog));
            this.SharepointCredentialsGroupBox = new System.Windows.Forms.GroupBox();
            this.SharepointCredentialsControl = new DemoArchivers.SharepointRestCredentials();
            this.RecordSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.FinancialNumberTextBox = new System.Windows.Forms.TextBox();
            this.AccessionNumberTextBox = new System.Windows.Forms.TextBox();
            this.DateOfServicePicker = new System.Windows.Forms.DateTimePicker();
            this.DateOfServiceLabel = new System.Windows.Forms.Label();
            this.FinancialNumberLabel = new System.Windows.Forms.Label();
            this.AccessionNumberLabel = new System.Windows.Forms.Label();
            this.PerformingLabComboBox = new System.Windows.Forms.ComboBox();
            this.PerformingLabLabel = new System.Windows.Forms.Label();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MedicalRecordNumberTextBox = new System.Windows.Forms.TextBox();
            this.MedicalRecordLabel = new System.Windows.Forms.Label();
            this.SharepointCredentialsGroupBox.SuspendLayout();
            this.RecordSettingsGroupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SharepointCredentialsGroupBox
            // 
            this.SharepointCredentialsGroupBox.Controls.Add(this.SharepointCredentialsControl);
            this.SharepointCredentialsGroupBox.Location = new System.Drawing.Point(22, 12);
            this.SharepointCredentialsGroupBox.Name = "SharepointCredentialsGroupBox";
            this.SharepointCredentialsGroupBox.Size = new System.Drawing.Size(421, 146);
            this.SharepointCredentialsGroupBox.TabIndex = 4;
            this.SharepointCredentialsGroupBox.TabStop = false;
            this.SharepointCredentialsGroupBox.Text = "Sharepoint Settings";
            // 
            // SharepointCredentialsControl
            // 
            this.SharepointCredentialsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SharepointCredentialsControl.Domain = "";
            this.SharepointCredentialsControl.Location = new System.Drawing.Point(3, 16);
            this.SharepointCredentialsControl.Name = "SharepointCredentialsControl";
            this.SharepointCredentialsControl.Password = "";
            this.SharepointCredentialsControl.ServerAddress = "";
            this.SharepointCredentialsControl.Size = new System.Drawing.Size(415, 127);
            this.SharepointCredentialsControl.TabIndex = 0;
            this.SharepointCredentialsControl.UserName = "";
            // 
            // RecordSettingsGroupBox
            // 
            this.RecordSettingsGroupBox.Controls.Add(this.MedicalRecordNumberTextBox);
            this.RecordSettingsGroupBox.Controls.Add(this.MedicalRecordLabel);
            this.RecordSettingsGroupBox.Controls.Add(this.FinancialNumberTextBox);
            this.RecordSettingsGroupBox.Controls.Add(this.AccessionNumberTextBox);
            this.RecordSettingsGroupBox.Controls.Add(this.DateOfServicePicker);
            this.RecordSettingsGroupBox.Controls.Add(this.DateOfServiceLabel);
            this.RecordSettingsGroupBox.Controls.Add(this.FinancialNumberLabel);
            this.RecordSettingsGroupBox.Controls.Add(this.AccessionNumberLabel);
            this.RecordSettingsGroupBox.Controls.Add(this.PerformingLabComboBox);
            this.RecordSettingsGroupBox.Controls.Add(this.PerformingLabLabel);
            this.RecordSettingsGroupBox.Location = new System.Drawing.Point(25, 164);
            this.RecordSettingsGroupBox.Name = "RecordSettingsGroupBox";
            this.RecordSettingsGroupBox.Size = new System.Drawing.Size(418, 209);
            this.RecordSettingsGroupBox.TabIndex = 5;
            this.RecordSettingsGroupBox.TabStop = false;
            this.RecordSettingsGroupBox.Text = "Record Settings";
            // 
            // FinancialNumberTextBox
            // 
            this.FinancialNumberTextBox.Location = new System.Drawing.Point(149, 100);
            this.FinancialNumberTextBox.Name = "FinancialNumberTextBox";
            this.FinancialNumberTextBox.Size = new System.Drawing.Size(248, 20);
            this.FinancialNumberTextBox.TabIndex = 8;
            // 
            // AccessionNumberTextBox
            // 
            this.AccessionNumberTextBox.Location = new System.Drawing.Point(149, 72);
            this.AccessionNumberTextBox.Name = "AccessionNumberTextBox";
            this.AccessionNumberTextBox.Size = new System.Drawing.Size(248, 20);
            this.AccessionNumberTextBox.TabIndex = 7;
            // 
            // DateOfServicePicker
            // 
            this.DateOfServicePicker.Location = new System.Drawing.Point(149, 150);
            this.DateOfServicePicker.Name = "DateOfServicePicker";
            this.DateOfServicePicker.Size = new System.Drawing.Size(200, 20);
            this.DateOfServicePicker.TabIndex = 5;
            // 
            // DateOfServiceLabel
            // 
            this.DateOfServiceLabel.AutoSize = true;
            this.DateOfServiceLabel.Location = new System.Drawing.Point(60, 156);
            this.DateOfServiceLabel.Name = "DateOfServiceLabel";
            this.DateOfServiceLabel.Size = new System.Drawing.Size(83, 13);
            this.DateOfServiceLabel.TabIndex = 4;
            this.DateOfServiceLabel.Text = "Date Of Service";
            // 
            // FinancialNumberLabel
            // 
            this.FinancialNumberLabel.AutoSize = true;
            this.FinancialNumberLabel.Location = new System.Drawing.Point(54, 103);
            this.FinancialNumberLabel.Name = "FinancialNumberLabel";
            this.FinancialNumberLabel.Size = new System.Drawing.Size(89, 13);
            this.FinancialNumberLabel.TabIndex = 3;
            this.FinancialNumberLabel.Text = "Financial Number";
            // 
            // AccessionNumberLabel
            // 
            this.AccessionNumberLabel.AutoSize = true;
            this.AccessionNumberLabel.Location = new System.Drawing.Point(47, 75);
            this.AccessionNumberLabel.Name = "AccessionNumberLabel";
            this.AccessionNumberLabel.Size = new System.Drawing.Size(96, 13);
            this.AccessionNumberLabel.TabIndex = 2;
            this.AccessionNumberLabel.Text = "Accession Number";
            // 
            // PerformingLabComboBox
            // 
            this.PerformingLabComboBox.FormattingEnabled = true;
            this.PerformingLabComboBox.Location = new System.Drawing.Point(149, 39);
            this.PerformingLabComboBox.Name = "PerformingLabComboBox";
            this.PerformingLabComboBox.Size = new System.Drawing.Size(248, 21);
            this.PerformingLabComboBox.TabIndex = 1;
            // 
            // PerformingLabLabel
            // 
            this.PerformingLabLabel.AutoSize = true;
            this.PerformingLabLabel.Location = new System.Drawing.Point(65, 42);
            this.PerformingLabLabel.Name = "PerformingLabLabel";
            this.PerformingLabLabel.Size = new System.Drawing.Size(78, 13);
            this.PerformingLabLabel.TabIndex = 0;
            this.PerformingLabLabel.Text = "Performing Lab";
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(241, 379);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(75, 75);
            this.CancelDialogButton.TabIndex = 7;
            this.CancelDialogButton.Text = "&Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(148, 379);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 75);
            this.SubmitButton.TabIndex = 6;
            this.SubmitButton.Text = "&Ok";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 470);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(464, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(16, 17);
            this.StatusLabel.Text = "...";
            // 
            // MedicalRecordNumberTextBox
            // 
            this.MedicalRecordNumberTextBox.Location = new System.Drawing.Point(149, 126);
            this.MedicalRecordNumberTextBox.Name = "MedicalRecordNumberTextBox";
            this.MedicalRecordNumberTextBox.Size = new System.Drawing.Size(248, 20);
            this.MedicalRecordNumberTextBox.TabIndex = 10;
            // 
            // MedicalRecordLabel
            // 
            this.MedicalRecordLabel.AutoSize = true;
            this.MedicalRecordLabel.Location = new System.Drawing.Point(21, 129);
            this.MedicalRecordLabel.Name = "MedicalRecordLabel";
            this.MedicalRecordLabel.Size = new System.Drawing.Size(122, 13);
            this.MedicalRecordLabel.TabIndex = 9;
            this.MedicalRecordLabel.Text = "Medical Record Number";
            // 
            // DohSingleRecordArchiverInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 492);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.RecordSettingsGroupBox);
            this.Controls.Add(this.SharepointCredentialsGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DohSingleRecordArchiverInputDialog";
            this.Text = "DOH - Single Record";
            this.SharepointCredentialsGroupBox.ResumeLayout(false);
            this.RecordSettingsGroupBox.ResumeLayout(false);
            this.RecordSettingsGroupBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox SharepointCredentialsGroupBox;
        private SharepointRestCredentials SharepointCredentialsControl;
        private System.Windows.Forms.GroupBox RecordSettingsGroupBox;
        private System.Windows.Forms.TextBox FinancialNumberTextBox;
        private System.Windows.Forms.TextBox AccessionNumberTextBox;
        private System.Windows.Forms.DateTimePicker DateOfServicePicker;
        private System.Windows.Forms.Label DateOfServiceLabel;
        private System.Windows.Forms.Label FinancialNumberLabel;
        private System.Windows.Forms.Label AccessionNumberLabel;
        private System.Windows.Forms.ComboBox PerformingLabComboBox;
        private System.Windows.Forms.Label PerformingLabLabel;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.TextBox MedicalRecordNumberTextBox;
        private System.Windows.Forms.Label MedicalRecordLabel;
    }
}