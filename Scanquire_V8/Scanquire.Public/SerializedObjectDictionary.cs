using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
	/// <summary>Serializes a directory of object to a local directory.</summary>
    /// <remarks>Does not handle updates to the individual objects.</remarks>
	public class SerializedObjectDictionary<T> where T:class
	{
		private Dictionary<string, T> Items = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public event EventHandler CollectionUpdated;
		
		protected void NotifyCollectionUpdated()
        {
            EventHandler handler = CollectionUpdated;
            if (handler != null)
            { handler(this, null); }
        }
		
		private string _DirectoryPath;
        /// <summary>Path to the directory to serialize the objects to.</summary>
        /// <remarks>Directory should be dedicated to objects of the provided type.</remarks>
		public virtual string DirectoryPath 
		{ 
			get { return _DirectoryPath; }
			set { _DirectoryPath = value; }
		}
		
        /// <summary>Retrieve the value that corresponds to the provided key.</summary>
        /// <returns>
        /// If the key exists: the value that corresponds to the key.
        /// If the key does not exist: null.
        /// </returns>
		public T this[string key]
		{
			get 
			{
				if (ContainsKey(key)) return Items[key];
				else return null;
			}
			set
			{
				Serialize(key, value);
				Items[key] = value;
                NotifyCollectionUpdated();
			}
		}
		
		public ICollection<string> Keys
		{ get { return Items.Keys; } }
		
		public ICollection<T> Values
		{ get { return Items.Values; } }
		
        /// <remarks>
        /// Intended for initialization from serializer only.
        /// For manual creation, use one of the other constructors.
        /// </remarks>
		public SerializedObjectDictionary()
		{ Refresh(); }
		
		public SerializedObjectDictionary(string directoryPath)
		{
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"SerializedObjectDictionary {directoryPath}");

            DirectoryPath = directoryPath;
			Refresh();
		}
		
        /// <summary>Ensure that the provided key exists in the dictionary.</summary>
        /// <exception cref="KeyNotFoundException">If the key does not exist in the dictionary.</exception>
		public void EnsureKeyExists(string key)
		{
			if (ContainsKey(key) == false)
			{
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Key " + key + " does not exist");
                throw new KeyNotFoundException("Key " + key + " does not exist");
            }
		}
		
		public bool ContainsKey(string key)
		{ return Items.ContainsKey(key); }
		
        /// <summary>Remove an object from the dictionary.</summary>
        /// <param name="key">The dictionary key of the object to remove.</param>
        /// <returns>
        /// On success: true.
        /// On failure (key not found, or error deleting): false.
        /// </returns>
        /// <remarks>Also deletes the corresponding file.</remarks>
		public bool Remove(string key)
		{
			if (ContainsKey(key) == false)
			{
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Attempted to remove non-existant key " + key);
				return false;
			}

            string objectFilePath = GetFilePathFromKey(key);
            try
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Deleting file " +objectFilePath);
                File.Delete(objectFilePath);
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Error deleting key file " + objectFilePath);
                EDL.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                return false;
            }

			bool removed = Items.Remove(key);
            NotifyCollectionUpdated();
			return removed;
		}
		
        /// <summary>Try to get a value from the dictionary.</summary>
        /// <param name="key">Dictionary key of the value.</param>
        /// <param name="value">
        /// On success: The value of the provided key.
        /// On failure: null.
        /// </param>
        /// <returns>
        /// On success: True.
        /// On failure (key not found): False.
        /// </returns>
		public bool TryGetValue(string key, out T value)
		{
			if (ContainsKey(key) == false) 
			{
				value = null;
				return false;
			}
			
			value = this[key];
			return true;
		}
		
		public Dictionary<string, T>.Enumerator GetEnumerator()
		{ return Items.GetEnumerator(); }
				
        /// <summary>Clear all entries from the dictionary and reload it from the directory.</summary>
		public void Refresh()
		{
			Items.Clear();
			
			if (Directory.Exists(DirectoryPath) == false)
			{
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Directory not found " + DirectoryPath);
                return;
			}
			
            //Loop through each xml file in the directory and attempt to deserialize.
			foreach (string serializedFile in Directory.GetFiles(DirectoryPath, "*.xml"))
			{
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Processing " + serializedFile);
                T value;
                if (TryDeserialize(serializedFile, out value) == true)
                { Items[GetKeyFromFilePath(serializedFile)] = value; }
                else
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Ignoring " + serializedFile);
                }
			}

            NotifyCollectionUpdated();
		}
		
        /// <returns>The dictionary key for a given file.</returns>
		protected string GetKeyFromFilePath(string path)
		{ return Path.GetFileNameWithoutExtension(path); }
		
        /// <returns>The absolute path to the file corresponding to the provided dictionary key.</returns>
		public string GetFilePathFromKey(string key)
		{ return Path.Combine(DirectoryPath, key + ".xml"); }
		
        /// <summary>Serialize an object to a file.</summary>
        /// <param name="key">The dictionary key for the object (also the serialized file name).</param>
        /// <param name="value">The object to serialize</param>
		protected void Serialize(string key, T value)
		{
            string path = GetFilePathFromKey(key);
			Serializer.Serialize(value, path);
		}
		
        /// <summary>Attempt to deserialize an individual object file.</summary>
        /// <param name="path">Path to the file to deserialize.</param>
        /// <param name="value">
        /// On success: The serialized file.
        /// On failure: null.
        /// </param>
        /// <returns>
        /// On success: true.
        /// On failure: false.
        /// </returns>
		protected bool TryDeserialize(string path, out T value)
		{
			try
			{
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Deserialize an individual object file " + path);
                value = Serializer.Deserialize<T>(path);
				return true;
			}
			catch (Exception ex)
			{
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Deserialization of " + path + " failed: " + ex.Message);
                
				if (ex.InnerException != null)
				{
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning(ex.InnerException.Message);
                }
				value = null;
				return false;
			}
		}
	}
}
