using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
   public class DailyTillReceiptModel:IID, IAlanoClubPrices, IAlanoClubProductName, IDateCreated
    {
       public int ID { get; set; }
      public  float ClubPrice { get; set; }
       public float ClubNonMemberPrice { get; set; }
         public string ProductName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
