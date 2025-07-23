using Edocs.ITS.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Models
{
     
    public class PSUSDDepartmentModel : IPSUDDepartment
    {
        public string Department { get; set; }
    }
}
