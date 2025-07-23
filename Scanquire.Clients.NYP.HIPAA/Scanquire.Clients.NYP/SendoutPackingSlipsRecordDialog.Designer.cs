namespace Scanquire.Clients.NYP
{
    partial class SendoutPackingSlipsRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendoutPackingSlipsRecordDialog));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpRecordSettings = new System.Windows.Forms.GroupBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dpDate = new EdocsUSA.Utilities.Controls.DatePicker();
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
            this.grpRecordSettings.Controls.Add(this.dpDate);
            this.grpRecordSettings.Controls.Add(this.lblDate);
            this.grpRecordSettings.Location = new System.Drawing.Point(148, 89);
            this.grpRecordSettings.Name = "grpRecordSettings";
            this.grpRecordSettings.Size = new System.Drawing.Size(320, 69);
            this.grpRecordSettings.TabIndex = 31;
            this.grpRecordSettings.TabStop = false;
            this.grpRecordSettings.Text = "Record Settings";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(42, 32);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 13);
            this.lblDate.TabIndex = 14;
            this.lblDate.Text = "Date";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(14, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(592, 51);
            this.lblTitle.TabIndex = 32;
            this.lblTitle.Text = "SENDOUT PACKING SLIPS";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(311, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(230, 195);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 29;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dpDate
            // 
            this.dpDate.DisplayFormat = "MM/dd/yyyy";
            this.dpDate.Location = new System.Drawing.Point(82, 32);
            this.dpDate.Name = "dpDate";
            this.dpDate.Size = new System.Drawing.Size(128, 20);
            this.dpDate.TabIndex = 15;
            this.dpDate.Value = null;
            // 
            // SendoutPackingSlipsRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 304);
            this.Controls.Add(this.grpRecordSettings);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SendoutPackingSlipsRecordDialog";
            this.Text = "Sendout Packing Slips Record Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpRecordSettings.ResumeLayout(false);
            this.grpRecordSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox grpRecordSettings;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblDate;
        private EdocsUSA.Utilities.Controls.DatePicker dpDate;
    }
}