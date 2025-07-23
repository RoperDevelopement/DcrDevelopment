using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;

namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class EmployeeComplianceLogsModel:IEmployeeComplianceLogs
    {
        [Display(Name = "Employee First Name:")]
        public string FirstName
        { get; set; }
        [Display(Name = "Employee Last Name:")]
        public string LastName
        { get; set; }
        [Display(Name = "Employee ID Number:")]
        public string EmpIdNumber
        { get; set; }

        [Display(Name = "Employee Department:")]
        public string EmpDepartment
        { get; set; }

        [Display(Name = "Employee Job Title:")]
        public string EmpJobTitle
        { get; set; }

        [Display(Name = "Employee Document Type:")]
        public string EmpDocumentType
        { get; set; }
        public string FileUrl
        { get; set; }

        public string ScanBatch
        { get; set; }


        public DateTime DateUpload
        { get; set; }

        [Display(Name = "Scan Operator:")]
        public string ScanOperator
        { get; set; }

        public string ScanMachine
        { get; set; }

        public DateTime ScanDate
        { get; set; }

        public string FileExtension
        { get; set; }

        public DateTime DateModify
        { get; set; }

        public string ModifyBy
        { get; set; }

        public bool SearchPartial
        { get; set; }
    }
}
