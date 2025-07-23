using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.OCR.FullText.PDF.Interfaces;
namespace Edocs.OCR.FullText.PDF.Models
{
    public class PDFFullTextModel:ILabReqsPDFFullText
    {
       public int ID
        { get; set; }
       public string PDFFullText
        { get; set; }
    }
}
