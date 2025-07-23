namespace Scanquire.Clients.NYP
{
    partial class ComplianceLogRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComplianceLogRecordDialog));
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.txtBoxLogStation = new System.Windows.Forms.TextBox();
            this.btnAddLogStation = new System.Windows.Forms.Button();
            this.lblLogDateDesc = new System.Windows.Forms.Label();
            this.dpLogDate = new EdocsUSA.Utilities.Controls.DatePicker();
            this.lblLogDate = new System.Windows.Forms.Label();
            this.cmbLogStation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.imgCurrent = new System.Windows.Forms.PictureBox();
            this.pnlControls.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(976, 51);
            this.lblTitle.TabIndex = 30;
            this.lblTitle.Text = "Maintenance Logs";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnCancel);
            this.pnlControls.Controls.Add(this.btnOk);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 399);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(976, 100);
            this.pnlControls.TabIndex = 31;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(396, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(315, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.txtBoxLogStation);
            this.pnlContent.Controls.Add(this.btnAddLogStation);
            this.pnlContent.Controls.Add(this.lblLogDateDesc);
            this.pnlContent.Controls.Add(this.dpLogDate);
            this.pnlContent.Controls.Add(this.lblLogDate);
            this.pnlContent.Controls.Add(this.cmbLogStation);
            this.pnlContent.Controls.Add(this.lblLocation);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 51);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(976, 82);
            this.pnlContent.TabIndex = 32;
            // 
            // txtBoxLogStation
            // 
            this.txtBoxLogStation.Location = new System.Drawing.Point(626, 25);
            this.txtBoxLogStation.MaxLength = 100;
            this.txtBoxLogStation.Name = "txtBoxLogStation";
            this.txtBoxLogStation.Size = new System.Drawing.Size(209, 20);
            this.txtBoxLogStation.TabIndex = 14;
            // 
            // btnAddLogStation
            // 
            this.btnAddLogStation.Location = new System.Drawing.Point(520, 24);
            this.btnAddLogStation.Name = "btnAddLogStation";
            this.btnAddLogStation.Size = new System.Drawing.Size(100, 23);
            this.btnAddLogStation.TabIndex = 13;
            this.btnAddLogStation.Text = "Add Log Station";
            this.btnAddLogStation.UseVisualStyleBackColor = true;
            this.btnAddLogStation.Click += new System.EventHandler(this.btnAddLogStation_Click);
            // 
            // lblLogDateDesc
            // 
            this.lblLogDateDesc.AutoSize = true;
            this.lblLogDateDesc.Location = new System.Drawing.Point(278, 6);
            this.lblLogDateDesc.Name = "lblLogDateDesc";
            this.lblLogDateDesc.Size = new System.Drawing.Size(231, 13);
            this.lblLogDateDesc.TabIndex = 12;
            this.lblLogDateDesc.Text = "(For monthly logs, use the first day of the month)";
            // 
            // dpLogDate
            // 
            this.dpLogDate.DisplayFormat = "MM/dd/yyyy";
            this.dpLogDate.Location = new System.Drawing.Point(366, 25);
            this.dpLogDate.Name = "dpLogDate";
            this.dpLogDate.Size = new System.Drawing.Size(95, 20);
            this.dpLogDate.TabIndex = 11;
            this.dpLogDate.Value = null;
            // 
            // lblLogDate
            // 
            this.lblLogDate.AutoSize = true;
            this.lblLogDate.Location = new System.Drawing.Point(309, 29);
            this.lblLogDate.Name = "lblLogDate";
            this.lblLogDate.Size = new System.Drawing.Size(51, 13);
            this.lblLogDate.TabIndex = 10;
            this.lblLogDate.Text = "Log Date";
            // 
            // cmbLogStation
            // 
            this.cmbLogStation.FormattingEnabled = true;
            this.cmbLogStation.Location = new System.Drawing.Point(366, 53);
            this.cmbLogStation.Name = "cmbLogStation";
            this.cmbLogStation.Size = new System.Drawing.Size(175, 21);
            this.cmbLogStation.Sorted = true;
            this.cmbLogStation.TabIndex = 2;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(299, 56);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(61, 13);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Log Station";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // imgCurrent
            // 
            this.imgCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgCurrent.Location = new System.Drawing.Point(0, 133);
            this.imgCurrent.Name = "imgCurrent";
            this.imgCurrent.Size = new System.Drawing.Size(976, 266);
            this.imgCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCurrent.TabIndex = 33;
            this.imgCurrent.TabStop = false;
            // 
            // ComplianceLogRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 499);
            this.Controls.Add(this.imgCurrent);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.lblTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ComplianceLogRecordDialog";
            this.Text = "Compliance Logs";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlControls.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCurrent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.ComboBox cmbLogStation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblLogDateDesc;
        private EdocsUSA.Utilities.Controls.DatePicker dpLogDate;
        private System.Windows.Forms.Label lblLogDate;
        private System.Windows.Forms.PictureBox imgCurrent;
        private System.Windows.Forms.TextBox txtBoxLogStation;
        private System.Windows.Forms.Button btnAddLogStation;
    }
}