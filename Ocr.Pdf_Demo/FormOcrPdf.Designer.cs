namespace Edocs.Ocr.Pdf
{
    partial class FormOcrPdf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOcrPdf));
            this.rTextBoxOcrPdf = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadImgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ocrImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ocrImageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuOcrImageTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.freeOcrTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsExportText = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pBoxImage = new System.Windows.Forms.PictureBox();
            this.saveFileDIalog = new System.Windows.Forms.SaveFileDialog();
            this.userControlPB1 = new Edocs.Ocr.Pdf.UserControlPB();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // rTextBoxOcrPdf
            // 
            this.rTextBoxOcrPdf.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rTextBoxOcrPdf.ImeMode = System.Windows.Forms.ImeMode.On;
            this.rTextBoxOcrPdf.Location = new System.Drawing.Point(160, 106);
            this.rTextBoxOcrPdf.Name = "rTextBoxOcrPdf";
            this.rTextBoxOcrPdf.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rTextBoxOcrPdf.Size = new System.Drawing.Size(100, 96);
            this.rTextBoxOcrPdf.TabIndex = 1;
            this.rTextBoxOcrPdf.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImgeToolStripMenuItem,
            this.ocrImageToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadImgeToolStripMenuItem
            // 
            this.loadImgeToolStripMenuItem.Name = "loadImgeToolStripMenuItem";
            this.loadImgeToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.loadImgeToolStripMenuItem.Text = "Load Imge";
            this.loadImgeToolStripMenuItem.Visible = false;
            this.loadImgeToolStripMenuItem.Click += new System.EventHandler(this.loadImgeToolStripMenuItem_Click);
            // 
            // ocrImageToolStripMenuItem
            // 
            this.ocrImageToolStripMenuItem.AutoToolTip = true;
            this.ocrImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMenuOcrImageTxt,
            this.ocrImageToolStripMenuItem1,
            this.freeOcrTextToolStripMenuItem});
            this.ocrImageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ocrImageToolStripMenuItem.Image")));
            this.ocrImageToolStripMenuItem.Name = "ocrImageToolStripMenuItem";
            this.ocrImageToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.ocrImageToolStripMenuItem.Text = "&OCR";
            this.ocrImageToolStripMenuItem.ToolTipText = "Edit Ocr Image";
            this.ocrImageToolStripMenuItem.Click += new System.EventHandler(this.ocrImageToolStripMenuItem_Click);
            // 
            // ocrImageToolStripMenuItem1
            // 
            this.ocrImageToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("ocrImageToolStripMenuItem1.Image")));
            this.ocrImageToolStripMenuItem1.Name = "ocrImageToolStripMenuItem1";
            this.ocrImageToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.ocrImageToolStripMenuItem1.Text = "&Image With Text";
            this.ocrImageToolStripMenuItem1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ocrImageToolStripMenuItem1.Click += new System.EventHandler(this.ocrImageToolStripMenuItem1_Click);
            // 
            // tsMenuOcrImageTxt
            // 
            this.tsMenuOcrImageTxt.Image = ((System.Drawing.Image)(resources.GetObject("tsMenuOcrImageTxt.Image")));
            this.tsMenuOcrImageTxt.Name = "tsMenuOcrImageTxt";
            this.tsMenuOcrImageTxt.Size = new System.Drawing.Size(180, 22);
            this.tsMenuOcrImageTxt.Text = "&Text";
            this.tsMenuOcrImageTxt.Click += new System.EventHandler(this.tsMenuOcrImageTxt_Click);
            // 
            // freeOcrTextToolStripMenuItem
            // 
            this.freeOcrTextToolStripMenuItem.Name = "freeOcrTextToolStripMenuItem";
            this.freeOcrTextToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.freeOcrTextToolStripMenuItem.Text = "Free Ocr Text";
            this.freeOcrTextToolStripMenuItem.Visible = false;
            this.freeOcrTextToolStripMenuItem.Click += new System.EventHandler(this.freeOcrTextToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsExportText,
            this.saveToolStripMenuItem1,
            this.saveCopyToolStripMenuItem});
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // tsExportText
            // 
            this.tsExportText.Image = global::Edocs.Ocr.Pdf.Properties.Resources.icons8_export_100;
            this.tsExportText.Name = "tsExportText";
            this.tsExportText.Size = new System.Drawing.Size(180, 22);
            this.tsExportText.Text = "&Export Text";
            this.tsExportText.Visible = false;
            this.tsExportText.Click += new System.EventHandler(this.tsExportText_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveCopyToolStripMenuItem
            // 
            this.saveCopyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveCopyToolStripMenuItem.Image")));
            this.saveCopyToolStripMenuItem.Name = "saveCopyToolStripMenuItem";
            this.saveCopyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveCopyToolStripMenuItem.Text = "Save Copy";
            this.saveCopyToolStripMenuItem.Click += new System.EventHandler(this.saveCopyToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pBoxImage);
            this.panel1.Location = new System.Drawing.Point(605, 214);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 3;
            // 
            // pBoxImage
            // 
            this.pBoxImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pBoxImage.Name = "pBoxImage";
            this.pBoxImage.Size = new System.Drawing.Size(196, 50);
            this.pBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxImage.TabIndex = 0;
            this.pBoxImage.TabStop = false;
            // 
            // userControlPB1
            // 
            this.userControlPB1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.userControlPB1.Location = new System.Drawing.Point(517, 260);
            this.userControlPB1.Name = "userControlPB1";
            this.userControlPB1.Size = new System.Drawing.Size(319, 75);
            this.userControlPB1.TabIndex = 4;
            // 
            // FormOcrPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1184, 594);
            this.Controls.Add(this.userControlPB1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rTextBoxOcrPdf);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormOcrPdf";
            this.Text = "FormOcrPdf";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.FormOcrPdf_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox rTextBoxOcrPdf;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadImgeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pBoxImage;
        private System.Windows.Forms.ToolStripMenuItem ocrImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ocrImageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsMenuOcrImageTxt;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private UserControlPB userControlPB1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem freeOcrTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsExportText;
        private System.Windows.Forms.SaveFileDialog saveFileDIalog;
    }
}