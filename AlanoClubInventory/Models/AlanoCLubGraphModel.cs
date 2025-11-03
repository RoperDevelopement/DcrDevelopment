using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    
    public class AlanoCLubGraphModel : IDateCreated, IDailyProductTotal, ItemsSold
    {
        public float DailyProductTotal { get; set; }
        public int ItemsSold { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
