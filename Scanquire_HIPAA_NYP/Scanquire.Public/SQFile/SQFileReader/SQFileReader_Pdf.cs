using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Scanquire.Public.Extensions;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using System.Threading;
using System.Drawing;
using DebenuPDFLibraryDLL0915;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{	
    /// <summary>Specialized ISQFileReader for reading PDF files.</summary>
	public class SQFileReader_Pdf : SQFileReaderBase
	{
		private List<string> _OwnerPasswords;
        /// <summary>List of possible owner passwords to apply to the document upon opening.</summary>
        /// <remarks>Currently a placeholder, reader does not handle decryption at this time.</remarks>
        public List<string> OwnerPasswords
		{
			get
			{
                if (_OwnerPasswords == null) _OwnerPasswords = new List<string>();
				return _OwnerPasswords;
			}
		}
		
		public void AddPassword(string password)
		{ OwnerPasswords.Add(Encryption.EncryptToString(password, DataProtectionScope.LocalMachine)); }

        public SQFileReader_Pdf()
		{ 	}

        public virtual IEnumerable<Task<SQImage>> Read(SQFile file, IEnumerable<string> passwords, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Reading PDF file{file.Checksum}{file.FileExtension} using assebmly:{file.ToString()}");
            //If no passwords were provided, apply the instance level passwords.
            if ((passwords == null) || (passwords.Count() == 0))
			{ passwords= OwnerPasswords; }

            PDFLibrary pdf = new PDFLibrary();
            try
            {
                pdf.SetOrigin(0); //Set coordinates to start top left
                ETL.TraceLoggerInstance.TraceInformation($"Loading file{file.Checksum}{file.FileExtension}");
                //TODO: Handle decryption
                pdf.LoadFromString(file.Data, null);

                int pageCount = pdf.PageCount();
                int progressCurrent = 0;
                int progressTotal = pageCount;

                //Loop through the pages and produce a single image for each image encountered.
                for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
                {
                    cToken.ThrowIfCancellationRequested();
                    progressCurrent++;
                    ETL.TraceLoggerInstance.TraceInformation("Reading page " + currentPageIndex);
                    
                    pdf.SelectPage(currentPageIndex);

                    //If a page has one and only one image, we just extract the image.
                    //If it has 0 or multiple images or non-image elements, we need to render it to a single image.
                    bool renderPage = false;

                    int imageListID = pdf.GetPageImageList(0);
                    if (imageListID == 0)
                    {
                        ETL.TraceLoggerInstance.TraceWarning("Page " + currentPageIndex + " does not have an image list");
                        renderPage = true;
                    }

                    ETL.TraceLoggerInstance.TraceInformation("Getting image ItemCount");
                    
                    int imageCount = pdf.GetImageListCount(imageListID);
                    if (imageCount == 0)
                    {
                        ETL.TraceLoggerInstance.TraceWarning("Image list on page " + currentPageIndex + " does not contain any images");
                        renderPage = true;
                    }

                    if (imageCount > 1)
                    {
                        ETL.TraceLoggerInstance.TraceWarning("Image list on page " + currentPageIndex + " does contains multiple images");
                        renderPage = true;
                    }
                    ETL.TraceLoggerInstance.TraceInformation("Checking for non-image elements");

                    //TODO: Implement non-image element check

                    if (renderPage == true)
                    {
                        yield return Task.Factory.StartNew<SQImage>(() =>
                        {
                            //TODO: Implement rendering
                            using (Bitmap bitmap = new Bitmap(2250, 3300))
                            {
                                bitmap.SetResolution(300, 300);
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.DrawString("Unable to render page", SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(300, 300));
                                }
                                bitmap.SetResolution(300, 300);
                                using (FreeImageBitmap fib = new FreeImageBitmap(bitmap))
                                {
                                    SQImage image = new SQImage(fib);
                                    return image;
                                }
                            }
                        });
                    }

                    yield return Task.Factory.StartNew<SQImage>(() =>
                    {
                        int imageIndex = 1;
                        ETL.TraceLoggerInstance.TraceInformation("Extracting image " + imageIndex);
                        byte[] imageData = pdf.GetImageListItemDataToString(imageListID, imageIndex, 0);
                        //byte[] imageData = pdf.GetImageListItemDataToBytes(imageListID, imageIndex, 0);
                        SQImage image = SQImage.FromBytes(imageData);

#if DEBUG

                        using (SQImageEditLock editLock = image.BeginEdit())
                        {
                            ETL.TraceLoggerInstance.TraceInformation("Image Size " + image.WorkingCopy.Width + "," + image.WorkingCopy.Height);
                            ETL.TraceLoggerInstance.TraceInformation("Image Resolution " + image.WorkingCopy.HorizontalResolution + "," + image.WorkingCopy.VerticalResolution);
                        }

#endif

                        return image;
                    });

                    progress.Report(new ProgressEventArgs(progressCurrent, progressTotal));
                }
            }
            finally
            { pdf.ReleaseLibrary(); }
		}

        public override IEnumerable<Task<SQImage>> Read(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            return Read(file, null, progress, cToken); 
        }
	}
}
