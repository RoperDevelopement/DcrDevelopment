using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    /// <summary>Serializes a dictionary of SerializedObjectDictionary to a directory.</summary>
    /// <remarks>Does not handle updates to the individual objects.</remarks>
    /// <example>
    /// Assume you want to organize a group of Students by Family
    /// 
    /// </example>
    public class SerializedObjectDictionaryCollection<T> where T : class
    {
        private Dictionary<string, SerializedObjectDictionary<T>> Items = new Dictionary<string, SerializedObjectDictionary<T>>();

        /// <summary>Raised when an item is added or removed from the dictionary.</summary>
        public event EventHandler CollectionUpdated;
        protected void NotifyCollectionUpdated()
        { if (CollectionUpdated != null) CollectionUpdated(this, null); }

        private string _DirectoryPath;
        /// <summary>Path to the directory to serialize the objects to.</summary>
        /// <remarks>Directory should be dedicated to objects of the provided type.</remarks>
        public virtual string DirectoryPath
        {
            get { return _DirectoryPath; }
            set { _DirectoryPath = value; }
        }

        public SerializedObjectDictionary<T> this[string key]
        {
            get
            {
                if (ContainsKey(key) == false)
                { Items[key] = new SerializedObjectDictionary<T>(GetDirectoryFromKey(key)); }
                return Items[key];
            }
        }

        public ICollection<string> Keys
        { get { return Items.Keys; } }

        public ICollection<SerializedObjectDictionary<T>> Values
        { get { return Items.Values; } }

        /// <remarks>
        /// Intended for initialization from serializer only.
        /// For manual creation, use one of the other constructors.
        /// </remarks>
        public SerializedObjectDictionaryCollection()
        { Refresh(); }

        public SerializedObjectDictionaryCollection(string directoryPath)
        {
            DirectoryPath = directoryPath;
            Refresh();
        }

        public bool ContainsKey(string key)
        { return Items.ContainsKey(key); }

        /// <summary>Ensure that the provided key exists in the dictionary.</summary>
        /// <exception cref="KeyNotFoundException">If the key does not exist in the dictionary.</exception>
        public void EnsureKeyExists(string key)
        {
            if (ContainsKey(key) == false)
            { throw new KeyNotFoundException("Key " + key + " does not exist"); }
        }

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
                Trace.TraceWarning("Attempted to remove non-existant key " + key);
                return false;
            }

            string directoryPath = this[key].DirectoryPath;
            try
            { Directory.Delete(directoryPath); }
            catch (Exception ex)
            {
                Trace.TraceWarning("Error deleting directory " + directoryPath);
                Trace.TraceError(ex.Message);
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
        public bool TryGetValue(string key, out SerializedObjectDictionary<T> value)
        {
            if (ContainsKey(key) == false)
            {
                value = null;
                return false;
            }

            value = this[key];
            return true;
        }

        public Dictionary<string, SerializedObjectDictionary<T>>.Enumerator GetEnumerator()
        { return Items.GetEnumerator(); }

        /// <summary>Clear all entries and reload from the directory.</summary>
        public void Refresh()
        {
            Items.Clear();
            if (Directory.Exists(DirectoryPath) == false)
            {
                Trace.TraceWarning("Directory not found " + DirectoryPath);
                return;
            }

            //Loop through the subdirectories and attempt to load a SerializedObjectDictionary<T> for each
            foreach (string directory in Directory.GetDirectories(DirectoryPath))
            {
                Trace.TraceInformation("Processing " + directory);
                string key = GetKeyFromDirectory(directory);
                SerializedObjectDictionary<T> value;
                if (TryDeserialize(directory, out value) == false)
                { Trace.TraceInformation("Ignoring " + directory); }

                Items[key] = value;
            }
        }

        /// <summary>Attempt to deserialize an individual directory</summary>
        /// <param name="directory">Directory to deserialize.</param>
        /// <param name="value">
        /// On success: A SerializedObjectDictionary for the specified directory.
        /// On failure: null.
        /// </param>
        /// <returns>
        /// On success: true.
        /// On failure (Directory not found or deserialize error): false.
        /// </returns>
        protected bool TryDeserialize(string directory, out SerializedObjectDictionary<T> value)
        {
            try
            {
                if (Directory.Exists(directory) == false)
                { throw new DirectoryNotFoundException("Directory not found " + directory); }
                value = new SerializedObjectDictionary<T>(directory);
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Error deserializing " + directory);
                Trace.TraceError(ex.Message);
                value = null;
                return false;
            }
        }

        protected string GetKeyFromDirectory(string path)
        { return new DirectoryInfo(path).Name; }

        protected string GetDirectoryFromKey(string key)
        { return Path.Combine(DirectoryPath, key); }


    }
}
