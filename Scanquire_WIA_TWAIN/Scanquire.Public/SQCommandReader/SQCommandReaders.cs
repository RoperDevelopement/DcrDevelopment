using System;
using System.IO;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    /// <summary>
    /// Singleton collection of available ISQCommandReaders
    /// </summary>
	public sealed class SQCommandReaders : SerializedObjectDictionary<ISQCommandReader>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Command Readers"); }
		}
		
		static readonly SQCommandReaders _Instance = new SQCommandReaders();
		public static SQCommandReaders Instance
		{ get { return _Instance; } }
		
		static SQCommandReaders()
		{
		}
	}
}
