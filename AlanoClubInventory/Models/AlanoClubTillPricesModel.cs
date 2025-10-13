using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class AlanoClubTillPricesModel : IID, IAlanoClubPrices, IItesSold, IAlanoClubProductName, IBarItem, IDateCreated, IDailyTill
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public float ClubPrice { get; set; }
        public float ClubNonMemberPrice { get; set; }
        public bool BarItem { get; set; }
        public int TotalMemberSold { get; set; }
        public int TotalNonMemberSold
        {
            get; set;
        }
        public DateTime DateCreated { get; set; }
        public float DailyProductTotal { get; set; }
        public float DailyTotal { get; set; }
      public  float DailyTillTotal { get; set; }
        
    }
}
