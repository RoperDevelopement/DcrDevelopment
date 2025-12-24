using AlanoClubInventory.Models;
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
using AlanoClubInventory.ViewModels;
namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for PrintEmailRecieptPage.xaml
    /// </summary>
    public partial class PrintEmailRecieptPage : Page
    {
        private PrintEmailReciptViewModel printEmailRecipt;
        public PrintEmailRecieptPage(DateTime recpDate,IList<PayDuesModel> payDues,int reciptNumber=0,int memberID=0)
        {
            InitializeComponent();
            DataContext = new PrintEmailReciptViewModel(recpDate,payDues,reciptNumber, memberID);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();

            ReceiptsPage rece = new ReceiptsPage();
            this.NavigationService.Navigate(rece);

        }
    }
}
