using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class SelfManageBatchDialog : Form
    {
        public IUserSource CredentialHost
        { get { return this.UserAuthenticationControl.UserSource; } }

        private SpecimenBatch _ActiveBatch = null;
        protected SpecimenBatch ActiveBatch
        {
            get { return _ActiveBatch; }
            set
            {
                if (_ActiveBatch != null)
                {
                    _ActiveBatch.WorkflowStepChanged -= ActiveBatch_WorkflowStepChanged;
                    _ActiveBatch.PropertyChanged -= ActiveBatch_PropertyChanged;
                    _ActiveBatch.CheckpointChanged -= ActiveBatch_CheckpointChanged;
                    _ActiveBatch.Closed -= ActiveBatch_Closed;
                }
                if (value != null)
                {
                    value.WorkflowStepChanged += ActiveBatch_WorkflowStepChanged;
                    value.PropertyChanged += ActiveBatch_PropertyChanged;
                    value.CheckpointChanged += ActiveBatch_CheckpointChanged;
                    value.Closed += ActiveBatch_Closed;
                }
                _ActiveBatch = value;
                OnActiveBatchUpdated();
            }
        }

        protected SpecimenBatch EnsureGetActiveBatch()
        {
            SpecimenBatch Batch = ActiveBatch;
            if (Batch == null)
            { throw new InvalidOperationException("No active Batch found"); }
            return Batch;
        }

        protected MasterCategoryPermissions EnsureGetActiveBatchPermissions()
        {
            SpecimenBatch Batch = EnsureGetActiveBatch();
            User user = CredentialHost.EnsureGetSelectedUser();
            return user.UserProfile.CategoryPermissions[Batch.EnsureGetCategory().MasterCategoryTitle];
        }

        protected void OnActiveBatchUpdated()
        {
            SpecimenBatch Batch = this.ActiveBatch;
            this.BatchStatusManager.ActiveBatch = Batch;
            txtComments.Clear();
            txtNewComment.Clear();
            if (Batch != null)
            {
                txtComments.Text = Batch.Comments;   
            }
        }

        protected void ActiveBatch_WorkflowStepChanged(object sender, EventArgs e)
        { OnActiveBatchUpdated(); }

        protected void ActiveBatch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnActiveBatchUpdated(); }

        protected void ActiveBatch_CheckpointChanged(object sender, EventArgs e)
        { OnActiveBatchUpdated(); }

        protected void ActiveBatch_Closed(object sender, EventArgs e)
        { this.ActiveBatch = null; }

        public SelfManageBatchDialog()
        {
            InitializeComponent();
            BatchStatusManager.CredentialHost = CredentialHost;
        }       

        public void Clear()
        { 
            ClearBatchDetails();
        }

        protected void ClearBatchDetails()
        {
            ActiveBatch = null;
        }       

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNewComment.Text))
                { throw new InvalidOperationException("Comment is required"); }

                User user = CredentialHost.EnsureGetSelectedUser();
                SpecimenBatch Batch = this.EnsureGetActiveBatch();

                MasterCategoryPermissions perm = user.EnsureGetUserProfile().CategoryPermissions[Batch.Category.MasterCategoryTitle];
                if (perm.CanAddComment == false)
                { throw new InvalidOperationException("You do not have permission to add comments to the selected Batch"); }

                DateTime timeStamp = DateTime.Now;
                Batch.AddComment(txtNewComment.Text, user.Id, timeStamp);
                txtNewComment.Clear();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void BatchLookup_SelectedBatchChanged(object sender, SelectedBatchChangedEventArgs e)
        {
            ActiveBatch = e.SelectedBatch;
        }

        private void SelfManageBatchDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                ErrorProvider.Clear();
                UserAuthenticationControl.RequestUserAuthentication();
                BatchLookup.Focus();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(UserAuthenticationControl, ex.Message);
            }
        }

        private void txtNewComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnAddComment.PerformClick(); }
        }
    }
}
