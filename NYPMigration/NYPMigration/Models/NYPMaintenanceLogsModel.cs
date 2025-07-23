using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
   public class NYPMaintenanceLogsModel: INYPMaintenanceLog
    {
       public int ID { get; set; }
       public DateTime ScanDate { get; set; }
       public string FileName { get; set; }
       public string FileUrl { get; set; }

       public string BatchID { get; set; }
       public DateTime DateUpload
        { get; set; }
        public DateTime LogDate
        { get; set; }
       public int LogStationId
        { get; set; }
       public string Checksum
        { get; set; }
    }
}
