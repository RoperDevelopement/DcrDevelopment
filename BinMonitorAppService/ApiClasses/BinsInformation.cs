using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using BinMonitorAppService.Models;
using Newtonsoft;
using Newtonsoft.Json;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using BinMonitorAppService.Logging;

namespace BinMonitorAppService.ApiClasses
{
    public class BinsInformation
    {
        public readonly string RegXNonDigitChar = "[^0-9]";
        public readonly string RegXNumsLetters = "[a-zA-Z0-9]*$";
        public readonly string RegXLowewrCase = "(.*[a-z].*)";
        public readonly string RegXLowewrUpperCase = "(.*[A-Z].*)";
        public readonly string RegXLoweDigits = @"(.*\d.*)";
        public readonly int PwMinLength = 8;


         
        private static BinsInformation instance = null;
        public static BinsInformation BinsApisInctance
        {
            get
            {
                if (instance == null)
                    instance = new BinsInformation();
                return instance;
            }
        }
        private BinsInformation()
        {
        }

        public async Task<IDictionary<string, SpecMonitorUserProfileRights>> ApiGetUserRightsModel(string apiUrl, string sp,ILog log)
        {
            IDictionary<string, SpecMonitorUserProfileRights> valuePairs =new Dictionary<string, SpecMonitorUserProfileRights>();
            
            try
            {
                log.LogInformation($"Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    var responseTask = client.GetAsync($"{sp}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<SpecMonitorUserProfileRights[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {

                            valuePairs = readTask.Result.ToDictionary(p => p.BinUserights);
                        }
                    }
                    else
                    {

                        throw new Exception($"Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} results code: {results.StatusCode}");
                    }
                }
            }

            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception  Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} {ex.Message}");
                throw new Exception($"Exception Method ApiGetUserRightsModel weburl: {apiUrl} controller: {sp} {ex.Message}");
            }
            return valuePairs;
        }
        public string GetLogginhPath(string path)
        {
            int indexPath = path.IndexOf(":");
            if (indexPath > 0)
                path = path.Substring(++indexPath);
                    path =path.Replace("\\", "/");
            if (!(path.EndsWith("/")))
               path= $"{path}/";
            path = $"{path}AuditLogs/";
            return path;
        }
        public async Task<List<string>> ApiGetActiveBinsModel(string apiUrl, string sp,ILog log)
        {
            List<string> binRegPro = new List<string>();
            try
            {

                log.LogInformation($"Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    var responseTask = client.GetAsync($"{sp}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} results stats code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<BinRegProcessModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                        {
                            binRegPro = readTask.Result.Select(p => p.LabRecNumber).ToList();
                        }
                    }
                    else
                    {

                        throw new Exception($"Return status code :{results.StatusCode.ToString()} for url {apiUrl} for sp:{sp}");
                    }
                }
            }

            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} {httpex.Message}");
                throw new Exception($"Metod  HttpRequestException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} {ex.Message}");
                throw new Exception($"Exception Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {sp} {ex.Message}");
            }
            return binRegPro;
        }
        public async Task<string> CategoyColor(string catName, DateTime stTime, IDictionary<string, CategoryCheckPointModel> checkPointModel)
        {

            if (checkPointModel.TryGetValue(catName, out CategoryCheckPointModel value))
            {
                double totalTime = GetTotalDurationHrs(stTime).Result;
                double catCheckPoint = ConvertToDouble(value.CategoryCheckPointOneDuration).Result;
                if (totalTime <= catCheckPoint)
                    return value.CategoryColorCheckPointOne;
                catCheckPoint = ConvertToDouble(value.CategoryCheckPointTwoDuration).Result;
                if (totalTime <= catCheckPoint)
                    return value.CategoryColorCheckPointTwo;
                else
                    return value.CategoryColorCheckPointThree;
            }

            return "#FFFFFF";
        }
        public async Task<double> GetTotalDurationHrs(DateTime stTime)
        {
            TimeSpan ts = DateTime.Now - stTime;
            return ts.TotalHours;
        }
        public async Task<double> ConvertToDouble(string strDouble)
        {
            if (double.TryParse(strDouble, out double results))
                return results;
            return 1.0;
        }

        public async Task<string> ReplaceStr(string strToReplace, string repStr, string newStr)
        {
            if (string.IsNullOrWhiteSpace(strToReplace))
                return strToReplace;
            if (string.IsNullOrWhiteSpace(repStr))
                return strToReplace;
            return strToReplace.Replace(repStr, newStr).Trim();
        }
        public async Task<string> LoginUser(string apiUrl, string controller, ILog log)
        {
            string retStr = string.Empty;
            try
            {
                log.LogInformation($"Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    var responseTask = client.GetAsync($"{controller}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        string[] rest = readTask.Result.Split(new char[] { ':' });

                        if (rest.Length == 2)
                        {

                            retStr = ReplaceStr(rest[1], "[", "").Result;
                            retStr = ReplaceStr(retStr, "{", "").Result;
                            retStr = ReplaceStr(retStr, "}", "").Result;
                            retStr = ReplaceStr(retStr, "]", "").Result;
                            retStr = ReplaceStr(retStr, SqlConstants.RepDoubleQuots, "").Result;
                            retStr = ReplaceStr(retStr, SqlConstants.RepDoubleSingleQuots, "").Result;
                        }
                    }
                    else
                        throw new Exception($"Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} {ex.Message}");
                throw new Exception($"Error: Exception Method ApiGetActiveBinsModel weburl: {apiUrl} controller: {controller} {ex.Message}");
            }
            return retStr;
        }

        public async Task<string> UpDateUserInfo(UsersModel users, string apiUrl, string controller,ILog log)
        {
            string retStr = string.Empty;
            try
            {
                log.LogInformation($"Method UpDateUserInfo weburl: {apiUrl} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
                    HttpResponseMessage message = client.GetAsync(apiUrl).Result;
                    var jsonString = JsonConvert.SerializeObject(users);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Method UpDateUserInfo weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        string[] rest = readTask.Result.Split(new char[] { ':' });

                        if (rest.Length == 2)
                        {

                            retStr = ReplaceStr(rest[1], "[", "").Result;
                            retStr = ReplaceStr(retStr, "{", "").Result;
                            retStr = ReplaceStr(retStr, "}", "").Result;
                            retStr = ReplaceStr(retStr, "]", "").Result;
                            retStr = ReplaceStr(retStr, SqlConstants.RepDoubleQuots, "").Result;
                            retStr = ReplaceStr(retStr, SqlConstants.RepDoubleSingleQuots, "").Result;
                        }
                    }
                    else
                        throw new Exception($"Method UpDateUserInfo weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                retStr = $"Error loging in {ex.Message}";
            }
            return retStr;
        }

        public async Task UpDateSpecMonSettings(SpectrumMonitorSettings monitorSettings, string apiUrl, string controller,ILog log)
        {
            string retStr = string.Empty;
            try
            {
                log.LogInformation($"Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
                   // HttpResponseMessage message = client.pu(apiUrl).Result;
                    var jsonString = JsonConvert.SerializeObject(monitorSettings);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    var results = responseTask.Result;
                    log.LogInformation($"Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    content.Dispose();
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                    }
                    else
                        throw new Exception($"Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {httpex.Message}");
                throw new Exception($"HttpRequestException  Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ag.Message}");
                throw new Exception($"ArgumentNullException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ex.Message}");
                throw new Exception($"Exception Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ex.Message}");
            }

        }
        public async Task<string> GetQueryString(string qString)
        {
            return qString.Substring(qString.IndexOf("=") + 1);

        }
        public async Task ApiUpdateUserInfo(UsersModel usersModel, string apiUrl,string controller,ILog log)
        {
            try
            {
                log.LogInformation($"Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller}");
            
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{apiUrl}");
                var jsonString = JsonConvert.SerializeObject(usersModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                responseTask.Wait();
                var results = responseTask.Result;
                    log.LogInformation($"Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} results code: { results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                {
                    throw new Exception($"Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} results code: { results.StatusCode}");
                    }
                
            }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {httpex.Message}");
                throw new Exception($"HttpRequestException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ag.Message}");
                throw new Exception($"ArgumentNullException Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller}  {ex.Message}");
                throw new Exception($"Exception Method UpDateSpecMonSettings weburl: {apiUrl} controller: {controller} {ex.Message}");
            }
        }

        public async Task ApiUpdateCreateProfile(UpdateProfilesModel profilesModel, string apiUrl, string controller,ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}");
            
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{apiUrl}");
                var jsonString = JsonConvert.SerializeObject(profilesModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                responseTask.Wait();
                var results = responseTask.Result;
                    log.LogInformation($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                {
                    throw new Exception($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    }

            }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}  {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}  {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}  {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}   {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}  {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {controller}  {ex.Message}");
            }
        }

        public async Task DelSpecMonitorUser(string sp, string apiUrl,ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp}");
            
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri($"{apiUrl}");
                var responseTask = client.DeleteAsync($"{client.BaseAddress.AbsoluteUri}{sp}");
                responseTask.Wait();
                var results = responseTask.Result;
                    log.LogInformation($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} results code: { results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                {
                    throw new Exception($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} results code: { results.StatusCode}");
                    }

            }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ex.Message}");
            }
        }

        public async Task<List<string>> ApiGetSpectrumMonitorCwid(string apiUrl, string sp,ILog log)
        {
            List<string> cwid = new List<string>();


            try
            {
                log.LogInformation($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    var responseTask = client.GetAsync($"{sp}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} results code: { results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsAsync<CwidModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                            cwid = readTask.Result.Select(p => p.Cwid).ToList();

                        else
                            throw new Exception($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} results code: { results.StatusCode}");

                    }
                    else
                    {

                        throw new Exception($"Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} results code: { results.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ex.Message}");
                throw new Exception($"Exception Method ApiUpdateCreateProfile weburl: {apiUrl} controller: {sp} {ex.Message}");
            }
            return cwid;
        }

        public async Task<List<string>> ApiGetSpectrumMonitorUserRights(string apiUrl, string sp,ILog log)
        {
            List<string> userRights = new List<string>();


            try
            {
                log.LogInformation($"Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    var responseTask = client.GetAsync($"{sp}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {

                        var readTask = results.Content.ReadAsAsync<SpectrumMonitorBinUsersProfile[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                            userRights = readTask.Result.Select(p => p.SpecMonitorRights).ToList();

                        else
                            throw new Exception($"Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} results code: {results.StatusCode}"); 

                    }
                    else
                    {

                        throw new Exception($"Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} results code: {results.StatusCode}"); 
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} {ag.Message}");
                throw new Exception($"ArgumentNullException  Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp}  {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp}  {ex.Message}");
                throw new Exception($"Exception Method ApiGetSpectrumMonitorUserRights weburl: {apiUrl} controller: {sp} {ex.Message}");
            }
            return userRights;
        }

        public bool CheckRegxString(string inStr, string regX, bool showMessageBox)
        {
            if (!(Regex.Match(inStr, regX).Success))
            {
                
                return false;
            }

            return true;
        }
        public string ChekcPasword(string pw)

        {
            if(pw.Length < PwMinLength)
            {
                return ($"Password min length {PwMinLength}");
            }
            if (!(CheckRegxString(pw,RegXNonDigitChar, false)))
            {
                return ("Invalid password must contain one non digit");

            }
            if (!(CheckRegxString(pw,RegXLowewrCase, false)))
            {

                return ("Invalid password must contain one lowercase  letter ");
            }
            if (!(CheckRegxString(pw,RegXLowewrUpperCase, false)))
            {
                return "Invalid password must contain one upercase letter";

            }
            if (!(CheckRegxString(pw,RegXNumsLetters, false)))
            {
                return("Invalid password must contain one digit ");
                

            }
            return string.Empty;
        }
        public async Task UpDateRegStatusByBinId(string apiUrl, string controller, BinLabRecModel recModel,ILog log)
        {
            try
            {
                log.LogInformation($"Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{apiUrl}");
                    var jsonString = JsonConvert.SerializeObject(recModel);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} results code: {results.StatusCode}");
                    }

                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} {httpex.Message}");
                throw new Exception($"HttpRequestException Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} {ag.Message}");
                throw new Exception($"ArgumentNullException Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller}  {ex.Message}");
                throw new Exception($"Exception Method UpDateRegStatusByBinId weburl: {apiUrl} controller: {controller} {ex.Message}");
            }
        }

        public async Task TransFer(TransFerModel transFer, string webApiUri, string webApiControllerName,ILog log)
        {
            

            try
            {
                log.LogInformation($"Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName}");
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(webApiUri);
                    var jsonString = JsonConvert.SerializeObject(transFer);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{webApiControllerName}", content);
                    responseTask.Wait();

                    //  var result = await client.PostAsync("Method Address", content);
                    // string resultContent = await result.Content.ReadAsStringAsync();
                    var results = responseTask.Result;
                    results.EnsureSuccessStatusCode();
                    var httpResponseBody = await results.Content.ReadAsStringAsync();
                    content.Dispose();
                    log.LogInformation($"Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} results code: {results.StatusCode}");
                    }
                    
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} {httpex.Message}");
                throw new Exception($"HttpRequestException Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} {ag.Message}");
                throw new Exception($"ArgumentNullException  Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} {ex.Message}");
                throw new Exception($"Exception Method UpDateRegStatusByBinId weburl: {webApiUri} controller: {webApiControllerName} {ex.Message}");
            }
        }

    }






}
