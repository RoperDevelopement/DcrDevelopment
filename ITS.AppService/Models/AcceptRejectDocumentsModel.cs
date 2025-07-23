using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class AcceptRejectDocumentsModel: IFileName,IID, IAcceptRecDoc
    {
        public int EdocsCustomerID
        { get; set; }
       public int ID
        { get; set; }

         public string TrackingID
        { get; set; }
        public  string FileName { get; set; }
        public string Comments { get; set; }
        public bool AcceptRejectDoc
        { get; set; }
    }
}
