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
using BinMonitorAppService.Logging;
namespace BinMonitorAppService.ApiClasses
{
    public class PostApis : PageModel
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
        public async Task ApiUpdateEmailInfo(string apiUrl, string sqlStoredProcedure, EmailReportModel emailReportModel, ILog log)
        {
            try
            {

                log.LogInformation($"Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure}");

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
                    content.Dispose();
                    log.LogInformation($"Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");



                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateEmailInfo weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
            }
        }


        public async Task ApiDelLabReq(string apiUrl, string controller,string labRecID,string delBy, ILog log)
        {
            try
            {

                log.LogInformation($"Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy}");

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri($"{apiUrl}");
                    
                    var responseTask = client.DeleteAsync($"{client.BaseAddress.AbsoluteUri}{controller}{labRecID}/{delBy}");
                    responseTask.Wait();
                   
                     
                    log.LogInformation($"Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} results code: {responseTask.Result.StatusCode}");
                    if (responseTask.Result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy}  results code: {responseTask.Result.StatusCode}");


                    }
                    else
                    {
                        var results = responseTask.Result;
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        if (!(readTask.Result.ToLower().Contains("labreq deleted")))
                            throw new Exception($"LabReqID {labRecID} not deleted");
                        log.LogInformation($"Method ApiDelLabReq labreqid {labRecID} deleted by {delBy}");
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} {httpex.Message}");
                throw new Exception($"HttpRequestException MMethod ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} {ag.Message}");
                throw new Exception($"ArgumentNullExceptionMethod ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} {ex.Message}");
                throw new Exception($"Exception Method ApiDelLabReq weburl: {apiUrl} controller: {controller} labrreqid {labRecID} deleted by {delBy} {ex.Message}");
            }
        }
        public async Task ApiUpdateTransFromEmailInfo(string apiUrl, string oldBatchID, string newBatchID, ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} controller: WorkFlowEmail/ old batchid: {oldBatchID} new batchid: {newBatchID}");

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
                    content.Dispose();
                    log.LogInformation($"Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ resutls stats code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ resutls stats code: {results.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/  {ex.Message}");
                throw new Exception($"Method ApiUpdateTransFromEmailInfo weburl: {apiUrl} transfer old batchid:{oldBatchID} to new batchid:{newBatchID} controller: WorkFlowEmail/ {ex.Message}");
            }
        }

        public async Task ApiUpdateCategoryColors(string apiUrl, string sqlStoredProcedure, CategoryColorModel categoryColor, ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure}");


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
                    content.Dispose();
                    log.LogInformation($"Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");

                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateCategoryColors weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
            }
        }

        public async Task ApiUpdateCategoryCheckPoints(string apiUrl, string sqlStoredProcedure, CategoryCheckPointModel checkPointModel, ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}");

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
                    content.Dispose();
                    log.LogInformation($"Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");



                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}  {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}  {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}  {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}  {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}  {ex.Message}");
            }
        }

        public async Task ApiUpdateCategoryDurations(string apiUrl, string sqlStoredProcedure, CategoryCheckPointEmailModel checkPointModel, ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure}");

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
                    content.Dispose();
                    log.LogInformation($"Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                {
                        throw new Exception($"Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateCategoryCheckPoints weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
            }
        }

        public async Task ApiEmailWorkFlow(string apiUrl, string sqlStoredProcedure, WorkFlowEmailModel workFlowEmailModel, ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure}");

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
                    content.Dispose();
                    log.LogInformation($"Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");

                    if (!(results.IsSuccessStatusCode))
                    {
                       throw new Exception($"Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} results code: {results.StatusCode}");



                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
                throw new Exception($"Exception Method ApiEmailWorkFlow weburl: {apiUrl} controller: {sqlStoredProcedure} {ex.Message}");
            }

        }
    }
}
