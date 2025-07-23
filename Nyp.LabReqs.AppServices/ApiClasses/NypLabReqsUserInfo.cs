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
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
    public class NypLabReqsUserInfo
    {

        private static NypLabReqsUserInfo instance = null;
        public static NypLabReqsUserInfo NypLabReqsUserInfoApiIntance
        {
            get
            {
                if (instance == null)
                    instance = new NypLabReqsUserInfo();
                return instance;
            }
        }
        private NypLabReqsUserInfo()
        {
        }

        public async Task<NypLabReqsUsersModel> GetNypUserInfor(Uri webUrl, string controller, ILog log)
        {
            NypLabReqsUsersModel retUserInfo = new NypLabReqsUsersModel();

            try
            {

                log.LogInformation($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = webUrl;
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} results: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypLabReqsUsersModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retUserInfo = readTask.Result[0];
                            
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
                log.LogError($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retUserInfo;

        }

        public async Task<IDictionary<string, NypLabReqsUsersModel>> GetAllNypUserInfor(Uri webUrl, string controller, ILog log)
        {
           IDictionary<string, NypLabReqsUsersModel> retUserInfo = new Dictionary<string, NypLabReqsUsersModel>();

            try
            {

                log.LogInformation($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = webUrl;
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} results: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypLabReqsUsersModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }

                            retUserInfo = readTask.Result.ToDictionary(p => p.Cwid);


                        }
                        else
                        {
                            if (results.StatusCode == System.Net.HttpStatusCode.OK)
                                return null;

                            throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
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
                log.LogError($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retUserInfo;

        }



        public async Task DelNypUser(Uri webUrl, string controller, ILog log)
        {
            

            try
            {

                log.LogInformation($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = webUrl;
                    var responseTask = client.DeleteAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} results: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {

                        throw new Exception($"User {controller} not deleted Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            

        }

        public async Task AddUpdateNypUser(Uri webUrl, string controller, ILog log, NypLabReqsUsersModel reqsUsersModel)
        {


            try
            {

                log.LogInformation($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = webUrl;
                    var jsonString = JsonConvert.SerializeObject(reqsUsersModel);

                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} results: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {

                        throw new Exception($"User {controller} not deleted Return status code :{results.StatusCode.ToString()} for url {webUrl} for sp:{controller}");
                    }
                }
            }

            catch (Exception ex)
            {
                log.LogError($"Method GetNypUserInfor webUrl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }




        }

    }
}
