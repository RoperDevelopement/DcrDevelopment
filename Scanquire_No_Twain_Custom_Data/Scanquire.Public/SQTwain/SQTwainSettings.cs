using System;
using System.Collections.Generic;
using System.IO;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    /// <summary>All available SQTwainSettings.</summary>
	public sealed class SQTwainSettings : SerializedObjectDictionaryCollection<SQTwainSetting>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.AllUsersSettingsDirectoryPath, "Twain Settings"); }
		}
		
		static readonly SQTwainSettings _Instance = new SQTwainSettings();
		public static SQTwainSettings Instance
		{ get { return _Instance; } }
		
		static SQTwainSettings()
		{
		}
	}
}