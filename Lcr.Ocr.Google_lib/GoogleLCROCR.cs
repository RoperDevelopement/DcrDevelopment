using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Api.Gax;
using Google.Cloud.DocumentAI.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Apis.ApiKeysService.v2;
using Google.Cloud.Location;
using Google.Protobuf;
//using  com.google.api.apikeys.v2.ApiKeysClient;
//import com.google.api.apikeys.v2.ApiTarget;
//import com.google.api.apikeys.v2.CreateKeyRequest;
//import com.google.api.apikeys.v2.Key;
//import com.google.api.apikeys.v2.LocationName;
//import com.google.api.apikeys.v2.Restrictions;
namespace Edocs.Icr.Ocr.Google
{
    public class GoogleLCROCR
    {
       
         
    private static GoogleLCROCR instance = null;
        GoogleLCROCR() { }
        public static GoogleLCROCR Instance
        {
            get
            {
                {
                    if (instance == null)
                    {
                        instance = new GoogleLCROCR();
                    }
                    return instance;
                }
            }
        }
        public string ProcessImage(string pdfImgFilePath, string mineType)
        {
            string retString = string.Empty;
            try
            {
               
                var bytes = System.IO.File.ReadAllBytes(pdfImgFilePath);
                ByteString content = ByteString.CopyFrom(bytes);
                 DocumentProcessorServiceClient documentProcessorServiceClient = DocumentProcessorServiceClient.CreateAsync().ConfigureAwait(false).GetAwaiter().GetResult();


               // DocumentProcessorServiceClient documentProcessorServiceClient = new DocumentProcessorServiceClientBuilder
                 //{
                // Endpoint = "https://us-documentai.googleapis.com/v1/projects/662066218235/locations/us/processors/3569decb0c5b650e:process:443"
                  // Endpoint = "us-documentai.googleapis.com:443"


                 // }.Build();
                //GetProcessorRequest getProcessorRequest = new GetProcessorRequest();

                ProcessRequest request = new ProcessRequest
                {
                    ResourceName = new UnparsedResourceName("projects/662066218235"),
                    SkipHumanReview = false,
                    RawDocument = new RawDocument(),
                    // InlineDocument = new Document(),
                    // FieldMask = new FieldMask(),
                };
                // Make the request
                //  request.RawDocument.MimeType = "image/png";
                request.RawDocument.MimeType = mineType;
                request.RawDocument.Content = content;
                request.Name = "projects/icrocredocsa/locations/us/processors/3569decb0c5b650e";
                //"projects/{project_id}/locations/{location}/processors/{processor_id}

               // ProcessorName p = getProcessorRequest.ProcessorName;
                ProcessResponse response = documentProcessorServiceClient.ProcessDocument(request);
                Document docResponse = response.Document;


                for (int page = 0; page < docResponse.Pages.Count; page++)
                    retString += $"{docResponse.Pages[page].Lines}";
               // string s = docResponse.Text;
            }
            catch (Exception ex)
            {
              throw new Exception($"LCROCR Error {ex.Message}");
            }
            return retString.Trim();
        }
    }
}
