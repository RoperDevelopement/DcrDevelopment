using AlanoClubInventory.Models;
using iTextSharp.tool.xml.html;
using PdfSharp.Drawing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
 
using System.Windows.Media.Imaging;
using System.Xml.Linq;


namespace AlanoClubInventory.Utilites
{
    public   class ALanoClubUtilites
    {
        public const string ApplicationJsonFile = "applicationsettings.json";

        public static bool SentCodePW { get; set; } = false;
        public static string ApplicationWorkingFolder
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }
        public static string AlanoClubDatabaseName {  get; set; }
        public static int UserVolHrsID { get; set; } = 0;
        public static bool IsLoggin { get; set; } = false;
        public static string AlanoClubJsFile
        {
            get
            {

                if (File.Exists(Path.Combine(ApplicationWorkingFolder, ApplicationJsonFile)))
                    return Path.Combine(Path.Combine(ApplicationWorkingFolder, ApplicationJsonFile));
                else
                    return string.Empty;

            }
        }
        public async static void ShowMessageBox(string message, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            await Task.CompletedTask;
        }
        public async static Task<MessageBoxResult> ShowMessageBoxResults(string message, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            return messageBoxResult;

        }
        //public async static Task<MessageBoxResult> ShowMessageBoxResults(string message, string caption, MessageBoxButton messageBoxButton, MessageBoxIcon messageBoxIcon)
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show(message, caption, messageBoxButton, messageBoxIcon);
        //    return messageBoxResult;

        //}
        public async static Task<float> ConvertToFloat(string floatStr)
        {
            if (!string.IsNullOrEmpty(floatStr))
            {
                if (float.TryParse(floatStr, out float value))
                {
                    return value;
                }
            }
            return -1f;
        }


        public static async Task<string> GetConnectionStr()
        {


            // System.Threading.Thread.Sleep(10000);
            ReadJsonFile readJson = new ReadJsonFile();
            var appSettings = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            if (appSettings != null)
            {
                return appSettings.AlanoClubSqlServer;
            }
            await Task.CompletedTask;
            return string.Empty;
        }


        public async static void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

        }
        public async static Task<int> ConvertToInt(string intStr)
        {
            if (!string.IsNullOrEmpty(intStr))
            {
                if (int.TryParse(intStr, out int value))
                {
                    return value;
                }
            }
            return int.MaxValue;
        }
        public static bool GoHome { get; set; }

        public static string TruncateToTwoDecimalPlaces(float value)
        {
            return (Math.Truncate(value * 100) / 100).ToString("F2");
        }
        public static async Task<float> TruncateToTwoDecimalPlacesFloat(float value)
        {
            string retValue = (Math.Truncate(value * 100) / 100).ToString("F2");
            return await ConvertToFloat(retValue);
        }

        public static async Task<FlowDocument> AddBlankPar(FlowDocument document, int numberLines)
        {
            string crlf = AlanoCLubConstProp.CarrageReturnLineFeed;
            for (int i = 0; i < numberLines - 1; i++)
                crlf += AlanoCLubConstProp.CarrageReturnLineFeed;
            if (numberLines > 0)
            {
                Paragraph titleParagraph = new Paragraph();
                Run titleRun = new Run(crlf);
                titleParagraph.Inlines.Add(titleRun);
                document.Blocks.Add(titleParagraph);
            }

            return document;


        }
        public static async Task<bool> RexMatchStr(string str, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            bool matches = regex.IsMatch(str.ToLower());
            if (matches)
            {
                return true;
            }
            await Task.CompletedTask;
            return false;
        }
        public static string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            StringBuilder result = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[sizeof(uint)];
                while (result.Length < length)
                {
                    rng.GetBytes(buffer);
                    uint num = BitConverter.ToUInt32(buffer, 0);
                    result.Append(validChars[(int)(num % validChars.Length)]);
                }
            }
            return result.ToString();
        }
        public static string GetRandomNumer()
        {
            string uniqueSixDigit = (DateTime.UtcNow.Ticks % 1000000).ToString("D6");
            return uniqueSixDigit;
        }
        public static string ImageToString(string image)
        {
            return Convert.ToBase64String(File.ReadAllBytes(image));

        }
        public static string BitmapImageToString(BitmapImage image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Choose an encoder (PNG preserves quality and transparency)
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);

                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
        public static BitmapImage GetImageFromResouceFile(string image)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(image);
            bitmap.EndInit();
            return bitmap;
        }
        public static string RevStr(string str)
        {
            return new string(str.Reverse().ToArray());
        }
        public static string ACInventoryWF { get; set; }
        public static string GetTempPath => Path.GetTempPath();
        public static async Task<BitmapImage> ConvertImageBackToWPf(byte[] imageData)
        {
            var bitmap = new BitmapImage();
            using (var ms = new System.IO.MemoryStream(imageData))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
            }
            return bitmap;

        }
        public static async Task<byte[]> ConvertWpfImageToByte(BitmapImage imageData)
        {
            byte[] data;
            var encoder = new JpegBitmapEncoder(); // or PngBitmapEncoder
            encoder.Frames.Add(BitmapFrame.Create(imageData));

            using (var ms = new System.IO.MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
        public static void ResizeBitmapImage(BitmapImage image)
        {
            var scale = new System.Windows.Media.ScaleTransform(0.5, 0.5); // 50% smaller
            var resized = new TransformedBitmap(image, scale);
            resized.Freeze();

        }
        public static byte[] ResizeImage(string filePath, int maxWidth, int maxHeight, int quality = 70)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            // Calculate new size preserving aspect ratio
            double ratioX = (double)maxWidth / bitmap.PixelWidth;
            double ratioY = (double)maxHeight / bitmap.PixelHeight;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(bitmap.PixelWidth * ratio);
            int newHeight = (int)(bitmap.PixelHeight * ratio);

            // Draw into a resized RenderTargetBitmap
            var group = new System.Windows.Media.DrawingGroup();
            group.Children.Add(new System.Windows.Media.ImageDrawing(bitmap, new System.Windows.Rect(0, 0, newWidth, newHeight)));

            var drawingVisual = new System.Windows.Media.DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
                dc.DrawDrawing(group);

            var resized = new RenderTargetBitmap(newWidth, newHeight, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            resized.Render(drawingVisual);

            // Encode to JPEG
            var encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;
            encoder.Frames.Add(BitmapFrame.Create(resized));

            using (var ms = new System.IO.MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        public static async Task<AlanoClubInformaitonModel> GetAlnoClubInfo()
        {
            // string ACIconPath = "pack://application:,,,/Resources/Images/butteac.ico";
            ReadJsonFile readJson = new ReadJsonFile();

            var ac = readJson.GetJsonData<AlanoClubInformaitonModel>("AlanoClubInformaitonModel").Result;
            AlanoClubInformaitonModel AcInfo = new AlanoClubInformaitonModel
            {
                ClubName = $"{ac.ClubName}",
                ClubAddress = $"{ac.ClubAddress}",
                ClubPOBox = $"{ac.ClubPOBox}",
                ClubCity = $"{ac.ClubCity} {ac.ClubSt} {ac.ClubZipCode}",
                ClubPhone = $"Phone: {ac.ClubPhone}",
                ClubEmail = $"Email: {ac.ClubEmail}",
                FacebookLink = $"FaceBook Page: Butte Alano Club",
                FBLink = $"{ac.FacebookLink}"

            };
            return AcInfo;
        }
        public static async void DeleteFile(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch { }
        }
        //public static byte[] CompressImage(string filePath, long quality)
        //{
        //    using (var img = Image.FromFile(filePath))
        //    using (var ms = new System.IO.MemoryStream())
        //    {
        //        var encoder = ImageCodecInfo.GetImageDecoders()
        //            .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

        //        var encParams = new EncoderParameters(1);
        //        encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        //        img.Save(ms, encoder, encParams);
        //        return ms.ToArray();
        //    }
        //}

        //public static byte[] ResizeAndCompress(string filePath, int maxWidth, int maxHeight, long quality)
        //{
        //    using (var img = Image.FromFile(filePath))
        //    {
        //        // Calculate new dimensions while preserving aspect ratio
        //        double ratioX = (double)maxWidth / img.Width;
        //        double ratioY = (double)maxHeight / img.Height;
        //        double ratio = Math.Min(ratioX, ratioY);

        //        int newWidth = (int)(img.Width * ratio);
        //        int newHeight = (int)(img.Height * ratio);

        //        using (var bmp = new Bitmap(newWidth, newHeight))
        //        using (var g = Graphics.FromImage(bmp))
        //        {
        //            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //            g.DrawImage(img, 0, 0, newWidth, newHeight);

        //            using (var ms = new MemoryStream())
        //            {
        //                var encoder = ImageCodecInfo.GetImageDecoders()
        //                    .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

        //                var encParams = new EncoderParameters(1);
        //                encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        //                bmp.Save(ms, encoder, encParams);
        //                return ms.ToArray();
        //            }
        //        }
        public static void SaveBitmapImageToFile(BitmapImage bitmap, string filePath)
        {
            // Choose encoder based on desired format
            BitmapEncoder encoder = new PngBitmapEncoder(); // or JpegBitmapEncoder, BmpBitmapEncoder, etc.
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        public static string GetSqlConnectionStrings(string currentDataBase)
        {
            ReadJsonFile readJson = new ReadJsonFile();
            var appSettings = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            if (appSettings != null)
            {
                if (string.Compare(currentDataBase,AlanoCLubConstProp.AlanoClubDBName,true) == 0)
                    return (appSettings.AlanoClubSqlServer);
               else if (string.Compare(currentDataBase, "master", true) == 0)
                    return (appSettings.AlanoClubSqlServer.Replace(Utilites.AlanoCLubConstProp.AlanoClubDBName, "master"));
                else
                { 
                    return(appSettings.AlanoClubBackUpSqlConnection.Replace(Utilites.AlanoCLubConstProp.SqlConnBackupDataBase,currentDataBase));
                }
            }
            return (string.Empty);
        }
        public static (string, string) GetSqlConnectionStrings()
        {
            ReadJsonFile readJson = new ReadJsonFile();
            var appSettings = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            if (appSettings != null)
            {
                return (appSettings.AlanoClubSqlServer, appSettings.AlanoClubBackUpSqlConnection);
            }
            return (string.Empty, string.Empty);
        }
        /// <summary>
        /// Checks if all specified values exist in the comma-separated string.
        /// </summary>
        public static bool ContainsAllValues(string source, string[] values)
        {
            if (string.IsNullOrWhiteSpace(source) || values == null || values.Length == 0)
                return false;

            var items = source
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase); // Case-insensitive

            return values.All(v => items.Contains(v.Trim()));
        }

        /// <summary>
        /// Checks if any of the specified values exist in the comma-separated string.
        /// </summary>
        public static bool ContainsAnyValue(string source, string[] values)
        {
            if (string.IsNullOrWhiteSpace(source) || values == null || values.Length == 0)
                return false;

            var items = source
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return values.Any(v => items.Contains(v.Trim()));
        }
        public static bool ContainsAnyValue(string source, string[] values, string split)
        {
            if (string.IsNullOrWhiteSpace(source) || values == null || values.Length == 0)
                return false;

            var items = source
                .Split(split, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return values.Any(v => items.Contains(v.Trim()));
        }

        public static async Task StartProcess(string exeName, string arguments = null, bool showWindow = false, bool useShellExecute = false, bool waitForExit = false, int waitSecond = 60)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = exeName,
                    Arguments = arguments ?? string.Empty,
                    UseShellExecute = useShellExecute,
                    CreateNoWindow = !showWindow
                };

                var ps = Process.Start(startInfo);

                if ((ps != null) && (waitForExit))
                {
                    int waitTime = AlanoCLubConstProp.MilliSeconds * waitSecond;
                    ps.WaitForExit(waitTime);
                }
                if (ps != null)
                {
                    ps.Dispose();
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"Failed to launch process: {exeName} arg: {arguments} {ex.Message}");
            }

        }

        public static async Task<bool> CheckDriveSpaveMB(string driveName)
        {
            DriveInfo driveInfo = new DriveInfo(driveName);
            if(driveInfo.IsReady)
            {
                long availableBytes = driveInfo.AvailableFreeSpace;
                if (availableBytes > 0)
                {
                    long availableMB = availableBytes / (1024 * 1024);
                    if (availableMB < 1)
                    {
                        return false;
                    }
                }

                
                    
                }
            return true;
            }

        public static async Task<double> CheckDriveSpaveDB(string driveName)
        {
            DriveInfo driveInfo = new DriveInfo(driveName);
            if (driveInfo.IsReady)
            {
                //double totalSizeGB = driveInfo.TotalSize / Math.Pow(1024, 3);
                //double totalFreeGB = driveInfo.TotalFreeSpace / Math.Pow(1024, 3);
                //double availableFreeGB = driveInfo.AvailableFreeSpace / Math.Pow(1024, 3);
                //return availableFreeGB;
                long totalSize = driveInfo.TotalSize;          // in bytes
                long freeSpace = driveInfo.AvailableFreeSpace; // in bytes
                double totalSizeGB = totalSize / (1024.0 * 1024 * 1024);
                double freeSpaceGB = freeSpace / (1024.0 * 1024 * 1024);
                string fs = $"{freeSpaceGB:F2}";
                return double.Parse(fs);

            }
            return 0.00;
        }

        public static async Task<string> GetFiles(string folder, string extension)
        {
            return await GetFiles(folder, extension);
        }

        public static async Task DeleteOldFiles(string folder,int numDaysKeep,string extension)
        {
            foreach (string file in Directory.GetFiles(folder, extension))
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(file);
                    //var fileCrDate = fileInfo.CreationTime.Date;
                    TimeSpan ts = fileInfo.CreationTime.Date - DateTime.Now.Date;
                    if (ts.Days >= numDaysKeep)
                        fileInfo.Delete();
                }
                catch { }
            }

        }

        public static async Task RunBackUp(string dbBackUp, int numDaysBcakUp)
        {
            SqlServices.AlanoClubDatabaseMaintenanceService databaseMaintenanceService = new SqlServices.AlanoClubDatabaseMaintenanceService();

            try
            {
                FileInfo fileInfo = new FileInfo(dbBackUp);
                if (fileInfo.Exists)
                {
                    TimeSpan ts = fileInfo.CreationTime.Date - DateTime.Now.Date;
                    if (ts.Days >= numDaysBcakUp)
                        await databaseMaintenanceService.BackupDataBase();
                }
                else
                {

                    await databaseMaintenanceService.BackupDataBase();
                }
                //var fileCrDate = fileInfo.CreationTime.Date;
                //    TimeSpan ts = fileInfo.CreationTime.Date - DateTime.Now.Date;
                //  if (ts.Days > numDaysKeep)
                //    fileInfo.Delete();
            }
            catch { }

            finally
            
            {
                databaseMaintenanceService.Dispose();
            }

            }
        public static async Task<double> CalcVolTotalHours(DateTime clockedIn,DateTime clockedOut,double totalHours=1.00)
        {
            if(totalHours <= 0 )
                return 0;
            return Math.Round((clockedOut - clockedIn).TotalHours, 2);

        }
        public static string IsValidDateTime(string dt)
        {
            if(DateTime.TryParse(dt, out DateTime dateTime))
            {  return dateTime.ToString(); }
            return string.Empty;
        }
        public static string IsValidDateRange(DateTime sDT,DateTime eDt,int year=2025)
        {
            
            if(sDT > eDt) return $"Start Date Time {sDT} is bigger then End Date Time {eDt}";
            if(eDt < sDT) return $"End Date Time {eDt} is smaller then Start Date Time {sDT}"; 
            if (eDt == sDT) return $"Start Date Time {sDT} and  End Date Time {eDt} are the same";
            if ((sDT.Year < year) || (eDt.Year < year)) return $"Start Date Time year {sDT.Year} or End Date Time year is invalid {eDt.Year} need years bigger the {year}";
            return string.Empty;
        }

    }

}

 


     


 

