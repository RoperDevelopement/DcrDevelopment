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
    public class InvoicesModel:IInvoices
    {
      public  string FileUrl
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

        [Display(Name = "Starts With")]
        public bool SearchPartial
        { get; set; }
        [Display(Name = "Invoice Department:")]
        public string Department
        { get; set; }

        [Display(Name = "Invoice Category:")]
        public string Category
        { get; set; }
        public DateTime InvoiceDate
        { get; set; }

        [Display(Name = "Invoice Account:")]
        public string Account
        { get; set; }

        [Display(Name = "Invoice Reference:")]
        public string Reference
        { get; set; }
    }
}
