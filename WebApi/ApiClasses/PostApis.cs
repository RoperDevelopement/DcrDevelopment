using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BinMonitorAppService.Models;
using Newtonsoft;
using Newtonsoft.Json;
using BinMonitorAppService.Constants;
//using BinMonitorAppService.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace BinMonitorAppService.ApiClasses
{
    public class PostApis:PageModel
    {
        private static PostApis instance = null;
        public static PostApis PostApisIntance
        {
            get
            {
                if (instance == null)
                    instance = new PostApis();
                return instance;
            }
        }
        private PostApis()
        {
        }
        public async Task ApiUpdateEmailInfo(string apiUrl, string sqlStoredProcedure,EmailReportModel emailReportModel)
        {
           

            using (var client = new HttpClient())
            {
               
                client.BaseAddress = new Uri($"{apiUrl}");
                var jsonString = JsonConvert.SerializeObject(emailReportModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{sqlStoredProcedure}", content);
                responseTask.Wait();
                var result = await client.PostAsync("Method Address", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    // var readTask = results.Content.ReadAsAsync<BinModel[]>();
                    // readTask.Wait();
                    // List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                    // tempList.Add("  ");
                    // tempList.Sort();
                    // BinID = tempList;

                    //BinID = readTask.Result.Select(p => p.BinID).ToList();
                    //BinID.Add("000");
                    //BinID = BinID.OrderBy(p => p).First();



                }
            }
        }

       

        public async Task ApiUpdateTransFromEmailInfo(string apiUrl, string oldBatchID,string newBatchID)
        {
            using (var client = new HttpClient())
            {
                OldNewBatchIdModel oldNewBatchIdModel = new OldNewBatchIdModel();
                oldNewBatchIdModel.NewBatchId = newBatchID;
                oldNewBatchIdModel.OldBatchId = oldBatchID;
                client.BaseAddress = new Uri($"{apiUrl}");
                  var jsonString = JsonConvert.SerializeObject(oldNewBatchIdModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}WorkFlowEmail/", content);
                responseTask.Wait();
                var result = await client.PostAsync("Method Address", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    // var readTask = results.Content.ReadAsAsync<BinModel[]>();
                    // readTask.Wait();
                    // List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                    // tempList.Add("  ");
                    // tempList.Sort();
                    // BinID = tempList;

                    //BinID = readTask.Result.Select(p => p.BinID).ToList();
                    //BinID.Add("000");
                    //BinID = BinID.OrderBy(p => p).First();



                }
            }
        }

        public async Task ApiUpdateCategoryColors(string apiUrl, string sqlStoredProcedure, CategoryColorModel categoryColor)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{apiUrl}");
                var jsonString = JsonConvert.SerializeObject(categoryColor);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{sqlStoredProcedure}", content);
                responseTask.Wait();
                var result = await client.PostAsync("Method Address", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    // var readTask = results.Content.ReadAsAsync<BinModel[]>();
                    // readTask.Wait();
                    // List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                    // tempList.Add("  ");
                    // tempList.Sort();
                    // BinID = tempList;

                    //BinID = readTask.Result.Select(p => p.BinID).ToList();
                    //BinID.Add("000");
                    //BinID = BinID.OrderBy(p => p).First();



                }
            }
        }

        public async Task ApiUpdateCategoryDurations(string apiUrl, string sqlStoredProcedure, CategoryCheckPointModel checkPointModel)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{apiUrl}");
                var jsonString = JsonConvert.SerializeObject(checkPointModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{sqlStoredProcedure}", content);
                responseTask.Wait();
                var result = await client.PostAsync("Method Address", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    // var readTask = results.Content.ReadAsAsync<BinModel[]>();
                    // readTask.Wait();
                    // List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                    // tempList.Add("  ");
                    // tempList.Sort();
                    // BinID = tempList;

                    //BinID = readTask.Result.Select(p => p.BinID).ToList();
                    //BinID.Add("000");
                    //BinID = BinID.OrderBy(p => p).First();



                }
            }
        }

        public async Task ApiEmailWorkFlow(string apiUrl, string sqlStoredProcedure, WorkFlowEmailModel workFlowEmailModel )
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{apiUrl}");
                var jsonString = JsonConvert.SerializeObject(workFlowEmailModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{sqlStoredProcedure}", content);
                responseTask.Wait();
                var result = await client.PutAsync("Method Address", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    // var readTask = results.Content.ReadAsAsync<BinModel[]>();
                    // readTask.Wait();
                    // List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                    // tempList.Add("  ");
                    // tempList.Sort();
                    // BinID = tempList;

                    //BinID = readTask.Result.Select(p => p.BinID).ToList();
                    //BinID.Add("000");
                    //BinID = BinID.OrderBy(p => p).First();



                }
            }
        }
    }
}
