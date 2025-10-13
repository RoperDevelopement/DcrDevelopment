using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlanoClubInventory.Models
{
    public class AlanoClubReportModel: IID, ICaegoryName, IAlanoClubPrices, IAlanoClubProductName, IBarItem, IDateCreated, IItesSold , IDailyTill, IIDCategory
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public float ClubPrice { get; set; }
        public float ClubNonMemberPrice { get; set; }
        public string ProductName { get; set; }
        public bool BarItem { get; set; }

       public int CategoryID { get; set; }
        public  int TotalMemberSold { get; set; }
       public int TotalNonMemberSold { get; set; }
    // public   float DailyTotal { get; set; }
    //   public float DailyTillTotal { get; set; }
      public  float DailyProductTotal { get; set; }
      public  float DailyTotal { get; set; }
       public float DailyTillTotal { get; set; }
        public DateTime DateCreated { get; set; }
      public float  MemBerPriceTotal { get; set; }
	public float NonMemBerPriceTotal { get; set; }

    }
}
