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
    public class LabReqsModel: ILabReqs
    {
        
       // [Display(Name = "File Name")]
       // public  Guid FileName
       // { get; set; }

        // [Display(Name = "File Name")]
        // public  Guid FileName
        // { get; set; }
        public int LabReqID
        { get; set; }
        public string FileUrl
        { get; set; }

        [Display(Name = "Scan Batch ID")]
        public string ScanBatch
        { get; set; }

        [BindProperty]

        [DataType(DataType.Date)]
        [Display(Name = "Date Images Uploaded")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        public DateTime DateUpload
        { get; set; }

        [Display(Name = "Financial/Index #")]
        public string IndexNumber
        { get; set; }
        [Display(Name = "Financial/Index #")]
        public string FinancialNumber
        { get; set; }
        [Display(Name = "CSN Number")]
        public string CSNNumber
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        [Display(Name = "Images Date of Service")]
        public DateTime DateOfService
        { get; set; }

        [Display(Name = "Dr Code")]
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

        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        [Display(Name = "LabReq's Search Begin Date")]
        public DateTime ReceiptDate
        { get; set; }


        public string Category
        { get; set; }
        public string ScanOperator
        { get; set; }

        public string ScanMachine
        { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        [Display(Name = "LabReq's Search End Date")]
        public DateTime ScanDate
        { get; set; }

        public string FileExtension
        { get; set; }

        public int BeingReviewed
        { get; set; }
        public int Merged
        { get; set; }
        public DateTime DateModify
        { get; set; }
        public string ModifyBy
        { get; set; }

        public bool SearchPartial
        { get; set; } = false;
      

        public string PatientName
        { get; set; }



       public string DrName
        { get; set; }
        public string MRN
        { get; set; }

    }
}
