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
		public BinManagerForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
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
            {return base.ProcessCmdKey(ref msg, keyData);}

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
            if (e.TabPage == tpageHome)
            { return; }
            else if (CredentialHost.SelectedUser == null)
            {
                MessageBox.Show("Not Authorized");
                e.Cancel = true;
            }
        }

        private void btnNewBatch_Click(object sender, EventArgs e)
        {
            try
            {
                CreateBatchDialog dlg = new CreateBatchDialog();
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
                    SpecimenBatchManager.ConfirmNextPriorityBatch(this,batch);
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
                    SpecimenBatchManager.ConfirmNextPriorityBatch(this,batch);
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
        /*
        public static void CheckOutForRegistration(IWin32Window parent)
        {

        }
         * */
	}
}
