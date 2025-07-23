using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
  public  class NYPDrCodesModel: INYPDrCodes
    {
       public string DrCode
        { get; set; }
        public string DrFirstName
        { get; set; }
        public string DrLastName
        { get; set; }
    }
}
