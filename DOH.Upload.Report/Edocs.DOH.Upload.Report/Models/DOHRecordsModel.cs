using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Edocs.DOH.Upload.Report.Models
{
    public class DOHRecordsModel  
    {
        public string City { get; set; }
        public string Church { get; set; }
        public string BookType { get; set; }
        public string DateRangeStartDate { get; set; }
        public string DateRangeEndDate { get; set; }
        public string BookFirstPage { get; set; }
        public string BookSecondPage { get; set; }
       
        public string ImgFileName { get; set; }
        public string FileName { get; set; }
        public string   DownLoadSubFolder
        { get; set; }
        public int ImagesScanned { get; set; }
        public int PDFFileID { get; set; }
        public DateTime DateAdded
        { get; set; }

    }
}
