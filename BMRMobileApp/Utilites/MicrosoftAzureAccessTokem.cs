using Microsoft.Graph;
using Microsoft.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Utilites
{
    public class MicrosoftAzureAccessTokem
    {
        public static readonly Lazy<MicrosoftAzureAccessTokem> AccessTokenInstance = new Lazy<MicrosoftAzureAccessTokem>(() => new MicrosoftAzureAccessTokem());

        public async Task AssessToken()
        {
            try
            {
                // Create a TokenCredential
                //   var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                // Initialize GraphServiceClient
                //    var graphClient = new GraphServiceClient(clientSecretCredential);



                // Create a confidential client application
                var confidentialClient = ConfidentialClientApplicationBuilder
                    .Create(ConfigurationManager.SettingsApp.AzureEmailClientID)
                    .WithTenantId(ConfigurationManager.SettingsApp.AzureEmaiTenantId)
                    .WithClientSecret(ConfigurationManager.SettingsApp.AzureEmaiClientSecert)
                    .Build();

                // Authenticate and get an access token
                var authProvider = new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    var authResult = await confidentialClient
                        .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                        .ExecuteAsync();

                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                });

                // Create a GraphServiceClient
                var graphClient = new GraphServiceClient(authProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task GetsAccessTokenUSingGraph()
        {
            var confidentialClient = ConfidentialClientApplicationBuilder
           .Create(ConfigurationManager.SettingsApp.AzureEmailClientID)
           .WithClientSecret(ConfigurationManager.SettingsApp.AzureEmaiClientSecert)
           .WithAuthority(new Uri($"https://login.microsoftonline.com/{ConfigurationManager.SettingsApp.AzureEmaiTenantId}"))
           .Build();
         //     var authProvider = new ClientCredentialProvider(confidentialClient);
          //  var graphClient = new GraphServiceClient(clientSecretCredential);
        }
        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                string[] scopes = { "Mail.Send" };
                var app = PublicClientApplicationBuilder.Create(ConfigurationManager.SettingsApp.AzureEmailClientID)
                   // .WithAuthority(AzureCloudInstance.AzurePublic, ConfigurationManager.SettingsApp.AzureEmaiTenantId)
                    //.WithRedirectUri("http://localhost")
                    .WithRedirectUri($"msal{ConfigurationManager.SettingsApp.AzureEmailClientID}://auth")
                    .Build();

                var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<string> GetAccessTokenAsync(string clientID,string clientSecret,string tenantID,string scope)
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientID)
                .WithClientSecret(clientSecret)
               // .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantID}")).
               .WithTenantId(tenantID)
                .Build();
            var authResult = await app
    .AcquireTokenForClient(new[] {scope})
    .ExecuteAsync();
            //   var result = await app.AcquireTokenForClient(new[] {scope}).ExecuteAsync();
            return authResult.AccessToken;
        }
        public   async Task SendEmailTask()
        {
            try
            {
                var fromAddress = new MailAddress("dancroper@gmail.com", "Your Name");
                var toAddress = new MailAddress("mtcharles@hotmail.com");
            const string appPassword = "dr080564@"; // Use Gmail App Password

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, appPassword)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "test",
                    Body = "test"
                };

                await Task.Run(() => smtp.Send(message)); // Background execution
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
