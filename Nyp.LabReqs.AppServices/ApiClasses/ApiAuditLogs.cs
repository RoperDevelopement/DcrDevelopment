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
using System.ComponentModel.DataAnnotations;

namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
    public class ALogAppName
    {
        [Display(Name = "Application Name:")]
        public string AuditLogApplicationName
        { get; set; }
    }
    public class ALogCwid
    {
        [Display(Name = "Cwod:")]

        public string Cwid
        { get; set; }
    }
    public class ALogMessType
    {
        [Display(Name = "Audit Log Message:")]
        public string AuditLogMessageType
        { get; set; }
    }
    public class ApiAuditLogs
    {
        private static ApiAuditLogs instance = null;
        public static ApiAuditLogs NypApiAuditLogsIntance
        {
            get
            {
                if (instance == null)
                    instance = new ApiAuditLogs();
                return instance;
            }
        }
        private ApiAuditLogs()
        {
        }

        //public async Task<IList<AuditLogsComboBoxModel>> GetAuditLogsCB(string storedProcedure, Uri webUri, string controller)
        //{
        //    IList<AuditLogsComboBoxModel> auditLogsComboBoxModels = new List<AuditLogsComboBoxModel>();


        //    try
        //    {


        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = webUri;
        //            var responseTask = client.GetAsync($"{controller}{storedProcedure}");
        //            responseTask.Wait();
        //            var results = responseTask.Result;

        //            if (results.IsSuccessStatusCode)
        //            {
        //                var readTask = results.Content.ReadAsAsync<AuditLogsComboBoxModel[]>();
        //                readTask.Wait();


        //                auditLogsComboBoxModels = readTask.Result.ToList();

        //            }
        //            else
        //            {

        //                throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUri.ToString()} for sp:{controller}");
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //        throw new Exception($"For url {webUri} for sp:{controller} {ex.Message}");
        //    }
        //    return auditLogsComboBoxModels;
        //}


        public async Task<IDictionary<int,AuditLogsModel>> GetNypAuditLogs(Uri webUrl, string controller, AuditLogsModel logsModel, ILog log)
        {
            IDictionary<int, AuditLogsModel> retALM = new Dictionary<int, AuditLogsModel>();

            try
            {
                log.LogInformation($"Method NypSendOutPackingSlips weburl: {webUrl.ToString()} controller: {controller}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(logsModel);

                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
                    responseTask.Wait();
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Method NypSendOutPackingSlips weburl: {webUrl.ToString()} controller: {controller} results status code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        // s.Wait();
                        var readTask = results.Content.ReadAsAsync<AuditLogsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            retALM = readTask.Result.ToDictionary(p => p.AuditLogID);
                            if (!(retALM.Any()))
                                log.LogWarning($"Method NypSendOutPackingSlips weburl: {webUrl.ToString()} controller: {controller} no result for SendOutPackingSlipsModel");
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
                log.LogError($"Method NypSendOutPackingSlips weburl: {webUrl.ToString()} controller: {controller} {ex.Message}");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retALM;
        }

        public async Task<IList<T>> GetAuditLogsCwid<T>(string storedProcedure, Uri webUri, string controller)

        {
            IList<T> auditLogsComboBoxModels = new List<T>();


            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUri.ToString()}{controller}");
                    var responseTask = client.GetAsync($"{storedProcedure}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<T[]>();
                        readTask.Wait();


                        auditLogsComboBoxModels = readTask.Result.ToList();

                    }
                    else
                    {

                        throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUri.ToString()} for sp:{controller}");
                    }
                }
            }

            catch (Exception ex)
            {

                throw new Exception($"For url {webUri} for sp:{controller} {ex.Message}");
            }
            return auditLogsComboBoxModels;
        }


    }
}
