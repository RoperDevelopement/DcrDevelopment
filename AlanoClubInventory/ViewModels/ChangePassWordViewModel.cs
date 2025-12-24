using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Scmd = AlanoClubInventory.SqlServices;
namespace AlanoClubInventory.ViewModels
{
    public class ChangePassWordViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string userName;
        private string userPassword;
        private string userNewPassword;
        private string userEmail;
        private ICommand updatePassword;
        private string txtNewPassWord;
        private string sqlConn;
        public ChangePassWordViewModel()
        {
            UserEmail = LoginUserModel.LoginInstance.UserEmailAddress;
            Username = LoginUserModel.LoginInstance.UserName;
            TxtNewPassWord = $"New PassWord Must be at lest 8 characters with 1 number 1 upper case letter and contain 1 {Utilites.AlanoCLubConstProp.PasswordPattern}";

        }
        public bool NewPassWordRev { get; set; } = true;
        public bool VerifyPassWordRev { get; set; } = true;

        public ICommand UpdatePassword
        {
            get
            {
                if (updatePassword == null)
                {
                    updatePassword = new RelayCommd(param => ChangePassword(), param => CanChangePW());

                }
                return updatePassword;
            }
        }
        public string TxtNewPassWord
        {
            get => txtNewPassWord
;
            set
            {
                if (txtNewPassWord != value)
                {
                    txtNewPassWord = value;
                    OnPropertyChanged(nameof(TxtNewPassWord));
                }
            }

        }
        public string UserEmail
        {
            get => userEmail
;
            set
            {
                if (userEmail != value)
                {
                    userEmail = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }
        public string Username
        {
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public void GoBack()
        {
            var navWindow = Application.Current.MainWindow as NavigationWindow;
            if (navWindow != null && navWindow.CanGoBack)
            {
                //  Utilites.ALanoClubUtilites.IsLoggin = true;
                navWindow.GoBack();
                navWindow.RemoveBackEntry();
            }
        }
        public string UserPassword
        {
            get => userPassword;
            set
            {
                if (userPassword != value)
                {
                    userPassword = value;
                    OnPropertyChanged(nameof(UserPassword));
                }
            }
        }
        public string VerifyNewPassword
        {
            get => userNewPassword;
            set
            {
                if (userNewPassword != value)
                {
                    userNewPassword = value;
                    OnPropertyChanged(nameof(UserPassword));
                }
            }
        }
        private async void ChangePassword()
        {
            if ((string.IsNullOrWhiteSpace(UserPassword)) || (string.IsNullOrWhiteSpace(VerifyNewPassword)))
                return;
            var npr = UserPassword;
            var vpwr = VerifyNewPassword;
            if (NewPassWordRev)
            {
                npr = Utilites.ALanoClubUtilites.RevStr(UserPassword);
            }
            if (VerifyPassWordRev)
            {
                vpwr = Utilites.ALanoClubUtilites.RevStr(VerifyNewPassword);
            }
            if (string.Compare(npr, vpwr, false) != 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Error Passwords Dont Match", "Passwords", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (UserPassword.Length < 8)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Error Password Must be at least 8 charaters", "Password length", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!(await Utilites.ALanoClubUtilites.RexMatchStr(UserPassword, Utilites.AlanoCLubConstProp.PasswordPattern)))
            {

                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Password Must contain at least one {Utilites.AlanoCLubConstProp.PasswordPattern} charaters", "Password length", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!(UserPassword.Any(char.IsLetter)))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Password Must contain a character", "Password  Character", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!(UserPassword.Any(char.IsDigit)))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Password Must contain a digit", "Password  digit", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(UserPassword.Any(char.IsUpper)))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Password Must contain at least one upper case character", "Password  Character", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!(UserPassword.Any(char.IsLower)))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Password Must contain at least one lower case character", "Password Character", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Utilites.ALanoClubUtilites.SentCodePW = false;
            GetSqlConn();
            await Scmd.SqlUserService.UserServiceIntance.ChangeUserPW(LoginUserModel.LoginInstance.ID,sqlConn,UserPassword);
            GoBack();
        }
        private async void GetSqlConn()
        {
            sqlConn = await Utilites.ALanoClubUtilites.GetConnectionStr();
        }
        private bool CanChangePW()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        protected void OnPropertyChanged(string propertyName = null) =>
     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
