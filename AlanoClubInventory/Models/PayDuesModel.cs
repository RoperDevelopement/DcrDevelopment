using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
  public  class PayDuesModel: IDuesReceipt,IReceiptNumber
    {
       public int ReceiptNumber { get; set; }
        public  int ID {  get; set; }
        public int Quanity { get; set; }
        public string Description { get; set; }
        public DateTime DatePaid { get; set; }
       public float Price { get; set; }
       public float Amount { get; set; }
        public string RecivedBy { get; set; }
        public int NumberMonthsPayDues { get; set; }
    }
}
