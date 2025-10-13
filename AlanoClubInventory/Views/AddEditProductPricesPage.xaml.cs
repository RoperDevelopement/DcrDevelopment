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
    /// Interaction logic for AddEditProductPricesPage.xaml
    /// </summary>
    public partial class AddEditProductPricesPage : Page
    {
        private readonly AddEditProductPricesViewModel viewModel = new AddEditProductPricesViewModel();
        public AddEditProductPricesPage()
        {
            InitializeComponent();
            this.DataContext = viewModel;
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
      

        private async void Till(object sender, RoutedEventArgs e)
        {
            DailyTillReceiptsPage dailyTillReceiptsPage = new DailyTillReceiptsPage();
            this.NavigationService.Navigate(dailyTillReceiptsPage);
        }

        private async void TextBoxClubPriceGotFocus(object sender, RoutedEventArgs e)
        {

            var but = sender as TextBox;
            if (but != null)
            {
                but.Background = Brushes.LightYellow;

            }

        }
        private void TextBoxClubPriceLostFocus(object sender, RoutedEventArgs e)
        {
            var but = sender as TextBox;
            if (but != null)
            {
                but.Background = Brushes.White;

            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
