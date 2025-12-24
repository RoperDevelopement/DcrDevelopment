using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.DataContracts;
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
    /// Interaction logic for AlanoClubLoginPage.xaml
    /// </summary>
    public partial class AlanoClubLoginPage : Page
    {
        private readonly ViewModels.AlanoClubLoginViewModel alanoClubLoginViewModel = new ViewModels.AlanoClubLoginViewModel();
        public AlanoClubLoginPage()
        {
            InitializeComponent();
            this.DataContext = alanoClubLoginViewModel;
         
        }
        private bool IsUpdating { get; set; } = false;
        private async void GoBack()
        {
            this.Button_Click(this, null);
            //this.NavigationService.RemoveBackEntry();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // MainWindow categoriesPage = new MainWindow();
            // this.NavigationService.Navigate(categoriesPage);
            //  NavigationService.GoBack();
            // Utilites.ALanoClubUtilites.IsLoggin = true;
            // NavigationWindow navWindow = Window.GetWindow(this) as NavigationWindow;
            // if (navWindow != null)
            //{
            //  navWindow.Close();
            // }
            this.NavigationService.GoBack();
            this.NavigationService.RemoveBackEntry();
        }
        private  void PasswordHidden_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (IsUpdating) return;
           //  if (PasswordBoxLogin.Visibility == Visibility.Visible)
            ////{
            //    if(!(string.IsNullOrWhiteSpace((PasswordBoxLogin.Password))))
            //    {
            //        string reversed = new string(PasswordBoxLogin.Password.Reverse().ToArray());
            //        IsUpdating = true;
            //        alanoClubLoginViewModel.UserPassword = reversed;
            //    PasswordBoxLogin.Password=reversed;
            //        IsUpdating = false;
            //    }
                
           // }
            

        }
        private async void ShowPasswordToggle_Checked(object sender, RoutedEventArgs e)
        {
            string reversed = new string(PasswordBoxLogin.Password.Reverse().ToArray());
            PasswordVisible.Text = reversed;
            PasswordBoxLogin.Visibility = Visibility.Collapsed;
            PasswordVisible.Visibility = Visibility.Visible;
            alanoClubLoginViewModel.UserPassword = PasswordVisible.Text;
        }
        private async void ShowPasswordToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            string reversed = new string(PasswordBoxLogin.Password.Reverse().ToArray());
            PasswordBoxLogin.Password = reversed;
            PasswordVisible.Visibility = Visibility.Collapsed;
            PasswordBoxLogin.Visibility = Visibility.Visible;
            alanoClubLoginViewModel.UserPassword = PasswordBoxLogin.Password;
        }
        private void PasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordVisible.Visibility == Visibility.Visible)
            { 
                 alanoClubLoginViewModel.UserPassword = PasswordVisible.Text;
                PasswordBoxLogin.Password= PasswordVisible.Text;
            }

        }


        private void ButtonCancel(object sender, RoutedEventArgs e)
        {
            // MainWindow categoriesPage = new MainWindow();
            // this.NavigationService.Navigate(categoriesPage);
            //  NavigationService.GoBack();
            // Utilites.ALanoClubUtilites.IsLoggin = true;
             NavigationWindow navWindow = Window.GetWindow(this) as NavigationWindow;
             if (navWindow != null)
            {
              navWindow.Close();
             }
            //this.NavigationService.GoBack();
            //this.NavigationService.RemoveBackEntry();
        }
        
    }
}
