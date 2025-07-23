using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

using System.Threading.Tasks;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Mobile_LabReqs.ViewsModels;
using System.Collections.ObjectModel;

namespace Mobile_LabReqs.Services
{
    public class RestApi
    {
        public static async Task<ObservableCollection<LabReqsModel>> GetLabReqs(string webUrl, string sp,LabReqsModel labReqsModel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri($"{webUrl}");
                var jsonString = JsonConvert.SerializeObject(labReqsModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                  var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}/{sp}", content);
              //  var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}");
                responseTask.Wait();
                content.Dispose();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsStringAsync();
                    readTask.Wait();
                    return JsonConvert.DeserializeObject<ObservableCollection<LabReqsModel>>(readTask.Result);

                }

                return null;
            }

        }
        public static async Task<ObservableCollection<LabReqsModel>> GetLabReqsByKeyWords(string webUrl,string controller,DateTime stDate,DateTime endDate,string searchBy,string keyWords)
        {

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri($"{webUrl}");
                //var jsonString = JsonConvert.SerializeObject(labReqsModel);
                //var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string searchStDate = stDate.ToString().Replace("/", "-");
                string searchEndDate = endDate.ToString().Replace("/", "-");
                string argStr = $"{controller}{searchStDate}/{ searchEndDate}/{searchBy}/{keyWords}";
                var responseTask = client.GetAsync($"{argStr}" );
              //   var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}");
                responseTask.Wait();
               // content.Dispose();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsStringAsync();
                    readTask.Wait();
                    return JsonConvert.DeserializeObject<ObservableCollection<LabReqsModel>>(readTask.Result);

                }

                return null;
            }

        }
        public static async Task Test(string webUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(129);
                    var responseTask = client.GetAsync(webUrl);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        string s = readTask.Result;

                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
