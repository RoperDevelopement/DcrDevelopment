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
    /// Interaction logic for ErrorLogViewerPage.xaml
    /// </summary>
    public partial class ErrorLogViewerPage : Window
    {
        private ErrorLogVieweModel vm = new ErrorLogVieweModel();
        public ErrorLogViewerPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private  async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void KeyWordFocus(object sender, RoutedEventArgs e)
        {
            KeywordBox.Text= "";
        }
    }
}
