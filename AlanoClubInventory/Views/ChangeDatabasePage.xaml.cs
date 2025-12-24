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

using AlanoClubInventory.ViewModels;
namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for ChangeDatabasePage.xaml
    /// </summary>
    public partial class ChangeDatabasePage : Window
    {
        private readonly ChangeDatabaseViewModel viewModel = new ChangeDatabaseViewModel();
        public ChangeDatabasePage()
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CloseApp += (s, appClose) => CloseApp(appClose);
        }

        private async void CloseApp(bool appClose)
        {
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
