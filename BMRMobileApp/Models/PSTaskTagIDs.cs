using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
using SQLite;
namespace BMRMobileApp.Models
{
   public class PSTaskTagIDs: ITagID,IPSTablesIndexID,IPSIndexID
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int IndexID { get; set; }
        [NotNull]
        public int ID { get; set; }
        [NotNull]
        public int TagID { get; set; }
    }
}
