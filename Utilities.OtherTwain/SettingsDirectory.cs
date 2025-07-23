/*
 * User: Sam Brinly
 * Date: 1/28/2013
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EdocsUSA.Utilities
{
	/// <summary>
	/// Description of SettingsDirectory.
	/// </summary>
	public class SettingsDirectory
	{
		private Dictionary<string, SettingsFile> SettingsFiles = new Dictionary<string, SettingsFile>(StringComparer.OrdinalIgnoreCase);
		
		private bool _AutoSave = true;
		public bool AutoSave
		{
			get { return _AutoSave; }
			set { AutoSave = value; }
		}
		
		private bool _ReadOnly = false;
		public bool ReadOnly
		{
			get { return _ReadOnly; }
			set { _ReadOnly = value; }
		}
		
		private string _DirectoryPath = null;
		public string DirectoryPath
		{
			get { return _DirectoryPath; }
			protected set { _DirectoryPath = value; }
		}
		
		public SettingsDirectory(string path)
		{
			DirectoryPath = path;
			Load();
		}
		
		public void Load()
		{
			EnsureDirectoryPathExists();
			foreach (string file in Directory.GetFiles(DirectoryPath, "*.cfg"))
			{
				string fileName = Path.GetFileNameWithoutExtension(file);
				SettingsFile settingsFile = new SettingsFile(file) {
					AutoSave=AutoSave,
					ReadOnly=ReadOnly
				};
				
				SettingsFiles[fileName] = settingsFile;
			}
		}
		
		/// <summary>Creates a directory at DirectoryPath if it doesn't already exist</summary>
		protected void EnsureDirectoryPathExists()
		{
			if (Directory.Exists(DirectoryPath) == false)
			{ Directory.CreateDirectory(DirectoryPath); }			
		}
		
		protected string GetSettingsFilePath(string fileName)
		{ return Path.Combine(DirectoryPath, fileName + ".cfg"); }
		
		public void Add(string fileName)
		{
			EnsureDirectoryPathExists();
			string settingsFilePath = GetSettingsFilePath(fileName);
			SettingsFile settingsFile = new SettingsFile(settingsFilePath) {
				AutoSave=AutoSave,
				ReadOnly=ReadOnly
			};
			
			SettingsFiles[fileName] = settingsFile;
		}
		
		/// <summary>
		/// Retrieves the named SettingsFile
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		/// <remarks>Creates the file if it doesn't exist</remarks>
		protected SettingsFile GetFile(string fileName)
		{
			if (SettingsFiles.ContainsKey(fileName) == false)
			{ Add(fileName); }
			
			return SettingsFiles[fileName];
		}
		
		public void WriteKey(string fileName, string key, object value)
		{ GetFile(fileName).WriteKey(key, value); }
		
		public T ReadKey<T>(string fileName, string key, T defaultValue, out bool found)
		{ return GetFile(fileName).ReadKey<T>(key, defaultValue, out found); }
		
		public T ReadKey<T>(string fileName, string key, T defaultValue)
		{ return GetFile(fileName).ReadKey<T>(key, defaultValue); }
	}
}