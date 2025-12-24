using AlanoClubInventory.Models;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Actions;
using PdfSharp.Pdf.Annotations;
using PdfSharp.Quality;
using PdfSharp.Snippets.Font;
using PdfSharp.UniversalAccessibility.Drawing;
using PdfSharpCore.Pdf.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using static ScottPlot.Plottables.Heatmap.RenderStrategies;
//https://docs.pdfsharp.net/PDFsharp/Topics/Start/First-PDF.html#document-creation
namespace AlanoClubInventory.Utilites
{
  public  class CreatePdfDocument
    {
        private int ImageHeigt { get; set; } = 100;
        private double XPostion { get; set; }
        private double YPostion { get; set; }
        private double TopMargin { get; set; }
        private XSize Size { get; set; }
        private PdfDocument Document { get; set; }
        private PdfPage Page { get; set; }
        private double VerticallyFromTop { get; set; } = 0;

        public async Task<string> PDFDocRec(TextRange textRange,string recNumber,string recDate)
        {
            //   WindowsFontResolver windowsFontResolver = new WindowsFontResolver();
            // var fontRes = windowsFontResolver.ResolveTypeface("Arial", false, false);
            // var i = fontRes.FaceName;
            
            await CreatePDFPage(PageOrientation.Landscape, PageSize.A3);
            XGraphics gfx = XGraphics.FromPdfPage(Page);
            var bitImg = ALanoClubUtilites.GetImageFromResouceFile("pack://application:,,,/Resources/Images/butteac.png");
           // ALanoClubUtilites.SaveBitmapImageToFile(bitImg, Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "ButteACImag.png"));

            await AddImage(bitImg,gfx,50,50);
           // page = await CreatePdfWithCenteredImage(page,gfx, Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "ButteACImag.png"), string.Empty);
         
            // Line height
            string fullText = textRange.Text;

            // Split the text into lines
            string[] lines = fullText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var width = Page.Width;
            var height = Page.Height;
            TopMargin = 50; // Margin from the left
            //double y = 50; // Margin from the top

            //  gfx.DrawLine(XPens.Red, 0, 0, width, height);
            //  gfx.DrawLine(XPens.Red, width, 0, 0, height);
            var font = new XFont("Arial", 12);
            double lineHeight = font.GetHeight();

            //  gfx.DrawString($"{acInfo.ClubName}", font, XBrushes.Black,
            //  new XRect(0, 0, x,y), XStringFormats.Center);
            
            int line;
            
            string text =string.Empty;
             
            for (line=0; line < lines.Count(); line++)
            {
                text = lines[line].Trim();
                //if(text.ToLower().StartsWith("facebook"))
                //{
                //    AddPdfAnnotations(gfx, text, font, lineHeight, "https://www.facebook.com/ButteAlanoClub");
                //}
                //else
                //{ 
                    await  AddPdfDocumentLiness(gfx, text, font, lineHeight);
           //     }
                if (text.StartsWith("Recived BY", StringComparison.Ordinal))
                {
                  
                    await AddPdfDocumentLiness(gfx,string.Empty, font, lineHeight);
                    await AddPdfDocumentLiness(gfx,string.Empty, font, lineHeight);

                    
                   
                    break;
                }
                 
            }
            //text = $"Recived By:{LoginUserModel.LoginInstance.UserIntils}";
            //size = gfx.MeasureString(text, font);
            // Center horizontally
            //    x = (page.Width.Point - size.Width) / 2;
            //   y = topMargin + (i++ * lineHeight)+4;
            //  gfx.DrawString(text, font, XBrushes.Black, new XPoint(x, y));
            //x = (page.Width.Point - size.Width) / 2;
            //y = topMargin + (i++ * lineHeight) + 3;
            //page = await CreatePdfWithCenteredImage(page,gfx, Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "signature.png"), string.Empty,topMargin++);
            XPostion = 225;
            await DrawTableCentered(gfx,lines, line,font);
            
            await DrawImageBottomCentered(gfx,Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "signature.png"));
            
           string pdfFileName = await SaveDocument(recNumber,recDate);
            gfx.Dispose();
            DisposePdfDocument();
            OpenPDFFile(pdfFileName);
            return pdfFileName;
           
         
        }
        private async void AddPdfDocumentLine(XGraphics gfx,string text,XFont font,double lineHeight)
        {
            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        }
        private async void AddPdfDocumentLineLeft(XGraphics gfx, string text, XFont font, double lineHeight)
        {
            Size = gfx.MeasureString(text, font);
            // Left align
            XPostion = 50; // Fixed left margin
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        }
        private async void AddPdfDocumentLineRight(XGraphics gfx, string text, XFont font, double lineHeight)
        {
            Size = gfx.MeasureString(text, font);
            // Right align
            XPostion = Page.Width.Point - Size.Width - 50; // Fixed right margin
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        }
        private async void AddPdfDocumentLineJustified(XGraphics gfx, string text, XFont font, double lineHeight)
        {
            Size = gfx.MeasureString(text, font);
            // Justified align
            XPostion = 50; // Fixed left margin
            double rightMargin = Page.Width.Point - 50; // Fixed right margin
            double availableWidth = rightMargin - XPostion;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Split text into words
            string[] words = text.Split(' ');
            double spaceWidth = gfx.MeasureString(" ", font).Width;
            double totalWordsWidth = words.Sum(word => gfx.MeasureString(word, font).Width);
            double totalSpacesWidth = availableWidth - totalWordsWidth;
            double spaceCount = words.Length - 1;
            double adjustedSpaceWidth = spaceCount > 0 ? totalSpacesWidth / spaceCount : spaceWidth;
            double currentX = XPostion;
            // Draw each word with adjusted spacing
            foreach (var word in words)
            {
                gfx.DrawString(word, font, XBrushes.Black, new XPoint(currentX, YPostion));
                currentX += gfx.MeasureString(word, font).Width + adjustedSpaceWidth;
            }
        }
        private async void AddPdfDocumentLineAtPosition(XGraphics gfx, string text, XFont font, double xPosition, double lineHeight)
        {
            Size = gfx.MeasureString(text, font);
            // Specific X position
            XPostion = xPosition;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        }
        private async void AddPdfDocumentLineWithBackground(XGraphics gfx, string text, XFont font, double lineHeight, XColor backgroundColor)
        {
            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw background rectangle
            gfx.DrawRectangle(new XSolidBrush(backgroundColor), XPostion - 5, YPostion - font.Height, Size.Width + 10, font.Height + 5);
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        }
        private async void AddPdfDocumentLineWithBorder(XGraphics gfx, string text, XFont font, double lineHeight, XColor borderColor)
        {
            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw border rectangle
            gfx.DrawRectangle(new XPen(borderColor), XPostion - 5, YPostion - font.Height, Size.Width + 10, font.Height + 5);
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        }
        private async void AddPdfDocumentLineWithRotation(XGraphics gfx, string text, XFont font, double lineHeight, double angle)
        {
            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Save the current state
            gfx.Save();
            // Apply rotation
            gfx.RotateAtTransform(angle, new XPoint(XPostion + Size.Width / 2, YPostion - font.Height / 2));
            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
            // Restore the state
            gfx.Restore();
        }
        //private async void AddPdfAnnotations(XGraphics gfx, string text, XFont font, double lineHeight, string annotationText)
        //{
        //PdfSharp.Pdf.Annotations.PdfLinkAnnotation link = new PdfSharp.Pdf.Annotations.PdfLinkAnnotation();
        //    link.Action = new PdfSharp.Pdf.Actions.PdfUriAction("https://www.facebook.com/ButteAlanoClub");
        //    Size = gfx.MeasureString(text, font);
        //    // Center horizontally
        //    XPostion = (Page.Width.Point - Size.Width) / 2;
        //    // Position vertically from top
        //    YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
        //    // Draw the string
        //    gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion, YPostion));
        //    // Add annotation (tooltip)
        //    var annotationRect = new XRect(XPostion, YPostion - font.Height, Size.Width, font.Height);
        //    var annotation = new PdfSharp.Pdf.Annotations.PdfTextAnnotation
        //    {
        //        Title = "Info",
        //        Contents = annotationText,
        //        Rectangle = new PdfSharp.Pdf.PdfRectangle(annotationRect)
        //    };
        //    Page.Annotations.Add(annotation);
        //}
        private async void AddPdfAnnotationsLink(XGraphics gfx, string text, XFont font, double lineHeight, string annotationText)
        {
            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;
            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);
            // Draw the string
            XRect rect = new XRect(50, 100, 300, 20);
            gfx.DrawString(text, font, XBrushes.Blue,rect, XStringFormats.TopLeft);
            // Add annotation (tooltip)
            // Create a link annotation
            PdfSharp.Pdf.Annotations.PdfLinkAnnotation link = new PdfSharp.Pdf.Annotations.PdfLinkAnnotation();
            link.Rectangle = new PdfSharp.Pdf.PdfRectangle(rect);
            
            // Add annotation to page
            Page.Annotations.Add(link);
        }
        private async void DisposePdfDocument()
        {
            Document.Close();
            Document.Dispose();
        }
        private async Task<string> SaveDocument(string recNumber,string recDate)

        {

            var filename = Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, $"AlanoClubReceipt_#{recNumber}_RecDate_{recDate}.pdf");
            Utilites.ALanoClubUtilites.DeleteFile(filename);
            Document.Save(filename);
            return filename;
        }
        private async Task AddPdfDocumentLiness(XGraphics gfx,string text,XFont font,double lineHeight)
        {
            
            Size = gfx.MeasureString(text, font);
            // Center horizontally
            XPostion = (Page.Width.Point - Size.Width) / 2;

            // Position vertically from top
            YPostion = TopMargin + (VerticallyFromTop++ * lineHeight);

            // Draw the string
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(XPostion,YPostion));

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
                    gfx.DrawImage(xImg, 0,0, x, y);
                }
           // }
           
        }
        private async Task<PdfPage> CreatePdfWithCenteredImage(PdfPage page, XGraphics gfx,string imagePath, string outputPath,double y=50 )
        {
            // Create a new PDF document
            
            // Get graphics object
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            // Load the image
            XImage image = XImage.FromFile(imagePath);

            // Calculate horizontal center
            //  double x = (page.Width - image.PixelWidth * 72.0 / image.HorizontalResolution) / 2;
            double widthInPoints = image.PixelWidth * 72.0 / image.HorizontalResolution;
            double x = (page.Width - widthInPoints) / 2;
            
            gfx.DrawImage(image, x, y);

            // Place at top (y = 0 or a small margin)

            ImageHeigt = image.PixelHeight;

            // Draw the image
            gfx.DrawImage(image, x,y);
            image.Dispose();
            // Save the document
           return page;
        }
        public async Task DrawImageBottomCentered(XGraphics gfx, string imagePath, double marginBottom = 20)
        {
            XImage image = XImage.FromFile(imagePath);
            double widthInPoints = image.PixelWidth * 72.0 / image.HorizontalResolution;
            double heightInPoints = image.PixelHeight * 72.0 / image.VerticalResolution;

            double x = (Page.Width - widthInPoints) / 2;
            double y = Page.Height - heightInPoints - marginBottom;

            gfx.DrawImage(image, x, y);
            image.Dispose();
            
        }
        public async Task DrawTableCentered(XGraphics gfx,string[] lines,int currentLine, XFont font)
        {
             currentLine++;
            string[,] table = new string[lines.Length,4];
            int tr = 0;
            //table[0,0] = lines[currentLine].Replace("\t", ",");
            
            
           
            for (int i = currentLine;i<lines.Length;i++)
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
            
            double cellWidth = 150;
            double cellHeight = 30;
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            for (int r = 0; r < rows; r++)
            {
                if(string.IsNullOrEmpty(table[r,0]))
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
       private async Task CreatePDFPage(PageOrientation pageOrientation,PageSize pageSize)
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

    }
}
// Set once at app startup (or before creating any XFont)
// Then create fonts using the family name (no need to call ResolveTypeface)

// If you must use the resolved face name, ensure the resolver is the global one:
// var fontRes = GlobalFontSettings.FontResolver.ResolveTypeface("Arial", false, false);
// var faceName = fontRes.FaceName; // then use faceName with XFont

