namespace ScanquireLogin
{
    partial class ScanquireUsersControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTipTxtBoxes = new System.Windows.Forms.ToolTip(this.components);
            this.chkBoxIsAdmin = new System.Windows.Forms.CheckBox();
            this.txtBoxFName = new System.Windows.Forms.TextBox();
            this.txtBoxVerifyPassWord = new System.Windows.Forms.TextBox();
            this.txtBoxLoginId = new System.Windows.Forms.TextBox();
            this.txtBoxPassWord = new System.Windows.Forms.TextBox();
            this.txtBoxEmailAddress = new System.Windows.Forms.TextBox();
            this.txtBoxLName = new System.Windows.Forms.TextBox();
            this.chkBoxShowPassWord = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labPw = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labVerPW = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTipTxtBoxes
            // 
            this.toolTipTxtBoxes.AutomaticDelay = 100;
            this.toolTipTxtBoxes.IsBalloon = true;
            this.toolTipTxtBoxes.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // chkBoxIsAdmin
            // 
            this.chkBoxIsAdmin.AutoSize = true;
            this.chkBoxIsAdmin.Location = new System.Drawing.Point(33, 217);
            this.chkBoxIsAdmin.Name = "chkBoxIsAdmin";
            this.chkBoxIsAdmin.Size = new System.Drawing.Size(80, 17);
            this.chkBoxIsAdmin.TabIndex = 7;
            this.chkBoxIsAdmin.Text = "User Admin";
            this.toolTipTxtBoxes.SetToolTip(this.chkBoxIsAdmin, "User Admin");
            this.chkBoxIsAdmin.UseVisualStyleBackColor = true;
            this.chkBoxIsAdmin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkBoxIsAdmin_MouseUp);
            // 
            // txtBoxFName
            // 
            this.txtBoxFName.Location = new System.Drawing.Point(121, 39);
            this.txtBoxFName.MaxLength = 40;
            this.txtBoxFName.Name = "txtBoxFName";
            this.txtBoxFName.Size = new System.Drawing.Size(146, 20);
            this.txtBoxFName.TabIndex = 1;
            this.toolTipTxtBoxes.SetToolTip(this.txtBoxFName, "User First Name");
            this.txtBoxFName.Enter += new System.EventHandler(this.TxtBoxFName_Enter);
            this.txtBoxFName.MouseHover += new System.EventHandler(this.TxtBoxFName_MouseHover);
            // 
            // txtBoxVerifyPassWord
            // 
            this.txtBoxVerifyPassWord.Location = new System.Drawing.Point(121, 179);
            this.txtBoxVerifyPassWord.MaxLength = 50;
            this.txtBoxVerifyPassWord.Name = "txtBoxVerifyPassWord";
            this.txtBoxVerifyPassWord.PasswordChar = '*';
            this.txtBoxVerifyPassWord.Size = new System.Drawing.Size(146, 20);
            this.txtBoxVerifyPassWord.TabIndex = 6;
            this.toolTipTxtBoxes.SetToolTip(this.txtBoxVerifyPassWord, "Verify User Passeord");
            // 
            // txtBoxLoginId
            // 
            this.txtBoxLoginId.Location = new System.Drawing.Point(121, 123);
            this.txtBoxLoginId.MaxLength = 50;
            this.txtBoxLoginId.Name = "txtBoxLoginId";
            this.txtBoxLoginId.Size = new System.Drawing.Size(146, 20);
            this.txtBoxLoginId.TabIndex = 4;
            this.toolTipTxtBoxes.SetToolTip(this.txtBoxLoginId, "Login ID");
            this.txtBoxLoginId.Enter += new System.EventHandler(this.TxtBoxLoginId_Enter);
            // 
            // txtBoxPassWord
            // 
            this.txtBoxPassWord.Location = new System.Drawing.Point(121, 151);
            this.txtBoxPassWord.MaxLength = 50;
            this.txtBoxPassWord.Name = "txtBoxPassWord";
            this.txtBoxPassWord.PasswordChar = '*';
            this.txtBoxPassWord.Size = new System.Drawing.Size(146, 20);
            this.txtBoxPassWord.TabIndex = 5;
            this.toolTipTxtBoxes.SetToolTip(this.txtBoxPassWord, "User Password");
            this.txtBoxPassWord.Enter += new System.EventHandler(this.TxtBoxPassWord_Enter);
            // 
            // txtBoxEmailAddress
            // 
            this.txtBoxEmailAddress.Location = new System.Drawing.Point(121, 95);
            this.txtBoxEmailAddress.MaxLength = 50;
            this.txtBoxEmailAddress.Name = "txtBoxEmailAddress";
            this.txtBoxEmailAddress.Size = new System.Drawing.Size(146, 20);
            this.txtBoxEmailAddress.TabIndex = 3;
            this.toolTipTxtBoxes.SetToolTip(this.txtBoxEmailAddress, "Email Address email@.com");
            this.txtBoxEmailAddress.Enter += new System.EventHandler(this.TxtBoxEmailAddress_Enter);
            // 
            // txtBoxLName
            // 
            this.txtBoxLName.Location = new System.Drawing.Point(121, 67);
            this.txtBoxLName.MaxLength = 40;
            this.txtBoxLName.Name = "txtBoxLName";
            this.txtBoxLName.Size = new System.Drawing.Size(146, 20);
            this.txtBoxLName.TabIndex = 2;
            this.toolTipTxtBoxes.SetToolTip(this.txtBoxLName, "User Last Name");
            this.txtBoxLName.Enter += new System.EventHandler(this.TxtBoxLName_Enter);
            this.txtBoxLName.MouseHover += new System.EventHandler(this.TxtBoxLName_MouseHover);
            // 
            // chkBoxShowPassWord
            // 
            this.chkBoxShowPassWord.AutoSize = true;
            this.chkBoxShowPassWord.Location = new System.Drawing.Point(141, 217);
            this.chkBoxShowPassWord.Name = "chkBoxShowPassWord";
            this.chkBoxShowPassWord.Size = new System.Drawing.Size(105, 17);
            this.chkBoxShowPassWord.TabIndex = 8;
            this.chkBoxShowPassWord.Text = "Show PassWord";
            this.toolTipTxtBoxes.SetToolTip(this.chkBoxShowPassWord, "Show Password");
            this.chkBoxShowPassWord.UseVisualStyleBackColor = true;
            this.chkBoxShowPassWord.CheckedChanged += new System.EventHandler(this.ChkBoxShowPassWord_CheckedChanged);
            this.chkBoxShowPassWord.Enter += new System.EventHandler(this.ChkBoxShowPassWord_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User First Name:";
            // 
            // labPw
            // 
            this.labPw.AutoSize = true;
            this.labPw.Location = new System.Drawing.Point(30, 155);
            this.labPw.Name = "labPw";
            this.labPw.Size = new System.Drawing.Size(59, 13);
            this.labPw.TabIndex = 1;
            this.labPw.Text = "PassWord:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Logid ID:";
            // 
            // labVerPW
            // 
            this.labVerPW.AutoSize = true;
            this.labVerPW.Location = new System.Drawing.Point(30, 183);
            this.labVerPW.Name = "labVerPW";
            this.labVerPW.Size = new System.Drawing.Size(93, 13);
            this.labVerPW.TabIndex = 4;
            this.labVerPW.Text = "Verify PasssWord:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Emal Address:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "User Last Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightGray;
            this.groupBox1.Controls.Add(this.chkBoxShowPassWord);
            this.groupBox1.Controls.Add(this.txtBoxLName);
            this.groupBox1.Controls.Add(this.txtBoxEmailAddress);
            this.groupBox1.Controls.Add(this.txtBoxPassWord);
            this.groupBox1.Controls.Add(this.txtBoxLoginId);
            this.groupBox1.Controls.Add(this.txtBoxVerifyPassWord);
            this.groupBox1.Controls.Add(this.txtBoxFName);
            this.groupBox1.Controls.Add(this.chkBoxIsAdmin);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.labVerPW);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labPw);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 254);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // ScanquireUsersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.groupBox1);
            this.Name = "ScanquireUsersControl";
            this.Size = new System.Drawing.Size(270, 254);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipTxtBoxes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.CheckBox chkBoxIsAdmin;
        public System.Windows.Forms.TextBox txtBoxFName;
        public System.Windows.Forms.TextBox txtBoxVerifyPassWord;
        public System.Windows.Forms.TextBox txtBoxLoginId;
        public System.Windows.Forms.TextBox txtBoxPassWord;
        public System.Windows.Forms.TextBox txtBoxEmailAddress;
        public System.Windows.Forms.TextBox txtBoxLName;
        public System.Windows.Forms.CheckBox chkBoxShowPassWord;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label labPw;
        public System.Windows.Forms.Label labVerPW;
    }
}
