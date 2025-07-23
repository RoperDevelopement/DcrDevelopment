using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    public class UploadTrackingByProjectNameModel

    {
      public  string TrackingID
        { get; set; }
       public string UserName
        { get; set; }
      public  int NumberDocsScanned
        { get; set; }
        public int NumberTypedPerDoc
        { get; set; }
        public int NumberDocsUploaded
        { get; set; }
        public int NumberImagesSaved
        { get; set; }
        public int NumberDocOCR
        { get; set; }
      public  int StandardLargeDocument { get; set; }
        public string ScanBatchID
        { get; set; }
        public string ScanOperator
        { get; set; }
       public int EdocsCustomerID
        { get; set; }
      public  string ScanMachine
        { get; set; }
        public string FileName { get; set; }
    }
}
