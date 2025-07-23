using System;
using System.Collections.Generic;
using System.IO;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    //Singleton collection of all available SQCommandImageBuilders.
	public sealed class SQCommandImageBuilders : SerializedObjectDictionary<ISQCommandImageBuilder>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Command Image Builders"); }
		}
		
		static readonly SQCommandImageBuilders _Instance = new SQCommandImageBuilders();
		public static SQCommandImageBuilders Instance
		{ get { return _Instance; } }
		
		static SQCommandImageBuilders()
		{
		}
	}
}