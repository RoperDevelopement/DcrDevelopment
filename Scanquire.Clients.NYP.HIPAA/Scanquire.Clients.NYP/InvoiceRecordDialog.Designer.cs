namespace Scanquire.Clients.NYP
{
    partial class InvoiceRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceRecordDialog));
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.cmbAccount = new System.Windows.Forms.ComboBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.lblInvoiceDateDesc = new System.Windows.Forms.Label();
            this.dpInvoiceDate = new EdocsUSA.Utilities.Controls.DatePicker();
            this.lblReference = new System.Windows.Forms.Label();
            this.lblAccount = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.lblInvoiceDate = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlControls.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(551, 51);
            this.lblTitle.TabIndex = 29;
            this.lblTitle.Text = "Invoices";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnCancel);
            this.pnlControls.Controls.Add(this.btnOk);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 201);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(551, 100);
            this.pnlControls.TabIndex = 30;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(272, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(191, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.cmbCategory);
            this.pnlContent.Controls.Add(this.lblCategory);
            this.pnlContent.Controls.Add(this.txtReference);
            this.pnlContent.Controls.Add(this.cmbAccount);
            this.pnlContent.Controls.Add(this.cmbDepartment);
            this.pnlContent.Controls.Add(this.lblInvoiceDateDesc);
            this.pnlContent.Controls.Add(this.dpInvoiceDate);
            this.pnlContent.Controls.Add(this.lblReference);
            this.pnlContent.Controls.Add(this.lblAccount);
            this.pnlContent.Controls.Add(this.lblDepartment);
            this.pnlContent.Controls.Add(this.lblInvoiceDate);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 51);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(551, 150);
            this.pnlContent.TabIndex = 31;
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "",
            "OTHER",
            "OUTREACH",
            "REF LAB",
            "TRANSPORTATION"});
            this.cmbCategory.Location = new System.Drawing.Point(334, 52);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(121, 21);
            this.cmbCategory.TabIndex = 7;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(260, 55);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.TabIndex = 9;
            this.lblCategory.Text = "Category";
            // 
            // txtReference
            // 
            this.txtReference.Location = new System.Drawing.Point(128, 108);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(174, 20);
            this.txtReference.TabIndex = 9;
            // 
            // cmbAccount
            // 
            this.cmbAccount.FormattingEnabled = true;
            this.cmbAccount.Items.AddRange(new object[] {
            "AMBRY GENETICS",
            "ATHENA DIAGNOSTICS - 77149",
            "BEACON DIAGNOSTICS",
            "BLOOD CENTER OF WISCONSIN",
            "CINCINNATI CHILDRENS HOSPITALS",
            "EDOCS",
            "ESOTERIX - 31900852",
            "GENEDX",
            "GENOPTIX",
            "HISTOGENETICS",
            "INTERPACE",
            "JOHN HOPKINS",
            "LABCORP - 31780450",
            "LABCORP - 31704535",
            "LABCORP - 88580021",
            "MAYO MEDICAL LAB - 7012412",
            "MEDICAL NEUROGENETICS (MNG)",
            "PALO ALTO - 6576",
            "PREVENTION GENETICS",
            "QUEST DIAGNOSTICS - 32902",
            "QUEST DIAGNOSTICS - 37964",
            "REPEAT DIAGNOSTIC",
            "ROGOSIN INSTITUTE ",
            "UNIVERSITY OF IOWA ",
            "UNIVERSITY OF WASHINGTON ",
            "VIRACOR"});
            this.cmbAccount.Location = new System.Drawing.Point(128, 79);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new System.Drawing.Size(327, 21);
            this.cmbAccount.TabIndex = 8;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Items.AddRange(new object[] {
            "",
            "COURIER",
            "CS",
            "K09",
            "L8 SENDOUT",
            "L8 ACCESSIONING",
            "OUTREACH"});
            this.cmbDepartment.Location = new System.Drawing.Point(128, 52);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(121, 21);
            this.cmbDepartment.TabIndex = 6;
            // 
            // lblInvoiceDateDesc
            // 
            this.lblInvoiceDateDesc.AutoSize = true;
            this.lblInvoiceDateDesc.Location = new System.Drawing.Point(247, 28);
            this.lblInvoiceDateDesc.Name = "lblInvoiceDateDesc";
            this.lblInvoiceDateDesc.Size = new System.Drawing.Size(250, 13);
            this.lblInvoiceDateDesc.TabIndex = 5;
            this.lblInvoiceDateDesc.Text = "(For multiple invoices, use the first day of the month)";
            // 
            // dpInvoiceDate
            // 
            this.dpInvoiceDate.DisplayFormat = "MM/dd/yyyy";
            this.dpInvoiceDate.Location = new System.Drawing.Point(128, 25);
            this.dpInvoiceDate.Name = "dpInvoiceDate";
            this.dpInvoiceDate.Size = new System.Drawing.Size(95, 20);
            this.dpInvoiceDate.TabIndex = 4;
            this.dpInvoiceDate.Value = null;
            // 
            // lblReference
            // 
            this.lblReference.AutoSize = true;
            this.lblReference.Location = new System.Drawing.Point(54, 111);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(57, 13);
            this.lblReference.TabIndex = 3;
            this.lblReference.Text = "Reference";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(54, 82);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(47, 13);
            this.lblAccount.TabIndex = 2;
            this.lblAccount.Text = "Account";
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(54, 55);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(62, 13);
            this.lblDepartment.TabIndex = 1;
            this.lblDepartment.Text = "Department";
            // 
            // lblInvoiceDate
            // 
            this.lblInvoiceDate.AutoSize = true;
            this.lblInvoiceDate.Location = new System.Drawing.Point(54, 28);
            this.lblInvoiceDate.Name = "lblInvoiceDate";
            this.lblInvoiceDate.Size = new System.Drawing.Size(68, 13);
            this.lblInvoiceDate.TabIndex = 0;
            this.lblInvoiceDate.Text = "Invoice Date";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // InvoiceRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 301);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.lblTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InvoiceRecordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice Record Dialog";
            this.pnlControls.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.ComboBox cmbAccount;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblInvoiceDateDesc;
        private EdocsUSA.Utilities.Controls.DatePicker dpInvoiceDate;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.Label lblInvoiceDate;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblCategory;
    }
}