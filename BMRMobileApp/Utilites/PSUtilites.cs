using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BMRMobileApp.SQLiteDBCommands;

namespace BMRMobileApp.Utilites
{
    public class PSUtilites
    {
        public const SQLite.SQLiteOpenFlags Flags =
         // open the database in read/write mode
         SQLite.SQLiteOpenFlags.ReadWrite |
         // create the database if it doesn't exist
         SQLite.SQLiteOpenFlags.Create |
         // enable multi-threaded database access
         SQLite.SQLiteOpenFlags.SharedCache;
        public static int UserID { get; set; } // Static property to hold the user ID   
        public static string GenerateSalt(int size = 32)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
        public static (string Salt, string Hash) HashPassword(string password, int saltSize = 32)
        {
            byte[] saltBytes = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);
            string saltedPassword = password + salt;

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            string hash = Convert.ToBase64String(hashBytes);

            return (salt, hash);
        }

        public static bool VerifyPassword(string password, string salt, string storedHash)
        {
            string saltedPassword = password + salt;
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            string computedHash = Convert.ToBase64String(hashBytes);

            return computedHash == storedHash;
        }
        public static async Task GetUserID(string psUserFN,string psUserLN)
        {
            var sqliteService = new SQLiteService();
            UserID = sqliteService.GetPSUserById(psUserFN, psUserLN).Result?.ID ?? 0;
        }
        public static async Task<byte[]> ConvertStringToBye(string byteStr)
        {
            return string.IsNullOrEmpty(byteStr) ? null : Convert.FromBase64String(byteStr);
        }
        public static async Task<string> ConvertByteToStr(byte[] byteStr)
        {
            return byteStr == null ? null : Convert.ToBase64String(byteStr);
        }
          
        public static async Task<byte[]> ConvertImageToBytes(FileResult file)
        {
            if (file == null)
                return null;

            using var stream = await file.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static async Task<byte[]> ConvertImageToBlod(string img)
        {
            if (string.IsNullOrEmpty(img))
                return null;

            //  using var stream = new FileStream(img, FileMode.OpenOrCreate);

            // using var memoryStream = new MemoryStream();
            // await stream.CopyToAsync(memoryStream);
            // return memoryStream.ToArray();
            return File.ReadAllBytes(img);
        }
        public static async Task<ImageSource> ConvertBlodToImageSource(byte[] img)
        {
            try
            { 
           
            if (img == null)
                return null;

            ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(img));
            return imageSource;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        public static ImageSource NoTaskConvertBlodToImageSource(byte[] img)
        {
            try
            {

                if (img == null)
                    return null;

                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(img));
                return imageSource;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
 
