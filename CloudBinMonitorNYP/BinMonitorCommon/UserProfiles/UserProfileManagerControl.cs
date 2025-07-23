using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security;

namespace BinMonitor.Common
{
    public partial class UserProfileManagerControl : UserControl
    {
        private IUserSource _CredentialHost = null;
        public IUserSource CredentialHost
        {
            get { return _CredentialHost; }
            set { _CredentialHost = value; }
        }

        public UserProfileManagerControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode == false)
            { cmbExistingProfiles.Source = UserProfiles.Instance; }
        }

        protected void OnExistingProfiles_SelectionChanged()
        {
            UserProfile profile = cmbExistingProfiles.SelectedValue;
            if (profile == null)
            {
                chkIsAdmin.Checked = false;
                permProblem.Clear();
                permRoutine.Clear();
                permStat.Clear();
                permReady.Clear();
            }
            else
            {
                chkIsAdmin.Checked = profile.IsAdmin;
                permProblem.LoadFromExisting(profile.ProblemBinPermissions);
                permRoutine.LoadFromExisting(profile.RoutineBinPermissions);
                permStat.LoadFromExisting(profile.StatBinPermissions);
                permReady.LoadFromExisting(profile.ReadyBinPermissions);
            }
        }

        private void btnCreateNewProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (CredentialHost.EnsureGetSelectedUser().EnsureGetUserProfile().IsAdmin == false)
                { throw new SecurityException(); }
                string profileId = txtNewProfileName.Text;
                if (string.IsNullOrWhiteSpace(profileId))
                { throw new InvalidOperationException("New profile id is required"); }
                UserProfiles.Instance.EnsureKeyDoesNotExist(profileId);

                UserProfile profile = new UserProfile()
                { 
                    Id = profileId,
                    IsAdmin = chkIsAdmin.Checked,
                    ProblemBinPermissions = new MasterCategoryPermissions(),
                    RoutineBinPermissions = new MasterCategoryPermissions(),
                    StatBinPermissions = new MasterCategoryPermissions(),
                    ReadyBinPermissions = new MasterCategoryPermissions()
                };

                UserProfiles.Instance.Add(profile.Id, profile);
                BmSqlServerXmlFiles.AddBatchesCloud.XmlFileUserProfiles(profile.Id, SqlCommands.SqlConstants.ProcessingBatchFalse);
                cmbExistingProfiles.EnsureSelectKey(profileId);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (CredentialHost.EnsureGetSelectedUser().EnsureGetUserProfile().IsAdmin == false)
                { throw new SecurityException(); }
                UserProfile profile = cmbExistingProfiles.EnsureGetSelectedValue();
                profile.IsAdmin = chkIsAdmin.Checked;
                profile.ProblemBinPermissions = permProblem.EnsureGetValue();
                profile.RoutineBinPermissions = permRoutine.EnsureGetValue();
                profile.StatBinPermissions = permStat.EnsureGetValue();
                profile.ReadyBinPermissions = permReady.EnsureGetValue();
                UserProfiles.Instance.Save(profile.Id);
                BmSqlServerXmlFiles.AddBatchesCloud.XmlFileUserProfiles(profile.Id, SqlCommands.SqlConstants.ProcessingBatchFalse);
                //UserProfiles.Instance[profile.Id] = profile;
                MessageBox.Show("Profile updated");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CredentialHost.EnsureGetSelectedUser().EnsureGetUserProfile().IsAdmin == false)
                { throw new SecurityException(); }
                UserProfile profile = cmbExistingProfiles.EnsureGetSelectedValue();
                if (MessageBox.Show(this, "Are you sure you want to delete " + profile.Id, "Confirm Delete") != DialogResult.OK)
                { throw new OperationCanceledException(); }
                BmSqlServerXmlFiles.AddBatchesCloud.XmlFileUserProfiles(profile.Id, SqlCommands.SqlConstants.ProcessingBatchTrue);
                UserProfiles.Instance.EnsureDelete(profile.Id);
                MessageBox.Show(this, "Profile deleted", "Success");
                
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void cmbExistingProfiles_SelectedKeyChanged(object sender, EventArgs e)
        {
            OnExistingProfiles_SelectionChanged();
        }
    }
}
