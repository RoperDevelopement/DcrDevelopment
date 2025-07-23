using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
namespace Edocs.Service.UploadAuditLogs
{
   public class AuditLogsWebApi
    {
        static AuditLogsWebApi instance = null;
        public static AuditLogsWebApi ALInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuditLogsWebApi();
                }
                return instance;
            }
        }

        private AuditLogsWebApi()
        {
        }

        public async Task UpLoadAuditLogsDataBase(Uri apiUrl, string controller, AuditLogsModel auditLogs)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = apiUrl;
                    var jsonString = JsonConvert.SerializeObject(auditLogs);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{controller}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (!(results.IsSuccessStatusCode))
                        throw new Exception($"Status code returned {results.StatusCode} message {results.RequestMessage}");

                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }


    }
}
