using AlanoClubInventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for InvenntoryPage.xaml
    /// </summary>
    public partial class InvenntoryPage : Page
    {
        private readonly InventoryViewModel inventoryViewModel = new InventoryViewModel();
        
        public InvenntoryPage()
        {
            InitializeComponent();
            DataContext=inventoryViewModel;
           // CheckCategories();
            

        }
     


        private async void GoBack(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }
        private async void InventoryPage(object sender, RoutedEventArgs e)
        {
            InvenntoryPage invPage = new InvenntoryPage();
            this.NavigationService.Navigate(invPage);
        }
        private async void OtherProd(object sender, RoutedEventArgs e)
        {
            OtherProductsPage otherProductsPage = new OtherProductsPage();
            this.NavigationService.Navigate(otherProductsPage);
        }

        private async void Prices(object sender, RoutedEventArgs e)
        {
            AddEditProductPricesPage addEditProductPricesPage = new AddEditProductPricesPage();
            this.NavigationService.Navigate(addEditProductPricesPage);
        }
        private async void Categories(object sender, RoutedEventArgs e)
        {
            AddEditDeleteCategoriesPage addEditDeleteCategoriesPage = new AddEditDeleteCategoriesPage();
            this.NavigationService.Navigate(addEditDeleteCategoriesPage);
        }
        private async void Reports(object sender, RoutedEventArgs e)
        {
            CreateReportPage createReportPage = new CreateReportPage();
            this.NavigationService.Navigate(createReportPage);
        }
        private async void RloadPage(object sender, RoutedEventArgs e)
        {
            AddEditProductPricesPage addEditProductPricesPage1 = new AddEditProductPricesPage();
            this.NavigationService.Navigate(addEditProductPricesPage1);
        }

        private async void Till(object sender, RoutedEventArgs e)
        {
            AddEditProductPricesPage addEditProductPricesPage1 = new AddEditProductPricesPage();
            this.NavigationService.Navigate(addEditProductPricesPage1);
        }
        private async void UpDateInventory(object sender, RoutedEventArgs e)
        {
            AlanoClubCurrentInventoryPage alanoClubCurrentInventoryPage = new AlanoClubCurrentInventoryPage();
            this.NavigationService.Navigate(alanoClubCurrentInventoryPage);


        }


        private async void AddEditProductPrices(object sender, RoutedEventArgs e)
        {
            AddEditProductPricesPage addEditProductPricesPage = new AddEditProductPricesPage();
            this.NavigationService.Navigate(addEditProductPricesPage);
        }
        private async void DailyRecpts(object sender, RoutedEventArgs e)
        {
            DailyTillReceiptsPage dailyTillReceiptsPage = new DailyTillReceiptsPage();
            this.NavigationService.Navigate(dailyTillReceiptsPage);
        }

    }

}
