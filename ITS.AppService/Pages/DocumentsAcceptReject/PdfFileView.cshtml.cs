using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITS.AppService.Pages.DocumentsAcceptReject
{
    public class PdfFileViewModel : PageModel
    {
        public string DocName  
        { get; set; }
        public async Task<IActionResult> Get(string docName)
        {
            docName = @"D:\ArchiverBackup\02-04-2023\danno\danno.pdf";
            DocName = docName;
            //< embed src = "@imgUrl" style = "width=100%; height=2100px;" />;
            // string docName = @"D:\ArchiverBackup\02-04-2023\dsdsds\dsdsds.pdf";
            // string contentType = "application/pdf";
            // byte[] bytes = System.IO.File.ReadAllBytes(docName);
            return Page();
            //return new JsonResult(new { FileName = docName, ContentType = contentType, Data = bytes });
        }
    }
}
