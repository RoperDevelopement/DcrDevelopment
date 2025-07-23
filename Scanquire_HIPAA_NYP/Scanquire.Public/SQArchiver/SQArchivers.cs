/*
 * User: Sam Brinly
 * Date: 2/6/2013
 */
using System;
using System.IO;

using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    /// <summary>Singleton collection of available SQArchivers.</summary>
	public sealed class SQArchivers : SerializedObjectDictionary<ISQArchiver>
	{
        private string ArchivesFolder
        {
            get { return (AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.GetAppConfigSqlSetting("ArchivesFolder", "Scanquire.Public.dll.config", "Archivers")); }
        }
		public override string DirectoryPath
		{
			get
			{ //return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Archivers");
                return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, ArchivesFolder);
            }
		}
		static readonly SQArchivers _Instance = new SQArchivers();
		
		public static SQArchivers Instance
		{ get { return _Instance; } }
		
		static SQArchivers()
		{
		}
	}
}
