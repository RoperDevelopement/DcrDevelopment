using MobileEdocsTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileEdocsTracking.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileEdocsTracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool _areCredentialsInvalid;
        public LoginViewModel ViewModel { get; set; } = new LoginViewModel();
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
            //  UsernameEntry.Completed += (sender, args) => { PasswordEntry.Focus(); };
            UsernameEntry.Completed += (sender, args) => { PasswordEntry.Focus(); };
            PasswordEntry.Completed += (sender, args) => { OnLoginButtonClicked(sender,args); };
            //   PasswordEntry.Completed += (sender, args) => { ViewModel.AuthenticateCommand.Execute(null); };
            //  PasswordEntry.Completed += (sender, args) => { ViewModel.PropertyChanged("PasswordEntry"); };
            if (App.IsUserLoggedIn)
                Application.Current.MainPage = new MainPage();
            //   Application.Current.MainPage =new MainPage();
            //   Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        async void OnLoginButtonClicked(Object sender, EventArgs e)
        {
            ViewModel.AuthenticateCommand.Execute(null);
               await  Navigation.PushModalAsync(new MainPage());
            /// Application.Current.MainPage = new MainPage();
           ///   Application.Current.MainPage = new MainPage();

        }


    }
}