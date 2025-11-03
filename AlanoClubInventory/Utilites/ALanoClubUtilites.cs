using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;

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


        public static async Task<string>  GetConnectionStr()
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
            string retValue= (Math.Truncate(value * 100) / 100).ToString("F2");
            return await ConvertToFloat(retValue);
        }

        public static async Task<FlowDocument> AddBlankPar(FlowDocument document,int numberLines)
        {
            string crlf = AlanoCLubConstProp.CarrageReturnLineFeed;
            for(int i=0;i<numberLines-1; i++)
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
        public static async Task<bool> RexMatchStr(string str,string pattern)
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
    }
}
