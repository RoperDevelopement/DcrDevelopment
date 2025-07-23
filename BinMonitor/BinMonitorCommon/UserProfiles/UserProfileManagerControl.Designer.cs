namespace BinMonitor.Common
{
    partial class UserProfileManagerControl
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
            this.lblNewProfileName = new System.Windows.Forms.Label();
            this.txtNewProfileName = new System.Windows.Forms.TextBox();
            this.btnCreateNewProfile = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gpExistingProfiles = new System.Windows.Forms.GroupBox();
            this.pnlProfileSettings = new System.Windows.Forms.Panel();
            this.chkIsAdmin = new System.Windows.Forms.CheckBox();
            this.cmbExistingProfiles = new BinMonitor.Common.UserProfilesDropDown();
            this.permRoutine = new BinMonitor.Common.MasterCategoryPermissionControl();
            this.permProblem = new BinMonitor.Common.MasterCategoryPermissionControl();
            this.permReady = new BinMonitor.Common.MasterCategoryPermissionControl();
            this.permStat = new BinMonitor.Common.MasterCategoryPermissionControl();
            this.gpCreateNewProfile = new System.Windows.Forms.GroupBox();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.gpExistingProfiles.SuspendLayout();
            this.pnlProfileSettings.SuspendLayout();
            this.gpCreateNewProfile.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNewProfileName
            // 
            this.lblNewProfileName.AutoSize = true;
            this.lblNewProfileName.Location = new System.Drawing.Point(19, 32);
            this.lblNewProfileName.Name = "lblNewProfileName";
            this.lblNewProfileName.Size = new System.Drawing.Size(35, 13);
            this.lblNewProfileName.TabIndex = 2;
            this.lblNewProfileName.Text = "Name";
            // 
            // txtNewProfileName
            // 
            this.txtNewProfileName.Location = new System.Drawing.Point(60, 32);
            this.txtNewProfileName.Name = "txtNewProfileName";
            this.txtNewProfileName.Size = new System.Drawing.Size(372, 20);
            this.txtNewProfileName.TabIndex = 0;
            // 
            // btnCreateNewProfile
            // 
            this.btnCreateNewProfile.AutoSize = true;
            this.btnCreateNewProfile.Location = new System.Drawing.Point(438, 30);
            this.btnCreateNewProfile.Name = "btnCreateNewProfile";
            this.btnCreateNewProfile.Size = new System.Drawing.Size(75, 23);
            this.btnCreateNewProfile.TabIndex = 1;
            this.btnCreateNewProfile.Text = "Create";
            this.btnCreateNewProfile.UseVisualStyleBackColor = true;
            this.btnCreateNewProfile.Click += new System.EventHandler(this.btnCreateNewProfile_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(18, 281);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(495, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(18, 255);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(495, 23);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // gpExistingProfiles
            // 
            this.gpExistingProfiles.Controls.Add(this.pnlProfileSettings);
            this.gpExistingProfiles.Controls.Add(this.cmbExistingProfiles);
            this.gpExistingProfiles.Controls.Add(this.permRoutine);
            this.gpExistingProfiles.Controls.Add(this.btnDelete);
            this.gpExistingProfiles.Controls.Add(this.btnUpdate);
            this.gpExistingProfiles.Controls.Add(this.permProblem);
            this.gpExistingProfiles.Controls.Add(this.permReady);
            this.gpExistingProfiles.Controls.Add(this.permStat);
            this.gpExistingProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpExistingProfiles.Location = new System.Drawing.Point(0, 72);
            this.gpExistingProfiles.Name = "gpExistingProfiles";
            this.gpExistingProfiles.Size = new System.Drawing.Size(523, 317);
            this.gpExistingProfiles.TabIndex = 19;
            this.gpExistingProfiles.TabStop = false;
            this.gpExistingProfiles.Text = "Existing Profiles";
            // 
            // pnlProfileSettings
            // 
            this.pnlProfileSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProfileSettings.Controls.Add(this.chkIsAdmin);
            this.pnlProfileSettings.Location = new System.Drawing.Point(18, 42);
            this.pnlProfileSettings.Name = "pnlProfileSettings";
            this.pnlProfileSettings.Size = new System.Drawing.Size(495, 23);
            this.pnlProfileSettings.TabIndex = 21;
            // 
            // chkIsAdmin
            // 
            this.chkIsAdmin.AutoSize = true;
            this.chkIsAdmin.Location = new System.Drawing.Point(3, 3);
            this.chkIsAdmin.Name = "chkIsAdmin";
            this.chkIsAdmin.Size = new System.Drawing.Size(223, 17);
            this.chkIsAdmin.TabIndex = 3;
            this.chkIsAdmin.Text = "Is Admin (Can manage settings and users)";
            this.chkIsAdmin.UseVisualStyleBackColor = true;
            // 
            // cmbExistingProfiles
            // 
            this.cmbExistingProfiles.Location = new System.Drawing.Point(18, 16);
            this.cmbExistingProfiles.Margin = new System.Windows.Forms.Padding(0);
            this.cmbExistingProfiles.Name = "cmbExistingProfiles";
            this.cmbExistingProfiles.Size = new System.Drawing.Size(495, 21);
            this.cmbExistingProfiles.TabIndex = 2;
            this.cmbExistingProfiles.SelectedKeyChanged += new System.EventHandler(this.cmbExistingProfiles_SelectedKeyChanged);
            // 
            // permRoutine
            // 
            this.permRoutine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.permRoutine.Caption = "Routine";
            this.permRoutine.Location = new System.Drawing.Point(143, 74);
            this.permRoutine.Name = "permRoutine";
            this.permRoutine.Size = new System.Drawing.Size(120, 175);
            this.permRoutine.TabIndex = 5;
            // 
            // permProblem
            // 
            this.permProblem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.permProblem.Caption = "Problem";
            this.permProblem.Location = new System.Drawing.Point(18, 74);
            this.permProblem.Name = "permProblem";
            this.permProblem.Size = new System.Drawing.Size(120, 175);
            this.permProblem.TabIndex = 4;
            // 
            // permReady
            // 
            this.permReady.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.permReady.Caption = "Ready";
            this.permReady.Location = new System.Drawing.Point(393, 74);
            this.permReady.Name = "permReady";
            this.permReady.Size = new System.Drawing.Size(120, 175);
            this.permReady.TabIndex = 7;
            // 
            // permStat
            // 
            this.permStat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.permStat.Caption = "Stat";
            this.permStat.Location = new System.Drawing.Point(268, 74);
            this.permStat.Name = "permStat";
            this.permStat.Size = new System.Drawing.Size(120, 175);
            this.permStat.TabIndex = 6;
            // 
            // gpCreateNewProfile
            // 
            this.gpCreateNewProfile.Controls.Add(this.lblNewProfileName);
            this.gpCreateNewProfile.Controls.Add(this.txtNewProfileName);
            this.gpCreateNewProfile.Controls.Add(this.btnCreateNewProfile);
            this.gpCreateNewProfile.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpCreateNewProfile.Location = new System.Drawing.Point(0, 0);
            this.gpCreateNewProfile.Name = "gpCreateNewProfile";
            this.gpCreateNewProfile.Size = new System.Drawing.Size(523, 72);
            this.gpCreateNewProfile.TabIndex = 20;
            this.gpCreateNewProfile.TabStop = false;
            this.gpCreateNewProfile.Text = "Create New Profile";
            // 
            // pnlScroll
            // 
            this.pnlScroll.Controls.Add(this.gpExistingProfiles);
            this.pnlScroll.Controls.Add(this.gpCreateNewProfile);
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(523, 389);
            this.pnlScroll.TabIndex = 21;
            // 
            // UserProfileManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScroll);
            this.Name = "UserProfileManagerControl";
            this.Size = new System.Drawing.Size(523, 389);
            this.gpExistingProfiles.ResumeLayout(false);
            this.pnlProfileSettings.ResumeLayout(false);
            this.pnlProfileSettings.PerformLayout();
            this.gpCreateNewProfile.ResumeLayout(false);
            this.gpCreateNewProfile.PerformLayout();
            this.pnlScroll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNewProfileName;
        private System.Windows.Forms.TextBox txtNewProfileName;
        private System.Windows.Forms.Button btnCreateNewProfile;
        private MasterCategoryPermissionControl permProblem;
        private MasterCategoryPermissionControl permRoutine;
        private MasterCategoryPermissionControl permStat;
        private MasterCategoryPermissionControl permReady;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox gpExistingProfiles;
        private System.Windows.Forms.GroupBox gpCreateNewProfile;
        private System.Windows.Forms.Panel pnlScroll;
        private UserProfilesDropDown cmbExistingProfiles;
        private System.Windows.Forms.Panel pnlProfileSettings;
        private System.Windows.Forms.CheckBox chkIsAdmin;
    }
}
