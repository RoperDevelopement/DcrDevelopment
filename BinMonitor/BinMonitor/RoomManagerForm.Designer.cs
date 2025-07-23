/*
 * User: Sam Brinly
 * Date: 11/19/2014
 */
using BinMonitor.Common;
namespace BinMonitor
{
    partial class RoomManagerForm
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
            if (disposing)
            {
                if (components != null)
                {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomManagerForm));
            this.grpUserCredentials = new System.Windows.Forms.GroupBox();
            this.userAuthenticationControl1 = new BinMonitor.Common.UserAuthenticationControl();
            this.tPageSettings = new System.Windows.Forms.TabPage();
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
            this.userManagerControl1 = new BinMonitor.Common.UserManagerControl();
            this.tpageManageCategories = new System.Windows.Forms.TabPage();
            this.ManageDefaultCategoryControl = new BinMonitor.Common.ManageDefaultCategoryControl();
            this.tpageManageBatch = new System.Windows.Forms.TabPage();
            this.BinLookupControl = new BinMonitor.Common.BinLookupControl();
            this.tpageHome = new System.Windows.Forms.TabPage();
            this.grpManagerFunctions = new System.Windows.Forms.GroupBox();
            this.btnManageBatch = new System.Windows.Forms.Button();
            this.lblHomeMessage = new System.Windows.Forms.Label();
            this.grpUserFunctions = new System.Windows.Forms.GroupBox();
            this.btnCloseBatch = new System.Windows.Forms.Button();
            this.btnCompleteProcessing = new System.Windows.Forms.Button();
            this.btnCompleteRegistration = new System.Windows.Forms.Button();
            this.btnBeginRegistrationAndProcessing = new System.Windows.Forms.Button();
            this.btnBeginProcessing = new System.Windows.Forms.Button();
            this.btnBeginRegistration = new System.Windows.Forms.Button();
            this.btnFindSpecimen = new System.Windows.Forms.Button();
            this.btnUserManageBin = new System.Windows.Forms.Button();
            this.TabPanel = new System.Windows.Forms.TabControl();
            this.grpUserCredentials.SuspendLayout();
            this.tPageSettings.SuspendLayout();
            this.gpTesting.SuspendLayout();
            this.gpAbout.SuspendLayout();
            this.tpageManageUserProfiles.SuspendLayout();
            this.tpageManageUsers.SuspendLayout();
            this.tpageManageCategories.SuspendLayout();
            this.tpageManageBatch.SuspendLayout();
            this.tpageHome.SuspendLayout();
            this.grpManagerFunctions.SuspendLayout();
            this.grpUserFunctions.SuspendLayout();
            this.TabPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpUserCredentials
            // 
            this.grpUserCredentials.Controls.Add(this.userAuthenticationControl1);
            this.grpUserCredentials.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpUserCredentials.Location = new System.Drawing.Point(0, 0);
            this.grpUserCredentials.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpUserCredentials.Name = "grpUserCredentials";
            this.grpUserCredentials.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpUserCredentials.Size = new System.Drawing.Size(1045, 62);
            this.grpUserCredentials.TabIndex = 0;
            this.grpUserCredentials.TabStop = false;
            this.grpUserCredentials.Text = "User Credentials";
            // 
            // userAuthenticationControl1
            // 
            this.userAuthenticationControl1.AdminOverrideVisible = true;
            this.userAuthenticationControl1.AutoSize = true;
            this.userAuthenticationControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.userAuthenticationControl1.Location = new System.Drawing.Point(12, 20);
            this.userAuthenticationControl1.Margin = new System.Windows.Forms.Padding(0);
            this.userAuthenticationControl1.Name = "userAuthenticationControl1";
            this.userAuthenticationControl1.Size = new System.Drawing.Size(605, 35);
            this.userAuthenticationControl1.TabIndex = 0;
            this.userAuthenticationControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.userAuthenticationControl1_Paint);
            // 
            // tPageSettings
            // 
            this.tPageSettings.AutoScroll = true;
            this.tPageSettings.Controls.Add(this.gpTesting);
            this.tPageSettings.Controls.Add(this.gpAbout);
            this.tPageSettings.Location = new System.Drawing.Point(4, 25);
            this.tPageSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tPageSettings.Name = "tPageSettings";
            this.tPageSettings.Size = new System.Drawing.Size(1037, 601);
            this.tPageSettings.TabIndex = 6;
            this.tPageSettings.Text = "Settings";
            this.tPageSettings.UseVisualStyleBackColor = true;
            // 
            // gpTesting
            // 
            this.gpTesting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gpTesting.Controls.Add(this.btnClearAllBins);
            this.gpTesting.Location = new System.Drawing.Point(100, 170);
            this.gpTesting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gpTesting.Name = "gpTesting";
            this.gpTesting.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gpTesting.Size = new System.Drawing.Size(400, 123);
            this.gpTesting.TabIndex = 2;
            this.gpTesting.TabStop = false;
            this.gpTesting.Text = "Testing Utilities";
            // 
            // btnClearAllBins
            // 
            this.btnClearAllBins.Location = new System.Drawing.Point(8, 23);
            this.btnClearAllBins.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearAllBins.Name = "btnClearAllBins";
            this.btnClearAllBins.Size = new System.Drawing.Size(100, 92);
            this.btnClearAllBins.TabIndex = 0;
            this.btnClearAllBins.Text = "Clear All Bins";
            this.btnClearAllBins.UseVisualStyleBackColor = true;
            this.btnClearAllBins.Click += new System.EventHandler(this.btnClearAllBins_Click);
            // 
            // gpAbout
            // 
            this.gpAbout.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gpAbout.Controls.Add(this.lblThirdPartyLicensing);
            this.gpAbout.Controls.Add(this.txtAboutVersion);
            this.gpAbout.Controls.Add(this.lblAboutVersionCaption);
            this.gpAbout.Controls.Add(this.lblAboutProductName);
            this.gpAbout.Location = new System.Drawing.Point(100, 4);
            this.gpAbout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gpAbout.Name = "gpAbout";
            this.gpAbout.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gpAbout.Size = new System.Drawing.Size(400, 159);
            this.gpAbout.TabIndex = 1;
            this.gpAbout.TabStop = false;
            this.gpAbout.Text = "About";
            // 
            // lblThirdPartyLicensing
            // 
            this.lblThirdPartyLicensing.Location = new System.Drawing.Point(8, 100);
            this.lblThirdPartyLicensing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThirdPartyLicensing.Name = "lblThirdPartyLicensing";
            this.lblThirdPartyLicensing.Size = new System.Drawing.Size(384, 55);
            this.lblThirdPartyLicensing.TabIndex = 3;
            this.lblThirdPartyLicensing.Text = "Includes the following 3rd Party components:\r\n(Licensing can be found in install " +
    "folder)\r\n- SharpSerializer";
            // 
            // txtAboutVersion
            // 
            this.txtAboutVersion.Location = new System.Drawing.Point(72, 63);
            this.txtAboutVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAboutVersion.Name = "txtAboutVersion";
            this.txtAboutVersion.ReadOnly = true;
            this.txtAboutVersion.Size = new System.Drawing.Size(132, 22);
            this.txtAboutVersion.TabIndex = 2;
            // 
            // lblAboutVersionCaption
            // 
            this.lblAboutVersionCaption.AutoSize = true;
            this.lblAboutVersionCaption.Location = new System.Drawing.Point(8, 66);
            this.lblAboutVersionCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAboutVersionCaption.Name = "lblAboutVersionCaption";
            this.lblAboutVersionCaption.Size = new System.Drawing.Size(56, 17);
            this.lblAboutVersionCaption.TabIndex = 1;
            this.lblAboutVersionCaption.Text = "Version";
            // 
            // lblAboutProductName
            // 
            this.lblAboutProductName.AutoSize = true;
            this.lblAboutProductName.Location = new System.Drawing.Point(8, 20);
            this.lblAboutProductName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAboutProductName.Name = "lblAboutProductName";
            this.lblAboutProductName.Size = new System.Drawing.Size(144, 34);
            this.lblAboutProductName.TabIndex = 0;
            this.lblAboutProductName.Text = "NYP L8 Bin Monitor\r\n(C) e-Docs USA 2014";
            // 
            // tpageManageUserProfiles
            // 
            this.tpageManageUserProfiles.AutoScroll = true;
            this.tpageManageUserProfiles.Controls.Add(this.userProfileManagerControl1);
            this.tpageManageUserProfiles.Location = new System.Drawing.Point(4, 25);
            this.tpageManageUserProfiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageUserProfiles.Name = "tpageManageUserProfiles";
            this.tpageManageUserProfiles.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageUserProfiles.Size = new System.Drawing.Size(1037, 601);
            this.tpageManageUserProfiles.TabIndex = 7;
            this.tpageManageUserProfiles.Text = "Manage Profiles";
            this.tpageManageUserProfiles.UseVisualStyleBackColor = true;
            // 
            // userProfileManagerControl1
            // 
            this.userProfileManagerControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.userProfileManagerControl1.CredentialHost = null;
            this.userProfileManagerControl1.Location = new System.Drawing.Point(0, 0);
            this.userProfileManagerControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.userProfileManagerControl1.Name = "userProfileManagerControl1";
            this.userProfileManagerControl1.Size = new System.Drawing.Size(699, 486);
            this.userProfileManagerControl1.TabIndex = 0;
            // 
            // tpageManageUsers
            // 
            this.tpageManageUsers.Controls.Add(this.userManagerControl1);
            this.tpageManageUsers.Location = new System.Drawing.Point(4, 25);
            this.tpageManageUsers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageUsers.Name = "tpageManageUsers";
            this.tpageManageUsers.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageUsers.Size = new System.Drawing.Size(1037, 601);
            this.tpageManageUsers.TabIndex = 3;
            this.tpageManageUsers.Text = "Manage Users";
            this.tpageManageUsers.UseVisualStyleBackColor = true;
            // 
            // userManagerControl1
            // 
            this.userManagerControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.userManagerControl1.CredentialHost = null;
            this.userManagerControl1.Location = new System.Drawing.Point(0, 0);
            this.userManagerControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.userManagerControl1.Name = "userManagerControl1";
            this.userManagerControl1.Size = new System.Drawing.Size(432, 416);
            this.userManagerControl1.TabIndex = 0;
            // 
            // tpageManageCategories
            // 
            this.tpageManageCategories.AutoScroll = true;
            this.tpageManageCategories.Controls.Add(this.ManageDefaultCategoryControl);
            this.tpageManageCategories.Location = new System.Drawing.Point(25, 25);
            this.tpageManageCategories.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageCategories.Name = "tpageManageCategories";
            this.tpageManageCategories.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageCategories.Size = new System.Drawing.Size(1037, 601);
            this.tpageManageCategories.TabIndex = 5;
            this.tpageManageCategories.Text = "Manage Categories";
            this.tpageManageCategories.UseVisualStyleBackColor = true;
            // 
            // ManageDefaultCategoryControl
            // 
            this.ManageDefaultCategoryControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ManageDefaultCategoryControl.CredentialHost = null;
            this.ManageDefaultCategoryControl.Location = new System.Drawing.Point(25, 25);
            this.ManageDefaultCategoryControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ManageDefaultCategoryControl.Name = "ManageDefaultCategoryControl";
            this.ManageDefaultCategoryControl.Size = new System.Drawing.Size(843, 677);
            this.ManageDefaultCategoryControl.TabIndex = 0;
            // 
            // tpageManageBatch
            // 
            this.tpageManageBatch.AutoScroll = true;
            this.tpageManageBatch.Controls.Add(this.BinLookupControl);
            this.tpageManageBatch.Location = new System.Drawing.Point(4, 25);
            this.tpageManageBatch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageManageBatch.Name = "tpageManageBatch";
            this.tpageManageBatch.Size = new System.Drawing.Size(1037, 601);
            this.tpageManageBatch.TabIndex = 2;
            this.tpageManageBatch.Text = "Manage Batch";
            this.tpageManageBatch.UseVisualStyleBackColor = true;
            // 
            // BinLookupControl
            // 
            this.BinLookupControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BinLookupControl.Location = new System.Drawing.Point(20, 20);
            this.BinLookupControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.BinLookupControl.Name = "BinLookupControl";
            this.BinLookupControl.Size = new System.Drawing.Size(821, 833);
            this.BinLookupControl.TabIndex = 0;
            // 
            // tpageHome
            // 
            this.tpageHome.AutoScroll = true;
            this.tpageHome.Controls.Add(this.grpManagerFunctions);
            this.tpageHome.Controls.Add(this.lblHomeMessage);
            this.tpageHome.Controls.Add(this.grpUserFunctions);
            this.tpageHome.Location = new System.Drawing.Point(4, 25);
            this.tpageHome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageHome.Name = "tpageHome";
            this.tpageHome.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpageHome.Size = new System.Drawing.Size(1037, 601);
            this.tpageHome.TabIndex = 0;
            this.tpageHome.Text = "Home";
            this.tpageHome.UseVisualStyleBackColor = true;
            // 
            // grpManagerFunctions
            // 
            this.grpManagerFunctions.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpManagerFunctions.Controls.Add(this.btnManageBatch);
            this.grpManagerFunctions.Location = new System.Drawing.Point(571, 196);
            this.grpManagerFunctions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpManagerFunctions.Name = "grpManagerFunctions";
            this.grpManagerFunctions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpManagerFunctions.Size = new System.Drawing.Size(262, 129);
            this.grpManagerFunctions.TabIndex = 4;
            this.grpManagerFunctions.TabStop = false;
            this.grpManagerFunctions.Text = "Manager Functions";
            // 
            // btnManageBatch
            // 
            this.btnManageBatch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnManageBatch.Location = new System.Drawing.Point(14, 27);
            this.btnManageBatch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnManageBatch.Name = "btnManageBatch";
            this.btnManageBatch.Size = new System.Drawing.Size(100, 92);
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
            this.lblHomeMessage.Location = new System.Drawing.Point(157, 66);
            this.lblHomeMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHomeMessage.Name = "lblHomeMessage";
            this.lblHomeMessage.Size = new System.Drawing.Size(676, 58);
            this.lblHomeMessage.TabIndex = 3;
            this.lblHomeMessage.Text = "Please fill in your credentials and select a tab to continue\r\nYou are responsible" +
    " for logging out";
            // 
            // grpUserFunctions
            // 
            this.grpUserFunctions.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grpUserFunctions.Controls.Add(this.btnCloseBatch);
            this.grpUserFunctions.Controls.Add(this.btnCompleteProcessing);
            this.grpUserFunctions.Controls.Add(this.btnCompleteRegistration);
            this.grpUserFunctions.Controls.Add(this.btnBeginRegistrationAndProcessing);
            this.grpUserFunctions.Controls.Add(this.btnBeginProcessing);
            this.grpUserFunctions.Controls.Add(this.btnBeginRegistration);
            this.grpUserFunctions.Controls.Add(this.btnFindSpecimen);
            this.grpUserFunctions.Controls.Add(this.btnUserManageBin);
            this.grpUserFunctions.Location = new System.Drawing.Point(215, 196);
            this.grpUserFunctions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpUserFunctions.Name = "grpUserFunctions";
            this.grpUserFunctions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpUserFunctions.Size = new System.Drawing.Size(348, 351);
            this.grpUserFunctions.TabIndex = 2;
            this.grpUserFunctions.TabStop = false;
            this.grpUserFunctions.Text = "User Functions";
            // 
            // btnCloseBatch
            // 
            this.btnCloseBatch.Location = new System.Drawing.Point(224, 226);
            this.btnCloseBatch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseBatch.Name = "btnCloseBatch";
            this.btnCloseBatch.Size = new System.Drawing.Size(100, 92);
            this.btnCloseBatch.TabIndex = 12;
            this.btnCloseBatch.Text = "Close \r\nBatch\r\n[F11]";
            this.btnCloseBatch.UseVisualStyleBackColor = true;
            this.btnCloseBatch.Click += new System.EventHandler(this.btnCloseBatch_Click);
            // 
            // btnCompleteProcessing
            // 
            this.btnCompleteProcessing.Location = new System.Drawing.Point(116, 226);
            this.btnCompleteProcessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCompleteProcessing.Name = "btnCompleteProcessing";
            this.btnCompleteProcessing.Size = new System.Drawing.Size(100, 92);
            this.btnCompleteProcessing.TabIndex = 11;
            this.btnCompleteProcessing.Text = "Complete \r\nProcessing\r\n[F10]";
            this.btnCompleteProcessing.UseVisualStyleBackColor = true;
            this.btnCompleteProcessing.Click += new System.EventHandler(this.btnCompleteProcessing_Click);
            // 
            // btnCompleteRegistration
            // 
            this.btnCompleteRegistration.Location = new System.Drawing.Point(8, 226);
            this.btnCompleteRegistration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCompleteRegistration.Name = "btnCompleteRegistration";
            this.btnCompleteRegistration.Size = new System.Drawing.Size(100, 92);
            this.btnCompleteRegistration.TabIndex = 10;
            this.btnCompleteRegistration.Text = "Complete \r\nRegistration\r\n[F9]";
            this.btnCompleteRegistration.UseVisualStyleBackColor = true;
            this.btnCompleteRegistration.Click += new System.EventHandler(this.btnCompleteRegistration_Click);
            // 
            // btnBeginRegistrationAndProcessing
            // 
            this.btnBeginRegistrationAndProcessing.Location = new System.Drawing.Point(224, 127);
            this.btnBeginRegistrationAndProcessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBeginRegistrationAndProcessing.Name = "btnBeginRegistrationAndProcessing";
            this.btnBeginRegistrationAndProcessing.Size = new System.Drawing.Size(100, 92);
            this.btnBeginRegistrationAndProcessing.TabIndex = 9;
            this.btnBeginRegistrationAndProcessing.Text = "Begin\r\nBoth\r\n[F8]";
            this.btnBeginRegistrationAndProcessing.UseVisualStyleBackColor = true;
            this.btnBeginRegistrationAndProcessing.Click += new System.EventHandler(this.btnBeginRegistrationAndProcessing_Click);
            // 
            // btnBeginProcessing
            // 
            this.btnBeginProcessing.Location = new System.Drawing.Point(116, 127);
            this.btnBeginProcessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBeginProcessing.Name = "btnBeginProcessing";
            this.btnBeginProcessing.Size = new System.Drawing.Size(100, 92);
            this.btnBeginProcessing.TabIndex = 8;
            this.btnBeginProcessing.Text = "Begin\r\nProcessing\r\n[F7]";
            this.btnBeginProcessing.UseVisualStyleBackColor = true;
            this.btnBeginProcessing.Click += new System.EventHandler(this.btnBeginProcessing_Click);
            // 
            // btnBeginRegistration
            // 
            this.btnBeginRegistration.Location = new System.Drawing.Point(8, 127);
            this.btnBeginRegistration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBeginRegistration.Name = "btnBeginRegistration";
            this.btnBeginRegistration.Size = new System.Drawing.Size(100, 92);
            this.btnBeginRegistration.TabIndex = 7;
            this.btnBeginRegistration.Text = "Begin\r\nRegistration\r\n[F6]";
            this.btnBeginRegistration.UseVisualStyleBackColor = true;
            this.btnBeginRegistration.Click += new System.EventHandler(this.btnBeginRegistration_Click);
            // 
            // btnFindSpecimen
            // 
            this.btnFindSpecimen.Location = new System.Drawing.Point(8, 27);
            this.btnFindSpecimen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFindSpecimen.Name = "btnFindSpecimen";
            this.btnFindSpecimen.Size = new System.Drawing.Size(100, 92);
            this.btnFindSpecimen.TabIndex = 6;
            this.btnFindSpecimen.Text = "Find \r\nSpecimen\r\n[F4]";
            this.btnFindSpecimen.UseVisualStyleBackColor = true;
            this.btnFindSpecimen.Click += new System.EventHandler(this.btnFindSpecimen_Click);
            // 
            // btnUserManageBin
            // 
            this.btnUserManageBin.Location = new System.Drawing.Point(116, 27);
            this.btnUserManageBin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUserManageBin.Name = "btnUserManageBin";
            this.btnUserManageBin.Size = new System.Drawing.Size(100, 92);
            this.btnUserManageBin.TabIndex = 4;
            this.btnUserManageBin.Text = "Manage\r\nBatch\r\n[F5]";
            this.btnUserManageBin.UseVisualStyleBackColor = true;
            this.btnUserManageBin.Click += new System.EventHandler(this.btnUserManageBin_Click);
            // 
            // TabPanel
            // 
            this.TabPanel.Controls.Add(this.tpageHome);
            this.TabPanel.Controls.Add(this.tpageManageBatch);
            this.TabPanel.Controls.Add(this.tpageManageCategories);
            this.TabPanel.Controls.Add(this.tpageManageUsers);
            this.TabPanel.Controls.Add(this.tpageManageUserProfiles);
            this.TabPanel.Controls.Add(this.tPageSettings);
            this.TabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPanel.Location = new System.Drawing.Point(0, 62);
            this.TabPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabPanel.Name = "TabPanel";
            this.TabPanel.SelectedIndex = 0;
            this.TabPanel.Size = new System.Drawing.Size(1045, 630);
            this.TabPanel.TabIndex = 0;
            this.TabPanel.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabPanel_Selecting);
            // 
            // RoomManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 692);
            this.Controls.Add(this.TabPanel);
            this.Controls.Add(this.grpUserCredentials);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RoomManagerForm";
            this.Text = "Room Manager - e-Docs USA, Inc.";
            this.grpUserCredentials.ResumeLayout(false);
            this.grpUserCredentials.PerformLayout();
            this.tPageSettings.ResumeLayout(false);
            this.gpTesting.ResumeLayout(false);
            this.gpAbout.ResumeLayout(false);
            this.gpAbout.PerformLayout();
            this.tpageManageUserProfiles.ResumeLayout(false);
            this.tpageManageUsers.ResumeLayout(false);
            this.tpageManageCategories.ResumeLayout(false);
            this.tpageManageBatch.ResumeLayout(false);
            this.tpageHome.ResumeLayout(false);
            this.tpageHome.PerformLayout();
            this.grpManagerFunctions.ResumeLayout(false);
            this.grpUserFunctions.ResumeLayout(false);
            this.TabPanel.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tpageHome;
        private System.Windows.Forms.GroupBox grpManagerFunctions;
        private System.Windows.Forms.Button btnManageBatch;
        private System.Windows.Forms.Label lblHomeMessage;
        private System.Windows.Forms.GroupBox grpUserFunctions;
        private System.Windows.Forms.Button btnFindSpecimen;
        private System.Windows.Forms.TabControl TabPanel;
        public System.Windows.Forms.Button btnUserManageBin;
        private UserAuthenticationControl userAuthenticationControl1;
        private System.Windows.Forms.Button btnBeginRegistration;
        private System.Windows.Forms.Button btnBeginProcessing;
        private System.Windows.Forms.Button btnBeginRegistrationAndProcessing;
        private System.Windows.Forms.Button btnCloseBatch;
        private System.Windows.Forms.Button btnCompleteProcessing;
        private System.Windows.Forms.Button btnCompleteRegistration;
    }
}
