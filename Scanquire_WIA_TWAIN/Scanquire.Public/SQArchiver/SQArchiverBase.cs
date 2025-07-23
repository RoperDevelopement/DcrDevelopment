using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Polenter.Serialization;
using Scanquire.Public.Extensions;
using Microsoft;
using System.Drawing;
using EDL = EdocsUSA.Utilities.Logging;

namespace Scanquire.Public
{
    /// <summary>A base implementation of an ISQArchiver, that handles the majority of common cases.</summary>
	public abstract class SQArchiverBase : ISQArchiver
	{
		#region Properties
		
        /// <summary>If set, will specify the scanner's device name to automatically use.</summary>
        /// <remarks>This must be verbatim to the name that appears in the Twain scanner source menu.</remarks>
		public virtual string ScannerName { get; set; }
		
        /// <summary>If set will specify the pre-defined custom setting to apply to the scan.</summary>
		public virtual string ScannerSetting { get; set; }

		private string _CommandReaderName = "DEFAULT";
        /// <summary>Common name of the desired SQCommandReader.</summary>
		public virtual string CommandReaderName
		{
			get { return _CommandReaderName; }
			set { _CommandReaderName = value; }
		}		
		
		private ISQCommandReader _CommandReader = null;
		/// <summary>Command Reader to read commands from images during a save operation.</summary>
        [ExcludeFromSerialization]
		public ISQCommandReader CommandReader
		{
			get 
			{
				if (_CommandReader == null)
				{ _CommandReader = SQCommandReaders.Instance[CommandReaderName]; }
				return _CommandReader;
			}
			set { _CommandReader = value; }
		}
		
		protected string _FileWriterName = "PDF";
        /// <summary>Name of the defined ISQFileWriter to use when compiling files.</summary>
		public virtual string FileWriterName
		{
			get { return _FileWriterName; }
			set { _FileWriterName = value; }
		}
		
		private ISQFileWriter _FileWriter = null;
        /// <summary>File Writer to compile files during a save operation.</summary>
		[ExcludeFromSerializationAttribute]
		public ISQFileWriter FileWriter 
		{ 
			get 
			{ 
				if (_FileWriter == null)
                {
                    
                      _FileWriter = SQFileWriters.Instance[FileWriterName];
                    }
                    return _FileWriter;
			} 
			set { _FileWriter = value; }
		}
		
        private string _CommandImageBuilderName = "DEFAULT";
        /// <summary>Name of the defined ISQCommandImageBuilder to use when SQCommandImages are requested.</summary>
        public virtual string CommandImageBuilderName
        {
            get { return _CommandImageBuilderName; }
            set { _CommandImageBuilderName = value; }
        }

        private ISQCommandImageBuilder _CommandImageBuilder = null;
        /// <summary>The ISQCommandImageBuilder to use when SQCommandImages are requested.</summary>
        [ExcludeFromSerialization]
        public ISQCommandImageBuilder CommandImageBuilder
        {
            get
            {
                if (_CommandImageBuilder == null)
                { _CommandImageBuilder = SQCommandImageBuilders.Instance[CommandImageBuilderName]; }
                return _CommandImageBuilder;
            }
            set { _CommandImageBuilder = value; }
        }

		#endregion Properties
		
		public SQArchiverBase()
		{ }

        public virtual IEnumerable<Task<SQImage>> Acquire(SQAcquireIntent intent, SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Acquire images from {intent.ToString()}");
            
            //Call the correct Acquire method based on the requested intent.
			switch (intent)
			{
				case SQAcquireIntent.New:
					return AcquireForNew(source, progress, cToken);
				case SQAcquireIntent.Append:
                    return AcquireForAppend(source, progress, cToken);
				case SQAcquireIntent.Insert:
                    return AcquireForInsert(source, progress, cToken);
				default:
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Unexpected SQAcquireIntent " + intent.ToString()+ "-intent");
                    throw new ArgumentException("Unexpected SQAcquireIntent " + intent.ToString(), "intent");					
			}
		}

        public virtual IEnumerable<Task<SQImage>> AcquireForNew(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Acquire image for New");
            
            //Call the correct Acquire method based on the requested source
			switch (source)
			{
				case SQAcquireSource.Command:
					return AcquireFromCommandForNew(progress, cToken);
				case SQAcquireSource.File:
					return AcquireFromFileForNew(progress, cToken);
				case SQAcquireSource.Scanner:
                    return AcquireFromScannerForNew(progress, cToken);
				case SQAcquireSource.Custom:
                    return AcquireFromCustomForNew(progress, cToken);
				default:
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Unexpected SQAcquireSource " + source.ToString()+ "-source");
                    throw new ArgumentException("Unexpected SQAcquireSource " + source.ToString(),"source");
			}
		}

        public virtual IEnumerable<Task<SQImage>> AcquireForAppend(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Acquire image for append");
            
            //Call the correct Acquire method based on the requested source
            switch (source)
            {
                case SQAcquireSource.Command:
                    return AcquireFromCommandForAppend(progress, cToken);
                case SQAcquireSource.File:
                    return AcquireFromFileForAppend(progress, cToken);
                case SQAcquireSource.Scanner:
                    return AcquireFromScannerForAppend(progress, cToken);
                case SQAcquireSource.Custom:
                    return AcquireFromCustomForAppend(progress, cToken);
                default:
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Unexpected SQAcquireSource " + source.ToString()+ "-source");
                    throw new ArgumentException("Unexpected SQAcquireSource " + source.ToString(), "source");
            }
        }

        public virtual IEnumerable<Task<SQImage>> AcquireForInsert(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Acquire image for insert");
            
            //Call the correct Acquire method based on the requested source
            switch (source)
            {
                case SQAcquireSource.Command:
                    return AcquireFromCommandForInsert(progress, cToken);
                case SQAcquireSource.File:
                    return AcquireFromFileForInsert(progress, cToken);
                case SQAcquireSource.Scanner:
                    return AcquireFromScannerForInsert(progress, cToken);
                case SQAcquireSource.Custom:
                    return AcquireFromCustomForInsert(progress, cToken);
                default:
                    EDL.TraceLogger.TraceLoggerInstance.TraceError("Unexpected SQAcquireSource " + source.ToString()+ "-source");
                    throw new ArgumentException("Unexpected SQAcquireSource " + source.ToString(), "source");
            }
        }
		
        /// <summary>Default method for acquiring from the scanner</summary>
		public virtual IEnumerable<Task<SQImage>> AcquireFromScanner(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{
            //Request the images from the scanner
            //If the scanner and setting names have been configured, use those.
            if ((string.IsNullOrWhiteSpace(ScannerName) == false)
                && (string.IsNullOrWhiteSpace(ScannerSetting) == false))
            { return SQTwain.Instance.Acquire(ScannerName, ScannerSetting, false, true, progress, cToken); }
            else
            { return SQTwain.Instance.Acquire(progress, cToken); }
        }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromScannerForNew(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromScanner(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromScannerForAppend(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromScanner(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromScannerForInsert(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromScanner(progress, cToken); }

        /// <summary>Acquiring from file depends on the type of archive, so defer to custom archiver implementation</summary>
        public abstract IEnumerable<Task<SQImage>> AcquireFromFile(IProgress<ProgressEventArgs> progress, CancellationToken cToken);
       // public abstract IEnumerable<Task<SQImage>> AcquireFromFile(IProgress<ProgressEventArgs> progress, CancellationToken cToken,string[] openFilePaths);


        public virtual IEnumerable<Task<SQImage>> AcquireFromFileForNew(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromFile(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromFileForAppend(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromFile(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromFileForInsert(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromFile(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCommand(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return CommandImageBuilder.Build(progress, cToken); }
	
		public virtual IEnumerable<Task<SQImage>> AcquireFromCommandForNew(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        { return AcquireFromCommand(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCommandForAppend(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        { return AcquireFromCommand(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCommandForInsert(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        { return AcquireFromCommand(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCustom(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{
            EDL.TraceLogger.TraceLoggerInstance.TraceError("This archiver does not support aquiring from 'Custom'");
            throw new NotImplementedException("This archiver does not support aquiring from 'Custom'");
        }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCustomForNew(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromCustom(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCustomForAppend(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromCustom(progress, cToken); }
		
		public virtual IEnumerable<Task<SQImage>> AcquireFromCustomForInsert(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
		{ return AcquireFromCustom(progress, cToken); }

        public virtual async Task<IList<ISQCommand>> ReadCommands(SQImage image, int documentNumber, int pageNumber, CancellationToken cToken)
        { 
            //By default, just call the command reader.
            return await CommandReader.Read(image, documentNumber, pageNumber, cToken); 
        }

        /// <summary>Translate a set of commands.</summar>
        /// <example>To ignore a command, convert it to SQCommand_Null</example>
        public virtual async Task<IList<ISQCommand>> TranslateCommands(IEnumerable<ISQCommand> commands, int documentNumber, int pageNumber, CancellationToken cToken)
        {
            //By default, just return everything recieved.
            return await TaskEx.FromResult(commands.ToList());
        }

        /// <summary>Send the specified images to the destination archive.</summary>
        public virtual async Task Send(IEnumerable<SQImage> images, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Sending images");
            
            int progressCurrent = 0;
            int progressTotal;
            images.TryCount(out progressTotal);
            
            //Current page index of the current document.
            int pageNumber = 0;
            //Keeps track of the number of documents
            int documentNumber = 0;

            //Start with a fresh document.
            SQDocument currentDocument = new SQDocument();

            foreach (SQImage image in images)
            {
                
                cToken.ThrowIfCancellationRequested();
                progressCurrent++;
                //Read any commands from the current page
                IList<ISQCommand> commands = await ReadCommands(image, documentNumber, pageNumber, cToken);
                //Perform any command translation
                commands = await TranslateCommands(commands, documentNumber, pageNumber, cToken);
                //If the commands included any terminate document commands and the current document is not empty, finalize the document and send it.
                    if ((commands.OfType<SQCommand_TerminateDocument>().Count() > 0)
                      && (currentDocument.Pages.Count > 0))
                  //  if  (currentDocument.Pages.Count > 0)
                {
                    documentNumber++;
                    Action<ProgressEventArgs> sendDocumentProgressAction = new Action<ProgressEventArgs>(p =>
                    { progress.Report(p); });
                    Progress<ProgressEventArgs> sendDocumentProgress = new Progress<ProgressEventArgs>(sendDocumentProgressAction);
                    await Send(currentDocument, documentNumber, sendDocumentProgress, cToken);
                    //Initialize a new document
                    currentDocument = new SQDocument();
                    pageNumber = 0;
                }
                //Add all document commands to the current document.
                currentDocument.Commands.AddRange(commands.OfType<ISQCommand_Document>());

                //Extract all page commands, create a new page, and add it to the document
                IEnumerable<ISQCommand_Page> pageCommands = commands.OfType<ISQCommand_Page>();

                //using (System.Drawing.Bitmap targetBmp = image.Thumbnail.Clone(new System.Drawing.Rectangle(0, 0, image.Thumbnail.Width, image.Thumbnail.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                //{
                //    // targetBmp is now in the desired format.
                //    targetBmp.Save("l:\\t.tif", System.Drawing.Imaging.ImageFormat.Tiff);
                    
                //}

                SQPage page = new SQPage(image, pageCommands);
                currentDocument.Pages.Add(page);

                pageNumber++;
            }
            //Finalize and send the last document (if not empty)
            if (currentDocument.Pages.Count > 0)
            {
                documentNumber++;
                Action<ProgressEventArgs> sendDocumentProgressAction = new Action<ProgressEventArgs>(p =>
                { progress.Report(p); });
                Progress<ProgressEventArgs> sendDocumentProgress = new Progress<ProgressEventArgs>(sendDocumentProgressAction);
                await Send(currentDocument, documentNumber, sendDocumentProgress, cToken);
            }
        }


        public virtual async Task Send(IProgress<ProgressEventArgs> progress, CancellationToken cToken, IEnumerable<SQImage> images)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Sending images");

            int progressCurrent = 0;
            int progressTotal;
            images.TryCount(out progressTotal);

            //Current page index of the current document.
            int pageNumber = 0;
            //Keeps track of the number of documents
            int documentNumber = 0;

            //Start with a fresh document.
            SQDocument currentDocument = new SQDocument();

            foreach (SQImage image in images)
            {
                cToken.ThrowIfCancellationRequested();
                progressCurrent++;
                //Read any commands from the current page
                IList<ISQCommand> commands = await ReadCommands(image, documentNumber, pageNumber, cToken);
                //Perform any command translation
                commands = await TranslateCommands(commands, documentNumber, pageNumber, cToken);
                //If the commands included any terminate document commands and the current document is not empty, finalize the document and send it.
               // if ((commands.OfType<SQCommand_TerminateDocument>().Count() > 0)
                 // && (currentDocument.Pages.Count > 0))
                  if  (currentDocument.Pages.Count > 0)
                {
                    documentNumber++;
                    Action<ProgressEventArgs> sendDocumentProgressAction = new Action<ProgressEventArgs>(p =>
                    { progress.Report(p); });
                    Progress<ProgressEventArgs> sendDocumentProgress = new Progress<ProgressEventArgs>(sendDocumentProgressAction);
                    await Send(currentDocument, documentNumber, sendDocumentProgress, cToken);
                    //Initialize a new document
                    currentDocument = new SQDocument();
                    pageNumber = 0;
                }
                //Add all document commands to the current document.
                currentDocument.Commands.AddRange(commands.OfType<ISQCommand_Document>());

                //Extract all page commands, create a new page, and add it to the document
                IEnumerable<ISQCommand_Page> pageCommands = commands.OfType<ISQCommand_Page>();
                SQPage page = new SQPage(image, pageCommands);
                currentDocument.Pages.Add(page);

                pageNumber++;
            }
            //Finalize and send the last document (if not empty)
            if (currentDocument.Pages.Count > 0)
            {
                documentNumber++;
                Action<ProgressEventArgs> sendDocumentProgressAction = new Action<ProgressEventArgs>(p =>
                { progress.Report(p); });
                Progress<ProgressEventArgs> sendDocumentProgress = new Progress<ProgressEventArgs>(sendDocumentProgressAction);
                await Send(currentDocument, documentNumber, sendDocumentProgress, cToken);
            }
        }

        /// <summary>Sends a single document to the archive.</summary>
        /// <param name="document">The document to archive.</param>
        /// <param name="documentNumber">For sending multiple documents, the index of the document in the overall listing.</param>
        public virtual async Task Send(SQDocument document, int documentNumber, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Sending document");
            

            string fileWriterProgressCaption = "Writing file " + documentNumber;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(fileWriterProgressCaption);

            //Encode the document into a file.
            Action<ProgressEventArgs> fileWriterProgressAction = new Action<ProgressEventArgs>(p =>
            { progress.Report(new ProgressEventArgs(p.Current, p.Total, fileWriterProgressCaption)); });
            Progress<ProgressEventArgs> fileWriterProgress = new Progress<ProgressEventArgs>(fileWriterProgressAction);
            SQFile file = await FileWriter.Write(document, fileWriterProgress, cToken);

            //Send the file to the archive
            string sendFileProgressCaption = "Sending file " + documentNumber;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(sendFileProgressCaption);
            Action<ProgressEventArgs> sendFileProgressAction = new Action<ProgressEventArgs>(p =>
            { progress.Report(new ProgressEventArgs(p.Current, p.Total, sendFileProgressCaption)); });
            Progress<ProgressEventArgs> sendFileProgress = new Progress<ProgressEventArgs>(sendFileProgressAction);
            await Send(file, documentNumber, sendFileProgress, cToken);
        }

        /// <summary>Sends a single file (encoded document) to the archiver.</summary>
        /// <param name="file">The file to archive.</param>
        /// <param name="fileNumber">For sending multiple files, the index of the file in the overall listing.</param>
        /// <param name="progress"></param>
        /// <param name="cToken"></param>
        /// <returns></returns>
        public abstract Task Send(SQFile file, int fileNumber, IProgress<ProgressEventArgs> progress, CancellationToken cToken);
	}
}
