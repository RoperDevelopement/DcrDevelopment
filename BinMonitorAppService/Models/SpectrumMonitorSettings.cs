using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BinMonitorAppService.Models
{
    public class SpectrumMonitorSettings
    {
        [Display(Name = "Refresh BinMonitor Screen Seonds:")]
        public int UpDateBmScreenSeconds
        { get; set; }
        
        [Display(Name = "BinMonitor Bar Code Seconds:")]
        public int BarCodesTimerSeconds
        { get; set; }

        [Display(Name = "Days To Keep Log Files:")]
        public int DaysToKeepLogFiles
        { get; set; }

        [Display(Name = "Number of Days Check for Updates:")]
         public int NumDaysCheckForUpDates
        { get; set; }

        [Display(Name = "Update Folder:")]
        public string UpDateFolder
        { get; set; }
        [Display(Name = "BinMonitor Working Folder:")]
        public string WorkingFolder
        { get; set; }
        [Display(Name = "Error Folder:")]
        public string NotUploadFolder
        { get; set; }

        [Display(Name = "Restart BinMonitor Nightly?")]
        public bool RestartNightly
        { get; set; }

        [Display(Name = "Run BinMonitor on Weekends?")]
        public bool RunOnWeeksEnds
        { get; set; }

        [Display(Name = "BinMonitor Start Time hour:")]
        public int StartTime
        { get; set; }

        [Display(Name = "BinMonitor End Time hour:")]
        public int EndTime
        { get; set; }

        [Display(Name = "BinMonitor RestApi Url:")]
        public string WebApi
        { get; set; }
        [Display(Name = "Azure Cloud AccountName/Sharename:")]
        public string AzureCloudUpdateFolder
        { get; set; }
        [Display(Name = "Update BinMonitor AppConfig File?")]
        public bool UpdateConfigFile
        { get; set; }

        [Display(Name = "BinMonitor Always Top Window?")]
        public bool AppTopWindow
        { get; set; }

        [Display(Name = "Alert Sender Email Password:")]
        public string EmailPw
        { get; set; }

        [Display(Name = "BinMonitor Session TimeOut Minutes:")]
        public int SessionTimeOutMin
        { get; set; }
        [Display(Name = "Service Url:")]
        public string SpectrumMonitorAppServiceUrl
        { get; set; }
        [Display(Name = "Workstation Default Web Browser:")]
        public string DefaultWebBrowser
        { get; set; }

        [Display(Name = "Max LabReqs Per Bin:")]
        public int MaxLabReqsPerBin
        { get; set; }
        [Display(Name = "Hours Add Subtract Time:")]
        public int AddDateTime
        { get; set; }
    }
}
