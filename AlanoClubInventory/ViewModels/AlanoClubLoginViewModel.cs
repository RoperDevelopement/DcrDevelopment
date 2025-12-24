using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using Scmd = AlanoClubInventory.SqlServices;

namespace AlanoClubInventory.ViewModels
{
   public class AlanoClubLoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string userPassword;
        private string email;
        private ICommand userLogin;
        private ICommand forgotPassword;
        private bool isLoggingIn;
        private bool isSendingPassword;
        private ICommand sendPassCode;
        private string randomNumber;
        private string txtRandomNumEmail;
        private string sendPWCode;
        private bool isForgotPassWord;
        private DispatcherTimer timer;
        private int remainingSeconds;
        private bool isTimmerOn;
        private string loginResendCode;
        private bool isLoggingInResendCode;
        public AlanoClubLoginViewModel()
        {
            Utilites.ALanoClubUtilites.SentCodePW = false;
            GetSqlConn();
            //     Messenger.Default.Send(new NavigationMessage("GoBack"));
            Utilites.ALanoClubUtilites.IsLoggin = false;
            IsLoggingIn = true;
            IsSendingPassword = false;
            IsForgotPassWord = false;
            TxtRandomNumEmail = "Eamil Adddress:";
            SendPWCode = "Send Code";
            IsTimmerOn = false;
            LoginResendCode = "Login";
            IsLoggingInResendCode = true;
            if (Environment.MachineName.ToUpper() == "DCR")
            {
                Username = "mtcharles@hotmail.com";
                UserPassword = "122495Aa@";
            }
            // TimerModel(30);
        }
        public bool IsLoggingInResendCode
        {
            get => isLoggingInResendCode;
            set
            {
                isLoggingInResendCode = value;
                OnPropertyChanged(nameof(IsLoggingInResendCode));
            }
        }
        public int RemainingSeconds
        {
            get => remainingSeconds;
            set
            {
                remainingSeconds = value;
                OnPropertyChanged();
            }
        }
        public string LoginResendCode
        {
            get => loginResendCode;
            set
            {
                if (loginResendCode != value)
                {
                    loginResendCode = value;
                    OnPropertyChanged(nameof(LoginResendCode));
                }
            }
        }


        public bool IsTimmerOn
        {
            get => isTimmerOn;
            set
            {
                if (isTimmerOn != value)
                {
                    isTimmerOn = value;
                    OnPropertyChanged(nameof(IsTimmerOn));
                }
            }
        }
        public bool IsSendingPassword
        {
            get => isSendingPassword;
            set
            {
                if (isSendingPassword != value)
                {
                    isSendingPassword = value;
                    OnPropertyChanged(nameof(IsSendingPassword));
                }
            }

        }
        public bool IsForgotPassWord
        {
            get => isForgotPassWord;
            set
            {
                if (isForgotPassWord != value)
                {
                    isForgotPassWord = value;
                    OnPropertyChanged(nameof(IsForgotPassWord));
                }
            }

        }
        
        public bool IsLoggingIn
        {
            get => isLoggingIn;
            set
            {
                if (isLoggingIn != value)
                {
                    isLoggingIn = value;
                    OnPropertyChanged(nameof(IsLoggingIn));
                }
            }
        }
        public async void TimerModel(int seconds)
        {
            RemainingSeconds = seconds;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (RemainingSeconds > 0)
            {
                RemainingSeconds--;
            }
            else
            {
                timer.Stop();
                OnTimerExpired();
            }
        }

        protected virtual void OnTimerExpired()
        {
            // Raise an event or call back to close the page
            //TimerExpired?.Invoke(this, EventArgs.Empty);
            NavigationWindow navWindow = Application.Current.MainWindow as NavigationWindow;
            if (navWindow != null)
            {
                navWindow.Close();
            }
        }

        public event EventHandler TimerExpired;
        public string RandomNumber
        {
            get => randomNumber;
            set
            {
                if (randomNumber != value)
                {
                    randomNumber = value;
                    OnPropertyChanged(nameof(RandomNumber));
                }
            }
        }
        public string SendPWCode
        {
            get => sendPWCode;
            set
            {
                if (sendPWCode != value)
                {
                    sendPWCode = value;
                    OnPropertyChanged(nameof(SendPWCode));
                }
            }
        }
        public string TxtRandomNumEmail
        {
            get => txtRandomNumEmail;
            set
            {
                if (txtRandomNumEmail != value)
                {
                    txtRandomNumEmail = value;
                    OnPropertyChanged(nameof(TxtRandomNumEmail));
                }
            }
        }
        
        private string SqlConnectionStr { get; set; }
        private IList<ALanoClubUsersModel> ClubUsersModel { get; set; }
        private IList<StoredParValuesModel> ParValuesModels { get; set; }
        public void GoBack()
        {
            var navWindow = Application.Current.MainWindow as NavigationWindow;
            if (navWindow != null && navWindow.CanGoBack)
            {
             Utilites.ALanoClubUtilites.IsLoggin = true;
                navWindow.GoBack(); 
                navWindow.RemoveBackEntry();
            }
        }
        public ICommand SendPassCode
        {
            get
            {
                if (sendPassCode == null)
                {
                    sendPassCode = new RelayCommd(param => ResetPassword(), param => CanLogin());

                }
                return sendPassCode;
            }
        }
        public ICommand ForgotPassword
        {
            get
            {
                if (forgotPassword == null)
                {
                    forgotPassword = new RelayCommd(param => SendPassword(), param => CanLogin());

                }
                return forgotPassword;
            }
        }
        public ICommand LogIn
        {
            get
            {
                if (userLogin == null)
                {
                    userLogin = new RelayCommd(param => UserLoginExecute(), param => CanLogin());
                    
                }
                return userLogin;
            }
        }
        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
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
        private async void GetSqlConn()
        {
            Utilites.ALanoClubUtilites.AlanoClubDatabaseName = Utilites.AlanoCLubConstProp.AlanoClubDBName;
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        private async void SendPassword()
        {
            IsLoggingIn=false;
            IsSendingPassword = true;
            IsLoggingInResendCode=false;    
            IsForgotPassWord = false;
            Username=string.Empty;
            Utilites.ALanoClubUtilites.SentCodePW = true;
        }
        private async void ResetPassword()
        {
            
            if(!IsTimmerOn)
            {
                GetUserInfor();
                if ((ClubUsersModel == null) || (ClubUsersModel.Count == 0))
            {
                IsForgotPassWord = true;
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Eamil Not Found {Email} not found", "Email Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
            TxtRandomNumEmail = "Enter PassCode";
            
            SendPWCode = "Verify Code:";
                GetCode();


            }
            else
            {
                if (!(VerifyCode())) { return; }
                else
                {
                    IsTimmerOn=false;
                    timer.Stop();
                    LogInAdmin(ClubUsersModel[0].ID);
                    GoBack();
                }

                    
            }
        }
        private async void GetCode()
        {
            RandomNumber = Utilites.ALanoClubUtilites.GetRandomNumer();
            SendEmailCode(RandomNumber);
        }
        private  bool VerifyCode()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Error Invalid Code", "Code", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if ((string.Compare(Username.Trim(),RandomNumber,true) != 0))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Invalid Code {Username}", "Code", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
       private async void SendEmailCode(string code)
        {
            
            LoginResendCode = "Resend Code";
            IsLoggingInResendCode = true;
            CreateHtmlEMails htmlEMails = new CreateHtmlEMails();
            StringBuilder sb = await htmlEMails.CreateHeader(DateTime.Now,"Butte ALano CLub Code", "pack://application:,,,/Resources/Images/butteac.ico");
            sb = await htmlEMails.AddSendCodeHtmlFile(ClubUsersModel[0].UserName, code,sb);
            sb = await htmlEMails.CloseHtmlFile(sb);
           // File.WriteAllText("l:\\d.html",sb.ToString());
            Emails.EmailsInstance.SendEmail(ClubUsersModel[0].UserEmailAddress, string.Empty, "Butte ALano Club Login Code", sb.ToString(), true,string.Empty);
            Username = string.Empty;
            if(IsTimmerOn)
                timer.Stop();
            IsTimmerOn = true;
          
            TimerModel(120);
        }
        private async void UserLoginExecute()
        {
            if (!(IsTimmerOn))
            {

                if (string.IsNullOrWhiteSpace(Username))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Error Need User Name", "User Name", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(UserPassword))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Error Need User Password", "User Password", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if ((string.Compare(Username, Utilites.AlanoCLubConstProp.AlanoCLubAdmin, true) == 0))
                {
                    var pwrev = new string(UserPassword.Reverse().ToArray());
                    if ((string.Compare(UserPassword, Utilites.AlanoCLubConstProp.AlanoCLubAdminPW, false) == 0) || (string.Compare(pwrev, Utilites.AlanoCLubConstProp.AlanoCLubAdminPW, false) == 0))
                        LogInAdmin(0);

                }
                else
                {
                    GetUserInfor();
                    if ((ClubUsersModel == null) || (ClubUsersModel.Count == 0))
                    {
                        IsForgotPassWord = true;
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Error User Name {Username} not found", "User Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (await LoginSystem())
                    {
                        LogInAdmin(ClubUsersModel[0].ID);
                    }
                    else
                    {
                        return;
                    }
                }


                GoBack();
            }
            else
            {
                GetCode();
            }

            // Implement login logic here
            // For example, validate username and password
            //   await Task.Run(() =>
            //  {
            // Simulate a login process
            System.Threading.Thread.Sleep(1000);
           // });
            // After successful login, navigate back
           // GoBack();
        }
        private async void LogInAdmin(int id)
        {
            LoginUserModel.LoginInstance.ID = id;
            if (id == 0)
            {
                LoginUserModel.LoginInstance.UserEmailAddress = "buttealano@gmail.com";
                LoginUserModel.LoginInstance.UserName = "AlanoCLubAdmin";
                LoginUserModel.LoginInstance.IsAdmin = true;
                LoginUserModel.LoginInstance.UserIntils = "AC";
            }
            else
            {
                LoginUserModel.LoginInstance.UserEmailAddress = ClubUsersModel[0].UserEmailAddress;
                LoginUserModel.LoginInstance.IsAdmin= ClubUsersModel[0].IsAdmin;
                LoginUserModel.LoginInstance.UserName = ClubUsersModel[0].UserName;
                LoginUserModel.LoginInstance.UserIntils = ClubUsersModel[0].UserIntils;
                Scmd.SqlUserService.UserServiceIntance.LogInOutVol();
            }
         
        }

            
        public bool IsAdmin { get; set; }
         
        private async Task<bool> LoginSystem()
        {
            bool unpw = await Scmd.SqlUserService.UserServiceIntance.VerifyPassword(UserPassword, ClubUsersModel[0].Salt, ClubUsersModel[0].UserPassword);
            if (!unpw)
            {
                unpw = await Scmd.SqlUserService.UserServiceIntance.VerifyPassword(UserPassword, ClubUsersModel[0].Salt, ClubUsersModel[0].UserPasswordReversed);
                IsForgotPassWord=true;

                
            }
            if (!unpw)
            {
                IsForgotPassWord = true;
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Invalid Password", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Error);
           
            }
            await Task.CompletedTask;
            return unpw;

        }
        private bool CanLogin()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        private async void SPParams()
        {
            ParValuesModels = new List<StoredParValuesModel>();
            ParValuesModels.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaUserName, ParmaValue = Username });
        }
        private async void GetUserInfor()
        {
            SPParams();
            ClubUsersModel =   Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<ALanoClubUsersModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoClubUserInfo, ParValuesModels).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        protected void OnPropertyChanged(string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
