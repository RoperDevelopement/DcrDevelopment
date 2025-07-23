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
    public class NypInvoiceApi
    {
        private static NypInvoiceApi instance = null;
        public static NypInvoiceApi InvoiceIntance
        {
            get
            {
                if (instance == null)
                    instance = new NypInvoiceApi();
                return instance;
            }
        }
        private NypInvoiceApi()
        {
        }

        public async Task<IList<string>> NypInvoiceDepartment(Uri webUrl, string controller,ILog log)
        {
            IList<string> retInvDep = new List<string>();

            try
            {

                log.LogInformation($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} results: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypInvoiceDepartment[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retInvDep = readTask.Result.Select(p => p.Department).ToList();
                            if(!(retInvDep.Any()))
                                log.LogWarning($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} no results for list NypInvoiceDepartment ");
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
                log.LogError($"Method NypInvoiceDepartment webUrl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retInvDep;

        }
        public async Task<IList<string>> NypInvoiceAccount(Uri webUrl, string controller,ILog log)
        {
            IList<string> retInvAcc = new List<string>();

            try
            {
                log.LogInformation($"Method NypInvoiceAccount weburl: {webUrl.ToString()} controller: {controller} ");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoiceAccount weburl: {webUrl.ToString()} controller: {controller} results statues code retruned: {results.StatusCode} ");
                    if (results.IsSuccessStatusCode)
                    {
                       // var s = results.Content.ReadAsStringAsync();
                       // s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypInvoiceAccount[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retInvAcc = readTask.Result.Select(p => p.Account).ToList();
                            if(!(retInvAcc.Any()))
                                log.LogWarning($"Method NypInvoiceAccount weburl: {webUrl.ToString()} controller: {controller} no results for list NypInvoiceAccount  ");
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
                log.LogError($"Method NypInvoiceAccount weburl: {webUrl.ToString()} controller: {controller} {ex.Message} ");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retInvAcc;

        }

        public async Task<IList<string>> NypInvoiceCategory(Uri webUrl, string controller,ILog log)
        {
            IList<string> retInvCat = new List<string>();

            try
            {

                log.LogInformation($"Method NypInvoiceCategory weburl: {webUrl.ToString()} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoiceCategory weburl: {webUrl.ToString()} controller: {controller} results retruned: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                      //  var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<NypInvoiceCategory[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retInvCat = readTask.Result.Select(p => p.Category).ToList();
                            if(!(retInvCat.Any()))
                                log.LogWarning($"Method NypInvoiceCategory weburl: {webUrl.ToString()} controller: {controller} results retruned: {results.StatusCode} no results for list NypInvoiceCategory");

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
                log.LogError($"Method NypInvoiceCategory weburl: {webUrl.ToString()} controller: {controller} {ex.Message}");

                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retInvCat;

        }
        public async Task<IDictionary<string, InvoicesModel>> NypInvoices(Uri webUrl, string controller, InvoicesModel invoices,ILog log)
        {
            IDictionary<string, InvoicesModel> retInv = new Dictionary<string, InvoicesModel>();
            log.LogInformation($"Method NypInvoices for webUrl: {webUrl.ToString()} controller: {controller} ");
            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{webUrl}");
                    var jsonString = JsonConvert.SerializeObject(invoices);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method NypInvoices for webUrl: {webUrl.ToString()} controller: {controller} results code: {results.StatusCode} ");
                    content.Dispose();
                    if (results.IsSuccessStatusCode)
                    {
                        //var s = results.Content.ReadAsStringAsync();
                        //s.Wait();
                        var readTask = results.Content.ReadAsAsync<InvoicesModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            // foreach(var str in readTask.Result)
                            // {
                            //  retReasons.Add(str.ToString());
                            // }
                            retInv = readTask.Result.ToDictionary(p => p.FileUrl);
                            if(!(retInv.Any()))
                                log.LogWarning($"Method NypInvoices for webUrl: {webUrl.ToString()} controller: {controller} results code: {results.StatusCode} no results returned for InvoicesModel ");
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

                log.LogWarning($"Method NypInvoices for webUrl: {webUrl.ToString()} controller: {controller} {ex.Message} ");
                throw new Exception($"For url {webUrl} for sp:{controller} {ex.Message}");
            }


            return retInv;
        }

    }
}
