using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeDemographicsCloud.Interfaces;
namespace MergeDemographicsCloud.Models
{
   public class LabRecsModel : ILabRecs
    {
        public DateTime DateOfService { get; set; }
        public string PatientID { get; set; }
        public string ClientCode { get; set; }
        public string RequisitionNumber { get; set; }
        public string IndexNumber { get; set; }
       public string FileName { get; set; }
       public string IndexnumberNoZeros { get; set; }
        public string FinancialNumber { get; set; }
        public string MRN { get; set; }
       public int LabReqID
        { get; set; }
    }

    public class HL7Model:IHlSeven
    {
       public DateTime DateOfService { get; set; }
        public string PatientID { get; set; }
        public string ClientCode { get; set; }
        public string FinancialNumber { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string DrCode { get; set; }
        public string DrFName { get; set; }
        public string DrLName { get; set; }
        public string RequisitionNumber { get; set; }
        public int HL7RowID 
        { get; set; }
    }
}
