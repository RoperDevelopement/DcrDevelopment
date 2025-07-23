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
    public class SendOutResultsModel:ISendOutResults
    {
        [Display(Name = "First Name:")]
        public  string FirstName
        { get; set; }
        [Display(Name = "Last Name:")]
        public string LastName
        { get; set; }
        [Display(Name = "Performing Lab Code:")]
        public string PerformingLabCode
        { get; set; }

        [Display(Name = "Accession Number:")]
        public string AccessionNumber
        { get; set; }

        [Display(Name = "Medical Record Number:")]
        public string MRN
        { get; set; }

        public  DateTime DateOFService
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
        [Display(Name = "Finanical Number:")]
        public string FinancialNumber
        { get; set; }

        [Display(Name = "Search Partial")]
        public bool SearchPartial
        { get; set; }
    }
}
