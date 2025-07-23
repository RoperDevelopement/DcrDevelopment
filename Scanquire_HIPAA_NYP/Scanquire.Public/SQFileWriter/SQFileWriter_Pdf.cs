/*
 * User: Sam Brinly
 * Date: 2/14/2013
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Scanquire.Public.Extensions;
using System.Threading.Tasks;
using System.Threading;
using DebenuPDFLibraryDLL0915;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    public class SQFileWriter_Pdf : SQFileWriterBase
    {
        #region Properties

        private string _FileExtension = PathExtensions.DotExtension("pdf");
        public override string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = PathExtensions.DotExtension(value); }
        }


        private PDFLibrary.Encryption _Encryption = PDFLibrary.Encryption.NONE;
        public PDFLibrary.Encryption Encryption
        {
            get { return _Encryption; }
            set { _Encryption = value; }
        }

        private PDFLibrary.Permission _Permissions = PDFLibrary.Permission.ALL;
        public PDFLibrary.Permission Permissions
        {
            get { return _Permissions; }
            set { _Permissions = value; }
        }

        public string OwnerPassword { get; set; }

        public string UserPassword { get; set; }

        private FREE_IMAGE_FORMAT _MonochromeImageFormat = FREE_IMAGE_FORMAT.FIF_TIFF;
        public FREE_IMAGE_FORMAT MonochromeImageFormat
        {
            get { return _MonochromeImageFormat; }
            set { _MonochromeImageFormat = value; }
        }

        private FREE_IMAGE_SAVE_FLAGS _MonochromeImageSaveFlags = FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4;
        public FREE_IMAGE_SAVE_FLAGS MonochromeImageSaveFlags
        {
            get { return _MonochromeImageSaveFlags; }
            set { _MonochromeImageSaveFlags = value; }
        }

        private FREE_IMAGE_FORMAT _GreyscaleImageFormat = FREE_IMAGE_FORMAT.FIF_PNG;
        public FREE_IMAGE_FORMAT GreyscaleImageFormat
        {
            get { return _GreyscaleImageFormat; }
            set { _GreyscaleImageFormat = value; }
        }

        private FREE_IMAGE_SAVE_FLAGS _GreyscaleImageSaveFlags = FREE_IMAGE_SAVE_FLAGS.PNG_Z_BEST_COMPRESSION;
        public FREE_IMAGE_SAVE_FLAGS GreyscaleImageSaveFlags
        {
            get { return _GreyscaleImageSaveFlags; }
            set { _GreyscaleImageSaveFlags = value; }
        }

        private FREE_IMAGE_FORMAT _ColorImageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
        public FREE_IMAGE_FORMAT ColorImageFormat
        {
            get { return _ColorImageFormat; }
            set { _ColorImageFormat = value; }
        }

        private FREE_IMAGE_SAVE_FLAGS _ColorImageSaveFlags = (FREE_IMAGE_SAVE_FLAGS)90;
        public FREE_IMAGE_SAVE_FLAGS ColorImageSaveFlags
        {
            get { return _ColorImageSaveFlags; }
            set { _ColorImageSaveFlags = value; }
        }

        private bool _ScalePagesToNearestHalfInch = true;
        public bool ScalePagesToNearestHalfInch
        {
            get { return _ScalePagesToNearestHalfInch; }
            set { _ScalePagesToNearestHalfInch = value; }
        }

        #endregion Properties

        protected void InitializeDocument(PDFLibrary pdf, ref string ownerPassword, ref string userPassword)
        {
            ETL.TraceLoggerInstance.TraceInformation("Creating an PDF document");
            pdf.SetOrigin((int)PDFLibrary.DocumentOrigin.TopLeft);
            pdf.CompressImages((int)PDFLibrary.ImageCompression.None);
            pdf.SetMeasurementUnits((int)PDFLibrary.MeasurementUnit.Points);
            pdf.SetInformation((int)PDFLibrary.DocumentProperty.Creator, "Scanquire");
            pdf.SetInformation((int)PDFLibrary.DocumentProperty.Producer, "E-Docs USA");

            //Encrypt if required
            if (Encryption != PDFLibrary.Encryption.NONE)
            {
                ETL.TraceLoggerInstance.TraceInformation("Encrypting Pdf document");


                //If the owner or user passwords are not provided, use the default
                ownerPassword = string.IsNullOrWhiteSpace(ownerPassword) ? OwnerPassword : ownerPassword;
                userPassword = string.IsNullOrWhiteSpace(userPassword) ? UserPassword : userPassword;

                //If the user password is still empty, generate a random one
                userPassword = string.IsNullOrWhiteSpace(userPassword) ? StringTools.GenerateRandomString(8, 10) : userPassword;

                //Encrypt the file.
                pdf.Encrypt(ownerPassword, userPassword, (int)Encryption, (int)Permissions);
            }
        }

        protected void DrawText(PDFLibrary pdf, string text)
        {
            ETL.TraceLoggerInstance.TraceInformation("Drawing text for Pdf document");

            //Set the margin to 1 inch.
            double margin = 72.0;
            //Set the starting position to the top left margin.
            double x = margin;
            double y = margin;

            //Set the text block width to the page width - left and right margin.
            double width = pdf.PageWidth() - (2 * margin);

            //Draw the text.
            pdf.DrawWrappedText(x, y, width, text);
        }

        protected void DrawImage(PDFLibrary pdf, SQImage image)
        {
            ETL.TraceLoggerInstance.TraceInformation("Drawing page image for Pdf document");

            byte[] data = EncodeImage(image);

            double left = 0;
            double top = 0;
            double width = pdf.PageWidth();
            double height = pdf.PageHeight();

            int imageId = pdf.AddImageFromString(data, (int)PDFLibrary.AddImageOption.Default);
            pdf.DrawImage(left, top, width, height);
            pdf.ReleaseImage(imageId);
        }

        protected void WriteBookmarks(PDFLibrary pdf, IEnumerable<SQCommand_Page_Bookmark> bookmarks)
        {
            ETL.TraceLoggerInstance.TraceInformation("Adding bookmarks for Pdf document");
            if (bookmarks.Count() < 0)
            {
                ETL.TraceLoggerInstance.TraceWarning("No bookmarks for Pdf document returning ");
                return;
            }

            foreach (SQCommand_Page_Bookmark bookmarkCommand in bookmarks)
            {
                ETL.TraceLoggerInstance.TraceInformation("Adding bookmark " + bookmarkCommand.Title);

                pdf.NewOutline(0, bookmarkCommand.Title, pdf.PageCount(), 0);
            }
            ETL.TraceLoggerInstance.TraceInformation("Done Adding bookmarks for Pdf document");
        }

        /// <summary>
        /// Sets the size of the current page.
        /// </summary>
        /// <param name="pdf"></param>
        /// <param name="pageSize">Size of the page (in points)</param>
        protected void SetPageSize(PDFLibrary pdf, SizeF pageSize)
        {
            ETL.TraceLoggerInstance.TraceInformation("Setting page size for Pdf document");
            //Scale the page size to the nearest half inch (if required)
            if (ScalePagesToNearestHalfInch)
            {
                ETL.TraceLoggerInstance.TraceInformation("Scaling to nearest half inch");

                //Convert to inches and round to nearest half.
                float widthInInches = ImageTools.PointsToInches(pageSize.Width);
                widthInInches = MathExtensions.RoundToNearestDecimal(widthInInches, 0.5M);
                float heightInInches = ImageTools.PointsToInches(pageSize.Height);
                heightInInches = MathExtensions.RoundToNearestDecimal(heightInInches, 0.5M);

                //Convert back to points
                float scaledWidth = ImageTools.InchesToPoints(widthInInches);
                float scaledHeight = ImageTools.InchesToPoints(heightInInches);

                pageSize = new SizeF(scaledWidth, scaledHeight);
            }

            //Finally, set the page size.
            ETL.TraceLoggerInstance.TraceInformation("Setting page size to " + pageSize.ToString() + " for PDF document");
            pdf.SetPageDimensions(pageSize.Width, pageSize.Height);
        }

        protected void WritePage(PDFLibrary pdf, SQPage page)
        {
            ETL.TraceLoggerInstance.TraceInformation("Writing page for PDF document");
            ETL.TraceLoggerInstance.TraceInformation("Adding single page with " + page.Commands.Count + " commands");

            using (SQImageEditLock currentPageEditLock = page.Image.BeginEdit())
            {
                SizeF imageSizeInPoints = new SizeF()
                {
                    Width = ImageTools.PixelsToPoints(page.Image.WorkingCopy.Width, page.Image.WorkingCopy.HorizontalResolution),
                    Height = ImageTools.PixelsToPoints(page.Image.WorkingCopy.Height, page.Image.WorkingCopy.VerticalResolution)
                };
                SetPageSize(pdf, imageSizeInPoints);
                DrawImage(pdf, page.Image);
            }
            ETL.TraceLoggerInstance.TraceInformation("Checking for bookmarks");
            SQCommand_Page_Bookmark[] drawBookmarkCommands = page.Commands.OfType<SQCommand_Page_Bookmark>().ToArray();
            if (drawBookmarkCommands.Length > 0)
            {
                ETL.TraceLoggerInstance.TraceInformation("Found bookmarks for PDF document and writting bookmarks");
                WriteBookmarks(pdf, drawBookmarkCommands);
            }
            else
            {
                ETL.TraceLoggerInstance.TraceWarning("No bookmark commands to process");

            }
        }

        public override Task<SQFile> Write(SQDocument document, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation("Creating file page for PDF document");
            if (document.Pages.Count <= 0)
            {
                ETL.TraceLoggerInstance.TraceError("Cannot create file with no pages");
                throw new Exception("Cannot create file with no pages");
            }

            return Task.Factory.StartNew<SQFile>(() =>
            {
                int progressCurrent = 0;
                int progressTotal;
                document.Pages.TryCount(out progressTotal);
                progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing"));
                ETL.TraceLoggerInstance.TraceInformation("Writing Pdf file: " + progressTotal + " pages");
                

                //Get the owner password from the document commands (if present)
                string ownerPassword = null;
                foreach (SQCommand_Document_OwnerPassword ownerPwCommand in document.Commands.OfType<SQCommand_Document_OwnerPassword>())
                {
                    //Can't handle multiple conflicting owner passwords
                    if ((string.IsNullOrWhiteSpace(ownerPassword) == false)
                        && (string.Compare(ownerPassword, ownerPwCommand.Password, false) != 0))
                    {
                        ETL.TraceLoggerInstance.TraceError("Conflicting owner password commands");
                        throw new Exception("Conflicting owner password commands");
                    }
                    else
                    { ownerPassword = ownerPwCommand.Password; }
                }

                //Get the user password from the document commands (if present)
                string userPassword = null;
                foreach (SQCommand_Document_UserPassword userPwCommand in document.Commands.OfType<SQCommand_Document_UserPassword>())
                {
                    //Can't handle multiple conflicting user passwords
                    if ((string.IsNullOrWhiteSpace(userPassword) == false)
                        && (string.Compare(userPassword, userPwCommand.Password, false) != 0))
                    {

                        ETL.TraceLoggerInstance.TraceError("Conflicting user password commands");
                        throw new Exception("Conflicting user password commands");
                    }
                    else
                    { userPassword = userPwCommand.Password; }
                }

                PDFLibrary pdf = new PDFLibrary();
                try
                {
                    InitializeDocument(pdf, ref ownerPassword, ref userPassword);

                    //Add the pages
                    //QuickPDF starts with a blank page, so use the enumerator directly
                    // to handle the first page differently (rather than foreach)
                    using (IEnumerator<SQPage> pages = document.Pages.GetEnumerator())
                    {
                        Action addPageAction = new Action(() =>
                        {
                            progressCurrent++;
                            ETL.TraceLoggerInstance.TraceInformation("Writing " + progressCurrent + " of " + progressTotal);
                            //TODO: Handle exclude page commands
                            WritePage(pdf, pages.Current);
                            progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing File"));
                        });

                        //If no pages, throw error
                        if (pages.MoveNext() == false)
                        {
                            ETL.TraceLoggerInstance.TraceError("Error moving to first page");
                            throw new Exception("Error moving to first page");
                        }

                        //Otherwise, write the first page
                        addPageAction();

                        //Then, loop through the rest of the pages (if any) and add them
                        while (pages.MoveNext())
                        {
                            pdf.NewPage();
                            addPageAction();
                        }
                    }

                    //All pages written, so finalize the document
                    ETL.TraceLoggerInstance.TraceInformation("Finalizing PDF document");

                    return new SQFile_Pdf()
                    {
                        PageCount = pdf.PageCount(),
                        //TODO: SaveToBytes() ?
                        Data = pdf.SaveToString(),
                        FileExtension = this.FileExtension,
                        MimeType = "Application/pdf",
                        OwnerPassword = ownerPassword,
                        UserPassword = userPassword,
                        Commands = document.Commands
                    };
                }
                finally
                { pdf.ReleaseLibrary(); }
            });
        }

        protected byte[] EncodeImage(SQImage image)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Encoding PDF image:{image.LatestRevision.OriginalImageFilePath}");

            using (SQImageEditLock editLock = image.BeginEdit())
            {
                FREE_IMAGE_FORMAT imageFormat;
                FREE_IMAGE_SAVE_FLAGS imageSaveFlags;
                //TODO: Change to used colors instead of colordepth?
                if (image.WorkingCopy.ColorDepth == 1)
                {
                    imageFormat = MonochromeImageFormat;
                    imageSaveFlags = MonochromeImageSaveFlags;
                }
                else if (image.WorkingCopy.ColorDepth <= 8)
                {
                    imageFormat = GreyscaleImageFormat;
                    imageSaveFlags = GreyscaleImageSaveFlags;
                }
                else
                {
                    imageFormat = ColorImageFormat;
                    imageSaveFlags = ColorImageSaveFlags;
                }

                ETL.TraceLoggerInstance.TraceInformation($"Saving Encoding PDF image:{image.LatestRevision.OriginalImageFilePath}");
                return image.WorkingCopy.Save(imageFormat, imageSaveFlags);
            }
        }
    }
}
