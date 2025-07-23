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
using Edocs.Employees.Time.Clock.App.Service.InterFaces;

namespace Edocs.Employees.Time.Clock.App.Service.Pages.TimeClockReports
{
    public class TimeClockPrintHoursWorkedViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(TimeClockConst.JsonKeyEdocsTimeClokResApi); } }
        public string TotalHrsWorked
        { get; set; }
        public IList<TimeClockEntriesModel> TimeClockHrs
        { get; set; }
        public string EmpID
        { get; set; }
        public DateTime TimeCardStDate
        { get; set; }
        public DateTime TimeCardEndDate
        { get; set; }
        public TimeClockPrintHoursWorkedViewModel(IConfiguration config, IHttpClientFactory factoryClient)
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
        public async Task<IActionResult> OnGetAsync(string empId, DateTime timeClockStartDate, DateTime timeClockEndDate)
        {
            string edocsAdmin = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, TimeClockConst.SessionEmpID).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(edocsAdmin)))
                edocsAdmin = edocsAdmin.Replace(TimeClockConst.DoubleQuotes, "").Trim();
            if (string.IsNullOrWhiteSpace(edocsAdmin))
            {
                return Redirect($"/ClockInOut/ClockInOutView");
            }
            TimeCardStDate = timeClockStartDate;
            TimeCardEndDate = timeClockEndDate;
            if (string.Compare(timeClockStartDate.ToString("MM-dd-yyyy"), timeClockEndDate.ToString("MM-dd-yyyy"), true) == 0)
                timeClockEndDate = DateTime.Now.AddDays(1); ;
            ViewData["EdocsAdmin"] = "1";
            ViewData["EmpID"] = edocsAdmin;
            EmpID = empId;
            TimeClockHrs = WebApis.EdocsEmpsApi.ClockEmpHrs(empId, timeClockStartDate, timeClockEndDate, TimeClockConst.SpsTimeClockTimeWorkedReports, WebApiUrl, TimeClockConst.ControllerTimeClockReports, clientFactory).ConfigureAwait(false).GetAwaiter().GetResult();
            GetTotalHrs().ConfigureAwait(false).GetAwaiter().GetResult();
           // if(TimeClockHrs.Count > 0)
              //  return Redirect($"/ClockInOut/ClockInOutView");
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
