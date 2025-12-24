using AlanoClubInventory.ViewModels;
using ScottPlot;
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
    /// Interaction logic for AlanoCLubMembersPage.xaml
    /// </summary>
    public partial class AlanoCLubMembersPage : Page
    {
        AlanoClubMembersViewModel vm = new AlanoClubMembersViewModel();    
        public AlanoCLubMembersPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var mID = await Utilites.ALanoClubUtilites.ConvertToInt(vm.MemberID.ToString());
            if ((mID == int.MaxValue) || (mID == 0))
            {
             
                return;
            }
            if (await vm.CheckMemberIDNotUsed())
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Member ID {vm.MemberID} Already In Use Please Enter A Different Member ID", "Member ID In Use", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
	    var listView = sender as ListView;
	    var selectedItem = listView.SelectedItem;
            if (selectedItem != null)
            {
                 var selItemIndex =   (Models.AlanoCLubMembersModel)listView.SelectedItem;
                vm.SelectedMemberID = vm.Members.IndexOf(selItemIndex);
                vm.MemberID = (selectedItem as Models.AlanoCLubMembersModel).MemberID;
                vm.MemberFirstName = (selectedItem as Models.AlanoCLubMembersModel).MemberFirstName;
                vm.MemberLastName = (selectedItem as Models.AlanoCLubMembersModel).MemberLastName;
                
                vm.MemberEmail = (selectedItem as Models.AlanoCLubMembersModel).MemberEmail;
                if (string.Compare(vm.MemberEmail,Utilites.AlanoCLubConstProp.NA,true) == 0)
                    vm.MemberEmail = string.Empty;
                vm.MemberPhoneNumber = (selectedItem as Models.AlanoCLubMembersModel).MemberPhoneNumber;
                if (string.Compare(vm.MemberPhoneNumber, Utilites.AlanoCLubConstProp.NA, true) == 0)
                    vm.MemberPhoneNumber = string.Empty;
                vm.SobrietyDate = (selectedItem as Models.AlanoCLubMembersModel).SobrietyDate;    
             //   vm.MembershipEndDate = (selectedItem as Models.AlanoCLubMembersModel).MembershipEndDate;
                vm.IsActiveMember = (selectedItem as Models.AlanoCLubMembersModel).IsActiveMember;
                vm.IsBoardMember = (selectedItem as Models.AlanoCLubMembersModel).IsBoardMember;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
