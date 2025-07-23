using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.Demo.Application.Services.ApiConsts;
namespace Edocs.Demo.Application.Services.Pages.Partial_Pages
{
    public class GetPdfDocViewModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;

        private readonly string AcceptRejectPdfFname = "acceptrejectpdf.pdf";
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
       
        public string DocName
        { get; set; }
        public GetPdfDocViewModel(IWebHostEnvironment webHostEnvironment, IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync(string pdfDocName)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            if (!(System.IO.Directory.Exists(System.IO.Path.Combine(webRootPath, $"pdfdocuments"))))
                Directory.CreateDirectory(System.IO.Path.Combine(webRootPath, $"pdfdocuments"));
            string docName = System.IO.Path.Combine(webRootPath, $"pdfdocuments\\{AcceptRejectPdfFname}");
            pdfDocName = Path.Combine(@"D:\BSB\\PublicWorks\", pdfDocName);
            byte[] bytes = System.IO.File.ReadAllBytes(pdfDocName);
            System.IO.File.WriteAllBytes(docName, bytes);
            DocName = docName;
            return Page();
        }
    }
}
