using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDT.OCT.Projects.Models;
using System.Net.Http;
using Newtonsoft.Json;
namespace MDT.OCT.Projects.WebApis
{
    internal class MDTWebApis
    {
        public static async Task<IList<MDTOCRModel>> GetRecordsToOCT(Uri webUri,string controllerName)
        {
            IList<MDTOCRModel> lResults = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = webUri;
                var responseTask = client.GetAsync($"{controllerName}");
                responseTask.Wait();
                var results = responseTask.Result;
                if(results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                         
                       lResults = JsonConvert.DeserializeObject<List<MDTOCRModel>>(readTask.Result);

                        // retModel = JsonConvert.DeserializeObject<Dictionary<int, object>>(s);
                    }
                    

                }
                else
                {
                    throw new Exception($"Getting MDT records to OCR WebURi {webUri} controller {controllerName} IsSuccessStatusCode {results.StatusCode.ToString()}");
                }
            }
            return lResults;
        }

        public static async Task UpDateRecordsOCR(Uri webUri, string controllerName,int id,int totalDocsOcr)
        {
            IList<MDTOCRModel> lResults = null;


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

                       if(readTask.Result.ToLower().StartsWith("error"))
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
