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
    public partial class UserManagerControl : UserControl
    {
        private IUserSource _CredentialHost = null;
        public IUserSource CredentialHost
        {
            get { return _CredentialHost; }
            set { _CredentialHost = value; }
        }

        public UserManagerControl()
        {
            InitializeComponent();
            //cmbExistingUsers.Source = Users.Instance;
            cmbNewUserProfile.Source = UserProfiles.Instance;
            cmbSelectedUserProfile.Source = UserProfiles.Instance;
            //RefreshUserList();
            //RefreshUserProfileList();
            //Users.Instance.CollectionChanged += UserManager_Updated;
            //UserProfiles.Instance.CollectionChanged += UserProfileManager_Updated;
        }

        /*
        protected void RefreshUserProfileList()
        {
            cmbProfile.SelectedIndex = -1;
            cmbProfile.DataSource = UserProfiles.Instance.Values.ToArray();
            Debug.WriteLine("Refreshed, selected " + cmbProfile.SelectedIndex);
        }

        protected void RefreshUserList()
        {
            lbAllUsers.DataSource = Users.Instance.Values.ToArray();
            lbAllUsers.SelectedIndex = -1;
        }*/

        protected void ClearSelectedUserDetails()
        {
            Debug.WriteLine("Clearing perm");
            //cmbProfile.Clear = -1;
        }

        protected bool ValidateNewUserInput()
        {
            bool errors = false;
            NewUserErrorProvider.Clear();

            string newUserId = txtNewUserId.Text;
            if (string.IsNullOrWhiteSpace(newUserId))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtNewUserId, "Value required");
            }

            string newUserFirstName = txtNewUserFirstName.Text;
            if (string.IsNullOrWhiteSpace(newUserFirstName))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtNewUserFirstName, "Value required");
            }

            string newUserLastName = txtNewUserLastName.Text;
            if (string.IsNullOrWhiteSpace(newUserLastName))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtNewUserLastName, "Value required");
            }

            string newUserDisplayName = User.EncodeDisplayName(newUserFirstName, newUserLastName);
            if (Users.Instance.ContainsKey(newUserDisplayName))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtNewUserLastName, "User already exists");
                NewUserErrorProvider.SetError(txtNewUserFirstName, "User already exists");
            }

            string newUserCardId = txtNewUserCardId.Text;
            if (string.IsNullOrWhiteSpace(newUserCardId))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtNewUserCardId, "Value required");
            }
            string userEmailAddress = txtEmailAddress.Text;
            if (string.IsNullOrWhiteSpace(userEmailAddress))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtEmailAddress, "Value required");
            }

            if (!(BinUtilities.BinMointorUtilties.CheckEmailAddress(userEmailAddress)))
            {
                errors = true;
                NewUserErrorProvider.SetError(txtEmailAddress, "Invalid email address");
            }

            UserProfile newUserProfile = cmbNewUserProfile.SelectedValue;
            if (newUserProfile == null)
            {
                errors = true;
                NewUserErrorProvider.SetError(cmbNewUserProfile, "Value required");
            }

            return errors == false;
        }

        protected void AddNewUser()
        {
            try
            {
                if (CredentialHost.EnsureGetSelectedUser().EnsureGetUserProfile().IsAdmin == false)
                { throw new SecurityException(); }

                if (ValidateNewUserInput() == false)
                { return; }
                
                string userId = txtNewUserId.Text;
                string firstName = txtNewUserFirstName.Text;
                string lastName = txtNewUserLastName.Text;
                string profileId = cmbNewUserProfile.EnsureGetSelectedKey();
                string cardId = txtNewUserCardId.Text;
                string emailAddress = txtEmailAddress.Text;

                User user = new User()
                {
                    Id = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    UserProfileId = profileId,
                    CardId = cardId,
                    EmailAddress = emailAddress
                };

                Users.Instance.Add(user.DisplayName, user);
                BmSqlServerXmlFiles.AddBatchesCloud.AddUpDateBmUsersXmlFiles(user.DisplayName,user.Id);
                BinMonitorSqlServer.SqlServerInstance.AddUpdateUsers(user);
                cmbExistingUsers.EnsureSelectKey(user.DisplayName);

                txtNewUserId.Clear();
                txtNewUserFirstName.Clear();
                txtNewUserLastName.Clear();
                cmbNewUserProfile.ClearSelection();
                txtSelectedUserCardId.Clear();
            txtEmailAddress.Clear();
            
                MessageBox.Show(this,"User Added");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            AddNewUser();
            
        }

        protected bool ValidateUpdateUserInput()
        {
            
            ExistingUserErrorProvider.Clear();
            bool hasErrors = false;

            if (cmbExistingUsers.HasSelectedKey == false)
            {
                ExistingUserErrorProvider.SetError(cmbExistingUsers, "Selection required");
                return false;
            }

            User selectedUser = cmbExistingUsers.EnsureGetSelectedValue();

            if (cmbSelectedUserProfile.HasSelectedKey == false)
            {
                ExistingUserErrorProvider.SetError(cmbSelectedUserProfile, "Value required");
                hasErrors = true;
            }

            string newCardId = txtSelectedUserCardId.Text;
            if (string.IsNullOrWhiteSpace(newCardId))
            {
                hasErrors = true;
                ExistingUserErrorProvider.SetError(txtSelectedUserCardId, "Value required");
            }
            
            return hasErrors == false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {            
            try
            {
                if (CredentialHost.EnsureGetSelectedUser().EnsureGetUserProfile().IsAdmin == false)
                { throw new SecurityException(); }

                if (ValidateUpdateUserInput() == false)
                { return; }

                User user = cmbExistingUsers.EnsureGetSelectedValue();
                user.UserProfileId = cmbSelectedUserProfile.EnsureGetSelectedKey();
                user.CardId = txtSelectedUserCardId.Text;
                if(user.FirstName.ToLower() == SqlCommands.SqlCmd.EdocsUserFN)
                {
                    MessageBox.Show(string.Format("User:{0} cannot be updated.", user.FirstName));
                    return;
                }
                Users.Instance.Save(user.DisplayName);
                BmSqlServerXmlFiles.AddBatchesCloud.AddUpDateBmUsersXmlFiles(user.DisplayName,user.Id);
                BinMonitorSqlServer.SqlServerInstance.AddUpdateUsers(user);
                MessageBox.Show(this, "User updated");
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

                User user = cmbExistingUsers.EnsureGetSelectedValue();
                Users.Instance.EnsureDelete(user.DisplayName);
               
                if (user.FirstName.ToLower() == SqlCommands.SqlCmd.EdocsUserFN)
                {
                    MessageBox.Show(string.Format("User:{0} cannot be deleted.", user.FirstName));
                    return;
                }
                Users.Instance.EnsureDelete(user.DisplayName);
                BinMonitorSqlServer.SqlServerInstance.DeleteUser(user.Id, user.DisplayName);
                MessageBox.Show(this, "User Deleted");
                
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }
     
        private void txtNewUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { AddNewUser(); }
        }

        protected void OnSelectedUserChanged(User selectedUser)
        {
            if (selectedUser == null)
            {
                cmbSelectedUserProfile.ClearSelection();
            }
            else
            {
                cmbSelectedUserProfile.TrySelectKey(selectedUser.UserProfileId);
                txtSelectedUserCardId.Text = selectedUser.CardId;
            }
        }

        private void cmbExistingUsers_SelectedUserChanged(object sender, SelectedUserChangedEventArgs e)
        {
            OnSelectedUserChanged(e.SelectedUser);
        }

        private void btnRequestCardId_Click(object sender, EventArgs e)
        {
            try
            {
                string uid = SmartCardReaderDialog.PromptForUid(this);
                uid = User.EncodeSmartCardUid(uid);
                txtSelectedUserCardId.Text = uid;
            }
            catch (Exception ex)
            {MessageBox.Show(ex.Message);}
        }

        

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    if (CredentialHost.EnsureGetSelectedUser().EnsureGetUserProfile().IsAdmin == false)
                    { throw new SecurityException(); }

                    User user = cmbExistingUsers.EnsureGetSelectedValue();
                   
                    if (user.FirstName.ToLower() == SqlCommands.SqlCmd.EdocsUserFN)
                    {
                        MessageBox.Show(string.Format("User:{0} cannot be deleted.", user.FirstName));
                        return;
                    }
                    Users.Instance.EnsureDelete(user.DisplayName);
                    BinMonitorSqlServer.SqlServerInstance.DeleteUser(user.Id, user.DisplayName);
                    MessageBox.Show(this, "User Deleted");

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
}
