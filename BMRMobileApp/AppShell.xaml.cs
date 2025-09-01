
using Microsoft.Maui.Controls;
using BMRMobileApp.Utilites;
using BMRMobileApp.Utilites;
namespace BMRMobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {

            InitializeComponent();
            InitRoutes();
            //ChatService chatService = new ChatService();
            //chatService.MessageReceived += async (msg) =>
            //{
            //    var currentPage = Shell.Current.CurrentPage;

            //    // Optional: Check if already on ChatPage
            //    if (currentPage is not ChatPage)
            //    {
            //        await Shell.Current.GoToAsync("//ChatViewPage", true);
            //    }

            //    // Optionally pass the message via query parameters or a shared store
            //};


        }
        private void InitRoutes()
        {
            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute("mainpage", typeof(MainPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            
           Routing.RegisterRoute("feelingpage", typeof(FeelingPage));
          Routing.RegisterRoute("loginpage", typeof(LoginPage));
            Routing.RegisterRoute("addedituserspage", typeof(AddEditUsersPage));
            Routing.RegisterRoute("chatviewpage", typeof(ChatViewPage));
            Routing.RegisterRoute("settingspage", typeof(SettingsPage));
            Routing.RegisterRoute(nameof(RecordViedoPage), typeof(RecordViedoPage));
            Routing.RegisterRoute("droptablespage", typeof(DropTablesPage));
            Routing.RegisterRoute(nameof(RecordVoicePage), typeof(RecordVoicePage));
            Routing.RegisterRoute(nameof(SearchWebPage), typeof(SearchWebPage));
            


            //    Routing.RegisterRoute(nameof(PayPage), typeof(PayPage));
            //    Routing.RegisterRoute(nameof(SignaturePage), typeof(SignaturePage));
            //    Routing.RegisterRoute(nameof(ReceiptPage), typeof(ReceiptPage));
            //    Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
        }

    }
}
