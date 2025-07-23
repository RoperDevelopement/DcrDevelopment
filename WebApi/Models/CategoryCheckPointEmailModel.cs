using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class CategoryCheckPointEmailModel:IBinCategory
    {
       public int CategoryID
        { get; set; }
       public string CategoryName
        { get; set; }

        public bool Flash
        { get; set; }

        [Display(Name = "Duration")]
        [DataType(DataType.Text)]
     //   [Range(typeof(TimeSpan), "00:00", "23:59")]
       //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "00:00")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00 to 23:59")]
        public string Duration
        { get; set; }
        public bool EmailAlerts
        { get; set; }

        public string EmailTo
        { get; set; }
    }
}
