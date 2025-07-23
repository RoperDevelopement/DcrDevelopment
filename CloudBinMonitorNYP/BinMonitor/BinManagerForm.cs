/*
 * User: Sam Brinly
 * Date: 11/19/2014
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using BinMonitor.Common;
using System.Net;
using System.Diagnostics;
using System.Reflection;
using SqlCommands;
using System.Data.SqlClient;
using System.Data;
using BinMonitor.Common.Sharepoint;


//using SP = Microsoft.SharePoint.Client;
using BinMonitor.Common.Batches;

namespace BinMonitor
{
    public partial class BinManagerForm : Form
    {
        IUserSource CredentialHost;
        public void Test()
        {
            //MessageBox.Show("TEST");
            try
            {
                ManageBatchDialog dlg = new ManageBatchDialog();
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }





        public BinManagerForm(bool getXmlFiles)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //


            if (getXmlFiles)
            {

                BinUtilities.BinMointorUtilties.NumberDays = Settings.Application.Default.DaysToKeepArchiveFiles;
                BinUtilities.BinMointorUtilties.CleanUpFolders();
                BmSqlServerXmlFiles.AddBatchesCloud.ReloadAllBinMomitorXmlFiles(true);
            }
            InitializeComponent();
            CredentialHost = userAuthenticationControl1.UserSource;
            BinLookupControl.CredentialHost = CredentialHost;
            CreateBatchControl.CredentialHost = CredentialHost;
            ManageDefaultCategoryControl.CredentialHost = CredentialHost;
            userManagerControl1.CredentialHost = CredentialHost;
            userProfileManagerControl1.CredentialHost = CredentialHost;
            txtAboutVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            CredentialHost.SelectedUserChanged += CredentialHost_SelectedUserChanged;
        }

        void CredentialHost_SelectedUserChanged(object sender, SelectedUserChangedEventArgs e)
        {
            TabPanel.SelectedTab = tpageHome;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (TabPanel.SelectedTab != tpageHome)
            { return base.ProcessCmdKey(ref msg, keyData); }

            switch (keyData)
            {
                case Keys.F4:
                    btnFindSpecimen.PerformClick();
                    return true;
                case Keys.F5:
                    btnUserManageBin.PerformClick();
                    return true;
                case Keys.F6:
                    btnBeginRegistration.PerformClick();
                    return true;
                case Keys.F7:
                    btnBeginProcessing.PerformClick();
                    return true;
                case Keys.F8:
                    btnBeginRegistrationAndProcessing.PerformClick();
                    return true;
                case Keys.F9:
                    btnCompleteRegistration.PerformClick();
                    return true;
                case Keys.F10:
                    btnCompleteProcessing.PerformClick();
                    return true;
                case Keys.F11:
                    btnCloseBatch.PerformClick();
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

        }

        private void TabPanel_Selecting(object sender, TabControlCancelEventArgs e)
        {
            this.txtAboutVersion.Text = BinUtilities.BinMointorUtilties.GetAssemblyVersion();
            if (e.TabPage == tpageHome)
            { return; }
            else if (CredentialHost.SelectedUser == null)
            {
                MessageBox.Show("Not Authorized");
                e.Cancel = true;
            }
            else if (e.TabPage == tpageManageCategories)
            {
                SpecimenCategories.Instance.DirectoryPath = BinUtilities.BinMonCategoriesFolder;
                SpecimenCategories.Instance.Reload();
            }
                
            else if (e.TabPage == tabPageChanges)
            {

                txtBoxEmail.Width = CredentialHost.SelectedUser.EmailAddress.Length + 150;
                txtBoxEmail.Text = CredentialHost.SelectedUser.EmailAddress;
                //    this.txtBoxEmail.Size = new System.Drawing.Size(txtBoxEmail.Width, 20);

            }
            else
            {
                if (e.TabPage == tPageSettings)
                {
                    GetEmailAddress();

                }
            }
        }

        private void GetEmailAddress()
        {
            SqlCmd CmdSql = new SqlCommands.SqlCmd();
            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {

                DataSet ds = CmdSql.GetEmailAddress(SqlCommands.SqlConstants.SpEmailAddress, sqlConnection);
                cmbEmailTo.DataSource = ds.Tables[0].DefaultView;
                cmbEmailTo.DisplayMember = "Email";
                cmbEmailTo.BindingContext = this.BindingContext;
                cmbEmailTo.SelectedItem = null;
                GetCurrentEmailReoprtInfo();
            }
        }

        private void GetCurrentEmailReoprtInfo()
        {
            SqlCmd CmdSql = new SqlCommands.SqlCmd();
            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {

                using (SqlDataReader dr = CmdSql.SqlDataReader(SqlConstants.SpEmailReportsUsers, sqlConnection))
                {
                    while (dr.Read())
                    {
                        cmbEmailTo.Text = dr[BinUtilities.BinMointorUtilties.ReplaceString(SqlConstants.SpParmaEmailTo, SqlConstants.RepStrAt, string.Empty)].ToString();
                        txtEmailAddress.Text = dr[BinUtilities.BinMointorUtilties.ReplaceString(SqlConstants.SpParmaEmailCC, SqlConstants.RepStrAt, string.Empty)].ToString();
                        txtEmailFreq.Text = dr[BinUtilities.BinMointorUtilties.ReplaceString(SqlConstants.SpParmaEmailFrequency, SqlConstants.RepStrAt, string.Empty)].ToString();
                    }
                }
            }
        }
        private void btnNewBatch_Click(object sender, EventArgs e)
        {
            try
            {
                CreateBatchDialog dlg = new CreateBatchDialog();
                //dlg.WindowState = FormWindowState.Maximized;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }

            /*
            CreateBatchControl.Clear();
            TabPanel.SelectedTab = tpageNewBatch;
            */
        }
        private void btnArchiveBatch_Click(object sender, EventArgs e)
        {
            try
            {
                ArchiveBatchDialog dlg = new ArchiveBatchDialog();
                dlg.ShowDialog(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }

            //TabPanel.SelectedTab = tpageManageBatch;
        }
        private void btnManageBatch_Click(object sender, EventArgs e)
        {
            try
            {
                ManageBatchDialog dlg = new ManageBatchDialog();
                // dlg.WindowState = FormWindowState.Maximized;
                // dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }

            //TabPanel.SelectedTab = tpageManageBatch;
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            TabPanel.SelectedTab = tpageManageUsers;
        }


        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            TabPanel.SelectedTab = tpageManageCategories;
        }

        private void btnClearAllBins_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Intended for testing only, the active bins may not be archived", "Are you sure", MessageBoxButtons.OKCancel) != DialogResult.OK)
                { throw new OperationCanceledException(); }

                foreach (Bin bin in Bins.Instance.Values.ToArray())
                {
                    if (bin.BatchId != null)
                    {
                        SpecimenBatches.Instance.TryMoveToArchiveWithLock(bin.BatchId);
                        SpecimenBatches.Instance.TryRemove(bin.BatchId);
                        bin.Clear();
                        Bins.Instance.Save(bin.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }



        private void btnUserManageBin_Click(object sender, EventArgs e)
        {
            try
            {
                SelfManageBatchDialog dlg = new SelfManageBatchDialog()
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }


        private void btnFindSpecimen_Click(object sender, EventArgs e)
        {
            BinLookupBySpecimenIdDialog dlg = new BinLookupBySpecimenIdDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);
        }





        private void btnBeginRegistration_Click(object sender, EventArgs e)
        {
            try
            {

                Action<SpecimenBatch, User> batchAction = (batch, user) =>
                {
                    SpecimenBatchManager.ConfirmNextPriorityBatch(this, batch);
                    SpecimenBatchManager.CheckOutForRegistration(batch, user);
                };
                BatchQuickStepDialog.PerformBatchQuickStep(this, "Begin Registration", batchAction);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBeginProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                Action<SpecimenBatch, User> batchAction = (batch, user) =>
                {
                    SpecimenBatchManager.ConfirmNextPriorityBatch(this, batch);
                    SpecimenBatchManager.CheckOutForProcessing(batch, user);
                };
                BatchQuickStepDialog.PerformBatchQuickStep(this, "Begin Processing", batchAction);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBeginRegistrationAndProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                Action<SpecimenBatch, User> batchAction = (batch, user) =>
                {
                    SpecimenBatchManager.ConfirmNextPriorityBatch(this, batch);
                    SpecimenBatchManager.CheckOutForRegistrationAndProcessing(batch, user);
                };
                BatchQuickStepDialog.PerformBatchQuickStep(this, @"Begin Registration & Processing", batchAction);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCloseBatch_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine();
                Action<SpecimenBatch, User> batchAction = (batch, user) =>
                { SpecimenBatchManager.FinalizeBatch(batch, user, true); };
                BatchQuickStepDialog.PerformBatchQuickStep(this, "Close Batch", batchAction);

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCompleteRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                Action<SpecimenBatch, User> batchAction = (batch, user) =>
                { SpecimenBatchManager.CompleteRegistration(batch, user); };
                BatchQuickStepDialog.PerformBatchQuickStep(this, "Complete Registration", batchAction);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCompleteProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                Action<SpecimenBatch, User> batchAction = (batch, user) =>
                { SpecimenBatchManager.CompleteProcessing(batch, user); };
                BatchQuickStepDialog.PerformBatchQuickStep(this, "Complete Processing", batchAction);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void userAuthenticationControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BinMonitorSqlServer.SqlServerInstance.UserBinsBatches();
            BinMonitorSqlServer.SqlServerInstance.GetUnactiveBatches();
            Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
            Bins.Instance.Reload();
            SpecimenBatches.Instance.DirectoryPath = BinUtilities.BinMonSpecimenBatchesFolder;
            SpecimenBatches.Instance.Reload();
        }

        private void tPageSettings_Click(object sender, EventArgs e)
        {

        }

        private void BinManagerForm_Shown(object sender, EventArgs e)
        {
            //     BinMonitorSqlServer.SqlServerInstance.UserBinsBatches();
            //BinMonitorSqlServer.SqlServerInstance.GetUnactiveBatches();
            //   SpecimenBatches.Instance.DirectoryPath = BinUtilities.BinMonSpecimenBatchesFolder;
            //   SpecimenBatches.Instance.Reload();
            //   Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
            //     Bins.Instance.Reload();

        }

        private void btnRefreshBimMonitor_Click(object sender, EventArgs e)
        {
            BinMonitorSqlServer.SqlServerInstance.UserBinsBatches();
            BinMonitorSqlServer.SqlServerInstance.GetUnactiveBatches();
            Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
            Bins.Instance.Reload();
        }

        private void btnSyncCloud_Click(object sender, EventArgs e)
        {
            BinMonitorSqlServer.SqlServerInstance.UserBinsBatches();
            BinMonitorSqlServer.SqlServerInstance.GetUnactiveBatches();
            BinUtilities.BinMointorUtilties.DeleteFiles(BinUtilities.BinMonUserFolder);
            BmSqlServerXmlFiles.AddBatchesCloud.LoadBinUsersXmlFiles();

            Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
            Bins.Instance.Reload();
            Users.Instance.DirectoryPath = BinUtilities.BinMonUserFolder;
            Users.Instance.Reload();
        }

        private void btnSyncUsers_Click(object sender, EventArgs e)
        {
            BinUtilities.BinMointorUtilties.DeleteFiles(BinUtilities.BinMonUserFolder);
            BmSqlServerXmlFiles.AddBatchesCloud.LoadBinUsersXmlFiles();
            Users.Instance.DirectoryPath = BinUtilities.BinMonUserFolder;
            Users.Instance.Reload();
        }

        private void txtEmailAddress_Enter(object sender, EventArgs e)
        {
            txtEmailAddress.Text = string.Empty;
            EmailAddress emailAddress = new EmailAddress();
            emailAddress.ShowDialog();
            txtEmailAddress.Text = emailAddress.txtEmailAdd.Text;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmailFreq.Text))
            {
                txtEmailFreq.Text = "24";
            }
            else
            {
                int tempInt = 0;
                if (!(int.TryParse(txtEmailFreq.Text, out tempInt)))
                {
                    MessageBox.Show(string.Format("Email frequency {0} is invald must be an integer between .1 and 24", txtEmailFreq.Text));
                    txtEmailFreq.Text = "24";
                    return;
                }
                else
                {
                    if ((tempInt <= 0) || (tempInt > 24))
                    {
                        MessageBox.Show(string.Format("Email frequency {0} is invald must be an integer between .1 and 24", tempInt));
                        txtEmailFreq.Text = "24";
                        return;
                    }

                }
                if ((string.IsNullOrEmpty(cmbEmailTo.Text)) && (string.IsNullOrEmpty(txtEmailAddress.Text)))
                {
                    MessageBox.Show("Nothing updated");
                    return;
                }
                if ((string.IsNullOrEmpty(cmbEmailTo.Text)))
                {
                    MessageBox.Show("Email To cannot be empty");
                    return;
                }
                SqlCmd CmdSql = new SqlCommands.SqlCmd();
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    Dictionary<string, string> dicEmail = new Dictionary<string, string>();
                    if ((string.IsNullOrEmpty(cmbEmailTo.Text)))
                        dicEmail.Add(SqlConstants.SpParmaEmailTo, string.Empty);
                    else
                        dicEmail.Add(SqlConstants.SpParmaEmailTo, cmbEmailTo.Text);

                    if ((string.IsNullOrEmpty(txtEmailAddress.Text)))
                        dicEmail.Add(SqlConstants.SpParmaEmailCC, string.Empty);
                    else
                        dicEmail.Add(SqlConstants.SpParmaEmailCC, txtEmailAddress.Text);
                    dicEmail.Add(SqlConstants.SpParmaEmailFrequency, txtEmailFreq.Text);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlConstants.spEmailReports, dicEmail, sqlConnection))
                    { }

                    MessageBox.Show("Email Report user updated");

                    //cmbEmailTo.Text;
                    //txtEmailAddress.Text;

                    //    DataSet ds = CmdSql.GetEmailAddress(SqlCommands.SqlConstants.Sp_EmailAddress, sqlConnection);

                    // cmbEmailTo.DataSource = ds.Tables[0].DefaultView;
                    //cmbEmailTo.DisplayMember = "Email";
                    /// cmbEmailTo.BindingContext = this.BindingContext;



                }

            }
        }

        private void butSubChanges_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtBoxEmail.Text)) || (!(BinUtilities.BinMointorUtilties.CheckEmailAddress(txtBoxEmail.Text))))
                MessageBox.Show($"Invaild Email Address {txtBoxEmail.Text}", "Invalid Email Address", MessageBoxButtons.OK);
            else if (string.IsNullOrWhiteSpace(txtBoxSubject.Text))
                MessageBox.Show($"Need a subject", "Invalid Subject", MessageBoxButtons.OK);
            else if ((richTxtBoxChanges.TextLength == 0) || (string.IsNullOrWhiteSpace(richTxtBoxChanges.Text)))
                MessageBox.Show($"Need issues found", "No Issues", MessageBoxButtons.OK);
            else
            {
                string retStr = BinMonitorSqlServer.SqlServerInstance.AddBinMonitorChages(txtBoxEmail.Text, txtBoxSubject.Text, richTxtBoxChanges.Text);
                if (!(string.IsNullOrWhiteSpace(retStr)))
                    MessageBox.Show($"Error adding changes message:{retStr}", "Changes Not Submitted", MessageBoxButtons.OK);
                else
                {
                    MessageBox.Show("Changes have been submitted you should receive and email from edocs support in 24 hrs", "Changes Submitted", MessageBoxButtons.OK);
                    richTxtBoxChanges.Clear();
                }
               
            }
        }


        /*
public static void CheckOutForRegistration(IWin32Window parent)
{

}
* */
    }
}
