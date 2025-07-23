
namespace PSSDArchiver
{
    partial class PSSDDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBoxTotalScanned = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.dateDOB = new System.Windows.Forms.DateTimePicker();
            this.txtBoxFName = new System.Windows.Forms.TextBox();
            this.txtBoxLname = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBoxMethFiling = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rTxtBoxDescRecords = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dRecsEDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtRecsSDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBoxOrigDept = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBoxDept = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ImageViewerPSD = new Scanquire.Public.UserControls.SQImageListViewer();
            this.cmbBoxTrackingIDS = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBoxTrackingIDS);
            this.groupBox1.Controls.Add(this.txtBoxTotalScanned);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.dateDOB);
            this.groupBox1.Controls.Add(this.txtBoxFName);
            this.groupBox1.Controls.Add(this.txtBoxLname);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtBoxMethFiling);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.rTxtBoxDescRecords);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dRecsEDate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtRecsSDate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbBoxOrigDept);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbBoxDept);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1147, 374);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PSUSD  Demo Index Fields";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtBoxTotalScanned
            // 
            this.txtBoxTotalScanned.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTotalScanned.Location = new System.Drawing.Point(560, 292);
            this.txtBoxTotalScanned.Name = "txtBoxTotalScanned";
            this.txtBoxTotalScanned.Size = new System.Drawing.Size(75, 26);
            this.txtBoxTotalScanned.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(389, 296);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(168, 18);
            this.label11.TabIndex = 23;
            this.label11.Text = "Total Records Scanned:";
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(522, 334);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(387, 334);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 21;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(830, 258);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 18);
            this.label10.TabIndex = 20;
            this.label10.Text = "Date of Birth:";
            // 
            // dateDOB
            // 
            this.dateDOB.CustomFormat = "MM/dd/yyyy";
            this.dateDOB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateDOB.Location = new System.Drawing.Point(931, 254);
            this.dateDOB.Name = "dateDOB";
            this.dateDOB.Size = new System.Drawing.Size(115, 26);
            this.dateDOB.TabIndex = 10;
            // 
            // txtBoxFName
            // 
            this.txtBoxFName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxFName.Location = new System.Drawing.Point(610, 254);
            this.txtBoxFName.Name = "txtBoxFName";
            this.txtBoxFName.Size = new System.Drawing.Size(213, 26);
            this.txtBoxFName.TabIndex = 9;
            // 
            // txtBoxLname
            // 
            this.txtBoxLname.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxLname.Location = new System.Drawing.Point(303, 254);
            this.txtBoxLname.Name = "txtBoxLname";
            this.txtBoxLname.Size = new System.Drawing.Size(213, 26);
            this.txtBoxLname.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(522, 258);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 18);
            this.label9.TabIndex = 16;
            this.label9.Text = "First Name:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(215, 258);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 18);
            this.label8.TabIndex = 15;
            this.label8.Text = "Last Name:";
            // 
            // txtBoxMethFiling
            // 
            this.txtBoxMethFiling.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMethFiling.Location = new System.Drawing.Point(335, 200);
            this.txtBoxMethFiling.MaxLength = 50;
            this.txtBoxMethFiling.Name = "txtBoxMethFiling";
            this.txtBoxMethFiling.Size = new System.Drawing.Size(375, 26);
            this.txtBoxMethFiling.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(215, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "Method of Filing:";
            // 
            // rTxtBoxDescRecords
            // 
            this.rTxtBoxDescRecords.Location = new System.Drawing.Point(383, 133);
            this.rTxtBoxDescRecords.MaxLength = 150;
            this.rTxtBoxDescRecords.Name = "rTxtBoxDescRecords";
            this.rTxtBoxDescRecords.Size = new System.Drawing.Size(327, 49);
            this.rTxtBoxDescRecords.TabIndex = 6;
            this.rTxtBoxDescRecords.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(215, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "Description of Records:";
            // 
            // dRecsEDate
            // 
            this.dRecsEDate.CustomFormat = "MM/dd/yyyy";
            this.dRecsEDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dRecsEDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dRecsEDate.Location = new System.Drawing.Point(485, 95);
            this.dRecsEDate.Name = "dRecsEDate";
            this.dRecsEDate.Size = new System.Drawing.Size(115, 26);
            this.dRecsEDate.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(466, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 18);
            this.label6.TabIndex = 9;
            this.label6.Text = "-";
            // 
            // dtRecsSDate
            // 
            this.dtRecsSDate.CustomFormat = "MM/dd/yyyy";
            this.dtRecsSDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtRecsSDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtRecsSDate.Location = new System.Drawing.Point(340, 95);
            this.dtRecsSDate.Name = "dtRecsSDate";
            this.dtRecsSDate.Size = new System.Drawing.Size(115, 26);
            this.dtRecsSDate.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(215, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Date of Records:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(418, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Box Label / Tracking ID:";
            // 
            // cmbBoxOrigDept
            // 
            this.cmbBoxOrigDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBoxOrigDept.FormattingEnabled = true;
            this.cmbBoxOrigDept.Location = new System.Drawing.Point(736, 53);
            this.cmbBoxOrigDept.Name = "cmbBoxOrigDept";
            this.cmbBoxOrigDept.Size = new System.Drawing.Size(223, 28);
            this.cmbBoxOrigDept.Sorted = true;
            this.cmbBoxOrigDept.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(572, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Orignating Department:";
            // 
            // cmbBoxDept
            // 
            this.cmbBoxDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBoxDept.FormattingEnabled = true;
            this.cmbBoxDept.Location = new System.Drawing.Point(304, 53);
            this.cmbBoxDept.Name = "cmbBoxDept";
            this.cmbBoxDept.Size = new System.Drawing.Size(261, 28);
            this.cmbBoxDept.Sorted = true;
            this.cmbBoxDept.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(215, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Department:";
            // 
            // ImageViewerPSD
            // 
            this.ImageViewerPSD.ActiveItem = null;
            this.ImageViewerPSD.CurrentThumbnailSizeMode = Scanquire.Public.UserControls.SQImageListViewer.ThumbnailSizeMode.Small;
            this.ImageViewerPSD.DeskewAngle = 1F;
            this.ImageViewerPSD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageViewerPSD.FillColor = System.Drawing.Color.White;
            this.ImageViewerPSD.LargeThumbnailSize = new System.Drawing.Size(255, 330);
            this.ImageViewerPSD.Location = new System.Drawing.Point(0, 374);
            this.ImageViewerPSD.MediumThumbnailSize = new System.Drawing.Size(170, 220);
            this.ImageViewerPSD.Name = "ImageViewerPSD";
            this.ImageViewerPSD.Saved = true;
            this.ImageViewerPSD.Size = new System.Drawing.Size(1147, 266);
            this.ImageViewerPSD.SmallThumbnailSize = new System.Drawing.Size(85, 110);
            this.ImageViewerPSD.TabIndex = 1;
            this.ImageViewerPSD.ViewMode = Scanquire.Public.UserControls.SQImageListViewer.ImageThumbnailViewMode.ThumbnailsAndImage;
            // 
            // cmbBoxTrackingIDS
            // 
            this.cmbBoxTrackingIDS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBoxTrackingIDS.FormattingEnabled = true;
            this.cmbBoxTrackingIDS.Location = new System.Drawing.Point(585, 12);
            this.cmbBoxTrackingIDS.Name = "cmbBoxTrackingIDS";
            this.cmbBoxTrackingIDS.Size = new System.Drawing.Size(252, 28);
            this.cmbBoxTrackingIDS.Sorted = true;
            this.cmbBoxTrackingIDS.TabIndex = 25;
            // 
            // PSSDDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 640);
            this.Controls.Add(this.ImageViewerPSD);
            this.Controls.Add(this.groupBox1);
            this.Name = "PSSDDialog";
            this.Text = "PSUSDDialog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.PSSDDialog_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtRecsSDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBoxOrigDept;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBoxDept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dateDOB;
        private System.Windows.Forms.TextBox txtBoxFName;
        private System.Windows.Forms.TextBox txtBoxLname;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBoxMethFiling;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox rTxtBoxDescRecords;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dRecsEDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private Scanquire.Public.UserControls.SQImageListViewer ImageViewerPSD;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBoxTotalScanned;
        private System.Windows.Forms.ComboBox cmbBoxTrackingIDS;
    }
}