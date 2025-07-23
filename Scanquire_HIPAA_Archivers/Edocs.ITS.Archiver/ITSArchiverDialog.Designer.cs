
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTRec = new System.Windows.Forms.TextBox();
            this.labTRecScanned = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbBoxIdentifer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEFinYear = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpSFinYear = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxTrackingID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.txtTRec);
            this.panel1.Controls.Add(this.labTRecScanned);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.cmbBoxIdentifer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dtpEFinYear);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dtpSFinYear);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtBoxTrackingID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 303);
            this.panel1.TabIndex = 0;
            // 
            // txtTRec
            // 
            this.txtTRec.Location = new System.Drawing.Point(412, 79);
            this.txtTRec.Name = "txtTRec";
            this.txtTRec.Size = new System.Drawing.Size(100, 26);
            this.txtTRec.TabIndex = 17;
            // 
            // labTRecScanned
            // 
            this.labTRecScanned.AutoSize = true;
            this.labTRecScanned.Location = new System.Drawing.Point(226, 79);
            this.labTRecScanned.Name = "labTRecScanned";
            this.labTRecScanned.Size = new System.Drawing.Size(173, 18);
            this.labTRecScanned.TabIndex = 16;
            this.labTRecScanned.Text = "Total Records Scanned:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(354, 241);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 25);
            this.button2.TabIndex = 15;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(237, 238);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 14;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbBoxIdentifer
            // 
            this.cmbBoxIdentifer.FormattingEnabled = true;
            this.cmbBoxIdentifer.Location = new System.Drawing.Point(412, 179);
            this.cmbBoxIdentifer.Name = "cmbBoxIdentifer";
            this.cmbBoxIdentifer.Size = new System.Drawing.Size(231, 26);
            this.cmbBoxIdentifer.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Collection Name:";
            // 
            // dtpEFinYear
            // 
            this.dtpEFinYear.CustomFormat = "MM/dd/yyyy";
            this.dtpEFinYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEFinYear.Location = new System.Drawing.Point(412, 142);
            this.dtpEFinYear.Name = "dtpEFinYear";
            this.dtpEFinYear.Size = new System.Drawing.Size(145, 26);
            this.dtpEFinYear.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(226, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "End Date:";
            // 
            // dtpSFinYear
            // 
            this.dtpSFinYear.CustomFormat = "MM/dd/yyyy";
            this.dtpSFinYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSFinYear.Location = new System.Drawing.Point(412, 110);
            this.dtpSFinYear.Name = "dtpSFinYear";
            this.dtpSFinYear.Size = new System.Drawing.Size(145, 26);
            this.dtpSFinYear.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start Date:";
            // 
            // txtBoxTrackingID
            // 
            this.txtBoxTrackingID.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTrackingID.Location = new System.Drawing.Point(324, 42);
            this.txtBoxTrackingID.Name = "txtBoxTrackingID";
            this.txtBoxTrackingID.Size = new System.Drawing.Size(147, 23);
            this.txtBoxTrackingID.TabIndex = 1;
            this.txtBoxTrackingID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxTrackingID_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(226, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tracking ID:";
            // 
            // ITSArchiverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 303);
            this.Controls.Add(this.panel1);
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
        private System.Windows.Forms.DateTimePicker dtpEFinYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpSFinYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTRec;
        private System.Windows.Forms.Label labTRecScanned;
    }
}