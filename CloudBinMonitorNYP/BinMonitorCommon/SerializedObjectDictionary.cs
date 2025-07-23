using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    /// <summary>Serializes a directory of object to a local directory.</summary>
    /// <remarks>Does not handle updates to the individual objects.</remarks>
    public class SerializedObjectDictionary<T> where T : class
    {
        #region Events

        public class SerializedObjectEventArgs : EventArgs
        {
            public string Key { get; set; }

            public T Value { get; set; }

            public SerializedObjectEventArgs()
            { }

            public SerializedObjectEventArgs(string key, T value)
                : this()
            {
                this.Key = key;
                this.Value = value;
            }
        }

        public class SerializedObjectReplacedEventArgs : EventArgs
        {
            public string Key { get; set; }

            public T OldValue { get; set; }

            public T NewValue { get; set; }

            public SerializedObjectReplacedEventArgs()
            { }

            public SerializedObjectReplacedEventArgs(string key, T oldValue, T newValue)
            {
                this.Key = key;
                this.OldValue = oldValue;
                this.NewValue = newValue;
            }
        }

        public event EventHandler<SerializedObjectEventArgs> ObjectChanged;
        private void NotifyObjectChanged(string key, T value)
        {
            EventHandler<SerializedObjectEventArgs> handler = this.ObjectChanged;
            if (handler != null)
            { handler(null, new SerializedObjectEventArgs(key, value)); }
        }

        public event EventHandler<SerializedObjectEventArgs> ObjectAdded;
        protected void NotifyObjectAdded(string key, T value)
        {
            EventHandler<SerializedObjectEventArgs> handler = this.ObjectAdded;
            if (handler != null)
            { handler(null, new SerializedObjectEventArgs(key, value)); }
        }

        public event EventHandler<SerializedObjectEventArgs> ObjectRemoved;
        protected void NotifyObjectRemoved(string key, T value)
        {
            EventHandler<SerializedObjectEventArgs> handler = this.ObjectRemoved;
            if (handler != null)
            { handler(null, new SerializedObjectEventArgs(key, value)); }
        }

        public event EventHandler<SerializedObjectReplacedEventArgs> ObjectReplaced;
        protected void NotifyObjectReplaced(string key, T oldValue, T newValue)
        {
            EventHandler<SerializedObjectReplacedEventArgs> handler = this.ObjectReplaced;
            if (handler != null)
            { handler(null, new SerializedObjectReplacedEventArgs(key, oldValue, newValue)); }
        }

        public event EventHandler CollectionChanged;

        protected void NotifyCollectionChanged()
        {
            EventHandler handler = CollectionChanged;
            if (handler != null)
            { handler(this, null); }
        }

        #endregion Events

        #region Event Handlers

        public virtual void OnObjectChanged(string key, T value)
        {
            if (this.AutoSave)
            {
                this.SerializeWithLock(key, value);
            }
            this.NotifyObjectChanged(key, value);



            //this.NotifyCollectionChanged();
        }

        public virtual void OnObjectReplaced(string key, T oldValue, T newValue)
        {
            this.NotifyObjectReplaced(key, oldValue, newValue);
            this.NotifyCollectionChanged();
        }

        public virtual void OnObjectAdded(string key, T value)
        {
            this.RegisterObjectChangedEvents(key, value);
            this.NotifyObjectAdded(key, value);
            this.NotifyCollectionChanged();
        }

        public virtual void OnObjectRemoved(string key, T value)
        {
            this.NotifyObjectRemoved(key, value);
            this.NotifyCollectionChanged();
        }

        public virtual void OnCollectionChanged()
        { NotifyCollectionChanged(); }


        #endregion Event Handlers

        #region Properties

        protected object FileIOLock = new object();

        protected ConcurrentDictionary<string, T> Items = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public ICollection<string> Keys
        { get { return Items.Keys; } }

        public ICollection<T> Values
        { get { return Items.Values; } }

        private string _DirectoryPath;
        /// <summary>Path to the directory to serialize the objects to.</summary>
        /// <remarks>Directory should be dedicated to objects of the provided type.</remarks>
        public virtual string DirectoryPath
        {
            get { return _DirectoryPath; }
            set { _DirectoryPath = value; }
        }

        private string _ArchiveDirectoryPath = null;
        public virtual string ArchiveDirectoryPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ArchiveDirectoryPath))
                { _ArchiveDirectoryPath = Path.Combine(this.DirectoryPath, "Archive"); }
                return _ArchiveDirectoryPath;
            }
            set
            { _ArchiveDirectoryPath = value; }
        }

        private bool _TimestampArchivedFiles = false;
        public virtual bool TimestampArchivedFiles
        {
            get { return _TimestampArchivedFiles; }
            set { _TimestampArchivedFiles = value; }
        }

        private bool _AutoSave = true;
        /// <summary>When true, objects will be automatically saved when they are changed.</summary>
        /// <remarks>RegisterObjectChangedEvents and UnRegisterObjectEvents must be overriden to function.</remarks>
        public virtual bool AutoSave
        {
            get { return _AutoSave; }
            set { _AutoSave = value; }
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
                if (this.ContainsKey(key))
                { Replace(key, value); }
                else
                { Add(key, value); }
            }
        }

        #endregion Properties

        #region Constructors

        /// <remarks>
        /// Intended for initialization from serializer only.
        /// For manual creation, use one of the other constructors.
        /// </remarks>
        public SerializedObjectDictionary()
        { Reload(); }

        public SerializedObjectDictionary(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
            Reload();
        }

        #endregion Constructors

        #region Item Management

        public bool ContainsKey(string key)
        { return Items.ContainsKey(key); }

        /// <summary>Ensure that the provided key exists in the dictionary.</summary>
        /// <exception cref="KeyNotFoundException">If the key does not exist in the dictionary.</exception>
        public void EnsureKeyExists(string key)
        {
            if (ContainsKey(key) == false)
            { throw new KeyNotFoundException("Key " + key + " does not exist"); }
        }

        /// <summary>
        /// Ensure that the provided key does not exist</summary>
        /// </summary>
        /// <exception cref="InvalidOperationException">If the key does exist</exception>
        public void EnsureKeyDoesNotExist(string key)
        {
            if (ContainsKey(key) == true)
            { throw new InvalidOperationException("The requested operation cannot be performed on an existing item"); }
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

        /// <summary>Ensures that a key exists, and returns it</summary>
        /// <param name="key"></param>
        /// <returns>The value associated with the provided key</returns>
        /// <exception cref="KeyNotFoundException">When the key does not exist</exception>
        public T EnsureGetValue(string key)
        {
            T value;
            if (TryGetValue(key, out value) == false)
            { throw new KeyNotFoundException(key + " not found"); }
            else
            { return value; }
        }

        /// <summary>Add an item to the list with a key that does not exist</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException">If the key is already present</exception>
        public void Add(string key, T value)
        {
            EnsureKeyDoesNotExist(key);
            SerializeWithLock(key, value);
            RegisterObjectChangedEvents(key, value);
            this.Items[key] = value;
            OnObjectAdded(key, value);
        }

        /// <summary>Replace the value of an existing key</summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <exception cref="KeyNotFoundException">If the key does nto exist</exception>
        public void Replace(string key, T newValue)
        {
            T oldValue = this.EnsureGetValue(key);
            if (ReferenceEquals(oldValue, newValue) == false)
            {
                RegisterObjectChangedEvents(key, newValue);
            }
            SerializeWithLock(key, newValue);
            this.Items[key] = newValue;
            OnObjectReplaced(key, oldValue, newValue);
        }

        public void Save(string key)
        {
            T value = EnsureGetValue(key);
            Serialize(key, value);
        }

        public void SaveWithLock(string key)
        {
            lock (FileIOLock)
            { Save(key); }
        }

        /// <summary>Remove an object from the dictionary, but leave the serialized file</summary>
        /// <param name="key">The dictionary key of the object to remove.</param>
        /// <returns>
        /// On success: true.
        /// On failure: false.
        /// </returns>
        /// <remarks>Any call to Reload will re-instate the item.</remarks>
        public bool TryRemove(string key, out T oldValue)
        {
            if (Items.TryRemove(key, out oldValue))
            {
                OnObjectRemoved(key, oldValue);
                return true;
            }
            else return false;
        }

        public bool TryRemove(string key)
        {
            T value;
            return TryRemove(key, out value);
        }

        /// <summary>Remove an object from the dictionary and delete the assicaited file</summary>
        /// <param name="key"></param>
        /// <param name="oldValue"></param>
        /// <returns>
        /// On success: true.
        /// On failure: false.
        /// </returns>
        public bool TryDelete(string key, out T oldValue)
        {
            if (TryRemove(key, out oldValue))
            { return (TryDeleteObjectFile(key)); }
            else
            { return false; }
        }

        public void EnsureDelete(string key, out T oldValue)
        {
            if (TryDelete(key, out oldValue) == false)
            { throw new InvalidOperationException("Error deleting " + key); }
        }

        public bool TryDelete(string key)
        {
            T value;
            return TryDelete(key, out value);
        }

        public void EnsureDelete(string key)
        {
            if (TryDelete(key) == false)
            { throw new InvalidOperationException("Error deleting " + key); }
        }

        public bool TryDeleteWithLock(string key, out T oldValue)
        {
            lock (FileIOLock)
            { return TryDelete(key, out oldValue); }
        }

        public void EnsureDeleteWithLock(string key, out T oldValue)
        {
            if (TryDeleteWithLock(key, out oldValue) == false)
            { throw new InvalidOperationException("Error deleting " + key); }
        }

        public bool TryDeleteWithLock(string key)
        {
            T value;
            return TryDeleteWithLock(key, out value);
        }

        public void EnsureDeleteWithLock(string key)
        {
            if (TryDeleteWithLock(key) == false)
            { throw new InvalidOperationException("Error deleting " + key); }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        { return Items.GetEnumerator(); }

        /// <summary>Clear all entries from the dictionary and reload it from the directory.</summary>
        public void Reload()
        {
            if (Directory.Exists(DirectoryPath) == false)
            {
                Trace.TraceWarning("Directory not found " + DirectoryPath);
                return;
            }

            lock (FileIOLock)
            {
                foreach (KeyValuePair<string, T> item in this.Items.ToArray())
                {
                    T oldValue;
                    this.TryRemove(item.Key, out oldValue);
                }

                //Loop through each xml file in the directory and attempt to deserialize.
                foreach (string serializedFile in Directory.GetFiles(DirectoryPath, "*.xml"))
                {
                    Trace.TraceInformation("Processing " + serializedFile);
                    T value;
                    if (TryDeserialize(serializedFile, out value) == true)
                    {
                        string key = GetKeyFromFilePath(serializedFile);
                        this.Items[key] = value;
                        RegisterObjectChangedEvents(key, value);
                        OnObjectAdded(key, value);
                    }
                    else
                    { Trace.TraceInformation("Ignoring " + serializedFile); }
                }
            }

            OnCollectionChanged();
        }

        public bool CheckKey(string keyItem)
        {
            return ContainsKey(keyItem);

        }
        public void Reload(string itemKey)
        {

            if (Directory.Exists(DirectoryPath) == false)
            {
                Trace.TraceWarning("Directory not found " + DirectoryPath);
                return;
            }

            lock (FileIOLock)
            {
                //if (ContainsKey(itemKey))
                //{
                //    return true;
                //}
                string serializedFile = string.Format("{0}\\{1}.xml", DirectoryPath, itemKey);
                Trace.TraceInformation("Processing " + serializedFile);
                T value;
                T oldValue;
                this.TryRemove(itemKey, out oldValue);
                if (TryDeserialize(serializedFile, out value) == true)
                {
                    SpecimenBatch Batch = value as SpecimenBatch;
                    SpecimenBatches.Instance.Add(Batch.Id, Batch);

                    //string key = GetKeyFromFilePath(serializedFile);
                    //this.Items[key] = value;
                    //RegisterObjectChangedEvents(key, value);
                    //OnObjectAdded(key, value);

                }
                else
                { Trace.TraceInformation("Ignoring " + serializedFile); }
                //foreach (KeyValuePair<string, T> item in this.Items.ToArray())
                //{
                //    T oldValue;
                //    this.TryRemove(item.Key, out oldValue);
                //}

                //Loop through each xml file in the directory and attempt to deserialize.
                //foreach (string serializedFile in Directory.GetFiles(DirectoryPath, "*.xml"))
                //{
                //    Trace.TraceInformation("Processing " + serializedFile);
                //    T value;
                //    if (TryDeserialize(serializedFile, out value) == true)
                //    {
                //        string key = GetKeyFromFilePath(serializedFile);
                //        this.Items[key] = value;
                //        RegisterObjectChangedEvents(key, value);
                //        OnObjectAdded(key, value);
                //    }
                //    else
                //    { Trace.TraceInformation("Ignoring " + serializedFile); }
                //}
            }

            OnCollectionChanged();
             
        }
        #endregion Item Management

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

        protected void SerializeWithLock(string key, T value)
        {
            lock (FileIOLock)
            { Serialize(key, value); }
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
        public bool TryDeserialize(string path, out T value)
        {
            try
            {
                //string test = "<Complex type=\"BinMonitor.Common.User, BinMonitor.Common\" name=\"Root\">";
                //test += "< Properties> < Simple name =\"Id\" value=\"sam.brinly\"/>  < Simple name =\"FirstName\" value=\"SAM\"/>";
                //test += "< Simple name =\"LastName\" value=\"BRINLY\"/> < Simple name =\"UserProfileId\" value=\"ADMIN\"/> < Simple name =\"CardId\" value=\"12345\"/>";
                //test += "</ Properties ></ Complex >";
                //          byte[] byteArray = Encoding.ASCII.GetBytes(test);
                //MemoryStream mstream = new MemoryStream(byteArray);
                // value = Serializer.Deserialize<T>(path);

                value = Serializer.Deserialize<T>(path);
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Deserialization of " + path + " failed: " + ex.Message);
                if (ex.InnerException != null)
                { Trace.TraceWarning(ex.InnerException.Message); }
                value = null;
                return false;
            }
        }

        protected bool TryDeserializeWithLock(string path, out T value)
        {
            lock (FileIOLock)
            { return TryDeserialize(path, out value); }
        }

        protected bool TryDeleteObjectFile(string key)
        {
            string path = GetFilePathFromKey(key);
            if (File.Exists(path) == false)
            { return true; }

            File.Delete(path);

            return (File.Exists(path) == false);
        }

        protected bool TryDeleteObjectFileWithLock(string key)
        {
            lock (FileIOLock)
            { return TryDeleteObjectFile(key); }
        }

        protected void EnsureDeleteObjectFile(string key)
        {
            string path = GetFilePathFromKey(key);
            if (TryDeleteObjectFile(path) == false)
            { throw new IOException("Error deleting " + path); }
        }

        protected void EnsureDeleteObjectFileWithLock(string key)
        {
            lock (FileIOLock)
            { EnsureDeleteObjectFile(key); }
        }

        public bool TryExport(string key, string dest, out string message)
        {
            try
            {
                T value = this.EnsureGetValue(key);
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                Serializer.Serialize(key, dest);
                message = null;
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Failed to export " + key);
                Trace.TraceWarning(ex.Message);
                Trace.TraceWarning(ex.StackTrace);
                message = string.Format("Failed to export {0} - {1}", key, ex.Message);
                return false;
            }
        }

        public void Export(string key, string dest)
        {
            string message;
            if (this.TryExport(key, dest, out message) == false)
            { throw new Exception(message); }
        }

        public bool TryMoveToArchive(string key, out T value)
        {
            try
            {
                value = this.EnsureGetValue(key);

                Directory.CreateDirectory(this.ArchiveDirectoryPath);
                string originalFilePath = GetFilePathFromKey(key);
                string originalFileNameWithExt = Path.GetFileName(originalFilePath);

                string destFilePath = Path.Combine(ArchiveDirectoryPath, originalFileNameWithExt);

                Serializer.Serialize(value, destFilePath);

                return this.TryDelete(key, out value);
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Failed to archive " + key);
                Trace.TraceWarning(ex.Message);
                Trace.TraceWarning(ex.StackTrace);
                value = default(T);
                return false;
            }
        }

        public bool TryMoveToArchive(string key)
        {
            T value;
            return TryMoveToArchive(key, out value);
        }

        public bool TryMoveToArchiveWithLock(string key, out T value)
        {
            lock (FileIOLock)
            { return TryMoveToArchive(key, out value); }
        }

        public bool TryMoveToArchiveWithLock(string key)
        {
            T value;
            return TryMoveToArchiveWithLock(key, out value);
        }

        public T Get(string path)
        {
            T value;
            value = Serializer.Deserialize<T>(path);
            return value;
        }
        public void EnsureMoveToArchive(string key, out T value)
        {
            if (TryMoveToArchive(key, out value) == false)
            { throw new Exception("Failed to move item " + key + " to archive"); }
        }

        public void EnsureMoveToArchive(string key)
        {
            T value;
            EnsureMoveToArchive(key, out value);
        }

        public void EnsureMoveToArchiveWithLock(string key, out T value)
        {
            lock (FileIOLock)
            { EnsureMoveToArchive(key, out value); }
        }

        public void EnsureMoveToArchiveWithLock(string key)
        {
            T value;
            EnsureMoveToArchiveWithLock(key, out value);
        }

        protected virtual void RegisterObjectChangedEvents(string key, T value)
        { }
    }
}
