using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Edocs.Demo.Application.Services.Models;
using System.Xml.Linq;
using Newtonsoft.Json;
 
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Edocs.Demo.Application.Services.ApiConsts
{
    public class FileNameModel
    {
        public string FileName
        { get; set; }
    }
    public class DemoApis
    {
        private static DemoApis instance = null;

        private DemoApis() { }

        public static DemoApis DemoInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DemoApis();
                }
                return instance;
            }
        }
        //public async Task<IDictionary<int,BspPlanDepPermitsModel>> GetBSBPlanDepPermits(IHttpClientFactory clientFactory, Uri uri, string controller, BspPlanDepPermitsModel BspPlanDepModel)

        //{
        //    IDictionary<int, BspPlanDepPermitsModel> dicBSBPD = new Dictionary<int, BspPlanDepPermitsModel>();
        //    try
        //    {
        //        using (var httpClient = clientFactory.CreateClient())
        //        {
        //            httpClient.BaseAddress = uri;
        //            string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}/{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
        //            using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
        //            //   httpResponse.EnsureSuccessStatusCode();
        //            var readTask = httpResponse.Content.ReadAsStringAsync();
        //            readTask.Wait();
        //            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
        //            {

        //               dicBSBPD = JsonConvert.DeserializeObject<Dictionary<int,BspPlanDepPermitsModel>(readTask.Result);
        //                //dicBSBPD = readTask.Result.ToDictionary(p => p.TrackingID);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return null;
        //}
        public async Task<IDictionary<int, BspPlanDepPermitsModel>> GetBSBPlanDepPermits(Uri uri, string controller, BspPlanDepPermitsModel BspPlanDepModel,bool addSW)

        {

            IList<BspPlanDepPermitsModel> lstBSBPD = new List<BspPlanDepPermitsModel>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string reqUri = $"{controller}/{BspPlanDepModel.PermitNum}/{BspPlanDepModel.Address}/{addSW}";
                    httpClient.BaseAddress = uri;
                    //  string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}///{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
                    // using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress}{reqUri}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {

                        //   dicBSBPD = JsonConvert.DeserializeObject < Dictionary<int, BspPlanDepPermitsModel>(readTask.Result);
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();

                        lstBSBPD = JsonConvert.DeserializeObject<List<BspPlanDepPermitsModel>>(readTask.Result.ToString());
                        IDictionary<int, BspPlanDepPermitsModel> dicBSBPD = new Dictionary<int, BspPlanDepPermitsModel>();
                        foreach (BspPlanDepPermitsModel bsp in lstBSBPD)
                        {
                            bsp.FileUrl = System.IO.Path.Combine(bsp.FileUrl, bsp.FileName);
                            dicBSBPD.Add(bsp.PermitNum, bsp);
                        }
                        return dicBSBPD;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }
        public async Task<IList<BSBPlublicWorksDepartmentYearModel>> GetBSBPWDYears(Uri uri, string controller)
        {
            IList<BSBPlublicWorksDepartmentYearModel> bSBPlublicWorksDepartmentYears = new List<BSBPlublicWorksDepartmentYearModel>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string reqUri = $"{controller}/{DemoConstants.SpBSBPublicWorksGetYears}";
                    httpClient.BaseAddress = uri;
                    //  string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}///{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
                    // using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress}{reqUri}"); 
                    responseTask.Wait();
                     var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {

                        //   dicBSBPD = JsonConvert.DeserializeObject < Dictionary<int, BspPlanDepPermitsModel>(readTask.Result);
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();

                        bSBPlublicWorksDepartmentYears = JsonConvert.DeserializeObject<List<BSBPlublicWorksDepartmentYearModel>>(readTask.Result.ToString());
                        
                        

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bSBPlublicWorksDepartmentYears;
        }
        public async Task<IList<BSBPlublicWorksDepartmentModel>> GetBSBPWDDepartment(Uri uri, string controller)
        {
            IList<BSBPlublicWorksDepartmentModel> bSBPlublicWorksDepartmentYears = new List<BSBPlublicWorksDepartmentModel>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string reqUri = $"{controller}/{DemoConstants.SpBSBPublicWorksGetDepartment}";
                    httpClient.BaseAddress = uri;
                    //  string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}///{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
                    // using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress}{reqUri}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {

                        //   dicBSBPD = JsonConvert.DeserializeObject < Dictionary<int, BspPlanDepPermitsModel>(readTask.Result);
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();

                        bSBPlublicWorksDepartmentYears = JsonConvert.DeserializeObject<List<BSBPlublicWorksDepartmentModel>>(readTask.Result.ToString());

                        return bSBPlublicWorksDepartmentYears;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bSBPlublicWorksDepartmentYears;
        }
        public async Task<IDictionary<int, BspPlanDepPermitsModel>> GetBSBPlanDepPermitsBySearchTxt(Uri uri, string controller, string searchText)

            {

                IList<BspPlanDepPermitsModel> lstBSBPD = new List<BspPlanDepPermitsModel>();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        string reqUri = $"{controller}/{searchText}";
                        httpClient.BaseAddress = uri;
                        //  string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}///{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
                        // using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                        //   httpResponse.EnsureSuccessStatusCode();
                        var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress}{reqUri}");
                        responseTask.Wait();
                        var results = responseTask.Result;

                        if (results.IsSuccessStatusCode)
                        {

                            //   dicBSBPD = JsonConvert.DeserializeObject < Dictionary<int, BspPlanDepPermitsModel>(readTask.Result);
                            var readTask = results.Content.ReadAsStringAsync();
                            readTask.Wait();

                            lstBSBPD = JsonConvert.DeserializeObject<List<BspPlanDepPermitsModel>>(readTask.Result.ToString());
                            IDictionary<int, BspPlanDepPermitsModel> dicBSBPD = new Dictionary<int, BspPlanDepPermitsModel>();
                            foreach (BspPlanDepPermitsModel bsp in lstBSBPD)
                            {
                                bsp.FileUrl = System.IO.Path.Combine(bsp.FileUrl, bsp.FileName);
                                dicBSBPD.Add(bsp.PermitNum, bsp);
                            }
                            return dicBSBPD;

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return null;
            }

        public async Task<IDictionary<string,BSBPlublicWorksDepartmentProjectNameModel>> GetBSBPWDRecords(IHttpClientFactory clientFactory,Uri uri, string controller, string dep,string year,string pName,string keyWords)

        {

           
            IDictionary<string, BSBPlublicWorksDepartmentProjectNameModel> retDic = new Dictionary<string, BSBPlublicWorksDepartmentProjectNameModel>();
            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    string reqUri = $"{controller}/{dep}/{year}/{pName}/{keyWords}";
                    httpClient.BaseAddress = uri;
                    //  string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}///{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
                    // using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress}{reqUri}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {
                        IList<BSBPlublicWorksDepartmentProjectNameModel> lstBSBPD = new List<BSBPlublicWorksDepartmentProjectNameModel>();
                        //   dicBSBPD = JsonConvert.DeserializeObject < Dictionary<int, BspPlanDepPermitsModel>(readTask.Result);
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();

                        lstBSBPD = JsonConvert.DeserializeObject<List<BSBPlublicWorksDepartmentProjectNameModel>>(readTask.Result.ToString());
                         
                        foreach (BSBPlublicWorksDepartmentProjectNameModel bsp in lstBSBPD)
                        {
                            bsp.FileUrl = System.IO.Path.Combine(bsp.FileUrl, bsp.ProdFileName);
                            retDic.Add(bsp.ProjectName, bsp);
                        }
                      

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return retDic;
        }

        public async Task<List<BSBPlublicWorksProjectNameModel>> GetBSBPWDPNames(IHttpClientFactory clientFactory, Uri uri,string controller, string pName, string sp)
        {
            List<BSBPlublicWorksProjectNameModel> retList = new List<BSBPlublicWorksProjectNameModel>();
            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    string reqUri = $"{controller}/{pName}/{sp}";
                    httpClient.BaseAddress = uri;
                    //  string reqUri = $"{controller}{BspPlanDepModel.PermitNum}/{BspPlanDepModel.ParcelNumber}/{BspPlanDepModel.ExePermitNumber}///{BspPlanDepModel.ZoneNumber}/{BspPlanDepModel.GoCode}/{BspPlanDepModel.ConstCompany}/{BspPlanDepModel.OwnerLot}";
                    // using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress}{reqUri}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {
                        
                        //   dicBSBPD = JsonConvert.DeserializeObject < Dictionary<int, BspPlanDepPermitsModel>(readTask.Result);
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();

                        retList = JsonConvert.DeserializeObject<List<BSBPlublicWorksProjectNameModel>>(readTask.Result.ToString());

                         


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return retList;
        }
        //public   async Task<string> GetRepFileName(IHttpClientFactory clientFactory, Uri uri, string controller, string ID)
        //{
        //    FileNameModel fileNameModel = new FileNameModel();

        //    try
        //    {

        //        using (var httpClient = clientFactory.CreateClient())
        //        {

        //            httpClient.BaseAddress = uri;
        //            string reqUri = $"{controller}{ID}";
        //            using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
        //            //   httpResponse.EnsureSuccessStatusCode();
        //            var readTask = httpResponse.Content.ReadAsAsync<FileNameModel[]>();
        //            readTask.Wait();
        //            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                fileNameModel.FileName = readTask.Result[0].FileName;

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Could not get ile for  {ID} {ex.Message}");
        //    }
        //    return fileNameModel.FileName;
        //}
    }
}
