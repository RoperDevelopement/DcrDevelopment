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
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using  EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
   

    public class SpecimenRejectionApi
    {
        private static SpecimenRejectionApi instance = null;
        public static SpecimenRejectionApi SpecimenRejectionIntance
        {
            get
            {
                if (instance == null)
                    instance = new SpecimenRejectionApi();
                return instance;
            }
        }
        private SpecimenRejectionApi()
        {
        }

        public async Task<IList<string>> RejectionLogsReason(string webUrl, string controller,ILog log)
        {
            IList<string> retReasons = new List<string>();

            try
            {
                log.LogInformation($"Method RejectionLogsReason weburl: {webUrl} controller: {controller} ");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method RejectionLogsReason weburl: {webUrl} controller: {controller} results status code: {results.StatusCode} ");
                    if (results.IsSuccessStatusCode)
                    {
                       // var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<RejectionLogs[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retReasons = readTask.Result.Select(p => p.ReasonRej).ToList();
                            if(!(retReasons.Any()))
                                log.LogWarning($"Method RejectionLogsReason weburl: {webUrl} controller: {controller} no results for RejectionLogs ");
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
                log.LogError($"Method RejectionLogsReason weburl: {webUrl} controller: {controller} {ex.Message} ");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retReasons;
        }
        //}  public async Task<IList<string>> RejectionLogsReason(string webUrl, string controller)
        //{
        //    IList<string> retReasons = new List<string>();

        //    try
        //    {


        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri($"{webUrl}");
        //            var responseTask = client.GetAsync($"{controller}");
        //            responseTask.Wait();
        //            var results = responseTask.Result;

        //            if (results.IsSuccessStatusCode)
        //            {
        //                var s = results.Content.ReadAsStringAsync();
        //                s.Wait();
        //                var readTask = results.Content.ReadAsAsync<RejectionLogs[]>();
        //                readTask.Wait();
        //                if (readTask.Result.Length > 0)
        //                {
        //                   // foreach(var str in readTask.Result)
        //                   // {
        //                      //  retReasons.Add(str.ToString());
        //                   // }
        //                    retReasons = readTask.Result.Select(p => p.ReasonRej).ToList();
        //                }
        //                else
        //                {

        //                    throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //        throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
        //    }


        //    return retReasons;
        //}


     


        public async Task<IDictionary<string,SpecimenRejectionModel>> RejectionLogs(string webUrl, string controller,SpecimenRejectionModel rejectionModel,ILog log)
        {
            IDictionary<string, SpecimenRejectionModel> retReasons = new Dictionary<string, SpecimenRejectionModel>();

            try
            {
                log.LogInformation($"Method RejectionLogs weburl:{webUrl} controller:{controller} ");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(rejectionModel);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    content.Dispose();
                    var results = responseTask.Result;
                    log.LogInformation($"Method RejectionLogs weburl:{webUrl} controller:{controller} results status code: {results.StatusCode} ");
                    if (results.IsSuccessStatusCode)
                    {
                     //   var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<SpecimenRejectionModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retReasons = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retReasons.Any()))
                                log.LogWarning($"Method RejectionLogs weburl:{webUrl} controller:{controller} no results for SpecimenRejectionModel ");
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
                log.LogError($"Method RejectionLogs weburl:{webUrl} controller:{controller} {ex.Message} ");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retReasons;
        }

        public async Task<IDictionary<string, DrCodeModel>> GetDrCodes(string webUrl, string controller, DrCodeModel drCode,ILog log)
        {
            IDictionary<string, DrCodeModel> retDrCodes = new Dictionary<string, DrCodeModel>();
            
            try
            {
                log.LogInformation($"Method GetDrCodes webUrl:{webUrl.ToString()} controller:{controller}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(drCode);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    content.Dispose(); content.Dispose();
                    log.LogInformation($"Method GetDrCodes webUrl:{webUrl.ToString()} controller:{controller} result code:{results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<DrCodeModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retDrCodes = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retDrCodes.Any()))
                                log.LogWarning($"Method GetDrCodes webUrl:{webUrl.ToString()} controller:{controller} returned no results");
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
                log.LogWarning($"Method GetDrCodes webUrl:{webUrl.ToString()} controller:{controller} {ex.Message}");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retDrCodes;
        }

        public async Task<IDictionary<string, GrantReceiptsModel>> GranteReceipts(string webUrl, string controller, GrantReceiptsModel receiptsModel,ILog log)
        {
            IDictionary<string, GrantReceiptsModel> retGranteRecp = new Dictionary<string, GrantReceiptsModel>();
            log.LogInformation($"Method GranteReceipts for webUrl: {webUrl} controller:{controller}");
            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(receiptsModel);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Method GranteReceipts for webUrl: {webUrl} controller:{controller} restult code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                       // var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<GrantReceiptsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retGranteRecp = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retGranteRecp.Any()))
                                log.LogWarning($"Method GranteReceipts for webUrl: {webUrl} controller:{controller} restult code: {results.StatusCode} not results for GrantReceiptsModel");
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
                log.LogError($"Method GranteReceipts for webUrl: {webUrl} controller:{controller} {ex.Message}");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retGranteRecp;
        }

        public async Task<IList<string>> DOHPerformingLabCodes(Uri webUrl, string controller,ILog log)
        {
            IList<string> retPerfCodes = new List<string>();
            log.LogInformation($"Method DOHPerformingLabCodes for weburl:{webUrl.ToString()} for controller {controller}");
            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method DOHPerformingLabCodes for weburl:{webUrl.ToString()} for controller {controller} results code:{results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                      //  var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypPerformingLabCodeDOH[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retPerfCodes = readTask.Result.Select(p => p.PerformingLabCodes).ToList();
                            if(!(retPerfCodes.Any()))
                                log.LogWarning($"Method DOHPerformingLabCodes for weburl:{webUrl.ToString()} for controller {controller} no lab codes returned");
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
                log.LogError($"Method DOHPerformingLabCodes url {webUrl} for sp:{controller} {ex.Message}");
                throw new Exception($"Url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retPerfCodes;
        }

        public async Task<IList<string>> NypManLogStations(Uri webUrl, string controller,ILog log)
        {
            IList<string> retLogStations = new List<string>();

            try
            {
                log.LogInformation($"Method NypManLogStations for weburl: {webUrl.ToString()} controller: {controller}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypManLogStations for weburl: {webUrl.ToString()} controller: {controller} results status code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                       // var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<MaintenanceLogsLogStation[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retLogStations = readTask.Result.Select(p => p.LogStation).ToList();
                            if(!(retLogStations.Any()))
                                log.LogWarning($"Method NypManLogStations for weburl: {webUrl.ToString()} controller: {controller} no resutls returned for list MaintenanceLogsLogStation");
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
                log.LogError($"Method NypManLogStations for weburl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retLogStations;
        }

        public async Task<IDictionary<int, DohModel>> DOHLabReqs(Uri webUrl, string controller, DohModel doh, ILog log)
        {
            IDictionary<int, DohModel> retDOH = new Dictionary<int, DohModel>();

            try
            {
                log.LogInformation($"Method DOHLabReqs for weburi:{webUrl.ToString()} for controller {controller}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(doh);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    content.Dispose();
                    var results = responseTask.Result;
                    log.LogInformation($"Method DOHLabReqs for weburi:{webUrl.ToString()} for controller {controller} status code returned {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<DohModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {

                            retDOH = readTask.Result.ToDictionary(p => p.DOHID);
                            if (!(retDOH.Any()))
                                log.LogInformation($"Method DOHLabReqs for weburi:{webUrl.ToString()} for controller {controller} no results returned");
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
                log.LogError($"Method DOHLabReqs for weburi:{webUrl.ToString()} for controller {controller} status code returned {ex.Message}");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retDOH;
        }
        public async Task<IDictionary<string, MaintenanceLogsModel>> NypMaintenanceLogs(Uri webUrl, string controller, MaintenanceLogsModel logsModel,ILog log)
        {
            IDictionary<string, MaintenanceLogsModel> retML = new Dictionary<string, MaintenanceLogsModel>();

            try
            {

                log.LogInformation($"Method NypMaintenanceLogs weburl: {webUrl.ToString()} controller: {controller} ");
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
                    log.LogInformation($"Method NypMaintenanceLogs weburl: {webUrl.ToString()} controller: {controller} result status code:{results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var s = results.Content.ReadAsStringAsync();
                        s.Wait();
                        var readTask = results.Content.ReadAsAsync<MaintenanceLogsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            retML = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retML.Any()))
                                log.LogWarning($"Method NypMaintenanceLogs weburl: {webUrl.ToString()} controller: {controller} no results for MaintenanceLogsModel");
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
                log.LogError($"Method NypMaintenanceLogs weburl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retML;
        }

        public async Task<IDictionary<string, SendOutPackingSlipsModel>> NypSendOutPackingSlips(Uri webUrl, string controller, SendOutPackingSlipsModel packingSlipsModel,ILog log)
        {
            IDictionary<string, SendOutPackingSlipsModel> retPS = new Dictionary<string, SendOutPackingSlipsModel>();

            try
            {
                log.LogInformation($"Method NypSendOutPackingSlips weburl: {webUrl.ToString()} controller: {controller}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(packingSlipsModel);

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
                        var readTask = results.Content.ReadAsAsync<SendOutPackingSlipsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            retPS = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retPS.Any()))
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


            return retPS;
        }

        public async Task<IDictionary<string,SendOutResultsModel>> NypSendOutResults(Uri webUrl, string controller, SendOutResultsModel sendOutResults,ILog log)
        {
            IDictionary<string, SendOutResultsModel> retRM = new Dictionary<string, SendOutResultsModel>();

            try
            {
                log.LogInformation($"Method NypSendOutResults weburl:{webUrl.ToString()} controller: {controller} ");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(sendOutResults);

                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
                    responseTask.Wait();
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Method NypSendOutResults weburl:{webUrl.ToString()} controller: {controller} results status code {results.StatusCode} ");
                    if (results.IsSuccessStatusCode)
                    {
                       // var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<SendOutResultsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            retRM = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retRM.Any()))
                                log.LogWarning($"Method NypSendOutResults weburl:{webUrl.ToString()} controller: {controller} not results for SendOutResultsModel");

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
                log.LogWarning($"Method NypSendOutResults weburl:{webUrl.ToString()} controller: {controller} {ex.Message}");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retRM;
        }
    }
}
