using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinMonitor.Common
{
    [Serializable]
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(propertyName)); }
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

        private string _FirstName = null;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
                OnPropertyChanged("DisplayName");
            }
        }

        private string _LastName = null;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
                OnPropertyChanged("DisplayName");
            }
        }

        private string _EmailAddress = null;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set
            {
                _EmailAddress = value;
                OnPropertyChanged("EmailAddress");
                
            }
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                { return Id; }
                else
                { return string.Format("{0}, {1}", LastName, FirstName); }
            }
        }

        private string _CardId = null;
        public string CardId
        {
            get { return _CardId;}
            set 
            {
                _CardId = value;
                OnPropertyChanged("CardId");
            }
            
        }

        private string _UserProfileId = null;
        public string UserProfileId
        {
            get { return _UserProfileId; }
            set
            {
                _UserProfileId = value;
                OnPropertyChanged("UserProfileId");
                OnPropertyChanged("UserProfile");
            }
        }

        public UserProfile UserProfile
        {
            get 
            {
                UserProfile profile;
                if (UserProfiles.Instance.TryGetValue(this.UserProfileId, out profile) == false)
                { return null; }
                else
                { return profile; }
            }
        }

        public UserProfile EnsureGetUserProfile()
        {
            UserProfile profile = this.UserProfile;
            if (profile == null)
            { throw new InvalidOperationException("The requested operation requires a valid user profile assigned"); }
            return profile;
        }

        public static string EncodeDisplayName(string firstName, string lastName)
        { return string.Format("{0}, {1}", lastName, firstName); }

        public static string EncodeSmartCardUid(string uid)
        {
            return Md5Util.ComputeHash(uid);
        }
    }
    

}
