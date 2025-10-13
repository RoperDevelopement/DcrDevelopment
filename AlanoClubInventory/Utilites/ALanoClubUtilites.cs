using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlanoClubInventory.Utilites
{
  public  class ALanoClubUtilites
    {
        public const string ApplicationJsonFile = "applicationsettings.json";
      
        public static string ApplicationWorkingFolder
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }
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
        public async static void ShowMessageBox(string message,string caption,MessageBoxButton messageBoxButton,MessageBoxImage messageBoxImage)
        {
            MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            await Task.CompletedTask;
        }
        public async static Task<MessageBoxResult> ShowMessageBoxResults(string message, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            return messageBoxResult;
             
        }
        public async static Task<float> ConvertToFloat(string floatStr)
        {
            if(!string.IsNullOrEmpty(floatStr))
            {
                if(float.TryParse(floatStr, out float value))
                {
                    return value;
                }
            }
            return -1f;
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
    }
}
