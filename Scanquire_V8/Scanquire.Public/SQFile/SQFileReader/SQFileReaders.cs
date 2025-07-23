using System;
using System.Collections.Generic;
using System.IO;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using System.Threading.Tasks;
using System.Threading;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
using ISQAcquireProgress = System.IProgress<EdocsUSA.Utilities.ProgressEventArgs<Scanquire.Public.SQImage>>;
using SQAcquireProgressEventArgs = EdocsUSA.Utilities.ProgressEventArgs<Scanquire.Public.SQImage>;
using System.Diagnostics;

namespace Scanquire.Public
{
    /// <summary>Singleton collection of available ISQFileReaders</summary>
	public sealed class SQFileReaders : SerializedObjectDictionary<ISQFileReader>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "File Readers"); }
		}
		
		static readonly SQFileReaders _Instance = new SQFileReaders();
		public static SQFileReaders Instance
		{ get { return _Instance; } }
		public static bool DecriptImage
        { get; set; }
		static SQFileReaders()
		{ }

        /// <summary>Map a file extension to the name of an ISQFileReader.</summary>
        public static string FileExtensionToReaderName(string ext)
        { return ext.TrimStart('.').ToUpper(); }

        public static IEnumerable<Task<SQImage>> Read(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Mapping file {file.Checksum}{file.FileExtension} to iso file reader usig assembly:{file.ToString()}");
            string readerName = FileExtensionToReaderName(file.FileExtension);
            ISQFileReader reader;
            if (Instance.TryGetValue(readerName, out reader) == false)
            {
                ETL.TraceLoggerInstance.TraceError("Cannot infer reader from file extension " + file.FileExtension);
                throw new InvalidOperationException("Cannot infer reader from file extension " + file.FileExtension);
            }

            return reader.Read(file, progress, cToken);
        }

        public static IEnumerable<Task<SQImage>> Read(string path, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Reading file {path}");
            string ext = Path.GetExtension(path);
            string readerName = FileExtensionToReaderName(ext);
            ETL.TraceLoggerInstance.TraceInformation($"Reading file extension {ext} readername:{readerName}");
            ISQFileReader reader;
            SQFile.DecriptImage = DecriptImage;
            if (Instance.TryGetValue(readerName, out reader) == false)
            {
                ETL.TraceLoggerInstance.TraceError("Cannot infer reader from file extension " + ext);
                throw new InvalidOperationException("Cannot infer reader from file extension " + ext);
            }

            return reader.Read(path, progress, cToken);
        }

        public static Task<SQImage[]> ReadAll(SQFile file, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Read all for file:{file.Checksum}{file.FileExtension} using assembly:{file.ToString()}");
            string readerName = FileExtensionToReaderName(file.FileExtension);
            ISQFileReader reader;
            
            if (Instance.TryGetValue(readerName, out reader) == false)
            {
                ETL.TraceLoggerInstance.TraceError("Cannot infer reader from file extension " + file.FileExtension);
                throw new InvalidOperationException("Cannot infer reader from file extension " + file.FileExtension);
            }

            return reader.ReadAll(file, progress, cToken);
        }

        public static Task<SQImage[]> ReadAll(string path, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            string ext = Path.GetExtension(path);
            string readerName = FileExtensionToReaderName(ext);
            ETL.TraceLoggerInstance.TraceError($"Read all file for path:{path} extension:{ext} readername:{readerName}");
            ISQFileReader reader;
            if (Instance.TryGetValue(readerName, out reader) == false)
            {
                ETL.TraceLoggerInstance.TraceError("Cannot infer reader from file extension " + ext); 
                throw new InvalidOperationException("Cannot infer reader from file extension " + ext);
            }

            return reader.ReadAll(path, progress, cToken);
        }
	}
}