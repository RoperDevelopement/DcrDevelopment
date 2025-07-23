using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NYPMigration.Interfaces;

namespace NYPMigration.Models
{
    public class NYPFullTextIndexModel : INYPRecordsID
    {
        public int ID { get; set; }
        public DateTime ScanDate { get; set; }
        public string PDFLabReqsFullText { get; set; }
    }
}
