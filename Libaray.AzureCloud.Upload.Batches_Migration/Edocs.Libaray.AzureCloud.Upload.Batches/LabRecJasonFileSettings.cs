using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    class LabRecJasonFileSettings
    {
     
        public string RequisitionNumber
        { get; set; }
        public string ClientCode
        { get; set; }
        public string PatientID
        { get; set; }
        public string IndexNumber
        { get; set; }
        public string FileName
        { get; set; }

        public string DrCode
        { get; set; }
        public string DrLName
        { get; set; }
        public string DrFName
        { get; set; }
        public string LastName
        { get; set; }
        public string FirstName
        { get; set; }
        public DateTime DateOfService
        {
            get; set;
        }
        public DateTime ReceiptDate
        {
            get; set;
        }
    }

}