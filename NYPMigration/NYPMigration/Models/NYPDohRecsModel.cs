using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;
namespace NYPMigration.Models
{
    class NYPDohRecsModel: INYPDOH
    {
       public int ID { get; set; }
       public DateTime ScanDate { get; set; }
       public string FileName { get; set; }
       public string FileUrl { get; set; }

       public string BatchID { get; set; }
       public string AccessionNumber
        { get; set; }
       public string DrID
        { get; set; }
       public DateTime DateUpload
        { get; set; }
       public string MRN { get; set; }
       public DateTime DateOfService { get; set; }
    }
}
