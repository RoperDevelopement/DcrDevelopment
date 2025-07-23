using System;
using System.Collections.Generic;
using System.IO;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    /// <summary>Manages all configured ISQFileWriters</summary>
	public sealed class SQFileWriters : SerializedObjectDictionary<ISQFileWriter>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "File Writers"); }
		}
		
		static readonly SQFileWriters _Instance = new SQFileWriters();
		public static SQFileWriters Instance
		{ get { return _Instance; } }
		
		static SQFileWriters()
		{
		}
	}
}