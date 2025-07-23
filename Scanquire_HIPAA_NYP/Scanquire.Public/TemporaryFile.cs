using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging;

namespace Scanquire.Public
{
    /// <summary>Class for managing temporary files</summary>
	public static class TemporaryFile
    {
        /// <summary>Root path to store all temporary files.</summary>
		private static string RootPath
        { get { return SettingsManager.TempDirectoryPath; } }

        /// <summary>
        /// If true, all temporary files will be encrypted.
        /// If false, all temporary files will be un-encrypted.
        /// </summary>
        /// <remarks>Configured in Properties.TemporaryFile.Encrypt</remarks>
        private static bool Encrypt
        { get { return Properties.TemporaryFile.Default.Encrypt; } }

        /// <returns>Absolue path to the provided path.</returns>
        private static string GetRootedPath(string path)
        {
            if (Path.IsPathRooted(path) == false) path = Path.Combine(RootPath, path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting root path:{path}");
            return path;
        }

        /// <returns>Path to the provided path relative to RootPath</returns>
        public static string GetRelativePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                path = path.Replace(RootPath, string.Empty);
                if (path.StartsWith(@"\")) path = path.TrimStart(new char[] { '\\' });

            }
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting relative path:{path}");
            return path;
        }

        /// <summary>Create a random temporary file and write all provided bytes to it.</summary>
        /// <param name="directory">Subdirectory (of RootPath) to write the file to, or null to write directly to RootPath</param>
        /// <param name="ext">File extension of the temporary file, or null for no extension.</param>
        /// <returns>Absolute path to the written file.</returns>
        public static string WriteAllBytesToRandom(byte[] data, string directory = null, string ext = null)
        {
            directory = GetRootedPath(directory);
            string path = FileExtensions.CreateRandomFile(directory, ext);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Writing out bytes to file:{path}");
            WriteAllBytes(path, data);
            return path;
        }

        /// <summary>Write all provided bytes to the specified path.</summary>
        /// <param name="path">Relative (from RootPath) or absolute path to the file to write.</param>
        /// <param name="data">Data to write to the file.</param>
        /// <remarks>Overwrites any previous file.</remarks>
        public static void WriteAllBytes(string path, byte[] data)
        {
            path = GetRootedPath(path);
            if (Encrypt) data = Encryption.Encrypt(data, DataProtectionScope.CurrentUser);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Writing all bytes to " + path); 
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, data);
        }

        /// <summary>Read the binary contents of a temporary file.</summary>
        /// <param name="path">Relative (from RootPath) or absolute path to the file to read.</param>
        /// <returns>Data contained in the specified file.</returns>
        public static byte[] ReadAllBytes(string path)
        {
            path = GetRootedPath(path);
            EnsureFileExists(path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Reading all bytes from " + path);
            byte[] data = File.ReadAllBytes(path);
            if (Encrypt) data = Encryption.Decrypt(data, DataProtectionScope.CurrentUser);
            return data;
        }

        /// <param name="path">Relative (from RootPath) or absolute path to the file to write.</param>
        /// <param name="value">Contents to write to the file.</param>
        /// <remarks>Overwrites any previous file.</remarks>
        public static void WriteAllText(string path, string value)
        {
            path = GetRootedPath(path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Writing txt to file:" + path);
            if (Encrypt)
            {
                byte[] encryptedValue = Encryption.Encrypt(value, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(path, encryptedValue);
            }
            else File.WriteAllText(path, value);
        }

        /// <summary>Write all provided text to a random temporary file.</summary>
        /// <param name="directory">Subdirectory (of RootPath) to write the file to, or null to write directly to RootPath</param>
        /// <param name="ext">File extension of the temporary file, or null for no extension.</param>
        /// <returns>Absolute path to the written file.</returns>
        public static string WriteAllTextToRandom(string value, string directory = null, string ext = null)
        {
            directory = GetRootedPath(directory);
            string path = FileExtensions.CreateRandomFile(directory, ext);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Write all text to random" + path);
            WriteAllText(path, value);
            return path;
        }

        /// <summary>Read all text from a temporary file.</summary>
        /// <param name="path">Relative (from RootPath) or absolute path to the file to read.</param>
        /// <returns>Text contents of the file.</returns>
        public static string ReadAllText(string path)
        {
            path = GetRootedPath(path);
            EnsureFileExists(path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Read all text to from file:" + path);
            if (Encrypt)
            {
                byte[] data = File.ReadAllBytes(path);
                return Encryption.DecryptToString(data, DataProtectionScope.CurrentUser);
            }
            else return File.ReadAllText(path);
        }

        /// <summary>Read all lines from a temporary file.</summary>
        /// <param name="path">Relative (from RootPath) or absolute path to the file to read.</param>
        /// <returns>Text lines from the file.</returns>
		public static IEnumerable<string> ReadAllLines(string path)
        {
            path = GetRootedPath(path);
            EnsureFileExists(path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Read all lines to from file:" + path);
            if (Encrypt)
            {
                string value = ReadAllText(path);
                string[] newLine = new string[] { Environment.NewLine.ToString() };
                return value.Split(newLine, StringSplitOptions.None);
            }
            else return File.ReadAllLines(path);
        }

        /// <summary>Write all lines to the specified temporary file.</summary>
        /// <param name="path">Relative (from RootPath) or absolute path to the file to read.</param>
        /// <param name="lines">Text lines to write to the file.</param>
        public static void WriteAllLines(string path, IEnumerable<string> lines)
        {
            path = GetRootedPath(path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Write all lines to file:" + path);
            StringBuilder valueBuilder = new StringBuilder();
            foreach (string line in lines) { valueBuilder.AppendLine(line); }
            WriteAllText(path, valueBuilder.ToString());
        }

        /// <summary>Write all lines to a random temporary file.</summary>
        /// <param name="lines">Text lines to write to the temporary file.</param>
        /// <param name="directory">Subdirectory (of RootPath) to write the file to, or null to write directly to RootPath</param>
        /// <param name="ext">File extension of the temporary file, or null for no extension.</param>
        /// <returns>Absolute path to the random file.</returns>
		public static string WriteAllLinesToRandom(IEnumerable<string> lines, string directory = null, string ext = null)
        {
            directory = GetRootedPath(directory);
            string path = FileExtensions.CreateRandomFile(directory, ext);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Wrtie all line random to file:" + path);
            WriteAllLines(path, lines);
            return path;
        }

        /// <summary>Create a random empty (zero byte) temporary file. </summary>
        /// <param name="directory">Subdirectory (of RootPath) to write the file to, or null to write directly to RootPath</param>
        /// <param name="ext">File extension of the temporary file, or null for no extension.</param>
        /// <returns>Absolute path to the created file.</returns>
        public static string CreateEmptyRandom(string directory = null, string ext = null)
        {
            directory = GetRootedPath(directory);
            string path = FileExtensions.CreateRandomFile(directory, ext);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Creating randome empty zero byte temporary file:" + path);
            return path;
        }

        /// <param name="path">Relative (to RootPath) or absolute path of the file to check.</param>
        /// <returns>
        /// If the file exists: true.
        /// If the file does not exist: false.
        /// </returns>
        public static bool Exists(string path)
        {
            if (path.IsEmpty())
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Path cannot be empty");
                return false;
            }
            path = GetRootedPath(path);
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation("Path exists:" + path);
            return File.Exists(path);
        }

        /// <summary>Ensures that a temporary file exists</summary>
        /// <param name="path">Relative (to RootPath) or absolute path of the file to check.</param>
        /// <exception cref="FileNotFoundException">If the temporary file does not exist.</exception>
        public static void EnsureFileExists(string path)
        {
            if (Exists(path) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceError("Cannot find temporary file " + path);
                throw new FileNotFoundException("Cannot find temporary file " + path);
            }
        }

        /// <summary>Delete a temporary file.</summary>
        /// <param name="path">Relative (to RootPath) or absolute path of the file to check.</param>
        public static void Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Delete requested with no path provided");
                return;
            }

            path = GetRootedPath(path);
            if (File.Exists(path) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Delete requested on a non-existing file " + path);
                return;
            }

            try
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Delete file " + path);
                File.Delete(path);
            }
            catch (Exception ex)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Unable to delete temporary file " + path);
                ETL.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
            }
        }
    }
}
