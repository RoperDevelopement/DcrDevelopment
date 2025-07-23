using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EDocs.Nyp.LabReqs.AppServices.Pages.LabReqs
{
    public class LabReqsHomeModel : PageModel
    {
        public void OnGet()
        {
            ViewData["CWID"] = "edocs";
            //< div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        }
    }
}