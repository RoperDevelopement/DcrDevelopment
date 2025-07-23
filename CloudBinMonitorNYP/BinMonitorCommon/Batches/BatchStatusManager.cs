using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BinMonitor.Common.Batches
{

    public enum WorkflowAssignmentMode
    { Self, Choice }

    public partial class BatchStatusManager : UserControl
    {
        ErrorProvider ErrorProvider = new ErrorProvider() 
        { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        WorkflowAssignmentMode _WorkflowAssignmentMode = WorkflowAssignmentMode.Self;
        public WorkflowAssignmentMode WorkflowAssignmentMode
        {
            get { return _WorkflowAssignmentMode; }
            set { _WorkflowAssignmentMode = value; }
        }

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
                    
                }
                if (value != null)
                {
                    
                    value.WorkflowStepChanged += ActiveBatch_WorkflowStepChanged;
                    value.PropertyChanged += ActiveBatch_PropertyChanged;
                    value.CheckpointChanged += ActiveBatch_CheckpointChanged;
                  

                    if (value.IsClosed) {
                        this.txtClosedAt.Text = value.ClosedAt.ToString();
                        this.txtClosedBy.Text = value.ClosedBy;
                        this.btnClose.Enabled = false;
                    }
                    if(this.IsArchive){
                        this.txtClosedAt.Text = value.ClosedAt.ToString();
                        this.txtClosedBy.Text = value.ClosedBy;
                        this.btnClose.Enabled = false;
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
            if (Batch.IsClosed)
            {
                this.txtClosedAt.Text = Batch.ClosedAt.ToString();
                this.txtClosedBy.Text = Batch.ClosedBy;
                this.btnClose.Enabled = false;
            }
            if (this.IsArchive)
            {
                this.txtClosedAt.Text = Batch.ClosedAt.ToString();
                this.txtClosedBy.Text = Batch.ClosedBy;
                this.btnClose.Enabled = false;
            }
            return Batch;
        }

        protected MasterCategoryPermissions EnsureGetActiveBatchPermissions()
        {
            SpecimenBatch Batch = EnsureGetActiveBatch();
            User user = EnsureGetSelectedUser();
            return user.UserProfile.CategoryPermissions[Batch.EnsureGetCategory().MasterCategoryTitle];
        }

        protected void OnActiveBatchUpdated()
        {
            SpecimenBatch Batch = this.ActiveBatch;
            if (Batch == null)
            {
                wfsRegister.Clear();
                wfsProcess.Clear();
                txtCreatedBy.Clear();
                txtCreatedAt.Clear();
                txtClosedBy.Clear();
                txtClosedAt.Clear();
                btnBeginProcessing.Enabled = false;
                btnCompleteProcessing.Enabled = false;
                btnBeginRegistration.Enabled = false;
                btnCompleteRegistration.Enabled = false;
                btnClose.Enabled = false;
            }
            else
            {
                if (this.IsArchive)
                {
                    this.txtClosedAt.Text = Batch.ClosedAt.ToString();
                    this.txtClosedBy.Text = Batch.ClosedBy;
                    this.btnClose.Enabled = false;
                }
                if (Batch.IsClosed == true)
                {
                    this.txtClosedAt.Text = Batch.ClosedAt.ToString();
                    this.txtClosedBy.Text = Batch.ClosedBy;
                    this.btnClose.Enabled = false;
                }
                wfsRegister.LoadFromExisting(Batch.Registration);
                wfsProcess.LoadFromExisting(Batch.Processing);
                txtCreatedBy.Text = Batch.CreatedBy;
                txtCreatedAt.Text = Batch.CreatedAt == DateTime.MinValue
                    ? string.Empty
                    : Batch.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                txtClosedBy.Text = Batch.ClosedBy;
                txtClosedAt.Text = Batch.ClosedAt == DateTime.MinValue
                    ? string.Empty
                    : Batch.ClosedAt.ToString("yyyy-MM-dd HH:mm:ss");
                btnBeginRegistration.Enabled = ActiveBatch.Registration.HasStarted == false;
                btnCompleteRegistration.Enabled = (ActiveBatch.Registration.HasStarted == true)
                    && (ActiveBatch.Registration.HasCompleted == false);
                btnBeginProcessing.Enabled = ActiveBatch.Processing.HasStarted == false;
                btnCompleteProcessing.Enabled = (ActiveBatch.Processing.HasStarted == true)
                    && (ActiveBatch.Processing.HasCompleted == false);
                btnClose.Enabled = (ActiveBatch.Registration.HasStarted == true)
                    && (ActiveBatch.Processing.HasStarted == true)
                    && (ActiveBatch.IsClosed == false);
                
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

        public BatchStatusManager()
        {
            InitializeComponent();
        }

        private void btnBeginRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                User assignedBy = this.EnsureGetSelectedUser();
                SpecimenBatch batch = this.EnsureGetActiveBatch();
                if (batch.Registration.HasStarted)
                { throw new InvalidOperationException("The registration step on the specified bin has already been started"); }

                SpecimenBatchManager.ConfirmNextPriorityBatch(this, batch);

                MasterCategoryPermissions BatchPerm = batch.EnsureGetPermissionsByUser(assignedBy);
                
                User assignedTo;
                switch (this.WorkflowAssignmentMode)
                {
                    case Batches.WorkflowAssignmentMode.Self:
                        if (BatchPerm.CanCheckOut == false)
                        { throw new InvalidOperationException("You do not have permission to check out bins of specified category"); }
                        assignedTo = assignedBy;
                        break;
                    case Batches.WorkflowAssignmentMode.Choice:
                        if (BatchPerm.CanAssign == false)
                        { throw new InvalidOperationException("You do not have permission to assign bins of specified category"); }
                        assignedTo = UserPickerDialog.SelectEmployee(this, "Select a user");
                        break;
                    default:
                        goto case WorkflowAssignmentMode.Self;
                }

                SpecimenBatchManager.AssignRegistration(batch, assignedBy, assignedTo);
                BmSqlServerXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id, batch.BinId, "True");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnCompleteRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                User activeUser = CredentialHost.EnsureGetSelectedUser();
                SpecimenBatch Batch = this.EnsureGetActiveBatch();
                if (Batch.Registration.HasStarted == false)
                { throw new InvalidOperationException("The registration step has not been started on the specified Batch"); }
                if (Batch.Registration.HasCompleted)
                { throw new InvalidOperationException("The registration step has already been completed on the specified Batch"); }
                
                MasterCategoryPermissions BatchPerm = Batch.EnsureGetPermissionsByUser(activeUser);
                if (BatchPerm.CanCheckIn == false)
                { throw new InvalidOperationException("You do not have permission to check in bins of the specified category"); }
                BmSqlServerXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(Batch.Id,Batch.BinId,"True");
                SpecimenBatchManager.CompleteRegistration(Batch, activeUser);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnBeginProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                User assignedBy = this.EnsureGetSelectedUser();
                SpecimenBatch batch = this.EnsureGetActiveBatch();

                SpecimenBatchManager.ConfirmNextPriorityBatch(this, batch);

                if (batch.Processing.HasStarted)
                { throw new InvalidOperationException("The processing step on the specified bin has already been started"); }
                MasterCategoryPermissions BatchPerm = batch.EnsureGetPermissionsByUser(assignedBy);

                User assignedTo;
                switch (this.WorkflowAssignmentMode)
                {
                    case Batches.WorkflowAssignmentMode.Self:
                        if (BatchPerm.CanCheckOut == false)
                        { throw new InvalidOperationException("You do not have permission to check out bins of specified category"); }
                        assignedTo = assignedBy;
                        break;
                    case Batches.WorkflowAssignmentMode.Choice:
                        if (BatchPerm.CanAssign == false)
                        { throw new InvalidOperationException("You do not have permission to assign bins of specified category"); }
                        assignedTo = UserPickerDialog.SelectEmployee(this, "Select a user");
                        break;
                    default:
                        goto case WorkflowAssignmentMode.Self;
                }
                if (batch.Registration.HasStarted == false)
                {
                    if (MessageBox.Show(this, "Would you like to begin the registration step as well?", "Registration not started", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    { SpecimenBatchManager.AssignRegistration(batch, assignedBy, assignedTo); }
                }
                
                SpecimenBatchManager.AssignProcessing(batch, assignedBy, assignedTo);
               // AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(batch.Id);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnCompleteProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                User completedBy = CredentialHost.EnsureGetSelectedUser();
                SpecimenBatch Batch = this.EnsureGetActiveBatch();
                if (Batch.Processing.HasStarted == false)
                { throw new InvalidOperationException("The processing step has not been started on the specified Batch"); }
                if (Batch.Processing.HasCompleted)
                { throw new InvalidOperationException("The processing step has already been completed on the specified Batch"); }

                MasterCategoryPermissions BatchPerm = Batch.EnsureGetPermissionsByUser(completedBy);
                if (BatchPerm.CanCheckIn == false)
                { throw new InvalidOperationException("You do not have permission to check in bins of the specified category"); }

                SpecimenBatchManager.CompleteProcessing(Batch, completedBy);
               // AddUpDateBmXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(Batch.Id);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                string message = ("Are you sure you want to close this batch (this will remove the batch and clear the bin)?");
                if (MessageBox.Show(this, message, "Confirm Close Batch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                { return; }
                User closedBy = this.EnsureGetSelectedUser();
                SpecimenBatch Batch = this.EnsureGetActiveBatch();

                Batch.IsClosed = true;
                Batch.ClosedBy = closedBy.DisplayName;
                Batch.ClosedAt = DateTime.Now;
                SpecimenBatchManager.FinalizeBatch(Batch, closedBy, true);
                BmSqlServerXmlFiles.AddBatchesCloud.UploadSpecimenBatchesXmlFile(Batch.Id,Batch.BinId,SqlCommands.SqlCmd.ProcessingBatchFalse);
                BmSqlServerXmlFiles.AddBatchesCloud.UpDateBinXmlFile(Batch.BinId);


                /*
                if (Batch.IsClosed)
                { throw new InvalidOperationException("The specified bin has already been closed"); }

                MasterCategoryPermissions BatchPerm = Batch.EnsureGetPermissionsByUser(closedBy);
                //Ensure the user is allowed to close the specified bin.
                if (BatchPerm.CanClose == false)
                { throw new InvalidOperationException("You do not have permission to close the specified bin"); }

                //Ensure all steps have been started
                if (Batch.Registration.HasStarted == false)
                { throw new Exception("The Registration step has not been started, cannot continue"); }
                if (Batch.Processing.HasStarted == false)
                { throw new InvalidOperationException("The Processing step has not been started, cannont continue"); }

                //If any steps are pending, make sure the user has permission to finish them, and do so
                bool allStepsCompleted = Batch.Registration.HasCompleted && Batch.Processing.HasCompleted;
                if (allStepsCompleted == false)
                {
                    if (BatchPerm.CanCheckIn == false)
                    { throw new InvalidOperationException("One or more steps have not been completed and you do not have permission to complete steps for the specified bin."); }

                    if (MessageBox.Show(this, "One or more steps have not been completed, would you like to complete them now?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    { throw new OperationCanceledException(); }

                    if (Batch.Registration.HasCompleted == false)
                    { SpecimenBatchManager.CompleteRegistration(Batch, closedBy); }
                    if (Batch.Processing.HasCompleted == false)
                    { SpecimenBatchManager.CompleteProcessing(Batch, closedBy); }
                }
                */
                //Close the bin


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
