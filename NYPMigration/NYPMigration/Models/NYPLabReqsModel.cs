using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
    public class NYPLabReqsModel : INYPRecordsID, INYPRecords, INYPLabReqs
    {
        public int ID { get; set; }
        public DateTime ScanDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string BatchID { get; set; }
        public string IndexNumber { get; set; }
        public string FinancialNumber { get; set; }
        public string MRN { get; set; }
        public DateTime DateOfService { get; set; }
        public string DrID { get; set; }
        public string PatientID { get; set; }
        public string ClientID { get; set; }
        public string RequisitionNumber { get; set; }
        public string ClientCode { get; set; }
    }
}
