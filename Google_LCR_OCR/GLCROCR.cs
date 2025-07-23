using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Google.Cloud.DocumentAI.V1;
using Google.Cloud.RecaptchaEnterprise.V1;
using Google.Apis.Discovery;
 
using Google.Apis.Services;

using Google.Protobuf;
using Google.Apis.Auth.OAuth2;
using Google.Api.Gax.ResourceNames;
using Google.Api.Gax;
using Google.Protobuf.WellKnownTypes;
using Google.Api.Gax.Grpc.Rest;
using Google.Api.Gax.Grpc;
//product end point https://us-documentai.googleapis.com/v1/projects/308372350131/locations/us/processors/47b0f3e8d60e05e3:process
namespace Google_LCR_OCR
{
    class GLCROCR
    {
        static void Main(string[] args)
        {
            try
            {


                //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\GoogleApiKey\service-account-file.json");
                // Environment.SetEnvironmentVariable("GOOGLE_CLOUD_PROJECT", "edocs-icr-ocr");
                //  FetchProcessorTypesRequestObject();
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\GoogleApiKey\icrocredocsa-8ee31dfb7bd9.json");
             //   FetchProcessorTypesRequestObject();
                GetTxt();
                //DocumentProcessorServiceClient client = new DocumentProcessorServiceClientBuilder
                //{
                //    Endpoint = "eu-documentai.googleapis.com"
                //}.Build();
                //// Initialize request argument(s)
                //ProcessRequest request = new ProcessRequest
                //{
                //    ResourceName = new UnparsedResourceName("a/wildcard/resource"),
                // SkipHumanReview = false,
                //    InlineDocument = new Document(),
                //    FieldMask = new FieldMask(),
                //};

                //// Make the request
                //ProcessResponse response = client.ProcessDocument(request);
                //// End snippet
                ///}
                ///
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            }
        static void GetTxt()
        {
            try
            {
                // env: GOOGLE_APPLICATION_CREDENTIALS = "KEY_PATH"

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\GoogleApiKey\icrocredocsa-8ee31dfb7bd9.json");
                 Environment.SetEnvironmentVariable("GOOGLE_CLOUD_PROJECT", "IcrOcrEdocsa");
                //   Environment.SetEnvironmentVariable("GOOGLE_CLOUD_PROJECT", "MyFirstProject");
                // Environment.SetEnvironmentVariable("GOOGLE_CLOUD_PROJECT", "IcrOcrImages");

                //  string pdfFilePath = @"L:\GoogleApi\a910f89a-aa32-4230-be7a-96faa3f78c94.png";
                string pdfFilePath = @"L:\a910f89a-aa32-4230-be7a-96faa3f78c941.png";

                String endpoint = string.Format("%s-documentai.googleapis.com:443", "us");
                //  DocumentProcessorServiceSettings settings =
                // DocumentProcessorServiceSettings.NewBuilder().setEndpoint(endpoint).build();
                // var bytes = Encoding.UTF8.GetBytes(pdfFilePath);
                var bytes = System.IO.File.ReadAllBytes(pdfFilePath);
                //DocumentProcessorServiceClient documentProcessorServiceClient = new DocumentProcessorServiceClientBuilder
                //    {
                //    //   Endpoint = "https://us-documentai.googleapis.com/v1/projects/662066218235/locations/us/processors/3569decb0c5b650e:process"
                //    Endpoint = "https://us-documentai.googleapis.com/v1/projects/662066218235/locations/us/processors/3569decb0c5b650e:process"
                //}.Build();
               
            ByteString content = ByteString.CopyFrom(bytes);
                // DocumentProcessorServiceClient documentProcessorServiceClient = DocumentProcessorServiceClient.CreateAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                // var documentProcessorServiceClient = new DocumentProcessorServiceClientBuilder
                // {
                //       Endpoint = "us-documentai.googleapis.com"
                //      Endpoint = "https://us-documentai.googleapis.com/v1/projects/662066218235/locations/us/processors/3569decb0c5b650e:process"
                //  Endpoint = "https://us-documentai.googleapis.com/v1/projects/662066218235/locations/us/processors/3569decb0c5b650e"
                // }.Build();
                //  DocumentProcessorServiceClient documentProcessorServiceClient = DocumentProcessorServiceClient.CreateAsync().ConfigureAwait//(false).GetAwaiter().GetResult();

                // DocumentProcessorServiceClient documentProcessorServiceClient = new DocumentProcessorServiceClientBuilder
                // {
                //     Endpoint =   "https://us-documentai.googleapis.com/v1/projects/662066218235/locations/us/processors/3569decb0c5b650e"
                // }.Build();

                var documentProcessorServiceClient = new DocumentProcessorServiceClientBuilder
                {
                    GrpcAdapter = GrpcCoreAdapter.Instance
                }.Build();
                ProcessRequest request = new ProcessRequest()

                {
                    //   ProcessorName = ProcessorName.FromProjectLocationProcessor(" * ****", "mycountry", "***"),
                  SkipHumanReview = false,
                 
                  //  Name = "https://us-documentai.googleapis.com/v1/" 
                    
                      InlineDocument = new Document(),
                    RawDocument = new RawDocument(),
                   // FieldMask = new FieldMask(),
               };
                Console.WriteLine(request.SourceCase.ToString());
               // request.Name =  "https://us-documentai.googleapis.com/v1/";
                 request.RawDocument.MimeType = "image/png";
                request.RawDocument.Content = content;
        
                // Make the request
                ProcessResponse response = documentProcessorServiceClient.ProcessDocumentAsync(request,null).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                Document docResponse = response.Document;
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }
        static void FetchProcessorTypesRequestObject()
        {
            try
            {
                RecaptchaEnterpriseServiceClient recaptchaEnterpriseServiceClient = RecaptchaEnterpriseServiceClient.CreateAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                // Initialize request argument(s)
                CreateAssessmentRequest request = new CreateAssessmentRequest
                {
                    ParentAsProjectName = ProjectName.FromProject("[IcrOcrEdocsa]"),
                    Assessment = new Assessment(),
                };
                // Make the request
                Assessment response = recaptchaEnterpriseServiceClient.CreateAssessmentAsync(request).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void FetchProcessorTypes()
        {
            // Snippet: FetchProcessorTypes(string, CallSettings)
            // Create client
            DocumentProcessorServiceClient documentProcessorServiceClient = DocumentProcessorServiceClient.Create();
            // Initialize request argument(s)
            string parent = "projects/[icrocredocsa]/locations/[us]";
            // Make the request
            FetchProcessorTypesResponse response = documentProcessorServiceClient.FetchProcessorTypes(parent);
            // End snippet
        }

    }
}
