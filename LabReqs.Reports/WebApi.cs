using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;
//using System.Web.Script.Serialization;

namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    public class LabReqsUploaded
    {
        public string Uploaded
        { get; set; }
         
    }
    class WebApi
    {
        public const string ApiNypLabReqsReportsParam = "{0}/{1}/{2}/{3}/{4}";
       public const string Quote = "\"";
        public static int GetTotalRecords(string fileName)
        {
            if(File.Exists(fileName))
            return File.ReadAllLines(fileName).Count();
            return -100;
        }
        public static StringBuilder CreateHeder()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine($"<html lang={Quote}en{Quote} xmlns={Quote}http://www.w3.org/1999/xhtml{Quote}>");
            sb.AppendLine("<head>");
            sb.AppendLine($"<meta charset={Quote}utf-8{Quote}/>");
            sb.AppendLine("<title>LabReqs Upload Report</title>");
            sb.AppendLine("</head>");
            sb.AppendLine($"<body style={Quote}background-color:lightgray{Quote}>");
            sb.AppendLine($"<h1 style={Quote}text-align:center{Quote}>Document upload Report ScanDates:ScanStartDate - ScanEndDate</h1>");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"<table border={Quote}1{Quote}  style={Quote}background-color:aqua;margin-left:auto;margin-right:auto;{Quote}>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th> Scan Date </th>");
            sb.AppendLine("<th> Scan Batch ID </th>");
            sb.AppendLine("<th> AzureTableName </th>");
            sb.AppendLine("<th> Total Scanned </th>");
            sb.AppendLine("<th> Total Azure Cloud </th>");
            sb.AppendLine("</tr>");
            return sb;
        }
        public static T GetBatchSettingsObject<T>(string batchSettingsFile) where T : new()
        {
           // edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting batch setting for file{batchSettingsFile}");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object className = serializer.Deserialize<T>(File.ReadAllText(batchSettingsFile));
            return (T)className;
        }

        public static async Task<int> GetLabRecsSent(string webAPiUri, string controllerName, string batchId, string jsonFile, string reportSrch, int totalJsonFile, string dateUpLoaded)
        {
            int totalNumupLoaded = 0;
            try
            {
             
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = new Uri($"{webAPiUri}");
                        dateUpLoaded = dateUpLoaded.Replace("/", "-");
                    //  edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab recs for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate} tableName {tableName}");
                    //var responseTask = client.GetAsync($"{controllerName}/{batchId}/{jsonFile}/{reportSrch}/{totalJsonFile}/{dateUpLoaded}");
                    string apiParms = string.Format(ApiNypLabReqsReportsParam, batchId, jsonFile, reportSrch, totalJsonFile, dateUpLoaded);
                    var responseTask = client.GetAsync($"{controllerName}{apiParms}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {
                          //  var s = results.Content.ReadAsStringAsync();
                          //s.Wait();
                         var readTask = results.Content.ReadAsAsync<LabReqsUploaded[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            List<string> totalResults = readTask.Result.Select(p => p.Uploaded).ToList(); 
                            if(totalResults.Count ==1)
                            totalNumupLoaded = int.Parse(totalResults[0]);
                        }
                        //  else
                        //     edl.TraceLogger.TraceLoggerInstance.TraceInformation($"No  lab recs found for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate}");


                    }
                    else
                    {
                    //    edl.TraceLogger.TraceLoggerInstance.TraceError($"Getting lab recsd for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate} IsSuccessStatusCode {results.StatusCode.ToString()}");
                        throw new Exception($"Getting lab recsd for webAPiUri {webAPiUri} controller {controllerName} scandate  IsSuccessStatusCode {results.StatusCode.ToString()}");

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Getting azure recs for webapi:{webAPiUri} controller:{controllerName} scandate:  tablename:  {ex.Message}");

            }
            return totalNumupLoaded;
        }

       
    }
}
