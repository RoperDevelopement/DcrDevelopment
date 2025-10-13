using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
  public  class AlanoCLubMontlyProductsSoldModel
    {
       public int Month { get; set; }
         public int TotalMemBerProductsSold { get; set; }
        public int TotalNonMemBerProductsSold { get; set; }
        public float TotalMemBerPrice { get; set; }
        public float TotalNonMemBerPrice { get; set; }
        public string ProductName { get; set; }
    }
}
