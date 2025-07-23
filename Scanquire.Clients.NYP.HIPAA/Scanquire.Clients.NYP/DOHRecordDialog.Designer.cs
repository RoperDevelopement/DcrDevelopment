namespace Scanquire.Clients.NYP
{
    partial class DOHRecordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DOHRecordDialog));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.PanelControls = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnUsePrevMRN = new System.Windows.Forms.Button();
            this.BTNUsePrevAccNum = new System.Windows.Forms.Button();
            this.TxtBoxPrevMRN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtBoxPrevAccNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dpDateOfService = new EdocsUSA.Utilities.Controls.DatePicker();
            this.lblDateOfService = new System.Windows.Forms.Label();
            this.lblMedicalRecordNumber = new System.Windows.Forms.Label();
            this.txtMedicalRecordNumber = new System.Windows.Forms.TextBox();
            this.txtAccessionNumber = new System.Windows.Forms.TextBox();
            this.lblAccessionNumber = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.PanBottom = new System.Windows.Forms.Panel();
            this.DOHImageViewer = new Scanquire.Public.UserControls.SQImageListViewer();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.PanelControls.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // PanelControls
            // 
            this.PanelControls.Controls.Add(this.label3);
            this.PanelControls.Controls.Add(this.groupBox1);
            this.PanelControls.Controls.Add(this.btnCancel);
            this.PanelControls.Controls.Add(this.btnOk);
            this.PanelControls.Controls.Add(this.dpDateOfService);
            this.PanelControls.Controls.Add(this.lblDateOfService);
            this.PanelControls.Controls.Add(this.lblMedicalRecordNumber);
            this.PanelControls.Controls.Add(this.txtMedicalRecordNumber);
            this.PanelControls.Controls.Add(this.txtAccessionNumber);
            this.PanelControls.Controls.Add(this.lblAccessionNumber);
            this.PanelControls.Controls.Add(this.lblTitle);
            this.PanelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelControls.Location = new System.Drawing.Point(0, 0);
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.Size = new System.Drawing.Size(1328, 263);
            this.PanelControls.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(606, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 51);
            this.label3.TabIndex = 54;
            this.label3.Text = "DOH";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnUsePrevMRN);
            this.groupBox1.Controls.Add(this.BTNUsePrevAccNum);
            this.groupBox1.Controls.Add(this.TxtBoxPrevMRN);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TxtBoxPrevAccNum);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 263);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Options";
            // 
            // BtnUsePrevMRN
            // 
            this.BtnUsePrevMRN.Location = new System.Drawing.Point(10, 229);
            this.BtnUsePrevMRN.Name = "BtnUsePrevMRN";
            this.BtnUsePrevMRN.Size = new System.Drawing.Size(150, 23);
            this.BtnUsePrevMRN.TabIndex = 58;
            this.BtnUsePrevMRN.Text = "Use Previous &MRN (F4)";
            this.BtnUsePrevMRN.UseVisualStyleBackColor = true;
            this.BtnUsePrevMRN.Click += new System.EventHandler(this.BtnUsePrevMRN_Click);
            // 
            // BTNUsePrevAccNum
            // 
            this.BTNUsePrevAccNum.Location = new System.Drawing.Point(10, 203);
            this.BTNUsePrevAccNum.Name = "BTNUsePrevAccNum";
            this.BTNUsePrevAccNum.Size = new System.Drawing.Size(170, 23);
            this.BTNUsePrevAccNum.TabIndex = 57;
            this.BTNUsePrevAccNum.Text = "&Use Previous Accession # (F3)";
            this.BTNUsePrevAccNum.UseVisualStyleBackColor = true;
            this.BTNUsePrevAccNum.Click += new System.EventHandler(this.BTNUsePrevAccNum_Click);
            // 
            // TxtBoxPrevMRN
            // 
            this.TxtBoxPrevMRN.Location = new System.Drawing.Point(9, 116);
            this.TxtBoxPrevMRN.Name = "TxtBoxPrevMRN";
            this.TxtBoxPrevMRN.ReadOnly = true;
            this.TxtBoxPrevMRN.Size = new System.Drawing.Size(170, 20);
            this.TxtBoxPrevMRN.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Previous Medical Number:";
            // 
            // TxtBoxPrevAccNum
            // 
            this.TxtBoxPrevAccNum.Location = new System.Drawing.Point(9, 46);
            this.TxtBoxPrevAccNum.Name = "TxtBoxPrevAccNum";
            this.TxtBoxPrevAccNum.ReadOnly = true;
            this.TxtBoxPrevAccNum.Size = new System.Drawing.Size(170, 20);
            this.TxtBoxPrevAccNum.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Previous Accession Number:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(668, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 75);
            this.btnCancel.TabIndex = 45;
            this.btnCancel.Text = "&Cancel (F12)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(559, 154);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 75);
            this.btnOk.TabIndex = 44;
            this.btnOk.Text = "&OK (F2)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dpDateOfService
            // 
            this.dpDateOfService.DisplayFormat = "MM/dd/yyyy";
            this.dpDateOfService.Location = new System.Drawing.Point(594, 119);
            this.dpDateOfService.Name = "dpDateOfService";
            this.dpDateOfService.Size = new System.Drawing.Size(118, 20);
            this.dpDateOfService.TabIndex = 52;
            this.dpDateOfService.Value = null;
            // 
            // lblDateOfService
            // 
            this.lblDateOfService.AutoSize = true;
            this.lblDateOfService.Location = new System.Drawing.Point(503, 125);
            this.lblDateOfService.Name = "lblDateOfService";
            this.lblDateOfService.Size = new System.Drawing.Size(83, 13);
            this.lblDateOfService.TabIndex = 51;
            this.lblDateOfService.Text = "Date Of Service";
            // 
            // lblMedicalRecordNumber
            // 
            this.lblMedicalRecordNumber.AutoSize = true;
            this.lblMedicalRecordNumber.Location = new System.Drawing.Point(503, 96);
            this.lblMedicalRecordNumber.Name = "lblMedicalRecordNumber";
            this.lblMedicalRecordNumber.Size = new System.Drawing.Size(122, 13);
            this.lblMedicalRecordNumber.TabIndex = 49;
            this.lblMedicalRecordNumber.Text = "Medical Record Number";
            // 
            // txtMedicalRecordNumber
            // 
            this.txtMedicalRecordNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMedicalRecordNumber.Location = new System.Drawing.Point(625, 93);
            this.txtMedicalRecordNumber.Name = "txtMedicalRecordNumber";
            this.txtMedicalRecordNumber.Size = new System.Drawing.Size(200, 20);
            this.txtMedicalRecordNumber.TabIndex = 50;
            // 
            // txtAccessionNumber
            // 
            this.txtAccessionNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAccessionNumber.Location = new System.Drawing.Point(605, 65);
            this.txtAccessionNumber.Name = "txtAccessionNumber";
            this.txtAccessionNumber.Size = new System.Drawing.Size(200, 20);
            this.txtAccessionNumber.TabIndex = 48;
            // 
            // lblAccessionNumber
            // 
            this.lblAccessionNumber.AutoSize = true;
            this.lblAccessionNumber.Location = new System.Drawing.Point(503, 65);
            this.lblAccessionNumber.Name = "lblAccessionNumber";
            this.lblAccessionNumber.Size = new System.Drawing.Size(96, 13);
            this.lblAccessionNumber.TabIndex = 47;
            this.lblAccessionNumber.Text = "Accession Number";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(569, -66);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(120, 51);
            this.lblTitle.TabIndex = 46;
            this.lblTitle.Text = "DOH";
            // 
            // PanBottom
            // 
            this.PanBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanBottom.Location = new System.Drawing.Point(0, 577);
            this.PanBottom.Name = "PanBottom";
            this.PanBottom.Size = new System.Drawing.Size(1328, 10);
            this.PanBottom.TabIndex = 63;
            // 
            // DOHImageViewer
            // 
            this.DOHImageViewer.ActiveItem = null;
            this.DOHImageViewer.CurrentThumbnailSizeMode = Scanquire.Public.UserControls.SQImageListViewer.ThumbnailSizeMode.Small;
            this.DOHImageViewer.DeskewAngle = 1F;
            this.DOHImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DOHImageViewer.FillColor = System.Drawing.Color.White;
            this.DOHImageViewer.LargeThumbnailSize = new System.Drawing.Size(255, 330);
            this.DOHImageViewer.Location = new System.Drawing.Point(0, 263);
            this.DOHImageViewer.MediumThumbnailSize = new System.Drawing.Size(170, 220);
            this.DOHImageViewer.Name = "DOHImageViewer";
            this.DOHImageViewer.Saved = true;
            this.DOHImageViewer.Size = new System.Drawing.Size(1328, 314);
            this.DOHImageViewer.SmallThumbnailSize = new System.Drawing.Size(85, 110);
            this.DOHImageViewer.TabIndex = 64;
            this.DOHImageViewer.ViewMode = Scanquire.Public.UserControls.SQImageListViewer.ImageThumbnailViewMode.Image;
            // 
            // DOHRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 587);
            this.Controls.Add(this.DOHImageViewer);
            this.Controls.Add(this.PanBottom);
            this.Controls.Add(this.PanelControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "DOHRecordDialog";
            this.Text = "DOH Record Dialog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.DOHRecordDialog_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DOHRecordDialog_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DOHRecordDialog_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.PanelControls.ResumeLayout(false);
            this.PanelControls.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel PanelControls;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnUsePrevMRN;
        private System.Windows.Forms.Button BTNUsePrevAccNum;
        private System.Windows.Forms.TextBox TxtBoxPrevMRN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtBoxPrevAccNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private EdocsUSA.Utilities.Controls.DatePicker dpDateOfService;
        private System.Windows.Forms.Label lblDateOfService;
        private System.Windows.Forms.Label lblMedicalRecordNumber;
        private System.Windows.Forms.TextBox txtMedicalRecordNumber;
        private System.Windows.Forms.TextBox txtAccessionNumber;
        private System.Windows.Forms.Label lblAccessionNumber;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel PanBottom;
        private Public.UserControls.SQImageListViewer DOHImageViewer;
    }
}