using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BinMonitorAppService.Models
{
    public class EmailReportModel
    {
        public string EmailTo
        { get; set; }
        public string EmailCC
        { get; set; }
        [Display(Name = "Email Frequency hours:")]
        
        public int EmailFrequency
        { get; set; }
    }
}
