namespace BinMonitor.Common
{
    partial class UserManagerControl
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
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.gpExistingUsers = new System.Windows.Forms.GroupBox();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.txtSelectedUserCardId = new System.Windows.Forms.TextBox();
            this.lblSelectedUserCardId = new System.Windows.Forms.Label();
            this.cmbSelectedUserProfile = new BinMonitor.Common.UserProfilesComboBox();
            this.cmbExistingUsers = new BinMonitor.Common.UsersComboBox();
            this.btnUpdateSelectedUser = new System.Windows.Forms.Button();
            this.lblSelectedUserProfileName = new System.Windows.Forms.Label();
            this.gpCreateUser = new System.Windows.Forms.GroupBox();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewUserCardId = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtNewUserFirstName = new System.Windows.Forms.TextBox();
            this.lblCardId = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.cmbNewUserProfile = new BinMonitor.Common.UserProfilesComboBox();
            this.txtNewUserLastName = new System.Windows.Forms.TextBox();
            this.lblNewUserID = new System.Windows.Forms.Label();
            this.lblNewUserProfileName = new System.Windows.Forms.Label();
            this.txtNewUserId = new System.Windows.Forms.TextBox();
            this.btnAddNewUser = new System.Windows.Forms.Button();
            this.NewUserErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ExistingUserErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlScroll.SuspendLayout();
            this.gpExistingUsers.SuspendLayout();
            this.gpCreateUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewUserErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExistingUserErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlScroll
            // 
            this.pnlScroll.Controls.Add(this.gpExistingUsers);
            this.pnlScroll.Controls.Add(this.gpCreateUser);
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(304, 416);
            this.pnlScroll.TabIndex = 0;
            // 
            // gpExistingUsers
            // 
            this.gpExistingUsers.Controls.Add(this.btnDeleteUser);
            this.gpExistingUsers.Controls.Add(this.txtSelectedUserCardId);
            this.gpExistingUsers.Controls.Add(this.lblSelectedUserCardId);
            this.gpExistingUsers.Controls.Add(this.cmbSelectedUserProfile);
            this.gpExistingUsers.Controls.Add(this.cmbExistingUsers);
            this.gpExistingUsers.Controls.Add(this.btnUpdateSelectedUser);
            this.gpExistingUsers.Controls.Add(this.lblSelectedUserProfileName);
            this.gpExistingUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpExistingUsers.Location = new System.Drawing.Point(0, 210);
            this.gpExistingUsers.Name = "gpExistingUsers";
            this.gpExistingUsers.Size = new System.Drawing.Size(304, 174);
            this.gpExistingUsers.TabIndex = 26;
            this.gpExistingUsers.TabStop = false;
            this.gpExistingUsers.Text = "Existing Users";
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDeleteUser.Location = new System.Drawing.Point(8, 120);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(268, 21);
            this.btnDeleteUser.TabIndex = 32;
            this.btnDeleteUser.Text = "Delete User";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // txtSelectedUserCardId
            // 
            this.txtSelectedUserCardId.Location = new System.Drawing.Point(72, 45);
            this.txtSelectedUserCardId.Name = "txtSelectedUserCardId";
            this.txtSelectedUserCardId.Size = new System.Drawing.Size(210, 20);
            this.txtSelectedUserCardId.TabIndex = 7;
            this.txtSelectedUserCardId.TabStop = false;
            this.txtSelectedUserCardId.UseSystemPasswordChar = true;
            // 
            // lblSelectedUserCardId
            // 
            this.lblSelectedUserCardId.AutoSize = true;
            this.lblSelectedUserCardId.Location = new System.Drawing.Point(17, 47);
            this.lblSelectedUserCardId.Name = "lblSelectedUserCardId";
            this.lblSelectedUserCardId.Size = new System.Drawing.Size(53, 13);
            this.lblSelectedUserCardId.TabIndex = 31;
            this.lblSelectedUserCardId.Text = "Password";
            this.lblSelectedUserCardId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbSelectedUserProfile
            // 
            this.cmbSelectedUserProfile.Location = new System.Drawing.Point(72, 68);
            this.cmbSelectedUserProfile.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSelectedUserProfile.Name = "cmbSelectedUserProfile";
            this.cmbSelectedUserProfile.Size = new System.Drawing.Size(209, 21);
            this.cmbSelectedUserProfile.TabIndex = 8;
            // 
            // cmbExistingUsers
            // 
            this.cmbExistingUsers.BackColor = System.Drawing.SystemColors.Control;
            this.cmbExistingUsers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbExistingUsers.Location = new System.Drawing.Point(13, 21);
            this.cmbExistingUsers.Margin = new System.Windows.Forms.Padding(0);
            this.cmbExistingUsers.Name = "cmbExistingUsers";
            this.cmbExistingUsers.Size = new System.Drawing.Size(268, 21);
            this.cmbExistingUsers.TabIndex = 6;
            this.cmbExistingUsers.SelectedUserChanged += new System.EventHandler<BinMonitor.Common.SelectedUserChangedEventArgs>(this.cmbExistingUsers_SelectedUserChanged);
            // 
            // btnUpdateSelectedUser
            // 
            this.btnUpdateSelectedUser.Location = new System.Drawing.Point(8, 96);
            this.btnUpdateSelectedUser.Name = "btnUpdateSelectedUser";
            this.btnUpdateSelectedUser.Size = new System.Drawing.Size(268, 21);
            this.btnUpdateSelectedUser.TabIndex = 9;
            this.btnUpdateSelectedUser.Text = "Update User";
            this.btnUpdateSelectedUser.UseVisualStyleBackColor = true;
            this.btnUpdateSelectedUser.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblSelectedUserProfileName
            // 
            this.lblSelectedUserProfileName.AutoSize = true;
            this.lblSelectedUserProfileName.Location = new System.Drawing.Point(30, 72);
            this.lblSelectedUserProfileName.Name = "lblSelectedUserProfileName";
            this.lblSelectedUserProfileName.Size = new System.Drawing.Size(36, 13);
            this.lblSelectedUserProfileName.TabIndex = 18;
            this.lblSelectedUserProfileName.Text = "Profile";
            this.lblSelectedUserProfileName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gpCreateUser
            // 
            this.gpCreateUser.Controls.Add(this.txtEmailAddress);
            this.gpCreateUser.Controls.Add(this.label1);
            this.gpCreateUser.Controls.Add(this.txtNewUserCardId);
            this.gpCreateUser.Controls.Add(this.lblLastName);
            this.gpCreateUser.Controls.Add(this.txtNewUserFirstName);
            this.gpCreateUser.Controls.Add(this.lblCardId);
            this.gpCreateUser.Controls.Add(this.lblFirstName);
            this.gpCreateUser.Controls.Add(this.cmbNewUserProfile);
            this.gpCreateUser.Controls.Add(this.txtNewUserLastName);
            this.gpCreateUser.Controls.Add(this.lblNewUserID);
            this.gpCreateUser.Controls.Add(this.lblNewUserProfileName);
            this.gpCreateUser.Controls.Add(this.txtNewUserId);
            this.gpCreateUser.Controls.Add(this.btnAddNewUser);
            this.gpCreateUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpCreateUser.Location = new System.Drawing.Point(0, 0);
            this.gpCreateUser.Name = "gpCreateUser";
            this.gpCreateUser.Size = new System.Drawing.Size(304, 210);
            this.gpCreateUser.TabIndex = 25;
            this.gpCreateUser.TabStop = false;
            this.gpCreateUser.Text = "Create New User";
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(76, 99);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(209, 20);
            this.txtEmailAddress.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Email Address";
            // 
            // txtNewUserCardId
            // 
            this.txtNewUserCardId.Location = new System.Drawing.Point(73, 124);
            this.txtNewUserCardId.Name = "txtNewUserCardId";
            this.txtNewUserCardId.Size = new System.Drawing.Size(209, 20);
            this.txtNewUserCardId.TabIndex = 3;
            this.txtNewUserCardId.UseSystemPasswordChar = true;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(1, 49);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 24;
            this.lblLastName.Text = "Last Name";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNewUserFirstName
            // 
            this.txtNewUserFirstName.Location = new System.Drawing.Point(75, 74);
            this.txtNewUserFirstName.Name = "txtNewUserFirstName";
            this.txtNewUserFirstName.Size = new System.Drawing.Size(209, 20);
            this.txtNewUserFirstName.TabIndex = 2;
            // 
            // lblCardId
            // 
            this.lblCardId.AutoSize = true;
            this.lblCardId.Location = new System.Drawing.Point(6, 128);
            this.lblCardId.Name = "lblCardId";
            this.lblCardId.Size = new System.Drawing.Size(53, 13);
            this.lblCardId.TabIndex = 29;
            this.lblCardId.Text = "Password";
            this.lblCardId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(2, 76);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 26;
            this.lblFirstName.Text = "First Name";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbNewUserProfile
            // 
            this.cmbNewUserProfile.BackColor = System.Drawing.SystemColors.Control;
            this.cmbNewUserProfile.Location = new System.Drawing.Point(65, 152);
            this.cmbNewUserProfile.Margin = new System.Windows.Forms.Padding(0);
            this.cmbNewUserProfile.Name = "cmbNewUserProfile";
            this.cmbNewUserProfile.Size = new System.Drawing.Size(209, 21);
            this.cmbNewUserProfile.TabIndex = 4;
            // 
            // txtNewUserLastName
            // 
            this.txtNewUserLastName.Location = new System.Drawing.Point(75, 49);
            this.txtNewUserLastName.Name = "txtNewUserLastName";
            this.txtNewUserLastName.Size = new System.Drawing.Size(209, 20);
            this.txtNewUserLastName.TabIndex = 1;
            // 
            // lblNewUserID
            // 
            this.lblNewUserID.AutoSize = true;
            this.lblNewUserID.Location = new System.Drawing.Point(-3, 27);
            this.lblNewUserID.Name = "lblNewUserID";
            this.lblNewUserID.Size = new System.Drawing.Size(78, 13);
            this.lblNewUserID.TabIndex = 10;
            this.lblNewUserID.Text = "User ID(CWID)";
            this.lblNewUserID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNewUserProfileName
            // 
            this.lblNewUserProfileName.AutoSize = true;
            this.lblNewUserProfileName.Location = new System.Drawing.Point(23, 156);
            this.lblNewUserProfileName.Name = "lblNewUserProfileName";
            this.lblNewUserProfileName.Size = new System.Drawing.Size(36, 13);
            this.lblNewUserProfileName.TabIndex = 27;
            this.lblNewUserProfileName.Text = "Profile";
            this.lblNewUserProfileName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNewUserId
            // 
            this.txtNewUserId.Location = new System.Drawing.Point(75, 24);
            this.txtNewUserId.Name = "txtNewUserId";
            this.txtNewUserId.Size = new System.Drawing.Size(119, 20);
            this.txtNewUserId.TabIndex = 0;
            this.txtNewUserId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewUserId_KeyPress);
            // 
            // btnAddNewUser
            // 
            this.btnAddNewUser.Location = new System.Drawing.Point(3, 179);
            this.btnAddNewUser.Name = "btnAddNewUser";
            this.btnAddNewUser.Size = new System.Drawing.Size(272, 23);
            this.btnAddNewUser.TabIndex = 5;
            this.btnAddNewUser.Text = "Add New User";
            this.btnAddNewUser.UseVisualStyleBackColor = true;
            this.btnAddNewUser.Click += new System.EventHandler(this.btnAddNewUser_Click);
            // 
            // NewUserErrorProvider
            // 
            this.NewUserErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.NewUserErrorProvider.ContainerControl = this;
            // 
            // ExistingUserErrorProvider
            // 
            this.ExistingUserErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ExistingUserErrorProvider.ContainerControl = this;
            // 
            // UserManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScroll);
            this.Name = "UserManagerControl";
            this.Size = new System.Drawing.Size(304, 378);
            this.pnlScroll.ResumeLayout(false);
            this.gpExistingUsers.ResumeLayout(false);
            this.gpExistingUsers.PerformLayout();
            this.gpCreateUser.ResumeLayout(false);
            this.gpCreateUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewUserErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExistingUserErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.Label lblSelectedUserProfileName;
        private System.Windows.Forms.Button btnUpdateSelectedUser;
        private System.Windows.Forms.TextBox txtNewUserId;
        private System.Windows.Forms.Label lblNewUserID;
        private System.Windows.Forms.Button btnAddNewUser;
        private System.Windows.Forms.GroupBox gpExistingUsers;
        private System.Windows.Forms.GroupBox gpCreateUser;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtNewUserFirstName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtNewUserLastName;
        private UserProfilesComboBox cmbSelectedUserProfile;
        private UsersComboBox cmbExistingUsers;
        private UserProfilesComboBox cmbNewUserProfile;
        private System.Windows.Forms.Label lblNewUserProfileName;
        private System.Windows.Forms.ErrorProvider NewUserErrorProvider;
        private System.Windows.Forms.ErrorProvider ExistingUserErrorProvider;
        private System.Windows.Forms.TextBox txtSelectedUserCardId;
        private System.Windows.Forms.Label lblSelectedUserCardId;
        private System.Windows.Forms.TextBox txtNewUserCardId;
        private System.Windows.Forms.Label lblCardId;
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteUser;
    }
}
