namespace EdocsUSA.Scanquire.Clients.PSE
{
    partial class PSEArchiverDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSEArchiverDialog));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTRec = new System.Windows.Forms.TextBox();
            this.labTRecScanned = new System.Windows.Forms.Label();
            this.rbFinRec = new System.Windows.Forms.RadioButton();
            this.rbStudents = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(76, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(185, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTRec);
            this.groupBox1.Controls.Add(this.labTRecScanned);
            this.groupBox1.Controls.Add(this.rbFinRec);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.rbStudents);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 173);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Archiver";
            // 
            // txtTRec
            // 
            this.txtTRec.Location = new System.Drawing.Point(209, 74);
            this.txtTRec.Name = "txtTRec";
            this.txtTRec.Size = new System.Drawing.Size(100, 20);
            this.txtTRec.TabIndex = 5;
            this.txtTRec.TextChanged += new System.EventHandler(this.txtTRec_TextChanged);
            // 
            // labTRecScanned
            // 
            this.labTRecScanned.AutoSize = true;
            this.labTRecScanned.Location = new System.Drawing.Point(84, 77);
            this.labTRecScanned.Name = "labTRecScanned";
            this.labTRecScanned.Size = new System.Drawing.Size(123, 13);
            this.labTRecScanned.TabIndex = 4;
            this.labTRecScanned.Text = "Total Records Scanned:";
            // 
            // rbFinRec
            // 
            this.rbFinRec.AutoSize = true;
            this.rbFinRec.Location = new System.Drawing.Point(84, 48);
            this.rbFinRec.Name = "rbFinRec";
            this.rbFinRec.Size = new System.Drawing.Size(110, 17);
            this.rbFinRec.TabIndex = 1;
            this.rbFinRec.TabStop = true;
            this.rbFinRec.Text = "&Financial Records";
            this.rbFinRec.UseVisualStyleBackColor = true;
            // 
            // rbStudents
            // 
            this.rbStudents.AutoSize = true;
            this.rbStudents.Checked = true;
            this.rbStudents.Location = new System.Drawing.Point(84, 19);
            this.rbStudents.Name = "rbStudents";
            this.rbStudents.Size = new System.Drawing.Size(67, 17);
            this.rbStudents.TabIndex = 0;
            this.rbStudents.TabStop = true;
            this.rbStudents.Text = "&Students";
            this.rbStudents.UseVisualStyleBackColor = true;
            // 
            // PSEArchiverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 173);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PSEArchiverDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PSEArchiverDialog";
            this.Shown += new System.EventHandler(this.PSEArchiverDialog_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFinRec;
        private System.Windows.Forms.RadioButton rbStudents;
        private System.Windows.Forms.TextBox txtTRec;
        private System.Windows.Forms.Label labTRecScanned;
    }
}