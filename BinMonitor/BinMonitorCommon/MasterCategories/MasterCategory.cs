using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public class MasterCategory : INotifyPropertyChanged
    {
        #region Properties

        private string _Title = null;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }

        #endregion Events

        #region Event Handlers

        protected virtual void OnPropertyChanged(string propertyName)
        { this.NotifyPropertyChanged(propertyName); }

        #endregion Event Handlers
    }

   
}
