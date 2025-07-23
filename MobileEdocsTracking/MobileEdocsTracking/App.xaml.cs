using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileEdocsTracking.Views;
namespace MobileEdocsTracking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage =  new NavigationPage(new MainPageProgram());
           // MainPage = new LoginPage();
           
            
        }
        public static bool IsUserLoggedIn { get; set; }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
