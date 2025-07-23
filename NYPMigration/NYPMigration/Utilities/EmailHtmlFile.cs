using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Send.Emails;
using EdocsUSA.Utilities;
using edl = EdocsUSA.Utilities.Logging;
namespace NYPMigration.Utilities
{
   public class EmailHtmlFile
    {
        private async Task<StringBuilder> CreateHeder(string scanDate,string dataBase )
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine($"<html lang={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}en{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes} xmlns={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}http://www.w3.org/1999/xhtml{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
            sb.AppendLine("<head>");
            sb.AppendLine($"<meta charset={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}utf-8{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}/>");
            sb.AppendLine("<title>DOH Upload Report</title>");
            sb.AppendLine("</head>");
            sb.AppendLine($"<body style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}background-color:lightgray{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
            sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Nyp Migration Errors for Database {dataBase} Total Errors: {PropertiesConst.PropertiesConstInstance.TotalErrors} </h1>");
            

                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Scan Date:{scanDate}</h1>");
                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Report Run Date:{DateTime.Now}</h1>");
                sb.AppendLine("<br/>");
                sb.AppendLine("<br/>");
                sb.AppendLine($"<table border={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}1{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}  style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}background-color:#80daeb;margin-left:auto;margin-right:auto;{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th> Error Message</th>");
                sb.AppendLine("</tr>");
            return sb;
        }
        public async Task SendEmail(string scanDate,string dataBase,bool logFileOpen=false)
        {
            StringBuilder sb = CreateHeder(scanDate, dataBase).ConfigureAwait(false).GetAwaiter().GetResult();
           try
            { 
            foreach(string err in PropertiesConst.PropertiesConstInstance.MigrationErrors)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{err}</td>");
                sb.AppendLine($"</tr>");

            }
            sb.AppendLine($"</table>");
            sb.AppendLine($"<br />");
            sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}> Total Errors: {PropertiesConst.PropertiesConstInstance.TotalErrors}</h2>");
            string errHtmlFile = System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.HtmlErrFile, DateTime.Now.ToString("yyyy-MM-dd"));
                 Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(errHtmlFile);
            errHtmlFile = System.IO.Path.Combine(errHtmlFile, $"{dataBase}_{scanDate}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.html");
            System.IO.File.WriteAllText(errHtmlFile, sb.ToString());
                if (logFileOpen)
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Sending email Errors found process database {dataBase} for scan date {scanDate} Running on Machine {Environment.MachineName} ");
            string eSub = $"Errors found process database {dataBase} for scan date {scanDate} Running on Machine {Environment.MachineName} ";
            Send_Emails.EmailInstance.SendNoCCEmail("dan.roper@edocsusa.com", sb.ToString(), eSub, errHtmlFile, true, string.Empty);
            }
            catch(Exception ex)
            {
                if(logFileOpen)
                {
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error sending email for scandate {scanDate} database {dataBase} {ex.Message}");
                }
            }
        }
    }
}
