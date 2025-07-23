using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    public class EdocsITSTrackingIDModel
    {
       public string TrackingID
        { get; set; }
     public   int NumberDocsScanned
        { get; set; }
      public  int NumberDocsUploaded
        { get; set; }
    }
}
