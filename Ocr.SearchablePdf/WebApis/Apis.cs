using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Edocs.Ocr.SearchablePdf.Models;
using Newtonsoft.Json;

namespace Edocs.Ocr.SearchablePdf.WebApis
{
    public class Apis
    {
        public static async Task UpDateRecordsOCR(Uri webUri, string controllerName, int id, int totalDocsOcr,int totalpages)
        {



            using (var client = new HttpClient())
            {
                client.BaseAddress = webUri;
                var responseTask = client.GetAsync($"{controllerName}/{id}/{totalDocsOcr}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                      //  JObject jsonObj = JObject.Parse(jsonString);
                      //  Dictionary<string, string> dictObj = jsonObj.ToObject<Dictionary<string, object>>();

                        if (readTask.Result.ToLower().StartsWith("error"))
                            throw new Exception(readTask.Result);

                        // retModel = JsonConvert.DeserializeObject<Dictionary<int, object>>(s);
                    }


                }
                else
                {
                    throw new Exception($"Getting MDT records to OCR WebURi {webUri} controller {controllerName} IsSuccessStatusCode {results.StatusCode.ToString()}");
                }
            }
        }

        public static async Task UploadEdocsITS(string webUri, string controllerName, UpLoadMDTrackingModel upLoadMD)
        {


            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(webUri);

                var jsonString = JsonConvert.SerializeObject(upLoadMD);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}", content);
                
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        //  JObject jsonObj = JObject.Parse(jsonString);
                        //  Dictionary<string, string> dictObj = jsonObj.ToObject<Dictionary<string, object>>();

                        if (readTask.Result.ToLower().StartsWith("error"))
                            throw new Exception(readTask.Result);

                        // retModel = JsonConvert.DeserializeObject<Dictionary<int, object>>(s);
                    }


                }
                else
                {
                    throw new Exception($"Getting MDT records to OCR WebURi {webUri} controller {controllerName} IsSuccessStatusCode {results.StatusCode.ToString()}");
                }
            }
        }

    }
}
