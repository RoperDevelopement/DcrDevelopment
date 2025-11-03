using AlanoClubInventory.Views;
using Azure;
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
        public MainWindow()
        {
            InitializeComponent();
            this.NavigationService.LoadCompleted += OnLoadCompleted;
            this.WindowState = WindowState.Maximized;
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/acicon.ico"));

        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
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
        private async void UpDateInventory(object sender, RoutedEventArgs e)
        {
            AlanoClubCurrentInventoryPage alanoClubCurrentInventoryPage = new AlanoClubCurrentInventoryPage();
            this.NavigationService.Navigate(alanoClubCurrentInventoryPage);


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

    }
}