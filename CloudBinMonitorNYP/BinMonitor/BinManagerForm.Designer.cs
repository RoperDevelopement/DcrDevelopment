/*
 * User: Sam Brinly
 * Date: 11/19/2014
 */
using BinMonitor.Common;
namespace BinMonitor
{
	partial class BinManagerForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinManagerForm));
            this.grpUserCredentials = new System.Windows.Forms.GroupBox();
            this.btnSyncCloud = new System.Windows.Forms.Button();
            this.userAuthenticationControl1 = new BinMonitor.Common.UserAuthenticationControl();
            this.tPageSettings = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtEmailFreq = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.cmbEmailTo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gpTesting = new System.Windows.Forms.GroupBox();
            this.btnClearAllBins = new System.Windows.Forms.Button();
            this.gpAbout = new System.Windows.Forms.GroupBox();
            this.lblThirdPartyLicensing = new System.Windows.Forms.Label();
            this.txtAboutVersion = new System.Windows.Forms.TextBox();
            this.lblAboutVersionCaption = new System.Windows.Forms.Label();
            this.lblAboutProductName = new System.Windows.Forms.Label();
            this.tpageManageUserProfiles = new System.Windows.Forms.TabPage();
            this.userProfileManagerControl1 = new BinMonitor.Common.UserProfileManagerControl();
            this.tpageManageUsers = new System.Windows.Forms.TabPage();
            this.btnSyncUsers = new System.Windows.Forms.Button();
            this.userManagerControl1 = new BinMonitor.Common.UserManagerControl();
            this.tpageManageCategories = new System.Windows.Forms.TabPage();
            this.ManageDefaultCategoryControl = new BinMonitor.Common.ManageDefaultCategoryControl();
            this.tpageManageBatch = new System.Windows.Forms.TabPage();
            this.BinLookupControl = new BinMonitor.Common.BinLookupControl();
            this.tpageNewBatch = new System.Windows.Forms.TabPage();
            this.CreateBatchControl = new BinMonitor.Common.CreateBatchControl();
            this.tpageArchive = new System.Windows.Forms.TabPage();
            this.ViewArchive = new BinMonitor.Common.ArchiveBatchLookupControl();
            this.tpageHome = new System.Windows.Forms.TabPage();
            this.grpManagerFunctions = new System.Windows.Forms.GroupBox();
            this.btnNewBatch = new System.Windows.Forms.Button();
            this.btnManageBatch = new System.Windows.Forms.Button();
            this.lblHomeMessage = new System.Windows.Forms.Label();
            this.grpUserFunctions = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCloseBatch = new System.Windows.Forms.Button();
            this.btnCompleteProcessing = new System.Windows.Forms.Button();
            this.btnCompleteRegistration = new System.Windows.Forms.Button();
            this.btnBeginRegistrationAndProcessing = new System.Windows.Forms.Button();
            this.btnBeginProcessing = new System.Windows.Forms.Button();
            this.btnBeginRegistration = new System.Windows.Forms.Button();
            this.btnFindSpecimen = new System.Windows.Forms.Button();
            this.btnUserManageBin = new System.Windows.Forms.Button();
            this.TabPanel = new System.Windows.Forms.TabControl();
            this.tabPageChanges = new System.Windows.Forms.TabPage();
            this.butSubChanges = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.richTxtBoxChanges = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBoxSubject = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grpUserCredentials.SuspendLayout();
            this.tPageSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gpTesting.SuspendLayout();
            this.gpAbout.SuspendLayout();
            this.tpageManageUserProfiles.SuspendLayout();
            this.tpageManageUsers.SuspendLayout();
            this.tpageManageCategories.SuspendLayout();
            this.tpageManageBatch.SuspendLayout();
            this.tpageNewBatch.SuspendLayout();
            this.tpageArchive.SuspendLayout();
            this.tpageHome.SuspendLayout();
            this.grpManagerFunctions.SuspendLayout();
            this.grpUserFunctions.SuspendLayout();
            this.TabPanel.SuspendLayout();
            this.tabPageChanges.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpUserCredentials
            // 
            this.grpUserCredentials.Controls.Add(this.btnSyncCloud);
            this.grpUserCredentials.Controls.Add(this.userAuthenticationControl1);
            this.grpUserCredentials.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpUserCredentials.Location = new System.Drawing.Point(0, 0);
            this.grpUserCredentials.Name = "grpUserCredentials";
            this.grpUserCredentials.Size = new System.Drawing.Size(1029, 50);
            this.grpUserCredentials.TabIndex = 0;
            this.grpUserCredentials.TabStop = false;
            this.grpUserCredentials.Text = "User Credentials";
            // 
            // btnSyncCloud
            // 
            this.btnSyncCloud.Location = new System.Drawing.Point(416, 16);
            this.btnSyncCloud.Name = "btnSyncCloud";
            this.btnSyncCloud.Size = new System.Drawing.Size(112, 29);
            this.btnSyncCloud.TabIndex = 1;
            this.btnSyncCloud.Text = "&Refresh Batches";
            this.btnSyncCloud.UseVisualStyleBackColor = true;
            this.btnSyncCloud.Click += new System.EventHandler(this.btnSyncCloud_Click);
            // 
            // userAuthenticationControl1
            // 
            this.userAuthenticationControl1.AdminOverrideVisible = true;
            this.userAuthenticationControl1.AutoSize = true;
            this.userAuthenticationControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.userAuthenticationControl1.Location = new System.Drawing.Point(9, 16);
            this.userAuthenticationControl1.Margin = new System.Windows.Forms.Padding(0);
            this.userAuthenticationControl1.Name = "userAuthenticationControl1";
            this.userAuthenticationControl1.Size = new System.Drawing.Size(401, 29);
            this.userAuthenticationControl1.TabIndex = 0;
            this.userAuthenticationControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.userAuthenticationControl1_Paint);
            // 
            // tPageSettings
            // 
            this.tPageSettings.AutoScroll = true;
            this.tPageSettings.Controls.Add(this.groupBox1);
            this.tPageSettings.Controls.Add(this.gpTesting);
            this.tPageSettings.Controls.Add(this.gpAbout);
            this.tPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tPageSettings.Name = "tPageSettings";
            this.tPageSettings.Size = new System.Drawing.Size(1021, 726);
            this.tPageSettings.TabIndex = 7;
            this.tPageSettings.Text = "Auto Generated Reports";
            this.tPageSettings.UseVisualStyleBackColor = true;
            this.tPageSettings.Click += new System.EventHandler(this.tPageSettings_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtEmailFreq);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtEmailAddress);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.cmbEmailTo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(24, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 168);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send Email Reports";
            // 
            // txtEmailFreq
            // 
            this.txtEmailFreq.Location = new System.Drawing.Point(104, 104);
            this.txtEmailFreq.Name = "txtEmailFreq";
            this.txtEmailFreq.Size = new System.Drawing.Size(100, 20);
            this.txtEmailFreq.TabIndex = 6;
            this.txtEmailFreq.Text = "24";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Frequency Hours:";
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(72, 72);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(240, 20);
            this.txtEmailAddress.TabIndex = 4;
            this.txtEmailAddress.Enter += new System.EventHandler(this.txtEmailAddress_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Email CC:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(112, 128);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "UpDate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // cmbEmailTo
            // 
            this.cmbEmailTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmailTo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbEmailTo.FormattingEnabled = true;
            this.cmbEmailTo.Items.AddRange(new object[] {
            "ddd",
            "dddd"});
            this.cmbEmailTo.Location = new System.Drawing.Point(72, 32);
            this.cmbEmailTo.Name = "cmbEmailTo";
            this.cmbEmailTo.Size = new System.Drawing.Size(208, 21);
            this.cmbEmailTo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email To:";
            // 
            // gpTesting
            // 
            this.gpTesting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gpTesting.Controls.Add(this.btnClearAllBins);
            this.gpTesting.Location = new System.Drawing.Point(-788, 24);
            this.gpTesting.Name = "gpTesting";
            this.gpTesting.Size = new System.Drawing.Size(335, 96);
            this.gpTesting.TabIndex = 2;
            this.gpTesting.TabStop = false;
            this.gpTesting.Text = "Testing Utilities";
            // 
            // btnClearAllBins
            // 
            this.btnClearAllBins.Location = new System.Drawing.Point(6, 19);
            this.btnClearAllBins.Name = "btnClearAllBins";
            this.btnClearAllBins.Size = new System.Drawing.Size(75, 75);
            this.btnClearAllBins.TabIndex = 0;
            this.btnClearAllBins.Text = "Clear All Bins";
            this.btnClearAllBins.UseVisualStyleBackColor = true;
            this.btnClearAllBins.Click += new System.EventHandler(this.btnClearAllBins_Click);
            // 
            // gpAbout
            // 
            this.gpAbout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.gpAbout.Controls.Add(this.lblThirdPartyLicensing);
            this.gpAbout.Controls.Add(this.txtAboutVersion);
            this.gpAbout.Controls.Add(this.lblAboutVersionCaption);
            this.gpAbout.Controls.Add(this.lblAboutProductName);
            this.gpAbout.Location = new System.Drawing.Point(24, 242);
            this.gpAbout.Name = "gpAbout";
            this.gpAbout.Size = new System.Drawing.Size(488, 208);
            this.gpAbout.TabIndex = 1;
            this.gpAbout.TabStop = false;
            this.gpAbout.Text = "About";
            // 
            // lblThirdPartyLicensing
            // 
            this.lblThirdPartyLicensing.Location = new System.Drawing.Point(6, 81);
            this.lblThirdPartyLicensing.Name = "lblThirdPartyLicensing";
            this.lblThirdPartyLicensing.Size = new System.Drawing.Size(288, 45);
            this.lblThirdPartyLicensing.TabIndex = 3;
            this.lblThirdPartyLicensing.Text = "Includes the following 3rd Party components:\r\n(Licensing can be found in install " +
    "folder)\r\n- SharpSerializer";
            // 
            // txtAboutVersion
            // 
            this.txtAboutVersion.Location = new System.Drawing.Point(54, 51);
            this.txtAboutVersion.Name = "txtAboutVersion";
            this.txtAboutVersion.ReadOnly = true;
            this.txtAboutVersion.Size = new System.Drawing.Size(100, 20);
            this.txtAboutVersion.TabIndex = 2;
            this.txtAboutVersion.Text = "V 1.1";
            // 
            // lblAboutVersionCaption
            // 
            this.lblAboutVersionCaption.AutoSize = true;
            this.lblAboutVersionCaption.Location = new System.Drawing.Point(6, 54);
            this.lblAboutVersionCaption.Name = "lblAboutVersionCaption";
            this.lblAboutVersionCaption.Size = new System.Drawing.Size(42, 13);
            this.lblAboutVersionCaption.TabIndex = 1;
            this.lblAboutVersionCaption.Text = "Version";
            // 
            // lblAboutProductName
            // 
            this.lblAboutProductName.AutoSize = true;
            this.lblAboutProductName.Location = new System.Drawing.Point(6, 16);
            this.lblAboutProductName.Name = "lblAboutProductName";
            this.lblAboutProductName.Size = new System.Drawing.Size(109, 26);
            this.lblAboutProductName.TabIndex = 0;
            this.lblAboutProductName.Text = "NYP L8 Bin Monitor\r\n(C) e-Docs USA 2018";
            // 
            // tpageManageUserProfiles
            // 
            this.tpageManageUserProfiles.AutoScroll = true;
            this.tpageManageUserProfiles.Controls.Add(this.userProfileManagerControl1);
            this.tpageManageUserProfiles.Location = new System.Drawing.Point(4, 22);
            this.tpageManageUserProfiles.Name = "tpageManageUserProfiles";
            this.tpageManageUserProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpageManageUserProfiles.Size = new System.Drawing.Size(1021, 726);
            this.tpageManageUserProfiles.TabIndex = 7;
            this.tpageManageUserProfiles.Text = "Manage Profiles";
            this.tpageManageUserProfiles.UseVisualStyleBackColor = true;
            // 
            // userProfileManagerControl1
            // 
            this.userProfileManagerControl1.CredentialHost = null;
            this.userProfileManagerControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.userProfileManagerControl1.Location = new System.Drawing.Point(3, 3);
            this.userProfileManagerControl1.Margin = new System.Windows.Forms.Padding(4);
            this.userProfileManagerControl1.Name = "userProfileManagerControl1";
            this.userProfileManagerControl1.Size = new System.Drawing.Size(521, 720);
            this.userProfileManagerControl1.TabIndex = 0;
            // 
            // tpageManageUsers
            // 
            this.tpageManageUsers.Controls.Add(this.btnSyncUsers);
            this.tpageManageUsers.Controls.Add(this.userManagerControl1);
            this.tpageManageUsers.Location = new System.Drawing.Point(4, 22);
            this.tpageManageUsers.Name = "tpageManageUsers";
            this.tpageManageUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpageManageUsers.Size = new System.Drawing.Size(1021, 726);
            this.tpageManageUsers.TabIndex = 3;
            this.tpageManageUsers.Text = "Manage Users";
            this.tpageManageUsers.UseVisualStyleBackColor = true;
            // 
            // btnSyncUsers
            // 
            this.btnSyncUsers.Location = new System.Drawing.Point(16, 360);
            this.btnSyncUsers.Name = "btnSyncUsers";
            this.btnSyncUsers.Size = new System.Drawing.Size(264, 24);
            this.btnSyncUsers.TabIndex = 1;
            this.btnSyncUsers.Text = "Sync Users";
            this.btnSyncUsers.UseVisualStyleBackColor = true;
            this.btnSyncUsers.Click += new System.EventHandler(this.btnSyncUsers_Click);
            // 
            // userManagerControl1
            // 
            this.userManagerControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userManagerControl1.CredentialHost = null;
            this.userManagerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userManagerControl1.Location = new System.Drawing.Point(3, 3);
            this.userManagerControl1.Margin = new System.Windows.Forms.Padding(4);
            this.userManagerControl1.Name = "userManagerControl1";
            this.userManagerControl1.Size = new System.Drawing.Size(1015, 720);
            this.userManagerControl1.TabIndex = 0;
            // 
            // tpageManageCategories
            // 
            this.tpageManageCategories.AutoScroll = true;
            this.tpageManageCategories.Controls.Add(this.ManageDefaultCategoryControl);
            this.tpageManageCategories.Location = new System.Drawing.Point(4, 22);
            this.tpageManageCategories.Name = "tpageManageCategories";
            this.tpageManageCategories.Padding = new System.Windows.Forms.Padding(3);
            this.tpageManageCategories.Size = new System.Drawing.Size(1021, 726);
            this.tpageManageCategories.TabIndex = 5;
            this.tpageManageCategories.Text = "Manage Categories";
            this.tpageManageCategories.UseVisualStyleBackColor = true;
            // 
            // ManageDefaultCategoryControl
            // 
            this.ManageDefaultCategoryControl.CredentialHost = null;
            this.ManageDefaultCategoryControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.ManageDefaultCategoryControl.Location = new System.Drawing.Point(3, 3);
            this.ManageDefaultCategoryControl.Margin = new System.Windows.Forms.Padding(4);
            this.ManageDefaultCategoryControl.Name = "ManageDefaultCategoryControl";
            this.ManageDefaultCategoryControl.Size = new System.Drawing.Size(1015, 550);
            this.ManageDefaultCategoryControl.TabIndex = 0;
            // 
            // tpageManageBatch
            // 
            this.tpageManageBatch.AutoScroll = true;
            this.tpageManageBatch.Controls.Add(this.BinLookupControl);
            this.tpageManageBatch.Location = new System.Drawing.Point(4, 22);
            this.tpageManageBatch.Name = "tpageManageBatch";
            this.tpageManageBatch.Size = new System.Drawing.Size(1021, 726);
            this.tpageManageBatch.TabIndex = 2;
            this.tpageManageBatch.Text = "Manage Batch";
            this.tpageManageBatch.UseVisualStyleBackColor = true;
            // 
            // BinLookupControl
            // 
            this.BinLookupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BinLookupControl.Location = new System.Drawing.Point(0, 0);
            this.BinLookupControl.Margin = new System.Windows.Forms.Padding(4);
            this.BinLookupControl.Name = "BinLookupControl";
            this.BinLookupControl.Size = new System.Drawing.Size(1021, 726);
            this.BinLookupControl.TabIndex = 0;
            // 
            // tpageNewBatch
            // 
            this.tpageNewBatch.AutoScroll = true;
            this.tpageNewBatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpageNewBatch.Controls.Add(this.CreateBatchControl);
            this.tpageNewBatch.Location = new System.Drawing.Point(4, 22);
            this.tpageNewBatch.Name = "tpageNewBatch";
            this.tpageNewBatch.Padding = new System.Windows.Forms.Padding(3);
            this.tpageNewBatch.Size = new System.Drawing.Size(1021, 726);
            this.tpageNewBatch.TabIndex = 1;
            this.tpageNewBatch.Text = "New Batch";
            this.tpageNewBatch.ToolTipText = "Add  New Batch";
            this.tpageNewBatch.UseVisualStyleBackColor = true;
            // 
            // CreateBatchControl
            // 
            this.CreateBatchControl.AdvancedOptionsVisible = true;
            this.CreateBatchControl.AutoSize = true;
            this.CreateBatchControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CreateBatchControl.CredentialHost = null;
            this.CreateBatchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreateBatchControl.Location = new System.Drawing.Point(3, 3);
            this.CreateBatchControl.Margin = new System.Windows.Forms.Padding(4);
            this.CreateBatchControl.Name = "CreateBatchControl";
            this.CreateBatchControl.Size = new System.Drawing.Size(1013, 718);
            this.CreateBatchControl.TabIndex = 0;
            // 
            // tpageArchive
            // 
            this.tpageArchive.AutoScroll = true;
            this.tpageArchive.Controls.Add(this.ViewArchive);
            this.tpageArchive.Location = new System.Drawing.Point(4, 22);
            this.tpageArchive.Margin = new System.Windows.Forms.Padding(2);
            this.tpageArchive.Name = "tpageArchive";
            this.tpageArchive.Padding = new System.Windows.Forms.Padding(2);
            this.tpageArchive.Size = new System.Drawing.Size(1021, 726);
            this.tpageArchive.TabIndex = 6;
            this.tpageArchive.Text = "View Archive";
            this.tpageArchive.UseVisualStyleBackColor = true;
            // 
            // ViewArchive
            // 
            this.ViewArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewArchive.Location = new System.Drawing.Point(2, 2);
            this.ViewArchive.Margin = new System.Windows.Forms.Padding(4);
            this.ViewArchive.Name = "ViewArchive";
            this.ViewArchive.Size = new System.Drawing.Size(1017, 722);
            this.ViewArchive.TabIndex = 0;
            // 
            // tpageHome
            // 
            this.tpageHome.AutoScroll = true;
            this.tpageHome.Controls.Add(this.grpManagerFunctions);
            this.tpageHome.Controls.Add(this.lblHomeMessage);
            this.tpageHome.Controls.Add(this.grpUserFunctions);
            this.tpageHome.Location = new System.Drawing.Point(4, 22);
            this.tpageHome.Name = "tpageHome";
            this.tpageHome.Padding = new System.Windows.Forms.Padding(3);
            this.tpageHome.Size = new System.Drawing.Size(1021, 726);
            this.tpageHome.TabIndex = 0;
            this.tpageHome.Text = "Home";
            this.tpageHome.UseVisualStyleBackColor = true;
            // 
            // grpManagerFunctions
            // 
            this.grpManagerFunctions.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpManagerFunctions.Controls.Add(this.btnNewBatch);
            this.grpManagerFunctions.Controls.Add(this.btnManageBatch);
            this.grpManagerFunctions.Location = new System.Drawing.Point(504, 159);
            this.grpManagerFunctions.Name = "grpManagerFunctions";
            this.grpManagerFunctions.Size = new System.Drawing.Size(188, 105);
            this.grpManagerFunctions.TabIndex = 4;
            this.grpManagerFunctions.TabStop = false;
            this.grpManagerFunctions.Text = "Manager Functions";
            // 
            // btnNewBatch
            // 
            this.btnNewBatch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNewBatch.Location = new System.Drawing.Point(16, 22);
            this.btnNewBatch.Name = "btnNewBatch";
            this.btnNewBatch.Size = new System.Drawing.Size(75, 75);
            this.btnNewBatch.TabIndex = 0;
            this.btnNewBatch.Text = "New Batch";
            this.btnNewBatch.UseVisualStyleBackColor = true;
            this.btnNewBatch.Click += new System.EventHandler(this.btnNewBatch_Click);
            // 
            // btnManageBatch
            // 
            this.btnManageBatch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnManageBatch.Location = new System.Drawing.Point(97, 22);
            this.btnManageBatch.Name = "btnManageBatch";
            this.btnManageBatch.Size = new System.Drawing.Size(75, 75);
            this.btnManageBatch.TabIndex = 3;
            this.btnManageBatch.Text = "Manage Batch";
            this.btnManageBatch.UseVisualStyleBackColor = true;
            this.btnManageBatch.Click += new System.EventHandler(this.btnManageBatch_Click);
            // 
            // lblHomeMessage
            // 
            this.lblHomeMessage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHomeMessage.AutoSize = true;
            this.lblHomeMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHomeMessage.Location = new System.Drawing.Point(194, 54);
            this.lblHomeMessage.Name = "lblHomeMessage";
            this.lblHomeMessage.Size = new System.Drawing.Size(540, 48);
            this.lblHomeMessage.TabIndex = 3;
            this.lblHomeMessage.Text = "Please fill in your credentials and select a tab to continue\r\nYou are responsible" +
    " for logging out";
            // 
            // grpUserFunctions
            // 
            this.grpUserFunctions.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpUserFunctions.Controls.Add(this.button1);
            this.grpUserFunctions.Controls.Add(this.btnCloseBatch);
            this.grpUserFunctions.Controls.Add(this.btnCompleteProcessing);
            this.grpUserFunctions.Controls.Add(this.btnCompleteRegistration);
            this.grpUserFunctions.Controls.Add(this.btnBeginRegistrationAndProcessing);
            this.grpUserFunctions.Controls.Add(this.btnBeginProcessing);
            this.grpUserFunctions.Controls.Add(this.btnBeginRegistration);
            this.grpUserFunctions.Controls.Add(this.btnFindSpecimen);
            this.grpUserFunctions.Controls.Add(this.btnUserManageBin);
            this.grpUserFunctions.Location = new System.Drawing.Point(237, 159);
            this.grpUserFunctions.Name = "grpUserFunctions";
            this.grpUserFunctions.Size = new System.Drawing.Size(261, 285);
            this.grpUserFunctions.TabIndex = 2;
            this.grpUserFunctions.TabStop = false;
            this.grpUserFunctions.Text = "User Functions";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 75);
            this.button1.TabIndex = 13;
            this.button1.Text = "View\rArchives";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnArchiveBatch_Click);
            // 
            // btnCloseBatch
            // 
            this.btnCloseBatch.Location = new System.Drawing.Point(168, 184);
            this.btnCloseBatch.Name = "btnCloseBatch";
            this.btnCloseBatch.Size = new System.Drawing.Size(75, 75);
            this.btnCloseBatch.TabIndex = 12;
            this.btnCloseBatch.Text = "Close \r\nBatch\r\n[F11]";
            this.btnCloseBatch.UseVisualStyleBackColor = true;
            this.btnCloseBatch.Click += new System.EventHandler(this.btnCloseBatch_Click);
            // 
            // btnCompleteProcessing
            // 
            this.btnCompleteProcessing.Location = new System.Drawing.Point(87, 184);
            this.btnCompleteProcessing.Name = "btnCompleteProcessing";
            this.btnCompleteProcessing.Size = new System.Drawing.Size(75, 75);
            this.btnCompleteProcessing.TabIndex = 11;
            this.btnCompleteProcessing.Text = "Complete \r\nProcessing\r\n[F10]";
            this.btnCompleteProcessing.UseVisualStyleBackColor = true;
            this.btnCompleteProcessing.Click += new System.EventHandler(this.btnCompleteProcessing_Click);
            // 
            // btnCompleteRegistration
            // 
            this.btnCompleteRegistration.Location = new System.Drawing.Point(6, 184);
            this.btnCompleteRegistration.Name = "btnCompleteRegistration";
            this.btnCompleteRegistration.Size = new System.Drawing.Size(75, 75);
            this.btnCompleteRegistration.TabIndex = 10;
            this.btnCompleteRegistration.Text = "Complete \r\nRegistration\r\n[F9]";
            this.btnCompleteRegistration.UseVisualStyleBackColor = true;
            this.btnCompleteRegistration.Click += new System.EventHandler(this.btnCompleteRegistration_Click);
            // 
            // btnBeginRegistrationAndProcessing
            // 
            this.btnBeginRegistrationAndProcessing.Location = new System.Drawing.Point(168, 103);
            this.btnBeginRegistrationAndProcessing.Name = "btnBeginRegistrationAndProcessing";
            this.btnBeginRegistrationAndProcessing.Size = new System.Drawing.Size(75, 75);
            this.btnBeginRegistrationAndProcessing.TabIndex = 9;
            this.btnBeginRegistrationAndProcessing.Text = "Begin\r\nBoth\r\n[F8]";
            this.btnBeginRegistrationAndProcessing.UseVisualStyleBackColor = true;
            this.btnBeginRegistrationAndProcessing.Click += new System.EventHandler(this.btnBeginRegistrationAndProcessing_Click);
            // 
            // btnBeginProcessing
            // 
            this.btnBeginProcessing.Location = new System.Drawing.Point(87, 103);
            this.btnBeginProcessing.Name = "btnBeginProcessing";
            this.btnBeginProcessing.Size = new System.Drawing.Size(75, 75);
            this.btnBeginProcessing.TabIndex = 8;
            this.btnBeginProcessing.Text = "Begin\r\nProcessing\r\n[F7]";
            this.btnBeginProcessing.UseVisualStyleBackColor = true;
            this.btnBeginProcessing.Click += new System.EventHandler(this.btnBeginProcessing_Click);
            // 
            // btnBeginRegistration
            // 
            this.btnBeginRegistration.Location = new System.Drawing.Point(6, 103);
            this.btnBeginRegistration.Name = "btnBeginRegistration";
            this.btnBeginRegistration.Size = new System.Drawing.Size(75, 75);
            this.btnBeginRegistration.TabIndex = 7;
            this.btnBeginRegistration.Text = "Begin\r\nRegistration\r\n[F6]";
            this.btnBeginRegistration.UseVisualStyleBackColor = true;
            this.btnBeginRegistration.Click += new System.EventHandler(this.btnBeginRegistration_Click);
            // 
            // btnFindSpecimen
            // 
            this.btnFindSpecimen.Location = new System.Drawing.Point(6, 22);
            this.btnFindSpecimen.Name = "btnFindSpecimen";
            this.btnFindSpecimen.Size = new System.Drawing.Size(75, 75);
            this.btnFindSpecimen.TabIndex = 6;
            this.btnFindSpecimen.Text = "Find \r\nSpecimen\r\n[F4]";
            this.btnFindSpecimen.UseVisualStyleBackColor = true;
            this.btnFindSpecimen.Click += new System.EventHandler(this.btnFindSpecimen_Click);
            // 
            // btnUserManageBin
            // 
            this.btnUserManageBin.Location = new System.Drawing.Point(87, 22);
            this.btnUserManageBin.Name = "btnUserManageBin";
            this.btnUserManageBin.Size = new System.Drawing.Size(75, 75);
            this.btnUserManageBin.TabIndex = 4;
            this.btnUserManageBin.Text = "Manage\r\nBatch\r\n[F5]";
            this.btnUserManageBin.UseVisualStyleBackColor = true;
            this.btnUserManageBin.Click += new System.EventHandler(this.btnUserManageBin_Click);
            // 
            // TabPanel
            // 
            this.TabPanel.Controls.Add(this.tpageHome);
            this.TabPanel.Controls.Add(this.tpageNewBatch);
            this.TabPanel.Controls.Add(this.tpageManageBatch);
            this.TabPanel.Controls.Add(this.tpageManageCategories);
            this.TabPanel.Controls.Add(this.tpageManageUsers);
            this.TabPanel.Controls.Add(this.tpageManageUserProfiles);
            this.TabPanel.Controls.Add(this.tPageSettings);
            this.TabPanel.Controls.Add(this.tpageArchive);
            this.TabPanel.Controls.Add(this.tabPageChanges);
            this.TabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPanel.Location = new System.Drawing.Point(0, 50);
            this.TabPanel.Name = "TabPanel";
            this.TabPanel.SelectedIndex = 0;
            this.TabPanel.Size = new System.Drawing.Size(1029, 752);
            this.TabPanel.TabIndex = 0;
            this.TabPanel.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabPanel_Selecting);
            // 
            // tabPageChanges
            // 
            this.tabPageChanges.Controls.Add(this.butSubChanges);
            this.tabPageChanges.Controls.Add(this.label7);
            this.tabPageChanges.Controls.Add(this.richTxtBoxChanges);
            this.tabPageChanges.Controls.Add(this.label6);
            this.tabPageChanges.Controls.Add(this.txtBoxSubject);
            this.tabPageChanges.Controls.Add(this.label5);
            this.tabPageChanges.Controls.Add(this.txtBoxEmail);
            this.tabPageChanges.Controls.Add(this.label4);
            this.tabPageChanges.Location = new System.Drawing.Point(4, 22);
            this.tabPageChanges.Name = "tabPageChanges";
            this.tabPageChanges.Size = new System.Drawing.Size(1021, 726);
            this.tabPageChanges.TabIndex = 8;
            this.tabPageChanges.Text = "BinMonitor Changes";
            this.tabPageChanges.UseVisualStyleBackColor = true;
            // 
            // butSubChanges
            // 
            this.butSubChanges.AllowDrop = true;
            this.butSubChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butSubChanges.Location = new System.Drawing.Point(330, 374);
            this.butSubChanges.Name = "butSubChanges";
            this.butSubChanges.Size = new System.Drawing.Size(144, 23);
            this.butSubChanges.TabIndex = 7;
            this.butSubChanges.Text = "Submit Changes";
            this.butSubChanges.UseVisualStyleBackColor = true;
            this.butSubChanges.Click += new System.EventHandler(this.butSubChanges_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(320, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "BinMonitor Changes";
            // 
            // richTxtBoxChanges
            // 
            this.richTxtBoxChanges.Location = new System.Drawing.Point(161, 116);
            this.richTxtBoxChanges.Name = "richTxtBoxChanges";
            this.richTxtBoxChanges.Size = new System.Drawing.Size(597, 236);
            this.richTxtBoxChanges.TabIndex = 5;
            this.richTxtBoxChanges.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Suggested Changes:";
            // 
            // txtBoxSubject
            // 
            this.txtBoxSubject.Location = new System.Drawing.Point(102, 77);
            this.txtBoxSubject.Name = "txtBoxSubject";
            this.txtBoxSubject.Size = new System.Drawing.Size(568, 20);
            this.txtBoxSubject.TabIndex = 3;
            this.txtBoxSubject.Text = "Changes BinMonitor";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Subject:";
            // 
            // txtBoxEmail
            // 
            this.txtBoxEmail.Location = new System.Drawing.Point(102, 35);
            this.txtBoxEmail.Name = "txtBoxEmail";
            this.txtBoxEmail.Size = new System.Drawing.Size(193, 20);
            this.txtBoxEmail.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Email Address:";
            // 
            // BinManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1029, 802);
            this.Controls.Add(this.TabPanel);
            this.Controls.Add(this.grpUserCredentials);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BinManagerForm";
            this.Text = "Bin Manager - e-Docs USA, Inc.";
            this.Shown += new System.EventHandler(this.BinManagerForm_Shown);
            this.grpUserCredentials.ResumeLayout(false);
            this.grpUserCredentials.PerformLayout();
            this.tPageSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpTesting.ResumeLayout(false);
            this.gpAbout.ResumeLayout(false);
            this.gpAbout.PerformLayout();
            this.tpageManageUserProfiles.ResumeLayout(false);
            this.tpageManageUsers.ResumeLayout(false);
            this.tpageManageCategories.ResumeLayout(false);
            this.tpageManageBatch.ResumeLayout(false);
            this.tpageNewBatch.ResumeLayout(false);
            this.tpageNewBatch.PerformLayout();
            this.tpageArchive.ResumeLayout(false);
            this.tpageHome.ResumeLayout(false);
            this.tpageHome.PerformLayout();
            this.grpManagerFunctions.ResumeLayout(false);
            this.grpUserFunctions.ResumeLayout(false);
            this.TabPanel.ResumeLayout(false);
            this.tabPageChanges.ResumeLayout(false);
            this.tabPageChanges.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.GroupBox grpUserCredentials;
        private System.Windows.Forms.TabPage tPageSettings;
        private System.Windows.Forms.GroupBox gpTesting;
        private System.Windows.Forms.Button btnClearAllBins;
        private System.Windows.Forms.GroupBox gpAbout;
        private System.Windows.Forms.Label lblThirdPartyLicensing;
        private System.Windows.Forms.TextBox txtAboutVersion;
        private System.Windows.Forms.Label lblAboutVersionCaption;
        private System.Windows.Forms.Label lblAboutProductName;
        private System.Windows.Forms.TabPage tpageManageUserProfiles;
        private Common.UserProfileManagerControl userProfileManagerControl1;
        private System.Windows.Forms.TabPage tpageManageUsers;
        private Common.UserManagerControl userManagerControl1;
        private System.Windows.Forms.TabPage tpageManageCategories;
        private Common.ManageDefaultCategoryControl ManageDefaultCategoryControl;
        private System.Windows.Forms.TabPage tpageManageBatch;
        private Common.BinLookupControl BinLookupControl;
        private System.Windows.Forms.TabPage tpageNewBatch;
        private Common.CreateBatchControl CreateBatchControl;
        private Common.ArchiveBatchLookupControl ViewArchive;
        private System.Windows.Forms.TabPage tpageHome;
        private System.Windows.Forms.GroupBox grpManagerFunctions;
        private System.Windows.Forms.Button btnNewBatch;
        private System.Windows.Forms.Button btnManageBatch;
        private System.Windows.Forms.Label lblHomeMessage;
        private System.Windows.Forms.GroupBox grpUserFunctions;
        private System.Windows.Forms.Button btnFindSpecimen;
        private System.Windows.Forms.TabControl TabPanel;
        private System.Windows.Forms.Button btnUserManageBin;
        private UserAuthenticationControl userAuthenticationControl1;
        private System.Windows.Forms.Button btnBeginRegistration;
        private System.Windows.Forms.Button btnBeginProcessing;
        private System.Windows.Forms.Button btnBeginRegistrationAndProcessing;
        private System.Windows.Forms.Button btnCloseBatch;
        private System.Windows.Forms.Button btnCompleteProcessing;
        private System.Windows.Forms.Button btnCompleteRegistration;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tpageArchive;
        private System.Windows.Forms.Button btnSyncCloud;
        private System.Windows.Forms.Button btnSyncUsers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cmbEmailTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEmailFreq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageChanges;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTxtBoxChanges;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBoxSubject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button butSubChanges;
    }
}
