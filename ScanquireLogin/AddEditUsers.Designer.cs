namespace ScanquireLogin
{
    partial class AddEditUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEditUsers));
            this.button4 = new System.Windows.Forms.Button();
            this.btnAddEditusers = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tabChangeUserInfo = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBoxUsers = new System.Windows.Forms.ComboBox();
            this.scanquireUsersControl2 = new ScanquireLogin.ScanquireUsersControl();
            this.tabPageAddUsers = new System.Windows.Forms.TabPage();
            this.scanquireUsersControl1 = new ScanquireLogin.ScanquireUsersControl();
            this.tabCntAddEditUsers = new System.Windows.Forms.TabControl();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabChangeUserInfo.SuspendLayout();
            this.tabPageAddUsers.SuspendLayout();
            this.tabCntAddEditUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(0, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 0;
            // 
            // btnAddEditusers
            // 
            this.btnAddEditusers.BackColor = System.Drawing.Color.LightBlue;
            this.btnAddEditusers.Location = new System.Drawing.Point(34, 316);
            this.btnAddEditusers.Name = "btnAddEditusers";
            this.btnAddEditusers.Size = new System.Drawing.Size(75, 23);
            this.btnAddEditusers.TabIndex = 1;
            this.btnAddEditusers.Text = "&Add User";
            this.btnAddEditusers.UseVisualStyleBackColor = false;
            this.btnAddEditusers.Click += new System.EventHandler(this.Button1_Click_1);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.LightBlue;
            this.btnReset.Location = new System.Drawing.Point(115, 316);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "&Clear";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // tabChangeUserInfo
            // 
            this.tabChangeUserInfo.BackColor = System.Drawing.Color.LightGray;
            this.tabChangeUserInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabChangeUserInfo.Controls.Add(this.linkLabel1);
            this.tabChangeUserInfo.Controls.Add(this.label1);
            this.tabChangeUserInfo.Controls.Add(this.cmbBoxUsers);
            this.tabChangeUserInfo.Controls.Add(this.scanquireUsersControl2);
            this.tabChangeUserInfo.Location = new System.Drawing.Point(4, 22);
            this.tabChangeUserInfo.Name = "tabChangeUserInfo";
            this.tabChangeUserInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabChangeUserInfo.Size = new System.Drawing.Size(339, 355);
            this.tabChangeUserInfo.TabIndex = 1;
            this.tabChangeUserInfo.Text = "Update-Delete";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.DarkSlateGray;
            this.linkLabel1.Location = new System.Drawing.Point(92, 325);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(87, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "&Reset PassWord";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select User:";
            // 
            // cmbBoxUsers
            // 
            this.cmbBoxUsers.AllowDrop = true;
            this.cmbBoxUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxUsers.FormattingEnabled = true;
            this.cmbBoxUsers.Location = new System.Drawing.Point(123, 14);
            this.cmbBoxUsers.Name = "cmbBoxUsers";
            this.cmbBoxUsers.Size = new System.Drawing.Size(165, 21);
            this.cmbBoxUsers.TabIndex = 1;
            this.cmbBoxUsers.SelectedIndexChanged += new System.EventHandler(this.CmbBoxUsers_SelectedIndexChanged);
            // 
            // scanquireUsersControl2
            // 
            this.scanquireUsersControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scanquireUsersControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanquireUsersControl2.Location = new System.Drawing.Point(3, 3);
            this.scanquireUsersControl2.Name = "scanquireUsersControl2";
            this.scanquireUsersControl2.Size = new System.Drawing.Size(329, 345);
            this.scanquireUsersControl2.TabIndex = 0;
            // 
            // tabPageAddUsers
            // 
            this.tabPageAddUsers.BackColor = System.Drawing.Color.LightGray;
            this.tabPageAddUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageAddUsers.Controls.Add(this.scanquireUsersControl1);
            this.tabPageAddUsers.ForeColor = System.Drawing.Color.Black;
            this.tabPageAddUsers.Location = new System.Drawing.Point(4, 22);
            this.tabPageAddUsers.Name = "tabPageAddUsers";
            this.tabPageAddUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAddUsers.Size = new System.Drawing.Size(339, 355);
            this.tabPageAddUsers.TabIndex = 0;
            this.tabPageAddUsers.Text = "Add Users";
            // 
            // scanquireUsersControl1
            // 
            this.scanquireUsersControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.scanquireUsersControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scanquireUsersControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanquireUsersControl1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.scanquireUsersControl1.Location = new System.Drawing.Point(3, 3);
            this.scanquireUsersControl1.Name = "scanquireUsersControl1";
            this.scanquireUsersControl1.Size = new System.Drawing.Size(329, 345);
            this.scanquireUsersControl1.TabIndex = 0;
            // 
            // tabCntAddEditUsers
            // 
            this.tabCntAddEditUsers.Controls.Add(this.tabPageAddUsers);
            this.tabCntAddEditUsers.Controls.Add(this.tabChangeUserInfo);
            this.tabCntAddEditUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCntAddEditUsers.Location = new System.Drawing.Point(0, 0);
            this.tabCntAddEditUsers.Name = "tabCntAddEditUsers";
            this.tabCntAddEditUsers.SelectedIndex = 0;
            this.tabCntAddEditUsers.Size = new System.Drawing.Size(347, 381);
            this.tabCntAddEditUsers.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabCntAddEditUsers.TabIndex = 0;
            this.tabCntAddEditUsers.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabCntAddEditUsers_Selected);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightBlue;
            this.btnClose.Location = new System.Drawing.Point(196, 316);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "&Exit";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // AddEditUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(347, 381);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAddEditusers);
            this.Controls.Add(this.tabCntAddEditUsers);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AddEditUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Edit Delete Users";
            this.Shown += new System.EventHandler(this.AddEditUsers_Shown);
            this.tabChangeUserInfo.ResumeLayout(false);
            this.tabChangeUserInfo.PerformLayout();
            this.tabPageAddUsers.ResumeLayout(false);
            this.tabCntAddEditUsers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    

        #endregion
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnAddEditusers;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tabChangeUserInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBoxUsers;
        private ScanquireUsersControl scanquireUsersControl2;
        private System.Windows.Forms.TabPage tabPageAddUsers;
        private ScanquireUsersControl scanquireUsersControl1;
        private System.Windows.Forms.TabControl tabCntAddEditUsers;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}