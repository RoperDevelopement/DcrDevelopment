using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using EDocs.Nyp.LabReqs.AppServices.Models;
         
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;


namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
    public class NypEmployeeComplianceApi
    {
        private static NypEmployeeComplianceApi instance = null;
        public static NypEmployeeComplianceApi EmployeeCompliamceIntance
        {
            get
            {
                if (instance == null)
                    instance = new NypEmployeeComplianceApi();
                return instance;
            }
        }
        private NypEmployeeComplianceApi()
        {
        }

        public async Task<IList<string>> NypEmployeeComplianceCodes(Uri webUrl, string controller,ILog log)
        {
            IList<string> retEmpCodes = new List<string>();
            

            try
            {
                log.LogInformation($"Method NypEmployeeComplianceCodes weburl: {webUrl.ToString()} controller: {controller}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypEmployeeComplianceCodes weburl: {webUrl.ToString()} controller: {controller} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                       // var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypEmployeeComplianceLogs[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retEmpCodes = readTask.Result.Select(p => p.NypEmployeeCompliance).ToList();
                            if(!(retEmpCodes.Any()))
                                log.LogWarning($"Method NypEmployeeComplianceCodes weburl: {webUrl.ToString()} controller: {controller} no results for list NypEmployeeComplianceLogs");
                        }
                        else
                        {
                            if (results.StatusCode == System.Net.HttpStatusCode.OK)
                                return null;

                            throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method NypEmployeeComplianceCodes weburl: {webUrl.ToString()} controller: {controller} {ex.Message}");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retEmpCodes;
        }

        public async Task<IDictionary<string, EmployeeComplianceLogsModel>> NypComplianceLogs(Uri webUrl, string controller, EmployeeComplianceLogsModel employeeCompliance,ILog log)
        {
            IDictionary<string, EmployeeComplianceLogsModel> retECL = new Dictionary<string, EmployeeComplianceLogsModel>();
            log.LogInformation($"Method NypComplianceLogs for weburl: {webUrl.ToString()} controller: {controller}");
            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(employeeCompliance);

                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypComplianceLogs for weburl: {webUrl.ToString()} controller: {controller} results code: {results.StatusCode}");
                    content.Dispose();
                    if (results.IsSuccessStatusCode)
                    {
                     //   var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<EmployeeComplianceLogsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            retECL = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retECL.Any()))
                                log.LogWarning($"Method NypComplianceLogs for weburl: {webUrl.ToString()} controller: {controller} results code: {results.StatusCode} not result for EmployeeComplianceLogsModel");
                        }

                    }
                    else
                    {

                        throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method NypComplianceLogs for weburl: {webUrl.ToString()} controller: {controller} {ex.Message}");
                
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retECL;
        }
    }
}
