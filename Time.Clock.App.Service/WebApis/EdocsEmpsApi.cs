using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Edocs.Employees.Time.Clock.App.Service.Constants;
using Edocs.Employees.Time.Clock.App.Service.Models;
using System.Net.Http;
using System.Xml.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Edocs.Employees.Time.Clock.App.Service.WebApis
{
    public class EdocsEmpsApi
    {
        public static async Task<string> ClockInOutEmp(string empid,bool isAdmin, Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            string clockInOutModel = string.Empty;
            string reqUri = $"{controller}{empid}/{isAdmin}";
            try
            {
                


            using (var httpClient = clientFactory.CreateClient())
            {

                httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                //   httpResponse.EnsureSuccessStatusCode();
                var readTask = httpResponse.Content.ReadAsStringAsync();
                readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MessageModel[] s = JsonConvert.DeserializeObject<MessageModel[]>(readTask.Result);
                        clockInOutModel = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                    else
                        throw new Exception($"Http return code {httpResponse.StatusCode}");
            }
            }
            catch(Exception ex)
            {
                clockInOutModel = $"{TimeClockConst.ErrorMessage}Logginn in {empid} {ex.Message}";
            }

            return clockInOutModel;
        }
        public static async Task<IList<TimeClockEntriesModel>> ClockEmpHrs(string empid,DateTime timeClockStartDateTime,DateTime timeClockEndDateTime,string spName, Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            IList<TimeClockEntriesModel> tcEntries = new List<TimeClockEntriesModel>();
            string reqUri = $"{controller}{empid}/{timeClockStartDateTime.ToString("yyyy-MM-dd")}/{timeClockEndDateTime.ToString("yyyy-MM-dd")}/{spName}";
            try
            {


                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        tcEntries = JsonConvert.DeserializeObject<IList<TimeClockEntriesModel>>(readTask.Result);//.ToDictionary(p=>p.Key,p1=>p1.Value);
                        //keyValues = JsonConvert.Deserialize<KeyValuePair<TKey, TValue>[]>(readTask.Result);
                        //keyValues = JsonConvert.to<IDictionary<int, TimeClockEntriesModel>>(readTask.Result);
                        //MessageModel[] s = JsonConvert.DeserializeObject<MessageModel[]>(readTask.Result);
                        //clockInOutModel = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tcEntries;
        }
        public static async Task UpdateTimeClockEmpHrs(string empid, DateTime timeClockStartDateTime, DateTime timeClockEndDateTime, string spName, Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            string reqUri = $"{controller}{empid}/{timeClockStartDateTime.ToString("yyyy-MM-dd HH:mm:ss")}/{timeClockEndDateTime.ToString("yyyy-MM-dd HH:mm:ss")}/{spName}";
            try
            {


                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                       //cEntries = JsonConvert.DeserializeObject<IList<TimeClockEntriesModel>>(readTask.Result);//.ToDictionary(p=>p.Key,p1=>p1.Value);
                        //keyValues = JsonConvert.Deserialize<KeyValuePair<TKey, TValue>[]>(readTask.Result);
                        //keyValues = JsonConvert.to<IDictionary<int, TimeClockEntriesModel>>(readTask.Result);
                        //MessageModel[] s = JsonConvert.DeserializeObject<MessageModel[]>(readTask.Result);
                        //clockInOutModel = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

             
        }
        public static async Task<string> TimeClockLogin(TimeClockAdminLogin timeClockAdmin, Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            string login = string.Empty;
            string reqUri = $"{controller}{timeClockAdmin.EmpID}/{timeClockAdmin.EmpPW}";
            try
            {


                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MessageModel[] s = JsonConvert.DeserializeObject<MessageModel[]>(readTask.Result);
                        login = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return login;
        }
        public static async Task<string> DelTimeClockEntry(int ID, Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            string login = string.Empty;
            string reqUri = $"{controller}{ID}";
            try
            {


                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    using var httpResponse = httpClient.DeleteAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MessageModel[] s = JsonConvert.DeserializeObject<MessageModel[]>(readTask.Result);
                        login = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return login;
        }

        public static async Task  AddEditTimeClockUsers(Uri uri, string controller, IHttpClientFactory clientFactory,EmpModel empModel)
        {
          //  EmpClockInOutModel clockInOutModel = null;
         //   string reqUri = $"{controller}/{uri}";
            using (var httpClient = clientFactory.CreateClient())
            {

                httpClient.BaseAddress = uri;
                var jsonString = new StringContent(System.Text.Json.JsonSerializer.Serialize(empModel), Encoding.UTF8, "application/json");
                //JsonConvert.SerializeObject(empModel);
                //var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
           
                  var httpResponse = httpClient.PostAsync(controller, jsonString).ConfigureAwait(false).GetAwaiter().GetResult();
               
                //   httpResponse.EnsureSuccessStatusCode();
                var readTask = httpResponse.Content.ReadAsStringAsync();
                readTask.Wait();
                httpResponse.Dispose();
                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Error adding edit timeclock emp");
                    //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                }
            }

             
        }
        public static async Task<IList<EmpIDModel>> TimeClockLoginEmpID(Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            string login = string.Empty;
            IList<EmpIDModel> empIds = new List<EmpIDModel>();
            string reqUri = $"{controller}";
            try
            {


                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    using var httpResponse = httpClient.GetAsync(reqUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsStringAsync();
                    readTask.Wait();
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        empIds = JsonConvert.DeserializeObject<IList<EmpIDModel>>(readTask.Result);
                        //login = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return empIds;
        }
        public static async Task<string> TimeClockAddNewEntry(TimeClockEntriesModel empClockInOut, Uri uri, string controller, IHttpClientFactory clientFactory)
        {
            string clockInOutModel = string.Empty;
             
            try
            {



                using (var httpClient = clientFactory.CreateClient())
                {

                    httpClient.BaseAddress = uri;
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    httpClient.BaseAddress = uri;
                    var jsonString = new StringContent(System.Text.Json.JsonSerializer.Serialize(empClockInOut), Encoding.UTF8, "application/json");
                    using var httpResponse = httpClient.PostAsync(controller,jsonString).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   httpResponse.EnsureSuccessStatusCode();
                    var readTask = httpResponse.Content.ReadAsStringAsync();
                    readTask.Wait();
                    
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MessageModel[] s = JsonConvert.DeserializeObject<MessageModel[]>(readTask.Result);
                        clockInOutModel = s[0].ClockInOutMessage;
                        //   edocsITSInventory = readTask.Result.ToDictionary(p => p.TrackingID);
                        if (clockInOutModel.ToLower().StartsWith(TimeClockConst.ErrorMessage.ToLower()))
                            throw new Exception(clockInOutModel);

                    }
                    else
                        throw new Exception($"Http return code {httpResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                clockInOutModel = $"{TimeClockConst.ErrorMessage} adding new time entry for empid;{empClockInOut.EmpID} {ex.Message}";
                throw new Exception($"{clockInOutModel}");
            }

            return clockInOutModel;
        }
    }
}
