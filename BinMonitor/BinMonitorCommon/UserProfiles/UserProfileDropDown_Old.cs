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
    public partial class UserProfileDropDown_Old : UserControl
    {
        public event EventHandler SelectionChanged;
        protected void NotifySelectionChanged()
        {
            EventHandler handler = this.SelectionChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected void OnSelectionChanged()
        { NotifySelectionChanged(); }

        protected void OnSelectionChanged(object sender, EventArgs e)
        { OnSelectionChanged(); }

        public UserProfileDropDown_Old()
        { InitializeComponent(); }

        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);
            if (DesignMode == false)
            {
                UserProfiles.Instance.CollectionChanged += OnUserProfiles_CollectionChanged;
                OnUserProfiles_CollectionChanged();
            }
            cmbUserProfiles.SelectedIndexChanged += OnSelectionChanged;
        }

        public string SelectedUserProfileId
        { 
            get  { return (string)(cmbUserProfiles.SelectedValue);}
            set
            { cmbUserProfiles.Text = value; }
        }

        public bool TrySelectUserProfile(string userProfileId)
        {
            if (string.IsNullOrWhiteSpace(userProfileId))
            {
                ClearSelection();
                return true;
            }
            if (UserProfiles.Instance.ContainsKey(userProfileId) == false)
            {
                Trace.TraceInformation("Specified user profile does not exist");
                return false; 
            }
            cmbUserProfiles.Text = userProfileId;
            return ((string)(cmbUserProfiles.SelectedItem)).Equals(userProfileId, StringComparison.OrdinalIgnoreCase);
        }

        public void EnsureSelectUserProfile(string userProfileId)
        {
            if (TrySelectUserProfile(userProfileId) == false)
            { throw new InvalidOperationException("Unable to select " + userProfileId); }
        }

        public UserProfile SelectedUserProfile
        {
            get 
            {
                string selectedUserProfileId = this.SelectedUserProfileId;
                if (string.IsNullOrEmpty(selectedUserProfileId))
                {return null;}
                
                UserProfile selectedUserProfile;
                if (UserProfiles.Instance.TryGetValue(selectedUserProfileId, out selectedUserProfile) == false)
                {return null;}
                return selectedUserProfile;
            }
        }

        public UserProfile EnsureGetSelectedUserProfile()
        {
            UserProfile selectedUserProfile = this.SelectedUserProfile;
            if (selectedUserProfile == null)
            {throw new InvalidOperationException("The requested operation requires a selected user profile");}
            return selectedUserProfile;
        }

        protected virtual void OnUserProfiles_CollectionChanged()
        {
            cmbUserProfiles.SelectedIndexChanged -= OnSelectionChanged;
            try
            {
                string previousSelection = SelectedUserProfileId;
                cmbUserProfiles.DataSource = null;
                cmbUserProfiles.SelectedIndex = -1;
                cmbUserProfiles.DataSource = UserProfiles.Instance.Keys.OrderBy(key=>key).ToArray();
                
                if (string.IsNullOrWhiteSpace(previousSelection))
                { ClearSelection(); }
                else if (UserProfiles.Instance.ContainsKey(previousSelection) == false)
                { ClearSelection(); }
                else
                { cmbUserProfiles.Text = previousSelection; }
            }
            finally
            { cmbUserProfiles.SelectedIndexChanged += OnSelectionChanged; }
        }

        protected virtual void OnUserProfiles_CollectionChanged(object sender, EventArgs e)
        { OnUserProfiles_CollectionChanged();}

        protected void ClearSelection()
        { cmbUserProfiles.SelectedIndex = -1; }
    }
}