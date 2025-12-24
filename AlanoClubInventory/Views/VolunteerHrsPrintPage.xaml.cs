using AlanoClubInventory.Models;
using AlanoClubInventory.Reports;
using AlanoClubInventory.Utilites;
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
    /// Interaction logic for VolunteerHrsPrintPage.xaml
    /// </summary>
    public partial class VolunteerHrsPrintPage : Window
    {
        public VolunteerHrsPrintPage(IList<VolunteerHoursModel> volunteerHours,DateTime sDate,DateTime eDate)
        {
            InitializeComponent();
            DataContext = new VolunteerHrsPrintViewModel(volunteerHours,sDate,eDate);
        }
        
             

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
