// Application (client) ID 5c5c5986-5cf6-4e5c-a701-6bce886a2ded
//Directory(tenant) ID
//45f5448b-3ad5-4943-ae69-a192a3e59acf
//redir url https://login.microsoftonline.com/common/oauth2/nativeclient
//client ser value taR8Q~jN5tyuMQEkGgKd_HDGiOSmR7CcK5ap6aOc id 450142fa-cadb-4d44-aed5-a0472362900f
using BMRMobileApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
 using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.Utilites;

namespace BMRMobileApp
{
    public static class ConfigurationManager
    {
        public static AppSettings SettingsApp { get; set; }

        // public static AppSettings SettingsApp { get;   set; }
        public static ChatHubModel ChatHubModel { get; set; }
        public static async Task Initialize()
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "appsettings.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (!File.Exists(filePath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
                using var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                File.WriteAllText(filePath, content);

            }
            string json = File.ReadAllText(filePath);
            SettingsApp = Deserialize<AppSettings>(json, "AppSettings");
            ChatHubModel = Deserialize<ChatHubModel>(json, "ChatHubModel");
            EmailService emailService = new EmailService();
         // await  emailService.SendEmailAsync(string.Empty,string.Empty,string.Empty);

            //        var jObj = JObject.Parse(json);
            //        var singularJson = jObj["ChatHubModel"]?.ToString();
            //        ChatHubModel = JsonConvert.DeserializeObject<ChatHubModel>(singularJson);
            //        var config = JsonConvert.DeserializeObject<AppSettings>(json);
            //        SettingsApp = JsonConvert.DeserializeObject<AppSettings>(json);
            //        ChatHubModel = JsonConvert.DeserializeObject<ChatHubModel>(json);
            //        //   SettingsApp.AppName = "Peer Support App";
            //        SettingsApp.SmtpMailOutlookCom = "smtp-mail.outlook.com";
            //SettingsApp.SmtpMailPortOutlookCom = "587";
            //        SettingsApp.PassWordOutlookCom = "122495Aa@";
            //        SettingsApp.EmailFrom = "dcrpsmobleapp@outlook.com";
            //        SettingsApp.EmailSubject = "PeerSupport Mobile App";
        }
public static T  Deserialize<T>(string json,string model) where T : new ()
        {
            var jObj = JObject.Parse(json);
            var jsonStr = jObj[model]?.ToString();
            var obj = JsonConvert.DeserializeObject<T>(jsonStr);
            return obj;
        }
    }

   
}
 