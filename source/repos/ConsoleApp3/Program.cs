using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
namespace ConsoleApp3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scopes = new[] { "User.Read" };
            string clientId = "3a36c32e-626c-453a-b743-3410546ec12a";

            string tenantId = "0c3186e8-34a0-4aba-bc46-4a02f9176983";

            string clientSecret = "3x18Q~8e4WftZO2eRR.KLsWqXBKV6KiOm7Vz0atc";

            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = clientId,
                TenantId = tenantId,
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };

            var deviceCodeCredential = new DeviceCodeCredential(options);
            var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);
            string userEmail = "alersender@edocsusa.com";
            var siteHostname = "veccoop.sharepoint.com";
            var sitePath = "/sites/VECDocuments";
            var userCollection = await graphClient.Users.GetAsync(requestConfiguration => requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{userEmail}'");
            //return userCollection?.Value?.FirstOrDefault();
            //  var site = await graphClient.Sites.GetAllSites.;// GetAllSites().Request().GetAsync();
            // Example: Get the current user's profile
            //    var user = await graphClient.Me.Request().GetAsync();
            // Console.WriteLine($"Hello, {user.DisplayName}");
        }
    }
}
