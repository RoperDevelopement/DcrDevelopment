using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Edocs.ITS.AppService.Models;
using System.Xml.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Edocs.ITS.AppService.ApisConst
{
    public class EdocsITSApis
    {
        public static async Task AddNewCustomer(IHttpClientFactory clientFactory, Uri uri, string controller, EdocsITSCustomersModel customersModel)
        {

            try
            {
                var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(customersModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not add new customer {customersModel.EdocsCustomerName} {ex.Message}");
            }
        }
        public static async Task<IList<TrackingIDProjectNameModel>> GetTrackingIDs(IHttpClientFactory clientFactory, Uri uri, string controller, string custNumber)
        {
            IList<TrackingIDProjectNameModel> trackingIDProjects = new List<TrackingIDProjectNameModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{custNumber}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        trackingIDProjects = JsonConvert.DeserializeObject<List<TrackingIDProjectNameModel>>(readTask.Result);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        
                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {custNumber} {ex.Message}");
            }
            return trackingIDProjects;
        }
        public static async Task<IList<TrackingIDProjectNameModel>> GetTrackingDocNames(IHttpClientFactory clientFactory, Uri uri, string controller, string custNumber,string sDate,string eDate)
        {
            IList<TrackingIDProjectNameModel> trackingIDProjects = new List<TrackingIDProjectNameModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{sDate}/{eDate}/{custNumber}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        trackingIDProjects = JsonConvert.DeserializeObject<List<TrackingIDProjectNameModel>>(readTask.Result);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {custNumber} {ex.Message}");
            }
            return trackingIDProjects;
        }
        public static async Task AddCustomInvoice(IHttpClientFactory clientFactory, Uri uri, string controller, IList<CustomInvoiceModel> customInvoices)
        {

            try
            {
                var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(customInvoices), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var responseStream = await httpResponse.Content.ReadAsStringAsync();
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not  Add Custom Invoice   {ex.Message}");
            }
        }
        public static async Task<IList<EdocsITSMinMaxDateModel>> GetMaxMinDate(IHttpClientFactory clientFactory, Uri uri, string controller, string custID)
        {
            IList<EdocsITSMinMaxDateModel> retStr = new List<EdocsITSMinMaxDateModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.GetAsync($"{controller}{custID}").ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();

                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var readTask = httpResponse.Content.ReadAsStringAsync();
                        readTask.Wait();
                        if (!(readTask.Result.ToLower().Contains("null")))
                            retStr = JsonConvert.DeserializeObject<List<EdocsITSMinMaxDateModel>>(readTask.Result);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get min max dates for customer id {custID} {ex.Message}");
            }
            return retStr;
        }
        public static async Task<string> GetRepFileName(IHttpClientFactory clientFactory, Uri uri, string controller, string ID)
        {
            FileNameModel fileNameModel = new FileNameModel();

            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    string reqUri = $"{controller}{ID}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsAsync<FileNameModel[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        fileNameModel.FileName = readTask.Result[0].FileName;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get ile for  {ID} {ex.Message}");
            }
            return fileNameModel.FileName;
        }
        public static async Task InventoryTransfer(IHttpClientFactory clientFactory, Uri uri, string controller, EdocsITSInventoryTransfer edocsITSInventory)
        {

            try
            {


                var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(edocsITSInventory), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
        }

        public static async Task UploadInventoryHtml(IHttpClientFactory clientFactory, Uri uri, string controller, HtmlFileModel fileModel)
        {

            try
            {


                var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fileModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    /// string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Error upload invoice html data {fileModel.InvoiceNum}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error upload invoice html data {fileModel.InvoiceNum} {ex.Message}");
            }
        }
        //InvoiceNumberDateSentModel
        public static async Task<IList<InvoiceNumberDateSentModel>> GetInvoiceNumDateSent(IHttpClientFactory clientFactory, Uri uri, string controller, int custNum)
        {

            try
            {


                //   var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fileModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;

                    using var httpResponse = httpClient.GetAsync($"{controller}{custNum}").ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    // using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                    var invNumSent = JsonConvert.DeserializeObject<List<InvoiceNumberDateSentModel>>(responseBody);
                    return invNumSent;
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Error getting invoice#  {custNum}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error upload invoice html data {custNum} {ex.Message}");
            }
            return null;
        }
        public static List<T> Deserialize<T>(string serializedJSONString)
        {
            var stuff = JsonConvert.DeserializeObject<List<T>>(serializedJSONString);
            return stuff;
        }
        public static async Task<IList<UpdateInvoicesView>> GetInvoices(IHttpClientFactory clientFactory, Uri uri, string controller, int invNum)
        {

            try
            {


                //   var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fileModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;

                    using var httpResponse = httpClient.GetAsync($"{controller}/{invNum}").ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    // using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                    // responseBody = responseBody.Replace("[", "").Replace("]", "").Trim();
                    var invoices = JsonConvert.DeserializeObject<List<UpdateInvoicesView>>(responseBody);
                    return invoices;
                    //  if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Error getting invoice#  {invNum}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error upload invoice html data {invNum} {ex.Message}");
            }
            return null;
        }
        public static async Task<IList<UpdateInvoicesView>> UpdateInvoicePaid(IHttpClientFactory clientFactory, Uri uri, string controller, int invNum, int custID, string amountPaid)
        {

            try
            {


                //   var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fileModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;

                    using var httpResponse = httpClient.GetAsync($"{controller}/{invNum}/{amountPaid}/{custID}").ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    // using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                    // responseBody = responseBody.Replace("[", "").Replace("]", "").Trim();
                    var invoices = JsonConvert.DeserializeObject<List<UpdateInvoicesView>>(responseBody);
                    return invoices;
                    //  if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Error getting invoice#  {invNum}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error upload invoice html data {invNum} {ex.Message}");
            }
            return null;
        }
        public static async Task<IList<CustomersInvoicesModel>> GetCustomerUnpaidInvoices(IHttpClientFactory clientFactory, Uri uri, string controller)
        {

            try
            {


                //   var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fileModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;

                    using var httpResponse = httpClient.GetAsync($"{controller}").ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    // using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                    // responseBody = responseBody.Replace("[", "").Replace("]", "").Trim();
                    var invoices = JsonConvert.DeserializeObject<List<CustomersInvoicesModel>>(responseBody);
                    return invoices;
                    //  if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Error getting invoices");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting incoices {ex.Message}");
            }
            return null;
        }
        public static async Task<HtmlFileModel> GetInventoryHtml(IHttpClientFactory clientFactory, Uri uri, string controller, int invNum)
        {

            try
            {


                //   var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fileModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.GetAsync($"{controller}/{invNum}").ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    // using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                    var htmlData = JsonConvert.DeserializeObject<List<HtmlFileModel>>(responseBody);
                    return htmlData[0];
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Error getting invoice#  {invNum}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error upload invoice html data {invNum} {ex.Message}");
            }
            return null;
        }
        public static async Task UpDateAcceptRejectDocs(IHttpClientFactory clientFactory, Uri uri, string controller, AcceptRejectDocumentsModel acceptReject, string sp)
        {

            try
            {


                var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(acceptReject), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PutAsync($"{controller}/{sp}", newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    // httpResponse.EnsureSuccessStatusCode();
                    using var responseStream = httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Cound not update accept reject docs id {acceptReject.ID}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cound not update accept reject docs id {acceptReject.ID} {ex.Message}");
            }
        }
        public static async Task<IDictionary<string, EdocsITSInventoryTransfer>> GetInventorySent(IHttpClientFactory clientFactory, Uri uri, string controller, string stDate, string endDate, string spName, string repType, string custName)
        {
            IDictionary<string, EdocsITSInventoryTransfer> edocsITSInventory = new Dictionary<string, EdocsITSInventoryTransfer>();
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {
                    stDate = stDate.Replace('/', '-');
                    endDate = endDate.Replace('/', '-');
                    httpClient.BaseAddress = uri;
                    string reqUri = $"{controller}{repType}/{stDate}/{endDate}/{spName}/{custName}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSInventoryTransfer[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
            return edocsITSInventory;
        }

        public static async Task<IDictionary<string, EdocsITSScanningManModel>> GetInventoryRec(IHttpClientFactory clientFactory, Uri uri, string controller, string stDate, string endDate, string spName, string repType, string custName)
        {
            IDictionary<string, EdocsITSScanningManModel> edocsITSInventory = new Dictionary<string, EdocsITSScanningManModel>();
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {
                    stDate = stDate.Replace('/', '-');
                    endDate = endDate.Replace('/', '-');
                    httpClient.BaseAddress = uri;
                    string reqUri = $"{controller}{repType}/{stDate}/{endDate}/{spName}/{custName}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //    httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSScanningManModel[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
            return edocsITSInventory;
        }

        public static async Task<IDictionary<int, EdocsITSCostScanningModel>> GetScanninCost(IHttpClientFactory clientFactory, Uri uri, string controller, DateTime stDate, DateTime endDate, string custID,string invType)
        {
            IDictionary<int, EdocsITSCostScanningModel> edocsITSInventory = new Dictionary<int, EdocsITSCostScanningModel>();
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {
                    string sDate = stDate.ToString("yyyy-MM-dd").Replace('/', '-');
                    string eDate = endDate.ToString(("yyyy-MM-dd")).Replace('/', '-');
                    httpClient.BaseAddress = uri;
                    string reqUri = $"{controller}{sDate}/{eDate}/{custID}/{invType}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //    httpResponse.EnsureSuccessStatusCode();
                    var readTask1 = httpResponse.Content.ReadAsStringAsync();
                    readTask1.Wait();
                    //  var readTask = httpResponse.Content.ReadAsStringAsync();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSCostScanningModel[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        edocsITSInventory = readTask.Result.ToDictionary(p => p.ID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
            return edocsITSInventory;
        }


        public static async Task<IDictionary<string, EdocsITSProjectNameNumber>> GetReportsByProjectNameNum(IHttpClientFactory clientFactory, Uri uri, string controller, string stDate, string endDate, string spName)
        {
            IDictionary<string, EdocsITSProjectNameNumber> projNameNum = new Dictionary<string, EdocsITSProjectNameNumber>();
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {
                    stDate = stDate.Replace('/', '-');
                    endDate = endDate.Replace('/', '-');
                    httpClient.BaseAddress = uri;
                    string reqUri = $"{controller}{stDate}/{endDate}/{spName}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //    httpResponse.EnsureSuccessStatusCode();
                    var str = httpResponse.Content.ReadAsStringAsync();
                    str.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSProjectNameNumber[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (readTask.Result == null)
                            return null;
                        projNameNum = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
            return projNameNum;
        }
        public static async Task<IDictionary<string, EdocsITSProjectNameNumber>> GetReportsByTrackIDDocName(IHttpClientFactory clientFactory, Uri uri, string controller, string stDate, string endDate, string trackID,string repType)
        {
            IDictionary<string, EdocsITSProjectNameNumber> projNameNum = new Dictionary<string, EdocsITSProjectNameNumber>();
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {
                    stDate = stDate.Replace('/', '-');
                    endDate = endDate.Replace('/', '-');
                    httpClient.BaseAddress = uri;
                    string reqUri = $"{controller}{stDate}/{endDate}/{trackID}/{repType}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //    httpResponse.EnsureSuccessStatusCode();
                    var str = httpResponse.Content.ReadAsStringAsync();
                    str.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSProjectNameNumber[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (readTask.Result == null)
                            return null;
                        projNameNum = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
            return projNameNum;
        }

        // public static async Task<IDictionary<int, EdocsITSScanningManModel>> GetTaskID(IHttpClientFactory clientFactory, Uri uri, string controller, string trackingID,string custName)
        public static async Task<EdocsITSScanningManModel> GetTaskID(IHttpClientFactory clientFactory, Uri uri, string controller, string trackingID, string custName)
        {

            IDictionary<int, EdocsITSScanningManModel> keyValues = new Dictionary<int, EdocsITSScanningManModel>();
            EdocsITSScanningManModel edocsITSScanning = null;

            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}{trackingID}/{custName}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSScanningManModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        keyValues = readTask.Result.ToDictionary(p => p.IDTracking);
                        edocsITSScanning = keyValues.Values.ElementAt(0);
                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return edocsITSScanning;
        }


        public static async Task<EdocsITSCustomersModel> GetEdocsCustomer(IHttpClientFactory clientFactory, Uri uri, string controller, string customerID, string sProcedure)
        {



            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}/{sProcedure}/{customerID}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSCustomersModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        // EdocsITSCustomersModel edocsITSCustomers = readTask.Result[0];// as EdocsITSCustomersModel;
                        // r/eturn edocsITSCustomers;
                        return readTask.Result[0];
                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not Get Edocs Customer id {customerID}");
            }
            return null;
        }

        public static async Task<EdocsITSCustomersModel> GetEdocsCustomerByName(IHttpClientFactory clientFactory, Uri uri, string controller, string custName, string sProcedure)
        {



            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}/{custName}/{sProcedure}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSCustomersModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        // EdocsITSCustomersModel edocsITSCustomers = readTask.Result[0];// as EdocsITSCustomersModel;
                        // r/eturn edocsITSCustomers;
                        return readTask.Result[0];
                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not Get Edocs Customer id {custName}");
            }
            return null;
        }

        public static async Task<InvoiceNum> CreateInvoiceNumber(IHttpClientFactory clientFactory, Uri uri, string controller, InvoiceModel invoice)
        {


            InvoiceNum iNum = new InvoiceNum();
            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(invoice), Encoding.UTF8, "application/json");

                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<InvoiceNum[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        // EdocsITSCustomersModel edocsITSCustomers = readTask.Result[0];// as EdocsITSCustomersModel;
                        // r/eturn edocsITSCustomers;
                        iNum.NumberInvoice = readTask.Result[0].NumberInvoice;
                        return iNum;
                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not Create invoice for Edocs Customer id {invoice.EdocsCustomerID}");
            }
            return null;
        }

        public static async Task<IList<EdocsITCustomerIDNameModel>> GetITSCustomers(IHttpClientFactory clientFactory, Uri uri, string controller, string storedProcedure)
        {


            IList<EdocsITCustomerIDNameModel> retList = new List<EdocsITCustomerIDNameModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}/{storedProcedure}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();

                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITCustomerIDNameModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        retList = readTask.Result.ToList();

                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cold not Get Customers {ex.Message}");
            }
            return retList;
        }

        public static async Task<IList<AcceptRejectDocumentsModel>> GetPdfDocuments(IHttpClientFactory clientFactory, Uri uri, string controller, int custID)
        {


            IList<AcceptRejectDocumentsModel> retList = new List<AcceptRejectDocumentsModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}/{custID}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();

                    var readTask = httpResponse.Content.ReadAsAsync<AcceptRejectDocumentsModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        retList = readTask.Result.ToList();

                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cold not Get Customers {ex.Message}");
            }
            return retList;
        }

        public static async Task<IList<AcceptRejectDocumentsModel>> GetPdfDocuments(IHttpClientFactory clientFactory, Uri uri, string controller, int custID, string trackingID, string repType, string repSDate, string repEDate)
        {


            IList<AcceptRejectDocumentsModel> retList = new List<AcceptRejectDocumentsModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}/{custID}/{trackingID}/{repType}/{repSDate}/{repEDate}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();

                    var readTask = httpResponse.Content.ReadAsAsync<AcceptRejectDocumentsModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        retList = readTask.Result.ToList();

                    }
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cold not Get Customers {ex.Message}");
            }
            return retList;
        }
        public static async Task UpdateTaskID(IHttpClientFactory clientFactory, Uri uri, string controller, EdocsITSScanningManModel edocsITSScanning)
        {



            try
            {
                var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(edocsITSScanning), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                    //httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        //var readTask = httpResponse.Content.ReadAsAsync<RetMess[]>();
                        //readTask.Wait();
                        //RetMess retStr = readTask.Result[0] as RetMess;
                    }

                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    //if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    // {
                    //   Console.WriteLine();

                    // }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not Update Tracking ID {edocsITSScanning.TrackingID} {ex.Message}");
            }

        }

        public static async Task<IDictionary<string, ITSTrackingIDModel>> ReportTrackingIDsByTrackingID(IHttpClientFactory clientFactory, Uri uri, string controller, string trackingID, string spName)
        {
            IDictionary<string, ITSTrackingIDModel>  idsTracking = new Dictionary<string, ITSTrackingIDModel>();
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {
                
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    string reqUri = $"{controller}{trackingID}/{spName}";
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //    httpResponse.EnsureSuccessStatusCode();
                    var str = httpResponse.Content.ReadAsStringAsync();
                    str.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<ITSTrackingIDModel[]>();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (readTask.Result == null)
                            return null;
                        idsTracking = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // throw new Exception($"Cound not transfer inventory tracking id {edocsITSInventory.TrackingID} {ex.Message}");
            }
            return idsTracking;
        }

        public static async Task<IList<TrackingIDProjectNameModel>> GetTrackingIDsDocNamesAcceptReject(IHttpClientFactory clientFactory, Uri uri, string controller, string custNumber, string sDate, string eDate,string trackingID,string reptype)
        {
            IList<TrackingIDProjectNameModel> trackingIDProjects = new List<TrackingIDProjectNameModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{custNumber}/{trackingID}/{reptype}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        trackingIDProjects = JsonConvert.DeserializeObject<List<TrackingIDProjectNameModel>>(readTask.Result);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {custNumber} {ex.Message}");
            }
            return trackingIDProjects;
        }

        public static async Task<List<object>> GetPSUSDLookRecordsInfo<T>(IHttpClientFactory clientFactory, Uri uri, string controller,string spName,string searchFor,string repType,DateTime stDate,DateTime endDate) 
        {
            List<object> records = new List<object>();
            try
            {
                string sDate = stDate.ToString("yyyy-MM-dd");
                string eDate = endDate.ToString("yyyy-MM-dd");
              
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{spName}/{searchFor}/{repType}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                       
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        records = JsonConvert.DeserializeObject<List<object>>(readTask.Result);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {searchFor} {ex.Message}");
            }
            return records;
        }
        public static async Task<List<PSUSDDateOfRecordsModel>> GetPSUSDLookRecordsDateRange(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
        {
            List<PSUSDDateOfRecordsModel> records = new List<PSUSDDateOfRecordsModel>();
            try
            {
                string sDate = stDate.ToString("yyyy-MM-dd");
                string eDate = endDate.ToString("yyyy-MM-dd");

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{spName}/{searchFor}/{repType}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        records = JsonConvert.DeserializeObject<List<PSUSDDateOfRecordsModel>>(readTask.Result);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {searchFor} {ex.Message}");
            }
            return records;
        }
        public static async Task<List<PSUSDDepartmentModel>> GetPSUSDLookRecordsDept(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
        {
            List<PSUSDDepartmentModel> records = new List<PSUSDDepartmentModel>();
            try
            {
                string sDate = stDate.ToString("yyyy-MM-dd");
                string eDate = endDate.ToString("yyyy-MM-dd");

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{spName}/{searchFor}/{repType}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        records = JsonConvert.DeserializeObject<List<PSUSDDepartmentModel>>(readTask.Result);
                        if((records != null) && (records.Count > 0))
                        {
                            PSUSDDepartmentModel pSUSD = new PSUSDDepartmentModel();
                            pSUSD.Department = "Select Department";
                            records.Insert(0,pSUSD);
                        }
                            
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {searchFor} {ex.Message}");
            }
            return records;
        }
        public static async Task<List<PSUSDOrginationDepartmentModel>> GetPSUSDLookRecordsOrginationDepartment(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
        {
            List<PSUSDOrginationDepartmentModel> records = new List<PSUSDOrginationDepartmentModel>();
            try
            {
                string sDate = stDate.ToString("yyyy-MM-dd");
                string eDate = endDate.ToString("yyyy-MM-dd");

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{spName}/{searchFor}/{repType}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        records = JsonConvert.DeserializeObject<List<PSUSDOrginationDepartmentModel>>(readTask.Result);
                        if ((records != null) && (records.Count > 0))
                        {
                            PSUSDOrginationDepartmentModel pSUSD = new PSUSDOrginationDepartmentModel();
                            pSUSD.OrginationDepartment = "Select Orgination Department";
                            records.Insert(0, pSUSD);
                        }

                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {searchFor} {ex.Message}");
            }
            return records;
        }
        public static async Task<List<PSUDFirstNameModel>> GetPSUSDLookRecordsFirstName(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
        {
            List<PSUDFirstNameModel> records = new List<PSUDFirstNameModel>();
            try
            {
                string sDate = stDate.ToString("yyyy-MM-dd");
                string eDate = endDate.ToString("yyyy-MM-dd");

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{spName}/{searchFor}/{repType}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                         records = JsonConvert.DeserializeObject<List<PSUDFirstNameModel>>(readTask.Result);
                        //PSUDFirstNameModel records1 = new PSUDFirstNameModel();
                        //records1.FirstName = "All";
                        //records.Insert(0, records1);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {searchFor} {ex.Message}");
            }
            return records;
        }
        public static async Task<List<PSUDLastNameModel>> GetPSUSDLookRecordsLastName(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
        {
            List<PSUDLastNameModel> records = new List<PSUDLastNameModel>();
            try
            {
                string sDate = stDate.ToString("yyyy-MM-dd");
                string eDate = endDate.ToString("yyyy-MM-dd");

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{spName}/{searchFor}/{repType}/{sDate}/{eDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        records = JsonConvert.DeserializeObject<List<PSUDLastNameModel>>(readTask.Result);


                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {searchFor} {ex.Message}");
            }
            return records;
        }
        public static async Task<IDictionary<int,PSUSDRecsordsModel>> PSUSDRecordsFLName(IHttpClientFactory clientFactory, Uri uri, string controller, string custNumber)
        {
            IDictionary<int, PSUSDRecsordsModel> dic = new Dictionary<int, PSUSDRecsordsModel>();
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{custNumber}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<PSUSDRecsordsModel[]>();
                        
                      //  var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        dic = readTask.Result.ToDictionary(p => p.ID);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {custNumber} {ex.Message}");
            }
            return dic;
        }
        public static async Task<IDictionary<int, PSUSDRecsordsModel>> PSUSDRecordsFullText(IHttpClientFactory clientFactory, Uri uri, string controller, PSUSDFullTextModel fText)
        {
            IDictionary<int, PSUSDRecsordsModel> dic = new Dictionary<int, PSUSDRecsordsModel>();
            var newClientJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(fText), Encoding.UTF8, "application/json");
            try
            {

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    using var httpResponse = httpClient.PostAsync(controller, newClientJson).ConfigureAwait(false).GetAwaiter().GetResult();
                   
                     
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var readTask = httpResponse.Content.ReadAsAsync<PSUSDRecsordsModel[]>();

                        //  var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        dic = readTask.Result.ToDictionary(p => p.ID);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {fText.SearchText} {ex.Message}");
            }
            return dic;
        }
        public static async Task<IDictionary<int, PSUSDRecsordsModel>> PSUSDRecordsFLName(IHttpClientFactory clientFactory, Uri uri, string controller, string fName,string lName,string sDate,string endDate)
        {
            IDictionary<int, PSUSDRecsordsModel> dic = new Dictionary<int, PSUSDRecsordsModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(fName))
                    fName = "NA";
                if (string.IsNullOrWhiteSpace(lName))
                    lName = "NA";

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{sDate}/{endDate}/{fName}/{lName}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<PSUSDRecsordsModel[]>();

                        //  var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        dic = readTask.Result.ToDictionary(p => p.ID);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {fName} {ex.Message}");
            }
            return dic;
        }
        public static async Task<IDictionary<int, PSUSDRecsordsModel>> PSUSDRecordsDept(IHttpClientFactory clientFactory, Uri uri, string controller, string fName, string lName, string sDate, string endDate,string dept,string orgDept)
        {
            IDictionary<int, PSUSDRecsordsModel> dic = new Dictionary<int, PSUSDRecsordsModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(fName))
                    fName = "NA";
                if (string.IsNullOrWhiteSpace(lName))
                    lName = "NA";

                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(120);
                    var responseTask = httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}{controller}{sDate}/{endDate}/{fName}/{lName}/{dept}/{orgDept}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<PSUSDRecsordsModel[]>();

                        //  var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        dic = readTask.Result.ToDictionary(p => p.ID);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);

                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get trackingids project names for customer {fName} {ex.Message}");
            }
            return dic;
        }

        public static async Task AddInvoiceNumber(IHttpClientFactory clientFactory, Uri uri, string controller, AddInvoiceNumberModel numberModel)
        {

            try
            {


                var newInvNum = new StringContent(System.Text.Json.JsonSerializer.Serialize(numberModel), Encoding.UTF8, "application/json");
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    using var httpResponse = httpClient.PostAsync(controller, newInvNum).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    // string   s = JsonSerializer.DeserializeAsync<string>(responseStream).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    string responseBody = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult(); ;
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Cound not add new invoice number {numberModel.InvoiceNum} for customer {numberModel.EdocsCustomerID}");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cound not add new invoice number {numberModel.InvoiceNum} for customer {numberModel.EdocsCustomerID} {ex.Message}");
            }
        }

    }
}
