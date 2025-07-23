using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class InvoiceModel:IInvoice
    {
        public int EdocsCustomerID
        { get; set; }
        public  DateTime InvoiceStartDate { get; set; }
      public  DateTime InvoiceEndDate { get; set; }
     public   float InvoiceTotalAmount { get; set; }
      public  string FileName { get; set; }
    }
    public class InvoiceNum
    {
        public string NumberInvoice
        { get; set; }
    }
}
