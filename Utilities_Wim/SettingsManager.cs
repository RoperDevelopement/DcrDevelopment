/*
 * User: Sam Brinly
 * Date: 1/28/2013
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace EdocsUSA.Utilities
{

	public static class SettingsManager
	{
		
		private static bool _PortableMode = false;
		public static bool PortableMode
		{
			get { return _PortableMode; }
			set { _PortableMode = value; }
		}
		
		public enum SettingsScope
		{
			AllUsers,
			Application,
			CurrentUser
		}
		
		public static string RelativePath
		{ get { return Path.Combine(Application.CompanyName, Application.ProductName); }}

		
		
		public static string ApplicationDirectory
		{ get { return Path.GetDirectoryName(Application.ExecutablePath); } }
		
		public static string ApplicationSettingsDirectoryPath
		{ get { return Path.Combine(ApplicationDirectory, "Config"); } }
		
		public static string AllUsersSettingsDirectoryPath
		{
			get 
			{
				if (PortableMode) 
				{ return Path.Combine(ApplicationDirectory, "All Users"); }
				else
				{ return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), RelativePath); }
			}
		}
		
		public static string CurrentUserSettingsDirectoryPath
		{
			get
			{
				{ return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RelativePath); }
			}
		}
		
        private static string AuditFolder
        {
             get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),Application.CompanyName); } 
        }
        public static string AuditLogsDirectroy
        {
            get
            {
                { return Path.Combine(AuditFolder, "AuditLogs"); }
            }

        }
        public static string AuditLogsUploadDirectroy
        {
            get
            {
                { return Path.Combine(AuditFolder, "AuditLogsUpload"); }
            }

        }
        public static string TempDirectoryPath
		{
			get
			{
				{
					string configFile = Path.Combine(Edocs_Utilities.EdocsUtilitiesInstance.GetApplicationDir(), "EdocsUSA.Utilities.dll.config");
					string scanQuireImages = Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting("ImagesFolder",configFile);
					if(!(string.IsNullOrWhiteSpace(scanQuireImages)))
						return Path.Combine($"{scanQuireImages}", $"{RelativePath}\\{DateTime.Now.ToString("MM_dd_yyyy")}");
					// return Path.Combine(Path.GetTempPath(), RelativePath); 
					return Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Local", $"{RelativePath}\\{DateTime.Now.ToString("MM_dd_yyyy")}");
                }
            }
		}

		/*
		public static SettingsDirectory ApplicationSettings
		{
			get
			{
				if (PortableMode)
				{ return Path.Combine(ApplicationDirectory, "App Settings"); }
				else return 
			}
				if (_ApplicationSettings == null)
				{ _ApplicationSettings = new SettingsDirectory(ApplicationSettingsDirectoryPath); }
				return _ApplicationSettings;
			}
		}
		
		private static SettingsDirectory _AllUsersSettings = null;
		public static SettingsDirectory AllUsersSettings
		{
			get
			{
				if (_AllUsersSettings == null)
				{ 
					_AllUsersSettings = new SettingsDirectory(AllUsersSettingsDirectoryPath); 
				}
				return _AllUsersSettings;
			}
		}
		
		private static SettingsDirectory _CurrentUserSettings = null;
		public static SettingsDirectory CurrentUserSettings
		{
			get
			{
				if (_CurrentUserSettings == null)
				{ _CurrentUserSettings = new SettingsDirectory(CurrentUserSettingsDirectoryPath); }
				return _CurrentUserSettings;
			}
		}
		
		/// <summary>Maps a SettingsScope to a SettingsDictionary</summary>
		private static SettingsDirectory SettingsScopeToSettingsDirectory(SettingsScope scope)
		{
			switch (scope)
			{
				case SettingsScope.AllUsers: 
					return AllUsersSettings;
				case SettingsScope.Application: 
					return ApplicationSettings;
				case SettingsScope.CurrentUser:
					return CurrentUserSettings;
				default:
					throw new ArgumentException("Unexpected SettingsScope " + scope);
			}
		}
		
		public static void WriteKey(SettingsScope scope, string fileName, string key, object value)
		{ SettingsScopeToSettingsDirectory(scope).WriteKey(fileName, key, value); }
		
		public static T ReadKey<T>(SettingsScope scope, string fileName, string key, T defaultValue, out bool found)
		{ return SettingsScopeToSettingsDirectory(scope).ReadKey<T>(fileName, key, defaultValue, out found); }
		
		public static T ReadKey<T>(SettingsScope scope, string fileName, string key, T defaultValue)
		{ return SettingsScopeToSettingsDirectory(scope).ReadKey<T>(fileName, key, defaultValue); }
		
		/// <summary>Looks for a key in the following order: CurrentUser, AllUsers, Application</summary>
		public static T ReadKey<T>(string fileName, string key, T defaultValue, out bool found)
		{
			T value;
			value = ReadKey<T>(SettingsScope.CurrentUser, fileName, key, defaultValue, out found);
			if (found == false)
			{ value = ReadKey<T>(SettingsScope.AllUsers, fileName, key, defaultValue, out found); }
			if (found == false)
			{ value = ReadKey<T>(SettingsScope.Application, fileName, key, defaultValue, out found); }
			
			return value;
		}
		
		/// <summary>Looks for a key in the following order: CurrentUser, AllUsers, Application</summary>
		public static T ReadKey<T>(string fileName, string key, T defaultValue)
		{
			bool found;
			return ReadKey<T>(fileName, key, defaultValue, out found);
		}
	}
	*/
	}
}