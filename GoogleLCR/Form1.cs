using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth;
 
using Google.Cloud.Vision.V1;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Json;
using System.IO;
using System.Threading;
using Google.Apis.Util;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GoogleLCR
{
    public class ApiList
    {
        [JsonProperty("items")]
        public List<ApiMetadata> Items { get; set; }
    }
    public class ApiMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("discoveryRestUrl")]
        public string DiscoveryUrl { get; set; }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private void GetList()
        {
            var client = new HttpClient();
            string destination = @"L:\gapi";
        // Make sure the Discovery server never treats us as a Google-internal client.
        client.DefaultRequestHeaders.Add("X-User-Ip", "0.0.0.0");
            var apis = LoadApiListAsync(client).ConfigureAwait(false).GetAwaiter().GetResult();
        Console.WriteLine($"Discovery returned {apis.Count} APIs");
            foreach (var api in apis)
            {
                try
                {
                     string text =   FetchAndReformat(client, api).ConfigureAwait(false).GetAwaiter().GetResult(); 
        string filename = $"{api.Name}_{api.Version}.json";
        File.WriteAllText(Path.Combine(destination, filename), text);
                    Console.WriteLine($"Written {filename}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to download API '{api.Name}' version '{api.Version}': {e.Message}");
                }
            }
 
        }
        private   async Task<string> FetchAndReformat(HttpClient client, ApiMetadata api)
        {
            var json = await client.GetStringAsync(api.DiscoveryUrl);
            var root = JObject.Parse(json);
            var sorted = Sort(root);
            var stringWriter = new StringWriter();
            var writer = new JsonTextWriter(stringWriter)
            {
                Indentation = 1,
                IndentChar = ' ',
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            sorted.WriteTo(writer);
            return stringWriter.ToString().Replace("\r\n", "\n");
        }
        private JToken Sort(JToken token)
        {
                switch(token)
            {
                case JArray array:
                    return Sort(array);
                case JObject obj: return Sort(obj);
                default: return null;
            };
        }

        private async Task<List<ApiMetadata>> LoadApiListAsync(HttpClient client)
        {
            var json = await client.GetStringAsync("https://www.googleapis.com/discovery/v1/apis");
            var list = JsonConvert.DeserializeObject<ApiList>(json);
            return list.Items
                .OrderBy(api => api.Name, StringComparer.Ordinal)
                .ThenBy(api => api.Version, StringComparer.Ordinal)
                .ToList();
        }
        private void Lcr()
        {
            try
            {

                JsonCredentialParameters credentialParameters;
                //var client = ImageAnnotatorClient.Create();
                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(@"L:\Polics\icrtest-359115-ce319020864d.json").CreateScoped(ImageAnnotatorClient.DefaultScopes);
          //  var channel = new Grpc.Core.Channel(ImageAnnotatorClient.DefaultEndpoint.ToString(), credential.);
            // Instantiates a client
           // var client = ImageAnnotatorClient.Create(channel);
            //var image = Image.FromFile(@"<IMAGE_FILE_PATH>");
            //// Performs label detection on the image file
            //var response = client.DetectLabels(image);
            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //        Console.WriteLine(annotation.Description);
            //}
            
            var client = ImageAnnotatorClient.Create();
            var image = Google.Cloud.Vision.V1.Image.FromFile(@"C:\Archives\icr.png");
            var annotations = client.DetectDocumentText(image);
            var paragraphs = annotations.Pages
                .SelectMany(page => page.Blocks)
                .SelectMany(block => block.Paragraphs);
            foreach (var para in paragraphs)
            {
                var box = para.BoundingBox;
                Console.WriteLine($"Bounding box: {string.Join(" / ", box.Vertices.Select(v => $"({v.X}, {v.Y})"))}");
                var symbols = string.Join("", para.Words.SelectMany(w => w.Symbols).SelectMany(s => s.Text));
                Console.WriteLine($"Paragraph: {symbols}");
                Console.WriteLine();
            }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
              Lcr();
          //  ImageAnnotatorClient imageAnnotatorClient = ImageAnnotatorClient.Create();
            GetList();
        }
        /// <summary>Creates a default credential from a stream that contains JSON credential data.</summary>
        //internal async Task<GoogleCredential> CreateDefaultCredentialFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        //{
        //    JsonCredentialParameters credentialParameters;
        //    try
        //    {
        //        credentialParameters = await NewtonsoftJsonSerializer.Instance
        //            .DeserializeAsync<JsonCredentialParameters>(stream, cancellationToken)
        //            .ConfigureAwait(false);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new InvalidOperationException("Error deserializing JSON credential data.", e);
        //    }
        //    return CreateDefaultCredentialFromParameters(credentialParameters);
        //}
        /// <summary>Creates a default credential from JSON data.</summary>
        /// <summary>Creates a default credential from JSON data.</summary>
        //internal GoogleCredential CreateDefaultCredentialFromParameters(JsonCredentialParameters credentialParameters) =>
        //     switch nameof(credentialParameters)).Type
        //    {
        //        JsonCredentialParameters.AuthorizedUserCredentialType => new GoogleCredential(CreateUserCredentialFromParameters(credentialParameters)),
        //        JsonCredentialParameters.ServiceAccountCredentialType => GoogleCredential.FromServiceAccountCredential(CreateServiceAccountCredentialFromParameters(credentialParameters)),
        //        JsonCredentialParameters.ExternalAccountCredentialType => new GoogleCredential(CreateExternalCredentialFromParametes(credentialParameters)),
        //        _ => throw new InvalidOperationException($"Error creating credential from JSON or JSON parameters. Unrecognized credential type {credentialParameters.Type}."),
        //    };
    }
}
