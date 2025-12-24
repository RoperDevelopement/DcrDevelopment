using AlanoClubInventory.Models;
using Microsoft.VisualBasic.Logging;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
namespace AlanoClubInventory.Utilites
{
    public class CreatePDFFileErrorLogs
    {
        private int ImageHeigt { get; set; } = 100;
        private double XPostion { get; set; }
        private double YPostion { get; set; }
        private double TopMargin { get; set; }
        private XSize Size { get; set; }
        private PdfDocument Document { get; set; }
        private PdfPage Page { get; set; }
        private double VerticallyFromTop { get; set; } = 0;
        private double CursorY { get; set; } = 0;
        public async Task CreateErrorLogsPdgFile(IList<ErrorLogEntry> errorLogs,string savePDFFolder)
        {
           await CreatePDFPage(PageOrientation.Landscape, PageSize.A4);
            TopMargin = 50; // Margin from the left 
            CursorY = TopMargin;
            XGraphics gfx = XGraphics.FromPdfPage(Page);
            var bitImg = ALanoClubUtilites.GetImageFromResouceFile("pack://application:,,,/Resources/Images/butteac.png");
             ALanoClubUtilites.SaveBitmapImageToFile(bitImg, Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "ButteACImag.png"));
           
            await AddImage(bitImg, gfx, 50, 50);
            string[] lines = new string[errorLogs.Count + 1];
            var width = Page.Width;
            var height = Page.Height;
           
            //double y = 50; // Margin from the top
            lines[0] =$"Log Date\t Process Info\t Error Text";
            var font = new XFont("Arial", 14);
            double lineHeight = font.GetHeight();
            int lineIndex = 1;

            //WritePdfText(,font, gfx,lineHeight);
            //  await AddPdfDocumentLines(gfx, $"Database Error Report Run Date {DateTime.Now.ToString("MM-dd-yyyy")}", font, lineHeight);
            //await AddPdfDocumentLines(gfx, $"Total Sql Messages {errorLogs.Count()}", font, lineHeight);
            WritePdfText($"Database Error Report Run Date {DateTime.Now.ToString("MM-dd-yyyy")}", font, gfx, lineHeight, XStringFormats.Center);
            WritePdfText($"Total Sql Messages {errorLogs.Count()}", font, gfx, lineHeight, XStringFormats.Center);
            WritePdfText(string.Empty, font, gfx, lineHeight,XStringFormats.TopLeft);
            WritePdfText(string.Empty, font, gfx, lineHeight, XStringFormats.TopLeft);
            foreach (var log in errorLogs)
            {
                //var lt = log.Text.Replace(Utilites.AlanoCLubConstProp.CRLF, " ").Replace("\t", " ");
                //  lines[lineIndex++] = $"{log.LogDate}\t{log.ProcessInfo}\t{lt}";
                //var txt = $"{log.LogDate}";
                WritePdfText($"Log Date:{log.LogDate.ToString()}", font, gfx, lineHeight, XStringFormats.TopLeft);
                WritePdfText($"ProcessInfo:{log.ProcessInfo}log.ProcessInfo", font, gfx, lineHeight, XStringFormats.TopLeft);
                log.Text = $"Message:{log.Text}";
               if (log.Text.Length <= 101)
                   
                {

                    WritePdfText(log.Text, font, gfx, lineHeight, XStringFormats.TopLeft);
                }
               else
                {
                    
                    var lt = log.Text.Substring(0, 101);
                    WritePdfText(lt, font, gfx, lineHeight, XStringFormats.TopLeft);
                    lt = log.Text.Substring(102);
                    if (lt.Length> 100)
                    {
                        WritePdfText(lt.Substring(0,100), font, gfx, lineHeight, XStringFormats.TopLeft);
                        WritePdfText(lt.Substring(101), font, gfx, lineHeight, XStringFormats.TopLeft);
                    }
                    
                    else
                     WritePdfText(lt, font, gfx, lineHeight, XStringFormats.TopLeft);

                }
                
                WritePdfText(string.Empty, font, gfx, lineHeight, XStringFormats.TopLeft);
                //   await AddPdfDocumentLines(gfx, log.LogDate.ToString(), font, lineHeight);
                //  await AddPdfDocumentLines(gfx, log.ProcessInfo, font, lineHeight);
                //  await AddPdfDocumentLines(gfx, log.Text, font, lineHeight);
                //  await AddPdfDocumentLines(gfx, string.Empty, font, lineHeight);
                if (CursorY + lineHeight > Page.Height - TopMargin)
                {
                  gfx=  AddPage(gfx, PageOrientation.Landscape, PageSize.A4);
                }
            }
          //  XPostion = 0;
           //await DrawTableCentered(gfx, lines, 0, font);
            SaveDocument(savePDFFolder);
            DisposePdfDocument();
            OpenPDFFile(savePDFFolder);
        }
        private async Task AddPdfDocumentLines(XGraphics gfx, string text, XFont font, double lineHeight)
        {

            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;

            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);

            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
            CursorY = YPostion;

        }
        public void WritePdfText(string text, XFont font, XGraphics gfx,double lineHeight,XStringFormat xString)
        {
            // Check if we need a new page
           
            gfx.DrawString(text,
                font,
                XBrushes.Black,
                new XRect(TopMargin, CursorY, Page.Width - 2 * TopMargin, lineHeight),
                xString);

            CursorY += lineHeight;
        }

        private async Task CreatePDFPage(PageOrientation pageOrientation, PageSize pageSize)
        {

            var acInfo = await Utilites.ALanoClubUtilites.GetAlnoClubInfo();
            Document = new PdfDocument();
            Document.Info.Title = $"{acInfo.ClubName}";
            Document.Info.Author = $"{acInfo.ClubName} {LoginUserModel.LoginInstance.UserIntils} {LoginUserModel.LoginInstance.UserEmailAddress}";
            Document.Info.Subject = "Recepit";
            Page = Document.AddPage();
            //  page.Size = PageSize.A3;
            Page.Size = pageSize;
            Page.Orientation = pageOrientation;
            //Document.AddPage(Page);

        }
        private XGraphics AddPage(XGraphics gfx,PageOrientation pageOrientation, PageSize pageSize)
        {
            TopMargin = 50;
            CursorY = TopMargin;
            Page = Document.AddPage();
            Page.Size = pageSize;
            Page.Orientation = pageOrientation;
            return XGraphics.FromPdfPage(Page);
        }
        private async void DisposePdfDocument()
        {
            Document.Close();
            Document.Dispose();
        }
        private async void OpenPDFFile(string filePath)
        {
            try
            {
             await  ALanoClubUtilites.StartProcess(filePath, string.Empty, false, true);
                ALanoClubUtilites.ShowMessageBox($"PDF Document Saved under {filePath}", "PDF Document", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ALanoClubUtilites.ShowMessageBox($"Unable to Open PDF file {filePath} {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task AddImage(BitmapImage bitmapImage, XGraphics gfx, double width, double height)
        {
            // using (XGraphics gfx = XGraphics.FromPdfPage(page))
            //  {
            // Convert BitmapImage to Stream
            Stream imageStream = null;
            double x = (Page.Width.Point - Page.Height.Point) / 2;

            // Position vertically from top
            double y = (Page.Width.Point - Page.Height.Point) / 2;
            if (bitmapImage.StreamSource != null)
            {
                imageStream = bitmapImage.StreamSource;
                imageStream.Position = 0;
            }
            else
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                imageStream = new MemoryStream();
                encoder.Save(imageStream);
                imageStream.Position = 0;
            }

            using (XImage xImg = XImage.FromStream(imageStream))
            {
                gfx.DrawImage(xImg, 0, 0, x, y);
            }
            CursorY += y;
            
            // }

        }
        
        private async Task<string> SaveDocument(string recNumber, string recDate)

        {

            var filename = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, $"AlanoClubReceipt_#{recNumber}_RecDate_{recDate}.pdf");
            Utilites.ALanoClubUtilites.DeleteFile(filename);
            Document.Save(filename);
            return filename;
        }


            private async void SaveDocument(string savePDFFolder)

        {
            
            
                Utilites.ALanoClubUtilites.DeleteFile(savePDFFolder);
                Document.Save(savePDFFolder);
            
            //var filename = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, $"AlanoClubReceipt.pdf");

        }


        public async Task DrawTableCentered(XGraphics gfx, string[] lines, int currentLine, XFont font)
        {
            //currentLine++;
            int errorLogCount = 0;
            string[,] table = new string[lines.Length, 3];
            int tr = 0;
            //table[0,0] = lines[currentLine].Replace("\t", ",");



            for (int i = currentLine; i < lines.Length; i++)
            {
                string[] split = lines[i].Split('\t');
                int tc = 0;
                foreach (var str in split)
                {


                    // if(tc <3 )
                    //   table[tr, tc++] = $"{str},";
                    //else
                    if (string.IsNullOrEmpty(str))
                        table[tr, tc] = AlanoCLubConstProp.NA;
                    else
                        table[tr, tc] = $"{str}";
                    tc++;
                }
                tr++;

            }

            double cellWidth = 250;
            double cellHeight = 30;
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            for (int r = 0; r < rows; r++)
            {
                if (string.IsNullOrEmpty(table[r, 0]))
                    break;

                for (int c = 0; c < cols; c++)
                {

                    double x = XPostion + c * cellWidth;
                    double y = YPostion + r * cellHeight;

                    // Draw cell border
                    gfx.DrawRectangle(XPens.Black, x, y, cellWidth, cellHeight);

                    // Center text horizontally
                    XSize size = gfx.MeasureString(table[r, c], font);
                    double textX = x + (cellWidth - size.Width) / 2;
                    double textY = y + (cellHeight - size.Height) / 2 + font.Height;

                    if (string.IsNullOrEmpty(table[r, c]))
                        table[r, c] = AlanoCLubConstProp.NA;
                    gfx.DrawString(table[r, c], font, XBrushes.Black, new XPoint(textX, textY));

                }

            }

            //table[0]=

        }
    }

    
}
