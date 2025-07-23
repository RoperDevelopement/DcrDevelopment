/*
 * User: Sam Brinly
 * Date: 1/28/2013
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace EdocsUSA.Utilities
{
    public class SettingsFile
    {
        private Dictionary<string, string> Items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private bool _AutoSave = true;
        public bool AutoSave
        {
            get { return _AutoSave; }
            set { _AutoSave = value; }
        }

        private bool _ReadOnly = false;
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set { _ReadOnly = value; }
        }

        private string _FilePath = null;
        public string FilePath
        {
            get { return _FilePath; }
            protected set { _FilePath = value; }
        }

        public SettingsFile(string path)
        {
            FilePath = path;
            if (File.Exists(FilePath))
            { Load(); }
        }

        public void Load()
        {
            Items.Clear();
            if (File.Exists(FilePath) == false)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("SettingsFile " + FilePath + " does not exist");
                return;
            }
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Loading SettingsFile " + FilePath);
            foreach (string line in File.ReadAllLines(FilePath))
            {
                //Ignore blank lines
                if (string.IsNullOrWhiteSpace(line)) continue;

                //Ignore INI comment and section lines
                if (line.StartsWith("[") || line.StartsWith(";") || line.StartsWith("#")) continue;

                int separatorIndex = line.IndexOf('=');
                if (separatorIndex < 0)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceWarning(line + " is not a valid settings file line");
                    continue;
                }

                string key = line.Substring(0, separatorIndex);
                string value = line.Substring(separatorIndex + 1);

                Items[key] = value;
            }
        }

        public T ReadKey<T>(string key, T defaultValue, out bool found)
        {
            found = false;
            if (Items.ContainsKey(key))
            {
                found = true;
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter.CanConvertFrom(typeof(string)) == false)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceWarning(typeof(T) + " cannot be converted from string");
                    return defaultValue;
                }
                string stringValue = Items[key];

                if (converter.IsValid(stringValue) == false)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceWarning(stringValue + " is not a valid " + typeof(T));
                    return defaultValue;
                }

                found = true;
                return (T)(converter.ConvertFromString(stringValue));
            }
            else { return defaultValue; }
        }

        public T ReadKey<T>(string key, T defaultValue)
        {
            bool found;
            return ReadKey<T>(key, defaultValue, out found);
        }

        public void WriteKey(string key, object value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter.CanConvertTo(typeof(string)) == false)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("Cannot convert " + value.GetType() + " to string");
                throw new InvalidCastException("Cannot convert " + value.GetType() + " to string");
            }

            Items[key] = converter.ConvertToString(value);

            if (AutoSave && (ReadOnly == false)) Save();
        }

        public void Save()
        {
            if (ReadOnly)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("SettingsFile " + FilePath + " is marked ReadOnly");
                throw new InvalidOperationException("SettingsFile " + FilePath + " is marked ReadOnly");
            }

            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, string> item in Items)
                lines.Add(item.Key + "=" + item.Value);
            File.WriteAllLines(FilePath, lines);
        }
    }
}