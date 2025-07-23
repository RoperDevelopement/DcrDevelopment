namespace BinMonitor.Common
{
    partial class EmailAddress
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEmailAdd = new System.Windows.Forms.CheckedListBox();
            this.txtEmailAdd = new System.Windows.Forms.TextBox();
            this.BtnCancle = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkEmailAdd);
            this.groupBox1.Controls.Add(this.txtEmailAdd);
            this.groupBox1.Controls.Add(this.BtnCancle);
            this.groupBox1.Controls.Add(this.btnDone);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Email Address";
            // 
            // chkEmailAdd
            // 
            this.chkEmailAdd.FormattingEnabled = true;
            this.chkEmailAdd.Location = new System.Drawing.Point(8, 24);
            this.chkEmailAdd.Name = "chkEmailAdd";
            this.chkEmailAdd.Size = new System.Drawing.Size(280, 124);
            this.chkEmailAdd.TabIndex = 4;
            // 
            // txtEmailAdd
            // 
            this.txtEmailAdd.Enabled = false;
            this.txtEmailAdd.Location = new System.Drawing.Point(192, 160);
            this.txtEmailAdd.Name = "txtEmailAdd";
            this.txtEmailAdd.Size = new System.Drawing.Size(100, 20);
            this.txtEmailAdd.TabIndex = 3;
            this.txtEmailAdd.Visible = false;
            // 
            // BtnCancle
            // 
            this.BtnCancle.Location = new System.Drawing.Point(104, 152);
            this.BtnCancle.Name = "BtnCancle";
            this.BtnCancle.Size = new System.Drawing.Size(75, 23);
            this.BtnCancle.TabIndex = 2;
            this.BtnCancle.Text = "Cancel";
            this.BtnCancle.UseVisualStyleBackColor = true;
            this.BtnCancle.Click += new System.EventHandler(this.BtnCancle_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(8, 152);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // EmailAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 206);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailAddress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "EmailAddress";
            this.Shown += new System.EventHandler(this.EmailAddress_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox txtEmailAdd;
        private System.Windows.Forms.Button BtnCancle;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.CheckedListBox chkEmailAdd;
    }
}