using EdocsUSA.Utilities;
/*
 * User: Sam Brinly
 * Date: 2/13/2013
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Scanquire.Public
{
    /// <summary>Definition of an object that can read a file to produce a series of SQImages.</summary>
	public interface ISQFileReader
	{
        IEnumerable<Task<SQImage>> Read(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        IEnumerable<Task<SQImage>> Read(string path, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        Task<SQImage[]> ReadAll(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken);

        Task<SQImage[]> ReadAll(string path, IProgress<ProgressEventArgs> progress, CancellationToken cToken);
    }
}
