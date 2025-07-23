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
    public class DohModel: ISendOutResults,IDrInformation, IDohAccession
    {
     public   int DOHID
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

       

        [Display(Name = "Accession #:")]
        public string AccessionNumber
        { get; set; }

        [Display(Name = "Medical Record Number:")]
        public string MRN
        { get; set; }

        public DateTime DateOFService
        { get; set; }
        [Display(Name = "Patient First Name:")]
        public string FirstName
        { get; set; }
        [Display(Name = "Patient Last Name:")]
        public string LastName
        { get; set; }
        public string FinancialNumber
        { get; set; }
     public   string DrCode
        { get; set; }
        [Display(Name = "Droctor First Name:")]
        public  string DrFName
        { get; set; }
        [Display(Name = "Droctor Last Name:")]
        public string DrLName
        { get; set; }
        public string PatientName
        { get; set; }
        public string DrName
        { get; set; }
    }
}
