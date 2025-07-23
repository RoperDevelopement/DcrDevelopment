using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinMonitor.Common
{
    static class Md5Util
    {
        public static string ComputeHash(string uid, Encoding encoding)
        {
            MD5 md5 = MD5.Create();
            byte[] data = encoding.GetBytes(uid);
            byte[] hashedData = md5.ComputeHash(data);
            return BitConverter.ToString(hashedData).Replace("-", string.Empty);
        }

        public static string ComputeHash(string value)
        { return ComputeHash(value, Encoding.ASCII); }
    }
}
