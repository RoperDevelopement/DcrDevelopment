/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 8:44 AM
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Utilities
{
	public static class StringTools
	{
		#region Extension Methods
		
		/// <summary>Determine if a string is empty.</summary>
		/// <returns>True if the specified string contains no content, false otherwise.</returns>
		/// <remarks>Checks null, string.empty and ""</remarks>
		public static bool IsEmpty(this string s)
        { return s == null || s == string.Empty || s.Trim() == ""; }

		/// <summary>Determine if a string has content.</summary>
		/// <returns>True if the string contains content, false otherwise.</returns>
		/// <remarks>See is empty for checks.</remarks>
        public static bool IsNotEmpty(this string s) { return !s.IsEmpty(); }
        
        public static string ConvertFrom(object value)
        {
        	//TODO:Account for nullable types
        	if (value == null) return null;
        	TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
        	return (string)converter.ConvertToString(value);
        }
        
        /// <summary>Convert a string</summary>
        public static T ConvertTo<T>(this string value)
        {
        	//TODO:Account for nullable types
        	if (string.IsNullOrWhiteSpace(value)) return TypeExtensions.GetDefaultValue<T>();
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
			return (T)(converter.ConvertFromString(value));
        }
        
        /// <summary>Convert the string to an object.</summary>
        /// <returns>The object converted from the string.</returns>
        /// <remarks>Empty strings or "NULL" are returned as null.</remarks>
        [Obsolete]
        public static T ConvertToObject<T>(this string s) where T: class
        {
        	if ((s.IsEmpty()) || (s.ToUpper() == "NULL")) return null;
        	else 
        	{
        		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        		return (T)(converter.ConvertFromString(s));
        	}
        }
        
        /// <summary>Convert the string to a nullable struct.</summary>
        /// <returns>The converted value type.</returns>
        /// /// <remarks>Empty strings or "NULL" are returned as null.</remarks>
        [Obsolete]
        public static T? ConvertToStruct<T>(this string s) where T: struct
        {
        	if ((s.IsEmpty()) || (s.ToUpper() == "NULL")) return null;
        	else
        	{
        		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        		return (T)(converter.ConvertFromString(s));
        	}
        }
        
        /// <summary>Determine if the specified string matches a regular expression pattern</summary>
        public static bool MatchesRegex(this string s, string pattern)
        {
            Regex rex = new Regex(pattern);
            return rex.IsMatch(s);
        }
        
        public static string RemoveLineBreaks(this string s, string replacementString)
        { return Regex.Replace(s, @"\r\n?|\n", replacementString); }
        
        /// <summary>Calculate the MD5 hash of the string.</summary>
        /// <param name="s"></param>
        /// <returns>MD5 hash of the string.</returns>
        public static string CalculateMD5(this string s)
        {
            return CalculateMD5(ASCIIEncoding.Default.GetBytes(s));
        }
        
        #endregion Extension Methods
        
        /// <summary>Generate a random string</summary>
        /// <param name="minLength">Minimum allowable length</param>
        /// <param name="maxLength">Maximum allowable length</param>
        /// <returns>Random string</returns>
        /// <remarks>Character pool = {bcdfghkmnpqrswxzBCDFGHKMNPQRSWXZ23456789!@#$%&*}</remarks>
        public static string GenerateRandomString(int minLength, int maxLength)
        {
            string charPool = "bcdfghkmnpqrswxz"
                                + "BCDFGHKMNPQRSWXZ"
                                + "23456789"
                                + "!@#$%&*";

            return GenerateRandomString(minLength, maxLength, charPool);
        }
        
        /// <summary>Generate a random string</summary>
        /// <param name="minLength">Minimum allowable length</param>
        /// <param name="maxLength">Maximum allowable length</param>
        /// <returns>Random string</returns>
        public static string GenerateRandomString(int minLength, int maxLength, string charPool)
        {
        	Random rand = new Random();
            int length = rand.Next(minLength, maxLength);

            string password = "";
            for (int charIndex = 0; charIndex < length; charIndex++)
            {
                password += charPool[rand.Next(0, charPool.Length)];
            }
            return password;
        }

        /// <summary>Generate the MD5 has of a byte array</summary>
        public static string CalculateMD5(byte[] value)
        {
            byte[] encodedBytes = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(value);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encodedBytes.Length; i++)
            { sb.Append(encodedBytes[i].ToString("x2")); }
            
            return sb.ToString();
        }        
        
        /// <summary>
        /// Generate string of separated values (ie csv)
        /// </summary>
        /// <param name="separator">Separator to place between each element</param>
        /// <param name="separatorReplacement">In case the separator is encountered in a value, replace it with this.</param>
        public static string GenerateSeparatedList(IEnumerable<string> values, string separator, string separatorReplacement)
        {
        		StringBuilder csvBuilder = new StringBuilder();
	        	using (IEnumerator<string> enumerator = values.GetEnumerator())
	        	{
	        		if (enumerator.MoveNext())
	        		{csvBuilder.Append(enumerator.Current.Replace(separator, separatorReplacement)); }
	        		while (enumerator.MoveNext())
	        		{ csvBuilder.Append(separator + enumerator.Current.Replace(separator, separatorReplacement)); }
	        	}
	        	return csvBuilder.ToString();
        }
        
      	/// <summary>
       /// Generate comma separated value list.
       /// </summary>
       /// <remarks>Separators found in the values will be replaced with a single space.</remarks>
       public static string GenerateSeparatedList(IEnumerable<string> values, string separator)
       { return GenerateSeparatedList(values, separator, " "); }
       
       /// <summary>
       /// Generate comma separated value list.
       /// </summary>
       /// <remarks>Commas found in the values will be replaced with a single space.</remarks>
    	 public static string GenerateSeparatedList(IEnumerable<string> values)
		{ return GenerateSeparatedList(values, ","); }
	}
}
