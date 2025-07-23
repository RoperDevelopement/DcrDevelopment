using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using Edocs.ITS.AppService.Models;

namespace Edocs.ITS.AppService.ApisConst
{
    public class RetMess
    {
        public string ReturnMessage
        { get; set; }
    }
    public class EdocsITSUsersApi
    {
        private static EdocsITSUsersApi instance = null;

        private EdocsITSUsersApi() { }

        public static EdocsITSUsersApi UsersInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EdocsITSUsersApi();
                }
                return instance;
            }
        }
        public async Task<string> LoginUser(LoginModel login, string controllerName, Uri webAPiUri, string spName)
        {
            try
            { 
                using (var client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(login);

                client.BaseAddress = webAPiUri;
                client.Timeout = TimeSpan.FromMinutes(10);
                //spName = $"storedProcedure=sp_UserLogin";
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                //controllerName = $"{controllerName}{spName}";
                var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}{spName}", content);
                //var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}",content);
                responseTask.Wait();
                var results = responseTask.Result;
                content.Dispose();
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<RetMess[]>();
                    readTask.Wait();
                    RetMess retStr = readTask.Result[0] as RetMess;
                    return retStr.ReturnMessage;
                }
                else
                    throw new Exception($"Method LoginUser weburl: {webAPiUri} controller: {controllerName} results code: {results.StatusCode}");
            }
            }
            catch(Exception ex)
            {
                return ($"error: {ex.Message}"); 
               // Console.Write(ex.Message);
            }
            

        }
        public async Task<string> AddNewUser(EdocsITSUsersModel iTSUsersNamesModel, string controllerName, Uri webAPiUri, string spName)
        {
            using (var client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(iTSUsersNamesModel);

                client.BaseAddress = webAPiUri;
                client.Timeout = TimeSpan.FromMinutes(10);
                //spName = $"storedProcedure=sp_UserLogin";
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                //controllerName = $"{controllerName}{spName}";
                var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}{spName}", content);
                //var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}",content);
                responseTask.Wait();
                var results = responseTask.Result;
                content.Dispose();
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<RetMess[]>();
                    readTask.Wait();
                    RetMess retStr = readTask.Result[0] as RetMess;
                    return retStr.ReturnMessage;
                }
                else
                    throw new Exception($"Method LoginUser weburl: {webAPiUri} controller: {controllerName} results code: {results.StatusCode}");
            }



        }
        public async Task<string> UpdateUserProfile(EdocsITSUsersModel iTSUsersNamesModel, string controllerName, Uri webAPiUri, string spName)
        {
            using (var client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(iTSUsersNamesModel);

                client.BaseAddress = webAPiUri;
                client.Timeout = TimeSpan.FromMinutes(10);
                //spName = $"storedProcedure=sp_UserLogin";
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                //controllerName = $"{controllerName}{spName}";
               // var responseTask = client.PutAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}{spName}", content);
                var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controllerName}",content);
                responseTask.Wait();
                var results = responseTask.Result;
                content.Dispose();
                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsAsync<RetMess[]>();
                    readTask.Wait();
                    RetMess retStr = readTask.Result[0] as RetMess;
                    return retStr.ReturnMessage;
                }
                else
                    throw new Exception($"Method LoginUser weburl: {webAPiUri} controller: {controllerName} results code: {results.StatusCode}");
            }



        }
        public async Task UpDateNextMFLA(string spName, string userLoginId, Uri webAPiUri, int nextMFLA)
        {
            using (var client = new HttpClient())
            {


                client.BaseAddress = webAPiUri;
                client.Timeout = TimeSpan.FromMinutes(10);
                //spName = $"storedProcedure=sp_UserLogin";
                string postParams = $"{userLoginId}/{nextMFLA}/{spName}";
                var content = new StringContent(postParams, Encoding.UTF8, "text/plain");
                //controllerName = $"{controllerName}{spName}";
                //HttpContent _Body = new StringContent(postParams);
                //_Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
                var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}{postParams}");
                //var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}",content);
                responseTask.Wait();
                var results = responseTask.Result;

                if (results.IsSuccessStatusCode)
                {
                    var readTask = results.Content.ReadAsStringAsync();
                    readTask.Wait();


                }
                else
                    throw new Exception($"Method UpDateSpecMonSettings weburl: {webAPiUri}  results code: {results.StatusCode}");
            }



        }
        public static async Task<IDictionary<int, EdocsITSUsersNamesModel>> GetUserNames(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string edocsCusUserNameID)
        {

            IDictionary<int, EdocsITSUsersNamesModel> keyValues = new Dictionary<int, EdocsITSUsersNamesModel>();


            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}{edocsCusUserNameID}/{spName}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSUsersNamesModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        keyValues = readTask.Result.ToDictionary(p => p.UserID);

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
            return keyValues;
        }

        public static async Task<IDictionary<int, EdocsITSUsersModel>> GetAllUserNames(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string edocsCusUserNameID)
        {

            IDictionary<int, EdocsITSUsersModel> keyValues = new Dictionary<int, EdocsITSUsersModel>();


            try
            {
                using (var httpClient = clientFactory.CreateClient())
                {
                    httpClient.BaseAddress = uri;
                    controller = $"{controller}{edocsCusUserNameID}/{spName}";
                    using var httpResponse = httpClient.GetAsync(controller).ConfigureAwait(false).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    var s = httpResponse.Content.ReadAsStringAsync();
                    s.Wait();
                    var readTask = httpResponse.Content.ReadAsAsync<EdocsITSUsersModel[]>();
                    readTask.Wait();
                    if (readTask.Result.Length > 0)
                    {
                        keyValues = readTask.Result.ToDictionary(p => p.UserID);

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
            return keyValues;
        }
    }
}
