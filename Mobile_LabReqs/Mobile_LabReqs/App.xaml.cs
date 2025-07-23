using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile_LabReqs.Views;
namespace Mobile_LabReqs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

              MainPage = new MainPage();
           // MainPage = new NavigationPage(new HomePage());

        }

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
