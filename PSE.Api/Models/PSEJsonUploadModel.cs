using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.PSE.Api.Models
{
    public class PSEJsonUploadModel
    {
        public int TotalScanned
        { get; set; }
        public DateTime StartYear
        { get; set; }
        public DateTime EndYear
        { get; set; }
        public string FinancialCaterogyName
        { get; set; }
        public int TotalRecords
        { get; set; }

        public string StudentFirstName
        { get; set; }

        public string StudentLastName
        { get; set; }

        public DateTime StudentDateOfBirth
        { get; set; }
        public Guid ScanBatch
        { get; set; }
        public DateTime ScanDate
        { get; set; }
        public string ScanOperator
        { get; set; }

        public string ScanMachine
        { get; set; }

        public string FileName
        { get; set; }

        public string FileUrl
        { get; set; }

        public string AzureSPName
        { get; set; }
        public string DocumentType
        { get; set; }
    }
}
