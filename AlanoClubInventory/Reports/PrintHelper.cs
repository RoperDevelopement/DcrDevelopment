using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace AlanoClubInventory.Reports
{
    public static class PrintHelper
    {
        public static void Print(UserControl report)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Convert the UserControl to a FixedDocument for printing
                var document = new FixedDocument();
                var pageContent = new PageContent();
                var fixedPage = new FixedPage();

                fixedPage.Width = printDialog.PrintableAreaWidth;
                fixedPage.Height = printDialog.PrintableAreaHeight;

                // Add the report to the FixedPage
                fixedPage.Children.Add(report);

                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);

                // Print the document
                printDialog.PrintDocument(document.DocumentPaginator, "Report");
            }
        }
        public static bool PrintFlowDocument(FlowDocument report,int padding =20)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                report.PageHeight = printDialog.PrintableAreaHeight;
                report.PageWidth = printDialog.PrintableAreaWidth;
               // report.ColumnGap = 0;
                DpiScale dpi = new DpiScale(96, 96);

                report.FontSize = 10;
               // report.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
                report.SetDpi(dpi);
                report.PagePadding = new Thickness(padding);
               // report.ColumnWidth = double.PositiveInfinity;
                // Create a DocumentPaginator from the FlowDocument
                IDocumentPaginatorSource idpSource = report;
                /// printDialog.PrintDocument(idpSource.DocumentPaginator, "Printing FlowDocument");
                printDialog.PrintDocument(((IDocumentPaginatorSource)report).DocumentPaginator, "My Document");

                return true;
            }
            return false;
        }
    }
    //var report = new ReportView();
    //PrintHelper.Print(report);
}
