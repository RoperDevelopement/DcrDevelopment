using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using System.IO;

namespace Edocs.ITS.AppService.Pages.DocumentsAcceptReject
{
    public class GetPdfDocViewModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;

        private readonly string AcceptRejectPdfFname = "acceptrejectpdf.pdf";
        public GetPdfDocViewModel(IWebHostEnvironment webHostEnvironment, IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }
        private string PDFViewFolder
        { get { return configuration.GetValue<string>(EdocsITSConstants.JsonPDFViewFolder); } }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public IList<AcceptRejectDocumentsModel> RejectDocumentsModels
        { get; set; }
        public string DocName
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string pdfDocName)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;

            // docName = @"wwwroot\pdfdocuments\danno.pdf";
            //   docName = "~/pdfdocuments/danno.pdf";
            if (!(System.IO.Directory.Exists(System.IO.Path.Combine(webRootPath, $"pdfdocuments"))))
                Directory.CreateDirectory(System.IO.Path.Combine(webRootPath, $"pdfdocuments"));
           string  docName = System.IO.Path.Combine(webRootPath,$"pdfdocuments\\{AcceptRejectPdfFname}");
              pdfDocName = pdfDocName.Replace("@", "\\").Trim();
          //  pdfDocName = @"D:\invoiceshtml\1ecf0f94-4833-411c-9c50-c27868a926fc.pdf.pdf";

              RejectDocumentsModels = EdocsITSApis.GetPdfDocuments(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSAcceptRejectDocumentsController, EdocsITSConstants.CurrentCustIDInt).ConfigureAwait(false).GetAwaiter().GetResult();
            var dName = RejectDocumentsModels.First(p =>p.FileName == pdfDocName);
                byte[] bytes = System.IO.File.ReadAllBytes(dName.FileName);
            //  byte[] bytes = System.IO.File.ReadAllBytes(pdfDocName);
            //pdfDocName = Path.Combine(PDFViewFolder, Path.GetFileName(dName.FileName));
         //   pdfDocName = Path.Combine(, Path.GetFileName(dName.FileName));
            System.IO.File.WriteAllBytes(docName, bytes);
          //  DocName = RejectDocumentsModels[0].FileName;
            DocName = docName;
            //< embed src = "@imgUrl" style = "width=100%; height=2100px;" />;
            // string docName = @"D:\ArchiverBackup\02-04-2023\dsdsds\dsdsds.pdf";
            // string contentType = "application/pdf";
            // byte[] bytes = System.IO.File.ReadAllBytes(docName);
            return Page();
            // <embed src="@Model.DocName" style="width=100%; height=2100px;" />;
            //return new JsonResult(new { FileName = docName, ContentType = contentType, Data = bytes });
        }
    }
}
