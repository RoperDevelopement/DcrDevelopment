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
    public partial class CreateBatchControl : UserControl
    {
        private List<String> specList = new List<String>();
        private List<String> specList2 = new List<String>();
        private bool _AdvancedOptionsVisible = true;
        /// <summary>Show/Hide the workflow and checkpoint configuration options.</summary>
        public bool AdvancedOptionsVisible
        {
            get { return _AdvancedOptionsVisible; }
            set
            {
                grpCheckpoints.Visible = value;
                grpWorkflow.Visible = value;
                _AdvancedOptionsVisible = value;
            }
        }

        public IUserSource _CredentialHost  = null;
        public IUserSource CredentialHost
        {
            get { return _CredentialHost; }
            set { _CredentialHost = value; }
        }

        /// <summary>Event on completion of a new batch</summary>
        public event EventHandler BatchCreated;

        public CreateBatchControl()
        {
            InitializeComponent();
            if (_CredentialHost != null)
            {
                if (!CredentialHost.SelectedUser.UserProfileId.Equals("ADMIN") && !CredentialHost.SelectedUser.UserProfileId.Equals("BIN MANAGER"))
                {
                    MessageBox.Show("User");
                    grpWorkflow.Enabled= false;

                }
                else
                {
                    grpWorkflow.Enabled = true;
                }
            }
            if (DesignMode == false)
            {
                cmbCategories.Source = SpecimenCategories.Instance;
                cmbUsersForProcessing.Source = Users.Instance;
                cmbUsersForRegistration.Source = Users.Instance;
            }
            Clear();
        }

        private void btnAddSpecimenToBatch_Click(object sender, EventArgs e)
        {
            AddSpecimenToBatch();
        }

        protected void AddSpecimenToBatch()
        {
            //Added time stamps 2/22 per Raj
            if (string.IsNullOrWhiteSpace(txtAddSpecimenToBatch.Text))
            { return; }
            Bin[] bins = Bins.Instance.GetBinsContainingSpecimen(txtAddSpecimenToBatch.Text).ToArray();
            //If it's a duplicate, warn the user and only add if requested.
            String timeStamp = GetTimestamp(DateTime.Now);
            String specWithTimeStamp = String.Concat(txtAddSpecimenToBatch.Text, " ", timeStamp);
            if (specList.Contains(txtAddSpecimenToBatch.Text))
            {
                //Play an audio alert in case they aren't looking
                System.Media.SystemSounds.Exclamation.Play();
                
                MessageBox.Show(this, "Specimen already added.");
                txtAddSpecimenToBatch.Clear();
                txtAddSpecimenToBatch.Focus();
            }

            else if (bins.Length > 0)
            {
                
                MessageBox.Show(this, "Specimen already in Bin: " + bins[0].Id);
                txtAddSpecimenToBatch.Clear();
                txtAddSpecimenToBatch.Focus();
                /*
                String timeStamp = GetTimestamp(DateTime.Now);         
                if (MessageBox.Show(this, "Specimen already in an active Bin, if this was intended, select ok, otherwise, select cancel", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    String test = String.Concat(txtAddSpecimenToBatch.Text, " ",timeStamp);
                    lbBatchContents.Items.Add(test);
                }*/
            }
            else // Not a duplicate, just add it.
            {
                lbBatchContents.Items.Add(specWithTimeStamp);

                specList.Add(txtAddSpecimenToBatch.Text);
                specList2.Add(specWithTimeStamp);
                txtAddSpecimenToBatch.Clear();
                txtAddSpecimenToBatch.Focus();
            }
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM-dd-yyy HH:mm:ss");
        }
        private void txtAddSpecimenToBatch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { AddSpecimenToBatch(); }
        }

        public void LoadFromTransfer(SpecimenBatch origin, List<string> Specimens)
        {
           

        }

        public void Clear()
        {
            txtBinId.Clear();
            txtTransferredFromBatchId.Clear();
            txtComments.Clear();
            lbBatchContents.ClearSelected();
            lbBatchContents.Items.Clear();
            cmbCategories.ClearSelection();
            chkRequiresRegistration.Checked = false;
            cmbUsersForRegistration.ClearSelection();
            cmbUsersForProcessing.ClearSelection();
            dtpCheckpointOrigin.Value = DateTime.Now;
            checkpointConfigurationControl1.Clear();
            checkpointConfigurationControl2.Clear();
            checkpointConfigurationControl3.Clear();
            checkpointConfigurationControl4.Clear();
            wfcfgCreate.Clear();
            wfcfgRegister.Clear();
            wfcfgProcess.Clear();

            specList.Clear();
            specList2.Clear();
            txtBinId.Focus();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        { CreateBatch(); }
        
        private bool ValidateInput()
        {
            bool errors = false;
            ErrorProvider.Clear();

            User activeUser = CredentialHost.SelectedUser;
            if (activeUser == null)
            { 
                ErrorProvider.SetError(this, "No active user found");
                return false;
            }

            string binId = txtBinId.Text;
            if (string.IsNullOrWhiteSpace(binId))
            {
                ErrorProvider.SetError(txtBinId, "Value required");
                errors = true;
            }
            else
            {
                binId = Bin.NormalizeId(binId);
                if (Bins.Instance.ContainsKey(binId) == false)
                {
                    ErrorProvider.SetError(txtBinId, "The specified bin does not exist");
                    errors = true;
                }
                else if (Bins.Instance.EnsureGetValue(binId).HasBatch)
                {
                    ErrorProvider.SetError(txtBinId, "Specified bin is already in use");
                    errors = true;
                }
            }

            if (cmbCategories.HasSelectedKey == false)
            {
                ErrorProvider.SetError(cmbCategories, "Value required");
                errors = true;
            }
            else
            {
                Category category = cmbCategories.EnsureGetSelectedValue();
                MasterCategoryPermissions categoryPermissions = activeUser.UserProfile.CategoryPermissions[category.MasterCategoryTitle];
                if (categoryPermissions.CanCreate == false)
                {
                    ErrorProvider.SetError(cmbCategories, "You do not have permission to create bins for the selected category");
                    errors = true;
                }

                if (chkRequiresRegistration.Checked == true)
                {
                    if (categoryPermissions.CanAssign == false)
                    {
                        ErrorProvider.SetError(chkRequiresRegistration, "You do not have permission to assign registration for the selected category.");
                        errors = true;
                    }
                    if (categoryPermissions.CanCheckIn == false)
                    {
                        ErrorProvider.SetError(chkRequiresRegistration, "You do not have permission to complete registration for the selected category.");
                        errors = true;
                    }
                }

                if (cmbUsersForRegistration.HasSelectedKey)
                {
                    if (categoryPermissions.CanAssign == false)
                    {
                        ErrorProvider.SetError(cmbUsersForRegistration, "You do not have permission to assign registration for the selected category.");
                        errors = true;
                    }
                }

                if (cmbUsersForProcessing.HasSelectedKey)
                {
                    if (categoryPermissions.CanAssign == false)
                    {
                        ErrorProvider.SetError(cmbUsersForProcessing, "You do not have permission to assign processing for the selected category.");
                        errors = true;
                    }
                }

                if (string.IsNullOrWhiteSpace(txtComments.Text) == false)
                {
                    if (categoryPermissions.CanAddComment == false)
                    {
                        ErrorProvider.SetError(txtComments, "You do not have permission to add comments for the selected category");
                        errors = true;
                    }
                }

                
                if (string.IsNullOrWhiteSpace(txtTransferredFromBatchId.Text) == false)
                {
                    if (string.IsNullOrWhiteSpace(txtComments.Text))
                    {
                        ErrorProvider.SetError(txtComments, "Value required when transferring");
                        errors = true;
                    }
                }

                if (checkpointConfigurationControl1.Validate() == false)
                { errors = true; }
                if (checkpointConfigurationControl2.Validate() == false)
                { errors = true; }
                if (checkpointConfigurationControl3.Validate() == false)
                { errors = true; }
                if (checkpointConfigurationControl4.Validate() == false)
                { errors = true; }
                if (wfcfgCreate.Validate() == false)
                { errors = true; }
                if (wfcfgRegister.Validate() == false)
                { errors = true; }
                if (wfcfgProcess.Validate() == false)
                { errors = true; }
            }
            return errors == false;
        }

        void CreateBatch()
        {
            try
            {
                if (ValidateInput() == false)
                {
                    MessageBox.Show("One or more values is invalid, please review your input and try again");
                    return;
                }

                string binId = txtBinId.Text;
                binId = Bin.NormalizeId(binId);
               
                User activeUser = CredentialHost.EnsureGetSelectedUser();
                
                DateTime startTime = DateTime.Now;

                SpecimenBatch Batch = new SpecimenBatch(activeUser.Id);
                Batch.Id = SpecimenBatch.GenerateNewId();
                Batch.BinId = binId;

                Batch.CategoryId = cmbCategories.EnsureGetSelectedKey();
                Batch.TransferredFrom = txtTransferredFromBatchId.Text;
                if (string.IsNullOrWhiteSpace(txtComments.Text) == false)
                { Batch.AddComment(txtComments.Text, activeUser.Id, startTime); }
                foreach (object o in lbBatchContents.Items)
                { Batch.Specimens.Add((string)o); }

                DateTime checkpointOrigin = startTime;
                if (dtpCheckpointOrigin.Value != DateTimePicker.MinimumDateTime)
                { checkpointOrigin = dtpCheckpointOrigin.Value; }

                 
                Batch.CheckPoint1 = new CheckPoint(checkpointConfigurationControl1.Value, checkpointOrigin);
                Batch.CheckPoint2 = new CheckPoint(checkpointConfigurationControl2.Value, checkpointOrigin);
                Batch.CheckPoint3 = new CheckPoint(checkpointConfigurationControl3.Value, checkpointOrigin);
                Batch.CheckPoint4 = new CheckPoint(checkpointConfigurationControl4.Value, checkpointOrigin);
                Batch.CheckpointOrigin = checkpointOrigin;

              
                
                Batch.Registration = new WorkflowStep(wfcfgRegister.Value);

                if (chkRequiresRegistration.Checked == false)
                {
                    Batch.Registration.Start(activeUser.Id, activeUser.Id, startTime);
                    Batch.Registration.Complete(activeUser.Id, startTime);
                }
                else
                {
                    User regUser = cmbUsersForRegistration.SelectedValue;
                    if (regUser != null)
                    { Batch.Registration.Start(activeUser.Id, regUser.Id); }
                }

                Batch.Processing = new WorkflowStep(wfcfgProcess.Value);
                User procUser = cmbUsersForProcessing.SelectedValue;
                if (procUser != null)
                { Batch.Processing.Start(activeUser.Id, procUser.Id, startTime); }
                //AddBatches.AddBatchesCloud.AddNewCategory(Batch);
                SpecimenBatches.Instance.Add(Batch.Id, Batch);
                SpecimenBatchManager.AssignNewBatch(Batch, binId, activeUser);
                BmSqlServerXmlFiles.AddBatchesCloud.UpDateBinXmlFile(binId);


                if (!(string.IsNullOrWhiteSpace(wfcfgCreate.Value.EmailRecipients)))
                {
                    BinMonitorSqlServer.SqlServerInstance.SpecEmails(SqlCommands.SqlConstants.EmailSpecimsCreate, Batch.Id,wfcfgCreate.Value.EmailRecipients, wfcfgCreate.Value.EmailOnStart.ToString(), wfcfgCreate.Value.EmailOnCompletion.ToString(), wfcfgCreate.Value.IncludeContentsInEmail.ToString());

                }
                if (!(string.IsNullOrWhiteSpace(wfcfgProcess.Value.EmailRecipients)))
                {
                    BinMonitorSqlServer.SqlServerInstance.SpecEmails(SqlCommands.SqlConstants.EmailSpecimsProcess, Batch.Id, wfcfgProcess.Value.EmailRecipients, wfcfgProcess.Value.EmailOnStart.ToString(), wfcfgProcess.Value.EmailOnCompletion.ToString(), wfcfgProcess.Value.IncludeContentsInEmail.ToString());
                }

                if (!(string.IsNullOrWhiteSpace(wfcfgRegister.Value.EmailRecipients)))
                {
                    BinMonitorSqlServer.SqlServerInstance.SpecEmails(SqlCommands.SqlConstants.NoValue, Batch.Id, wfcfgRegister.Value.EmailRecipients, wfcfgRegister.Value.EmailOnStart.ToString(), wfcfgRegister.Value.EmailOnCompletion.ToString(), wfcfgRegister.Value.IncludeContentsInEmail.ToString());
                }
                base.Refresh();
                Clear();
                OnBatchCreated();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        protected void OnBatchCreated()
        {
            EventHandler handler = BatchCreated;
            if (handler != null)
            { handler(this, default(EventArgs)); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        BatchTransferDialog BatchTransferDialog = new BatchTransferDialog();

        private void btnTransferredFromLookup_Click(object sender, EventArgs e)
        {
            BatchTransferDialog.Clear();
            if (BatchTransferDialog.ShowDialog(this) == DialogResult.OK)
            {
                SpecimenBatch origin = BatchTransferDialog.OriginBatch;
                txtTransferredFromBatchId.Text = origin.Id;
                foreach (string Specimen in BatchTransferDialog.SpecimensToTransfer)
                { 
                    lbBatchContents.Items.Add(Specimen); 
                }

                cmbCategories.EnsureSelectKey(origin.Category.Title);
                dtpCheckpointOrigin.Value = origin.CreatedAt;
                checkpointConfigurationControl1.LoadFromExisting(origin.CheckPoint1.Configuration);
                checkpointConfigurationControl2.LoadFromExisting(origin.CheckPoint2.Configuration);
                checkpointConfigurationControl3.LoadFromExisting(origin.CheckPoint3.Configuration);
                checkpointConfigurationControl4.LoadFromExisting(origin.CheckPoint4.Configuration);
            }
        }

        private void btnRemoveSelectedSpecimens_Click(object sender, EventArgs e)
        {
            try
            {
                string activeSpecimen = (string)lbBatchContents.SelectedItem;
                if (string.IsNullOrWhiteSpace(activeSpecimen))
                { throw new InvalidOperationException("A Specimen must be selected"); }
                int temp = specList2.IndexOf((string)lbBatchContents.SelectedItem);
                lbBatchContents.Items.Remove(activeSpecimen);
                
                specList2.RemoveRange(temp, 1);
                specList.RemoveRange(temp, 1);
                //MessageBox.Show(""+temp);
                txtAddSpecimenToBatch.Clear();
                txtAddSpecimenToBatch.Focus();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void chkSkipRegistration_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiresRegistration.Checked == true)
            { cmbUsersForRegistration.Enabled = true; }
            else
            {
                cmbUsersForRegistration.ClearSelection();
                cmbUsersForRegistration.Enabled = false;
            }
        }

        private void cmbCategories_SelectedKeyChanged(object sender, EventArgs e)
        {
            Category category = cmbCategories.SelectedValue;
            if (_CredentialHost != null)
            {
                if (!CredentialHost.SelectedUser.UserProfileId.Equals("ADMIN") && !CredentialHost.SelectedUser.UserProfileId.Equals("BIN MANAGER"))
                {
                    grpWorkflow.Enabled = false;

                }
                else
                {
                    grpWorkflow.Enabled = true;
                }
            }
            if (category == null)
            {
                checkpointConfigurationControl1.Clear();
                checkpointConfigurationControl2.Clear();
                checkpointConfigurationControl3.Clear();
                checkpointConfigurationControl4.Clear();
                wfcfgCreate.Clear();
                wfcfgProcess.Clear();
                wfcfgRegister.Clear();
            }
            else
            {
                checkpointConfigurationControl1.LoadFromExisting(category.CheckPoint1Configuration);
                checkpointConfigurationControl2.LoadFromExisting(category.CheckPoint2Configuration);
                checkpointConfigurationControl3.LoadFromExisting(category.CheckPoint3Configuration);
                checkpointConfigurationControl4.LoadFromExisting(category.CheckPoint4Configuration);
                wfcfgCreate.LoadFromExisting(category.CreateConfiguration);
                wfcfgRegister.LoadFromExisting(category.RegisterConfiguration);
                wfcfgProcess.LoadFromExisting(category.ProcessConfiguration);
                chkRequiresRegistration.Checked = category.RequiresRegistration;
            }
        }
    }
}
