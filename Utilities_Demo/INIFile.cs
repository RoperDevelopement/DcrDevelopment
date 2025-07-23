/*
 * User: Sam
 * Date: 9/29/2011
 * Time: 8:35 AM
 */
using System;
using System.ComponentModel;
using System.Text;

using EdocsUSA.Utilities.Interop;

namespace EdocsUSA.Utilities
{
	/// <summary>
	/// Description of INIFile.
	/// </summary>
	public class INIFile
	{
		public string FilePath { get; set; }
		
		
		public INIFile(string path)
		{
			this.FilePath = path;
		}
		
		public void WriteKey<T>(string section, string key, T value)
		{
			string valueString = (value == null) ? "NULL": value.ToString();
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Writing key:{key} to section:{section} in ini file:{FilePath}");
			Kernel32.WritePrivateProfileString(section, key, valueString, this.FilePath);
		}
		
		private string ReadKeyString(string section, string key)
		{
			StringBuilder valueBuilder = new StringBuilder(255);
			Kernel32.GetPrivateProfileString(section, key, "INIKEYNOTFOUND", valueBuilder, 255, this.FilePath);
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Reading key:{key} to section:{section} in ini file:{FilePath}");
            return valueBuilder.ToString();
		}
		
		public T ReadKey<T>(string section, string key, T defaultValue)
		{
			string valueString = ReadKeyString(section, key);
			if (valueString.ToUpper().Equals("NULL"))
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning($"Key:{key}  section:{section} not found in ini file:{FilePath}");
                valueString = null;
            }
                
			if (valueString.ToUpper().Equals("INIKEYNOTFOUND"))
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning($"Key:{key}  section:{section} not found in ini file:{FilePath}");
                return defaultValue;
            }
                
			return valueString.ConvertTo<T>();
		}
		
		[Obsolete]
		public T? ReadStructKey<T>(string section, string key, T? defaultValue) where T: struct
		{
			string valueString = ReadKeyString(section, key);
			switch (valueString.ToUpper())
			{
				case "NULL": return null;
				case "INIKEYNOTFOUND": return defaultValue;
				default:
		            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
		            return (T)converter.ConvertFromString(valueString);
			}
		}
		
		[Obsolete]
		public T ReadClassKey<T>(string section, string key, T defaultValue) where T: class
		{
			string valueString = ReadKeyString(section, key);
			switch (valueString.ToUpper())
			{
				case "NULL": return null;
				case "INIKEYNOTFOUND": return defaultValue;
				default: 
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
		            return (T)converter.ConvertFromString(valueString);
			}
		}
	}
}
