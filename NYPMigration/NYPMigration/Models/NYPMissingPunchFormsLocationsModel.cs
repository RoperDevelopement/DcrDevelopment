using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
 public   class NYPMissingPunchFormsLocationsModel:INYPMissingPunchFormsLocation
    {
       public int ID
        { get; set; }
       public string PunchFormsLocation
        { get; set; }
    }
}
