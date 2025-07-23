namespace Scanquire.Clients.NYP
{
    partial class SendoutRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendoutRecordDialog));
            this.cmbPerformingLab = new System.Windows.Forms.ComboBox();
            this.lblPerformingLab = new System.Windows.Forms.Label();
            this.lblAccessionNumber = new System.Windows.Forms.Label();
            this.lblFinancialNumber = new System.Windows.Forms.Label();
            this.lblMedicalRecordNumber = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblDateOfService = new System.Windows.Forms.Label();
            this.txtAccessionNumber = new System.Windows.Forms.TextBox();
            this.txtFinancialNumber = new System.Windows.Forms.TextBox();
            this.txtMedicalRecordNumber = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpRecordSettings = new System.Windows.Forms.GroupBox();
            this.dpDateOfService = new EdocsUSA.Utilities.Controls.DatePicker();
            this.chkUsePatchCards = new System.Windows.Forms.CheckBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpRecordSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPerformingLab
            // 
            this.cmbPerformingLab.FormattingEnabled = true;
            this.cmbPerformingLab.Items.AddRange(new object[] {
            "",
            "AMBRY GENETICS",
            "ARUP",
            "ATHENA DIAGNOSTICS",
            "BLOOD CENTER OF WISCONSIN",
            "CINCINNATI CHILDRENS HOSPITALS",
            "COLUMBIA - IRVING INSTITUTE",
            "ESOTERIX",
            "GENOPTIX",
            "HISTOGENETICS",
            "INTEGRATED GENETICS",
            "INTERPACE",
            "JOHN HOPKINS",
            "LABCORP",
            "MAYO MEDICAL LABORATORIES",
            "PALO ALTO",
            "PREVENTION GENETICS",
            "PROMETHEUS LABORATORY",
            "QUEST DIAGNOSTICS",
            "REPEAT DIAGNOSTIC",
            "ROGOSIN INSTITUTE",
            "UNIVERSITY OF ROCHESTER",
            "UNIVERSITY OF WASHINGTON",
            "VIRACOR"});
            this.cmbPerformingLab.Location = new System.Drawing.Point(139, 13);
            this.cmbPerformingLab.Name = "cmbPerformingLab";
            this.cmbPerformingLab.Size = new System.Drawing.Size(245, 21);
            this.cmbPerformingLab.TabIndex = 0;
            // 
            // lblPerformingLab
            // 
            this.lblPerformingLab.AutoSize = true;
            this.lblPerformingLab.Location = new System.Drawing.Point(55, 16);
            this.lblPerformingLab.Name = "lblPerformingLab";
            this.lblPerformingLab.Size = new System.Drawing.Size(78, 13);
            this.lblPerformingLab.TabIndex = 1;
            this.lblPerformingLab.Text = "Performing Lab";
            // 
            // lblAccessionNumber
            // 
            this.lblAccessionNumber.AutoSize = true;
            this.lblAccessionNumber.Location = new System.Drawing.Point(37, 43);
            this.lblAccessionNumber.Name = "lblAccessionNumber";
            this.lblAccessionNumber.Size = new System.Drawing.Size(96, 13);
            this.lblAccessionNumber.TabIndex = 2;
            this.lblAccessionNumber.Text = "Accession Number";
            // 
            // lblFinancialNumber
            // 
            this.lblFinancialNumber.AutoSize = true;
            this.lblFinancialNumber.Location = new System.Drawing.Point(44, 70);
            this.lblFinancialNumber.Name = "lblFinancialNumber";
            this.lblFinancialNumber.Size = new System.Drawing.Size(89, 13);
            this.lblFinancialNumber.TabIndex = 3;
            this.lblFinancialNumber.Text = "Financial Number";
            // 
            // lblMedicalRecordNumber
            // 
            this.lblMedicalRecordNumber.AutoSize = true;
            this.lblMedicalRecordNumber.Location = new System.Drawing.Point(11, 97);
            this.lblMedicalRecordNumber.Name = "lblMedicalRecordNumber";
            this.lblMedicalRecordNumber.Size = new System.Drawing.Size(122, 13);
            this.lblMedicalRecordNumber.TabIndex = 4;
            this.lblMedicalRecordNumber.Text = "Medical Record Number";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(75, 124);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 5;
            this.lblLastName.Text = "Last Name";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(76, 151);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 6;
            this.lblFirstName.Text = "First Name";
            // 
            // lblDateOfService
            // 
            this.lblDateOfService.AutoSize = true;
            this.lblDateOfService.Location = new System.Drawing.Point(50, 178);
            this.lblDateOfService.Name = "lblDateOfService";
            this.lblDateOfService.Size = new System.Drawing.Size(83, 13);
            this.lblDateOfService.TabIndex = 7;
            this.lblDateOfService.Text = "Date Of Service";
            // 
            // txtAccessionNumber
            // 
            this.txtAccessionNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAccessionNumber.Location = new System.Drawing.Point(139, 40);
            this.txtAccessionNumber.Name = "txtAccessionNumber";
            this.txtAccessionNumber.Size = new System.Drawing.Size(200, 20);
            this.txtAccessionNumber.TabIndex = 8;
            // 
            // txtFinancialNumber
            // 
            this.txtFinancialNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFinancialNumber.Location = new System.Drawing.Point(139, 67);
            this.txtFinancialNumber.Name = "txtFinancialNumber";
            this.txtFinancialNumber.Size = new System.Drawing.Size(200, 20);
            this.txtFinancialNumber.TabIndex = 9;
            // 
            // txtMedicalRecordNumber
            // 
            this.txtMedicalRecordNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMedicalRecordNumber.Location = new System.Drawing.Point(139, 94);
            this.txtMedicalRecordNumber.Name = "txtMedicalRecordNumber";
            this.txtMedicalRecordNumber.Size = new System.Drawing.Size(200, 20);
            this.txtMedicalRecordNumber.TabIndex = 10;
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(139, 121);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(200, 20);
            this.txtLastName.TabIndex = 11;
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(139, 148);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(200, 20);
            this.txtFirstName.TabIndex = 12;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(141, 320);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(222, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpRecordSettings
            // 
            this.grpRecordSettings.Controls.Add(this.dpDateOfService);
            this.grpRecordSettings.Controls.Add(this.lblPerformingLab);
            this.grpRecordSettings.Controls.Add(this.cmbPerformingLab);
            this.grpRecordSettings.Controls.Add(this.lblAccessionNumber);
            this.grpRecordSettings.Controls.Add(this.lblFinancialNumber);
            this.grpRecordSettings.Controls.Add(this.txtFirstName);
            this.grpRecordSettings.Controls.Add(this.lblMedicalRecordNumber);
            this.grpRecordSettings.Controls.Add(this.txtLastName);
            this.grpRecordSettings.Controls.Add(this.lblLastName);
            this.grpRecordSettings.Controls.Add(this.txtMedicalRecordNumber);
            this.grpRecordSettings.Controls.Add(this.lblFirstName);
            this.grpRecordSettings.Controls.Add(this.txtFinancialNumber);
            this.grpRecordSettings.Controls.Add(this.lblDateOfService);
            this.grpRecordSettings.Controls.Add(this.txtAccessionNumber);
            this.grpRecordSettings.Location = new System.Drawing.Point(12, 63);
            this.grpRecordSettings.Name = "grpRecordSettings";
            this.grpRecordSettings.Size = new System.Drawing.Size(414, 212);
            this.grpRecordSettings.TabIndex = 16;
            this.grpRecordSettings.TabStop = false;
            this.grpRecordSettings.Text = "Record Settings";
            // 
            // dpDateOfService
            // 
            this.dpDateOfService.DisplayFormat = "MM/dd/yyyy";
            this.dpDateOfService.Location = new System.Drawing.Point(139, 174);
            this.dpDateOfService.Name = "dpDateOfService";
            this.dpDateOfService.Size = new System.Drawing.Size(95, 20);
            this.dpDateOfService.TabIndex = 13;
            this.dpDateOfService.Value = null;
            // 
            // chkUsePatchCards
            // 
            this.chkUsePatchCards.AutoSize = true;
            this.chkUsePatchCards.Location = new System.Drawing.Point(166, 297);
            this.chkUsePatchCards.Name = "chkUsePatchCards";
            this.chkUsePatchCards.Size = new System.Drawing.Size(106, 17);
            this.chkUsePatchCards.TabIndex = 17;
            this.chkUsePatchCards.Text = "Use Patch Cards";
            this.chkUsePatchCards.UseVisualStyleBackColor = true;
            this.chkUsePatchCards.CheckedChanged += new System.EventHandler(this.chkUsePatchCards_CheckedChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(100, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(239, 51);
            this.lblTitle.TabIndex = 18;
            this.lblTitle.Text = "SENDOUT";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // SendoutRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 420);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.chkUsePatchCards);
            this.Controls.Add(this.grpRecordSettings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SendoutRecordDialog";
            this.Text = "Sendout Record Dialog";
            this.Load += new System.EventHandler(this.SendoutRecordDialog_Load);
            this.grpRecordSettings.ResumeLayout(false);
            this.grpRecordSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPerformingLab;
        private System.Windows.Forms.Label lblPerformingLab;
        private System.Windows.Forms.Label lblAccessionNumber;
        private System.Windows.Forms.Label lblFinancialNumber;
        private System.Windows.Forms.Label lblMedicalRecordNumber;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblDateOfService;
        private System.Windows.Forms.TextBox txtAccessionNumber;
        private System.Windows.Forms.TextBox txtFinancialNumber;
        private System.Windows.Forms.TextBox txtMedicalRecordNumber;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpRecordSettings;
        private System.Windows.Forms.CheckBox chkUsePatchCards;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private EdocsUSA.Utilities.Controls.DatePicker dpDateOfService;
    }
}