using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Edocs.ITS.AppService.Pages.TrackingSystemReports
{
    public enum DropListChartTypes
    {
        [Display(Name = "bar")]
        bar,
        [Display(Name = "pie")]
        pie,
        [Display(Name = "line")]
        line

    }
    public enum DropListBinOptionOptions
    {

        [Display(Name = "CreatedBY")]
        CreatedBY,
        [Display(Name = "ProcessedBY")]
        ProcessedBy,
        [Display(Name = "ClosedBY")]
        ClosedBY

    }
    public enum DropListBinCwidRepOptions
    {
        [Display(Name = "All")]
        All,
        [Display(Name = "Register")]
        Register,
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "Closed")]
        Closed

    }
    public class ITSReportChartVIewModel : PageModel
{
        private readonly IConfiguration configuration;
        private string WebApiUrl
             
        
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        [BindProperty]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime UsageRepStDate
        { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime UsageRepEndDate
        { get; set; }
        [BindProperty]
        [Display(Name = "Bin Status")]
        [DataType(DataType.Text)]
        public DropListBinOptionOptions ReportType { get; set; }
        public DropListChartTypes TypeOfChart { get; set; }
      
        public void OnGet()
        {
        }
    }
}
