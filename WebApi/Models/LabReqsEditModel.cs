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
        public string IndexNumber
        { get; set; }

        public string FinancialNumber
        { get; set; }

        public string DrCode
        { get; set; }
        public string PatientID
        { get; set; }
        public string ClientID
        { get; set; }
        public string RequisitionNumber
        { get; set; }
        public string ClientCode
        { get; set; }
        public DateTime DateOfService
        { get; set; }

        public DateTime ReceiptDate
        { get; set; }

        public int Merged
        { get; set; }

        public string PatientFName
        { get; set; }
        public string PatientLName
        { get; set; }

        public string DrFName
        { get; set; }
        public string DrLName
        { get; set; }
        public string MRN
        { get; set; }
        public string FileUrl
        { get; set; }

        public string ScanBatch
        { get; set; }


        public DateTime DateUpload
        { get; set; }

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
