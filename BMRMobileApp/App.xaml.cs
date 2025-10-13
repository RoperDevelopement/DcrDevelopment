
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
namespace BMRMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Global error handlers
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            Init();
            // MainPage = new NavigationPage(new MainPage());
            // MainPage = new LoginPage();
            //  MainPage = new NavigationPage(new LoginPage());
            //   MainPage = new NavigationPage(new LoginPage());
            //MainPage = new AddEditUsersPage(true);
            //   MainPage = new LoginPage();

        }
        private void Init()
        {
            Utilites.EmailService emailService = new Utilites.EmailService();
            //    emailService.SendEmailAsync("mtcharles@hotmail.com", "Test Email from Peer Support App", "This is a test email sent from the Peer Support App.").Wait();

bool deleteFile = false;
            if(deleteFile)
            { 
                  if (File.Exists(Path.Combine(FileSystem.AppDataDirectory, "dcrpeersuppotusers.db")))
               { 
               File.Delete(Path.Combine(FileSystem.AppDataDirectory, "dcrpeersuppotusers.db"));
             }
            }

            if (!(File.Exists(Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "dcrpeersuppotusers.db"))))
            {

                MainPage = new LoadingPage();
                Task.Run(async () =>
                {
                    //    // Simulate app initialization
                    await Task.Delay(2000);
                    MainThread.BeginInvokeOnMainThread(() => MainPage = new CommunityGuidelinesPage());

                });

            }
            else
            {
                MainPage = new LoadingPage();
                Task.Run(async () =>
                {
                    //    // Simulate app initialization
                    await Task.Delay(2000);
                    MainThread.BeginInvokeOnMainThread(() => MainPage = new LoginPage());

                });
            }
        }
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            HandleGlobalException("UnhandledException", ex);
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleGlobalException("UnobservedTaskException", e.Exception);
            e.SetObserved(); // Prevents app crash
        }

        private void HandleGlobalException(string source, Exception ex)
        {
            // Log to file, send to Azure App Insights, or show alert
           // Debug.WriteLine($"[{source}] {ex?.Message}");

            // Optional: Show user-friendly message
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Oops!", $"Something went wrong. Please try again. {ex.Message}", "OK");
            });
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        // return new Window(new AppShell());
        // }
    }
}