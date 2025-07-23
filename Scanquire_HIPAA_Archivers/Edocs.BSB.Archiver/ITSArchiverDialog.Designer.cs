
namespace Edocs.ITS.Archiver
{
    partial class ITSArchiverDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ITSArchiverDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkBoxAddTitle = new System.Windows.Forms.CheckBox();
            this.tichTxtBoxDesc = new System.Windows.Forms.RichTextBox();
            this.txtBoxDate = new System.Windows.Forms.TextBox();
            this.txtBoxTitle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTRec = new System.Windows.Forms.TextBox();
            this.labTRecScanned = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbBoxIdentifer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxTrackingID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chkBoxAddTitle);
            this.panel1.Controls.Add(this.tichTxtBoxDesc);
            this.panel1.Controls.Add(this.txtBoxDate);
            this.panel1.Controls.Add(this.txtBoxTitle);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtTRec);
            this.panel1.Controls.Add(this.labTRecScanned);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.cmbBoxIdentifer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtBoxTrackingID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 433);
            this.panel1.TabIndex = 0;
            // 
            // chkBoxAddTitle
            // 
            this.chkBoxAddTitle.AutoSize = true;
            this.chkBoxAddTitle.Location = new System.Drawing.Point(293, 312);
            this.chkBoxAddTitle.Name = "chkBoxAddTitle";
            this.chkBoxAddTitle.Size = new System.Drawing.Size(88, 22);
            this.chkBoxAddTitle.TabIndex = 19;
            this.chkBoxAddTitle.Text = "Add Title";
            this.chkBoxAddTitle.UseVisualStyleBackColor = true;
            // 
            // tichTxtBoxDesc
            // 
            this.tichTxtBoxDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tichTxtBoxDesc.Location = new System.Drawing.Point(222, 213);
            this.tichTxtBoxDesc.Name = "tichTxtBoxDesc";
            this.tichTxtBoxDesc.Size = new System.Drawing.Size(337, 78);
            this.tichTxtBoxDesc.TabIndex = 6;
            this.tichTxtBoxDesc.Text = "";
            // 
            // txtBoxDate
            // 
            this.txtBoxDate.Location = new System.Drawing.Point(170, 172);
            this.txtBoxDate.Name = "txtBoxDate";
            this.txtBoxDate.Size = new System.Drawing.Size(179, 26);
            this.txtBoxDate.TabIndex = 4;
            // 
            // txtBoxTitle
            // 
            this.txtBoxTitle.Location = new System.Drawing.Point(170, 138);
            this.txtBoxTitle.Name = "txtBoxTitle";
            this.txtBoxTitle.Size = new System.Drawing.Size(380, 26);
            this.txtBoxTitle.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(124, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 18);
            this.label5.TabIndex = 18;
            this.label5.Text = "Description:";
            // 
            // txtTRec
            // 
            this.txtTRec.Location = new System.Drawing.Point(310, 72);
            this.txtTRec.Name = "txtTRec";
            this.txtTRec.Size = new System.Drawing.Size(100, 26);
            this.txtTRec.TabIndex = 2;
            // 
            // labTRecScanned
            // 
            this.labTRecScanned.AutoSize = true;
            this.labTRecScanned.Location = new System.Drawing.Point(124, 76);
            this.labTRecScanned.Name = "labTRecScanned";
            this.labTRecScanned.Size = new System.Drawing.Size(124, 18);
            this.labTRecScanned.TabIndex = 16;
            this.labTRecScanned.Text = "Pages Scanned:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(345, 359);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 28);
            this.button2.TabIndex = 8;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 7;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbBoxIdentifer
            // 
            this.cmbBoxIdentifer.FormattingEnabled = true;
            this.cmbBoxIdentifer.Location = new System.Drawing.Point(260, 105);
            this.cmbBoxIdentifer.Name = "cmbBoxIdentifer";
            this.cmbBoxIdentifer.Size = new System.Drawing.Size(231, 26);
            this.cmbBoxIdentifer.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Collection Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Title:";
            // 
            // txtBoxTrackingID
            // 
            this.txtBoxTrackingID.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTrackingID.Location = new System.Drawing.Point(222, 40);
            this.txtBoxTrackingID.Name = "txtBoxTrackingID";
            this.txtBoxTrackingID.Size = new System.Drawing.Size(147, 23);
            this.txtBoxTrackingID.TabIndex = 1;
            this.txtBoxTrackingID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxTrackingID_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(124, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tracking ID:";
            // 
            // ITSArchiverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 433);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ITSArchiverDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ITSArchiverDialog";
            this.Shown += new System.EventHandler(this.ITSArchiverDialog_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBoxTrackingID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbBoxIdentifer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTRec;
        private System.Windows.Forms.Label labTRecScanned;
        private System.Windows.Forms.RichTextBox tichTxtBoxDesc;
        private System.Windows.Forms.TextBox txtBoxDate;
        private System.Windows.Forms.TextBox txtBoxTitle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkBoxAddTitle;
    }
}