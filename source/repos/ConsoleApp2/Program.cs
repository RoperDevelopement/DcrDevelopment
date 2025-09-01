using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Net.Http;
using System.IO;
namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string clientId = "3a36c32e-626c-453a-b743-3410546ec12a";

           string tenantId = "0c3186e8-34a0-4aba-bc46-4a02f9176983";
          
            string clientSecret = "3x18Q~8e4WftZO2eRR.KLsWqXBKV6KiOm7Vz0atc";
            string siteId = "ab9b2e20-48eb-46ec-bd34-364764b43ca8";
            string listId = "9B344641-592F-4A59-AE0F-9495976E46AA";
            string siteUrl = "https://veccoop.sharepoint.com";

            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                 .Create(clientId)
                 .WithClientSecret(clientSecret)
                 .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                 .Build();

            // Define the required scopes
            string[] scopes = { "https://graph.microsoft.com/.default" };

            // Acquire token interactively
            AuthenticationResult authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            // Initialize GraphServiceClient
            var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                return Task.CompletedTask;
            }));

                // Fetch the user's drive
                //   var drive = await graphClient.Me.Drive.Request().GetAsync();

                // Output the Drive ID
                // Console.WriteLine($"Drive ID: {drive.Id}");
                var drives = await graphClient.Sites[siteId].Drives
               .Request()
               .GetAsync();
         
                foreach (var drive in drives)
                {
                    Console.WriteLine($"Drive Name: {drive.Name}, Drive ID: {drive.Id}");
                }
                Console.WriteLine("Drives (Document Libraries):");
                foreach (var drive in drives)
                {
                    Console.WriteLine($"- {drive.Name} (ID: {drive.Id})");
                    
                    // Get sub-folders (sub-drives) in the drive
                    var rootItems = await graphClient.Drives[drive.Id].Root.Children
                        .Request()
                        .GetAsync();

                    Console.WriteLine("Sub-Drives/Folders:");
                    foreach (var item in rootItems)
                    {
                        if (item.Folder != null) // Check if it's a folder
                        {
                            Console.WriteLine($"  - {item.Name} (ID: {item.Id})");
                        }
                    }
                }
                    string filePath = @"L:\Dillion\VCC\Jan_2022_WO2020050_v000.pdf";

                var driveName = "NewDocumentLibrary";
                var driveDescription = "This is a new document library created via Microsoft Graph API.";

                //var list = new List
                //{
                //    DisplayName = driveName,
                //    Description = driveDescription,
                //    ListInfo = new ListInfo
                //    {
                //        Template = "documentLibrary"
                //    }
                //};

                
                //    var createdList = await graphClient.Sites[siteId].Lists
                //        .Request()
                //        .AddAsync(list);

                 //   Console.WriteLine($"Drive '{createdList.DisplayName}' created successfully!");
                await CreateFolderAsync(siteId, "b!IC6bq-tI7Ea9NDZHZLQ8qHUHC8nz-txGjuERXUZqcQRBRjSbL1lZSq4PlJWXbkaq", "VCCInVoices", authResult.AccessToken);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    var uploadResult = await graphClient.Sites[siteId]
                        .Drives["b!IC6bq-tI7Ea9NDZHZLQ8qHUHC8nz-txGjuERXUZqcQRBRjSbL1lZSq4PlJWXbkaq"]
                        .Root
                        .ItemWithPath("an_2022_WO2020050_v000.pdf")
                        .Content
                        .Request()
                        .PutAsync<DriveItem>(fileStream);

                }


                    // SharePoint details
                    //string siteId = "YOUR_SITE_ID"; // Use Graph Explorer to find Site ID
                    //string driveId = "YOUR_DRIVE_ID"; // Use Graph Explorer to find Drive ID
                    //string filePath = @"C:\Path\To\Your\File.txt";
                    //string fileName = "File.txt";

                    // Authenticate using MSAL
                    //IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                    //    .Create(clientId)
                    //    .WithClientSecret(clientSecret)
                    //    .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                    //    .Build();

                    //     // string[] scopes = new[] { "https://graph.microsoft.com/.default" };
                    ////    string[] scopes = new[] { "User.Profile" };
                    //    //   string[] scopes = { $"https://{tenantId}.sharepoint.com/.default" };
                    //    // string[] scopes = { "https://graph.microsoft.com/Sites.ReadWrite.All" };

                    //    AuthenticationResult authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                    //  string d =  authResult.TokenType;
                    //// Initialize Graph Client
                    //GraphServiceClient graphClient = new GraphServiceClient(
                    //    new DelegateAuthenticationProvider((requestMessage) =>
                    //    {
                    //        requestMessage.Headers.Authorization =
                    //            new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    //        return Task.CompletedTask;
                    //    }));



                    //    // var driveItems = await graphClient.Me.Drive.Root.Children.Request().GetAsync();
                    //    var ddd = graphClient.Sites[siteId].Lists[listId].Drive;


                    //    //HttpClient client = new HttpClient();
                    //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    //    ///HttpResponseMessage response = await client.GetAsync($"https://{tenantId}.sharepoint.com/_api/web");
                    //    // // Authenticate using client credentials
                    //    // var confidentialClient = ConfidentialClientApplicationBuilder
                    //    //     .Create(clientId)
                    //    //     .WithTenantId(tenantId)
                    //    //     .WithClientSecret(clientSecret)
                    //    //     .Build();

                    //    // var authProvider = new DelegateAuthenticationProvider(async (requestMessage) =>
                    //    // {
                    //    //     var authResult = await confidentialClient
                    //    //         .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                    //    //         .ExecuteAsync();

                    //    //     requestMessage.Headers.Authorization =
                    //    //         new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    //    // });

                    //    // // Initialize GraphServiceClient
                    //    // var graphClient = new GraphServiceClient(authProvider);

                    //    // // Example: Access a SharePoint site
                    //    //// var siteId = "YOUR_SITE_ID"; // Replace with your SharePoint site ID
                    //    var siteHostname = "veccoop.sharepoint.com";
                    //var sitePath = "/sites";

                    //    var site = await graphClient.Sites
                    //    .GetByPath(sitePath, siteHostname)
                    //    .Request()
                    //    .GetAsync();
                    //    //var site = await graphClient.Sites[siteId].Request().GetAsync();

                    //    // Console.WriteLine($"Site Name: {site.DisplayName}");
                    //    // Console.WriteLine($"Web URL: {site.WebUrl}");
                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static async Task CreateFolderAsync(string siteId, string driveId, string folderName, string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set up the request headers
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Define the API endpoint
                string endpoint = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drives/{driveId}/root/children";

                // Define the request body
                var requestBody = new
                {
                    name = folderName,
                    folder = new { }

                };

                // Serialize the request body to JSON
                string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);

                // Send the POST request
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Folder '{folderName}' created successfully!");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error creating folder: {response.StatusCode} - {error}");
                }
            }
            }
        }
    }
 