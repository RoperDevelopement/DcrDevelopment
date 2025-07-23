using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <credit>http://stackoverflow.com/questions/398388/convert-bitmaps-to-one-multipage-tiff-image-in-net-2-0</credit>
    public class SQFileWriter_Tiff : SQFileWriterBase
    {
        private EncoderValue _Compression = EncoderValue.CompressionLZW;
        public EncoderValue Compression
        {
            get { return _Compression; }
            set { _Compression = value; }
        }

        private string _FileExtension = PathExtensions.DotExtension("tif");
        public override string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = PathExtensions.DotExtension(value); }
        }

        protected const string TIFF_MIME_TYPE = "image/tiff";

        protected static ImageCodecInfo TiffImageCodecInfo = ImageCodecInfo.GetImageEncoders().First(ici => ici.MimeType == TIFF_MIME_TYPE);


        public override Task<SQFile> Write(SQDocument document, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation("Start Writing tif file");
            return Task.Factory.StartNew<SQFile>(()=>
            {
                int progressCurrent = 0;
                int pageCount = document.Pages.Count;
                int progressTotal = pageCount;
                
                progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing File"));

                //Open a memory stream for temp storage of the tiff data.
                //First page and subsequent pages are handled differently, so get the enumerator for drawImageCommands
                using (MemoryStream tiffStream = new MemoryStream())
                using (IEnumerator<SQPage> pagesEnumerator = document.Pages.GetEnumerator())
                {
                    if (pagesEnumerator.MoveNext() == false)
                    {
                        ETL.TraceLoggerInstance.TraceError("The specified document did not contain any images");
                        throw new Exception("The specified document did not contain any images");
                    }

                    //First page encoder parameters to compression and multiframe.
                    EncoderParameters firstPageEncoderParameters = new EncoderParameters(2);
                    firstPageEncoderParameters.Param[0] = new EncoderParameter(Encoder.Compression, (long)Compression);
                    firstPageEncoderParameters.Param[1] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);

                    //Get the bitmap for the first page and save it to the stream
                    
                    SQPage currentPage = pagesEnumerator.Current;
                    using (SQImageEditLock firstPageEditLock = currentPage.Image.BeginEdit())
                    using (Bitmap tiff = currentPage.Image.WorkingCopy.ToBitmap())
                    {
                        //Save the first page to the stream.
                        tiff.Save(tiffStream, TiffImageCodecInfo, firstPageEncoderParameters);
                        progressCurrent++;
                        progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing File"));
                        ETL.TraceLoggerInstance.TraceInformation($"Progress Current {progressCurrent.ToString()} progress total {progressTotal.ToString()} Writing File:{currentPage.Image.LatestRevision.OriginalImageFilePath}");
                        //Loop through the subsequent pages and add them to the stream.
                        EncoderParameters subsequentPageEncoderParameters = new EncoderParameters(2);
                        subsequentPageEncoderParameters.Param[0] = new EncoderParameter(Encoder.Compression, (long)Compression);
                        subsequentPageEncoderParameters.Param[1] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                        while (pagesEnumerator.MoveNext())
                        {
                            progressCurrent++;
                            //Get the current page and SaveAdd it to the main tiff.
                            currentPage = pagesEnumerator.Current;
                            using (SQImageEditLock subsequentPageEditLock = currentPage.Image.BeginEdit())
                            using (Bitmap subsequentTiff = currentPage.Image.WorkingCopy.ToBitmap())
                            { tiff.SaveAdd(subsequentTiff, subsequentPageEncoderParameters); }
                            ETL.TraceLoggerInstance.TraceInformation($"Progress Current {progressCurrent.ToString()} progress total {progressTotal.ToString()} Writing File:{currentPage.Image.LatestRevision.OriginalImageFilePath}");
                            progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Writing File"));
                        }
                        //Finally, flush the encoder to force all data to the stream.
                        EncoderParameters flushEncoderParameters = new EncoderParameters(1);
                        flushEncoderParameters.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);
                        tiff.SaveAdd(flushEncoderParameters);
                    }
                    ETL.TraceLoggerInstance.TraceInformation($"Done Writing tif file:{currentPage.Image.LatestRevision.OriginalImageFilePath}");


                    return new SQFile()
                    {
                        Data = tiffStream.ToArray(),
                        PageCount = pageCount,
                        FileExtension = this.FileExtension,
                        MimeType = TIFF_MIME_TYPE,
                        IndexFields = GetIndexFields(document.Commands.OfType<SQCommand_Document_IndexField>()),
                        Commands = document.Commands
                    };
                }
            });
        }

        protected Dictionary<string, object> GetIndexFields(IEnumerable<SQCommand_Document_IndexField> indexFieldCommands)
        {
            Dictionary<string, object> indexFields = new Dictionary<string,object>();
            foreach (SQCommand_Document_IndexField indexFieldCommand in indexFieldCommands)
            {
                indexFields[indexFieldCommand.Name] = indexFieldCommand.Value;
            }
            return indexFields;
        }
    }
}