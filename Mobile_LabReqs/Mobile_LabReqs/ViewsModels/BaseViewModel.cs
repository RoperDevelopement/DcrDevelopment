 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace   Mobile_LabReqs.ViewsModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
       

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        DateTime searchStartDate = DateTime.Now.AddDays(-10);
        public DateTime SearchStartDate
        {
            get { return searchStartDate; }
            set { SetProperty(ref searchStartDate, value); }
        }
        DateTime searchEndDate = DateTime.Now;
        public DateTime SearchEndDate
        {
            get { return searchEndDate; }
            set { SetProperty(ref searchEndDate, value); }
        }

        string searchStr = string.Empty ;
        public string SearchStr
        {
            get { return searchStr; }
            set { SetProperty(ref searchStr, value); }
        }
        bool searchPartial = false;
        public bool SearchPartial
        {
            get { return searchPartial; }
            set { SetProperty(ref searchPartial, value); }
        }
        bool searchScanDate = true;
        public bool SearchScanDate
        {
            get { return searchScanDate; }
            set { SetProperty(ref searchScanDate, value); }
        }

         bool searchDateofService = false;
        public   bool SearchDateofService
        { 
            get { return searchDateofService; }
    
            set { SetProperty(ref searchDateofService, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
