using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlanoClubInventory.Utilites
{
    public class CreateFlowDocument
    {
        private FlowDocument Document { get; set; }
        private Table FlowDocTable { get; set; }
        private TableRowGroup FlowDocTableRowGroup { get; set; }
        private TableRow FlowDocTableHeader { get; set; }
        private TableRow FlowDocTableRow { get; set; }
        private TableRow FlowDocTableBottomRow { get; set; }
        private TableColumn FlowDocTableColumn { get; set; }
        private Paragraph TitleParagraph { get; set; }
        private Run TitleRun { get; set; }
        private float TotalMemeberPrice { get; set; } = 0;
        private float TotalNonMemeberPrice { get; set; } = 0;
        private int TotalNonMemberItems { get; set; } = 0;
        private int TotalMemberItems { get; set; } = 0;
        private IList<AlanoCLubMontlyProductsSoldModel> TotalItemsSold { get; set; } = new List<AlanoCLubMontlyProductsSoldModel>();
        private AlanoCLubMontlyProductsSoldModel SoldByMonthItems { get; set; } = new AlanoCLubMontlyProductsSoldModel();
        private int TotalItems { get; set; } = 0;
        private float TotalCost { get; set; } = 0;
        private float TotalMemeberMonthlyPrice { get; set; } = 0;
       
        private async Task InitializeFlowDocument(string title)
        {
            Document = new FlowDocument
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
            Document.Blocks.Add(titleParagraph);
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 24;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            titleParagraph.Foreground = Brushes.DarkBlue;
            titleRun = new Run($"\r\nReport Created {DateTime.Now.ToString("MM-dd-yyyy")}");
            // Add the title paragraph to the FlowDocument
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
        }
        private async Task InitializeFlowDocumentTable(bool mainRep)
        {


            FlowDocTable = new Table
            {
                CellSpacing = 0, // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Black // Outer border color
            };
            Document.Blocks.Add(FlowDocTable);
            var header = new TableRow();
            if (mainRep)
            { 
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // month
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // pn
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // member items
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // member price
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Rent
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Coins
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Donations
            FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(100) }); // Daily Total Other
            //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // TotalTotal
            //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(70) }); // tape
            //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort
           
            header.Cells.Add(new TableCell(new Paragraph(new Run("Month"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Procuct Name"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Member Items "))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Member Total Cost"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Non-Member Items"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Non-Member Total Cost"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Total Items"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Total Cost"))) { FontWeight = FontWeights.Bold });

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
                                                                                           //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // TotalTotal
                                                                                           //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(70) }); // tape
                                                                                           //FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort

                header.Cells.Add(new TableCell(new Paragraph(new Run("Month"))) { FontWeight = FontWeights.Bold });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Member Items "))) { FontWeight = FontWeights.Bold });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Member Sales"))) { FontWeight = FontWeights.Bold });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Non-Member Items"))) { FontWeight = FontWeights.Bold });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Non-Member Sles"))) { FontWeight = FontWeights.Bold });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Items"))) { FontWeight = FontWeights.Bold });
                header.Cells.Add(new TableCell(new Paragraph(new Run("Total Sales"))) { FontWeight = FontWeights.Bold });

            }

            FlowDocTableRowGroup = new TableRowGroup();
            FlowDocTableRowGroup.Rows.Add(header);

        }
        public async Task<FlowDocument> CreateAlanoMontlyInvReport(string title, IList<AlanoCLubMontlyProductsSoldModel> alanoCLubMontlySales)
        {
            await InitializeFlowDocument(title);
            await InitializeFlowDocumentTable(true);
            FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);
            var sorted = alanoCLubMontlySales.OrderBy(p => p.Month).ToList();
            var stMon = sorted[0].Month;
            
            foreach (var item in sorted)
            {
                 
                float    tPrice = item.TotalMemBerPrice+item.TotalNonMemBerPrice;
                int tItems = item.TotalMemBerProductsSold+item.TotalNonMemBerProductsSold   ;
                if (stMon != item.Month)
                {
                    TotalItemsSold.Add(new AlanoCLubMontlyProductsSoldModel
                    {
                        Month = stMon,
                        TotalMemBerPrice = SoldByMonthItems.TotalMemBerPrice,
                        TotalNonMemBerPrice = SoldByMonthItems.TotalNonMemBerPrice,
                        TotalMemBerProductsSold = SoldByMonthItems.TotalMemBerProductsSold,
                        TotalNonMemBerProductsSold = SoldByMonthItems.TotalNonMemBerProductsSold,



                    });
                    SoldByMonthItems.TotalMemBerPrice = 0.0f;
                    SoldByMonthItems.TotalNonMemBerPrice = 0.0f;
                    SoldByMonthItems.TotalMemBerProductsSold = 0;
                    SoldByMonthItems.TotalNonMemBerProductsSold = 0;

                   
                }

                FlowDocTableRow   = new TableRow();
                var month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Month);
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{month}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.ProductName}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.TotalMemBerProductsSold}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalMemBerPrice:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{item.TotalNonMemBerProductsSold}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalNonMemBerPrice:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{tItems}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${tPrice:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });

                TotalMemeberPrice += item.TotalMemBerPrice;
               
                TotalNonMemeberPrice += item.TotalNonMemBerPrice;
                TotalNonMemberItems += item.TotalNonMemBerProductsSold;
                TotalMemberItems += item.TotalMemBerProductsSold;
                TotalItems += tItems;
                TotalCost += tPrice;

                SoldByMonthItems.TotalMemBerPrice += item.TotalMemBerPrice;
                SoldByMonthItems.TotalNonMemBerPrice += item.TotalNonMemBerPrice; 
                SoldByMonthItems.TotalMemBerProductsSold  += item.TotalMemBerProductsSold;
                SoldByMonthItems.TotalNonMemBerProductsSold += item.TotalNonMemBerProductsSold;

                stMon = item.Month;
                FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
            }
            TotalItemsSold.Add(new AlanoCLubMontlyProductsSoldModel
            {
                Month = stMon,
                TotalMemBerPrice = SoldByMonthItems.TotalMemBerPrice,
                TotalNonMemBerPrice = SoldByMonthItems.TotalNonMemBerPrice,
                TotalMemBerProductsSold = SoldByMonthItems.TotalMemBerProductsSold,
                TotalNonMemBerProductsSold = SoldByMonthItems.TotalNonMemBerProductsSold,



            });
            await AddFooter();
            await InitializeFlowDocumentTable(false);
            FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);
           await AddTotalsPerMonth();
            return Document;
        }
        private async Task AddTotalsPerMonth()
        {
            foreach (var items in TotalItemsSold)
            {
                FlowDocTableRow = new TableRow();
                TotalItems = items.TotalMemBerProductsSold+items.TotalNonMemBerProductsSold;
                TotalCost =items.TotalMemBerPrice+items.TotalNonMemBerPrice;
                var month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(items.Month);
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{month}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{items.TotalMemBerProductsSold}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${items.TotalMemBerPrice.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{items.TotalNonMemBerProductsSold}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${items.TotalNonMemBerPrice.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{TotalItems}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${TotalCost.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
            }
        }
        private async Task AddFooter()
        {

            FlowDocTableRow = new TableRow();
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run("")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run("")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{TotalMemberItems}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${TotalMemeberPrice:F2}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{TotalNonMemberItems}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${TotalNonMemeberPrice:F2}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"{TotalItems}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run($"${TotalCost:F2}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            
            FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);

        }
        #region Stwert rep
        public async Task<FlowDocument> CreateAlanoMonthlyStwwertReport(IList<AlanClubPrintReportModel> printReportModels, string title, IList<AlanoCLubMontlySalesDepositsModel> alanoCLubMontlySales)
        {
            AlanoClubReportTotals clubReportTotals = new AlanoClubReportTotals();
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
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 24;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            titleParagraph.Foreground = Brushes.DarkBlue;
            titleRun = new Run($"\r\nReport Created {DateTime.Now.ToString("MM-dd-yyyy")}");
            // Add the title paragraph to the FlowDocument
            titleParagraph.Inlines.Add(titleRun);
            doc.Blocks.Add(titleParagraph);
            var table = new Table
            {
                CellSpacing = 0, // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Black // Outer border color
            };
            doc.Blocks.Add(table);

            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });

            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // date
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // bar
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // books
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // cofee club
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Rent
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Coins
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Donations
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Daily Total Other
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // TotalTotal
            table.Columns.Add(new TableColumn { Width = new GridLength(70) }); // tape
            table.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort
            var header = new TableRow();
            header.Cells.Add(new TableCell(new Paragraph(new Run("Date"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Bar Item"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Dues"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Cofee Club"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Donations"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Misc Items"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Daily Total Other"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Total Tape"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Total Dropped"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Till Over/Short"))) { FontWeight = FontWeights.Bold });



            var rowGroup = new TableRowGroup();
            rowGroup.Rows.Add(header);

            foreach (var item in printReportModels)
            {
                var row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.TillDate.ToString("MM-dd-yyyy"))))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });


                clubReportTotals.BarItemsTotal += item.BarItemsTotal;
                // clubReportTotals.DailyTotalOther=0.0f;
                clubReportTotals.Dues += item.Dues;
                clubReportTotals.CoffeeClub += item.CoffeeClub;
                clubReportTotals.Donations += item.Donations;
                // clubReportTotals.DailyTotalOther += item.DailyTotalOther;
                clubReportTotals.TotalSales += item.TotalSales;
                clubReportTotals.Tape += item.Tape;
                clubReportTotals.OverShort += item.OverShort;
                clubReportTotals.DailyTotalOther += item.CoffeeClub + item.Dues + item.Donations + item.MiscItems;

                clubReportTotals.MiscItems += item.MiscItems;
                float otherTotal = item.CoffeeClub + item.Dues + item.Donations + item.MiscItems;
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.BarItemsTotal:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });

                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.Dues:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.CoffeeClub:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.Donations:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.MiscItems:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${otherTotal:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalSales:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.Tape:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                if (item.OverShort < 0)
                {
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.OverShort:F2}")))
                    {
                        BorderThickness = new Thickness(1), // Cell border thickness
                        BorderBrush = Brushes.Gray, // Cell border color
                        Padding = new Thickness(1), // Padding inside the cell
                        Background = Brushes.PaleVioletRed,
                        FontWeight = FontWeights.Bold

                    });
                }
                else
                {
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.OverShort:F2}")))
                    {
                        BorderThickness = new Thickness(1), // Cell border thickness
                        BorderBrush = Brushes.Gray, // Cell border color
                        Padding = new Thickness(1), // Padding inside the cell
                        Background = Brushes.LightGreen,
                        FontWeight = FontWeights.Bold
                    });
                }

                rowGroup.Rows.Add(row);
            }


            // Add a bottom row for totals
            var rowBotton = new TableRow();
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            //{
            //    BorderThickness = new Thickness(1), // Cell border thickness
            //    BorderBrush = Brushes.Gray, // Cell border color
            //    Padding = new Thickness(1) // Padding inside the cell
            //});
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.BarItemsTotal.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.Dues.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.CoffeeClub.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.Donations.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.MiscItems.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.DailyTotalOther.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.TotalSales.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.Tape.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            if (clubReportTotals.OverShort < 0)
            {
                rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.OverShort.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1), // Padding inside the cell
                    Background = Brushes.PaleVioletRed,
                    FontWeight = FontWeights.Bold
                });

            }
            else
            {
                rowBotton.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.OverShort.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1), // Padding inside the cell
                    Background = Brushes.LightGreen,
                    FontWeight = FontWeights.Bold
                });
            }
            //rowBotton.Cells.Add(new TableCell(new Paragraph(new Run("OverShort")))
            //{
            //    BorderThickness = new Thickness(1), // Cell border thickness
            //    BorderBrush = Brushes.Gray, // Cell border color
            //    Padding = new Thickness(1) // Padding inside the cell
            //});
            rowGroup.Rows.Add(rowBotton);
            table.RowGroups.Add(rowGroup);

            doc.Blocks.Add(table);
            var tableFooter = new Table
            {
                CellSpacing = 0, // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Black, // Outer border color
                Padding = new Thickness(1),
                
                
            };
            doc.Blocks.Add(tableFooter);
            //string fullMonthName = date.ToString("MMMM", CultureInfo.InvariantCulture);
            tableFooter.Columns.Add(new TableColumn { Width = new GridLength(60) }); // date
            tableFooter.Columns.Add(new TableColumn { Width = new GridLength(75) }); // Tape Total
            tableFooter.Columns.Add(new TableColumn { Width = new GridLength(75) }); // cash total
            tableFooter.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort
            tableFooter.Columns.Add(new TableColumn { Width = new GridLength(75) }); // overshort
            var headerFooter = new TableRow();
            headerFooter.Cells.Add(new TableCell(new Paragraph(new Run("Month"))) { FontWeight = FontWeights.Bold });
            headerFooter.Cells.Add(new TableCell(new Paragraph(new Run("Total Tape"))) { FontWeight = FontWeights.Bold });
            headerFooter.Cells.Add(new TableCell(new Paragraph(new Run("Total Droped"))) { FontWeight = FontWeights.Bold });
            headerFooter.Cells.Add(new TableCell(new Paragraph(new Run("Total Depsoit"))) { FontWeight = FontWeights.Bold });
            headerFooter.Cells.Add(new TableCell(new Paragraph(new Run("OverShort"))) { FontWeight = FontWeights.Bold });
            var rowGroupFooter = new TableRowGroup();
            rowGroupFooter.Rows.Add(headerFooter);
            clubReportTotals.OverShort = 0;
            clubReportTotals.TotalSales = 0;
            clubReportTotals.Tape = 0;
            clubReportTotals.Deposit = 0;
            foreach (var item in alanoCLubMontlySales)
            {
                var row = new TableRow();
                var month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Month);
                row.Cells.Add(new TableCell(new Paragraph(new Run(month)))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalTillSale:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalTape:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                row.Cells.Add(new TableCell(new Paragraph(new Run($"${item.TotalDeposit:F2}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1) // Padding inside the cell
                });
                float os =  item.TotalTape  - item.TotalTillSale;
                if (os < 0)
                {
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"${os:F2}")))
                    {
                        BorderThickness = new Thickness(1), // Cell border thickness
                        BorderBrush = Brushes.Gray, // Cell border color
                        Padding = new Thickness(1), // Padding inside the cell
                        Background = Brushes.OrangeRed,
                        FontWeight = FontWeights.Bold
                    });
                }
                else
                {
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"${os:F2}")))
                    {
                        BorderThickness = new Thickness(1), // Cell border thickness
                        BorderBrush = Brushes.Gray, // Cell border color
                        Padding = new Thickness(1), // Padding inside the cell
                        Background = Brushes.LightGreen,
                        FontWeight = FontWeights.Bold
                    });
                }
                clubReportTotals.TotalSales += item.TotalTillSale;
                clubReportTotals.Tape += item.TotalTape;
                clubReportTotals.OverShort += os;
                clubReportTotals.Deposit += item.TotalDeposit;
                rowGroupFooter.Rows.Add(row);
            }



            var rowMS = new TableRow();
            rowMS.Cells.Add(new TableCell(new Paragraph(new Run($"Monthly Totals")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowMS.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.TotalSales.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });
            rowMS.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.Tape.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell

            });
            rowMS.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.Deposit.ToString("0.00")}")))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell

            });
            if (clubReportTotals.OverShort < 0)
            {
                rowMS.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.OverShort.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1), // Padding inside the cell
                    Background = Brushes.Red,
                    FontWeight = FontWeights.Bold
                });
            }
            else
            {
                rowMS.Cells.Add(new TableCell(new Paragraph(new Run($"${clubReportTotals.OverShort.ToString("0.00")}")))
                {
                    BorderThickness = new Thickness(1), // Cell border thickness
                    BorderBrush = Brushes.Gray, // Cell border color
                    Padding = new Thickness(1), // Padding inside the cell
                    Background = Brushes.LightGreen
                });
            }

            rowGroupFooter.Rows.Add(rowMS);
            tableFooter.RowGroups.Add(rowGroupFooter);
            doc.Blocks.Add(tableFooter);
            return doc;
        }
        #endregion
    }
}
