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
using System.Windows.Shapes;
using AlanoClubInventory.Models;
using AlanoClubInventory.ViewModels;
using AlanoClubInventory.Utilites;
using Scm =  AlanoClubInventory.SqlServices;
namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for PrintVolunteerHrsPage.xaml
    /// </summary>
    public partial class PrintVolunteerHrsPage : Page
    {
        PrintVolunteerHrsViewModel viewModel = new PrintVolunteerHrsViewModel();
     //   private ItemListModel selectedItem = new ItemListModel();

        public PrintVolunteerHrsPage()
        {
            InitializeComponent();
           this.DataContext=viewModel;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.GoBack();
        }
        private async void BtnClickPrint(object sender, RoutedEventArgs e)
        {
            VolunteerHrsPrintPage printViewModel = new VolunteerHrsPrintPage(viewModel.VolunteerHours.ToList(),viewModel.SDate,viewModel.EDate);
            printViewModel.Owner = Window.GetWindow(this);
            printViewModel.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            printViewModel.Height = 1000;
            printViewModel.Width = 1000;
            printViewModel.Title = "Print Vol Hours";
            printViewModel.WindowState = WindowState.Maximized;
            printViewModel.WindowStyle = WindowStyle.ThreeDBorderWindow;
            //errorLogViewerPage.ResizeMode = ResizeMode.NoResize;

            var results = printViewModel.ShowDialog();

        }

    }
}

