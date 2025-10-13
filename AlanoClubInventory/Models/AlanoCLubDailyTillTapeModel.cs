using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
  public  class AlanoCLubDailyTillTapeModel:IID, IDateCreated, IDailyTapeTill
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public float DailyTillTotal { get; set; }
       public float DailyTillTape { get; set; }
       public float Depsoit { get; set; }

    }
}
