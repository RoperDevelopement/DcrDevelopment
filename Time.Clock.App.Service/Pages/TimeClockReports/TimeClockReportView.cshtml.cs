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
using System.ComponentModel.DataAnnotations;
namespace Edocs.Employees.Time.Clock.App.Service.Pages.TimeClockReports
{
    public class TimeClockReportViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        [BindProperty]
        public IList<EmpIDModel> EmpId
        { get; set; }
        [BindProperty]
        [Display(Name = "Time Card Start Date:")]
        [DataType(DataType.Date)]
        public DateTime ClockInDate
        { get; set; }
        [BindProperty]
        [Display(Name = "Time Card End Date:")]
        [DataType(DataType.Date)]
        public DateTime ClockOutDate
        { get; set; }
        [BindProperty]
        [Display(Name = "Employee Login ID:")]
       
        public string EmpLogInID
        { get; set; }
        public TimeClockReportViewModel(IConfiguration config, IHttpClientFactory factoryClient)
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
        public async Task<IActionResult> OnGetAsync(string empID)
        { 
            EmpLogInID = "N\\A";
            if (InitPage().ConfigureAwait(false).GetAwaiter().GetResult())
                return Redirect($"/ClockInOut/ClockInOutView");
          
            EmpId = WebApis.EdocsEmpsApi.TimeClockLoginEmpID(WebApiUrl, TimeClockConst.ControllerTimeClockReports, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
            EmpIDModel empIDModel = new EmpIDModel();
            empIDModel.EmpID = "Select EMP ID";
            EmpId.Insert(0, empIDModel);
            if (!string.IsNullOrWhiteSpace(empID))
            {
           
                EmpLogInID = empID;
                if(!(string.IsNullOrWhiteSpace(EmpLogInID)))
                {
                    ClockInDate = DateTime.Now.AddDays(-14);
                    ClockOutDate = DateTime.Now;
                }
            }
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

            ViewData["EdocsAdmin"] = "1";
            ViewData["EmpID"] = edocsAdmin;
            return false;
        }

        public async Task<IActionResult> OnPostAsync(string empID)
        {
            if (InitPage().ConfigureAwait(false).GetAwaiter().GetResult())
                return Redirect($"/ClockInOut/ClockInOutView");
            return Page();

        }
    }
}
