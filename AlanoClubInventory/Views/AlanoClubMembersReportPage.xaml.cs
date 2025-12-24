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
    /// Interaction logic for AlanoClubMemersReport.xaml
    /// </summary>
    public partial class AlanoClubMembersReportPage : Page
    {
        AlanoClubMemberReoportViewModel reoportViewModel = new AlanoClubMemberReoportViewModel();
        public AlanoClubMembersReportPage()
        {
            InitializeComponent();
            DataContext= reoportViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
