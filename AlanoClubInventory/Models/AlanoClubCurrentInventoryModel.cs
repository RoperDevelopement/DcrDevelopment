using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class AlanoClubCurrentInventoryModel : IALanoCLubCurrentInventory
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
       
       public int InStock { get; set; }
       public int ItemsSold { get; set; }
        public int   InventoryCurrent {  get; set; }
        public int NewCount { get; set; }
    }
}
