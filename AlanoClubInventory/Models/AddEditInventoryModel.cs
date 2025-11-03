using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class AddEditInventoryModel : IID, IInventoryQuanity, IIDCategory, IInventoryProductName,IALanoCLubCurrentInventoryItemsByUnit, IAlanoClubInventoryItem
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
      //  public string CategoryName { get; set; }
        public string ProductName { get; set; }
      public  float   Price { get; set; }
        public int Quantity { get; set; }
       // public int TotalSold { get; set; }
       public int ItemsPerCase { get; set; }
        public bool InventoryItem {  get; set; }
    }
}
