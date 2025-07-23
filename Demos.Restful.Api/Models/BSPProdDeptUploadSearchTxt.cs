using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Demos.Restful.Api.Interfaces;
namespace Edocs.Demos.Restful.Api.Models
{
    public class BSPProdDeptUploadSearchTxt : IUploadSearchTxt
    {
       public int PermitNumber
        { get; set; }
       public string SearchStr
        { get; set; }
    }
}
