using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using MobileEdocsTracking.ViewModels;
namespace MobileEdocsTracking.ViewModels
{
  public  class LoginViewModel : INotifyPropertyChanged
    {

        private string _username;
        private string _password;
        private bool _areCredentialsInvalid;
        public event PropertyChangedEventHandler PropertyChanged;
      
        public LoginViewModel()
        {
            AuthenticateCommand = new Command(() =>
            {
                AreCredentialsInvalid = UserAuthenticated(Username, Password);
                     if (AreCredentialsInvalid)
                { App.IsUserLoggedIn = true;
                    //
                    //Application.Current.MainPage = new MainPage();
                    AreCredentialsInvalid = true;
                    return;

                   
                }

                
                
            });

            AreCredentialsInvalid = false;

        }
        private bool UserAuthenticated(string username, string password)
        {
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
            {
                return false;
            }
            return true;
            //return username.ToLowerInvariant() == "joe"
            //    && password.ToLowerInvariant() == "secret";
        }


        public ICommand AuthenticateCommand { get; set; }
        public bool AreCredentialsInvalid
        {
            get => _areCredentialsInvalid;
            set
            {
                if (value == _areCredentialsInvalid) return;
                _areCredentialsInvalid = value;
                OnPropertyChanged(nameof(AreCredentialsInvalid));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

    }
}
