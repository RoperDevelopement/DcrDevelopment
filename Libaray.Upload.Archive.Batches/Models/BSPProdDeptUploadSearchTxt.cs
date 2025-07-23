using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    public class BSPProdDeptUploadSearchTxt : IUploadSearchTxt
    {
       public int PermitNumber
        { get; set; }
       public string SearchStr
        { get; set; }
    }
}
