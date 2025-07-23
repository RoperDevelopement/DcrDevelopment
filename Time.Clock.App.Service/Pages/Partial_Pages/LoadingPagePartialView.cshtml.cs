using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class LoadingPagePartialViewModel : PageModel
    {
        public string StartDate
        { get; set; }

        public string EndDate
        { get; set; }

        public async Task<IActionResult>  OnGetAsync()
        {
            
            return Page();
        }
    }
}