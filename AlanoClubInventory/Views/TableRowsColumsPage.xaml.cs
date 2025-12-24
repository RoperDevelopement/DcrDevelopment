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
using AlanoClubInventory.Utilites;
namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for TableRowsColumsPage.xaml
    /// </summary>
    public partial class TableRowsColumsPage : Window
    {
        public TableRowsColumsPage()
        {
            InitializeComponent();
            this.DataContext = this;
            NumberRows.Text = "0";
            NumberColums.Text = "0";
            RowsCount = 0;
            ColumsCount = 0;
            HasHeader = true;


        }
        public int RowsCount { get; set; }
        public int ColumsCount { get; set; }
        public bool HasHeader { get; set; }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
              ColumsCount = Utilites.ALanoClubUtilites.ConvertToInt(NumberColums.Text).Result;
            if ((ColumsCount == int.MaxValue) || (ColumsCount == 0))
            {
                await Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Invalid number of columns {NumberColums.Text}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RowsCount = Utilites.ALanoClubUtilites.ConvertToInt(NumberRows.Text).Result;
            if ((RowsCount == int.MaxValue) || (RowsCount == 0))
            {
                await Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Invalid number of rows {NumberRows.Text}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
          
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TableHasHeaders_Checked(object sender, RoutedEventArgs e)
        {
            if(TableHasHeaders.IsChecked == true)
                HasHeader = true;
            else
                HasHeader = false;
        }
        private void TableHasHeaders_UnChecked(object sender, RoutedEventArgs e)
        {
            if (TableHasHeaders.IsChecked == false)
                HasHeader = false;
            else
                HasHeader = false;
        }
    }
}
