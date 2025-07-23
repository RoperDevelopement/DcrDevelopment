using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    public class DOHRecordsModel : IDOH
    {
        public string City { get; set; }
        public string Church { get; set; }
        public string BookType { get; set; }
        public string DateRangeStartDate { get; set; }
        public string DateRangeEndDate { get; set; }
        public string BookFirstPage { get; set; }
        public string BookSecondPage { get; set; }
        public string ImgOrgFileName { get; set; }
        public string ImgFileName { get; set; }
        public string FileName { get; set; }
        
    }
}
