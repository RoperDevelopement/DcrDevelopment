using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
   public class NYPSendOutResultsPerformingLabCodesModel: INYPSendOutResultsPerformingLabCodes
    {
       public int Id
        { get; set; }
       public string PerformingLabCode
        { get; set; }
    }
}
