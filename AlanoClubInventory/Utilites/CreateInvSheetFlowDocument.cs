using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AlanoClubInventory.Utilites
{
    public class CreateInvSheetFlowDocument
    {
        public async Task<FlowDocument> CreateInventorySheet(string title, IList<AlanoClubCurrentInventoryModel> inventory)
        {
            AlanoClubCurrentInventoryModel alanoClubCurrent = new AlanoClubCurrentInventoryModel();
            var doc = new FlowDocument
            {
                PagePadding = new Thickness(1),
                ColumnWidth = double.PositiveInfinity,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13
            };
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 24;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            titleParagraph.Foreground = Brushes.DarkBlue;

            // Add the title text
            Run titleRun = new Run(title);
            titleParagraph.Inlines.Add(titleRun);
            doc.Blocks.Add(titleParagraph);
            var table = new Table
            {
                CellSpacing = 1,
                // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Black // Outer border color,
               
            };
            doc.Blocks.Add(table);
            table.Columns.Add(new TableColumn { Width = new GridLength(100) }); // date
            table.Columns.Add(new TableColumn { Width = new GridLength(100) }); // bar
            table.Columns.Add(new TableColumn { Width = new GridLength(100) }); // books
            table.Columns.Add(new TableColumn { Width = new GridLength(100) }); // cofee club
            table.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Rent
            table.Columns.Add(new TableColumn { Width = new GridLength(250) }); // Rent

            var header = new TableRow();
            header.Cells.Add(new TableCell(new Paragraph(new Run("Item Description"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Quanity Total Stock"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Quanity Sold"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Quanity InStock"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Inventory Count"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run(" Total Quanity"))) { FontWeight = FontWeights.Bold });
            var rowGroup = new TableRowGroup();
            rowGroup.Rows.Add(header);
            foreach (var item in inventory)
            {
                var row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ProductName.ToString())))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                alanoClubCurrent.Quantity += item.Quantity;
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Quantity.ToString())))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                alanoClubCurrent.ItemsSold += item.ItemsSold;
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ItemsSold.ToString())))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                alanoClubCurrent.InStock += item.InStock;
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.InStock.ToString())))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                alanoClubCurrent.InventoryCurrent += item.InventoryCurrent;
                //if (item.InventoryCurrent > 0)
               // {
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.InventoryCurrent.ToString())))
                    {
                        BorderThickness = new Thickness(1), // Cell border thickness
                        BorderBrush = Brushes.Gray, // Cell border color
                        Padding = new Thickness(1) // Padding inside the cell
                    });
                //}
                //else
               // {

                 //   row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
                   // {
                     //   BorderThickness = new Thickness(1), // Cell border thickness
                      // / BorderBrush = Brushes.Gray, // Cell border color
                       // Padding = new Thickness(1) // Padding inside the cell
                   // });
               // }

          
                alanoClubCurrent.NewCount += item.NewCount;
               // if(item.NewCount > 0)
                //{ 
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.NewCount.ToString())))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                //}
                //else
                //{
                //    row.Cells.Add(new TableCell(new Paragraph(new Run(string.Empty)))
                //    {
                //        BorderThickness = new Thickness(1), // Cell border thickness
                //        BorderBrush = Brushes.Gray, // Cell border color
                //        Padding = new Thickness(1) // Padding inside the cell
                //    });
                //}
                rowGroup.Rows.Add(row);

            }
            var rowBotton = new TableRow();
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run("Total Inventory"))));
            //{
            //    BorderThickness = new Thickness(1), // Cell border thickness
            //    BorderBrush = Brushes.Gray, // Cell border color
            //    Padding = new Thickness(1) // Padding inside the cell
            //});
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"{alanoClubCurrent.Quantity}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"{alanoClubCurrent.ItemsSold}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"{alanoClubCurrent.InStock}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"{alanoClubCurrent.InventoryCurrent}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"{alanoClubCurrent.NewCount}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowGroup.Rows.Add(rowBotton);
            table.RowGroups.Add(rowGroup);
            doc.Blocks.Add(table);
            await Task.CompletedTask;
            return doc;
        }
    }
}
