namespace DemoArchivers
{
    partial class WorkersCompRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkersCompRecordDialog));
            this.dpFormDate = new EdocsUSA.Utilities.Controls.DatePicker();
            this.cmbFormType = new System.Windows.Forms.ComboBox();
            this.txtClientName = new System.Windows.Forms.TextBox();
            this.lblFormDate = new System.Windows.Forms.Label();
            this.lblFormType = new System.Windows.Forms.Label();
            this.lblClientName = new System.Windows.Forms.Label();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.txtCaseNumber = new System.Windows.Forms.TextBox();
            this.lblCaseNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dpFormDate
            // 
            this.dpFormDate.DisplayFormat = "MM/dd/yyyy";
            this.dpFormDate.Location = new System.Drawing.Point(114, 111);
            this.dpFormDate.Name = "dpFormDate";
            this.dpFormDate.Size = new System.Drawing.Size(95, 20);
            this.dpFormDate.TabIndex = 4;
            this.dpFormDate.Value = null;
            // 
            // cmbFormType
            // 
            this.cmbFormType.FormattingEnabled = true;
            this.cmbFormType.Items.AddRange(new object[] {
            "",
            "BILLING",
            "CONTRACT",
            "EVIDENCE",
            "MISC",
            "NONDISCLOSURE"});
            this.cmbFormType.Location = new System.Drawing.Point(114, 84);
            this.cmbFormType.Name = "cmbFormType";
            this.cmbFormType.Size = new System.Drawing.Size(121, 21);
            this.cmbFormType.TabIndex = 3;
            // 
            // txtClientName
            // 
            this.txtClientName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtClientName.Location = new System.Drawing.Point(114, 32);
            this.txtClientName.Name = "txtClientName";
            this.txtClientName.Size = new System.Drawing.Size(121, 20);
            this.txtClientName.TabIndex = 1;
            // 
            // lblFormDate
            // 
            this.lblFormDate.AutoSize = true;
            this.lblFormDate.Location = new System.Drawing.Point(52, 115);
            this.lblFormDate.Name = "lblFormDate";
            this.lblFormDate.Size = new System.Drawing.Size(56, 13);
            this.lblFormDate.TabIndex = 21;
            this.lblFormDate.Text = "Form Date";
            // 
            // lblFormType
            // 
            this.lblFormType.AutoSize = true;
            this.lblFormType.Location = new System.Drawing.Point(51, 87);
            this.lblFormType.Name = "lblFormType";
            this.lblFormType.Size = new System.Drawing.Size(57, 13);
            this.lblFormType.TabIndex = 20;
            this.lblFormType.Text = "Form Type";
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(44, 35);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(64, 13);
            this.lblClientName.TabIndex = 18;
            this.lblClientName.Text = "Client Name";
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(151, 174);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(75, 75);
            this.CancelDialogButton.TabIndex = 17;
            this.CancelDialogButton.TabStop = false;
            this.CancelDialogButton.Text = "&Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(58, 174);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 75);
            this.SubmitButton.TabIndex = 5;
            this.SubmitButton.Text = "&Ok";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click_1);
            // 
            // txtCaseNumber
            // 
            this.txtCaseNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCaseNumber.Location = new System.Drawing.Point(114, 55);
            this.txtCaseNumber.Name = "txtCaseNumber";
            this.txtCaseNumber.Size = new System.Drawing.Size(121, 20);
            this.txtCaseNumber.TabIndex = 2;
            // 
            // lblCaseNumber
            // 
            this.lblCaseNumber.AutoSize = true;
            this.lblCaseNumber.Location = new System.Drawing.Point(37, 58);
            this.lblCaseNumber.Name = "lblCaseNumber";
            this.lblCaseNumber.Size = new System.Drawing.Size(71, 13);
            this.lblCaseNumber.TabIndex = 23;
            this.lblCaseNumber.Text = "Case Number";
            // 
            // WorkersCompRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtCaseNumber);
            this.Controls.Add(this.lblCaseNumber);
            this.Controls.Add(this.dpFormDate);
            this.Controls.Add(this.cmbFormType);
            this.Controls.Add(this.txtClientName);
            this.Controls.Add(this.lblFormDate);
            this.Controls.Add(this.lblFormType);
            this.Controls.Add(this.lblClientName);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.SubmitButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorkersCompRecordDialog";
            this.Text = "Cases";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EdocsUSA.Utilities.Controls.DatePicker dpFormDate;
        private System.Windows.Forms.ComboBox cmbFormType;
        private System.Windows.Forms.TextBox txtClientName;
        private System.Windows.Forms.Label lblFormDate;
        private System.Windows.Forms.Label lblFormType;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.TextBox txtCaseNumber;
        private System.Windows.Forms.Label lblCaseNumber;
    }
}