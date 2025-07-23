using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;

namespace Edocs.Employees.Time.Clock.App.Service.Models
{
    public class EmpClockInOutModel : IEmpID, IEmpClockInOut, IEmpName
    {
        public string EmpID
        { get; set; }

       
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
      public  string EmpFirstName
        { get; set; }
       public string EmpLastName { get; set; }
    }
}
