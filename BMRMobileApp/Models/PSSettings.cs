using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Models
{
 public   class PSSettings:IPSSettings
    {
        [AutoIncrement, PrimaryKey, Unique]
        public   int ID { get; set; }
        [NotNull]
      public  int ShowFeelingsPage { get; set; }
    }
}
