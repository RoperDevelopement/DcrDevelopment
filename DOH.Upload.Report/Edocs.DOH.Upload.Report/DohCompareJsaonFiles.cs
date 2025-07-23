using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.DOH.Upload.Report.Models;
using Edocs.Send.Emails;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using EU = EdocsUSA.Utilities;
namespace Edocs.DOH.Upload.Report
{
    public class DohCompareJsaonFiles
    {
        private static DohCompareJsaonFiles instance = null;
        public static DohCompareJsaonFiles CompJsonInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DohCompareJsaonFiles();
                }
                return instance;
            }
        }

        private async Task<DOHIDImagesScanned> JsonIDImagesScanned(string spName, string sqlConnection, string city, string church, string bookType, string sDate, string eDate)
        {
            DOHIDImagesScanned dOHIDImages = new DOHIDImagesScanned();
            if (string.IsNullOrWhiteSpace(city))
                return dOHIDImages;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter(Utilities_Const.SqlParmaCity, city));
                        command.Parameters.Add(new SqlParameter(Utilities_Const.SqlParmaChurch, church));
                        command.Parameters.Add(new SqlParameter(Utilities_Const.SqlParmaBookType, bookType));
                        command.Parameters.Add(new SqlParameter(Utilities_Const.SqlParmaStartDate, sDate));
                        command.Parameters.Add(new SqlParameter(Utilities_Const.SqlParmaEndDate, eDate));
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                        using (SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            if (!reader.HasRows)
                            {
                                return dOHIDImages;
                            }
                            else
                            {
                                while (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                                {
                                    dOHIDImages.ID = reader.GetInt32(0);
                                    dOHIDImages.ImagesScanned = reader.GetInt32(1);
                                    dOHIDImages.PDFFIleName = reader.GetString(2);
                                    dOHIDImages.DateAdded = reader.GetDateTime(3);

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
            return dOHIDImages;

        }
        private async Task<int> GotTotalBooksScanned(string spName, string sqlConnection)
        {
           
            
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                        using (SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            if (!reader.HasRows)
                            {
                                return 0;
                            }
                            else
                            {
                                while (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                                {
                                    return reader.GetInt32(0);
                                    

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
            return 0;

        }
        private async Task UpDateCompareBit(string spName, string sqlConnection, int id)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter(Utilities_Const.SqlParmaID, id));
                        connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                        SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        reader.Close();
                        // Console.WriteLine(jsonResult.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Send_Emails.EmailInstance.SendEmail($"Getting documents that need downloaded {ex.Message}");
                throw new Exception($"Getting documents that need downloaded {ex.Message}");
            }


        }
        private async Task<StringBuilder> GetJsonInfo(string spName, string sqlConnection)
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
            if (docFound)
                //Send_Emails.EmailInstance.SendEmailTOCC(Utilities_Const.UtilityConstInstance.EmailTo,Utilities_Const.UtilityConstInstance.EmailTo, Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), "DOH Compare Json Files TO Uploaded Document Report", Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), true, string.Empty);
                Send_Emails.EmailInstance.SendEmailTOCC(Utilities_Const.UtilityConstInstance.EmailCC, Utilities_Const.UtilityConstInstance.EmailTo, Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), "DOH Compare Json Files TO Uploaded Document Report", Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), true, string.Empty);
            else
                Send_Emails.EmailInstance.SendEmailTOCC(Utilities_Const.UtilityConstInstance.EmailCC, Utilities_Const.UtilityConstInstance.EmailTo, Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), "DOH Compare Json Files TO Uploaded Document Report No Documents Uploaded", Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), true, string.Empty);

        }
        

        private async Task<StringBuilder> CreateHeder(DateTime uplloadDate, bool docsUploaded)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine($"<html lang={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}en{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes} xmlns={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}http://www.w3.org/1999/xhtml{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
            sb.AppendLine("<head>");
            sb.AppendLine($"<meta charset={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}utf-8{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}/>");
            sb.AppendLine("<title>DOH Compare Json Files</title>");
            sb.AppendLine("</head>");
            sb.AppendLine($"<body style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}background-color:lightgray{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");

            if (docsUploaded)
            {
                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>DOH Report Compare Images Uploaded to Images Scanned </h1>");
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
                sb.AppendLine("<th> Images Upload </th>");
                sb.AppendLine("<th> Document Name </th>");
                sb.AppendLine("<th> Date Uploaded </th>");
                sb.AppendLine("</tr>");
            }
            else
            {
                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>DOH Report Compare Images Uploaded to Images Scanned No Images Uploaded</h1>");
                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Report Run Date:{DateTime.Now}</h1>");
            }
            return sb;
        }
        private async Task Cleanup(IList<int> id, List<string> folders)
        {
            foreach (int ids in id)
            {
                UpDateCompareBit(Utilities_Const.SpUpDateJsonCompare, Utilities_Const.UtilityConstInstance.SqlServerConnStr, ids).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(Utilities_Const.UtilityConstInstance.JsonProcessFolder);
            foreach (string fName in folders)
            {
                string destFileName = Path.Combine(Utilities_Const.UtilityConstInstance.JsonProcessFolder, Path.GetFileName(fName));
                EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(fName, destFileName, true, true);
                string settingFname = Path.GetFileName(fName).Replace(Utilities_Const.UtilityConstInstance.RecJsonFileExt, Utilities_Const.UtilityConstInstance.SettingsJsonFileExt);
                destFileName = Path.Combine(Utilities_Const.UtilityConstInstance.JsonProcessFolder, settingFname);
                string sFName = Path.Combine(Path.GetDirectoryName(fName), settingFname);
                EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(sFName, destFileName, true, true);
            }
        }
        private async Task<StringBuilder> JsFilesNotFound(StringBuilder sb)
        {

            IList<DOHDownLoadModel> jsonUploaded = new List<DOHDownLoadModel>();
            string jsonString = GetJsonInfo(Utilities_Const.SpJsonFilesNotFound, Utilities_Const.UtilityConstInstance.SqlServerConnStr).ConfigureAwait(false).GetAwaiter().GetResult().ToString();
            var jString = JsonConvert.DeserializeObject<DOHDownLoadModel[]>(jsonString);
            jsonUploaded = jString.ToList();
            if ((jsonUploaded != null) && (jsonUploaded.Count() > 0))
            {
                sb.AppendLine($"<h1 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Json Files Not Found</h1>");
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
                sb.AppendLine("<th> Json FileName </th>");
                sb.AppendLine("<th> Date Uploaded </th>");
                sb.AppendLine("</tr>");

                foreach (DOHDownLoadModel dOHDownLoad in jsonUploaded)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td>{dOHDownLoad.City}</td>");
                    sb.AppendLine($"<td>{dOHDownLoad.Church}</td>");
                    sb.AppendLine($"<td>{dOHDownLoad.BookType}</td>");
                    sb.AppendLine($"<td>{dOHDownLoad.SDate} thru {dOHDownLoad.EDate} </td>");
                    sb.AppendLine($"<td>{dOHDownLoad.ImagesScanned}</td>");
                    sb.AppendLine($"<td>{dOHDownLoad.FName}</td>");
                    sb.AppendLine($"<td>{dOHDownLoad.FName.Replace(".pdf",Utilities_Const.UtilityConstInstance.RecJsonFileExt)}</td>");
                    sb.AppendLine($"<td>{dOHDownLoad.DateAdded.ToString("MM-dd-yyyyy")}</td>");
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table>");
            }
            return sb;
        }
        public async Task CompareJsonFiles()
        {
            StringBuilder sb = null;
            bool docsFound = false;
            int totalImages = 0;
            int totalImagesUpload = 0;
            IList<int> id = new List<int>();
            List<string> delUpLoadFile = new List<string>();
            string[] upLoadFile = EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.GetFiles(Utilities_Const.UtilityConstInstance.JsonFolder, $"*{Utilities_Const.UtilityConstInstance.RecJsonFileExt}");
            if (upLoadFile.Count() > 0)
            {
                docsFound = true;
                sb = CreateHeder(DateTime.Now, true).ConfigureAwait(false).GetAwaiter().GetResult();
                foreach (string file in upLoadFile)
                {

                    List<object> downLoadModel = Utilities_Const.UtilityConstInstance.GetRecords<DOHRecordsModel>(file).ConfigureAwait(false).GetAwaiter().GetResult();
                    //DOHRecordsModel recordsModel = new DOHRecordsModel();
                    var recordsModel = downLoadModel.First() as DOHRecordsModel;
                    DOHIDImagesScanned dOHIDImages = JsonIDImagesScanned(Utilities_Const.SpDOHGetJsonFileID, Utilities_Const.UtilityConstInstance.SqlServerConnStr, recordsModel.City, recordsModel.Church, recordsModel.BookType, recordsModel.DateRangeStartDate, recordsModel.DateRangeEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
                    var lines = File.ReadAllLines(file);
                    var count = lines.Length;
                    totalImages = totalImages + count;
                    if ((dOHIDImages != null) && (dOHIDImages.ID > 0))
                    {

                        delUpLoadFile.Add(file);
                        id.Add(dOHIDImages.ID);
                       
                        totalImagesUpload = totalImagesUpload + dOHIDImages.ImagesScanned;
                        if (count != dOHIDImages.ImagesScanned)
                            sb.AppendLine("<tr style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
                        else
                            sb.AppendLine("<tr>");
                        sb.AppendLine($"<td>{recordsModel.City}</td>");
                        sb.AppendLine($"<td>{recordsModel.Church}</td>");
                        sb.AppendLine($"<td>{recordsModel.BookType}</td>");
                        sb.AppendLine($"<td>{recordsModel.DateRangeStartDate} thru {recordsModel.DateRangeEndDate}</td>");
                        if (count == dOHIDImages.ImagesScanned)
                        {
                            sb.AppendLine($"<td>{count}</td>");
                            sb.AppendLine($"<td>{dOHIDImages.ImagesScanned}</td>");
                        }
                        else
                        {
                            sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Error: {count}</td>");
                            sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Error: {dOHIDImages.ImagesScanned}</td>");
                        }

                        sb.AppendLine($"<td>{dOHIDImages.PDFFIleName}</td>");
                        sb.AppendLine($"<td>{dOHIDImages.DateAdded.ToString("MM/dd/yyyy")}</td>");

                        sb.AppendLine($"</tr>");

                    }
                    else
                    {
                        sb.AppendLine("<tr style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>{recordsModel.City}</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>{recordsModel.Church}</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>{recordsModel.BookType}</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>{recordsModel.DateRangeStartDate} thru {recordsModel.DateRangeEndDate}</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>{count}</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>0</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>No Records upload For PDf File {file.Replace(Utilities_Const.UtilityConstInstance.RecJsonFileExt, ".pdf")}</td>");
                        sb.AppendLine($"<td style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}color:red{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>{DateTime.Now.ToString("MM/dd/yyyy")}</td>");
                    }
                }
                sb.AppendLine($"</table>");
            }

            else
            {
                sb = CreateHeder(DateTime.Now, false).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            if (totalImages != totalImagesUpload)
            {
                sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center;color:red;{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Error: Total Images Scanned {totalImages} Does Not Match Total Images Uploaded {totalImagesUpload}</h2>");
                sb.AppendLine($"<br />");

            }
            else
            {
                sb.AppendLine($"<br />");
                sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Total Images Scanned: {totalImages}</h2>");
                sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Total Images Uploaded: {totalImagesUpload}</h2>");
            }
            if ((id != null) && (id.Count() > 0))
            {
                if (Utilities_Const.UtilityConstInstance.EmailReport)
                    Cleanup(id, delUpLoadFile).ConfigureAwait(false).GetAwaiter().GetResult();
            }
                
            sb = JsFilesNotFound(sb).ConfigureAwait(false).GetAwaiter().GetResult();
            int tScanned = GotTotalBooksScanned(Utilities_Const.SpGetTotalBooksScanned, Utilities_Const.UtilityConstInstance.SqlServerConnStr).ConfigureAwait(false).GetAwaiter().GetResult();
            sb.AppendLine($"<br />"); 
            sb.AppendLine($"<br />");
            sb.AppendLine($"<h2 style={EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}text-align:center{EdocsUSA.Utilities.Edocs_Utilities.DoubleQuotes}>Total Books Scanned: {tScanned}</h2>");
            sb.AppendLine($"<br />");
            EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(Utilities_Const.UtilityConstInstance.ReportFolder);
            File.WriteAllText(Path.Combine(Utilities_Const.UtilityConstInstance.ReportFolder, Utilities_Const.UtilityConstInstance.JsonHtmlFileName), sb.ToString());
            if(Utilities_Const.UtilityConstInstance.EmailReport)
                SendEmail(docsFound).ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }
}
