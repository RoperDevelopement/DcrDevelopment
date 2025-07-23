using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.Inventory.Tracking.System.RestFul.Api.Models
{
    public class AddInvoiceNumberModel : IInvNumber, IEdocsCustID
    {
        public int InvoiceNum { get; set; }
        public int EdocsCustomerID
        { get; set; }
    }
}
