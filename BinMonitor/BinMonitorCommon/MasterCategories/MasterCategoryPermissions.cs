using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    [Serializable]
    public class MasterCategoryPermissions : INotifyPropertyChanged
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

        private bool _CanCreate = false;
        public bool CanCreate
        {
            get { return _CanCreate; }
            set
            {
                _CanCreate = value;
                OnPropertyChanged("CanCreate");
            }
        }

        private bool _CanAddComment = false;
        public bool CanAddComment
        {
            get { return _CanAddComment; }
            set
            {
                _CanAddComment = value;
                OnPropertyChanged("CanAddComment");
            }
        }

        private bool _CanAssign = false;
        /// <summary>Can assign registration and processing steps to any user.</summary>
        public bool CanAssign
        {
            get { return _CanAssign; }
            set
            {
                _CanAssign = value;
                OnPropertyChanged("CanAssign");
            }
        }

        private bool _CanCheckOut = false;
        /// <summary>Can assign registration and processing steps to self.</summary>
        public bool CanCheckOut
        {
            get { return _CanCheckOut; }
            set 
            { 
                _CanCheckOut = value;
                OnPropertyChanged("CanCheckOut");
            }
        }

        private bool _CanCheckIn = false;
        /// <summary>Can complete registration and processing steps for any user.</summary>
        public bool CanCheckIn
        {
            get { return _CanCheckIn; }
            set
            {
                _CanCheckIn = value;
                OnPropertyChanged("CanCheckIn");
            }
        }

        private bool _CanClose = false;
        /// <summary>Can perform the final close bin.</summary>
        public bool CanClose
        {
            get { return _CanClose; }
            set
            {
                _CanClose = value;
                OnPropertyChanged("CanClose");
            }
        }
    }
}
