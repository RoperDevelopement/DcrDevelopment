using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Core;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.Kiota.Abstractions;
namespace UpLoadSharePoint
{
    class Program
    {
        static   void Main(string[] args)
        {

            

        }

       async void UploadSharePoint()
        {
            // Azure AD App details
            string clientId = "3a36c32e-626c-453a-b743-3410546ec12a";
            string tenantId = "Oc3186e8-34a0-4aba-bc46-4a02f9176983";
            string clientSecret = "718d8604-4685-4d89-860c-80f4e0eac57e";

            // SharePoint details
            string siteId = "YOUR_SITE_ID"; // Use Graph Explorer to find Site ID
            string driveId = "YOUR_DRIVE_ID"; // Use Graph Explorer to find Drive ID
            string filePath = @"C:\Path\To\Your\File.txt";
            string fileName = "File.txt";

            // Authenticate using MSAL
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            string[] scopes = { "https://graph.microsoft.com/.default" };
            AuthenticationResult authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            // Initialize Graph Client
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    return Task.CompletedTask;
                }));

            // Upload file
            //using (var fileStream = new FileStream(filePath, FileMode.Open))
            //{
            //    var uploadResult = await graphClient.Sites[siteId]
            //        .Drives[driveId]
            //        .Root
            //        .ItemWithPath(fileName)
            //        .Content
            //        .Request()
            //        .PutAsync<DriveItem>(fileStream);

            //    Console.WriteLine($"File uploaded successfully! File ID: {uploadResult.Id}");
            //}
        }
    }
}
