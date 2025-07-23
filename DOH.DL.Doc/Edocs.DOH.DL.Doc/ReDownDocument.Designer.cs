
namespace Edocs.DOH.DL.Doc
{
    partial class ReDownDocument
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReDownDocument));
            this.lViewReDownLoad = new System.Windows.Forms.ListView();
            this.CIty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Church = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BookType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lViewReDownLoad
            // 
            this.lViewReDownLoad.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lViewReDownLoad.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lViewReDownLoad.CheckBoxes = true;
            this.lViewReDownLoad.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.CIty,
            this.Church,
            this.BookType,
            this.SDate,
            this.EDate,
            this.FileName});
            this.lViewReDownLoad.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lViewReDownLoad.FullRowSelect = true;
            this.lViewReDownLoad.HideSelection = false;
            this.lViewReDownLoad.HoverSelection = true;
            this.lViewReDownLoad.Location = new System.Drawing.Point(0, 43);
            this.lViewReDownLoad.Name = "lViewReDownLoad";
            this.lViewReDownLoad.Size = new System.Drawing.Size(778, 173);
            this.lViewReDownLoad.TabIndex = 0;
            this.lViewReDownLoad.UseCompatibleStateImageBehavior = false;
            this.lViewReDownLoad.View = System.Windows.Forms.View.Details;
            this.lViewReDownLoad.SelectedIndexChanged += new System.EventHandler(this.lViewReDownLoad_SelectedIndexChanged);
            // 
            // CIty
            // 
            this.CIty.Text = "City";
            this.CIty.Width = 100;
            // 
            // Church
            // 
            this.Church.Text = "Church";
            this.Church.Width = 100;
            // 
            // BookType
            // 
            this.BookType.Text = "Book Type";
            this.BookType.Width = 100;
            // 
            // SDate
            // 
            this.SDate.Text = "Start Date";
            this.SDate.Width = 100;
            // 
            // EDate
            // 
            this.EDate.Text = "End Date";
            this.EDate.Width = 100;
            // 
            // FileName
            // 
            this.FileName.Text = "File name";
            this.FileName.Width = 450;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 40;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 271);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(772, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 235);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "&DownLoad";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(351, 235);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(238, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Check ID To ReDownload Document";
            // 
            // ReDownDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 293);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lViewReDownLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReDownDocument";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReDownLoad Documents";
            this.Shown += new System.EventHandler(this.ReDownDocument_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lViewReDownLoad;
        private System.Windows.Forms.ColumnHeader CIty;
        private System.Windows.Forms.ColumnHeader Church;
        private System.Windows.Forms.ColumnHeader BookType;
        private System.Windows.Forms.ColumnHeader SDate;
        private System.Windows.Forms.ColumnHeader EDate;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
    }
}