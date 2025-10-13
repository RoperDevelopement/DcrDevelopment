using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class CategorySoldModel
    {
        public int ID { get; set; }
        public DateTime  Date { get; set; }
      public string  CategoryName { get; set; }
        //   ID = x.ID,
        //BarITtemsTotal = x.BarITtemsTotal,
       public float CateogoryITtemsCost { get; set; }
        public bool BarItem { get; set; }
    }
}
