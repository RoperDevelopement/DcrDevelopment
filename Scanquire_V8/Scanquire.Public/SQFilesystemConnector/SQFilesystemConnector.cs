using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using System.Threading.Tasks;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    public class SQFilesystemConnector
    {
        public static string DefaultRootPath = @"C:\Archives\";

        private   string _RootPath = DefaultRootPath;
        public static bool ConvertedImages
        { get; set; }
        public static string ConvertedImagesFolder
        { get; set; }
        /// <summary>The directory to use as the initial directory for file dialogs, and for building relative paths.</summary>
        public   string RootPath
        {
            get { return _RootPath; }
            set { _RootPath = value; }
        }
        public static string[] RestorImagesPath
        { get; set; }
        public static string[] BackUpImagesInput
        { get; set; }
        private ExistingFileAction _ExistingFileAction = ExistingFileAction.AskUser;
        /// <summary>Action to take when a specified destination already exists.</summary>
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
                    ETL.TraceLoggerInstance.TraceError("value VersionPrefix cannot be empty");
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
                {
                    ETL.TraceLoggerInstance.TraceError("value VersionPadding must be >= 0");
                    throw new ArgumentOutOfRangeException("value", "VersionPadding must be >= 0");
                }

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

        private OpenFileDialog _OpenFileDialog = new OpenFileDialog() { RestoreDirectory = true, Multiselect = true };
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

            if ((RestorImagesPath != null) && (RestorImagesPath.Length > 0))
                return RestorImagesPath;
            //If the initial directory has not been set yet, use the archive root.
            if (string.IsNullOrWhiteSpace(OpenFileDialog.InitialDirectory))
            {
                ETL.TraceLoggerInstance.TraceInformation($"Opening file dialog for path:{RootPath}");
                OpenFileDialog.InitialDirectory = RootPath;
            }
            

            string[] paths = OpenFileDialog.SelectFiles(owner, context);
            string configFile = Path.Combine(Edocs_Utilities.EdocsUtilitiesInstance.GetApplicationDir(), "EdocsUSA.Utilities.dll.config");
            if (bool.Parse(Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting("BakupImages", configFile)))
                    BackUpImagesInput = paths;

            //If remember last directory is set, set the initialdirectory to the selected directory
            if (OpenFileDialogRememberLastDirectory)
            { OpenFileDialog.InitialDirectory = OpenFileDialog.GetCurrentDirectory(); }

            if (paths.Length > 1)
                ETL.TraceLoggerInstance.TraceInformation($"Getting files for path:{Path.GetDirectoryName(paths[0])}");
            else if (paths.Length > 0)
            {
                ETL.TraceLoggerInstance.TraceInformation($"Getting file :{paths[0]}");
            }
            else
                ETL.TraceLoggerInstance.TraceInformation($"User cancled selecting file for path:{RootPath}");
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
            ETL.TraceLoggerInstance.TraceInformation("Opening open file dialog");
            string[] paths = SelectFilesForOpen(owner, context);
            if ((paths == null) || (paths.Length <= 0))
            {
                ETL.TraceLoggerInstance.TraceInformation("Canceled open file dialog");
                return null;
            }
            else if (paths.Length > 1)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation requires a single file to be selected");
                throw new InvalidOperationException("The requested operation requires a single file to be selected");
            }
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
            public string AbsolutePath { get; set; }
            public string RelativePath { get; set; }
            public Uri AbsoluteUri { get; set; }
            public string RelativeUriPath { get; set; }
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, ExistingFileAction existingFileAction, IWin32Window owner, SynchronizationContext context)
        {
            //Ensure the path is rooted
            ETL.TraceLoggerInstance.TraceInformation("Checking root on " + path);
            string tPath = PathExtensions.GetRootedPath(path, RootPath);
            ETL.TraceLoggerInstance.TraceInformation("Rooted path is " + tPath);


            //Get the latest existing version (if any)
            int latestExistingVersionNumber;

            //Default new data to primary (0) version
            int newVersionNumber = 0;
            string newPath = AddOrChangeVersion(tPath, newVersionNumber);

            //Check for and get the latest existing version
            //If previous version(s) exist, handle based on existingFileAction
            if (TryGetLatestVersion(tPath, out latestExistingVersionNumber))
            {
                ETL.TraceLoggerInstance.TraceInformation($"File exists:{tPath}");

                //If ask user, do so and continue SaveFile with the new ExistingFileAction
                while (existingFileAction == EdocsUSA.Utilities.ExistingFileAction.AskUser)
                { existingFileAction = ExistingFileActionDialog.TryAskUser(owner, context); }

                switch (existingFileAction)
                {
                    //If newversion, increment the version number and generate the path.
                    case ExistingFileAction.NewVersion:
                        newVersionNumber = latestExistingVersionNumber + 1;
                        newPath = AddOrChangeVersion(tPath, newVersionNumber);
                        ETL.TraceLoggerInstance.TraceInformation($"Saving file as a new version:{newPath}");
                        break;
                    //If overwrite, generate the path from the latest version
                    case ExistingFileAction.OverwriteLatest:
                        newVersionNumber = latestExistingVersionNumber;
                        newPath = AddOrChangeVersion(tPath, newVersionNumber);
                        ETL.TraceLoggerInstance.TraceInformation($"OverWrite file:{tPath} with file as a new version:{newPath}");
                        break;
                    case ExistingFileAction.RequestRetry:
                        return new SaveFileResult()
                        {
                            Success = false,
                            RetryRequested = true,
                            AbsolutePath = null,
                            RelativePath = null,
                            AbsoluteUri = null,
                            RelativeUriPath = null,
                            VersionNumber = -1
                        };
                    default:
                        ETL.TraceLoggerInstance.TraceWarning("Unexpected ExistingFileAction " + existingFileAction);
                        return new SaveFileResult()
                        {
                            Success = false,
                            RetryRequested = false,
                            AbsolutePath = null,
                            RelativePath = null,
                            AbsoluteUri = null,
                            RelativeUriPath = null,
                            VersionNumber = -1
                        };
                }
            }
            else { ETL.TraceLoggerInstance.TraceInformation("File does not exist"); }

            //At this point we should be ready to save and have the final version and path information
            ETL.TraceLoggerInstance.TraceInformation($"Saving file as:{newPath}");
            await Task.Factory.StartNew(() =>
            {

                //Make sure the destination directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));

                //Write the file's contents
                File.WriteAllBytes(newPath, data);
            });
            ETL.TraceLoggerInstance.TraceInformation("Root Path " + RootPath);
            string absolutePath = newPath;
            string relativePath = absolutePath.Replace(RootPath, string.Empty);

            Uri baseUri = new Uri(RootPath, UriKind.Absolute);
            ETL.TraceLoggerInstance.TraceInformation("baseUri: " + baseUri.ToString());
            Uri absoluteUri = new Uri(absolutePath, UriKind.Absolute);

            ETL.TraceLoggerInstance.TraceInformation("absolutUri: " + absoluteUri.ToString());
            Uri relativeUri = baseUri.MakeRelativeUri(absoluteUri);

            string relativeUriPath = @"./" + relativeUri.ToString();
            ETL.TraceLoggerInstance.TraceInformation("relativeUriPath: " + relativeUriPath);


            return new SaveFileResult()
            {
                Success = true,
                RetryRequested = false,
                VersionNumber = newVersionNumber,
                AbsolutePath = absolutePath,
                RelativePath = relativePath,
                AbsoluteUri = absoluteUri,
                RelativeUriPath = relativeUriPath
            };
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, ExistingFileAction existingFileAction, IWin32Window owner)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, existingFileAction, owner, default(SynchronizationContext));
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, ExistingFileAction existingFileAction, SynchronizationContext context)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, existingFileAction, default(IWin32Window), context);
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, ExistingFileAction existingFileAction)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, existingFileAction, default(IWin32Window), default(SynchronizationContext));
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, IWin32Window owner, SynchronizationContext context)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, this.ExistingFileAction, owner, context);
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, IWin32Window owner)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, this.ExistingFileAction, owner, default(SynchronizationContext));
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data, SynchronizationContext context)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, this.ExistingFileAction, default(IWin32Window), context);
        }

        public async Task<SaveFileResult> SaveFile(string path, byte[] data)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Save file:{path}");
            return await SaveFile(path, data, this.ExistingFileAction, default(IWin32Window), default(SynchronizationContext));
        }

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
            ETL.TraceLoggerInstance.TraceInformation($"Parse the version information out of the provided path:{path}");
        /// <param name="path">Path to parse.</param>:{path}");
            if (string.IsNullOrWhiteSpace(path))
            {
                ETL.TraceLoggerInstance.TraceError("path is null");
                throw new ArgumentNullException("path cannot be null");
            }

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
                ETL.TraceLoggerInstance.TraceWarning($"No version location found for path:{path} version text:{versionText} version number:{versionNumber}");
                return false;
            }

            versionText = pathWithoutExt.Substring(versionLocation);
            ETL.TraceLoggerInstance.TraceWarning($"Version information found for path:{path} version text:{versionText} version number:{versionNumberDefault}");

            //Version number is everything after VersionPrefix
            string versionNumberText = versionText.Substring(VersionPrefix.Length);
            ETL.TraceLoggerInstance.TraceWarning($"Version information found for path:{path} version text:{versionNumberText} version number:{versionNumberDefault}");

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
            ETL.TraceLoggerInstance.TraceInformation($"parse the version information out of the provided path:{path}");
            if (string.IsNullOrWhiteSpace(path))
            {
                ETL.TraceLoggerInstance.TraceError("path cannot be empty");
                throw new ArgumentNullException("path");

            }

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
            ETL.TraceLoggerInstance.TraceInformation($"Parse the version information out of the provided path:{path}");
            string versionText;
            return TryParse(path, out versionText, out versionNumber);
        }

        /// <param name="path"></param>
        /// <returns>The provided path with all (if any) version info removed.</returns>
        public string RemoveVersion(string path)
        {
            ETL.TraceLoggerInstance.TraceInformation("Removing version from " + path);
            if (string.IsNullOrWhiteSpace(path))
            {
                ETL.TraceLoggerInstance.TraceError("path cannot be empty");
                throw new ArgumentNullException("path");
            }

            string versionText;
            //If the path does not contain version info, just return the provided path.
            if (TryParse(path, out versionText) == false)
            {
                ETL.TraceLoggerInstance.TraceWarning($"No version info found, returning path as is:{path}"); 
                return path;
            }

            string directory = Path.GetDirectoryName(path);
            string ext = Path.GetExtension(path);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);
            ETL.TraceLoggerInstance.TraceInformation("Path without ext " + fileNameWithoutExt);
            string fileNameWithoutVersionOrExt = fileNameWithoutExt.Substring(0, fileNameWithoutExt.Length - versionText.Length);
            string fileNameWithoutVersion = Path.ChangeExtension(fileNameWithoutVersionOrExt, ext);

            string filePath = Path.Combine(directory, fileNameWithoutVersion);
            ETL.TraceLoggerInstance.TraceInformation($"Removed version from:{filePath}");
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
            ETL.TraceLoggerInstance.TraceInformation($"Generate a version string from a version number:{versionNumber.ToString()}");
            if (versionNumber < 0)
            {
                ETL.TraceLoggerInstance.TraceError("versionNumber versionNumber must be >= 0");
                throw new ArgumentOutOfRangeException("versionNumber", "versionNumber must be >= 0");
            }

            if ((versionNumber == 0) && (AddVersionInfoToPrimaryVersion == false))
            { return string.Empty; }

            return VersionPrefix + versionNumber.ToString().PadLeft(3, '0');
        }

        /// <param name="path">Path (with or without version info) of the path to add version info to.</param>
        /// <param name="versionNumber"></param>
        /// <returns>The provided path with the provided version number.</returns>
        public string AddOrChangeVersion(string path, int versionNumber)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Path:{path} (with or without version info) of the path to add version info to verison:{versionNumber.ToString()}");
            if (string.IsNullOrWhiteSpace(path))
            {
                ETL.TraceLoggerInstance.TraceError("path cannot be empty");
                throw new ArgumentNullException("path");
            }

            if (versionNumber < 0)
            {
                ETL.TraceLoggerInstance.TraceError("versionNumber versionNumber must be >= 0");
                throw new ArgumentOutOfRangeException("versionNumber", "versionNumber must be >= 0");
            }

            //Remove any version info if present to get the base file path.
            ETL.TraceLoggerInstance.TraceError($"Remove any version info if present to get the base file path:{path}");
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
            ETL.TraceLoggerInstance.TraceError($"Return the changed version for the path:{path}");
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
            ETL.TraceLoggerInstance.TraceInformation($"Get latest file version for the :{path}");
            if (string.IsNullOrWhiteSpace(path))
            {
                ETL.TraceLoggerInstance.TraceError("Path cannot be empty");
                throw new ArgumentNullException("path");
            }

            //Ensure the path is rooted
            path = PathExtensions.GetRootedPath(path, RootPath);

            //Initialize to -1 to signify no versions
            latestVersionNumber = -1;
            latestVersionPath = null;

            //If the directory doesn't exist, just return false
            if (Directory.Exists(Path.GetDirectoryName(path)) == false)
            {
                ETL.TraceLoggerInstance.TraceError($"Directory not found for path:{path}");
                return false;
            }

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
            ETL.TraceLoggerInstance.TraceInformation($"Process all files matching the Search patteren:{searchPattern}");
            ETL.TraceLoggerInstance.TraceInformation($"Looking in directory:{directory}");
            foreach (string currentFile in Directory.GetFiles(directory, searchPattern))
            {
                ETL.TraceLoggerInstance.TraceInformation("Checking " + currentFile);

                //Debug.WriteLine("Checking " + currentFile);
                int currentVersionNumber;
                //If the file does not contain version info, skip it.
                if (TryParse(currentFile, out currentVersionNumber) == false)
                {
                    ETL.TraceLoggerInstance.TraceInformation("Ignoring " + currentFile+" does not contain a version number"); 
                    continue;
                }

                if (currentVersionNumber > latestVersionNumber)
                {
                    latestVersionNumber = currentVersionNumber;
                    latestVersionPath = currentFile;
                }
            }

            ETL.TraceLoggerInstance.TraceInformation($"Done checking version numbers for path:{path}");
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
            ETL.TraceLoggerInstance.TraceInformation($"latest version of a file for path:{path}");
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
            ETL.TraceLoggerInstance.TraceInformation($"latest version of a file for path:{path}");
            string latestVersionPath;
            return TryGetLatestVersion(path, out latestVersionPath, out latestVersionNumber);
        }
    }
}
