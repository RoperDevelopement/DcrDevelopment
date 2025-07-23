using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;
namespace Edocs.Employees.Time.Clock.App.Service.Models
{
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-6.0
    public class EmpModel : IEmp, IEmpID, IEmpEmail, IEmpName, IEmpAdmin, IEmpActive
    {
        [Display(Name = "Employee ID:")]
        [Required(ErrorMessage = "Employee ID is required")]
        public string EmpID
        { get; set; }
        [Display(Name = "Email Address:")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Address ID is required")]
        public string EmpEmailAddress { get; set; }
        [Display(Name = "First Name:")]
        public string EmpFirstName
        { get; set; }
        [Display(Name = "Last Name:")]
        public string EmpLastName { get; set; }
        [Display(Name = "Address:")]
        public string EmpAddress { get; set; }
        [Display(Name = "City:")]
        public string EmpCity { get; set; }
        [Display(Name = "State:")]
        public string EmpState { get; set; }
        [Display(Name = "Cell Phone Number:")]
        [DataType(DataType.PhoneNumber)]
        public string EmpCellPhone { get; set; }
        [Display(Name = "Home Phone Number:")]
        [DataType(DataType.PhoneNumber)]
        public string EmpHomePhone { get; set; }
        [Display(Name = "Pay Rate:")]
        [DataType(DataType.Currency)]
        public float EmpPayRate { get; set; }
        [Display(Name = "Holidya Pay Rate:")]
        [DataType(DataType.Currency)]
        public float EmpHolidayPayRate { get; set; }
        [Display(Name = "Federal Tax %:")]
        [DataType(DataType.Currency)]
        public float FedTaxesPercent { get; set; }
        [Display(Name = "State Tax %:")]
        [DataType(DataType.Currency)]
        public float StateTaxPercent { get; set; }
        [Display(Name = "Start Date:")]
        [DataType(DataType.DateTime)]
        public DateTime EmpStartDate { get; set; }
        [Display(Name = "Ter Date:")]
        [DataType(DataType.DateTime)]
        public DateTime EmpTerminationDate { get; set; }
        [Display(Name = "Commetns:")]
        public string Comments { get; set; }
        [Display(Name = "System Admin:")]
        public bool EdocsAdmin { get; set; }
        [Display(Name = "Zip Code:")]
        [DataType(DataType.PostalCode)]
        public string EmpZipCode { get; set; }
        public bool EmpActive { get; set; }

    }
}
