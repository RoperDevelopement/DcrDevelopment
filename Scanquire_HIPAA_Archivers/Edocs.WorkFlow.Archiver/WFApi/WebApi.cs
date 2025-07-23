 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Edocs.WorkFlow.Archiver.Models;
using System.Net.Http.Formatting;
namespace Edocs.WorkFlow.Archiver.WFApi
{
    class WebApi
    {
        public static async Task<IDictionary<int, WFUsersModel>> GetWFUsers(string webAPiUri, string controllerName)
        {
            IDictionary<int, WFUsersModel> wfUsers = new Dictionary<int, WFUsersModel>();
            try
            {
                  using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(webAPiUri);
                    client.Timeout = TimeSpan.FromSeconds(30); ;
                    var responseTask = client.GetAsync($"{controllerName}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        //    var s = results.Content.ReadAsStringAsync();
                        //  s.Wait();
                        var readTask1 = results.Content.ReadAsStringAsync();
                        var readTask = results.Content.ReadAsAsync<WFUsersModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                            wfUsers = readTask.Result.ToDictionary(p =>p.ID);
                        


                    }
                    else
                    {
                       
                        throw new Exception($"Getting lab recsd for webAPiUri {webAPiUri} controller {controllerName}   {results.StatusCode.ToString()}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return wfUsers;
        }
    }
}