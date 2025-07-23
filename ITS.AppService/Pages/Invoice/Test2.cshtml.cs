using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Edocs.ITS.AppService.Models;
namespace Edocs.ITS.AppService.Pages.Invoice
{
    public class Test2Model : PageModel
    {
        public void OnGet()
        {
        }
        [HttpPost]
        public IActionResult OnPost(IFormFile MyUpLoader)
        {
            return new ObjectResult(new { status = "Fail" });
        }
       
        
        [HttpPost]
        public IActionResult  MyUploader(IFormFile MyUpLoader )
        {
            return new ObjectResult(new { status = "Fail" });
        }
       
    }
}
