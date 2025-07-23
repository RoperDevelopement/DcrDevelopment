using Edocs.ITS.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Models
{
    public class ITSTrackingIDModel: IIDITrackingID, IScanRecTotals
    {
     public   string TrackingID
        { get; set; }
        public int TotalScanned
        { get; set; }
        public int TotalRecordsUploaded
        { get; set; }
        public int TotalDocsOCR
        { get; set; }
        public int TotalCharTyped
        { get; set; }
    }
}
