using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Controls;

namespace EdocsUSA.Utilities
{
	public class FilesystemConnector
	{
		private string _RootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		/// <summary>The directory to use as the initial directory for file dialogs, and for building relative paths.</summary>
		public string RootPath
		{
			get { return _RootPath; }
			set { _RootPath = value; }
		}
		
		private ExistingFileAction _ExistingFileAction = ExistingFileAction.AskUser;
		public ExistingFileAction ExistingFileAction
		{
			get { return _ExistingFileAction; }
			set { _ExistingFileAction = value; }
		}
		
		private string _VersionPrefix = "_v";
		/// <summary>Prefix used to denote version information in a path.</summary>
		/// Example, if VersionPadding is 3 and VersionPrefix is _v, a versioned file name would be foo_v001.ext.
		public string VersionPrefix
		{
			get { return _VersionPrefix; }
			set 
			{ 
				if (string.IsNullOrWhiteSpace(value))
				{
                    
                    throw new ArgumentNullException("value", "VersionPrefix cannot be empty");
                }
				
				_VersionPrefix = value;
			}
		}
		
		private int _VersionPadding = 2;
		/// <summary>Minimum width of a version number string.</summary>
		/// <remarks>
		/// Ensures correct file sorting. (without padding, version 10 would come before version 2).
		/// Example, if VersionPadding is 3 and VersionPrefix is _v, a versioned file name would be foo_v001.ext.
		/// </remarks>
		public int VersionPadding
		{
			get { return _VersionPadding; }
			set
			{
				if (value < 0)
				{ throw new ArgumentOutOfRangeException("value", "VersionPadding must be >= 0"); }
				
				_VersionPadding = value;
			}
		}
		
		private bool _AddVersionInfoToPrimaryVersion = true;
		/// <summary>
		/// If true, the primary version of a file will contain version information.
		/// If false, the primary version of a file will not contain any version information.
		/// </summary>
		public bool AddVersionInfoToPrimaryVersion
		{
			get { return _AddVersionInfoToPrimaryVersion; }
			set { _AddVersionInfoToPrimaryVersion = value; }
		}
		
		private OpenFileDialog _OpenFileDialog = new OpenFileDialog()
		{ RestoreDirectory = true, Multiselect = true };
		public OpenFileDialog OpenFileDialog
		{
			get { return _OpenFileDialog; }
			set { _OpenFileDialog = value; }
		}
		
		private bool _OpenFileDialogRememberLastDirectory = true;
		public bool OpenFileDialogRememberLastDirectory
		{ 
			get { return _OpenFileDialogRememberLastDirectory; }
			set { _OpenFileDialogRememberLastDirectory = value; }
		}
		
		private SaveFileDialog _SaveFileDialog = new SaveFileDialog()
		{ 
			RestoreDirectory = true,
			OverwritePrompt = false		
		};
		public SaveFileDialog SaveFileDialog
		{
			get { return _SaveFileDialog; }
			set { _SaveFileDialog = value; }
		}
		
		private bool _SaveFileDialogRememberLastDirectory = true;
		public bool SaveFileDialogRememberLastDirectory
		{
			get { return _SaveFileDialogRememberLastDirectory; }
			set { _SaveFileDialogRememberLastDirectory = value; }
		}
		
		private FolderBrowserDialog _FolderBrowserDialog = new FolderBrowserDialog();
		public FolderBrowserDialog FolderBrowserDialog
		{
			get { return _FolderBrowserDialog; }
			set { _FolderBrowserDialog = value; }
		}
		
		public string[] SelectFilesForOpen(IWin32Window owner, SynchronizationContext context)
		{ 
			//If the initial directory has not been set yet, use the archive root.
			if (string.IsNullOrWhiteSpace(OpenFileDialog.InitialDirectory))
			{ OpenFileDialog.InitialDirectory = RootPath; }
			
			string[] paths = OpenFileDialog.SelectFiles(owner, context);
			
			//If remember last directory is set, set the initialdirectory to the selected directory
			if (OpenFileDialogRememberLastDirectory)
			{ OpenFileDialog.InitialDirectory = OpenFileDialog.GetCurrentDirectory(); }
			
			return paths;
		}
		
		public string[] SelectFilesForOpen()
		{ return SelectFilesForOpen(default(IWin32Window), default(SynchronizationContext)); }
		
		public string[] SelectFilesForOpen(IWin32Window owner)
		{ return SelectFilesForOpen(owner, default(SynchronizationContext)); }
		
		public string[] SelectFilesForOpen(SynchronizationContext context)
		{ return SelectFilesForOpen(default(IWin32Window), context); }
		
		public string SelectFileForOpen(IWin32Window owner, SynchronizationContext context)
		{ 
			string[] paths = SelectFilesForOpen(owner, context);
			if ((paths == null) || (paths.Length <= 0))
			{ return null; }
			else if (paths.Length > 1)
			{ throw new InvalidOperationException("The requested operation requires a single file to be selected"); }
			else //TODO: Check for latest revision and prompt?
			{ return paths[0]; }
		}
		
		public string SelectFileForOpen()
		{ return SelectFileForOpen(default(IWin32Window), default(SynchronizationContext)); }
		
		public string SelectFileForOpen(IWin32Window owner)
		{ return SelectFileForOpen(owner, default(SynchronizationContext)); }
		
		public string SelectFileForOpen(SynchronizationContext context)
		{ return SelectFileForOpen(default(IWin32Window), context); }
		
		public string SelectFileForSave(string ext, IWin32Window owner, SynchronizationContext context)
		{
			//If the initial directory has not been set yet, use the archive root.
			if (string.IsNullOrWhiteSpace(SaveFileDialog.InitialDirectory))
			{ SaveFileDialog.InitialDirectory = RootPath; }
			
			string previousFilter = SaveFileDialog.Filter;
			bool filterChanged = false;
			
			try
			{
				if (String.IsNullOrWhiteSpace(ext) == false)
				{
					string unDottedExtension = PathExtensions.UnDotExtension(ext);
					string dottedExtension = PathExtensions.DotExtension(ext);
					SaveFileDialog.Filter = unDottedExtension + " files (*" + dottedExtension + ") | *" + dottedExtension;
					filterChanged = true;
				}
				return SaveFileDialog.SelectFile(owner, context);
			}
			finally
			{
				if (filterChanged)
				{ SaveFileDialog.Filter = previousFilter; }
				
				//If remember last directory is set, set the initialdirectory to the selected directory
				if (SaveFileDialogRememberLastDirectory)
				{ SaveFileDialog.InitialDirectory = SaveFileDialog.GetCurrentDirectory(); }
			}
		}
		
		public string SelectFileForSave(string ext, IWin32Window owner)
		{ return SelectFileForSave(ext, owner, default(SynchronizationContext)); }
		
		public string SelectFileForSave(string ext, SynchronizationContext context)
		{ return SelectFileForSave(ext, default(IWin32Window), context); }
		
		public string SelectFileForSave(string ext)
		{ return SelectFileForSave(ext, default(IWin32Window), default(SynchronizationContext)); }
		
		public string SelectFileForSave(IWin32Window owner, SynchronizationContext context)
		{ return SelectFileForSave(null, owner, context); }
		
		public string SelectFileForSave(IWin32Window owner)
		{ return SelectFileForSave(null, owner, default(SynchronizationContext)); }
		
		public string SelectFileForSave(SynchronizationContext context)
		{ return SelectFileForSave(null, default(IWin32Window), context); }
		
		public string SelectFileForSave()
		{ return SelectFileForSave(null, default(IWin32Window), default(SynchronizationContext)); }
		
		public class SaveFileResult
		{
			public bool Success { get; set; }
			public int VersionNumber { get; set; }
			public bool RetryRequested { get; set; }
			public string FinalPath { get; set; }
		}
		
		public SaveFileResult SaveFile(string path, byte[] data, ExistingFileAction existingFileAction, IWin32Window owner, SynchronizationContext context)
		{
			//Ensure the path is rooted
			string tPath = PathExtensions.GetRootedPath(path, RootPath);
			
			//Get the latest existing version (if any)
			int latestExistingVersionNumber;
			
			//Default new data to primary (0) version
			int newVersionNumber = 0;
			string newPath = AddOrChangeVersion(tPath, newVersionNumber);
			
			//Check for and get the latest existing version
			//If previous version(s) exist, handle based on existingFileAction
			if (TryGetLatestVersion(tPath, out latestExistingVersionNumber))
			{
				Trace.TraceInformation("File exists");
				switch (existingFileAction)
				{
					//If ask user, do so and recall SaveFile with the new ExistingFileAction
					case ExistingFileAction.AskUser:
						ExistingFileAction newExistingFileAction = ExistingFileActionDialog.TryAskUser(owner, context);
						return SaveFile(path, data, newExistingFileAction, owner, context);
					//If newrevision, increment the version number and generate the path.
					case ExistingFileAction.NewVersion:
						newVersionNumber = latestExistingVersionNumber + 1;
						newPath = AddOrChangeVersion(tPath, newVersionNumber);
						break;
					//If overwrite, generate the path from the latest version
					case ExistingFileAction.OverwriteLatest:
						newVersionNumber = latestExistingVersionNumber;
						newPath = AddOrChangeVersion(tPath, newVersionNumber);
						break;
					case ExistingFileAction.RequestRetry:
						return new SaveFileResult()
						{
							Success = false,
							RetryRequested = true,
							FinalPath = null,
							VersionNumber = -1							
						};
					default:
						Trace.TraceWarning("Unexpected ExistingFileAction " + existingFileAction);
						return new SaveFileResult()
						{
							Success = false,
							RetryRequested = false,
							FinalPath = null,
							VersionNumber = -1
						};
				}
			}
			else { Trace.TraceInformation("File does not exist"); }
			
			//At this point we should be ready to save and have the final version and path information
			
			//Make sure the destination directory exists
			Directory.CreateDirectory(Path.GetDirectoryName(newPath));
			
			//Finally save the file and return the results
			File.WriteAllBytes(newPath, data);
			
			return new SaveFileResult()
			{
				Success = true,
				RetryRequested = false,
				VersionNumber = newVersionNumber,
				FinalPath = newPath
			};
		}
		
		public SaveFileResult SaveFile(string path, byte[] data, ExistingFileAction existingFileAction, IWin32Window owner)
		{ return SaveFile(path, data, existingFileAction, owner, default(SynchronizationContext)); }
		
		public SaveFileResult SaveFile(string path, byte[] data, ExistingFileAction existingFileAction, SynchronizationContext context)
		{ return SaveFile(path, data, existingFileAction, default(IWin32Window), context); }
		
		public SaveFileResult SaveFile(string path, byte[] data, ExistingFileAction existingFileAction)
		{ return SaveFile(path , data, existingFileAction, default(IWin32Window), default(SynchronizationContext)); }
		
		public SaveFileResult SaveFile(string path, byte[] data, IWin32Window owner, SynchronizationContext context)
		{ return SaveFile(path, data, this.ExistingFileAction, owner, context); }
		
		public SaveFileResult SaveFile(string path, byte[] data, IWin32Window owner)
		{ return SaveFile(path, data, this.ExistingFileAction, owner, default(SynchronizationContext)); }
		
		public SaveFileResult SaveFile(string path, byte[] data, SynchronizationContext context)
		{ return SaveFile(path, data, this.ExistingFileAction, default(IWin32Window), context); }
		
		public SaveFileResult SaveFile(string path, byte[] data)
		{ return SaveFile(path, data, this.ExistingFileAction, default(IWin32Window), default(SynchronizationContext)); }
			
		/// <summary>Try to parse the version information out of the provided path.</summary>
		/// <param name="path">Path to parse.</param>
		/// <param name="versionText">
		/// On success, the text part of the version information (including the version prefix).
		/// On fail, null.
		/// </param>
		/// <param name="versionNumber">
		/// On success, the numeric version number.
		/// On fail, -1.
		/// </param>
		/// <returns>
		/// True if the path contained version info.
		/// False if the path does not contain version info.
		/// </returns>
		public bool TryParse(string path, out string versionText, out int versionNumber)
		{
			if (string.IsNullOrWhiteSpace(path))
			{ throw new ArgumentNullException("path"); }
			
			string versionTextDefault = null;
			int versionNumberDefault = -1;
			
			//Version text starts at the last VersionPrefix
			string pathWithoutExt = Path.GetFileNameWithoutExtension(path);
			int versionLocation = pathWithoutExt.LastIndexOf(VersionPrefix);
			//If no version info is present, just return the original path
			if (versionLocation < 0)
			{ 
				versionText = versionTextDefault;
				versionNumber = versionNumberDefault;
				return false; 
			}
			
			versionText = pathWithoutExt.Substring(versionLocation);
			
			//Version number is everything after VersionPrefix
			string versionNumberText = versionText.Substring(VersionPrefix.Length);
			
			//Try parsing the number
			bool valid = int.TryParse(versionNumberText, out versionNumber);
			
			//If the parse failed, reset version text to default
			versionText = valid ? versionText : versionTextDefault;
			
			return valid;
		}
		
		/// <summary>Try to parse the version information out of the provided path.</summary>
		/// <param name="path">Path to parse.</param>
		/// <param name="versionText">
		/// On success, the text part of the version information (including the version prefix).
		/// On fail, null.
		/// </param>
		/// <returns>
		/// True if the path contained version info.
		/// False if the path does not contain version info.
		/// </returns>
		public bool TryParse(string path, out string versionText)
		{
			if (string.IsNullOrWhiteSpace(path))
			{ throw new ArgumentNullException("path"); }
						
			int versionNumber;
			return TryParse(path, out versionText, out versionNumber);
		}
		
		/// <summary>Try to parse the version information out of the provided path.</summary>
		/// <param name="path">Path to parse.</param>
		/// <param name="versionNumber">
		/// On success, the numeric version number.
		/// On fail, -1.
		/// </param>
		/// <returns>
		/// True if the path contained version info.
		/// False if the path does not contain version info.
		/// </returns>
		public bool TryParse(string path, out int versionNumber)
		{						
			string versionText;
			return TryParse(path, out versionText, out versionNumber);
		}
		
		/// <param name="path"></param>
		/// <returns>The provided path with all (if any) version info removed.</returns>
		public string RemoveVersion(string path)
		{
			Trace.TraceInformation("Removing version from " + path);
			if (string.IsNullOrWhiteSpace(path))
			{ throw new ArgumentNullException("path"); }
			
			string versionText;
			//If the path does not contain version info, just return the provided path.
			if (TryParse(path, out versionText) == false)
			{ 
				Debug.WriteLine("No version info found, returning path as is");
				return path; 
			}
			
			string directory = Path.GetDirectoryName(path);
			string ext = Path.GetExtension(path);
			string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);
			
			Trace.TraceInformation("Path without ext " + fileNameWithoutExt);
			string fileNameWithoutVersionOrExt = fileNameWithoutExt.Substring(0, fileNameWithoutExt.Length - versionText.Length);
			string fileNameWithoutVersion = Path.ChangeExtension(fileNameWithoutVersionOrExt, ext);
			
			string filePath = Path.Combine(directory, fileNameWithoutVersion);
			
			return filePath;
		}
		
		/// <summary>Generate a version string from a version number</summary>
		/// <param name="versionNumber"></param>
		/// <returns>
		/// If versionNumber is zero and AddVersionInfoToPrimaryVersion is false, an empty string.
		/// Otherwise: A version string formatted [VersionPrefix][#] (ex _v002).
		/// </returns>
		public string GenerateVersionText(int versionNumber)
		{ 
			if (versionNumber < 0)
			{ throw new ArgumentOutOfRangeException("versionNumber", "versionNumber must be >= 0"); }
		
			if ((versionNumber == 0) && (AddVersionInfoToPrimaryVersion == false))
			{ return string.Empty; }
			
			return VersionPrefix + versionNumber.ToString().PadLeft(3, '0');
		}
		
		/// <param name="path">Path (with or without version info) of the path to add version info to.</param>
		/// <param name="versionNumber"></param>
		/// <returns>The provided path with the provided version number.</returns>
		public string AddOrChangeVersion(string path, int versionNumber)
		{
			if (string.IsNullOrWhiteSpace(path))
			{ throw new ArgumentNullException("path"); }
			
			if (versionNumber < 0)
			{ throw new ArgumentOutOfRangeException("versionNumber", "versionNumber must be >= 0"); }
			
			//Remove any version info if present to get the base file path.
			path = RemoveVersion(path);
			
			string ext = Path.GetExtension(path);
			string directory = Path.GetDirectoryName(path);
			
			//Remove the extension
			path = Path.GetFileNameWithoutExtension(path);
			//Create the version text
			string versionText = GenerateVersionText(versionNumber);
			//Combine the base path with the version text
			path = path + versionText;
			//Add the extension
			path = Path.ChangeExtension(path, ext);
			//Add the directory and return
			path = Path.Combine(directory, path);
			return path;
		}
		
		/// <returns>True if the provided path contains version information, false otherwise.</returns>
		public bool ContainsVersion(string path)
		{
			int rev;
			return TryParse(path, out rev);
		}
		
		/// <param name="path">Path (with or without version info) of the file to create the new version for.</param>
		/// <returns>Absolute path to a new version.</returns>
		/// <remarks>
		/// The returned path is not created.
		/// </remarks>
		public string GetPathForNewVersion(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{ throw new ArgumentNullException("path"); }
		
			path = PathExtensions.GetRootedPath(path, RootPath);
			
			int latestVersionNumber;
			if (TryGetLatestVersion(path, out latestVersionNumber) == false)
			{ return path; }
			
			int newVersionNumber = latestVersionNumber + 1;
			return AddOrChangeVersion(path, newVersionNumber);
		}
		
		/// <summary>
		/// Try to get the latest version of a file.
		/// </summary>
		/// <param name="path">Path (with or without version info) of the file to check.</param>
		/// <param name="latestVersionPath">
		/// On success, absolute path to the latest version.
		/// On fail, null.
		/// </param>
		/// <param name="latestVersionNumber">
		/// On success, version number of the latest version.
		/// On fail, -1;
		/// </param>
		/// <returns>True if any versions exist, False if no versions found.</returns>
		public bool TryGetLatestVersion(string path, out string latestVersionPath, out int latestVersionNumber)
		{
			if (string.IsNullOrWhiteSpace(path))
			{ throw new ArgumentNullException("path"); }
			
			//Ensure the path is rooted
			path = PathExtensions.GetRootedPath(path, RootPath);
			
			//Initialize to -1 to signify no versions
			latestVersionNumber = -1;
			latestVersionPath = null;
			
			//Remove any version info if present to get the base file path.
			path = RemoveVersion(path);
			
			//If the file exists as is, (without version info), set the latest version to 0
			if (File.Exists(path))
			{ 
				latestVersionNumber = 0;
				latestVersionPath = path;
			}
			
			string ext = PathExtensions.GetDottedExtension(path);
			string directory = Path.GetDirectoryName(path);
			string fileNameWithoutExtOrVersion = Path.GetFileNameWithoutExtension(path);
			
			//Process all files matching the fileName
			string searchPattern = fileNameWithoutExtOrVersion + VersionPrefix + "*" + ext;
			Trace.TraceInformation(searchPattern);
			Trace.TraceInformation(directory);
			foreach (string currentFile in Directory.GetFiles(directory, searchPattern))
			{
				Trace.TraceInformation("Checking " + currentFile);
				//Debug.WriteLine("Checking " + currentFile);
				int currentVersionNumber;
				//If the file does not contain version info, skip it.
				if (TryParse(currentFile, out currentVersionNumber) == false)
				{ 
					Debug.WriteLine("Ignoring " + currentFile);
					continue; 
				}
				
				if (currentVersionNumber > latestVersionNumber)
				{
					latestVersionNumber = currentVersionNumber;
					latestVersionPath = currentFile;	
				}				
			}
			Trace.TraceInformation("Done checking files");
			return latestVersionNumber >= 0;
		}
		
		/// <summary>
		/// Try to get the latest version of a file.
		/// </summary>
		/// <param name="path">Path (with or without version info) of the file to check.</param>
		/// <param name="latestVersionPath">
		/// On success, path to the latest version.
		/// On fail, null.
		/// </param>
		/// <returns>True if any versions exist, False if no versions found.</returns>
		public bool TryGetLatestVersion(string path, out string latestVersionPath)
		{
			int latestVersionNumber;
			return TryGetLatestVersion(path, out latestVersionPath, out latestVersionNumber);
		}
		
		/// <summary>
		/// Try to get the latest version of a file.
		/// </summary>
		/// <param name="path">Path (with or without version info) of the file to check.</param>
		/// <param name="latestVersionNumber">
		/// On success, version number of the latest version.
		/// On fail, -1;
		/// </param>
		/// <returns>True if any versions exist, False if no versions found.</returns>
		public bool TryGetLatestVersion(string path, out int latestVersionNumber)
		{
			string latestVersionPath;
			return TryGetLatestVersion(path, out latestVersionPath, out latestVersionNumber);
		}
	}
}
