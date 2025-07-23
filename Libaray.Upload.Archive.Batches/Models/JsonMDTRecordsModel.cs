using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    class JsonMDTRecordsModel : IJsonMDTRecords
    {
        public string FileName
        { get; set; }
        public string DocumentType
        { get; set; }
        public string ProjectNumber
        { get; set; }
        public string ProjectName
        { get; set; }
        public int TotalScanned { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalType { get; set; }
        public int TotalOCR { get; set; }
        public string BoxNumber
        { get; set; }
    }
}
