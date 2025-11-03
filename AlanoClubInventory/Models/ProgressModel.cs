using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    
        public class ProgressModel : INotifyPropertyChanged
        {
            private double progressValue;
            public double ProgressValue
            {
                get => progressValue;
                set
                {
                    if (progressValue != value)
                    {
                        progressValue = value;
                        OnPropertyChanged(nameof(ProgressValue));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
     
}
