using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITCustomerIDNameModel:IEdocsCustID
    {
        public int EdocsCustomerID
        { get; set; }
       public string EdocsCustomerName { get; set; }
    }
}
