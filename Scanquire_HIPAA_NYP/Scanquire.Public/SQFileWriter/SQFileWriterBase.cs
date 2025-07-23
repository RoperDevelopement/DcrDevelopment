using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using Microsoft;
using System.Diagnostics;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    public abstract class SQFileWriterBase : ISQFileWriter
    {
        private string _FileExtension;
        /// <summary>Windows file extension for the resulting files.</summary>
        /// <remarks>Automatically prepends the '.'</remarks>
        public virtual string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = PathExtensions.DotExtension(value); }
        }

        public abstract Task<SQFile> Write(SQDocument document, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        public virtual IEnumerable<Task<SQFile>> Write(IEnumerable<SQDocument> documents, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation("Creating image document");
            int progressCurrent = 0;
            int progressTotal;
            documents.TryCount(out progressTotal);
            ETL.TraceLoggerInstance.TraceInformation("Processing " + progressTotal + " documents");
            foreach (SQDocument document in documents)
            {
                cToken.ThrowIfCancellationRequested();
                progressCurrent++;

                //Set up the progress handler for the current file to report to the provided progress.
                string currentFileProgressCaption = "Writing " + progressCurrent + " of " + progressTotal;
                ETL.TraceLoggerInstance.TraceInformation(currentFileProgressCaption);

                Action<ProgressEventArgs> currentFileProgressHandler = new Action<ProgressEventArgs>(p =>
                { progress.Report(new ProgressEventArgs(p.Current, p.Total, currentFileProgressCaption)); });
                Progress<ProgressEventArgs> currentFileProgress = new Progress<ProgressEventArgs>(currentFileProgressHandler);

                yield return Write(document, currentFileProgress, cToken);
            }
        }

        public virtual Task<SQFile[]> WriteAll(IEnumerable<SQDocument> documents, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            return Task.Factory.StartNew<SQFile[]>(() =>
            { return Write(documents, progress, cToken).Select(task => task.Result).ToArray(); });
        }
    }
}
