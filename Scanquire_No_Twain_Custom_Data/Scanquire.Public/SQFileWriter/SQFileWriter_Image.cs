using System;
using System.Linq;
using EdocsUSA.Utilities.Extensions;
using Scanquire.Public.Extensions;
using FreeImageAPI;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using System.Threading;
using System.Collections.Generic;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
	public class SQFileWriter_Image : SQFileWriterBase
	{
        private FREE_IMAGE_FORMAT _ImageFormat = FREE_IMAGE_FORMAT.FIF_PNG;
		public FREE_IMAGE_FORMAT ImageFormat 
        {
            get { return _ImageFormat; }
            set { _ImageFormat = value; }
        }
		
		private FREE_IMAGE_SAVE_FLAGS _SaveFlags = FREE_IMAGE_SAVE_FLAGS.DEFAULT;
		public FREE_IMAGE_SAVE_FLAGS SaveFlags
		{
			get { return _SaveFlags; }
			set { _SaveFlags = value; }
		}	
		
		public string MimeType { get; set; }

        public SQFileWriter_Image() { }

        public override Task<SQFile> Write(SQDocument document, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation("Creating an image doucment");
            
            return Task.Factory.StartNew<SQFile>(() =>
            {
                int progressCurrent = 0;
                int progressTotal = document.Pages.Count;
                //Ensure that it's a single page document.
                if (progressTotal <= 0 || progressTotal > 1)
                {
                    ETL.TraceLoggerInstance.TraceError("Image File Writer requires a single page document.");
                    throw new Exception("Image File Writer requires a single page document.");
                }
                ETL.TraceLoggerInstance.TraceInformation($"{progressCurrent.ToString()} {progressTotal.ToString()} Writing File");
                progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing File"));

                //Only one page, so grab it.
                SQPage currentPage = document.Pages[0];
                using (SQImageEditLock currentPageEditLock = currentPage.Image.BeginEdit())
                using (FreeImageBitmap fib = currentPage.Image.WorkingCopy)
                {
                    SQFile file = new SQFile()
                    {
                        Data = fib.Save(this.ImageFormat, this.SaveFlags),
                        PageCount = 1,
                        FileExtension = this.FileExtension,
                        MimeType = this.MimeType,
                        Commands = document.Commands
                    };

                    progressCurrent++;

                    progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing File"));
                    ETL.TraceLoggerInstance.TraceInformation($"{progressCurrent.ToString()} {progressTotal.ToString()} Writing File");
                    ETL.TraceLoggerInstance.TraceInformation($"Done Writing File {file.Checksum}{file.FileExtension}");
                    return file;
                }
            });
		}		
	}
}
