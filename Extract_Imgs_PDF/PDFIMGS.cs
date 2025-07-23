using DebenuPDFLibraryDLL0915;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
using System.IO;
using System.Drawing;

namespace Extract_Imgs_PDF
{
    class PDFIMGS
    {
        static void Main(string[] args)
        {
            SavePDFAsImage(@"D:\AlanoClubTreasure\Property_Tax_ExemptionAppliction\Property_Tax_ExemptionAppliction_2021_v000.pdf", @"L:\Images", string.Empty, 300, "png");
        }
        public static void    SavePDFAsImage(string pdfFileName, string saveImgDir, string pdfPassWord, int imagedpi, string extension)
        {
            try
            { 
            //string retOctText = string.Empty;
            if (!(File.Exists(pdfFileName)))
                throw new Exception($"Error Converting pdf file to image file {pdfFileName} not found");
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            pdf.LoadFromFile(pdfFileName, pdfPassWord);
            int iNumPages = pdf.PageCount();
            pdfFileName = Path.GetFileNameWithoutExtension(pdfFileName);

            //    string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.tif");
            string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.{extension}");


            pdf.RenderDocumentToFile(imagedpi, 1, iNumPages, 0, bmpSaveFileName);
            pdf.ReleaseLibrary();
            }
            catch(Exception ex)
            { Console.WriteLine(ex.Message); }


        }
        public static IEnumerable<byte[]> GetPdfImages(string pdfFile, string pdfPassWord)
        {
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            byte[] b = File.ReadAllBytes(pdfFile);
            pdf.LoadFromString(b, pdfPassWord);
            int pageCount = pdf.PageCount();
            for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
            {
                pdf.SelectPage(currentPageIndex);
                int imageListID = pdf.GetPageImageList(0);
                int imageCount = pdf.GetImageListCount(imageListID);
                if (imageCount > 1)
                    throw new Exception($"More then one image count {imageCount} for pdffile {pdfFile}");

                int imageIndex = 1;
                byte[] imageData = pdf.GetImageListItemDataToString(imageListID, imageIndex, 0);

                Bitmap bitmap = new Bitmap(new MemoryStream(imageData));

                string fileName = @"D:\PDFIMage\test.png";
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                int h = bitmap.Height;
                int w = bitmap.Width;
                //if ((h > 10000) && (w > 10000))
                //    bitmap = ResizeBitmap(bitmap, 10000, 10000);
                //if (h > 10000)
                //    bitmap = ResizeBitmap(bitmap, bitmap.Width, 10000);
                //if (w > 10000)
                //    bitmap = ResizeBitmap(bitmap, 10000, bitmap.Height);
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    ms.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    ms.Close();
                    yield return imageBytes;
                }

                //   

                //bitmap.Save($"D:\\PDFIMage\\{GetNewGuid()}.png", System.Drawing.Imaging.ImageFormat.Png);

            }
            pdf.ReleaseLibrary();

            // int imageCound = pdf.GetImagePageCount(pdfFile)
        }
    }
}
