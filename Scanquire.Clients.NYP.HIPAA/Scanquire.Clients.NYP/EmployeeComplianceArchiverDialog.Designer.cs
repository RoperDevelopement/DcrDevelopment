namespace Scanquire.Clients.NYP
{
    partial class EmployeeComplianceArchiverDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeComplianceArchiverDialog));
            this.pnlContent = new System.Windows.Forms.Panel();
            this.job_title_dd = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.document_type_dd = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_last_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.department_dd = new System.Windows.Forms.ComboBox();
            this.txt_id_number = new System.Windows.Forms.TextBox();
            this.txt_first_name = new System.Windows.Forms.TextBox();
            this.lblFormType = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
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
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.job_title_dd);
            this.pnlContent.Controls.Add(this.label3);
            this.pnlContent.Controls.Add(this.document_type_dd);
            this.pnlContent.Controls.Add(this.label2);
            this.pnlContent.Controls.Add(this.txt_last_name);
            this.pnlContent.Controls.Add(this.label1);
            this.pnlContent.Controls.Add(this.department_dd);
            this.pnlContent.Controls.Add(this.txt_id_number);
            this.pnlContent.Controls.Add(this.txt_first_name);
            this.pnlContent.Controls.Add(this.lblFormType);
            this.pnlContent.Controls.Add(this.lblFirstName);
            this.pnlContent.Controls.Add(this.lblLastName);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 51);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(542, 158);
            this.pnlContent.TabIndex = 36;
            // 
            // job_title_dd
            // 
            this.job_title_dd.FormattingEnabled = true;
            this.job_title_dd.Items.AddRange(new object[] {
            "Coordinator",
            "System Administrator",
            "Program Manager",
            "Supervisor",
            "Sales Representative",
            "Sales Associate",
            "Client Services Representative",
            "Courier",
            "Senior Lab Assistant",
            "Lab Assistant",
            "Send Out Lab Assistant",
            "Phlebotomist"});
            this.job_title_dd.Location = new System.Drawing.Point(344, 91);
            this.job_title_dd.Name = "job_title_dd";
            this.job_title_dd.Size = new System.Drawing.Size(121, 21);
            this.job_title_dd.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(291, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Job Title";
            // 
            // document_type_dd
            // 
            this.document_type_dd.FormattingEnabled = true;
            this.document_type_dd.Items.AddRange(new object[] {
            "Orientation Checklist",
            "Diploma/Resume",
            "Training Documents",
            "Competency Assessment",
            "Job Description",
            "Miscellaneous"});
            this.document_type_dd.Location = new System.Drawing.Point(142, 125);
            this.document_type_dd.Name = "document_type_dd";
            this.document_type_dd.Size = new System.Drawing.Size(121, 21);
            this.document_type_dd.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Document Type";
            // 
            // txt_last_name
            // 
            this.txt_last_name.Location = new System.Drawing.Point(344, 25);
            this.txt_last_name.Name = "txt_last_name";
            this.txt_last_name.Size = new System.Drawing.Size(121, 20);
            this.txt_last_name.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(280, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Last Name";
            // 
            // department_dd
            // 
            this.department_dd.FormattingEnabled = true;
            this.department_dd.Items.AddRange(new object[] {
            "Supervisory Staff",
            "Client Services",
            "Couriers",
            "Specimen Processing",
            "Patient Service Center"});
            this.department_dd.Location = new System.Drawing.Point(142, 91);
            this.department_dd.Name = "department_dd";
            this.department_dd.Size = new System.Drawing.Size(121, 21);
            this.department_dd.TabIndex = 4;
            // 
            // txt_id_number
            // 
            this.txt_id_number.Location = new System.Drawing.Point(142, 56);
            this.txt_id_number.Name = "txt_id_number";
            this.txt_id_number.Size = new System.Drawing.Size(121, 20);
            this.txt_id_number.TabIndex = 3;
            // 
            // txt_first_name
            // 
            this.txt_first_name.Location = new System.Drawing.Point(142, 24);
            this.txt_first_name.Name = "txt_first_name";
            this.txt_first_name.Size = new System.Drawing.Size(121, 20);
            this.txt_first_name.TabIndex = 1;
            // 
            // lblFormType
            // 
            this.lblFormType.AutoSize = true;
            this.lblFormType.Location = new System.Drawing.Point(74, 97);
            this.lblFormType.Name = "lblFormType";
            this.lblFormType.Size = new System.Drawing.Size(62, 13);
            this.lblFormType.TabIndex = 25;
            this.lblFormType.Text = "Department";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(80, 60);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(58, 13);
            this.lblFirstName.TabIndex = 24;
            this.lblFirstName.Text = "ID Number";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(79, 27);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(57, 13);
            this.lblLastName.TabIndex = 23;
            this.lblLastName.Text = "First Name";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(542, 51);
            this.lblTitle.TabIndex = 35;
            this.lblTitle.Text = "Employee Compliance Forms";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnCancel);
            this.pnlControls.Controls.Add(this.btnOk);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 213);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(542, 101);
            this.pnlControls.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(189, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(83, 15);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // EmployeeComplianceArchiverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 314);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmployeeComplianceArchiverDialog";
            this.Text = "Employee Compliance Dialog";
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox department_dd;
        private System.Windows.Forms.TextBox txt_id_number;
        private System.Windows.Forms.TextBox txt_first_name;
        private System.Windows.Forms.Label lblFormType;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txt_last_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox document_type_dd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox job_title_dd;
        private System.Windows.Forms.Label label3;
    }
}