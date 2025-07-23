using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSScanningManModel : IEdocsITSScanningManagement
    {
        public int IDTracking
        { get; set; }
        public string TrackingID
        { get; set; }
        public string EdocsCustomerName { get; set; }
        public string UserName
        { get; set; }
        public DateTime DateDocumentsReceived
        { get; set; }
        public DateTime DateScanningStarted
        { get; set; }
        [Display(Name = "Number Files in Box:")]
        public int NumberDocumentsReceived
        { get; set; }
    }
}
