using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
   public class AlanoClubReportTotals
    {
        public float BarItemsTotal { get; set; } = 0.0f;
        public float NonBarItemsTotal { get; set; } = 0.0f;
        public float TotalSales { get; set; } = 0.0f;
       
        public float Dues { get; set; } = 0.0f;
        public float CoffeeClub { get; set; } = 0.0f;
        public float Coins { get; set; } = 0.0f;
        public float Donations { get; set; } = 0.0f;
        public float DailyTotalOther { get; set; } = 0.0f;
        
        public float Deposit { get; set; } = 0.0f;
        public float Tape { get; set; } = 0.0f;
        public float OverShort { get; set; } = 0.0f;
        public float MiscItems { get; set; } = 0.0f;
    }
}
