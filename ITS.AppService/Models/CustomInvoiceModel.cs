using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class CustomInvoiceModel: ICustomInvoice
    {
        public int EdocsCustomerID
        { get; set; }
        public string ItemDescription { get; set; }
         public int ItemQuantity { get; set; }
          public  float ItemCost { get; set; }
        public float ItemTotal { get; set; }
        public  DateTime DateofService { get; set; }

       
    }
}
