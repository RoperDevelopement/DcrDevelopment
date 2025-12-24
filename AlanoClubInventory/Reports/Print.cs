using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
namespace AlanoClubInventory.Reports
{
    public class Print
    {
        public void PrintGrid(Grid gridToPrint)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Optional: scale to fit printable area
                var capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                double scale = Math.Min(
                    capabilities.PageImageableArea.ExtentWidth / gridToPrint.ActualWidth,
                    capabilities.PageImageableArea.ExtentHeight / gridToPrint.ActualHeight);

                gridToPrint.LayoutTransform = new ScaleTransform(scale, scale);

                // Measure and arrange the visual
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                gridToPrint.Measure(sz);
                gridToPrint.Arrange(new Rect(new Point(
                    capabilities.PageImageableArea.OriginWidth,
                    capabilities.PageImageableArea.OriginHeight), sz));

                // Print the visual
                printDialog.PrintVisual(gridToPrint, "Printing Grid");
            }
        }
        private void PrintVisual(UIElement element)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(element, "Print Grid Data");
            }
        }
        public void PrintGridData(IEnumerable<AddEditInventoryModel> data)
        {
            FlowDocument doc = new FlowDocument();
            doc.PagePadding = new Thickness(50);
            doc.ColumnWidth = double.PositiveInfinity;

            Table table = new Table();
            doc.Blocks.Add(table);

            // Define columns
            table.Columns.Add(new TableColumn { Width = new GridLength(200) });
            table.Columns.Add(new TableColumn { Width = new GridLength(200) });

            // Header row
            TableRow header = new TableRow();
            header.Cells.Add(new TableCell(new Paragraph(new Run("Mood"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Notes"))) { FontWeight = FontWeights.Bold });
            table.RowGroups.Add(new TableRowGroup { Rows = { header } });

            // Data rows
            //foreach (var item in data)
            //{
            //    TableRow row = new TableRow();
            //    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Mood))));
            //    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Notes))));
            //    table.RowGroups[0].Rows.Add(row);
            //}

            // Print
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                IDocumentPaginatorSource idpSource = doc;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Print Grid Data");
            }
        }
        public void PrintDataGrid(DataGrid dataGrid)
        {
            dataGrid.EnableRowVirtualization = false;
            dataGrid.EnableColumnVirtualization = false;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                //   Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                // dataGrid.Measure(pageSize);
                // dataGrid.Arrange(new Rect(new Point(0, 0), pageSize));
                dataGrid.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                dataGrid.Arrange(new Rect(new Point(0, 0),
                    new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight)));
                printDialog.PrintVisual(dataGrid, "DataGrid Print");
                
            }
            dataGrid.EnableRowVirtualization = true;
            dataGrid.EnableColumnVirtualization = true;
        }
        public void PrintDataGridAllRows(DataGrid dataGrid)
        {
            FlowDocument doc = new FlowDocument();
            doc.ColumnWidth = double.PositiveInfinity;
            Table table = new Table();
            table.CellSpacing = 1;
            table.BorderThickness = new Thickness(1);
            table.BorderBrush = Brushes.Black;
            table.FontSize = 10;
            table.Margin = new Thickness(0, 0, 0, 0);
            doc.Blocks.Add(table);

            // Add columns
            foreach (var col in dataGrid.Columns)
                table.Columns.Add(new TableColumn { Width = new GridLength(75) });

            // Header row
            TableRow headerRow = new TableRow();
            foreach (var col in dataGrid.Columns)
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run(col.Header?.ToString() ?? ""))));
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(headerRow);

            // Data rows
            foreach (var item in dataGrid.ItemsSource)
            {
                TableRow row = new TableRow();
                foreach (var col in dataGrid.Columns)
                {
                    var binding = (col as DataGridBoundColumn)?.Binding as Binding;
                    string propName = binding?.Path.Path;
                    string text = item.GetType().GetProperty(propName)?.GetValue(item)?.ToString() ?? "";
                    row.Cells.Add(new TableCell(new Paragraph(new Run(text))));
                }
                table.RowGroups[0].Rows.Add(row);
            }
            PrintHelper.PrintFlowDocument(doc);
            //printHelper.PrintFlowDocument(doc);
            //PrintDialog pd = new PrintDialog();
            //if (pd.ShowDialog() == true)
              //  pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "DataGrid Print");
        }





    }
}
