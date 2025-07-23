using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Threading.Tasks;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
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
       
        public int NumberDocumentsReceived
        { get; set; }
    }
}
 
