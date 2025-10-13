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
    /// Interaction logic for DailyTillReceiptsPage.xaml
    /// </summary>
    public partial class DailyTillReceiptsPage : Page
    {
        private readonly ViewModels.DailyTillReceiptViewModel viewModel = new ViewModels.DailyTillReceiptViewModel();
        public DailyTillReceiptsPage()
        {
            InitializeComponent();
          

            this.DataContext = viewModel;
        }

        private void DatePickerDateValidationError(object sender, DatePickerDateValidationErrorEventArgs e)
        {

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                
                
                var listViewItem = FindAncestor<ListViewItem>(textBox);
             
                if (listViewItem != null)

    {
                    
                    var item = (sender as TextBox)?.DataContext as AlanoClubInventory.Models.AlanoClubTillPricesModel;
                    //var currentItem = listViewItem?.DataContext as AlanoClubInventory.Models.AlanoClubTillPricesModel;
                    //var currSelItem = vm.TillPrices.FirstOrDefault(i => i.ID == currentItem.ID);
                    GetTotal(item);
                    
                    textBox.Background = Brushes.White;
                    //   var item = MyListView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    //  MessageBox.Show($"Lost focus on item: {item}");
                }
            }
        }
        private async void GetTotal(AlanoClubInventory.Models.AlanoClubTillPricesModel alanoClubTillPrices)
        {
            var vm = (DailyTillReceiptViewModel)this.DataContext;
            alanoClubTillPrices.DailyProductTotal = (alanoClubTillPrices.ClubPrice * alanoClubTillPrices.TotalMemberSold) + (alanoClubTillPrices.ClubNonMemberPrice * alanoClubTillPrices.TotalNonMemberSold);
            int index = vm.TillPrices.IndexOf(alanoClubTillPrices);
            vm.TillPrices.RemoveAt(index);
            vm.TillPrices.Insert(index, alanoClubTillPrices);
            vm.TotalSales = vm.TillPrices.Sum(x => x.DailyProductTotal);
            vm.TillOverShort = await Utilites.ALanoClubUtilites.ConvertToFloat(vm.TotalTillReceipts) - vm.TotalSales;
            TShort.Background =Brushes.LightGray;
            if (vm.TillOverShort < 0)
            {
                TShort.Background = Brushes.Red;
            }
            //  vm.TillPrices[index].ClubPrice = alanoClubTillPrices.ClubPrice;
            //  vm.TillPrices[index].ClubNonMemberPrice = alanoClubTillPrices.ClubNonMemberPrice;
            //  vm.TillPrices[index].TotalMemberSold = alanoClubTillPrices.TotalMemberSold;
            //  vm.TillPrices[index].TotalNonMemberSold = alanoClubTillPrices.TotalNonMemberSold;
            // vm.TillPrices[index].DailyProductTotal = (alanoClubTillPrices.ClubPrice * alanoClubTillPrices.TotalMemberSold) + (alanoClubTillPrices.ClubNonMemberPrice * alanoClubTillPrices.TotalNonMemberSold);
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

        private async void TShort_LostFocus(object sender, RoutedEventArgs e)
        {
            var vm = (DailyTillReceiptViewModel)this.DataContext;
            var tr = await Utilites.ALanoClubUtilites.ConvertToFloat(TShort.Text.ToString());
            if (tr < 0)
            {
                 Utilites.ALanoClubUtilites.ShowMessageBox("Please enter valid amount for Total Till Receipts","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            
            
            vm.TillOverShort = await Utilites.ALanoClubUtilites.ConvertToFloat(vm.TotalTillReceipts) - vm.TotalSales;
            TShort.Background = Brushes.LightGray;
            TShort.Text = vm.TillOverShort.ToString("0.00");
            if (vm.TillOverShort < 0)
            {
                TShort.Background = Brushes.Red;
            }
            else
                TShort.Background = Brushes.LightGray;
            await Task.CompletedTask;
        }
        
        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            txt.Background= Brushes.White;
        }

        private void TotalMemberSold_GotFocus(object sender, RoutedEventArgs e)
        {
           // MainFrame.NavigationService.Navigate(new AddEditDeleteCategoriesPage());

            var txt = sender as TextBox;
            txt.Background = Brushes.AliceBlue;
        }

        private void TextBox_LostFocus_2(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            txt.Background = Brushes.White;
        }

        private void TShort_LostFocus_1(object sender, RoutedEventArgs e)
        {

        }
        private async void DepsopitLostFocous(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            var cd = await Utilites.ALanoClubUtilites.ConvertToFloat(txt.Text);
            if (cd < 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Please enter valid amount for Club Deposit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            txt.Background = Brushes.White;
            viewModel.ClubDeposit = cd.ToString();
            await Task.CompletedTask;
        }
        private async void TillReciptsLF(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            var tr = await Utilites.ALanoClubUtilites.ConvertToFloat(txt.Text);
            if (tr < 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Please enter valid amount for Total Till Receipts", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            txt.Background = Brushes.White;
            viewModel.TotalTillReceipts = tr.ToString("0.00");
            viewModel.TillOverShort = tr-viewModel.TotalSales ;
            TShort.Text = viewModel.TillOverShort.ToString("0.00");
            if (viewModel.TillOverShort < 0)
            {
                TShort.Background = Brushes.Red;
            }
            else
                TShort.Background = Brushes.LawnGreen;
            
            await    Task.CompletedTask;
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
            DailyTillReceiptsPage dailyTillReceiptsPage = new DailyTillReceiptsPage();
            this.NavigationService.Navigate(dailyTillReceiptsPage);
        }
    }
}
