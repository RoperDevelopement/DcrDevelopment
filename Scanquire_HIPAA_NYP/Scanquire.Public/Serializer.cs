using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Polenter.Serialization;
using System.Diagnostics;
using ETL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
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
            //Set default serialization options and initialize the serializer.
			SharpSerializerXmlSettings serializerSettings = new SharpSerializerXmlSettings(){
				IncludeAssemblyVersionInTypeName = false,
				IncludeCultureInTypeName = false,
				IncludePublicKeyTokenInTypeName = false};
			SharpSerializer = new SharpSerializer(serializerSettings);
		}
		
        /// <summary>Serialize an object into a byte array.</summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>Byte array of the serialized data.</returns>
		public static byte[] Serialize(object value)
		{ 
			using (MemoryStream stream = new MemoryStream())
			{
				SharpSerializer.Serialize(value, stream);
				return stream.ToArray();
			}			
		}
		
        /// <summary>Serialize an object to a file.</summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="path">Path to serialize the object to.</param>
		public static void Serialize(object value, string path)
		{
            //Ensure that the directory exists.
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            SharpSerializer.Serialize(value, path); 
        }

        /// <summary>Attempt to deserialize an object from a file.</summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="path">Path of the file containing the serialized data.</param>
        /// <param name="value">On success: the deserialized object.</param>
        /// <returns>
        /// True if the deserialization succeeds.
        /// False if the deserialization fails.
        /// </returns>
        public static bool TryDeserialize<T>(string path, out T value)
        {
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Deserialize file:{path}");
            //Make sure the file exists.
            if (File.Exists(path) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Attempted to deserialize file that does not exist " + path);
                value = default(T);
                return false;
            }

            //Try to deserialize to generic object.
            object tValue = null;
            try
            { tValue = SharpSerializer.Deserialize(path); }
            catch (Exception ex)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Deserialization error");
                ETL.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                value = default(T);
                return false;
            }

            //Make sure the deserialized object is a T.
            if ((tValue is T) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Value deserialized from " + path + " is not a valid " + typeof(T).ToString());
                value = default(T);
                return false;
            }

            value = (T)tValue;
            return true;
        }

        /// <summary>Deserialize an object from a file.</summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="path">Path of the file containing the serialized data.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(string path)
        {
            T value;
            if (TryDeserialize<T>(path, out value) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceError("Could not deserialize " + path + " to a " + typeof(T).ToString());
                throw new Exception("Could not deserialize " + path + " to a " + typeof(T).ToString());
            }

            return value;
        }

        /// <summary>Attempt to deserialize an object from a stream.</summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="stream">Stream containing the serialized data.</param>
        /// <param name="value">On success: the deserialized object.</param>
        /// <returns>
        /// True if the deserialization succeeds.
        /// False if the deserialization fails.
        /// </returns>
        public static bool TryDeserialize<T>(Stream stream, out T value)
        {
            object tValue = null;
            try
            { tValue = SharpSerializer.Deserialize(stream); }
            catch (Exception ex)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Deserialization error");
                ETL.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                value = default(T);
                return false;
            }

            if ((tValue is T) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceWarning("Value deserialized from stream is not a valid " + typeof(T).ToString());
                value = default(T);
                return false;
            }

            value = (T)tValue;
            return true;
        }

        /// <summary>Deserialize an object from a stream.</summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="stream">Stream containing the serialized data.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(Stream stream)
        {
            T value;
            if (TryDeserialize<T>(stream, out value) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceError("Could not deserialize stream to " + typeof(T).ToString());
                throw new Exception("Could not deserialize stream to " + typeof(T).ToString());
            }

            return value;
        }

        /// <summary>Attempt to deserialize an object from a byte array.</summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="data">Byte array containing the serialized data.</param>
        /// <param name="value">On success: the deserialized object.</param>
        /// <returns>
        /// True if the deserialization succeeds.
        /// False if the deserialization fails.
        /// </returns>
        public static bool TryDeserialize<T>(byte[] data, out T value)
        {
            using (MemoryStream stream = new MemoryStream(data))
            { return TryDeserialize(stream, out value); }
        }

        /// <summary>Deserialize an object from a byte array.</summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="data">Byte array containing the serialized data.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(byte[] data)
        {
            T value;
            if (TryDeserialize<T>(data, out value) == false)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceError("Could not deserialize data to " + typeof(T).ToString());
                throw new Exception("Could not deserialize data to " + typeof(T).ToString());
            }

            return value;
        }
	}
}
