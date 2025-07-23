using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinMonitorAppService.Models
{
    public class SpectrumMonitorSettings
    {

        public int UpDateBmScreenSeconds
        { get; set; }
        
        public int BarCodesTimerSeconds
        { get; set; }
        
        public int DaysToKeepLogFiles
        { get; set; }
        public int NumDaysCheckForUpDates
        { get; set; }
        public string UpDateFolder
        { get; set; }
        public string WorkingFolder
        { get; set; }
        public string NotUploadFolder
        { get; set; }
        public bool RestartNightly
        { get; set; }
        public bool RunOnWeeksEnds
        { get; set; }
        public int StartTime
        { get; set; }
        public int EndTime
        { get; set; }
        public string WebApi
        { get; set; }
        public string AzureCloudUpdateFolder
        { get; set; }

        public bool UpdateConfigFile
        { get; set; }
        public bool AppTopWindow
        { get; set; }
       
        
        public string EmailPw
        { get; set; }

        public int SessionTimeOutMin
        { get; set; }

        public string SpectrumMonitorAppServiceUrl
        { get; set; }

        public string DefaultWebBrowser
        { get; set; }
    }
}
