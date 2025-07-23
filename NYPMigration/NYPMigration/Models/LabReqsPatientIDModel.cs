using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
   public class LabReqsPatientIDModel:ILabReqsPatientID
    {
       public string PatientID
        { get; set; }
       public string PatientFirstName
        { get; set; }
       public string PatientLastName
        { get; set; }
       public string ClientCode
        { get; set; }
    }
}
