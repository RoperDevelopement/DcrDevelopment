using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AlanoClubInventory.Reports
{
  public  class CreateProfitLossReport
    {
        private FlowDocument Document { get; set; }
        private Table FlowDocTable { get; set; }
        private TableRowGroup FlowDocTableRowGroup { get; set; }
        private TableRow FlowDocTableHeader { get; set; }
        private TableRow FlowDocTableRow { get; set; }
        private TableRow FlowDocTableBottomRow { get; set; }
        private TableColumn FlowDocTableColumn { get; set; }
        private Paragraph TitleParagraph { get; set; }
        private AlanoCLubProfitLossModel Totals { get; set; }= new AlanoCLubProfitLossModel();
        private float TotalMemP { get; set; } = 0.0f;
        private float TotalNonMemP { get; set; } = 0.0f;
        private async Task InitializeFlowDocument(string title)
        {
            Document = new FlowDocument
            {
                
                PagePadding = new Thickness(6, 0, 0, 0),
                ColumnWidth = double.PositiveInfinity,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 15

            };
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 18;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            //titleParagraph.Foreground = Brushes.DarkBlue;

            // Add the title text
            Run titleRun = new Run($"\r\n\r\n{title}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 16;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            titleParagraph.Foreground = Brushes.DarkBlue;
            titleRun = new Run($"\r\nReport Created {DateTime.Now.ToString("MM-dd-yyyy")}");
            // Add the title paragraph to the FlowDocument
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
        }
        private async Task NetProfit()
        {
            float netProfitMargion = ((float)(TotalMemP)/ (float)Totals.TotalPrice) * (float)(100.00f);
            string netMargin = $"Net Profit Margin Member {netProfitMargion.ToString("0.00")}%";
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 18;
            titleParagraph.BorderBrush = Brushes.Gray;
            titleParagraph.BorderThickness = new Thickness(1);
            titleParagraph.FontWeight = FontWeights.Medium;
            titleParagraph.Margin = new Thickness(0, 4, 0, 0);
            //titleParagraph.Foreground = Brushes.DarkBlue;

            // Add the title text
            Run titleRun = new Run($"{netMargin}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
            netProfitMargion = ((float)(TotalNonMemP) / (float)Totals.TotalPrice) * (float)(100.00f);
            netMargin = $"Net Profit Margin Non Member {netProfitMargion.ToString("0.00")}%";
            titleRun = new Run($"{AlanoCLubConstProp.CarrageReturnLineFeed}{netMargin}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);

        }
    
        private async Task InitializeFlowDocumentTable(bool mainRep)
        {


            FlowDocTable = new Table
            {
                CellSpacing = 1, // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Gray, // Outer border color
                FontSize = 12,
                Margin=new Thickness(5,0,5,0)
                

            };
            Document.Blocks.Add(FlowDocTable);
            var header = new TableRow();
            if (mainRep)
            {
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // month
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // pn
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // member items
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // member price
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Rent
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Coins
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Donations
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Daily Total Other
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Daily Total Other
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Daily Total Other
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Daily Total Other
                                                                                          //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // TotalTotal
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Daily Total Other
                

                //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(70) }); // tape
                //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort

                header.Cells.Add(new TableCell(new Paragraph(new Run("Product")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                    

                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Per Case")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Perchase Price ")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("In Stock")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Cases")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Cost Per Item")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Cost")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Club Price")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Profit Member")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("TP Member")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Non Member Price")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
             
             
           

                header.Cells.Add(new TableCell(new Paragraph(new Run("Profit Non Member")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("TP Non Member")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });

            }
            else
            {
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // month
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // pn
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // member items
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // member price
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Rent
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Coins
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Donations
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Daily Total Other
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Donations
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Daily Total Other
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Daily Total Other
                                                                                           //FlowDo                                                                   //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // TotalTotal
                                                                                           //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(70) }); // tape
                                                                                           //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort

                header.Cells.Add(new TableCell(new Paragraph(new Run("Month")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Member Items ")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Member Sales")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Non-Member Items")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Non-Member sales")))
                {
                    FontWeight = FontWeights.Light,

                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Items")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Sales")))
                {
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1)
                });

            }

            FlowDocTableRowGroup = new TableRowGroup();
            FlowDocTableRowGroup.Rows.Add(header);

        }
        private async Task AddTableBody(IList<AlanoCLubProfitLossModel> profitLossModel,string exclude)
        {
            foreach (var item in profitLossModel)
            {
                FlowDocTableRow = new TableRow();
                float tp = 0.0f;
               if(!(await ALanoClubUtilites.RexMatchStr(item.ProductName,exclude)))
                { 
                               Totals.ClubNonMemberPrice += item.ClubNonMemberPrice;
                Totals.ClubPrice += item.ClubPrice;
                Totals.ItemsPerCase += item.ItemsPerCase;
                Totals.CostPerIteam += item.CostPerIteam;
                Totals.ProfitMemnber += item.ProfitMemnber*item.Quantity;
                Totals.ProfitNonMemnber += item.ProfitNonMemnber * item.Quantity;
                Totals.Price += item.Price;
                Totals.TotalPrice += item.TotalPrice;
                Totals.Quantity += item.Quantity;
                Totals.Volume += item.Volume;
                }

                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.ProductName}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.ItemsPerCase}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.Price.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.Quantity}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.Volume.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.CostPerIteam.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalPrice.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.ClubPrice.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
            
               
               
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.ProfitMemnber.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                tp = item.ProfitMemnber * item.Quantity;
                TotalMemP += tp;
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${tp.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.ClubNonMemberPrice.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.ProfitNonMemnber.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                tp = item.ProfitNonMemnber * item.Quantity;
                TotalNonMemP += tp;
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${tp.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
            }
            await AddTableFooter();
        }
        private async Task AddTableFooter()
        {
            FlowDocTableRow = new TableRow();
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Total")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{Totals.ItemsPerCase}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${Totals.Price.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{Totals.Quantity}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{Totals.Volume.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"N/A")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${Totals.TotalPrice.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"N/A")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"N/A")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
           
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${Totals.ProfitMemnber.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"N/A")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${Totals.ProfitNonMemnber.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${TotalNonMemP.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
        }
        public async Task<FlowDocument> AlanoClubCreateProfitLossReport(string title, IList<AlanoCLubProfitLossModel> cLubProfitLossModels,string exclude)
        {
            await InitializeFlowDocument(title);
            await InitializeFlowDocumentTable(true);
            FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);
            await AddTableBody(cLubProfitLossModels,exclude);
            await NetProfit();
            Document = await Utilites.ALanoClubUtilites.AddBlankPar(Document, 1);
         //   CreateBlankLines();
            await Task.CompletedTask;
            return Document;
        }
        }
}
