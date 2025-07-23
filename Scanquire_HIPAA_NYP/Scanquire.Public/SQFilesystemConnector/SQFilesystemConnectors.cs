using System;
using System.IO;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
    //Singleton collection of available SQFilesystemConnectors.
	public sealed class SQFilesystemConnectors : SerializedObjectDictionary<SQFilesystemConnector>
	{
		public override string DirectoryPath
		{
			get
			{ return Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, "Filesystem Connectors"); }
		}
		
		static readonly SQFilesystemConnectors _Instance = new SQFilesystemConnectors();
		public static SQFilesystemConnectors Instance
		{ get { return _Instance; } }

        static SQFilesystemConnectors()
		{
		}
	}
}
