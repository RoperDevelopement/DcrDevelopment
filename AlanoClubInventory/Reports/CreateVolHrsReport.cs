using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AlanoClubInventory.Reports
{
    public class CreateVolHrsReport
    {
       public event EventHandler<double> UpdateProgessBar;
        public event EventHandler<bool> ShowHideProgessBar;
        private FlowDocument Document { get; set; }
        private Table FlowDocTable { get; set; }
        private TableRowGroup FlowDocTableRowGroup { get; set; }
        private TableRow FlowDocTableHeader { get; set; }
        private TableRow FlowDocTableRow { get; set; }
        private TableRow FlowDocTableBottomRow { get; set; }
        private TableColumn FlowDocTableColumn { get; set; }
        private Paragraph TitleParagraph { get; set; }
        private Run TitleRun { get; set; }
        private double pBar { get; set; } = 10.00;
        private double TotalHrs { get; set; } = 0;
        private double TotalVolHrs { get; set; } = 0;
        private async Task CreateNewFlowDoc(DateTime sDate,DateTime eDate)
        {
            Document = new FlowDocument
            {
                PagePadding = new Thickness(3, 0, 0, 0),
                ColumnWidth = double.PositiveInfinity,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 12

            };
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 12;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            //titleParagraph.Foreground = Brushes.DarkBlue;

            // Add the title text
            Run titleRun = new Run($"Vol Hours Report for {sDate.ToString("MM-dd-yyyy")}-{{eDate.ToString(\"MM-dd-yyyy)}}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
        }
        private async void CreateNewTable()
        {
            FlowDocTable = new Table
            {
                CellSpacing = 1, // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Gray, // Outer border color
                FontSize = 12,
                Margin = new Thickness(10)




            };
            Document.Blocks.Add(FlowDocTable);
        }
        private async Task AddNewTableRows(string tableRow)
        {
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run(tableRow)))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

        }
        private async Task CreatNewTable(IList<FlowDocumentModel> documentModels)
        {
            FlowDocTable = new Table
            {
                CellSpacing = 1, // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = Brushes.Gray, // Outer border color
                FontSize = 12,
                Margin = new Thickness(10),
                TextAlignment = TextAlignment.Center,




            };
            Document.Blocks.Add(FlowDocTable);
            var header = new TableRow();
            for (int i = 0; i < documentModels.Count; i++)
            {
                int gl = documentModels[i].GridLength;
                FlowDocTable.Columns.Add(new TableColumn { Width = new GridLength(gl) });
            }
            for (int i = 0; i < documentModels.Count; i++)
            {
                header.Cells.Add(new TableCell(new Paragraph(new Run($"{documentModels[i].TableHeader}")))
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
         
        private async void AddNewPar(string parHeader)
        {
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 14;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            //titleParagraph.Foreground = Brushes.DarkBlue;

            // Add the title text
            Run titleRun = new Run($"{parHeader}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);

        }
        private async Task AddTableRows(string tableRow)
        {
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run(tableRow)))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

        }
        private async Task CreateRepByVol(string vol, IList<VolunteerHoursModel> volunteerHours, IList<FlowDocumentModel> documentModels)
        {
            TotalHrs = 0;
                var minDate = volunteerHours.Where(p=>p.UserName==vol).Select(g=>g.DateTimeClockedIn).FirstOrDefault();
            var maxDate = volunteerHours.Where(p => p.UserName == vol).Select(g => g.DateTimeClockedIn).Last();
            string title = $"Vol Hours Report For {vol} date range {minDate.ToString("MM-dd-yyyy")} - {maxDate.ToString("MM-dd-yyyy")}";
            AddNewPar(title);
           await CreatNewTable(documentModels);


            var hrsVol = volunteerHours.Where(p=>p.UserName== vol).ToList();
            foreach(var v in hrsVol)
            {
                FlowDocTableRow = new TableRow();
                await AddTableRows(v.UserName);
                await AddTableRows(v.UserFirstName);
                await AddTableRows(v.UserLastName);
                await AddTableRows(v.DateTimeClockedIn.ToString());

                await AddTableRows(v.DateTimeClockedOut.ToString());
                if (v.DateTimeClockedOut.Year < 2000)
                {
                    await AddTableRows("Not CLocked Out");
                }
                else
                await AddTableRows(v.TotalHours.ToString());
                TotalHrs += v.TotalHours;
                FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
            }
            FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);
            TotalVolHrs += TotalHrs;
            AddTableFooter();

        }
        private async void AddTableFooter()
        {
            FlowDocTableRow = new TableRow();
            await AddTableRows("Total Vol Hrs");
            await AddTableRows(string.Empty);
            await AddTableRows(string.Empty);
            await AddTableRows(string.Empty);
            await AddTableRows(string.Empty);
            await AddTableRows($"{TotalHrs.ToString("0.00")}");
            FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
            //  FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);

        }
        public async Task<FlowDocument> VolReport(IList<VolunteerHoursModel> volunteerHours, DateTime sDate, DateTime eDate, IList<FlowDocumentModel> documentModels)
        {
            OnShowHideProgessBar(true);
           await CreateNewFlowDoc(sDate, eDate);
            OnUpdateProgessBar(pBar);
           
            var volEmail = volunteerHours.GroupBy(test => test.UserName).Select(grp => grp.First()).ToList();
            //foreach (var vol in volunteerHours.OrderBy(o => o.UserEmailAddress))
            foreach(var vhrs in volEmail) 
            {
               await CreateRepByVol(vhrs.UserName, volunteerHours, documentModels);
            }
           Document = await Utilites.ALanoClubUtilites.AddBlankPar(Document, 3);
            AddNewPar($"Total Vol Hours {TotalVolHrs.ToString("0.00")} for Date Range {sDate.ToString("MM-dd-yyyy")} to {eDate.ToString("MM-dd-yyyy")}");
            OnShowHideProgessBar(false);
            return Document;
        }
        protected virtual void OnUpdateProgessBar(double count) => UpdateProgessBar?.Invoke(this, count);
        protected virtual void OnShowHideProgessBar(bool showHide) => ShowHideProgessBar?.Invoke(this, showHide);
        
    }
}
