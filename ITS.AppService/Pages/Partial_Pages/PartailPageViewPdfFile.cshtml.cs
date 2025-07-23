using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class PartailPageViewPdfFileModel : PageModel
    {
        public byte[] bytes
        { get; set; }
      public async Task<IActionResult> Get(string fileID )
        {
            string docName = @"D:\ArchiverBackup\02-04-2023\dsdsds\dsdsds.pdf";
            string contentType = "application/pdf";
             byte[] bytes = System.IO.File.ReadAllBytes(docName);
            return Page();
            //return new JsonResult(new { FileName = docName, ContentType = contentType, Data = bytes });
        }
    }
}
