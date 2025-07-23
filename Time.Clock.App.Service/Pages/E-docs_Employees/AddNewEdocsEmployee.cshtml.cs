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

namespace Edocs.Employees.Time.Clock.App.Service.Pages.E_docs_Employees
{
    public class AddNewEdocsEmployeeModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }

        public AddNewEdocsEmployeeModel(IConfiguration config, IHttpClientFactory factoryClient)
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
        [BindProperty]
        public EmpModel Emp

        { get; set; }
        public IList<string> StateAbb
        { get; set; }
        [BindProperty]
        public string EmpState
        { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
                edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
            if (string.IsNullOrWhiteSpace(edocsAdmin))
            {
                return Redirect($"/ClockInOut/ClockInOutView");
            }
            ViewData["EdocsAdmin"] = "1";
            ViewData["EmpID"] = edocsAdmin;
            //  EmpModel Emp = new EmpModel();
            //Emp.EmpHolidayPayRate = (float)0.0;
          //  ViewData["EdocsAdmin"] = "1";
            StateAbb = TimeClockConst.GetStatesABB().ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
           
            Emp.EmpActive = true;
            Emp.EmpState = EmpState;
            WebApis.EdocsEmpsApi.AddEditTimeClockUsers(WebApiUrl, TimeClockConst.ControllerAddEditTimeClockUsers, clientFactory, Emp).ConfigureAwait(false).GetAwaiter().GetResult();
            //  if (!(ModelState.IsValid))
            // ModelErrorCollection modelErrors = ModelState["Property"].Errors;
            // Console.WriteLine();

            
            string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
                edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
            if (string.IsNullOrWhiteSpace(edocsAdmin))
            {
                return Redirect($"/ClockInOut/ClockInOutView");
            }
            ViewData["EdocsAdmin"] = "1";
            ViewData["EmpID"] = edocsAdmin;
            return Redirect($"/E-docs_Employees/AddNewEdocsEmployee");
            return Page();
        }
    }
}
