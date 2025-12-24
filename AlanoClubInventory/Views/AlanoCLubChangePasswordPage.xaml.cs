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
    /// Interaction logic for AlanoCLubChangePassword.xaml
    /// </summary>
    public partial class AlanoCLubChangePasswordPage : Page
    {
        ChangePassWordViewModel viewModel = new ChangePassWordViewModel();
        public AlanoCLubChangePasswordPage()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordVisible.Visibility == Visibility.Visible)
            {
                viewModel.UserPassword = PasswordVisible.Text;
                PasswordBoxLogin.Password = PasswordVisible.Text;
            }




        }
        private async void ShowPasswordToggle_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.NewPassWordRev = false;
        

        string reversed = new string(PasswordBoxLogin.Password.Reverse().ToArray());
            PasswordVisible.Text = reversed;
            PasswordBoxLogin.Visibility = Visibility.Collapsed;
            PasswordVisible.Visibility = Visibility.Visible;
            viewModel.UserPassword = PasswordVisible.Text;
        }
        private async void ShowPasswordToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            viewModel.NewPassWordRev = true;
            string reversed = new string(PasswordBoxLogin.Password.Reverse().ToArray());
            PasswordBoxLogin.Password = reversed;
            PasswordVisible.Visibility = Visibility.Collapsed;
            PasswordBoxLogin.Visibility = Visibility.Visible;
            viewModel.UserPassword = PasswordBoxLogin.Password;

        }
        private async void ShowNewPasswordToggle_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.NewPassWordRev = false;
            string reversed = new string(VerifyPasswordNew.Password.Reverse().ToArray());
            VerifyPasswordNewVisible.Text = reversed;
            VerifyPasswordNew.Visibility = Visibility.Collapsed;
            VerifyPasswordNewVisible.Visibility = Visibility.Visible;
            viewModel.VerifyNewPassword = PasswordVisible.Text;
        }
        private async void ShowNewPasswordToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            viewModel.NewPassWordRev = true;
            string reversed = new string(VerifyPasswordNewVisible.Text.Reverse().ToArray());
            VerifyPasswordNew.Password = reversed;
            VerifyPasswordNewVisible.Visibility = Visibility.Collapsed;
            VerifyPasswordNew.Visibility = Visibility.Visible;
            viewModel.VerifyNewPassword = VerifyPasswordNew.Password;
        }
      
        private void PasswordVefy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VerifyPasswordNewVisible.Visibility == Visibility.Visible)
            {
                viewModel.VerifyNewPassword = VerifyPasswordNewVisible.Text;
                VerifyPasswordNew.Password = PasswordVisible.Text;
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Utilites.ALanoClubUtilites.SentCodePW)
            {
              if(await  Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Must Change Password","Change Password",MessageBoxButton.OKCancel,MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    return;
                }
             else
                {
                   Application.Current.Shutdown();
                }
            }
        
            NavigationService.GoBack();
        }
    }
    }
