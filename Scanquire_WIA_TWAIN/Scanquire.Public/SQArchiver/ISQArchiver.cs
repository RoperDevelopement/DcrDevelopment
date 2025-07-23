using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using FreeImageAPI;
using Microsoft;

namespace Scanquire.Public
{	
    /// <summary>
    /// Defines the interface between the Scanquire application and 
    /// </summary>
	public interface ISQArchiver
	{						
        /// <summary>
        /// Handles retrieving documents from an archiver and feeding them as images into Scanquire
        /// </summary>
        /// <param name="intent">The intent of the retrieval (new doc, append, insert, etc)</param>
        /// <param name="source">The expected source (scanner, file, etc) to retrieve the document from.</param>
		IEnumerable<Task<SQImage>> Acquire(SQAcquireIntent intent, SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cancelToken);
               
        /// <summary>
        /// The initial entry point to send images from Scanquire to the archiver.
        /// </summary>
        /// <param name="images">Individual images in the order they are presented in Scanquire</param>
        Task Send(IEnumerable<SQImage> images, IProgress<ProgressEventArgs> progress, CancellationToken cancelToken);

        /// <summary>
        /// Handles sending an individual file (composed of one or more images).
        /// Also in handles arbitrary (non image) files sent from a non-imaging application.
        /// </summary>
        /// <param name="file">File to be sent to the archiver.</param>
        /// <param name="fileNumber">Index of the file being sent (for multiple files).</param>
        Task Send(SQFile file, int fileNumber, IProgress<ProgressEventArgs> progress, CancellationToken cancelToken);
	}
}
