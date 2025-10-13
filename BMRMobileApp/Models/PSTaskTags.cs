using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
 public   class PSTaskTags: IToDoTaskTags
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
      public  string TagName { get; set; }
        [NotNull]
        public string TagColor { get; set; }
    }
}
