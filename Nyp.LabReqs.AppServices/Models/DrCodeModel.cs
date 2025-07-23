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
    public class DrCodeModel:IDrCodes
    {
       public string FileUrl
        { get; set; }

        [Display(Name = "Scan Batch ID:")]
        public string ScanBatch
        { get; set; }
        [Display(Name = "Dr Code:")]
        public string DrCode
        { get; set; }
        [Display(Name = "Starts With:")]
        public bool SearchPartial
        { get; set; }
        [Display(Name = "Scan Operator:")]
        public string ScanOperator
        { get; set; }
        [Display(Name = "Dr First Name:")]
        public string DrFName
        { get; set; }
        [Display(Name = "Dr Last Name:")]
        public string DrLName
        { get; set; }

        public DateTime ScanDate
        { get; set; }
        public DateTime ScanEndDate
        { get; set; }
    }
}
