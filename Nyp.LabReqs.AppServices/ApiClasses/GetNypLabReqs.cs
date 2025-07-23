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

namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
    public class GetNypLabReqs
    {

        private static GetNypLabReqs instance = null;
        public static GetNypLabReqs NypLabReqsApisInctance
        {
            get
            {
                if (instance == null)
                    instance = new GetNypLabReqs();
                return instance;
            }
        }
        private GetNypLabReqs()
        {
        }
        public DateTime GetLabRecDate(string labRecDate)
        {

            if (DateTime.TryParse(labRecDate, out DateTime result))
            {
                return result;
            }
            return DateTime.Now;
        }

        public async Task<IDictionary<int, LabReqsModel>> GetLabReqsByKeyWords(string webUrl, string controller, DateTime stDate, DateTime endDate, string searchBy, string keyWords, ILog log)
        {
            IDictionary<int, LabReqsModel> retDicLabReqsModel = new Dictionary<int, LabReqsModel>();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri($"{webUrl}");
                string searchStDate = stDate.ToString().Replace("/", "-");
                string searchEndDate = endDate.ToString().Replace("/", "-");
                string argStr = $"{controller}{searchStDate}/{searchEndDate}/{searchBy}/{keyWords}";
                var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}{argStr}");
                //   var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}");
                responseTask.Wait();
                // content.Dispose();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<LabReqsModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {

                        retDicLabReqsModel = readTask.Result.ToDictionary(p => p.LabReqID);
                        if (!(retDicLabReqsModel.Any()))
                            log.LogWarning($"Method NypLabReqs weburl: {webUrl} controller: {controller} no results for LabReqsModel");
                        else
                            return retDicLabReqsModel;
                    }

                }

                return null;
            }

        }

        public async Task<IDictionary<int, LabReqsModel>> NypLabReqs(string webUrl, string storedProcedure, LabReqsModel labReqsModel, ILog log)
        {
            IDictionary<int, LabReqsModel> retDicLabReqsModel = new Dictionary<int, LabReqsModel>();

            try
            {
                log.LogInformation($"Method NypLabReqs weburl: {webUrl} storedProcedure: {storedProcedure} ");

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(labReqsModel);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{storedProcedure}", content);

                    responseTask.Wait();
                    content.Dispose();

                    var results = responseTask.Result;
                    log.LogInformation($"Method NypLabReqs weburl: {webUrl} storedProcedure: {storedProcedure}  results status code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        // var d = results.Content.ReadAsStringAsync();
                        // d.Wait();
                        var readTask = results.Content.ReadAsAsync<LabReqsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {

                            retDicLabReqsModel = readTask.Result.ToDictionary(p => p.LabReqID);

                            if (!(retDicLabReqsModel.Any()))
                                log.LogWarning($"Method NypLabReqs weburl: {webUrl} storedProcedure: {storedProcedure} no results for LabReqsModel");
                        }
                    }
                    else
                    {

                        throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{storedProcedure}");
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method NypLabReqs weburl: {webUrl} storedProcedure: {storedProcedure} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{storedProcedure} {ex.Message}");
            }


            return retDicLabReqsModel;
        }


        public async Task<LabReqsEditModel> GetLabReqToEdit(string webUrl, string controller, int labReqID, ILog log)
        {
            LabReqsEditModel retDicLabReqsModel = new LabReqsEditModel();

            try
            {
                log.LogInformation($"Method NypLabReqs weburl: {webUrl} controller: {controller} for id {labReqID}");

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.BaseAddress = new Uri($"{webUrl}");



                    var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}{controller}{labReqID}");

                    responseTask.Wait();


                    var results = responseTask.Result;
                    log.LogInformation($"Method NypLabReqs weburl: {webUrl} controller: {controller} for id {labReqID}  results status code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        // var d = results.Content.ReadAsStringAsync();
                        // d.Wait();
                        var readTask = results.Content.ReadAsAsync<LabReqsEditModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {

                            retDicLabReqsModel = readTask.Result[0];
                            //if (!(retDicLabReqsModel))
                            //    log.LogWarning($"Method NypLabReqs weburl: {webUrl}  controller: {controller} for id {labReqID} no results for LabReqsModel");
                        }
                    }
                    else
                    {

                        throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for controller: {controller} for id {labReqID}");
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method NypLabReqs weburl: {webUrl}  controller: {controller} for id {labReqID} {ex.Message}");

                throw new Exception($"For url {webUrl}  controller: {controller} for id {labReqID} {ex.Message}");
            }


            return retDicLabReqsModel;
        }

        public async Task UpDateLabReqEdit(string webUrl, string controller, LabReqsEditModel labReqsEdit, ILog log)
        {


            try
            {
                log.LogInformation($"Method UpDateLabReqEdit weburl: {webUrl} controller: {controller} for id {labReqsEdit.LabReqID}");

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.BaseAddress = new Uri($"{webUrl}");

                    var jsonString = JsonConvert.SerializeObject(labReqsEdit);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);

                    responseTask.Wait();
                    content.Dispose();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypLabReqs weburl: {webUrl} controller: {controller} for id {labReqsEdit.LabReqID}  results status code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        // var d = results.Content.ReadAsStringAsync();
                        // d.Wait();
                        //  var readTask = results.Content.ReadAsAsync<LabReqsEditModel[]>();
                        // readTask.Wait();
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {


                            //if (!(retDicLabReqsModel))
                            //    log.LogWarning($"Method NypLabReqs weburl: {webUrl}  controller: {controller} for id {labReqID} no results for LabReqsModel");
                        }
                    }
                    else
                    {

                        throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for controller: {controller} for id {labReqsEdit.LabReqID}");
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method NypLabReqs weburl: {webUrl}  controller: {controller} for id {labReqsEdit.LabReqID} {ex.Message}");

                throw new Exception($"For url {webUrl}  controller: {controller} for id {labReqsEdit.LabReqID} {ex.Message}");
            }



        }

        //public IDictionary<int, LabReqsModel> NypLabReqs1(string webUrl, string controller, LabReqsModel labReqsModel)
        //{
        //    IDictionary<int, LabReqsModel> retDicLabReqsModel = new Dictionary<int, LabReqsModel>();

        //    try
        //    {


        //        using (var client = new HttpClient())
        //        {
        //            client.Timeout = TimeSpan.FromMinutes(30);
        //            client.BaseAddress = new Uri($"{webUrl}");
        //            var responseTask = client.GetAsync($"{controller}{labReqsModel}");
        //            responseTask.Wait();
        //            var results = responseTask.Result;

        //            if (results.IsSuccessStatusCode)
        //            {
        //                var readTask = results.Content.ReadAsAsync<LabReqsModel[]>();
        //                readTask.Wait();
        //                if (readTask.Result.Length > 0)
        //                {

        //                    retDicLabReqsModel = readTask.Result.ToDictionary(p => p.LabReqID);
        //                }
        //            }
        //            else
        //            {

        //                throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //        throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
        //    }


        //    return retDicLabReqsModel;
        //}

        public async Task<IDictionary<int, EditLabReqsReportModel>> GetLabReqsChangedRep(string webUrl, string controller, string stDate, string endDate, string indexNum, string csnFinNum, string mrn, ILog log)
        {
            IDictionary<int, EditLabReqsReportModel> retDicLabReqsModel = new Dictionary<int, EditLabReqsReportModel>();
            log.LogInformation($"GetLabReqsChangedRep for weburl {webUrl} controller {controller} date range {stDate}-{endDate} index num {indexNum} csn fin number {csnFinNum} mrn-patient id {mrn}");
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.BaseAddress = new Uri($"{webUrl}");
                    string searchStDate = stDate.ToString().Replace("/", "-");
                    string searchEndDate = endDate.ToString().Replace("/", "-");
                    string argStr = $"{controller}{searchStDate}/{searchEndDate}/{indexNum}/{csnFinNum}/{mrn}/{mrn}";
                    log.LogInformation($"GetLabReqsChangedRep calling api with args {argStr}");
                    var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}{argStr}");
                    //   var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}");
                    responseTask.Wait();
                    // content.Dispose();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<EditLabReqsReportModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {

                            retDicLabReqsModel = readTask.Result.ToDictionary(p => p.LabReqID);
                            if (!(retDicLabReqsModel.Any()))
                                log.LogWarning($"Method NypLabReqs weburl: {webUrl} controller: {controller} no results for LabReqsModel");
                            else
                                return retDicLabReqsModel;
                        }

                    }
                    else
                    {
                        throw new Exception($"Error on status code {results.StatusCode}");
                    }


                }
            }
            catch(Exception ex)
            {
                log.LogError($"GetLabReqsChangedRep for weburl {webUrl} controller {controller} date range {stDate}-{endDate} index num {indexNum} csn fin number {csnFinNum} mrn-patient id {mrn} {ex.Message}");
                throw new Exception($"GetLabReqsChangedRep for weburl {webUrl} controller {controller} date range {stDate}-{endDate} index num {indexNum} csn fin number {csnFinNum} mrn-patient id {mrn} {ex.Message}");
            }
            return null;
        }
    }
}
