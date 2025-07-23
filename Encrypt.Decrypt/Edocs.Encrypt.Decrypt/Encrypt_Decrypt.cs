using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Edocs.Encrypt.Decrypt
{
    public static class Encrypt_Decrypt
    {
        public static string EncryptDecryptKey
        { get; set; }     
        private static byte[] ProtectedDataEntropy
        { get { return Convert.FromBase64String(EncryptDecryptKey); } }

        public static Encoding StringEncoding { get { return Encoding.Unicode; } }

        /// <summary>Encrypt a byte array to an encrypted byte array.</summary>
        /// <param name="data">Data to encrypt.</param>
        /// <returns>Encrypted byte array.</returns>
        public static byte[] Encrypt(byte[] data, DataProtectionScope scope)
        {
            if (string.IsNullOrWhiteSpace(EncryptDecryptKey))
                throw new Exception("Encrypt Decrypt Key cannot be null ");
            return ProtectedData.Protect(data, ProtectedDataEntropy, scope);
        }

        /// <summary>Encrypt a string to an encrypted byte array.</summary>
        /// <param name="data">String value to encrypt.</param>
        /// <returns>Encrypted byte array.</returns>
        public static byte[] Encrypt(string data, DataProtectionScope scope)
        {
            byte[] decryptedData = StringEncoding.GetBytes(data);
            return Encrypt(decryptedData, scope);
        }

        /// <summary>Encrypt a byte array of data to a string.</summary>
        /// <param name="data">Data to encrypt.</param>
        /// <returns>Encrypted string.</returns>
        public static string EncryptToString(byte[] data, DataProtectionScope scope)
        {
            byte[] encryptedData = Encrypt(data, scope);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>Encrypt a string to a string.</summary>
        /// <param name="data">Data to encrypt.</param>
        /// <returns>Encrypted string.</returns>
		public static string EncryptToString(string data, DataProtectionScope scope)
        {
            byte[] decryptedData = StringEncoding.GetBytes(data);
            return EncryptToString(decryptedData, scope);
        }

        /// <summary>Decrypt a byte array to a byte array.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrpted byte array.</returns>
		public static byte[] Decrypt(byte[] data, DataProtectionScope scope)
        {
            if (string.IsNullOrWhiteSpace(EncryptDecryptKey))
                throw new Exception("Encrypt Decrypt Key cannot be null ");
            return ProtectedData.Unprotect(data, ProtectedDataEntropy, scope);
        }

        /// <summary>Decrypt a string to a byte array.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrpted byte array.</returns>        
		public static byte[] Decrypt(string data, DataProtectionScope scope)
        {
            byte[] encryptedData = Convert.FromBase64String(data);
            return Decrypt(encryptedData, scope);
        }

        /// <summary>Decrypt a byte array to a string.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrypted string.</returns>
        public static string DecryptToString(byte[] data, DataProtectionScope scope)
        {
            byte[] decryptedData = Decrypt(data, scope);
            return StringEncoding.GetString(decryptedData);
        }

        /// <summary>Decrypt a string to a string.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrypted string.</returns>
        public static string DecryptToString(string data, DataProtectionScope scope)
        {
            byte[] encryptedData = Convert.FromBase64String(data);
            return DecryptToString(encryptedData, scope);
        }
    }
}
