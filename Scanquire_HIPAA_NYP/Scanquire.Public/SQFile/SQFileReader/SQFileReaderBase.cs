using EdocsUSA.Utilities;
using Microsoft;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanquire.Public
{
    public abstract class SQFileReaderBase : ISQFileReader
    {
        public abstract IEnumerable<Task<SQImage>> Read(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        public virtual IEnumerable<Task<SQImage>> Read(string path, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        { return Read(SQFile.FromFile(path), progress, cToken); }


        public virtual Task<SQImage[]> ReadAll(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        { return TaskEx.FromResult(Read(file, progress, cToken).Select(task => task.Result).ToArray()); }

        public virtual Task<SQImage[]> ReadAll(string path, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        { return ReadAll(SQFile.FromFile(path), progress, cToken); }
    }
}
