
namespace Edocs.Dillion.VCC.Archiver
{
    partial class VCCArchiverrDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCCArchiverrDialog));
            this.panelVCC = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.txtBoxInvoiceDescription = new System.Windows.Forms.TextBox();
            this.txtBoxInvoiceNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.VCCImageViewer = new Scanquire.Public.UserControls.SQImageListViewer();
            this.panelVCC.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelVCC
            // 
            this.panelVCC.Controls.Add(this.dateTimePicker1);
            this.panelVCC.Controls.Add(this.txtBoxInvoiceDescription);
            this.panelVCC.Controls.Add(this.txtBoxInvoiceNumber);
            this.panelVCC.Controls.Add(this.label3);
            this.panelVCC.Controls.Add(this.label2);
            this.panelVCC.Controls.Add(this.label1);
            this.panelVCC.Controls.Add(this.button2);
            this.panelVCC.Controls.Add(this.button1);
            this.panelVCC.Controls.Add(this.groupBox1);
            this.panelVCC.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelVCC.Location = new System.Drawing.Point(0, 0);
            this.panelVCC.Name = "panelVCC";
            this.panelVCC.Size = new System.Drawing.Size(893, 242);
            this.panelVCC.TabIndex = 0;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "MM-yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(432, 127);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(85, 20);
            this.dateTimePicker1.TabIndex = 8;
            // 
            // txtBoxInvoiceDescription
            // 
            this.txtBoxInvoiceDescription.Location = new System.Drawing.Point(449, 75);
            this.txtBoxInvoiceDescription.Name = "txtBoxInvoiceDescription";
            this.txtBoxInvoiceDescription.Size = new System.Drawing.Size(100, 20);
            this.txtBoxInvoiceDescription.TabIndex = 7;
            // 
            // txtBoxInvoiceNumber
            // 
            this.txtBoxInvoiceNumber.Location = new System.Drawing.Point(460, 37);
            this.txtBoxInvoiceNumber.Name = "txtBoxInvoiceNumber";
            this.txtBoxInvoiceNumber.Size = new System.Drawing.Size(100, 20);
            this.txtBoxInvoiceNumber.TabIndex = 6;
            this.txtBoxInvoiceNumber.TextChanged += new System.EventHandler(this.txtBoxInvoiceNumber_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Invoice Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(363, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Invoice Description:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(363, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Invoice Numnber:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(474, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(319, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 242);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Invoice Numnber:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Invoice Numnber:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Invoice Numnber:";
            // 
            // VCCImageViewer
            // 
            this.VCCImageViewer.ActiveItem = null;
            this.VCCImageViewer.CurrentThumbnailSizeMode = Scanquire.Public.UserControls.SQImageListViewer.ThumbnailSizeMode.Small;
            this.VCCImageViewer.DeskewAngle = 1F;
            this.VCCImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VCCImageViewer.FillColor = System.Drawing.Color.White;
            this.VCCImageViewer.LargeThumbnailSize = new System.Drawing.Size(255, 330);
            this.VCCImageViewer.Location = new System.Drawing.Point(0, 242);
            this.VCCImageViewer.MediumThumbnailSize = new System.Drawing.Size(170, 220);
            this.VCCImageViewer.Name = "VCCImageViewer";
            this.VCCImageViewer.Saved = true;
            this.VCCImageViewer.Size = new System.Drawing.Size(893, 311);
            this.VCCImageViewer.SmallThumbnailSize = new System.Drawing.Size(85, 110);
            this.VCCImageViewer.TabIndex = 1;
            this.VCCImageViewer.ViewMode = Scanquire.Public.UserControls.SQImageListViewer.ImageThumbnailViewMode.ThumbnailsAndImage;
            // 
            // VCCArchiverrDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 553);
            this.Controls.Add(this.VCCImageViewer);
            this.Controls.Add(this.panelVCC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VCCArchiverrDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VCC Of Dillion Archiver";
            this.panelVCC.ResumeLayout(false);
            this.panelVCC.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelVCC;
        private Scanquire.Public.UserControls.SQImageListViewer VCCImageViewer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBoxInvoiceDescription;
        private System.Windows.Forms.TextBox txtBoxInvoiceNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}