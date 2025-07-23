using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IT= iTextSharp.LGPLv2.Core;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace EditPDF.Pages.PDF
{
    public class EditPDFModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            //string path = @"L:\Archives\Lab_Req_Copy_03-18-22\MaintenanceLogs\2022\03-17\fda09e76-919d-4b9a-9307-dba82a3e5124\1f301c0e-1c74-4b08-8f3e-291abb6d93c2.pdf";
            iTextSharp.text.pdf.PdfDocument()
            //// Get PDF as byte array from file (or database, browser upload, remote storage, etc)
            //byte[] pdfData = System.IO.File.ReadAllBytes(path);

            //// Create RAD PDF control
            //PdfWebControlLite pdfWebControl1 = new PdfWebControlLite(HttpContext);

            //// Create document from PDF data
            //pdfWebControl1.CreateDocument("Document Name", pdfData);

            //// Put control in ViewBag
            //ViewData["PdfWebControl1"] = pdfWebControl1;
            return Page();
        }
    }
}
