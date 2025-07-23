using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
   public class NYPMaintenanceLogsLogStationsModel: INYPMaintenanceLogsLogStations
    {
       public int ID { get; set; }
       public string LogStation { get; set; }
    }
}
