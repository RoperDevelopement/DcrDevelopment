using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.ApisConst;
using System.Net.Http;
using System.Text.RegularExpressions;
using Edocs.ITS.AppService.Models;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
namespace EDocs.Nyp.LabReqs.AppServices
{
    public class DisplayPdfFilePartialViewModel : PageModel
    {

        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IEmailSettings emailSettings;
        public DisplayPdfFilePartialViewModel(IConfiguration config, IHttpClientFactory factoryClient, IEmailSettings email)
        {
            try
            {
                configuration = config;
                clientFactory = factoryClient;
                emailSettings = email;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
     

      

        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
       

        public UserLoginModel UserLogin
        { get; set; }

        public async Task OnGet()
        {
            var qString = Request.QueryString;
            try
            {
                ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
                UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();

                if (qString.HasValue)
                {
                    string pdfFName = string.Empty;
                    // AzureBlobStorage.BlobStorageInstance.AzureBlobAccountKey = AzureBlobAccountKey;
                    //AzureBlobStorage.BlobStorageInstance.AzureBlobAccountName = AzureBlobAccountName;
                    // AzureBlobStorage.BlobStorageInstance.AzureBlobStorageConnectionString = AzureBlobStorageConnectionString;

                    string qStringValue = Request.Query["pdfurl"];
                    if(qStringValue.Contains("@"))
                    {
                        pdfFName = qStringValue.Replace("@", "\\").Trim();
                    }
                    else
                    pdfFName = EdocsITSApis.GetRepFileName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSTrackingByProjectNameController, qStringValue).ConfigureAwait(false).GetAwaiter().GetResult();
                    //if (!(pdfFName.ToLower().EndsWith(".pdf")))
                    //{
                    //    int indexEq = qString.Value.IndexOf("=");
                    //    pdfFName = qString.Value.Substring(++indexEq);
                    //}
                    if (!(System.IO.File.Exists(pdfFName)))
                        throw new Exception($"File not found {pdfFName}");


                    Response.Headers.Add("Content-Disposition", $"inline; filename=\"{pdfFName}\"");
                    byte[] pdfFile = System.IO.File.ReadAllBytes(pdfFName);
                    if (pdfFile == null)
                    {

                        throw new Exception($"Pdf file not found: {pdfFName}");
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