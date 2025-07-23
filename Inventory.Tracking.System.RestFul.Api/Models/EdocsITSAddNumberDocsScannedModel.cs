using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;

namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSAddNumberDocsScannedModel : IEdocsITSDocsScannedTrackingID
    {
        public int TotalScanned
        { get; set; }
        public int TotalRecordsUploaded
        { get; set; }
        public string SpAddITSDocsScanned
        { get; set; }
        public string TrackingID
        { get; set; }
      public  int TotalDocsOCR
        { get; set; }
       public int TotalCharTyped
        { get; set; }
       public float TotalOcrCost
        { get; set; }
       public float TotalCostPerDoc
        { get; set; }
       public float TotalCostPerChar
        { get; set; }
    }
}
