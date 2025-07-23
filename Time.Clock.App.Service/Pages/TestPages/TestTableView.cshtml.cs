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

namespace Edocs.Employees.Time.Clock.App.Service.Pages.TestPages
{
    public class TestTableViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        public string RetMess
        { get; set; }
        public string TotalHrsWorked
        { get; set; }
        public IList<TimeClockEntriesModel> TimeClockHrs
        { get; set; }
        public string EmpID

        { get; set; }
        public int PageInID
        { get; set; }

        public int PageOutID
        { get; set; }
        public int TotDurID
        { get; set; }
        public TestTableViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;

        }
       
            public async Task<IActionResult> OnGetAsync(string empId, DateTime timeClockStartDate, DateTime timeClockEndDate, string id = null)
            {
            
            string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
                edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
            if (string.IsNullOrWhiteSpace(edocsAdmin))
            {
                return Redirect($"/ClockInOut/ClockInOutView");
            }
            empId = edocsAdmin;
            timeClockStartDate = DateTime.Parse("08/01/2022");
            timeClockEndDate = DateTime.Parse("08/04/2022");
            TimeClockHrs = WebApis.EdocsEmpsApi.ClockEmpHrs(empId, timeClockStartDate, timeClockEndDate, TimeClockConst.SpsTimeClockTimeWorkedReports, WebApiUrl, TimeClockConst.ControllerTimeClockReports, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
            GetTotalHrs().ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["EdocsAdmin"] = "1";
            ViewData["EmpID"] = edocsAdmin;
            return Page();
        }
        private async Task GetTotalHrs()
        {
            TotalHrsWorked = "00:00:00";
            double totSecs = 0;
            if (TimeClockHrs.Count > 0)
            {

                //TimeClockEntriesModel stTime = TimeClockHrs[0];
                //TimeClockEntriesModel endTime = TimeClockHrs[TimeClockHrs.Count-1];
                //if(endTime.ClockOutTime.Year < 2022)
                //{
                //    if (TimeClockHrs.Count >= TimeClockHrs.Count-2)
                //        endTime = TimeClockHrs[TimeClockHrs.Count - 2];


                //}
                //TimeSpan ts = endTime.ClockInTime - stTime.ClockOutTime;
                //TotalHrsWorked = $"{ts.Hours}:{ts.Minutes}:{ts.Seconds}";
                foreach (var sec in TimeClockHrs)
                {
                    if (sec.ClockOutTime.Year > 2021)
                    {
                        TimeSpan ts = sec.ClockOutTime - sec.ClockInTime;
                        totSecs += ts.TotalSeconds;
                    }
                }
                TimeSpan tot = TimeSpan.FromSeconds(totSecs);
                TotalHrsWorked = $"{tot.Hours}:{tot.Minutes.ToString("D2")}:{tot.Seconds.ToString("D2")}";


            }

        }
    }
}
