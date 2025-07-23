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
using Microsoft.AspNetCore.Http;
namespace BinMonitorAppService.ApiClasses
{
    public class GetApis
    {
        private static GetApis instance = null;
        public static GetApis GetApisInctance
        {
            get
            {
                if (instance == null)
                    instance = new GetApis();
                return instance;
            }
            }
        private GetApis()
        {
        }
        public async Task<IList<string>> ApiBins(string apiUrl,string sqlStoredProcedure,bool allBIns)
        {
            IList<string> BinID = new List<string>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sqlStoredProcedure}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<BinModel[]>();
                    readTask.Wait();
                    //List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                     

                    List<string> tempList = (readTask.Result.Select(p => p.BinID).ToList());
                    //if (allBIns)
                    //{
                    //    tempList.Add("0000-OpenBins");
                    //    tempList.Add("0001-Not Register");
                    //    tempList.Add("0002-Register");
                    //    tempList.Add("0003-Not Assigned");
                    //}


                    //          
                    if(allBIns)
                    { 
                        tempList.Add(" ");
                        tempList.Sort();
                    }



                    BinID = tempList;

                    //BinID = readTask.Result.Select(p => p.BinID).ToList();
                    //BinID.Add("000");
                    //BinID = BinID.OrderBy(p => p).First();
                }
                else
                    throw new Exception($"Getting bins {results.Content}");
            }
            return  BinID;
        }
        public async Task<IList<string>> ApiGetActiveBinsID(string apiUrl, string sqlStoredProcedure)
        {
            IList<string> BinID = new List<string>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sqlStoredProcedure}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<BinRegProcessModel[]>();
                    readTask.Wait();
                    List<string> tempList = (readTask.Result.Select(p => p.BinID).ToList());
                    tempList.Add(" ");
                    tempList.Sort();
                    tempList.Sort();
                    BinID = tempList;
                }
                else
                    throw new Exception($"Getting bins {results.Content}");
            }
            return BinID;
        }

        public async Task<IList<string>> ApiGetIList(string apiUrl, string sqlStoredProcedure)
        {
            IList<string> BinID = new List<string>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sqlStoredProcedure}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<List<string>>();
                    readTask.Wait();
                    BinID = readTask.Result;
                }
                else
                    throw new Exception($"Getting bins {results.Content}");
            }
            return BinID;
        }


        public async Task<IDictionary<string,BinRegProcessModel>> ApiActiveBins(string apiUrl,string sp)
        {
            IDictionary<string, BinRegProcessModel> binRegPro = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {
                   
                    var readTask = results.Content.ReadAsAsync<BinRegProcessModel[]>();
                    readTask.Wait();
                    binRegPro = readTask.Result.ToDictionary(p => p.BinID);
                   


                }
            }
            return binRegPro;
        }


        public async Task<IDictionary<string, BinRegProcessModel>> ApiActiveBinsByBatchId(string apiUrl, string sp)
        {
            IDictionary<string, BinRegProcessModel> binRegPro = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {

                    var readTask = results.Content.ReadAsAsync<BinRegProcessModel[]>();
                    readTask.Wait();
                    binRegPro = readTask.Result.ToDictionary(p => p.BatchID.ToString());



                }
            }
            return binRegPro;
        }
        public async Task<BinRegProcessModel> ApiActiveBinsModel(string apiUrl, string sp)
        {
            BinRegProcessModel binRegPro = new BinRegProcessModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<BinRegProcessModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                        binRegPro = readTask.Result[0];
                }
            }
            return binRegPro;
        }
        public async Task<CategoryCheckPointModel> ApiCategoryCheckPoint(string apiUrl, string sp)
        {
            CategoryCheckPointModel cp = new CategoryCheckPointModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<CategoryCheckPointModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                        cp = readTask.Result[0];
                }
            }
            return cp;
        }

        public async Task<EmailReportModel> ApiEmailReportModel(string apiUrl, string sp)
        {
            EmailReportModel ERM = new EmailReportModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {
                    try
                    { 
                    var readTask = results.Content.ReadAsAsync<EmailReportModel[]>();
                    readTask.Wait();

                        if(readTask.Result.Length > 0)
                        ERM = readTask.Result[0];
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine();
                    }
                }
            }
            return ERM;
        }


        public async Task<IList<string>> ApiUserInfo(string sp,string webApiWrl)
        {
            IList <string> UserInfo = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{webApiWrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<UsersModel[]>();
                    readTask.Wait();
                    List<string> tempUserList = readTask.Result.Where(p => !(p.EmailAddress.ToLower().Contains("edocs"))).Select(p => p.FirstName + " " + p.LastName).ToList();
                    tempUserList.Add("  ");
                    tempUserList.Sort();
                    UserInfo = tempUserList;
                    
                    //UserFirstLastName = readTask.Result.Select(p => p.FirstName + " " + p.LastName).ToList();
                    //LassignedBy = readTask.Result.Where(p => !(p.EmailAddress.ToLower().Contains("edocs"))).Select(p => p.FirstName + " " + p.LastName).ToList();

                    //LassignedTo = LassignedBy;
                    //CategoryName = readTask.Result.Where(p => !(p.CategoryName.ToLower().Contains("edocs"))).Select(p => p.CategoryName).ToList();

                }
            }
            return UserInfo;
        }

        public async Task<BinRegistorModel> ConvertBinRegProcessModelToBinRegistorModel(BinRegProcessModel binRegProcessModel,string repProcCloseBatch)

        {
            BinRegistorModel binRegistorModel = new BinRegistorModel();
            binRegistorModel.BinProcessBinModel = new BinProcessBinModel();
            binRegistorModel.BinsClosedModel = new BinsClosedModel();
            Task.Factory.StartNew(async () =>
            {
                
                binRegistorModel.BinID = binRegProcessModel.BinID;
                binRegistorModel.BatchID = binRegProcessModel.BatchID;
                binRegistorModel.BinComments = binRegProcessModel.BinComments;
                binRegistorModel.RegAssignedTo = binRegProcessModel.RegAssignedTo;
                binRegistorModel.RegCompletedAt = binRegProcessModel.RegCompletedAt;
                binRegistorModel.RegCompletedBy = binRegProcessModel.RegCompletedBy;
                binRegistorModel.RegAssignedBy = binRegProcessModel.RegAssignedBy;
                binRegistorModel.BinContents = binRegProcessModel.BinContents;
                binRegistorModel.CategoryName = binRegProcessModel.CategoryName;
                binRegistorModel.RegAssignedBy = binRegProcessModel.RegAssignedBy;

                binRegistorModel.RegCreatedBy = binRegProcessModel.RegCreatedBy;
                binRegistorModel.RegStartedAt = binRegProcessModel.RegStartedAt;
                binRegistorModel.RegProcesClose = repProcCloseBatch;
                binRegistorModel.BinProcessBinModel.ProcessAssignedBy = binRegProcessModel.ProcessAssignedBy;
                binRegistorModel.BinProcessBinModel.ProcessAssignedTo = binRegProcessModel.ProcessAssignedTo;
                binRegistorModel.BinProcessBinModel.ProcessCompletedAt = binRegProcessModel.ProcessCompletedAt;
                binRegistorModel.BinProcessBinModel.ProcessCompletedBy = binRegProcessModel.ProcessCompletedBy;
                binRegistorModel.BinProcessBinModel.ProcessStartAt = binRegProcessModel.ProcessStartAt;

                binRegistorModel.BinsClosedModel.BinClosedBy = binRegProcessModel.BinClosedBy;
                binRegistorModel.BinsClosedModel.BinCompletedAt = binRegProcessModel.BinCompletedAt;
                binRegistorModel.BinsClosedModel.ClosedCreatedAt = binRegProcessModel.ClosedCreatedAt;
                
            }).Wait();
            return binRegistorModel;
        }
        
        public async Task<IList<string>> ApiCategories(string sp,string apiUrl)
        {
            IList<string> CategoryName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<CategoryIdName[]>();
                    readTask.Wait();

                    //CategoryName = readTask.Result.Where(p=>!(p.CategoryName.ToLower().Contains("edocs"))).Select(p => p.CategoryName).ToList();
                    List<string> tempList = readTask.Result.Select(p => p.CategoryName).ToList();
                    tempList.Add("  ");
                    tempList.Sort();
                    //CategoryName = readTask.Result.Select(p => p.CategoryName).ToList();
                    CategoryName = tempList;



                }
            }
            return CategoryName;


        }

        public async Task ApiCreateUpdateBatch(BinRegProcessModel binRegistorProcModel,string webApiUrl,string regProcessBatch)
        {
            using (var client = new HttpClient())
            {
                BinRegistorModel binRegistorModel = await GetApis.GetApisInctance.ConvertBinRegProcessModelToBinRegistorModel(binRegistorProcModel, regProcessBatch);
                client.BaseAddress = new Uri($"{webApiUrl}");
                var jsonString = JsonConvert.SerializeObject(binRegistorModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{SqlConstants.ApiCreateBatch}", content);
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

        public async Task<IDictionary<string,CategoryColorModel>> ApiCategoriesColors(string sp, string apiUrl,bool keyID)
        {
            IDictionary<string,CategoryColorModel> CategoryName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    try
                    {
                        var readTask1 = results.Content.ReadAsStringAsync();
                        readTask1.Wait();
                        var readTask = results.Content.ReadAsAsync<CategoryColorModel[]>();
                    readTask.Wait();
                    if(keyID)
                        CategoryName = readTask.Result.ToDictionary(p => p.CategorId);
                    else
                        CategoryName = readTask.Result.ToDictionary(p => p.CategoryName);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                }
            }
            return CategoryName;


        }

        public async Task<IDictionary<string, CategoryColorCodesModel>> ApiCategoriesColorsCodes(string sp, string apiUrl)
        {
            IDictionary<string, CategoryColorCodesModel> CategoryName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<CategoryColorCodesModel[]>();
                    readTask.Wait();
                    
                        CategoryName = readTask.Result.ToDictionary(p => p.ColorCodeId);
                    
                }
            }
            return CategoryName;
        }

        public async Task<IDictionary<string, CategoriesModel>> ApiGetCategoriesTotal(string sp, string apiUrl)
        {
            IDictionary<string, CategoriesModel> CategoryName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<CategoriesModel[]>();
                    readTask.Wait();
                    CategoryName = readTask.Result.ToDictionary(p => p.CategoryName);
                }
            }
            return CategoryName;
        }
        public async Task<CategoryCheckPointModel> ApiGetCategoriesDurations(string sp, string apiUrl)
        {
            CategoryCheckPointModel categoryCheckPointModel = new CategoryCheckPointModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.GetAsync($"{sp}");
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {
                    try
                    {
                        var k = results.Content.ReadAsStringAsync();
                        k.Wait();
                        var readTask = results.Content.ReadAsAsync<CategoryCheckPointModel[]>();
                        readTask.Wait();

                        if (readTask.Result.Length > 0)
                            categoryCheckPointModel = readTask.Result[0];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                    }
                }
            }
            return categoryCheckPointModel;
        }

    

    }
}

