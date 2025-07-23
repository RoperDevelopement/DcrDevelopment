/*
 * User: Sam Brinly
 * Date: 12/21/2012
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace EdocsUSA.Utilities.Extensions
{
	public static class FileExtensions
	{
		public static void AppendLine(string path, string line)
		{
			string[] lines = new string[1] {line};
			File.AppendAllLines(path, lines);
		}
		/*
		[SecurityCritical]
		public static void WriteAllBytes(string path, byte[] bytes, Progress progress, bool suppressProgressNotifications)
		{ WriteAllBytes(path, bytes, 1024, progress, suppressProgressNotifications); }
		
		[SecurityCritical]
		public static void WriteAllBytes(string path, byte[] bytes, int chunkSize, Progress progress, bool suppressProgressNotifications)
		{
			if (path == null)
			{ throw new ArgumentNullException("path"); }
			if (path.Length == 0)
			{ throw new ArgumentException("empty", "bytes"); }
			if (bytes == null)
			{ throw new ArgumentNullException("bytes"); }
			
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
			using (BinaryWriter writer = new BinaryWriter(fileStream))
			{
				int progressTotal = bytes.Length;
				int progressCurrent = 0;
				int bytesWritten = 0;
				int bytesLeft = bytes.Length;
				while (bytesLeft > 0)
				{
					chunkSize = Math.Min(chunkSize, bytesLeft);
					writer.Write(bytes, bytesWritten, chunkSize);
					bytesWritten += chunkSize;
					bytesLeft -= chunkSize;
					
					progressCurrent = bytesWritten;
					progress.NotifyProgressChanged(progressCurrent, progressTotal);
				}
			}
				       
		}
		*/
		/// <summary>
		/// Creates a random empty file on the filesystem.
		/// </summary>
		/// <param name="rootPath">Path to the folder to create the random file in.</param>
		/// <param name="ext">File extension for the new file.</param>
		/// <returns>Full path to the random file.</returns>
		/// <remarks>
		/// Does not overwrite existing files.
		/// Creates rootPath if it does not exist.
		/// </remarks>
		/// <exception cref="IOException">
		/// An I/O error occurred while opening the file.
		/// Although the method ensures that the generated file path does not exist before writing, 
		/// an external process could potentially write a file with an identical path 
		/// between the path generation and the file creation.
		/// </exception>
		public static string CreateRandomFile(string rootPath, string ext)
		{
			Directory.CreateDirectory(rootPath);
			string filePath = NextFileName(rootPath, ext);

			using (FileStream fs = File.Open(filePath, FileMode.CreateNew)) {}
			return filePath;
		}
		
		/// <summary>
		/// Generate a random file name that does not currently exist on the filesystem.
		/// </summary>
		/// <param name="rootPath">Path to the folder to create the random file in.</param>
		/// <param name="ext">File extension for the new file.</param>
		/// <returns>Full path to the random file.</returns>
		/// <remarks>
		/// File names are GUID strings.
		/// Checks for pre-existing files, but does not guarantee the file will not exist.
		/// Does not create rootPath if it does not exist.
		/// </remarks>
		public static string NextFileName(string rootPath, string ext)
		{
			string fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), ext);
			string filePath;
			do
			{ filePath = Path.Combine(rootPath, fileName); }
			while (File.Exists(filePath));
			
			return filePath;
		}
	}
}
