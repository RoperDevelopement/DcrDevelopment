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
using System.Windows.Shapes;

namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for AlanoCLubMaintenancePage.xaml
    /// </summary>
    public partial class AlanoCLubDataBaseMaintenancePage : Window
    {
        AlanoCLubDataBaseMaintenanceModel vm = new AlanoCLubDataBaseMaintenanceModel();
        public AlanoCLubDataBaseMaintenancePage()
        {
            InitializeComponent();
            DataContext= vm;
        }

        private void CloseMaintenancePage_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
        private void ClickViewErrorLogs(object sender, RoutedEventArgs e)
        {
            ErrorLogViewerPage errorLogViewerPage = new ErrorLogViewerPage();
            errorLogViewerPage.Owner = Window.GetWindow(this);
            errorLogViewerPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            errorLogViewerPage.Height = 1000;
            errorLogViewerPage.Width = 1000;
            errorLogViewerPage.Title = "DataBase Error Logs";
            errorLogViewerPage.WindowState = WindowState.Maximized;
            //errorLogViewerPage.ResizeMode = ResizeMode.NoResize;

            var results = errorLogViewerPage.ShowDialog();

        }
        
    }
}
