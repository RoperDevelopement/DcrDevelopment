using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using edl = EdocsUSA.Utilities.Logging;
namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    class WebApi
    {
        public static string EdocsWebApi
        { get { return Properties.Settings.Default.EdocsWebApi; } }
        public static bool UpLoadAzureCloud
        { get { return Properties.Settings.Default.UpLoadAzureCloud; } }
        public static async Task<List<string>> GetLabReqs(string webAPiUri, string controllerName, string recDate, string tableName)
        {
            List<string> labRec = new List<string>();
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(webAPiUri);
                    recDate = recDate.Replace("/", "-");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab recs for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate} tableName {tableName}");
                    var responseTask = client.GetAsync($"{controllerName}/{recDate}/{tableName}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {
                        //    var s = results.Content.ReadAsStringAsync();
                        //  s.Wait();
                        var readTask = results.Content.ReadAsAsync<JsonFileLabRecsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                            labRec = readTask.Result.Select(p => p.FileName).ToList();
                        else
                            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"No  lab recs found for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate}");


                    }
                    else
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceError($"Getting lab recsd for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate} IsSuccessStatusCode {results.StatusCode.ToString()}");
                        throw new Exception($"Getting lab recsd for webAPiUri {webAPiUri} controller {controllerName} scandate {recDate} IsSuccessStatusCode {results.StatusCode.ToString()}");

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Getting azure recs for webapi:{webAPiUri} controller:{controllerName} scandate:{recDate} tablename:{tableName} {ex.Message}");

            }
            return labRec;
        }

        public static async Task UplaodLabReqs(string webAPiUri, JsonFileLabRecsModel jsonString, string controllerName)
        {
           
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading labrecs json record to webAPiUri {webAPiUri} controller {controllerName}");
                using (var client = new HttpClient())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var ser = serializer.Serialize(jsonString);
                    client.BaseAddress = new Uri(webAPiUri);
                    var content = new StringContent(ser, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Successfully uplod json record resultContent");
                    }
                    else
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceError($"Uplaoding json file {results.StatusCode.ToString()}");
                        throw new Exception($"Uplaoding json file {results.StatusCode.ToString()}");
                        edl.TraceLogger.TraceLoggerInstance.TraceError($"Uploading azure labrecs to webapi:{webAPiUri} controller:{controllerName}");
                    }

                }
            
        }

        public static async Task UploadAzureBatches(string webAPiUri, AzureCloudBatchRecordsModel jsonString, string controllerName, string spName, string tableName)
        {
            
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading azure batch json record to webAPiUri {webAPiUri} controller {controllerName} spName:{spName} tablename:{tableName}");
                using (var client = new HttpClient())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var ser = serializer.Serialize(jsonString);
                    client.BaseAddress = new Uri(webAPiUri);
                    var content = new StringContent(ser, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}/{spName}/{tableName}", content);
                    responseTask.Wait();
                    //  var result = await client.PutAsync("Method Address", content);
                    // string resultContent = await result.Content.ReadAsStringAsync();
                    var results = responseTask.Result;
                    var resultsContent = results.Content.ReadAsStringAsync();
                    resultsContent.Wait();
                    if (results.IsSuccessStatusCode)
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Successfully uploaded json record resultContent");
                    }
                    else
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceError($"Uplaoding json file {results.StatusCode.ToString()}");
                        edl.TraceLogger.TraceLoggerInstance.TraceError($"Uploading azure batches to webapi:{webAPiUri} controller:{controllerName} spname:{spName} tableName:{tableName}");
                        throw new Exception($"Uplaoding json file {results.StatusCode.ToString()}");
                    }

                }
        }
    }
}
