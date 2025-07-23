using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Edocs.Clients.NYP.L8SpecimenBatchUploader
{
    static class ProtectedDataHelper
    {
        public static string ProtectString(string value, string entropy, DataProtectionScope scope)
        {
            if (string.IsNullOrWhiteSpace(value))
            { throw new ArgumentException("value required", "value"); }

            byte[] userData = Encoding.Unicode.GetBytes(value);
            byte[] entropyData = null;
            if (string.IsNullOrWhiteSpace(entropy) == false)
            { entropyData = Encoding.Unicode.GetBytes(entropy); }

            byte[] protectedData = ProtectedData.Protect(userData, entropyData, scope);
            return Convert.ToBase64String(protectedData);
        }

        public static string UnProtectString(string value, string entropy, DataProtectionScope scope)
        {
            if (string.IsNullOrWhiteSpace(value))
            { throw new ArgumentException("value required", "value"); }

            byte[] protectedData = Convert.FromBase64String(value);
            byte[] entropyData = null;
            if (string.IsNullOrWhiteSpace(entropy) == false)
            { entropyData = Convert.FromBase64String(entropy); }

            byte[] unprotecredData = ProtectedData.Unprotect(protectedData, entropyData, scope);
            return Encoding.Unicode.GetString(unprotecredData);
        }
    }
}
