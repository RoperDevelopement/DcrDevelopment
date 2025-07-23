using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.OCR.FullText.PDF.Interfaces;
namespace Edocs.OCR.FullText.PDF.Models
{
    class LabReqsModel: ILabReqsPDFInfo
    {
       public int LabReqID
        { get; set; }
        public string FileUrl
        { get; set; }
        public string IndexNumber
        { get; set; }
        public string FinancialNumber
        { get; set; }
        public string RequisitionNumber
        { get; set; }
        public DateTime ScanDate
        { get; set; }
    }
}
