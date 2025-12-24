using AlanoClubInventory.Utilites;
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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Page
    {
        public AboutWindow()
        {
            InitializeComponent();
            DataContext = new
            {
                Product = $"Product: {AssemblyInfoHelper.GetProduct()}",
                Company = $"Company: {AssemblyInfoHelper.GetCompany()}",
                Description = $"Description: {AssemblyInfoHelper.GetDescription()}",
                CopyRight = $"CopyRight: {AssemblyInfoHelper.GetCopyright()}",
                Version = $"Assembly Version: {AssemblyInfoHelper.GetVersion()}",
                FileVersion = $"File Version: {AssemblyInfoHelper.GetFileVersion()}",
                InfoVersion = $"Informational Version: {AssemblyInfoHelper.GetInformationalVersion()}"
                
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
