using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Scanquire.Public.Extensions;
using EdocsUSA.Utilities;
using System.Threading.Tasks;
using System.Threading;
using EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
    /// <summary>SQFileReader for reading single or multi-page image formats (jpeg, tiff, etc).</summary>
    public class SQFileReader_Image : SQFileReaderBase
    {
        public override IEnumerable<Task<SQImage>> Read(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation($"Reading image file using assembly {file.ToString()}");
            TraceLogger.TraceLoggerInstance.TraceInformation($"Reading image file {file.Checksum.ToString()}{file.FileExtension}");
            using (FreeImageBitmap fib = FreeImageBitmapExtensions.FromBytes(file.Data))
            {
                int frameCount = fib.FrameCount;
                int progressCurrent = 0;
                int progressTotal = frameCount;

                //For multi-frame image formats, loop through the frames and produce a single image per frame.
                for (int i = 0; i < frameCount; i++)
                {
                    cToken.ThrowIfCancellationRequested();
                    progressCurrent++;
                    TraceLogger.TraceLoggerInstance.TraceInformation("Reading frame " + i);
                    yield return Task.Factory.StartNew<SQImage>(() =>
            {

                fib.SelectActiveFrame(i);
                using (FreeImageBitmap pageFib = (FreeImageBitmap)(fib.Clone()))
                { return new SQImage(pageFib); }
            });
                    progress.Report(new ProgressEventArgs(progressCurrent, progressTotal));
                }
            }
        }
    }
}