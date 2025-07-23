using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using Edocs.DOH.Upload.Report.Models;
using Edocs.Send.Emails;
namespace Edocs.DOH.Upload.Report
{
    public class UpladedDOHDocuments
    {

        private async Task<StringBuilder> DownLoadDOHDOc(string spName, string sqlConnection)
        {
            StringBuilder jsonResult = new StringBuilder();
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                        using (SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            if (!reader.HasRows)
                            {
                                jsonResult.Append("[]");
                            }
                            else
                            {
                                while (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                                {
                                    jsonResult.Append(reader.GetString(0));
                                }
                            }
                        }

                        // Console.WriteLine(jsonResult.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Send_Emails.EmailInstance.SendEmail($"Getting documents that need downloaded {ex.Message}");
                throw new Exception($"Getting documents that need downloaded {ex.Message}");
            }
            return jsonResult;

        }
        private async Task SendEmail(bool docFound)
        {
            if(docFound)
            Send_Emails.EmailInstance.SendEmail(string.Empty, Utilities_Const.UtilityConstInstance.ReportName, "DOH Upload Document Report", Utilities_Const.UtilityConstInstance.ReportName, true, string.Empty);
            else
                Send_Emails.EmailInstance.SendEmail(string.Empty, Utilities_Const.UtilityConstInstance.ReportName, "DOH Upload Document Report No Documents Uploaded", Utilities_Const.UtilityConstInstance.ReportName, true, string.Empty);

        }
        private async Task<StringBuilder> CreateHeder(DateTime uplloadDate,bool docsUploaded)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine($"<html lang={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}en{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes} xmlns={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}http://www.w3.org/1999/xhtml{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
            sb.AppendLine("<head>");
            sb.AppendLine($"<meta charset={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}utf-8{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}/>");
            sb.AppendLine("<title>DOH Upload Report</title>");
            sb.AppendLine("</head>");
            sb.AppendLine($"<body style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}background-color:lightgray{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
            sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>DOH Documents Uploaded Report</h1>");
            if (docsUploaded)
            { 
            
            sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>DateUploaded:{uplloadDate.ToString("MM-dd-yyyyy")}</h1>");
            sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Report Run Date:{DateTime.Now}</h1>");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"<table border={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}1{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}  style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}background-color:#80daeb;margin-left:auto;margin-right:auto;{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th> City</th>");
            sb.AppendLine("<th> Church </th>");
            sb.AppendLine("<th> Book Type </th>");
            sb.AppendLine("<th> Date Range </th>");
            sb.AppendLine("<th> Images Scanned </th>");
            sb.AppendLine("<th> Document Name </th>");
            sb.AppendLine("<th> Date Uploaded </th>");
            sb.AppendLine("</tr>");
            }
            else
            {
                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Report Run Date:{DateTime.Now}</h1>");
            }
            return sb;
        }

        public async Task GetUploadedDocuments()
        {
            StringBuilder sb = null;
           IList <DOHDownLoadModel> uploaedDocs = new List<DOHDownLoadModel>();
            string jsonString = DownLoadDOHDOc(Utilities_Const.SpDOHUploadedDocsReport, Utilities_Const.UtilityConstInstance.SqlServerConnStr).ConfigureAwait(false).GetAwaiter().GetResult().ToString();
            var jString = JsonConvert.DeserializeObject<DOHDownLoadModel[]>(jsonString);
            uploaedDocs = jString.ToList();
            bool docFound = false;

            int totalImages = 0;
            if ((uploaedDocs != null) && (uploaedDocs.Count() > 0))
            {
                docFound = true;
                DOHDownLoadModel dOHDownLoad = uploaedDocs[0] as DOHDownLoadModel;
                sb = CreateHeder(dOHDownLoad.DateAdded,true).ConfigureAwait(false).GetAwaiter().GetResult();
                foreach (DOHDownLoadModel dOH in uploaedDocs)
                {
                    totalImages = totalImages + dOH.ImagesScanned;
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td>{dOH.City}</td>");
                    sb.AppendLine($"<td>{dOH.Church}</td>");
                    sb.AppendLine($"<td>{dOH.BookType}</td>");
                    sb.AppendLine($"<td>{dOH.SDate} thru {dOH.EDate}</td>");
                    sb.AppendLine($"<td>{dOH.ImagesScanned}</td>");
                    sb.AppendLine($"<td>{dOH.FName}</td>");
                    sb.AppendLine($"<td>{dOH.DateAdded.ToString("MM/dd/yyyyy")}</td>");
                    sb.AppendLine($"</tr>");
                }

                sb.AppendLine($"</table>");
                sb.AppendLine($"<br />");
                sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}> Total Images Scanned: {totalImages}</h2>");

            }
            else
            { 
                sb = CreateHeder(DateTime.Now,false).ConfigureAwait(false).GetAwaiter().GetResult();
                sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center;color:red;{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>No Documents Uploaded</h2>");
            }
            EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(Utilities_Const.UtilityConstInstance.ReportFolder);
            Utilities_Const.UtilityConstInstance.ReportName = Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.ReportName);
            System.IO.File.WriteAllText(Utilities_Const.UtilityConstInstance.ReportName, sb.ToString());
            if(Utilities_Const.UtilityConstInstance.EmailReport)
             SendEmail(docFound).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
