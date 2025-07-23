namespace Scanquire.Clients.NYP
{
    partial class SpecimenRejectionRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecimenRejectionRecordDialog));
            this.lblLogDate = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFormType = new System.Windows.Forms.ComboBox();
            this.txt_caseNumber = new System.Windows.Forms.TextBox();
            this.lblFormType = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.dpLogDate = new EdocsUSA.Utilities.Controls.DatePicker();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlContent.SuspendLayout();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLogDate
            // 
            this.lblLogDate.AutoSize = true;
            this.lblLogDate.Location = new System.Drawing.Point(81, 19);
            this.lblLogDate.Name = "lblLogDate";
            this.lblLogDate.Size = new System.Drawing.Size(58, 13);
            this.lblLogDate.TabIndex = 10;
            this.lblLogDate.Text = "Scan Date";
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.label1);
            this.pnlContent.Controls.Add(this.cmbFormType);
            this.pnlContent.Controls.Add(this.txt_caseNumber);
            this.pnlContent.Controls.Add(this.lblFormType);
            this.pnlContent.Controls.Add(this.lblLastName);
            this.pnlContent.Controls.Add(this.dpLogDate);
            this.pnlContent.Controls.Add(this.lblLogDate);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 51);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(354, 141);
            this.pnlContent.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 15);
            this.label1.TabIndex = 29;
            this.label1.Text = "Add New Reason in Drop Down Press Ok / Hit  Enter";
            // 
            // cmbFormType
            // 
            this.cmbFormType.FormattingEnabled = true;
            this.cmbFormType.Items.AddRange(new object[] {
            "Missing Source",
            "Source Discrepancy\t",
            "Incorrectly Labeled Specimen",
            "Date of Birth Issue",
            "No Patient History",
            "Unlabeled Specimen",
            "Duplicate Specimens",
            "Site Verification Issue"});
            this.cmbFormType.Location = new System.Drawing.Point(144, 76);
            this.cmbFormType.Name = "cmbFormType";
            this.cmbFormType.Size = new System.Drawing.Size(170, 21);
            this.cmbFormType.TabIndex = 3;
            this.cmbFormType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbFormType_KeyPress);
            // 
            // txt_caseNumber
            // 
            this.txt_caseNumber.Location = new System.Drawing.Point(144, 47);
            this.txt_caseNumber.Name = "txt_caseNumber";
            this.txt_caseNumber.Size = new System.Drawing.Size(121, 20);
            this.txt_caseNumber.TabIndex = 2;
            this.txt_caseNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_caseNumber_KeyPress);
            // 
            // lblFormType
            // 
            this.lblFormType.AutoSize = true;
            this.lblFormType.Location = new System.Drawing.Point(88, 80);
            this.lblFormType.Name = "lblFormType";
            this.lblFormType.Size = new System.Drawing.Size(44, 13);
            this.lblFormType.TabIndex = 25;
            this.lblFormType.Text = "Reason";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(69, 51);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(71, 13);
            this.lblLastName.TabIndex = 23;
            this.lblLastName.Text = "Case Number";
            // 
            // dpLogDate
            // 
            this.dpLogDate.DisplayFormat = "MM/dd/yyyy";
            this.dpLogDate.Location = new System.Drawing.Point(144, 16);
            this.dpLogDate.Margin = new System.Windows.Forms.Padding(4);
            this.dpLogDate.Name = "dpLogDate";
            this.dpLogDate.Size = new System.Drawing.Size(95, 20);
            this.dpLogDate.TabIndex = 1;
            this.dpLogDate.Value = null;
         
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(354, 51);
            this.lblTitle.TabIndex = 35;
            this.lblTitle.Text = "Rejection Logs";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnCancel);
            this.pnlControls.Controls.Add(this.btnOk);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 191);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(354, 116);
            this.pnlControls.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(189, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(83, 15);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // SpecimenRejectionRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 307);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpecimenRejectionRecordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rejection LogRecord Dialog";
            this.Load += new System.EventHandler(this.SpecimenRejectionRecordDialog_Load);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private EdocsUSA.Utilities.Controls.DatePicker dpLogDate;
        private System.Windows.Forms.Label lblLogDate;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cmbFormType;
        private System.Windows.Forms.TextBox txt_caseNumber;
        private System.Windows.Forms.Label lblFormType;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label label1;
    }
}