using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class CustomersInvoicesModel: IEdocsCustID, IInvNumber
    {
      public  int InvoiceNum { get; set; }
        public int EdocsCustomerID
        { get; set; }
        public string EdocsCustomerName { get; set; }
    }
}
