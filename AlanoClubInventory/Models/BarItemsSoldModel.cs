using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class BarItemsSoldModel
    {
        public DateTime Date { get; set; }
        public int ID { get; set; }
       public int BarITtemsTotal { get; set; }
       public float BarITtemsCost { get; set; }
        public bool BarItem { get; set; }
    }
}
