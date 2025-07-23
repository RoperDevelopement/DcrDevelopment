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
    public class LabReqsEditModel : ILabReqsEdit
    {
        public int LabReqID
        { get; set; }
        [Display(Name = "IndexNumber:")]
        public string IndexNumber
        { get; set; }
        [Display(Name = "Financial/CSN Number:")]
        public string FinancialNumber
        { get; set; }

        [Display(Name = "DrCode/NPI#:")]
        public string DrCode
        { get; set; }
        [Display(Name = "Patient ID:")]
        public string PatientID
        { get; set; }
        [Display(Name = "Client ID:")]
        public string ClientID
        { get; set; }
        [Display(Name = "Requisition Number:")]
        public string RequisitionNumber
        { get; set; }
        [Display(Name = "Client Colde:")]
        public string ClientCode
        { get; set; }
        [Display(Name = "Date of Service:")]
        public DateTime DateOfService
        { get; set; }

        [Display(Name = "Scan Date:")]
        public DateTime ReceiptDate
        { get; set; }

        [Display(Name = "Merged:")]
        public int Merged
        { get; set; }

        [Display(Name = "Patient First Name")]
        public string PatientFName
        { get; set; }

        [Display(Name = "Patient Last Name")]
        public string PatientLName
        { get; set; }

        [Display(Name = "Dr First Name")]
        public string DrFName
        { get; set; }
        [Display(Name = "Dr Last Name")]
        public string DrLName
        { get; set; }
        [Display(Name = "MRN / Patient ID")]
        public string MRN
        { get; set; }
        public string FileUrl
        { get; set; }

        public string ScanBatch
        { get; set; }

        [Display(Name = "Date UpLoaded:")]
        public DateTime DateUpload
        { get; set; }

        [Display(Name = "Scan Operator")]
        public string ScanOperator
        { get; set; }

        public string ScanMachine
        { get; set; }

        [Display(Name = "Scan Date:")]
        public DateTime ScanDate
        { get; set; }

        public string FileExtension
        { get; set; }

        [Display(Name = "Date Modify:")]
        public DateTime DateModify
        { get; set; }

        [Display(Name = "Modify By;")]
        public string ModifyBy
        { get; set; }

        public bool SearchPartial
        { get; set; }
    }
}
