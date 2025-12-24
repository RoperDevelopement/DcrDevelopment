using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.ViewModels;
using AlanoClubInventory.Views;
using Azure;
using PdfSharp.Diagnostics;
using PdfSharp.Fonts;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AlanoClubInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private readonly MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            GlobalFontSettings.UseWindowsFontsUnderWsl2 = true;
            //  PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = new PdfSharpCore.Fonts.WindowsFontResolver();
            if (GlobalFontSettings.FontResolver == null)
                GlobalFontSettings.FontResolver = new WindowsFontResolver();



            this.NavigationService.LoadCompleted += OnLoadCompleted;
            this.WindowState = WindowState.Maximized;
             //    this.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/butteac.ico"));
            // this.Name = "Alano Club Inventory Application";
            //  DocPanelShow.Visibility = Visibility.Collapsed;
            DataContext = viewModel;
            BtnLogin.Visibility = Visibility.Visible;
            // GetAlnoClubInfo();
            //   Login();

            //   MainFrame.Navigate(new AlanoClubLoginPage());
            //
            ///   Utilites.Emails.EmailsInstance.SendEmail(string.Empty, string.Empty, "Alano Club Inventory Application Started", "The Alano Club Inventory Application Has Started Successfully.");

        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            var loadedPage = e.Content;
            MainPageWindow = loadedPage as Page;
            
            string pageName = loadedPage.GetType().Name;
            if (MainPageWindow != null)
            {
                //MessageBox.Show($"{pageName} has been loaded!");

                if (!(string.IsNullOrEmpty(MainPageWindow.Name)) && (string.Compare(MainPageWindow.Name, "MainPageWindow", true) == 0))
                {
                     this.NavigationService.RemoveBackEntry();
                    if ((Utilites.ALanoClubUtilites.IsLoggin) && (!(viewModel.UserIsLoggingIn)))
                    {
                        BtnLogin.Visibility = Visibility.Hidden;
                        ButCancel.Visibility = Visibility.Hidden;
                        viewModel.UserIsLoggingIn = true;
                        viewModel.SetIsAdmin();
                        if(Utilites.ALanoClubUtilites.SentCodePW)
                        {
                           
                            ChangePassword(null, null);
                        }
                        // AlanoCLubChangePasswordPage cLubChangePasswordPage = new AlanoCLubChangePasswordPage();
                        //  this.NavigationService.Navigate(cLubChangePasswordPage);
                    }

                }
              //  if (Utilites.ALanoClubUtilites.IsLoggin)
              //  {
               //     viewModel.ChangeMainText();
               // }
                if (!(string.IsNullOrEmpty(pageName)) && (string.Compare(pageName, "AlanoClubLoginPage", true) == 0))
                {



                    // viewModel.IsLoggingIn = false;
                }

            }
            //if (loadedPage is MainWindow)
            // {
            //   MessageBox.Show("MainWindow has been loaded!");
            // }
            // this.NavigationService.RemoveBackEntry();
            //  var navService = e.Navigator as NavigationService;
            ////  NavigationService navService1 = NavigationService.GetNavigationService(e.Navigator);
            //  if (e.Content != null)
            //  {
            //      var t = NavigationService.CurrentSource;
            //      var ed = e.ExtraData as string;
            //      string pageTypeName = e.Content.GetType().Name;
            //      if (pageTypeName == "Page")
            //      {
            //        while (NavigationService.CanGoForward)
            //       {
            //      NavigationService.RemoveBackEntry();
            //              break;
            //      }
            //      }

            //      //  Console.WriteLine($"Navigated to page: {pageTypeName}");
            //  }
            //  //if (Title.StartsWith("Home Alano Club"))
            //  //{
            //     // if (navService != null)
            //   //   {
            //        //  var cs = navService.CurrentSource;

            //    //  }



        }
        //private async void Login()
        //{
        ////    if(!ALanoClubUtilites.IsLoggin)
        // //   { 
        //         AlanoClubLoginPage loginPage = new AlanoClubLoginPage();

        // //   this.NavigationService.Navigate(loginPage);
        // //   }

        //    // Create the NavigationWindow and set the login page as its content
        //      NavigationWindow navigationWindow = new NavigationWindow();
        //      navigationWindow.Content = loginPage;
        //     navigationWindow.Title = "Login";
        //    navigationWindow.Show();
        //}
        private async void MembersReportPage(object sender, RoutedEventArgs e)
        {
            AlanoClubMembersReportPage membersReportPage = new AlanoClubMembersReportPage();
            this.NavigationService.Navigate(membersReportPage);


        }

        private async void AlanoClubDashBoard(object sender, RoutedEventArgs e)
        {
            AlanoClubDashBoardPage categoriesPage = new AlanoClubDashBoardPage();
            this.NavigationService.Navigate(categoriesPage);


        }
        private async void AddEditDeleteCategories(object sender, RoutedEventArgs e)
        {
            AddEditDeleteCategoriesPage categoriesPage = new AddEditDeleteCategoriesPage();
            this.NavigationService.Navigate(categoriesPage);
        }



        private async void Members(object sender, RoutedEventArgs e)
        {
            AlanoCLubMembersPage alanoCLubMemnersPage = new AlanoCLubMembersPage();
            this.NavigationService.Navigate(alanoCLubMemnersPage);


        }
        private async void UpDateInventory(object sender, RoutedEventArgs e)
        {
            AlanoClubCurrentInventoryPage alanoClubCurrentInventoryPage = new AlanoClubCurrentInventoryPage();
            this.NavigationService.Navigate(alanoClubCurrentInventoryPage);


        }
        private async void ClickAboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            this.NavigationService.Navigate(aboutWindow);


        }


        private async void DailyRecpts(object sender, RoutedEventArgs e)
        {
            DailyTillReceiptsPage recp = new DailyTillReceiptsPage();

            this.NavigationService.Navigate(recp);



        }

        private async void AddEditDeleteOtherProducts(object sender, RoutedEventArgs e)
        {
            OtherProductsPage otherProductsPage = new OtherProductsPage();
            this.NavigationService.Navigate(otherProductsPage);


        }
        private async void AlanoCLubReports(object sender, RoutedEventArgs e)
        {
            CreateReportPage repPage = new CreateReportPage();
            this.NavigationService.Navigate(repPage);


        }
        private async void AddEditProductPrices(object sender, RoutedEventArgs e)
        {
            AddEditProductPricesPage productsPage = new AddEditProductPricesPage();
            this.NavigationService.Navigate(productsPage);


        }
        private async void AddEditDeleteInventory(object sender, RoutedEventArgs e)
        {
            InvenntoryPage invenntoryPage = new InvenntoryPage();
            this.NavigationService.Navigate(invenntoryPage);


        }


        private async void ProfitLossPage(object sender, RoutedEventArgs e)
        {


            ProfitLossPage profitLossPage = new ProfitLossPage();
            this.NavigationService.Navigate(profitLossPage);


        }
        private async void CurrentInv(object sender, RoutedEventArgs e)
        {


            AlanoClubCurrentInventoryPage alanoClubCurrentInventoryPage = new AlanoClubCurrentInventoryPage();
            this.NavigationService.Navigate(alanoClubCurrentInventoryPage);

        }

        private async void Exit(object sender, RoutedEventArgs e)
        {


            this.Close();

        }
        private async void ClickLogout(object sender, RoutedEventArgs e)
        {
            if (await Utilites.ALanoClubUtilites.ShowMessageBoxResults("Do you want to Logout", "Logout", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
            else return;


        }
        private async void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;
            AlanoClubLoginPage alanoClubLoginPage = new AlanoClubLoginPage();
            this.NavigationService.Navigate(alanoClubLoginPage);
            but.Visibility = Visibility.Hidden;

        }
        private async void AddEditUsers(object sender, RoutedEventArgs e)
        {
            AlanoCLubAddEditUserPage alanoCLubAddEditUserPage = new AlanoCLubAddEditUserPage();
            this.NavigationService.Navigate(alanoCLubAddEditUserPage);
        }
        private async void ChangePassword(object sender, RoutedEventArgs e)
        {
            AlanoCLubChangePasswordPage cLubChangePasswordPage = new AlanoCLubChangePasswordPage();
            this.NavigationService.Navigate(cLubChangePasswordPage);
        }
        private async void MembersPayDues(object sender, RoutedEventArgs e)
        {
            ReceiptsPage payDuesPage = new ReceiptsPage();
            this.NavigationService.Navigate(payDuesPage);
        }

         private async void ClickSendACMembersEmailsPage(object sender, RoutedEventArgs e)
        {
           // RibbonDocumentEditorPage ribbonDocumentEditorPage = new RibbonDocumentEditorPage();
           SendACMembersEmailsPage membersEmailsPage = new SendACMembersEmailsPage();

            this.NavigationService.Navigate(membersEmailsPage);
        }
        private async void ClickPrintVolunteerHrs(object sender, RoutedEventArgs e)
        {
            // RibbonDocumentEditorPage ribbonDocumentEditorPage = new RibbonDocumentEditorPage();
            PrintVolunteerHrsPage printVolunteer = new PrintVolunteerHrsPage();

            this.NavigationService.Navigate(printVolunteer);
        }
        
        private async void ClickDBMaintenance(object sender, RoutedEventArgs e)
        {
            // RibbonDocumentEditorPage ribbonDocumentEditorPage = new RibbonDocumentEditorPage();
            AlanoCLubDataBaseMaintenancePage maintenancePage = new AlanoCLubDataBaseMaintenancePage();
            maintenancePage.Owner = Window.GetWindow(this);
            maintenancePage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            maintenancePage.Height = 650;
            maintenancePage.Width = 600;
            maintenancePage.Title = "DataBase Maintenance";
            maintenancePage.WindowState = WindowState.Normal;
            maintenancePage.ResizeMode = ResizeMode.CanResize;
            maintenancePage.BorderThickness = new Thickness(2);
            maintenancePage.BorderBrush = Brushes.DodgerBlue;
            maintenancePage.WindowStyle = WindowStyle.ThreeDBorderWindow;
            

            var results = maintenancePage.ShowDialog();
        }
        private async void ClickChangeDatabasePage(object sender, RoutedEventArgs e)
        {
            // RibbonDocumentEditorPage ribbonDocumentEditorPage = new RibbonDocumentEditorPage();
            ChangeDatabasePage databasePage = new ChangeDatabasePage();
            databasePage.Owner = Window.GetWindow(this);
            databasePage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            databasePage.Height = 200;
            databasePage.Width = 300;
            databasePage.Title = "Change Current Database";
            databasePage.WindowState = WindowState.Normal;
            databasePage.ResizeMode = ResizeMode.NoResize;
            databasePage.BorderThickness = new Thickness(2);
            databasePage.BorderBrush = Brushes.DodgerBlue;
            databasePage.WindowStyle = WindowStyle.ThreeDBorderWindow;


            var results = databasePage.ShowDialog();
            viewModel.ChangeMainText();
        }

        
    }
}