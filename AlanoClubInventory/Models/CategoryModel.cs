using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
   public class CategoryModel: ICaegoryName, IID
    {
        public int ID { get; set; }
     public   string CategoryName { get; set; }
        public bool BarItem { get; set; }
    }
}
