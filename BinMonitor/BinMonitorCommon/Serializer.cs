using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Polenter.Serialization;
using System.Diagnostics;

namespace BinMonitor.Common
{
    /// <summary>
    /// Handles serialization and deserialization of objects.
    /// </summary>
    /// <remarks>Uses SharpSerializer for default (XML) serialization</remarks>
    public static class Serializer
    {
        private static SharpSerializer SharpSerializer;

        static Serializer()
        {
            SharpSerializerXmlSettings serializerSettings = new SharpSerializerXmlSettings()
            {
                IncludeAssemblyVersionInTypeName = false,
                IncludeCultureInTypeName = false,
                IncludePublicKeyTokenInTypeName = false
            };
            SharpSerializer = new SharpSerializer(serializerSettings);
        }

        public static byte[] Serialize(object value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SharpSerializer.Serialize(value, stream);
                return stream.ToArray();
            }
        }

        public static void Serialize(object value, string path)
        {
            //Ensure that the directory exists.
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            SharpSerializer.Serialize(value, path);
        }

        public static bool TryDeserialize<T>(string path, out T value)
        {
            if (File.Exists(path) == false)
            {
                Trace.TraceWarning("Attempted to deserialize file that does not exist " + path);
                value = default(T);
                return false;
            }

            object tValue = null;
            try
            { tValue = SharpSerializer.Deserialize(path); }
            catch (Exception ex)
            {
                Trace.TraceWarning("Deserialization error");
                Trace.TraceError(ex.Message);
                value = default(T);
                return false;
            }

            if ((tValue is T) == false)
            {
                Trace.TraceWarning("Value deserialized from " + path + " is not a valid " + typeof(T).ToString());
                value = default(T);
                return false;
            }

            value = (T)tValue;
            return true;
        }

        public static T Deserialize<T>(string path)
        {
            T value;
            if (TryDeserialize<T>(path, out value) == false)
            { throw new Exception("Could not deserialize " + path + " to a " + typeof(T).ToString()); }

            return value;
        }

        public static bool TryDeserialize<T>(Stream stream, out T value)
        {
            object tValue = null;
            try
            { tValue = SharpSerializer.Deserialize(stream); }
            catch (Exception ex)
            {
                Trace.TraceWarning("Deserialization error");
                Trace.TraceError(ex.Message);
                value = default(T);
                return false;
            }

            if ((tValue is T) == false)
            {
                Trace.TraceWarning("Value deserialized from stream is not a valid " + typeof(T).ToString());
                value = default(T);
                return false;
            }

            value = (T)tValue;
            return true;
        }

        public static T Deserialize<T>(Stream stream)
        {
            T value;
            if (TryDeserialize<T>(stream, out value) == false)
            { throw new Exception("Could not deserialize stream to " + typeof(T).ToString()); }

            return value;
        }

        public static bool TryDeserialize<T>(byte[] data, out T value)
        {
            using (MemoryStream stream = new MemoryStream(data))
            { return TryDeserialize(stream, out value); }
        }

        public static T Deserialize<T>(byte[] data)
        {
            T value;
            if (TryDeserialize<T>(data, out value) == false)
            { throw new Exception("Could not deserialize data to " + typeof(T).ToString()); }

            return value;
        }
    }
}
