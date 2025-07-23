using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.Employees.Time.Clock.App.Service.Constants;
using Edocs.Employees.Time.Clock.App.Service.WebApis;
namespace Edocs.Employees.Time.Clock.App.Service.Pages.ClockInOut
{
    public class ClockInOutViewModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["EdocsAdmin"] = "0";
            return Page();
        }
    }
}
