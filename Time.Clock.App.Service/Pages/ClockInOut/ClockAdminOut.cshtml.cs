using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employees.Time.Clock.App.Service.Constants;
using Edocs.Employees.Time.Clock.App.Service.WebApis;
using System.Net.Http;
using System;

namespace Edocs.Employees.Time.Clock.App.Service.Pages.ClockInOut
{
    public class ClockAdminOutModel : PageModel
    {

        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        public string RetMess
        { get; set; }
        public ClockAdminOutModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            //   return Redirect($"/ClockInOut/ClockInOutView");
            string empId = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(empId))
            {
                return Redirect($"/Index");
            }
          
            RetMess = WebApis.EdocsEmpsApi.ClockInOutEmp(empId.Replace(TimeClockConst.DoubleQuotes, "").Trim(), true, WebApiUrl, TimeClockConst.ControllerTimeClockInOut, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, TimeClockConst.SessionEdocsAdmin, string.Empty).ConfigureAwait(false).GetAwaiter().GetResult();
            

            return Redirect($"/ClockInOut/ClockInOutView");
            

        }
    }
}
