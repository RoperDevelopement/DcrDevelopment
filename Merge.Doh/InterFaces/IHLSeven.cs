using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdocsUSA.Merge.Doh.Interfaces
{
    public interface IHlSeven
    {
        DateTime DateOfService { get; set; }
        string PatientID { get; set; }
        string MRN { get; set; }
        
        string PatientLastName { get; set; }
        string PatientFirstName { get; set; }
        string DrCode { get; set; }
        string DrFName { get; set; }
        string DrLName { get; set; }
        string AccessionNumber { get; set; }
         string ClientCode { get; set; }



    }

    public interface IDOHRecs
    {
        DateTime DateOfService { get; set; }
         
      
        string AccessionNumber { get; set; }
        
        string MRN { get; set; }
        int DOHID
        { get; set; }

    }


}
