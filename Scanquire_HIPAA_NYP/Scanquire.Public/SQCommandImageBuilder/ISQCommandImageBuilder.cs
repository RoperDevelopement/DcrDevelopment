using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using Microsoft;

namespace Scanquire.Public
{		
    /// <summary>Definition of an object that can create an command based SQImage.</summary>
	public interface ISQCommandImageBuilder
	{
        IEnumerable<Task<SQImage>> Build(IProgress<ProgressEventArgs> progress, CancellationToken cToken);
    }
}
