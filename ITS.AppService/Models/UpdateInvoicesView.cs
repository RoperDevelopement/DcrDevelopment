using Edocs.ITS.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Models
{
    public class UpdateInvoicesView: IInvNumber, IInvoicePaid, IEdocsCustID
    {
       
       public int EdocsCustomerID
        { get; set; }
      public  string EdocsCustomerName { get; set; }
        public float TotalPaid { get; set; }
        public float TotalAmountPaid { get; set; }
        public float TotalInvoiceAmount { get; set; }
        public DateTime DateInvoicePaid { get; set; }
        public DateTime InvoiceStartDate { get; set; }
        public DateTime InvoiceEndDate { get; set; }
        public bool InvoicePaid { get; set; }
        public DateTime DateInvoiceSent { get; set; }


      public int InvoiceNum { get; set; }
    }
}
