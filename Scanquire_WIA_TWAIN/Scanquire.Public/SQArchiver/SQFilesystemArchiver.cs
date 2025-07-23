using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;
using Microsoft;

namespace Scanquire.Public
{
	/// <summary>A basic archiver for accessing a local or NAS file system.</summary>
	public class SQFilesystemArchiver : SQArchiverBase
	{
		#region Static Properties
		
        #endregion Static Properties

        /// <summary>
        /// 
        /// </summary>
	    /*
        //TODO:Move all pathing information to SQFilesystemConnector, it no longer belongs here.
        public static string DefaultBasePath 
        { get { return Properties.SQFilesystemArchiver.Default.DefaultRootPath; } }
		
		
		
        private static string _DefaultRootPath = @"C:\Archives\";
        private static string _DefaultLogPath = Path.Combine(_DefaultRootPath, "FilesystemArchiver", "FilesystemArchiver.log");
        */
		private SQFilesystemConnector _FilesystemConnector = new SQFilesystemConnector()
		{ };
		 
		public SQFilesystemConnector FilesystemConnector 
		{
			get { return _FilesystemConnector; }
			set { _FilesystemConnector = value; }
		}

		private FileDictionaryLoggerBase _Log;
        /// <summary>Log to use when writing metadata about saved files.</summary>
		public FileDictionaryLoggerBase Log
		{
			get { return _Log; }
			set { _Log = value; }
		}
		/*
		private bool _EnableLogging = true;
        /// <summary>When true, metadata for all saved files will be saved using the specified FileDictionaryLoggerBase</summary>
		public bool EnableLogging
		{
			get { return _EnableLogging; }
			set { _EnableLogging = value; }
		}*/
		
		public SQFilesystemArchiver() : base()
		{
			
		}

        public override IEnumerable<Task<SQImage>> AcquireFromFile(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Acquiring from file");
			string[] openFilePaths = null;
			if(SQFilesystemConnector.ConvertedImages)
            {
				openFilePaths = Directory.GetFiles(SQFilesystemConnector.ConvertedImagesFolder);
            }
			else
			{ 
			 openFilePaths = FilesystemConnector.SelectFilesForOpen();
			}
			if(openFilePaths != null)
			{ 
			int fileCount = openFilePaths.Length;
            int currentFileIndex = 0;
           
            foreach (string path in openFilePaths)
            {
                currentFileIndex++;
                string currentFileProgressCaption = "File " + currentFileIndex + " of " + fileCount;
                TraceLogger.TraceLoggerInstance.TraceInformation(currentFileProgressCaption);
                Action<ProgressEventArgs> currentFileProgressAction = new Action<ProgressEventArgs>(p =>
                { progress.Report(new ProgressEventArgs(p.Current, p.Total, currentFileProgressCaption)); });
                Progress<ProgressEventArgs> currentFileProgress = new Progress<ProgressEventArgs>(currentFileProgressAction);
                foreach (Task<SQImage> task in SQFileReaders.Read(path, currentFileProgress, cToken))
                { yield return task; }
            }
			}
		}
		//public override IEnumerable<Task<SQImage>> AcquireFromFile(IProgress<ProgressEventArgs> progress, CancellationToken cToken,string[] openFilePaths)
		//{
		//	TraceLogger.TraceLoggerInstance.TraceInformation("Acquiring from file");
		//	//string[] openFilePaths = FilesystemConnector.SelectFilesForOpen();

		//	int fileCount = openFilePaths.Length;
		//	int currentFileIndex = 0;

		//	foreach (string path in openFilePaths)
		//	{
		//		currentFileIndex++;
		//		string currentFileProgressCaption = "File " + currentFileIndex + " of " + fileCount;
		//		TraceLogger.TraceLoggerInstance.TraceInformation(currentFileProgressCaption);
		//		Action<ProgressEventArgs> currentFileProgressAction = new Action<ProgressEventArgs>(p =>
		//		{ progress.Report(new ProgressEventArgs(p.Current, p.Total, currentFileProgressCaption)); });
		//		Progress<ProgressEventArgs> currentFileProgress = new Progress<ProgressEventArgs>(currentFileProgressAction);
		//		foreach (Task<SQImage> task in SQFileReaders.Read(path, currentFileProgress, cToken))
		//		{ yield return task; }
		//	}
		//}

		public override async Task Send(SQFile file, int fileNumber, IProgress<ProgressEventArgs> progress, CancellationToken cancelToken)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Sending file");

            SQFilesystemConnector.SaveFileResult saveResult;
            do
            {
                string saveFilePath = FilesystemConnector.SelectFileForSave(file.FileExtension);
                TraceLogger.TraceLoggerInstance.TraceInformation($"Sending file savefilepath {saveFilePath}");
                saveResult = await FilesystemConnector.SaveFile(saveFilePath, file.Data);
            }
            while (saveResult.RetryRequested == true);
        }

		/*
		
		public override void Send(SQFile file, SynchronizationHelper synchronizationHelper, bool suppressProgressNotifications) 
		{ 
			Debug.WriteLine("Sending file");
			
			string savePath = FilesystemConnector.SelectFileForSave(synchronizationHelper.Context);
			FilesystemConnector.SaveFileResult saveResult = FilesystemConnector.SaveFile(savePath, file.Data, synchronizationHelper.Context);
			if (saveResult.Success == false)
			{
				if (saveResult.RetryRequested)
				{ Send(file, synchronizationHelper, suppressProgressNotifications); }
				else return;
			}
			
			Dictionary<string, string> logEntry = new Dictionary<string, string>();
			logEntry["FilePath"] = saveResult.FinalPath;
			logEntry["Revision"] = saveResult.VersionNumber.ToString();
			logEntry["PageCount"] = file.PageCount.ToString();
			logEntry["Checksum"] = file.Checksum;
			logEntry["UserName"] = Environment.UserName;
			logEntry["Timestamp"] = DateTime.Now.ToUniversalTime().ToLongTimeString();
			
			Log.Append(logEntry);
		}
		*/
	}
}
