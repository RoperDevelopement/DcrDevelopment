using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class PDFFullTextModel:ILabReqsPDFFullText
    {
       public int ID
        { get; set; }
       public string PDFFullText
        { get; set; }
    }
}
