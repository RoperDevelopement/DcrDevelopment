namespace Scanquire.Clients.NYP
{
    partial class DOHRecordDialogOLD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DOHRecordDialogOLD));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpRecordSettings = new System.Windows.Forms.GroupBox();
            this.dpDateOfService = new EdocsUSA.Utilities.Controls.DatePicker();
            this.lblPerformingLab = new System.Windows.Forms.Label();
            this.cmbPerformingLab = new System.Windows.Forms.ComboBox();
            this.lblAccessionNumber = new System.Windows.Forms.Label();
            this.lblMedicalRecordNumber = new System.Windows.Forms.Label();
            this.txtMedicalRecordNumber = new System.Windows.Forms.TextBox();
            this.lblDateOfService = new System.Windows.Forms.Label();
            this.txtAccessionNumber = new System.Windows.Forms.TextBox();
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
            this.grpRecordSettings.Controls.Add(this.dpDateOfService);
            this.grpRecordSettings.Controls.Add(this.lblPerformingLab);
            this.grpRecordSettings.Controls.Add(this.cmbPerformingLab);
            this.grpRecordSettings.Controls.Add(this.lblAccessionNumber);
            this.grpRecordSettings.Controls.Add(this.lblMedicalRecordNumber);
            this.grpRecordSettings.Controls.Add(this.txtMedicalRecordNumber);
            this.grpRecordSettings.Controls.Add(this.lblDateOfService);
            this.grpRecordSettings.Controls.Add(this.txtAccessionNumber);
            this.grpRecordSettings.Location = new System.Drawing.Point(39, 68);
            this.grpRecordSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRecordSettings.Name = "grpRecordSettings";
            this.grpRecordSettings.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRecordSettings.Size = new System.Drawing.Size(552, 158);
            this.grpRecordSettings.TabIndex = 21;
            this.grpRecordSettings.TabStop = false;
            this.grpRecordSettings.Text = "Record Settings";
            // 
            // dpDateOfService
            // 
            this.dpDateOfService.DisplayFormat = "MM/dd/yyyy";
            this.dpDateOfService.Location = new System.Drawing.Point(185, 113);
            this.dpDateOfService.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dpDateOfService.Name = "dpDateOfService";
            this.dpDateOfService.Size = new System.Drawing.Size(157, 25);
            this.dpDateOfService.TabIndex = 11;
            this.dpDateOfService.Value = null;
            // 
            // lblPerformingLab
            // 
            this.lblPerformingLab.AutoSize = true;
            this.lblPerformingLab.Location = new System.Drawing.Point(73, 20);
            this.lblPerformingLab.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPerformingLab.Name = "lblPerformingLab";
            this.lblPerformingLab.Size = new System.Drawing.Size(105, 17);
            this.lblPerformingLab.TabIndex = 1;
            this.lblPerformingLab.Text = "Performing Lab";
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
            "ESOTERIX",
            "GENOPTIX",
            "HISTOGENETICS",
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
            this.cmbPerformingLab.Location = new System.Drawing.Point(185, 16);
            this.cmbPerformingLab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbPerformingLab.Name = "cmbPerformingLab";
            this.cmbPerformingLab.Size = new System.Drawing.Size(325, 24);
            this.cmbPerformingLab.TabIndex = 0;
            // 
            // lblAccessionNumber
            // 
            this.lblAccessionNumber.AutoSize = true;
            this.lblAccessionNumber.Location = new System.Drawing.Point(49, 53);
            this.lblAccessionNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccessionNumber.Name = "lblAccessionNumber";
            this.lblAccessionNumber.Size = new System.Drawing.Size(126, 17);
            this.lblAccessionNumber.TabIndex = 2;
            this.lblAccessionNumber.Text = "Accession Number";
            // 
            // lblMedicalRecordNumber
            // 
            this.lblMedicalRecordNumber.AutoSize = true;
            this.lblMedicalRecordNumber.Location = new System.Drawing.Point(15, 85);
            this.lblMedicalRecordNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMedicalRecordNumber.Name = "lblMedicalRecordNumber";
            this.lblMedicalRecordNumber.Size = new System.Drawing.Size(160, 17);
            this.lblMedicalRecordNumber.TabIndex = 4;
            this.lblMedicalRecordNumber.Text = "Medical Record Number";
            // 
            // txtMedicalRecordNumber
            // 
            this.txtMedicalRecordNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMedicalRecordNumber.Location = new System.Drawing.Point(185, 81);
            this.txtMedicalRecordNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMedicalRecordNumber.Name = "txtMedicalRecordNumber";
            this.txtMedicalRecordNumber.Size = new System.Drawing.Size(265, 22);
            this.txtMedicalRecordNumber.TabIndex = 10;
            // 
            // lblDateOfService
            // 
            this.lblDateOfService.AutoSize = true;
            this.lblDateOfService.Location = new System.Drawing.Point(67, 121);
            this.lblDateOfService.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDateOfService.Name = "lblDateOfService";
            this.lblDateOfService.Size = new System.Drawing.Size(108, 17);
            this.lblDateOfService.TabIndex = 7;
            this.lblDateOfService.Text = "Date Of Service";
            // 
            // txtAccessionNumber
            // 
            this.txtAccessionNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAccessionNumber.Location = new System.Drawing.Point(185, 49);
            this.txtAccessionNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAccessionNumber.Name = "txtAccessionNumber";
            this.txtAccessionNumber.Size = new System.Drawing.Size(265, 22);
            this.txtAccessionNumber.TabIndex = 8;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(172, 9);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(283, 63);
            this.lblTitle.TabIndex = 23;
            this.lblTitle.Text = "DOH-OLD";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(315, 277);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 92);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(207, 277);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 92);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkUsePatchCards
            // 
            this.chkUsePatchCards.AutoSize = true;
            this.chkUsePatchCards.Location = new System.Drawing.Point(240, 249);
            this.chkUsePatchCards.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUsePatchCards.Name = "chkUsePatchCards";
            this.chkUsePatchCards.Size = new System.Drawing.Size(136, 21);
            this.chkUsePatchCards.TabIndex = 22;
            this.chkUsePatchCards.Text = "Use Patch Cards";
            this.chkUsePatchCards.UseVisualStyleBackColor = true;
            this.chkUsePatchCards.CheckedChanged += new System.EventHandler(this.chkUsePatchCards_CheckedChanged);
            // 
            // DOHRecordDialogOLD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 421);
            this.Controls.Add(this.grpRecordSettings);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkUsePatchCards);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DOHRecordDialogOLD";
            this.Text = "DOH Record Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpRecordSettings.ResumeLayout(false);
            this.grpRecordSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox grpRecordSettings;
        private System.Windows.Forms.Label lblPerformingLab;
        private System.Windows.Forms.ComboBox cmbPerformingLab;
        private System.Windows.Forms.Label lblAccessionNumber;
        private System.Windows.Forms.Label lblMedicalRecordNumber;
        private System.Windows.Forms.TextBox txtMedicalRecordNumber;
        private System.Windows.Forms.Label lblDateOfService;
        private System.Windows.Forms.TextBox txtAccessionNumber;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkUsePatchCards;
        private EdocsUSA.Utilities.Controls.DatePicker dpDateOfService;
    }
}