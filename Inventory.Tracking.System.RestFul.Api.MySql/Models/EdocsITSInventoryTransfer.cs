using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSInventoryTransfer: IInventroyTransfer
    {
        public string EdocsCustomerName
        { get; set; }
        public string TrackingID
        { get; set; }
        public string UserName
        { get; set; }
        public DateTime DateSent
        { get; set; }

        public int NumberDocsSent
        { get; set; }
        public string ScanType
        { get; set; }

        public string DeliveryMethod
        { get; set; }
    }
}
