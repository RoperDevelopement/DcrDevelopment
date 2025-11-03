using AlanoClubInventory.Models;
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
    /// Interaction logic for AlanoClubInventoryPage.xaml
    /// </summary>
    public partial class AlanoClubCurrentInventoryPage : Page
    {
        private readonly AlanoClubCurrentInventoryViewModel viewModel = new AlanoClubCurrentInventoryViewModel();
        public AlanoClubCurrentInventoryPage()
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

        private async void InvCountLostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;

            if (!(string.IsNullOrWhiteSpace(txt.Text)) && (txt != null))
            {
                
                var newCount = await Utilites.ALanoClubUtilites.ConvertToInt(txt.Text);
                if (newCount == 0)
                {
                    return;
                }
                if (newCount == int.MaxValue)
                {

                    Utilites.ALanoClubUtilites.ShowMessageBox($"Error Invalid entry {txt.Text}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt.Text = "0";
                }
                else if (newCount == 0)
                    return;
                else
                {
                    var listViewItem = FindAncestor<ListViewItem>(txt);
                    if (listViewItem != null)
                    {
                        var item = (sender as TextBox)?.DataContext as AlanoClubInventory.Models.AlanoClubCurrentInventoryModel;

                        int index = viewModel.Inventory.IndexOf(item);
                        //  var newItem = viewModel.Inventory.Where(p => p.ID == item.ID).ToList();
                        //var index = Indexof
                        //var vm = (AlanoClubCurrentInventoryModel)this.DataContext;
                        // item.NewCount = (newCount + item.Quantity + item.InStock) - item.ItemsSold;
                        item.NewCount = newCount - item.InStock;
                        item.InventoryCurrent = newCount;
                        viewModel.Inventory.RemoveAt(index);
                        viewModel.Inventory.Insert(index, item);
                        //  viewModel.Inventory.Insert(index,);
                        //newItem[0].NewCount = item.NewCount;
                        Task.Delay(2);
                        txt.Background = Brushes.BurlyWood;
                        //  txt.Background = Brushes.Red;
                        //var vm = (l)this.DataContext
                    }



                }
            }
            else
                txt.Text = "0";
        }
        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                    return (T)current;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void InvCountGotFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            txt.Background = Brushes.AliceBlue;
        }

      

      
        private async void DailyRecpts(object sender, RoutedEventArgs e)
        {
            DailyTillReceiptsPage dailyTillReceiptsPage = new DailyTillReceiptsPage();
            this.NavigationService.Navigate(dailyTillReceiptsPage);
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
        
    }
}
