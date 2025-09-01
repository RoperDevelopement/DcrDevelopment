using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BMRMobileApp
{
    public class AppSettings
    {
       // [JsonPropertyName("ApiUrl")]
       public string ApiUrl { get; set; }
        //[JsonPropertyName("AppName")]
        public string AppName { get; set; }
        //[JsonPropertyName("SmtpMailOutlookCom")]
        public string SmtpMailOutlookCom { get; set; }
        //[JsonPropertyName("SmtpMailPortOutlookCom")]
        public string SmtpMailPortOutlookCom { get; set; }
        //[JsonPropertyName("PassWordOutlookCom")]
        public string PassWordOutlookCom { get; set; }
        //[JsonPropertyName("EmailFrom")]
        public string EmailFrom { get; set; }
        //[JsonPropertyName("EmailSubject")]
        public string EmailSubject { get; set; }
        public string AzureEmaiTenantId { get; set; }
        public string AzureEmailClientID { get; set; }
        public string AzureEmaiClientSecert { get; set; }
        public string AzureScope { get; set; }
    }
}
