using AlanoClubInventory.Models;
using ScottPlot.TickGenerators.TimeUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AlanoClubInventory.Reports
{
    public class MemberShipListFlowDocument
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

        private float TotalAmount { get; set; } = 0;
        private int TotalQuanity {  get; set; }  = 0;
        private async Task InitializeFlowDocument(string title)
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
            Run titleRun = new Run($"\r\n\r\n{title}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 12;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            titleParagraph.Foreground = Brushes.DarkBlue;
            titleRun = new Run($"\r\nReport Created {DateTime.Now.ToString("MM-dd-yyyy")}");
            // Add the title paragraph to the FlowDocument
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
        }
        private async Task InitializeFlowDocumentAcHeader()
        {
            var acHeader = await Utilites.ALanoClubUtilites.GetAlnoClubInfo();
            if (acHeader != null)
            {
                Document = new FlowDocument
                {
                    PagePadding = new Thickness(3, 0, 0, 0),
                    ColumnWidth = double.PositiveInfinity,
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 10

                };
                Paragraph titleParagraph = new Paragraph();
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 12;
                titleParagraph.FontWeight = FontWeights.Bold;
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                //titleParagraph.Foreground = Brushes.DarkBlue;

                // Add the title text
                Run titleRun = new Run($"{acHeader.ClubName}");
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubAddress}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubPOBox}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubCity}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
           
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF} {acHeader.ClubSt}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);

                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubPhone}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.FacebookLink}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                titleParagraph.FontWeight = FontWeights.Bold;
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                titleParagraph.Foreground = Brushes.DarkBlue;
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF} {acHeader.ClubEmail}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);


            }

        }
        private async Task InitializeFlowDocumentAcHeaderPar(string recNumber)
        {
            var acHeader = await Utilites.ALanoClubUtilites.GetAlnoClubInfo();
            if (acHeader != null)
            {
                Document = new FlowDocument
                {
                    PagePadding = new Thickness(3, 0, 0, 0),
                    ColumnWidth = double.PositiveInfinity,
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 10,
                    

                };
                Paragraph titleParagraph = new Paragraph();
                titleParagraph.TextAlignment = TextAlignment.Center;
                titleParagraph.FontSize = 10;
                titleParagraph.Margin = new Thickness(0, 0, 0, 5);
                //titleParagraph.Foreground = Brushes.DarkBlue;
                 
                // Add the title text
                Run titleRun = new Run($"Receipt #{recNumber} {acHeader.ClubName}");
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubAddress}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubPOBox}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubCity}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);

                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.ClubPhone}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}{acHeader.FacebookLink}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
                
                titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF} {acHeader.ClubEmail}");
                // Add the title paragraph to the FlowDocument
                titleParagraph.Inlines.Add(titleRun);
                Document.Blocks.Add(titleParagraph);
              


            }

        }
        private async Task InitializeFlowDocumentTable(IList<FlowDocumentModel> documentModels)
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
        private async Task AddTableRows(string tableRow)
        {
            FlowDocTableRow.Cells.Add(new TableCell(new Paragraph(new Run(tableRow)))
            {
                BorderThickness = new Thickness(1), // Cell border thickness
                BorderBrush = Brushes.Gray, // Cell border color
                Padding = new Thickness(1) // Padding inside the cell
            });

        }
        private async Task CreateReport(IList<AlanoCLubMembersModel> cLubMembersModels)
            {
            foreach (var item in cLubMembersModels)
            {
                FlowDocTableRow = new TableRow();
                await AddTableRows(item.MemberID.ToString());
                await AddTableRows(item.MemberFirstName);
                await AddTableRows(item.MemberLastName);
                await AddTableRows(item.MemberEmail);
                await AddTableRows(item.MemberPhoneNumber);
                await AddTableRows(item.SobrietyDate.ToString("MM-dd-yyyy"));
                await AddTableRows(item.IsActiveMember.ToString());
                await AddTableRows(item.IsBoardMember.ToString());
                ;

                FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
            }
            FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);
        }
        private async Task RecInformaiton(AlanoCLubMembersModel alanoCLub, DateTime recDate)

        {
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.FontSize = 10;
            titleParagraph.Margin = new Thickness(0, 0, 0, 5);
            //titleParagraph.Foreground = Brushes.DarkBlue;

            // Add the title text
            Run titleRun = new Run($"Customer's Member ID {alanoCLub.MemberID} Receipt Date {recDate.ToString("MM-dd-yyyy")}" );
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
              titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}Customer Name {alanoCLub.MemberFirstName} {alanoCLub.MemberLastName}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);

            titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF}Customer Email address {alanoCLub.MemberEmail}");
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);
            titleRun = new Run($"{Utilites.AlanoCLubConstProp.CRLF} Recived BY: {LoginUserModel.LoginInstance.UserIntils}");
            // Add the title paragraph to the FlowDocument
            titleParagraph.Inlines.Add(titleRun);
            Document.Blocks.Add(titleParagraph);

        }
        private async Task CreateRec(IList<PayDuesModel> rec)
        {
            foreach (var item in rec)
            {
                
                FlowDocTableRow = new TableRow();
                await AddTableRows(item.Quanity.ToString());
                await AddTableRows(item.Description);
                await AddTableRows($"${item.Price.ToString("0.00")}");
                await AddTableRows($"${item.Amount.ToString("0.00")}");
                
                FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
                TotalAmount += item.Amount;
                TotalQuanity += item.Quanity;
            }
            
        }
        private async Task AddTableFooter()
        {
            FlowDocTableRow = new TableRow();
            await AddTableRows(TotalQuanity.ToString());
            await AddTableRows(string.Empty);
            await AddTableRows("TOTAL");
            await AddTableRows($"${TotalAmount.ToString("0.00")}");
            FlowDocTableRowGroup.Rows.Add(FlowDocTableRow);
          //  FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);

        }
        public async Task<FlowDocument> MemberShipList(IList<AlanoCLubMembersModel> cLubMembersModels,string title,IList<FlowDocumentModel> documentModels)
        {
            await InitializeFlowDocument(title);
            await InitializeFlowDocumentTable(documentModels);
            await CreateReport(cLubMembersModels);
            return Document;
        }
        public async Task<FlowDocument> Receipt(IList<PayDuesModel> duesModels, string title, IList<FlowDocumentModel> documentModels,DateTime reciptDate,AlanoCLubMembersModel alanoCLubMembers,int recNum)
        {
            // await InitializeFlowDocument(title);
            await InitializeFlowDocumentAcHeaderPar(recNum.ToString());
            await RecInformaiton(alanoCLubMembers,reciptDate);
            await InitializeFlowDocumentTable(documentModels);
            await CreateRec(duesModels);
            await AddTableFooter();
            FlowDocTable.RowGroups.Add(FlowDocTableRowGroup);
            //  await CreateReport(cLubMembersModels);
            //  await  InitializeFlowDocumentAcHeader();

            return Document;
        }
    }
}
