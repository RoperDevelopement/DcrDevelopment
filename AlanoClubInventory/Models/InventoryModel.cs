using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
    public class InventoryModel : IID, IInventoryProductName, ICaegoryName, IInventoryQuanity
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        //  public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        // public int TotalSold { get; set; }
    }
}
