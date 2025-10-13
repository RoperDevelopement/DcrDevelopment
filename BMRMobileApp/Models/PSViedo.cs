
using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMRMobileApp.InterFaces.IID;
namespace BMRMobileApp.Models
{
    public class PSViedo : IViedoServiceDB
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }

        [NotNull]
        public string VideoPath { get; set; }
        [NotNull]
        public string DateViedoTaken
        {
            get; set;
        }
        [NotNull]
        public string ViedoTitle {get;set;}
    }
}
