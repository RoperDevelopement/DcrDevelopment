using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employees.Time.Clock.App.Service.Constants;
using Edocs.Employees.Time.Clock.App.Service.WebApis;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;

namespace Edocs.Employees.Time.Clock.App.Service.Pages.E_docs_Employees
{
    public class TimeClockAdminLoginViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        [BindProperty]
        public string EmpID
        { get; set; }

        [BindProperty]
        public string EmpPW
        { get; set; }
        public string RetMess
        { get; set; }
        public TimeClockAdminLoginViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            try
            {
                configuration = config;
                clientFactory = factoryClient;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Redirect($"/Index");
            if (InitPage().ConfigureAwait(false).GetAwaiter().GetResult())
return Redirect($"/ClockInOut/ClockInOutView");
                //  EmpModel Emp = new EmpModel();
                //Emp.EmpHolidayPayRate = (float)0.0;


                return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            TimeClockAdminLogin timeClockAdmin = new TimeClockAdminLogin();
            timeClockAdmin.EmpID = EmpID;
            timeClockAdmin.EmpPW = EmpPW;
            RetMess = WebApis.EdocsEmpsApi.TimeClockLogin(timeClockAdmin, WebApiUrl, TimeClockConst.ControllerTimeClockLogIn, clientFactory).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(RetMess.ToLower().StartsWith(TimeClockConst.ErrorMessage.ToLower())))
                return Redirect($"/Index");
            if (InitPage().ConfigureAwait(false).GetAwaiter().GetResult())
                return Redirect($"/ClockInOut/ClockInOutView");
            RetMess = $"*{RetMess}";
            return Page();
        }
        private async Task<bool> InitPage()
        {
            string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
                edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
            if (string.IsNullOrWhiteSpace(edocsAdmin))
            {
                return true;
            }

            ViewData["EdocsAdmin"] = "0";
            ViewData["EmpID"] = edocsAdmin;
            return false;
        }
    }
}
