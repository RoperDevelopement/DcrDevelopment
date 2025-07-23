using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employees.Time.Clock.App.Service.Constants;
using Edocs.Employees.Time.Clock.App.Service.WebApis;
using System.Net.Http;

namespace Edocs.Employees.Time.Clock.App.Service.Partial_Pages
{
    public class ClockInOutEmpViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        public string RetMess
        { get; set; }
        public ClockInOutEmpViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;

        }
        public async Task<IActionResult> OnGetAsync(string empId,bool isAdmin)
        {
            //   return Redirect($"/ClockInOut/ClockInOutView");
            RetMess = WebApis.EdocsEmpsApi.ClockInOutEmp(empId, false, WebApiUrl, TimeClockConst.ControllerTimeClockInOut, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
            if(RetMess.ToLower().StartsWith(TimeClockConst.AdminMessage.ToLower()))
            {
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, TimeClockConst.SessionEdocsAdmin,empId).ConfigureAwait(false).GetAwaiter().GetResult();
            //    return Redirect($"/Index");
            }
            return Page();
        }
    }
}
