using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDT.OCT.Projects.Interfaces
{
    internal interface IID
    {
        int ID { get; set; }
    }
    internal interface IMDTOCRRecs : IID
    {
        int NumberDocsScanned
        { get; set; }
        int NumberDocsUploaded
        { get; set; }
        string FileName
        { get; set; }
    }
}
