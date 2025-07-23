namespace EdocsUSA.Scanquire.Clients.PSE
{
    partial class PSEFinancialRecordsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSEFinancialRecordsDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpEFinYear = new System.Windows.Forms.DateTimePicker();
            this.dtpSFinYear = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBoxIdentifer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpEFinYear);
            this.groupBox1.Controls.Add(this.dtpSFinYear);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbBoxIdentifer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 192);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Financial Records";
            // 
            // dtpEFinYear
            // 
            this.dtpEFinYear.CustomFormat = "MM/dd/yyyy";
            this.dtpEFinYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEFinYear.Location = new System.Drawing.Point(210, 68);
            this.dtpEFinYear.Name = "dtpEFinYear";
            this.dtpEFinYear.Size = new System.Drawing.Size(93, 20);
            this.dtpEFinYear.TabIndex = 9;
            // 
            // dtpSFinYear
            // 
            this.dtpSFinYear.CustomFormat = "MM/dd/yyyy";
            this.dtpSFinYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSFinYear.Location = new System.Drawing.Point(210, 29);
            this.dtpSFinYear.Name = "dtpSFinYear";
            this.dtpSFinYear.Size = new System.Drawing.Size(93, 20);
            this.dtpSFinYear.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(182, 135);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(97, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Financial Record";
            // 
            // cmbBoxIdentifer
            // 
            this.cmbBoxIdentifer.FormattingEnabled = true;
            this.cmbBoxIdentifer.Location = new System.Drawing.Point(182, 103);
            this.cmbBoxIdentifer.Name = "cmbBoxIdentifer";
            this.cmbBoxIdentifer.Size = new System.Drawing.Size(121, 21);
            this.cmbBoxIdentifer.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End Financial Quarter:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Financial Quarter:";
            // 
            // PSEFinancialRecordsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 192);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PSEFinancialRecordsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PSEFinancialRecordsDialog";
            this.Shown += new System.EventHandler(this.PSEFinancialRecordsDialog_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBoxIdentifer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpEFinYear;
        private System.Windows.Forms.DateTimePicker dtpSFinYear;
    }
}