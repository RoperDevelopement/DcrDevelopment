using Edocs.ITS.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Models
{
    public class PSUSDRecsordsModel: IPSUDDepartment, IPSUDOrginationDepartment, IPSUDDescriptionOfRecords, IPSUDMethOfFiling, IPSUDFirstName, IPSUDLastName, IPSUDDateOfBirth, IPSUDDateOfRecords,IID
    {
        public int ID
        { get; set; }
        public string Department { get; set; }
       public string OrginationDepartment { get; set; }
        public string DescriptionOfRecords { get; set; }
        public string MethOfFiling { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
  public  string DateOfRecords { get; set; }
        public string TrackingID { get; set; }
    }
}
