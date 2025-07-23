using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using DebenuPDFLibraryDLL0915;
using FreeImageAPI;
using System.Drawing.Imaging;
using System.Drawing;
using WordMS = Microsoft.Office.Interop.Word;
using System.Reflection;
//using Tesseract;
using Newtonsoft.Json;
namespace Edocs.Pdf.Converter
{
    public class Pdf
    {
        //private static readonly string tessPath = (@"C:\Program Files (x86)\Tesseract-OCR\tessdata");
        public static readonly string TIFExtension = ".Tif";
        public static readonly string RTFExtension = ".rtf";
        public static readonly string PDFExtension = ".pdf";
        private static readonly string WorkingFolderSaveFiles = @"Temp\e-DocsUsa\Convert\";
        public static Object OMSWORDMISSING = Missing.Value;
        public static Object OMSWORDTRUE = (bool)true;
        public static Object OMSWORDFALSE = (bool)false;
        static WordMS._Document wordDocument; //Word.Document  class
        static WordMS._Application wordApplication; //Word.Application class
        private static string guidFName = string.Empty;
        private static int MinFont = 1;
        private static int MaxFont = 10;
        private static readonly int PdfStandardFont = 0;
        public static string GuidFname
        {
            get
            {
                if (string.IsNullOrWhiteSpace(guidFName))
                    guidFName = Guid.NewGuid().ToString();
                return guidFName;
            }
        }
        public static string WorkingFolder
        {
            get
            {
                return GetWorkingFolder();
            }
        }
        private static string GetWorkingFolder()
        {
            string wFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            wFolder = Path.Combine(wFolder, WorkingFolderSaveFiles);
            if (!(Directory.Exists(wFolder)))
                Directory.CreateDirectory(wFolder);
            return wFolder;
        }
       
        private static async Task IntWordObject()
        {
            if (wordApplication == null)
                wordApplication = new WordMS.ApplicationClass();
            SetWordOptions();
        }
        private static async Task CloseWord()
        {
            try
            {


                object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                object originalFormat = Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
                object routeDocument = false;
                wordDocument.ActiveWindow.Application.Quit(ref saveOption, ref originalFormat, ref routeDocument);
                System.Threading.Thread.Sleep(2000);
                if (wordDocument != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDocument);
                wordDocument = null;
                wordApplication = null;
            }
            catch  { }
        }
      
        public static void ConvertBitMapToPdf(FreeImageBitmap imageBitmap)
        {
             
            ImageTOPdf(imageBitmap);
           
        }
        private static string ChectExtendsion(string extension)
        {
            if (!(extension.StartsWith(".")))
                return $".{extension}";
            return extension;
        }
        public static void ConvertPdfToRtfFile(string fileNameExtenToConvert, string savedFileNameExtension)
        {
            try
            {

            
            IntWordObject().ConfigureAwait(false).GetAwaiter().GetResult();

            fileNameExtenToConvert = ChectExtendsion(fileNameExtenToConvert);
            savedFileNameExtension = ChectExtendsion(savedFileNameExtension);
            IntWordObject().ConfigureAwait(false).GetAwaiter().GetResult();
            Object typeSave = (Object)WordMS.WdSaveFormat.wdFormatRTF;
            Object oSaveAsFileName = (Object)$"{WorkingFolder}{guidFName}{savedFileNameExtension}";
            Object oOpenAsFileName = (Object)$"{WorkingFolder}{guidFName}{fileNameExtenToConvert}";
         
            wordDocument = wordApplication.Documents.Open(oOpenAsFileName, ref OMSWORDFALSE, ref OMSWORDFALSE);
            wordApplication.Selection.Font.Size = 8;
            
            wordApplication.ActiveWindow.Selection.Font.Name = "Arial";
            wordApplication.Documents.Application.Selection.ParagraphFormat.Space1();
            wordApplication.ActiveDocument.SaveAs(ref oSaveAsFileName, ref typeSave);
            CloseWord().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception)
            {
                CloseWord().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        protected static void DrawImage(PDFLibrary pdf, System.Drawing.Bitmap image)
        {

            FreeImageBitmap freeImage = new FreeImageBitmap(image, image.Width, image.Height);
            byte[] data = EncodeImage(freeImage);

            double left = 0;
            double top = 0;
            double width = pdf.PageWidth();
            double height = pdf.PageHeight();

            int imageId = pdf.AddImageFromString(data, (int)PDFLibrary.AddImageOption.Default);
            pdf.DrawImage(left, top, width, height);
            pdf.ReleaseImage(imageId);
        }
        public static void ConvertRtfToPdfFile(string fileNameExtenToConvert, string savedFileNameExtension)
        {
            try 
            { 
            IntWordObject().ConfigureAwait(false).GetAwaiter().GetResult();

            fileNameExtenToConvert = ChectExtendsion(fileNameExtenToConvert);
            savedFileNameExtension = ChectExtendsion(savedFileNameExtension);
            IntWordObject().ConfigureAwait(false).GetAwaiter().GetResult();



            string saveAsFileName = $"{WorkingFolder}{guidFName}{savedFileNameExtension}";
            //object ss = saveAsFileName;
            Object oOpenAsFileName = (Object)$"{WorkingFolder}{guidFName}{fileNameExtenToConvert}";
            wordDocument = wordApplication.Documents.Open(oOpenAsFileName, ref OMSWORDFALSE, ref OMSWORDFALSE);
            if (File.Exists(saveAsFileName))
                File.Delete(saveAsFileName);
            //    Object typeSave = (Object)WordMS.WdSaveFormat.wdFormatPDF;
            //  wordApplication.ActiveDocument.SaveAs(ref ss, ref typeSave);
            wordDocument.ExportAsFixedFormat(saveAsFileName, WordMS.WdExportFormat.wdExportFormatPDF, false);
            System.Threading.Thread.Sleep(2000);
            CloseWord().ConfigureAwait(false).GetAwaiter().GetResult();
            PdfToImage().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception)
            {
                CloseWord().ConfigureAwait(false).GetAwaiter().GetResult();
            }


        }
        //public static void OpenPdfFIle(System.Drawing.Bitmap bitmap)
        //{
        //    string w = WorkingFolder;
        //    bitmap.Save(@"C:\Archives\TestImage\t.TIF", ImageFormat.Tiff);
        //    ImageTOPdf(@"C:\Archives\TestImage\t.TIF");
        //    byte[] vs = ConvertBitMapToByteArray(bitmap);
        //    byte[] vs = ConvertToPdfFile(@"C:\Archives\TestImage\5ac09824-5274-4d10-b7fd-0efa4ab9c5ca.TIF");
        //    byte[] vs = ConvertToPdfFile(@"C:\Archives\TestImage\t.TIF");
        //    File.WriteAllBytes(@"C:\Archives\TestImage\51.pdf", vs);
        //    wordApplication = new WordMS.ApplicationClass();
        //    wordApplication.Visible = true;
        //    wordApplication.DisplayAlerts = WordMS.WdAlertLevel.wdAlertsMessageBox;


        //    object t = false;
        //    wordDocument = wordApplication.Documents.Open(@"C:\Archives\TestImage\5.pdf", ref OMSWORDFALSE, ref OMSWORDFALSE);
        //    Object typeSave = (Object)WordMS.WdSaveFormat.wdFormatRTF;
        //    Object oSaveAsFileName = @"C:\Archives\TestImage\test.rtf";
        //    object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
        //    object originalFormat = Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
        //    object routeDocument = false;
        //    wordApplication.ActiveDocument.SaveAs(ref oSaveAsFileName, ref typeSave);
        //    wordDocument.Close(ref saveOption, ref originalFormat, ref routeDocument);
        //    System.Threading.Thread.Sleep(2000);
        //    wordDocument.ActiveWindow.Application.Quit(ref saveOption, ref originalFormat, ref routeDocument);
        //    System.Threading.Thread.Sleep(2000);
        //    if (wordDocument != null)
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDocument);
        //    wordDocument = null;
        //    wordApplication = null;

        //}
        private static void SetWordOptions()
        {
            try
            {

            
            wordApplication.Application.Options.CheckGrammarAsYouType = false;
            wordApplication.Application.Options.CheckGrammarWithSpelling = false;
            wordApplication.Application.Options.CheckSpellingAsYouType = false;
            wordApplication.Application.Options.CreateBackup = false;
            wordApplication.Application.Options.FormatScanning = false;
            wordApplication.Application.Options.IgnoreUppercase = false;
            wordApplication.Application.Options.SaveNormalPrompt = false;
            wordApplication.Application.Options.SavePropertiesPrompt = false;
            wordApplication.Application.Options.SuggestSpellingCorrections = false;
            wordApplication.DisplayAlerts = WordMS.WdAlertLevel.wdAlertsNone;
            //   wordApplication.ActiveDocument.Application.DisplayAlerts = WordMS.WdAlertLevel.wdAlertsNone;
            wordApplication.Application.Options.BackgroundSave = false;
            wordApplication.Application.Options.CreateBackup = false;
            wordApplication.Application.Options.SaveInterval = 0;
            wordApplication.Application.Options.Pagination = false;
            // wordApplication.Application.DisplayAutoCompleteTips = false;
            wordApplication.Application.DisplayRecentFiles = false;
            //   wordApplication.Application.DisplayScreenTips = false;
            // wordApplication.Application.DisplayScrollBars = false;
            wordApplication.Visible = false;
            //  wordApplication.ScreenUpdating = false;
        
            }
            catch(Exception)
            {
                CloseWord().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            }

        //public static void SavePdfFIle(string rtfFileName)
        //{


        //    wordApplication = new WordMS.ApplicationClass();
        //    wordApplication.Visible = true;
        //    wordApplication.DisplayAlerts = WordMS.WdAlertLevel.wdAlertsMessageBox;

        //    string of = @"C:\Archives\TestImage\danno.pdf";
        //    object t = false;
        //    Object oSaveAsFileName = @"C:\Archives\TestImage\test.rtf";
        //    object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
        //    object originalFormat = Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
        //    object routeDocument = false;
        //    wordDocument = wordApplication.Documents.Open(rtfFileName, ref OMSWORDFALSE, ref OMSWORDFALSE);
        //    wordDocument.ExportAsFixedFormat(of, WordMS.WdExportFormat.wdExportFormatPDF, false);
        //    //Object typeSave = (Object)WordMS.WdSaveFormat.wdFormatRTF;
        //    //Object oSaveAsFileName = @"C:\Archives\TestImage\test.rtf";
        //    //object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
        //    //object originalFormat = Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
        //    //object routeDocument = false;
        //    //wordApplication.ActiveDocument.SaveAs(ref oSaveAsFileName, ref typeSave);
        //    //   wordDocument.Close(ref saveOption, ref originalFormat, ref routeDocument);
        //    System.Threading.Thread.Sleep(2000);
        //    wordDocument.ActiveWindow.Application.Quit(ref saveOption, ref originalFormat, ref routeDocument);
        //    System.Threading.Thread.Sleep(2000);
        //    if (wordDocument != null)
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDocument);
        //    wordDocument = null;
        //    //PdfToImage(of);

        //}
        //protected void DrawImage(PDFLibrary pdf, System.Drawing.Bitmap image)
        //{


        //    byte[] data = EncodeImage(image);

        //    double left = 0;
        //    double top = 0;
        //    double width = pdf.PageWidth();
        //    double height = pdf.PageHeight();

        //    int imageId = pdf.AddImageFromString(data, (int)PDFLibrary.AddImageOption.Default);
        //    pdf.DrawImage(left, top, width, height);
        //    pdf.ReleaseImage(imageId);
        //}
        protected static void SetPageSize(PDFLibrary pdf, SizeF pageSize)
        {

            //Scale the page size to the nearest half inch (if required)



            //Convert to inches and round to nearest half.
            float widthInInches = ImageTools.PointsToInches(pageSize.Width);
            widthInInches = MathExtensions.RoundToNearestDecimal(widthInInches, 0.5M);
            float heightInInches = ImageTools.PointsToInches(pageSize.Height);
            heightInInches = MathExtensions.RoundToNearestDecimal(heightInInches, 0.5M);

            //Convert back to points
            float scaledWidth = ImageTools.InchesToPoints(widthInInches);
            float scaledHeight = ImageTools.InchesToPoints(heightInInches);

            pageSize = new SizeF(scaledWidth, scaledHeight);


            //Finally, set the page size.

            pdf.SetPageDimensions(pageSize.Width, pageSize.Height);
        }
        public static void ImageTOPdf(string imageFName)
        {
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin((int)PDFLibrary.DocumentOrigin.TopLeft);
            pdf.CompressImages((int)PDFLibrary.ImageCompression.None);
            pdf.SetMeasurementUnits((int)PDFLibrary.MeasurementUnit.Points);
            pdf.SetInformation((int)PDFLibrary.DocumentProperty.Creator, "Scanquire");
            pdf.SetInformation((int)PDFLibrary.DocumentProperty.Producer, "E-Docs USA");
            FreeImageBitmap freeImage = new FreeImageBitmap(imageFName);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageFName);
            SizeF imageSizeInPoints = new SizeF()
            {
                Width = ImageTools.PixelsToPoints(freeImage.Width, freeImage.HorizontalResolution),
                Height = ImageTools.PixelsToPoints(freeImage.Height, freeImage.VerticalResolution)
            };

            //  pdf.AddImageFromFile(imageFName, 0);
            // Get width, height of the image
            //    lWidth = si;
            //  int lHeight = pdf.ImageHeight();
            // Reformat the size of the page in the selected document
            SetPageSize(pdf, imageSizeInPoints);
            //pdf.SetPageDimensions(imageSizeInPoints.Width, imageSizeInPoints.Height);
            DrawImage(pdf, freeImage);

            // Draw the image onto the page using the specified width/height
            //  pdf.DrawImage(0, lHeight, lWidth, lHeight);
            // Store the updated PDF where you like
            pdf.AddStandardFont(PdfStandardFont); // Courier
            pdf.SetTextSize(8);
            byte[] b = pdf.SaveToString();
            File.WriteAllBytes($"{WorkingFolder}{GuidFname}{PDFExtension}", b);
            //pdf.SaveToFile($"{WorkingFolder}{GuidFname}{PDFExtension}");
            pdf.ReleaseLibrary();
        }

        public static void ImageTOPdf(FreeImageBitmap freeImage)
        {
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin((int)PDFLibrary.DocumentOrigin.TopLeft);
            pdf.CompressImages((int)PDFLibrary.ImageCompression.None);
            pdf.SetMeasurementUnits((int)PDFLibrary.MeasurementUnit.Points);
            pdf.SetInformation((int)PDFLibrary.DocumentProperty.Creator, "Scanquire");
            pdf.SetInformation((int)PDFLibrary.DocumentProperty.Producer, "E-Docs USA");
            //   FreeImageBitmap freeImage = new FreeImageBitmap(imageFName);
            // System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageFName);
            SizeF imageSizeInPoints = new SizeF()
            {
                Width = ImageTools.PixelsToPoints(freeImage.Width, freeImage.HorizontalResolution),
                Height = ImageTools.PixelsToPoints(freeImage.Height, freeImage.VerticalResolution)
            };

            //  pdf.AddImageFromFile(imageFName, 0);
            // Get width, height of the image
            //    lWidth = si;
            //  int lHeight = pdf.ImageHeight();
            // Reformat the size of the page in the selected document
            SetPageSize(pdf, imageSizeInPoints);
            //pdf.SetPageDimensions(imageSizeInPoints.Width, imageSizeInPoints.Height);
            DrawImage(pdf, freeImage);
            pdf.AddStandardFont(PdfStandardFont); // Courier
            pdf.SetTextSize(8);


            // Draw the image onto the page using the specified width/height
            //  pdf.DrawImage(0, lHeight, lWidth, lHeight);
            // Store the updated PDF where you like
            byte[] b = pdf.SaveToString();
            File.WriteAllBytes($"{WorkingFolder}{GuidFname}{PDFExtension}", b);
            //pdf.SaveToFile($"{WorkingFolder}{GuidFname}{PDFExtension}");
            pdf.ReleaseLibrary();
        }

        private static byte[] ImageToBase64(System.Drawing.Bitmap image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                //byte[] imageBytes = ms.ToArray();

                return ms.ToArray();
            }
        }
        public static async Task<string> GetImageOcrText(System.Drawing.Bitmap freeImage)
        {
            string resultTextString = string.Empty;
            string rResultJsonString = string.Empty;
            try
            {

                // byte[] bitmap= ImageToBase64(freeImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                // FreeImageBitmap bitmap = new FreeImageBitmap(freeImage);


                // byte[] ocrImg = EncodeImageJpg(bitmap);
                byte[] ocrImg = ImageToBase64(freeImage, System.Drawing.Imaging.ImageFormat.Png);
                
                HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(1, 1, 1);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent("OCRP8398898A"), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
            form.Add(new StringContent("eng"), "language");
            form.Add(new StringContent("2"), "OCREngine");
            //    form.Add(new StringContent("1"), "OCREngine");
                form.Add(new StringContent("true"), "isTable");
            form.Add(new ByteArrayContent(ocrImg, 0, ocrImg.Length), "image", "image.png");
                HttpResponseMessage response = httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form).ConfigureAwait(true).GetAwaiter().GetResult(); ;

                resultTextString =  response.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
            Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultTextString);
                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        rResultJsonString = rResultJsonString + ocrResult.ParsedResults[i].ParsedText;
                    }




                }
              //  else
                 //   rResultJsonString = GetImageText(freeImage).ConfigureAwait(true).GetAwaiter().GetResult();
                  
            }
            catch(Exception ex)
            {
               // rResultJsonString = GetImageText(freeImage).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            
            return rResultJsonString;
        }
        //public static async Task<string> GetImageText(System.Drawing.Bitmap freeImage)
        //{
        //    string strText = string.Empty;
        //    try
        //    {
        //        using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
        //        {
        //            using (var img = PixConverter.ToPix((Bitmap)freeImage))
        //            {

        //                using (var page = engine.Process(img, PageSegMode.Auto)) //was (img, pageSegMode))
        //                {
        //                      strText = page.GetText();
        //                  //  strText = page.GetHOCRText(1);//.Replace("/r/n", string.Empty);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        strText = $"Error OCR image message: {ex.Message}";
        //    }
        //    return strText;
        //}
        public static async Task<string> GetPdfText()
        {
            PDFLibrary pdf = new PDFLibrary();
            string strText = string.Empty;
            string pdfFileName = $"{WorkingFolder}{GuidFname}{PDFExtension}";
            pdf.LoadFromFile(pdfFileName, string.Empty);
            int iNumPages = pdf.PageCount(); // Calculate the number of pages
            for (int nPage = 1; nPage <= iNumPages; nPage++)
            {
                byte[] pag = pdf.ExtractFilePageContentToString(pdfFileName, string.Empty, nPage);
                //strText = strText + pdf.ExtractFilePageText(pdfFileName, "", nPage, 0);
                strText = strText + System.Text.ASCIIEncoding.Default.GetString(pag);
            }

            pdf.ReleaseLibrary();
            return strText;
        }
        //   public static async Task<string> GetPdfText(string fileName)
        public static string GetPdfText(string fileName)
        {
            PDFLibrary pdf = new PDFLibrary();
            string strText = string.Empty;
            string pdfFileName = fileName;
            pdf.LoadFromFile(pdfFileName, string.Empty);
            int iNumPages = pdf.PageCount(); // Calculate the number of pages
            for (int nPage = 1; nPage <= iNumPages; nPage++)
            {
                byte[] pag = pdf.ExtractFilePageContentToString(pdfFileName, string.Empty, nPage);
                //strText = strText + pdf.ExtractFilePageText(pdfFileName, "", nPage, 0);
                strText = strText + System.Text.ASCIIEncoding.Default.GetString(pag);
            }

            pdf.ReleaseLibrary();
            return strText;
        }
        public static async Task PdfToImage()
        {

            PDFLibrary pdf = new PDFLibrary();

            string pdfFileName = $"{WorkingFolder}{GuidFname}{PDFExtension}";
            string bmpFileName = $"{WorkingFolder}{GuidFname}{TIFExtension}";
            if (File.Exists($"{WorkingFolder}{GuidFname}{TIFExtension}"))
                File.Delete($"{WorkingFolder}{GuidFname}{TIFExtension}");
            pdf.LoadFromFile(pdfFileName, string.Empty);
            pdf.AddStandardFont(PdfStandardFont); // Courier

            //pdf.SaveToFile(pdfFileName);
            //pdf.LoadFromFile(pdfFileName, string.Empty);
            // Calculate the number of pages
            int iNumPages = pdf.PageCount();
            // Render each page of the document to a separate file.
            // To view the images open the output folder.
            if (iNumPages > 1)
            {
                while (iNumPages > MinFont)
                {
                    pdf.SetTextSize(MaxFont--);
                    iNumPages = pdf.PageCount();
                    if (MaxFont <= MinFont)
                    {
                        pdf.SetTextSize(MinFont);
                        break;
                    }
                }
                // pdf.LoadFromFile(pdfFileName, string.Empty);
                // pdf.AddStandardFont(0); // Courier
                // pdf.SetTextSize(3);
                // pdf.SaveToFile(pdfFileName);
                // pdf.LoadFromFile(pdfFileName, string.Empty);

            }

            pdf.RenderDocumentToFile(300, 1, iNumPages, 0, bmpFileName);
            pdf.SaveToFile(pdfFileName);

            //   pdf.RenderDocumentToFile(300, 1, iNumPages, 0, bmpFileName);
            pdf.ReleaseLibrary();

        }

        public static byte[] TiffToPdf(byte[] tiff)
        {
            PDFLibrary pdf = new PDFLibrary();

            try
            {
                pdf.CompressImages((int)PDFLibrary.ImageCompression.None);
                pdf.SetOrigin((int)PDFLibrary.DocumentOrigin.TopLeft);
                pdf.SetMeasurementUnits((int)PDFLibrary.MeasurementUnit.Points);
                pdf.SetInformation((int)PDFLibrary.DocumentProperty.Creator, "Scanquire");
                pdf.SetInformation((int)PDFLibrary.DocumentProperty.Producer, "E-Docs USA");

                IEnumerator<FreeImageBitmap> imagesEnumerator = GetImagesFromTiff(tiff).GetEnumerator();
                if (imagesEnumerator.MoveNext() == false)
                { throw new Exception("No images in tiff"); }
                DrawImage(pdf, imagesEnumerator.Current);
                while (imagesEnumerator.MoveNext())
                {
                    pdf.NewPage();
                    DrawImage(pdf, imagesEnumerator.Current);
                }
                return pdf.SaveToString();
            }
            finally { pdf.ReleaseLibrary(); }
        }
        public static byte[] ConvertBitMapToByteArray(Bitmap bitmap)
        {
            byte[] result = null;

            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, bitmap.RawFormat);
                result = stream.ToArray();
            }

            return TiffToPdf(result);
        }
        static IEnumerable<FreeImageBitmap> GetImagesFromTiff(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            using (FreeImageBitmap fib = new FreeImageBitmap(stream))
            {
                int frameCount = fib.FrameCount;
                int progressCurrent = 0;
                int progressTotal = frameCount;

                //Loop through the image's frames and yeild a new SQImage for each frame.			
                for (int i = 0; i < frameCount; i++)
                {
                    progressCurrent++;
                    fib.SelectActiveFrame(i);
                    yield return (FreeImageBitmap)(fib.Clone());
                }
            }
        }
        private static byte[] EncodeImageJpg(FreeImageBitmap image)
        {
            FREE_IMAGE_FORMAT imageFormat;
            FREE_IMAGE_SAVE_FLAGS imageSaveFlags;
            imageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
            imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, imageFormat, imageSaveFlags);
                return stream.ToArray();
            }
        }
        private static byte[] EncodeImage(FreeImageBitmap image)
        {
            FREE_IMAGE_FORMAT imageFormat;
            FREE_IMAGE_SAVE_FLAGS imageSaveFlags;
            //TODO: Change to used colors instead of colordepth?
            if (image.ColorDepth == 1)
            {
                imageFormat = FREE_IMAGE_FORMAT.FIF_TIFF;
                imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4;
            }
            else if (image.ColorDepth <= 8)
            {
                imageFormat = FREE_IMAGE_FORMAT.FIF_TIFF;
                imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.TIFF_LZW;
            }
            else
            {
                imageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
                imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, imageFormat, imageSaveFlags);
                return stream.ToArray();
            }
        }

        private static void DrawImage(PDFLibrary pdf, FreeImageBitmap image)
        {

            float imageWidth = ImageTools.PixelsToPoints(image.Width, image.HorizontalResolution);
            float imageHeight = ImageTools.PixelsToPoints(image.Height, image.VerticalResolution);

            //Convert to inches and round to nearest half.
            float widthInInches = ImageTools.PointsToInches(imageWidth);
            widthInInches = MathExtensions.RoundToNearestDecimal(widthInInches, 0.5M);
            float heightInInches = ImageTools.PointsToInches(imageHeight);
            heightInInches = MathExtensions.RoundToNearestDecimal(heightInInches, 0.5M);

            //Convert back to points
            float scaledWidth = ImageTools.InchesToPoints(widthInInches);
            float scaledHeight = ImageTools.InchesToPoints(heightInInches);

            pdf.SetPageDimensions(scaledWidth, scaledHeight);

            byte[] imageData = EncodeImage(image);
            int imageId = pdf.AddImageFromString(imageData, (int)PDFLibrary.AddImageOption.Default);
            pdf.DrawImage(0, 0, pdf.PageWidth(), pdf.PageHeight());
            pdf.ReleaseImage(imageId);
            image.Dispose();
        }
        public static async Task<byte[]> ConvertToPdfFile(string filePath)
        {

            byte[] tiffData = File.ReadAllBytes(filePath); ;
            return TiffToPdf(tiffData);

        }
        public static byte[] ConvertToPdfFile(Image img, System.Drawing.Imaging.ImageFormat imageFormat)
        {


            byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, imageFormat);

                data = memoryStream.ToArray();
            }
            return TiffToPdf(data);

        }


    }

}
