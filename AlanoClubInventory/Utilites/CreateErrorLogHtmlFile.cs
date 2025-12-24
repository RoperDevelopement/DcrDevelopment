using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlanoClubInventory.Utilites
{
    public class CreateErrorLogHtmlFile
    {
        public async Task CreatLogHtmlFile(IList<ErrorLogEntry> errorLogs,string filePath)
        {
            CreateHtmlEMails htmlEMails = new CreateHtmlEMails();
            string header = $"Database Error Report Run Date {DateTime.Now.ToString("MM-dd-yyyy")}<br/> Total Sql Messages {errorLogs.Count()}";
            
            StringBuilder sb = await htmlEMails.CreateHeader(DateTime.Now,header,  "pack://application:,,,/Resources/Images/butteac.ico");
            sb = await htmlEMails.CloseHtmlFile(sb);
            sb.AppendLine("<br/><br/>");
            sb.AppendLine("<table>");
            sb.AppendLine($"<tr style={Utilites.AlanoCLubConstProp.DoubleQuotes}text-align:center;{Utilites.AlanoCLubConstProp.DoubleQuotes}>");
            sb.AppendLine($"<th style={Utilites.AlanoCLubConstProp.DoubleQuotes}background-color: cornsilk;{Utilites.AlanoCLubConstProp.DoubleQuotes}>Log Date</th>");
            sb.AppendLine($"<th style={Utilites.AlanoCLubConstProp.DoubleQuotes}background-color: cornsilk;{Utilites.AlanoCLubConstProp.DoubleQuotes}>Process</th>");
            sb.AppendLine($"<th style={Utilites.AlanoCLubConstProp.DoubleQuotes}background-color: cornsilk;{Utilites.AlanoCLubConstProp.DoubleQuotes}>Message</th>");
            sb.AppendLine("</tr>");
            foreach (ErrorLogEntry errorLogEntry in errorLogs)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{errorLogEntry.LogDate.ToString()}</td>");
                sb.AppendLine($"<td>{errorLogEntry.ProcessInfo}</td>");
                sb.AppendLine($"<td>{errorLogEntry.Text}</td>");
                sb.AppendLine($"</tr>");
            }

            sb.AppendLine("</table>");
            SaveHtmlFile(filePath, sb);
            OpenHtmlFile(filePath);
        }
        private async void OpenHtmlFile(string filePath)
        {
            try
            {
              await  ALanoClubUtilites.StartProcess(filePath, string.Empty, false, true);
                ALanoClubUtilites.ShowMessageBox($"HTML File Saved under {filePath}", "PDF File", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ALanoClubUtilites.ShowMessageBox($"Unable to Open html file {filePath} {ex.Message}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        private async void SaveHtmlFile(string filePath,StringBuilder sb)
        {
            System.IO.File.WriteAllTextAsync(filePath, sb.ToString());
        }
    }
}
