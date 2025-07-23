namespace ScanquireLogin
{
    partial class ResetPassWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResetPassWord));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxUserID = new System.Windows.Forms.TextBox();
            this.txtBoxNewPassword = new System.Windows.Forms.TextBox();
            this.chkBoxShowPassWord = new System.Windows.Forms.CheckBox();
            this.btnChangePw = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.chkBoxGeneratePassWord = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxVerifyPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "New Password:";
            // 
            // txtBoxUserID
            // 
            this.txtBoxUserID.Location = new System.Drawing.Point(153, 76);
            this.txtBoxUserID.Name = "txtBoxUserID";
            this.txtBoxUserID.ReadOnly = true;
            this.txtBoxUserID.Size = new System.Drawing.Size(198, 20);
            this.txtBoxUserID.TabIndex = 3;
            // 
            // txtBoxNewPassword
            // 
            this.txtBoxNewPassword.Location = new System.Drawing.Point(153, 113);
            this.txtBoxNewPassword.Name = "txtBoxNewPassword";
            this.txtBoxNewPassword.PasswordChar = '*';
            this.txtBoxNewPassword.Size = new System.Drawing.Size(198, 20);
            this.txtBoxNewPassword.TabIndex = 0;
            // 
            // chkBoxShowPassWord
            // 
            this.chkBoxShowPassWord.AutoSize = true;
            this.chkBoxShowPassWord.Location = new System.Drawing.Point(54, 179);
            this.chkBoxShowPassWord.Name = "chkBoxShowPassWord";
            this.chkBoxShowPassWord.Size = new System.Drawing.Size(105, 17);
            this.chkBoxShowPassWord.TabIndex = 3;
            this.chkBoxShowPassWord.Text = "&Show PassWord";
            this.chkBoxShowPassWord.UseVisualStyleBackColor = true;
            this.chkBoxShowPassWord.CheckStateChanged += new System.EventHandler(this.ChkBoxShowPassWord_CheckStateChanged);
            // 
            // btnChangePw
            // 
            this.btnChangePw.BackColor = System.Drawing.Color.LightBlue;
            this.btnChangePw.Location = new System.Drawing.Point(63, 213);
            this.btnChangePw.Name = "btnChangePw";
            this.btnChangePw.Size = new System.Drawing.Size(123, 23);
            this.btnChangePw.TabIndex = 2;
            this.btnChangePw.Text = "Change &PassWord";
            this.btnChangePw.UseVisualStyleBackColor = false;
            this.btnChangePw.Click += new System.EventHandler(this.BtnChangePw_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.BackColor = System.Drawing.Color.LightBlue;
            this.BtnClose.Location = new System.Drawing.Point(216, 213);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 5;
            this.BtnClose.Text = "&Cancel";
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // chkBoxGeneratePassWord
            // 
            this.chkBoxGeneratePassWord.AutoSize = true;
            this.chkBoxGeneratePassWord.Location = new System.Drawing.Point(176, 179);
            this.chkBoxGeneratePassWord.Name = "chkBoxGeneratePassWord";
            this.chkBoxGeneratePassWord.Size = new System.Drawing.Size(175, 17);
            this.chkBoxGeneratePassWord.TabIndex = 4;
            this.chkBoxGeneratePassWord.Text = "&Generate Email New PassWord";
            this.chkBoxGeneratePassWord.UseVisualStyleBackColor = true;
            this.chkBoxGeneratePassWord.CheckStateChanged += new System.EventHandler(this.ChkBoxGeneratePassWord_CheckStateChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Verify Password:";
            // 
            // txtBoxVerifyPassword
            // 
            this.txtBoxVerifyPassword.Location = new System.Drawing.Point(153, 142);
            this.txtBoxVerifyPassword.Name = "txtBoxVerifyPassword";
            this.txtBoxVerifyPassword.PasswordChar = '*';
            this.txtBoxVerifyPassword.Size = new System.Drawing.Size(198, 20);
            this.txtBoxVerifyPassword.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(61, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(272, 36);
            this.label5.TabIndex = 12;
            this.label5.Text = "Change Password";
            // 
            // ResetPassWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(378, 254);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoxVerifyPassword);
            this.Controls.Add(this.txtBoxNewPassword);
            this.Controls.Add(this.txtBoxUserID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkBoxGeneratePassWord);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.btnChangePw);
            this.Controls.Add(this.chkBoxShowPassWord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResetPassWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reset PassWord";
            this.Shown += new System.EventHandler(this.ResetPassWord_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBoxUserID;
        private System.Windows.Forms.TextBox txtBoxNewPassword;
        private System.Windows.Forms.CheckBox chkBoxShowPassWord;
        private System.Windows.Forms.Button btnChangePw;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.CheckBox chkBoxGeneratePassWord;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxVerifyPassword;
        private System.Windows.Forms.Label label5;
    }
}