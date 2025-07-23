using Flowapp.Services;
using Flowapp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flowapp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
