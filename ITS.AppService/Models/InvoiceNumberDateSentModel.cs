using Edocs.ITS.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Models
{
    public class InvoiceNumberDateSentModel : IInvNumber, IInvDateSent
    {

        public int InvoiceNum { get; set; }

        public DateTime DateInvoiceSent


        { get; set; }

    }
}
