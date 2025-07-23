using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSCostScanningModel: ITSCoastRep
    {
      public  int ID
        { get; set; }
        public string EdocsCustomerName
        { get; set; }
        public string TrackingID
        { get; set; }
        public int Scanned
        { get; set; }
        public int Uploaded
        { get; set; }
        public int DocsOCR
        { get; set; }

        public float OcrCost
        { get; set; }

        public int CharTyped
        { get; set; }
        public float CostPerDoc
        { get; set; }
        public float CostPerChar
        { get; set; }

        public float PricePerDocument
        { get; set; }
        public float PriceOCR
        { get; set; }
        public float PriceCharTyped
        { get; set; }
         
        public string EmailAddress
        { get; set; }
       public DateTime ScannDate
        { get; set; }
        public string IDTracking
        { get; set; }

    }
}
