using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace EDocs.Nyp.LabReqs.AppServices
{
    public class DisplayErrorMessagesModel : PageModel
    {
        public string ErrorMessage
        { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
        {
             
            var qString = Request.QueryString;
            ErrorMessage = "Html Error";

            if (qString.HasValue)
            {
                ErrorMessage = $"Error Message {Request.Query["ErrMess"]}";
            }
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewData["CWID"] = "N/A";
             
        }
    }
}