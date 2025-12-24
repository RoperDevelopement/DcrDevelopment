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
    /// Interaction logic for AlanoCLubAddEditUserPage.xaml
    /// </summary>
    public partial class AlanoCLubAddEditUserPage : Page
    {
        private readonly AlanoCLubAddEditUserViewModel viewModel = new AlanoCLubAddEditUserViewModel();
        public AlanoCLubAddEditUserPage()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ButtonClickCan(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void EmailToggle_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (!((bool)checkBox.IsChecked))
            {
                checkBox.IsChecked = true;
                viewModel.EmailInfoUser = true;
            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (((bool)checkBox.IsChecked))
            {
                checkBox.IsChecked = false;
                viewModel.EmailInfoUser = false;
            }

        }
        private void CheckEditUsers(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            // if (!((bool)checkBox.IsChecked))
            //{
            checkBox.IsChecked = true;
            viewModel.IsEditing = true;
            viewModel.TxtButSaveUpDate = "Update";
            viewModel.IsPWVis = false;
            viewModel.EmailInfoUser = false;
            viewModel.EditACUser = "Add User:";
            viewModel.TextTitle = "Alano CLub Edit User";
            viewModel.CreateNewUser();
            // }

        }
        private void UnCheckEditUsers(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            // if (((bool)checkBox.IsChecked))
            // {
            checkBox.IsChecked = false;
            viewModel.TextTitle = "Alano CLub Add User";
            viewModel.IsEditing = false;
            //   viewModel.EmailInfoUser = true;
            viewModel.TxtButSaveUpDate = "Save";
            viewModel.IsPWVis = true;
            viewModel.EditACUser = "Edit User:";
            viewModel.CreateNewUser();
            // }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(viewModel.IsEditing))
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    viewModel.UserName = textBox.Text.Trim();
                    UName.Text = textBox.Text;
                }
            }

        }
        private async void GoBack(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }
    }
}