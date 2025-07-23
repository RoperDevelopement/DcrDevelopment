using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Polenter.Serialization;
using System.Security;

namespace BinMonitor.Common
{
   

    [Serializable]
    public class UserProfile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event EventHandler PermissionsChanged;
        protected void NotifyPermissionsChanged()
        {
            EventHandler handler = this.PermissionsChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        private string _Id = null;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private Dictionary<string, MasterCategoryPermissions> _CategoryPermissions = new Dictionary<string, MasterCategoryPermissions>();
        public Dictionary<string, MasterCategoryPermissions> CategoryPermissions
        {
            get { return _CategoryPermissions; }
            set { _CategoryPermissions = value; }
        }

        [ExcludeFromSerialization]
        public MasterCategoryPermissions ProblemBinPermissions
        {
            get 
            {
                MasterCategoryPermissions value;
                CategoryPermissions.TryGetValue(MasterCategories.PROBLEM_TITLE, out value);
                return value;
            }
            set
            {
                
                if(ProblemBinPermissions != null)
                { ProblemBinPermissions.PropertyChanged -= OnMasterCategoryPermissions_PropertyChanged; }

                value.PropertyChanged += OnMasterCategoryPermissions_PropertyChanged;
                CategoryPermissions[MasterCategories.PROBLEM_TITLE] = value;
            }
        }

        [ExcludeFromSerialization]
        public MasterCategoryPermissions RoutineBinPermissions
        {
            get 
            {
                MasterCategoryPermissions value;
                CategoryPermissions.TryGetValue(MasterCategories.ROUTINE_TITLE, out value);
                return value;
            }
            set
            {

                if (RoutineBinPermissions != null)
                { RoutineBinPermissions.PropertyChanged -= OnMasterCategoryPermissions_PropertyChanged; }

                value.PropertyChanged += OnMasterCategoryPermissions_PropertyChanged;
                CategoryPermissions[MasterCategories.ROUTINE_TITLE] = value;
            }
        }

        [ExcludeFromSerialization]
        public MasterCategoryPermissions StatBinPermissions
        {
            get 
            { 
                MasterCategoryPermissions value;
                CategoryPermissions.TryGetValue(MasterCategories.STAT_TITLE, out value);
                return value;
            }
            set
            {
                if(StatBinPermissions != null)
                { StatBinPermissions.PropertyChanged -= OnMasterCategoryPermissions_PropertyChanged; }

                value.PropertyChanged += OnMasterCategoryPermissions_PropertyChanged;
                CategoryPermissions[MasterCategories.STAT_TITLE] = value;
            }
        }

        [ExcludeFromSerialization]
        public MasterCategoryPermissions ReadyBinPermissions
        {
            get 
            {
                MasterCategoryPermissions value;
                CategoryPermissions.TryGetValue(MasterCategories.READY_TITLE, out value);
                return value;
            }
            set
            {
                if (ReadyBinPermissions != null)
                { ReadyBinPermissions.PropertyChanged -= OnMasterCategoryPermissions_PropertyChanged; }

                value.PropertyChanged += OnMasterCategoryPermissions_PropertyChanged;
                CategoryPermissions[MasterCategories.READY_TITLE] = value;
            }
        }

        protected virtual void OnMasterCategoryPermissions_PropertyChanged()
        { NotifyPermissionsChanged(); }

        protected virtual void OnMasterCategoryPermissions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnMasterCategoryPermissions_PropertyChanged();}

      
        private bool _IsAdmin = false;
        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set
            {
                _IsAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
    }

    public sealed class UserProfiles : SerializedObjectDictionary<UserProfile>
    {
        public override string DirectoryPath
        {
            get { return @"Config\UserProfiles"; }
        }

        static readonly UserProfiles _Instance = new UserProfiles();
        public static UserProfiles Instance
        { get { return _Instance; } }
    }
    /*
    public sealed class UserProfileManager
    {
        static readonly UserProfileManager _Instance = new UserProfileManager();
        public static UserProfileManager Instance
        {
            get { return _Instance; }
        }

        public void AddNewProfile(User addedBy, UserProfile profile)
        {
            if (addedBy.EnsureGetUserProfile().IsAdmin == false)
            { throw new SecurityException(); }

            UserProfiles.Instance.Add(profile.Id, profile);
        }

        public bool TryRemoveProfile(User removedBy, UserProfile profile)
        {
            if (removedBy.EnsureGetUserProfile().IsAdmin == false)
            { throw new SecurityException(); }

            return UserProfiles.Instance.TryRemove(profile.Id);
        }

        public UserProfile BeginUpdate()
        { }

        public void UpdateCategoryPermissions(User updatedBy, UserProfile profile, string category, MasterCategoryPermissions permissions)
        {
            if (updatedBy.EnsureGetUserProfile().IsAdmin == false)
            { throw new SecurityException(); }

            profile.CategoryPermissions[category] = permissions;
        }
    }
     * */
}
