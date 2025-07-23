namespace Edocs.Potomac.Elementary.School.Archiver
{
    partial class PESArchiverDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.rbStudentRecord = new System.Windows.Forms.RadioButton();
            this.rbFinRecords = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Archiver";
            // 
            // rbStudentRecord
            // 
            this.rbStudentRecord.AutoSize = true;
            this.rbStudentRecord.Location = new System.Drawing.Point(154, 138);
            this.rbStudentRecord.Name = "rbStudentRecord";
            this.rbStudentRecord.Size = new System.Drawing.Size(144, 17);
            this.rbStudentRecord.TabIndex = 1;
            this.rbStudentRecord.TabStop = true;
            this.rbStudentRecord.Text = "Archive Student Records";
            this.rbStudentRecord.UseVisualStyleBackColor = true;
            // 
            // rbFinRecords
            // 
            this.rbFinRecords.AutoSize = true;
            this.rbFinRecords.Location = new System.Drawing.Point(318, 138);
            this.rbFinRecords.Name = "rbFinRecords";
            this.rbFinRecords.Size = new System.Drawing.Size(152, 17);
            this.rbFinRecords.TabIndex = 2;
            this.rbFinRecords.TabStop = true;
            this.rbFinRecords.Text = "Archive Financial Records ";
            this.rbFinRecords.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(154, 192);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(279, 192);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // PotomacElementarySchoolArchiverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rbFinRecords);
            this.Controls.Add(this.rbStudentRecord);
            this.Controls.Add(this.label1);
            this.Name = "PotomacElementarySchoolArchiverDialog";
            this.Text = "Potomac Elementary School";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbStudentRecord;
        private System.Windows.Forms.RadioButton rbFinRecords;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}