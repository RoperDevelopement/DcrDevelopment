/*
 * User: Sam Brinly
 * Date: 11/18/2013
 */
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace EdocsUSA.Utilities.Extensions
{
	public static class DirectoryExtensions
	{
		/// <summary>Returns the files in the specified directory that match the specified regular expression.</summary>
		/// <param name="path"></param>
		/// <param name="pattern"></param>
		/// <param name="searchOption"></param>
		/// <returns></returns>
		/// <remarks>Automatically prepends (?i) (case insensitive) to pattern if not present</remarks>
		public static string[] GetFilesRegex(string path, string pattern, SearchOption searchOption)
		{ 
			if (pattern.StartsWith("?i") == false)
			{ pattern = "(?i)" + pattern; }
			return Directory.GetFiles(path, "*", searchOption).Where(file=>Regex.IsMatch(file, pattern)).ToArray(); 
		}
		
		public static string[] GetFilesRegex(string path, string pattern)
		{ return GetFilesRegex(path, pattern, SearchOption.TopDirectoryOnly); }
		
		public static bool CanWrite(string path)
		{ return CanAccess(path, FileIOPermissionAccess.Write); }
		
		public static bool CanRead(string path)
		{ return CanAccess(path, FileIOPermissionAccess.Read);  }
		
		public static bool CanAccess(string path, FileIOPermissionAccess accessRequested)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			FileIOPermission writePermission = new FileIOPermission(accessRequested, path);
			permissionSet.AddPermission(writePermission);
			
			return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
		}
	}
}
