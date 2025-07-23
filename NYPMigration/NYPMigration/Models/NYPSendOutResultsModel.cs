using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
    public class NYPSendOutResultsModel : INYPSendOutResults
    {
        public int ID { get; set; }
        public DateTime ScanDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public string BatchID { get; set; }
        public DateTime DateUpload
        { get; set; }
        public string MRN { get; set; }
        public DateTime DateOfService { get; set; }
        public int PerformingLabCode
        { get; set; }
        public string AccessionNumber
        { get; set; }
        public string FinancialNumber
        { get; set; }
        public string LastName
        { get; set; }
        public string FirstName
        { get; set; }
    }

}
 