using AlanoClubInventory.Interfaces;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using AlanoClubInventory.Utilites;
namespace AlanoClubInventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IShutDownService shutDownService;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            shutDownService = new ShutDownService();

         //   AlanoClubLoginPage loginPage = new AlanoClubLoginPage();

            // Create the NavigationWindow and set the login page as its content
           // NavigationWindow navigationWindow = new NavigationWindow();
          //  navigationWindow.Content = loginPage;
           // navigationWindow.Title = "Login";
           // navigationWindow.Show();

        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unhandled exception: {e.Exception.Message}");
            e.Handled = true; // Prevents app from crashing
            Environment.Exit(0 );
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SqlServices.SqlUserService.UserServiceIntance.LogInOutVol();

        }
    }


    }


