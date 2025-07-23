using System;
using System.Collections.Generic;
using System.IO;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    /// <summary>
    /// Manages all configured ISQImageProcessors.
    /// </summary>
	public sealed class SQImageProcessors : SerializedObjectDictionary<ISQImageProcessor>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Image Processors"); }
		}
		
		static readonly SQImageProcessors _Instance = new SQImageProcessors();
		public static SQImageProcessors Instance
		{ get { return _Instance; } }
		
		static SQImageProcessors()
		{
		}
	}
}