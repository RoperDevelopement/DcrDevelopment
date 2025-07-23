using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.Demo.Application.Services.ApiConsts;
namespace Edocs.Demo.Application.Services.Pages.Partial_Pages
{
    public class DisplayPdfFilePartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
       
        public DisplayPdfFilePartialViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            try
            {
                configuration = config;
                clientFactory = factoryClient;
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
        public async Task OnGet()
        {
            var qString = Request.QueryString;
            try
            {
                 
                if (qString.HasValue)
                {

                    // AzureBlobStorage.BlobStorageInstance.AzureBlobAccountKey = AzureBlobAccountKey;
                    //AzureBlobStorage.BlobStorageInstance.AzureBlobAccountName = AzureBlobAccountName;
                    // AzureBlobStorage.BlobStorageInstance.AzureBlobStorageConnectionString = AzureBlobStorageConnectionString;

                    string qStringValue = Request.Query["pdfurl"];
                     //string pdfFName = DemoApis.GetRepFileName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSTrackingByProjectNameController, //qStringValue).ConfigureAwait(false).GetAwaiter().GetResult();
                    //if (!(pdfFName.ToLower().EndsWith(".pdf")))
                    //{
                    //    int indexEq = qString.Value.IndexOf("=");
                    //    pdfFName = qString.Value.Substring(++indexEq);
                    //}
                    if (!(System.IO.File.Exists(qStringValue)))
                        throw new Exception($"File not found {qStringValue}");


                    Response.Headers.Add("Content-Disposition", $"inline; filename=\"{qStringValue}\"");
                    byte[] pdfFile = System.IO.File.ReadAllBytes(qStringValue);
                    if (pdfFile == null)
                    {

                        throw new Exception($"Pdf file not found: {qStringValue}");
                    }

                    await Response.Body.WriteAsync(pdfFile, 0, pdfFile.Length).ConfigureAwait(true);
                }
                else

                    throw new Exception("Invalid query string");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



        }
    }
}
