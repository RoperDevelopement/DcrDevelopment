using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace AlanoClubInventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unhandled exception: {e.Exception.Message}");
            e.Handled = true; // Prevents app from crashing
            Environment.Exit(0 );
        }

    }


    }


