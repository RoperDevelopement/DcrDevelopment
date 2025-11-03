using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class AlanoClubPricesModel: IID, ICaegoryName, IAlanoClubProductName, IBarItem, IAlanoClubPrices
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
       public string ProductName { get; set; }

        public bool BarItem { get; set; }
        public  float ClubPrice { get; set; }
      public  float ClubNonMemberPrice { get; set; }
        
    }
}
