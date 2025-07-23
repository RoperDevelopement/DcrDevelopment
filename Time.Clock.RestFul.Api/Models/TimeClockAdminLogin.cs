using Edocs.Employees.Time.Clock.App.Service.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Edocs.Employees.Time.Clock.App.Service.Models
{
    public class TimeClockAdminLogin : IEmpID, IEmpPW
    {
        [Display(Name = "Emp ID:")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Emp ID Required")]
        public string EmpID
        { get; set; }
        [Display(Name = "Emp Password:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string EmpPW
        { get; set; }

    }
}
