using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.OCR.FullText.PDF.Interfaces
{
    interface ILabReqsPDFInfo
    {
         

       int LabReqID
        { get; set; }
      string FileUrl
        { get; set; }
      string IndexNumber
        { get; set; }
      string FinancialNumber
        { get; set; }
      string  RequisitionNumber
        { get; set; }
      DateTime ScanDate
        { get; set; }
    }
    public interface ILabReqsPDFFullText
    {
        int ID
        { get; set; }
        string PDFFullText
        { get; set; }




    }
}
