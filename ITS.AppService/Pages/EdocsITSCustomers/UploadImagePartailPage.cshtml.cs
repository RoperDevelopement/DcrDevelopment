using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Edocs.ITS.AppService.Pages.EdocsITSCustomers
{
    public class UploadImagePartailPageModel : PageModel
    {
        public async Task<IActionResult> OnGet(string hellp)
        {
            return Page();
        }
    }
}
