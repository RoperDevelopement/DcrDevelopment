using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using Microsoft;

namespace Scanquire.Public
{
    /// <summary>An empty command reader</summary>
	public class SQCommandReader : ISQCommandReader
	{
        public Task<IList<ISQCommand>> Read(SQImage image, int documentNumber, int pageNumber, CancellationToken cToken)
        {
            return Task.Factory.StartNew<IList<ISQCommand>>(() =>
            { return new List<ISQCommand>(); });
        }
	}
}
