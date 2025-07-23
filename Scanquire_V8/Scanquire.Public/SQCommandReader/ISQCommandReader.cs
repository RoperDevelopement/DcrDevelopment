/*
 * User: Sam Brinly
 * Date: 11/20/2013
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using Microsoft;

namespace Scanquire.Public
{
    //Definition of an object capable of reading ISQCommands from SQImages
	public interface ISQCommandReader
	{
        Task<IList<ISQCommand>> Read(SQImage image, int documentNumber, int pageNumber, CancellationToken cToken);
	}
}
