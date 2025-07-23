using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.Employees.Time.Clock.App.Service.Constants;
using Edocs.Employees.Time.Clock.App.Service.WebApis;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Policy;

namespace Edocs.Employees.Time.Clock.App.Service.Pages.Partial_Pages
{
    public class TimeCardAddNewEntryViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        public string RetMess
        { get; set; }
        public TimeCardAddNewEntryViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;

        }
        public async Task<IActionResult> OnGetAsync(string empId, DateTime timeClockStartDate, DateTime timeClockEndDate, int delID)
        {
            string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
                edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
            if (string.IsNullOrWhiteSpace(edocsAdmin))
            {
                return Redirect($"/ClockInOut/ClockInOutView");
            }
            if (string.Compare(empId, "DelRec", true) != 0)
            {
                TimeClockEntriesModel timeClockEntries = new TimeClockEntriesModel();
                timeClockEntries.EmpID = empId;
                timeClockEntries.ClockInTime = timeClockStartDate;
                timeClockEntries.ClockOutTime = timeClockEndDate;
                timeClockEntries.TimeSpanDur = "0";
                RetMess = WebApis.EdocsEmpsApi.TimeClockAddNewEntry(timeClockEntries, WebApiUrl, TimeClockConst.ControllerTimeClockInOut, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                if(delID > 0)
{
                    RetMess = WebApis.EdocsEmpsApi.DelTimeClockEntry(delID, WebApiUrl, TimeClockConst.ControllerAddEditTimeClockUsers, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            ViewData["EdocsAdmin"] = "1";
            ViewData["EmpID"] = edocsAdmin;
            return Page();
        }
        //public async Task<IActionResult> OnGetAsync(int delID)
        //{
        //    string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
        //    if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
        //        edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
        //    if (string.IsNullOrWhiteSpace(edocsAdmin))
        //    {
        //        return Redirect($"/ClockInOut/ClockInOutView");
        //    }

        //    ViewData["EdocsAdmin"] = "1";
        //    ViewData["EmpID"] = edocsAdmin;
        //    return Page();
        //}
    }
}
