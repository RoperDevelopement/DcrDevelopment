using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdocsUSA.Merge.Doh.Interfaces;
namespace EdocsUSA.Merge.Doh.Models
{
   public class HL7Model:IHlSeven
    {
       public DateTime DateOfService { get; set; }
        public string PatientID { get; set; }
        public string MRN { get; set; }
        public string ClientCode { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string DrCode { get; set; }
        public string DrFName { get; set; }
        public string DrLName { get; set; }
        public  string AccessionNumber { get; set; }
    }
}
