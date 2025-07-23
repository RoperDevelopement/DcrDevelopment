using EdocsUSA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanquire.Public
{
    /// <summary>Definition of an object capable to converting SQDocuments into SQFiles</summary>
    public interface ISQFileWriter
    {
        /// <summary>File extension to apply when generating a destination file name.</summary>
        string FileExtension { get; set; }

        Task<SQFile> Write(SQDocument document, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        IEnumerable<Task<SQFile>> Write(IEnumerable<SQDocument> document, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        Task<SQFile[]> WriteAll(IEnumerable<SQDocument> document, IProgress<ProgressEventArgs> progress, CancellationToken cToken);
    }
}
