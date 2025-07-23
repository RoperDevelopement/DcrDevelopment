using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSUsersNamesModel: IEdocsITSUserName
    {
       public int EdocsCustomerID
        { get; set; }
        public string EdocsCustomerName { get; set; }
        
        public string UserName
        { get; set; }
       public int UserID
        { get; set; }
    }
}
