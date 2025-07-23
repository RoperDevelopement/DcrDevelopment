/*
 * User: Sam Brinly
 * Date: 2/6/2013
 */
using System;
using System.IO;

namespace EdocsUSA.Utilities.Extensions
{
	public static class PathExtensions
	{
		/// <summary>Standardizes extension format to {.ext}</summary>
		[Obsolete("Ambiguous, use DotExtension or UnDotExtension")]
		public static string NormalizeExtension(string extension)
		{
			if (extension.StartsWith(".")) return extension.ToLower();
			else return "." + extension.ToLower();
		}
		
		public static string DotExtension(string extension)
		{
			if ((string.IsNullOrWhiteSpace(extension)) || (extension.StartsWith(".")))
			{ return extension; }
			else { return "." + extension; }
		}
		
		public static string UnDotExtension(string extension)
		{ return extension.TrimStart('.'); }
		
		public static string GetFilePathWithoutExtension(string path)
		{ return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)); }
		
		public static string GetDottedExtension(string path)
		{ return DotExtension(Path.GetExtension(path)); }
		
		public static string GetUnDottedExtension(string path)
		{ return UnDotExtension(Path.GetExtension(path)); }
		
		/// <param name="relativePath"></param>
		/// <param name="basePath"></param>
		/// <returns>
		/// If relativePath is already rooted, relativePath.
		/// If relativePath is not rooted, basePath combinded with relativePath
		/// </returns>
		public static string GetRootedPath(string relativePath, string basePath)
		{
			if (Path.IsPathRooted(relativePath))
			{ return relativePath; }
			else
			{ return Path.Combine(basePath, relativePath); }
		}
		
	}
}
