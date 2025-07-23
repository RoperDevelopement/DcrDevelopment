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
            this.btnRequestCardId = new System.Windows.Forms.Button();
            this.txtSelectedUserCardId = new System.Windows.Forms.TextBox();
            this.lblSelectedUserCardId = new System.Windows.Forms.Label();
            this.cmbSelectedUserProfile = new BinMonitor.Common.UserProfilesComboBox();
            this.cmbExistingUsers = new BinMonitor.Common.UsersComboBox();
            this.btnUpdateSelectedUser = new System.Windows.Forms.Button();
            this.btnDeleteSelectedUser = new System.Windows.Forms.Button();
            this.lblSelectedUserProfileName = new System.Windows.Forms.Label();
            this.gpCreateUser = new System.Windows.Forms.GroupBox();
            this.txtNewUserCardId = new System.Windows.Forms.TextBox();
            this.lblCardId = new System.Windows.Forms.Label();
            this.cmbNewUserProfile = new BinMonitor.Common.UserProfilesComboBox();
            this.lblNewUserProfileName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtNewUserFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtNewUserLastName = new System.Windows.Forms.TextBox();
            this.lblNewUserID = new System.Windows.Forms.Label();
            this.btnAddNewUser = new System.Windows.Forms.Button();
            this.txtNewUserId = new System.Windows.Forms.TextBox();
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
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Margin = new System.Windows.Forms.Padding(4);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(392, 417);
            this.pnlScroll.TabIndex = 0;
            // 
            // gpExistingUsers
            // 
            this.gpExistingUsers.Controls.Add(this.btnRequestCardId);
            this.gpExistingUsers.Controls.Add(this.txtSelectedUserCardId);
            this.gpExistingUsers.Controls.Add(this.lblSelectedUserCardId);
            this.gpExistingUsers.Controls.Add(this.cmbSelectedUserProfile);
            this.gpExistingUsers.Controls.Add(this.cmbExistingUsers);
            this.gpExistingUsers.Controls.Add(this.btnUpdateSelectedUser);
            this.gpExistingUsers.Controls.Add(this.btnDeleteSelectedUser);
            this.gpExistingUsers.Controls.Add(this.lblSelectedUserProfileName);
            this.gpExistingUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpExistingUsers.Location = new System.Drawing.Point(0, 224);
            this.gpExistingUsers.Margin = new System.Windows.Forms.Padding(4);
            this.gpExistingUsers.Name = "gpExistingUsers";
            this.gpExistingUsers.Padding = new System.Windows.Forms.Padding(4);
            this.gpExistingUsers.Size = new System.Drawing.Size(392, 192);
            this.gpExistingUsers.TabIndex = 26;
            this.gpExistingUsers.TabStop = false;
            this.gpExistingUsers.Text = "Existing Users";
            // 
            // btnRequestCardId
            // 
            this.btnRequestCardId.Location = new System.Drawing.Point(324, 53);
            this.btnRequestCardId.Margin = new System.Windows.Forms.Padding(4);
            this.btnRequestCardId.Name = "btnRequestCardId";
            this.btnRequestCardId.Size = new System.Drawing.Size(51, 28);
            this.btnRequestCardId.TabIndex = 7;
            this.btnRequestCardId.Text = "...";
            this.btnRequestCardId.UseVisualStyleBackColor = true;
            this.btnRequestCardId.Click += new System.EventHandler(this.btnRequestCardId_Click);
            // 
            // txtSelectedUserCardId
            // 
            this.txtSelectedUserCardId.Location = new System.Drawing.Point(96, 55);
            this.txtSelectedUserCardId.Margin = new System.Windows.Forms.Padding(4);
            this.txtSelectedUserCardId.Name = "txtSelectedUserCardId";
            this.txtSelectedUserCardId.Size = new System.Drawing.Size(219, 22);
            this.txtSelectedUserCardId.TabIndex = 7;
            this.txtSelectedUserCardId.TabStop = false;
            this.txtSelectedUserCardId.UseSystemPasswordChar = true;
            // 
            // lblSelectedUserCardId
            // 
            this.lblSelectedUserCardId.AutoSize = true;
            this.lblSelectedUserCardId.Location = new System.Drawing.Point(23, 58);
            this.lblSelectedUserCardId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedUserCardId.Name = "lblSelectedUserCardId";
            this.lblSelectedUserCardId.Size = new System.Drawing.Size(69, 17);
            this.lblSelectedUserCardId.TabIndex = 31;
            this.lblSelectedUserCardId.Text = "Password";
            this.lblSelectedUserCardId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbSelectedUserProfile
            // 
            this.cmbSelectedUserProfile.Location = new System.Drawing.Point(96, 84);
            this.cmbSelectedUserProfile.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSelectedUserProfile.Name = "cmbSelectedUserProfile";
            this.cmbSelectedUserProfile.Size = new System.Drawing.Size(279, 26);
            this.cmbSelectedUserProfile.TabIndex = 8;
            // 
            // cmbExistingUsers
            // 
            this.cmbExistingUsers.Location = new System.Drawing.Point(17, 26);
            this.cmbExistingUsers.Margin = new System.Windows.Forms.Padding(0);
            this.cmbExistingUsers.Name = "cmbExistingUsers";
            this.cmbExistingUsers.Size = new System.Drawing.Size(357, 26);
            this.cmbExistingUsers.TabIndex = 6;
            this.cmbExistingUsers.SelectedUserChanged += new System.EventHandler<BinMonitor.Common.SelectedUserChangedEventArgs>(this.cmbExistingUsers_SelectedUserChanged);
            // 
            // btnUpdateSelectedUser
            // 
            this.btnUpdateSelectedUser.Location = new System.Drawing.Point(17, 113);
            this.btnUpdateSelectedUser.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateSelectedUser.Name = "btnUpdateSelectedUser";
            this.btnUpdateSelectedUser.Size = new System.Drawing.Size(357, 28);
            this.btnUpdateSelectedUser.TabIndex = 9;
            this.btnUpdateSelectedUser.Text = "Update";
            this.btnUpdateSelectedUser.UseVisualStyleBackColor = true;
            this.btnUpdateSelectedUser.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDeleteSelectedUser
            // 
            this.btnDeleteSelectedUser.Location = new System.Drawing.Point(17, 149);
            this.btnDeleteSelectedUser.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteSelectedUser.Name = "btnDeleteSelectedUser";
            this.btnDeleteSelectedUser.Size = new System.Drawing.Size(357, 28);
            this.btnDeleteSelectedUser.TabIndex = 10;
            this.btnDeleteSelectedUser.TabStop = false;
            this.btnDeleteSelectedUser.Text = "Delete";
            this.btnDeleteSelectedUser.UseVisualStyleBackColor = true;
            this.btnDeleteSelectedUser.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblSelectedUserProfileName
            // 
            this.lblSelectedUserProfileName.AutoSize = true;
            this.lblSelectedUserProfileName.Location = new System.Drawing.Point(40, 89);
            this.lblSelectedUserProfileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedUserProfileName.Name = "lblSelectedUserProfileName";
            this.lblSelectedUserProfileName.Size = new System.Drawing.Size(48, 17);
            this.lblSelectedUserProfileName.TabIndex = 18;
            this.lblSelectedUserProfileName.Text = "Profile";
            this.lblSelectedUserProfileName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gpCreateUser
            // 
            this.gpCreateUser.Controls.Add(this.txtNewUserCardId);
            this.gpCreateUser.Controls.Add(this.lblCardId);
            this.gpCreateUser.Controls.Add(this.cmbNewUserProfile);
            this.gpCreateUser.Controls.Add(this.lblNewUserProfileName);
            this.gpCreateUser.Controls.Add(this.lblLastName);
            this.gpCreateUser.Controls.Add(this.txtNewUserFirstName);
            this.gpCreateUser.Controls.Add(this.lblFirstName);
            this.gpCreateUser.Controls.Add(this.txtNewUserLastName);
            this.gpCreateUser.Controls.Add(this.lblNewUserID);
            this.gpCreateUser.Controls.Add(this.btnAddNewUser);
            this.gpCreateUser.Controls.Add(this.txtNewUserId);
            this.gpCreateUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpCreateUser.Location = new System.Drawing.Point(0, 0);
            this.gpCreateUser.Margin = new System.Windows.Forms.Padding(4);
            this.gpCreateUser.Name = "gpCreateUser";
            this.gpCreateUser.Padding = new System.Windows.Forms.Padding(4);
            this.gpCreateUser.Size = new System.Drawing.Size(392, 224);
            this.gpCreateUser.TabIndex = 25;
            this.gpCreateUser.TabStop = false;
            this.gpCreateUser.Text = "Create New User";
            // 
            // txtNewUserCardId
            // 
            this.txtNewUserCardId.Location = new System.Drawing.Point(100, 123);
            this.txtNewUserCardId.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewUserCardId.Name = "txtNewUserCardId";
            this.txtNewUserCardId.Size = new System.Drawing.Size(277, 22);
            this.txtNewUserCardId.TabIndex = 3;
            this.txtNewUserCardId.UseSystemPasswordChar = true;
            // 
            // lblCardId
            // 
            this.lblCardId.AutoSize = true;
            this.lblCardId.Location = new System.Drawing.Point(23, 126);
            this.lblCardId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCardId.Name = "lblCardId";
            this.lblCardId.Size = new System.Drawing.Size(69, 17);
            this.lblCardId.TabIndex = 29;
            this.lblCardId.Text = "Password";
            this.lblCardId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbNewUserProfile
            // 
            this.cmbNewUserProfile.Location = new System.Drawing.Point(100, 155);
            this.cmbNewUserProfile.Margin = new System.Windows.Forms.Padding(0);
            this.cmbNewUserProfile.Name = "cmbNewUserProfile";
            this.cmbNewUserProfile.Size = new System.Drawing.Size(279, 26);
            this.cmbNewUserProfile.TabIndex = 4;
            // 
            // lblNewUserProfileName
            // 
            this.lblNewUserProfileName.AutoSize = true;
            this.lblNewUserProfileName.Location = new System.Drawing.Point(44, 160);
            this.lblNewUserProfileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewUserProfileName.Name = "lblNewUserProfileName";
            this.lblNewUserProfileName.Size = new System.Drawing.Size(48, 17);
            this.lblNewUserProfileName.TabIndex = 27;
            this.lblNewUserProfileName.Text = "Profile";
            this.lblNewUserProfileName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(15, 60);
            this.lblLastName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(76, 17);
            this.lblLastName.TabIndex = 24;
            this.lblLastName.Text = "Last Name";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNewUserFirstName
            // 
            this.txtNewUserFirstName.Location = new System.Drawing.Point(100, 90);
            this.txtNewUserFirstName.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewUserFirstName.Name = "txtNewUserFirstName";
            this.txtNewUserFirstName.Size = new System.Drawing.Size(277, 22);
            this.txtNewUserFirstName.TabIndex = 2;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(16, 94);
            this.lblFirstName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(76, 17);
            this.lblFirstName.TabIndex = 26;
            this.lblFirstName.Text = "First Name";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNewUserLastName
            // 
            this.txtNewUserLastName.Location = new System.Drawing.Point(100, 57);
            this.txtNewUserLastName.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewUserLastName.Name = "txtNewUserLastName";
            this.txtNewUserLastName.Size = new System.Drawing.Size(277, 22);
            this.txtNewUserLastName.TabIndex = 1;
            // 
            // lblNewUserID
            // 
            this.lblNewUserID.AutoSize = true;
            this.lblNewUserID.Location = new System.Drawing.Point(35, 27);
            this.lblNewUserID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewUserID.Name = "lblNewUserID";
            this.lblNewUserID.Size = new System.Drawing.Size(55, 17);
            this.lblNewUserID.TabIndex = 11;
            this.lblNewUserID.Text = "User ID";
            this.lblNewUserID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnAddNewUser
            // 
            this.btnAddNewUser.Location = new System.Drawing.Point(39, 188);
            this.btnAddNewUser.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddNewUser.Name = "btnAddNewUser";
            this.btnAddNewUser.Size = new System.Drawing.Size(327, 28);
            this.btnAddNewUser.TabIndex = 5;
            this.btnAddNewUser.Text = "Add New User";
            this.btnAddNewUser.UseVisualStyleBackColor = true;
            this.btnAddNewUser.Click += new System.EventHandler(this.btnAddNewUser_Click);
            // 
            // txtNewUserId
            // 
            this.txtNewUserId.Location = new System.Drawing.Point(100, 23);
            this.txtNewUserId.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewUserId.Name = "txtNewUserId";
            this.txtNewUserId.Size = new System.Drawing.Size(132, 22);
            this.txtNewUserId.TabIndex = 0;
            this.txtNewUserId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewUserId_KeyPress);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScroll);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserManagerControl";
            this.Size = new System.Drawing.Size(392, 417);
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
        private System.Windows.Forms.Button btnDeleteSelectedUser;
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
        private System.Windows.Forms.Button btnRequestCardId;

    }
}
