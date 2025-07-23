using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BinMonitor.Common
{
    public partial class BatchManagerControl : UserControl
    {
        public BatchManagerControl()
        {
            InitializeComponent();
            BatchStatusManager.WorkflowAssignmentMode = Batches.WorkflowAssignmentMode.Choice;
        }

        public void Clear()
        {this.ActiveBatch = null; }

        private IUserSource _CredentialHost = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IUserSource CredentialHost
        {
            get { return _CredentialHost; }
            set 
            { 
                _CredentialHost = value;
                OnCredentialHostChanged();
            }
        }

        protected void OnCredentialHostChanged()
        {
            BatchStatusManager.CredentialHost = this.CredentialHost;
            this.Enabled = CredentialHost != null;            
        }

        protected IUserSource EnsureGetCredentialHost()
        {
            IUserSource cHost = this.CredentialHost;
            if (cHost == null)
            { throw new InvalidOperationException("No CredentialHost has been configured"); }
            return cHost;
        }

        protected User EnsureGetSelectedUser()
        {
            IUserSource host = EnsureGetCredentialHost();
            return host.EnsureGetSelectedUser();
        }

        private bool _IsArchive = false;
        public bool IsArchive
        {
            get { return _IsArchive; }
            set { _IsArchive = value; }
        }

        private SpecimenBatch _ActiveBatch = null;
        public SpecimenBatch ActiveBatch
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
                if (value != null)
                {
                    if (value.IsClosed)
                    {
                        this.txtNewComment.Enabled = false;
                        this.btnAddComment.Enabled = false;
                    }
                    if (this._IsArchive)
                    {
                        this.btnAddComment.Enabled = false;
                        this.txtNewComment.Enabled = false;
                    }
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

        protected void OnActiveBatchUpdated()
        {
            SpecimenBatch Batch = this.ActiveBatch;
            lbBatchContents.ClearSelected();
            
            if (Batch == null)
            {
                BatchStatusManager.ActiveBatch = null;
                txtCategory.Clear();
                txtComments.Clear();
                txtNewComment.Clear();
                txtTransferredFromBatchId.Clear();
                lbBatchContents.DataSource = null;
            }
            else
            {
                if (Batch.IsClosed)
                {
                    this.btnAddComment.Enabled = false;
                    this.txtNewComment.Enabled = false;
                }
                if (this._IsArchive)
                {
                    this.btnAddComment.Enabled = false;
                    this.txtNewComment.Enabled = false;
                }
                BatchStatusManager.ActiveBatch = Batch;
               
                txtCategory.Text = Batch.Category.Title;
                txtComments.Text = Batch.Comments;
                txtNewComment.Clear();
                txtTransferredFromBatchId.Text = Batch.TransferredFrom;
                lbBatchContents.DataSource = Batch.Specimens;
            }
        }/*
        public void OnActiveBatchUpdated(SpecimenBatch test)
        {
            lbBatchContents.ClearSelected();
            if (test == null)
            {
                BatchStatusManager.ActiveBatch = null;
                txtCategory.Clear();
                txtComments.Clear();
                txtNewComment.Clear();
                txtTransferredFromBatchId.Clear();
                lbBatchContents.DataSource = null;
            }
            else
            {
                BatchStatusManager.ActiveBatch = test;
                txtCategory.Text = test.Category.Title;
                txtComments.Text = test.Comments;
                txtNewComment.Clear();
                txtTransferredFromBatchId.Text = test.TransferredFrom;
                lbBatchContents.DataSource = test.Specimens;
            }
        }
        */
        protected void ActiveBatch_WorkflowStepChanged(object sender, EventArgs e)
        { OnActiveBatchUpdated(); }

        protected void ActiveBatch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnActiveBatchUpdated(); }

        protected void ActiveBatch_CheckpointChanged(object sender, EventArgs e)
        { OnActiveBatchUpdated(); }

        protected void ActiveBatch_Closed(object sender, EventArgs e)
        { this.ActiveBatch = null; }    

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNewComment.Text))
                { throw new InvalidOperationException("Comment is required"); }

                SpecimenBatch Batch = this.EnsureGetActiveBatch();
                User user = CredentialHost.EnsureGetSelectedUser();
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

        private void btnChangeCategory_Click(object sender, EventArgs e)
        {
            try
            {
                Category cat = CategoryPickerDialog.SelectCategory(this);
                //If they selected the same category, continue
                if (cat.Title.Equals(txtCategory.Text))
                { return; }

                string origCatTitle = txtCategory.Text;
                string newCatTitle = cat.Title;
                txtCategory.Text = newCatTitle;
                SpecimenBatch batch = EnsureGetActiveBatch();
                batch.CategoryId = cat.Title;
                User user = CredentialHost.EnsureGetSelectedUser();
                DateTime timeStamp = DateTime.Now;
                string comment = string.Format("Category changed from {0} to {1}", origCatTitle, newCatTitle);
                batch.AddComment(comment, user.Id, timeStamp);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }
    }
}
