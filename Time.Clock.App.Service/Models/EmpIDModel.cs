using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;
namespace Edocs.Employees.Time.Clock.App.Service.Models
{
    public class EmpIDModel: IEmpID
    {
        public string EmpID
        { get; set; }
    }
}
