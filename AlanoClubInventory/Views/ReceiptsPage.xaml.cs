using AlanoClubInventory.Models;
using AlanoClubInventory.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MemberPayDuesPage.xaml
    /// </summary>
    public partial class ReceiptsPage : Page
    {
        private readonly ReceiptsViewModel viewModel = new ReceiptsViewModel();

        public ReceiptsPage()
        {
            InitializeComponent();
           this.DataContext = viewModel;
            TAmount.Text = "$0.00";
            BtnAdding.Visibility = Visibility.Collapsed;
           // dataGrid += dataGrid.BeginningEdit
        }

        private async void SaveSoldBY()
        {
            if ((SignatureCanvas.DataContext != null) && (SignatureCanvas.Strokes.Count > 0))
            {
                viewModel.Signed = true;
                RenderTargetBitmap rtb = new RenderTargetBitmap(
          (int)SignatureCanvas.ActualWidth,
          (int)SignatureCanvas.ActualHeight,
          96d, 96d, PixelFormats.Default);

                rtb.Render(SignatureCanvas);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                var i =   System.IO.Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "signature.png");

                using (var fs = new FileStream(i, FileMode.Create))
                {
                    encoder.Save(fs);
                }
            }
        }
        private async void SignatureSave(object sender, RoutedEventArgs e)
        {

            SaveSoldBY();
        }

        private async void SaveSig(object sender, RoutedEventArgs e)
        {
            SaveSoldBY();
        }

        private async void ClickClearSig(object sender, RoutedEventArgs e)
        {
            viewModel.Signed=false;
            SignatureCanvas.Strokes.Clear();
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            var grid = sender as DataGrid;
            if (grid?.CurrentCell != null && grid.DataContext is ReceiptsViewModel vm)
            {
                var cellInfo = grid.CurrentCell;
                
                var column = cellInfo.Column.DisplayIndex;
                // vm.SelectedColumnIndex = grid.CurrentCell.Column.DisplayIndex;
                int rowIndex = grid.Items.IndexOf(grid.CurrentItem);


            }

        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
          //  var grid = sender as DataGrid;
           // if(grid != null)
            
                
             if (e.EditAction == DataGridEditAction.Commit)
            {
                TAmount.Text = viewModel.AmountTotal.ToString("0.00");
                var grid = sender as DataGrid;
                var cellInfo = grid.CurrentCell;
                var currItem = grid.CurrentItem as PayDuesModel;
                int index = viewModel.PayDues.IndexOf(currItem);
                var textBox = e.EditingElement as TextBox;
                Console.WriteLine(textBox.Text);
                Console.WriteLine(viewModel.PayDues[index].Quanity);
               // if (textBox != null)
               // {
                  //  var currItem = grid.CurrentItem as PayDuesModel;
                 //   string newValue = textBox.Text;
                    // Handle the new value here
               // }
             }
        }

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGrid;
            var cellInfo = grid.CurrentItem as PayDuesModel;  
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //var item = e.Row.Item;
            TAmount.Text = viewModel.AmountTotal.ToString("0.00");
        }

        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            TAmount.Text =  $"${viewModel.AmountTotal.ToString("0.00")}";
            if (viewModel.IsAdding)
                BtnAdding.Visibility = Visibility.Visible;
            else BtnAdding.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
