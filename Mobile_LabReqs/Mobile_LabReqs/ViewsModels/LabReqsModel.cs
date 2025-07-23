using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mobile_LabReqs.InterFaces;

namespace Mobile_LabReqs.ViewsModels
{
    public class LabReqsModel: ILabReqs
    {

        // [Display(Name = "File Name")]
        // public  Guid FileName
        // { get; set; }
       public int LabReqID
        { get; set; }
        public string FileUrl
        { get; set; }

        
        public string ScanBatch
        { get; set; }

         
        public DateTime DateUpload
        { get; set; }

        
        public string IndexNumber
        { get; set; }
       
        public string FinancialNumber
        { get; set; }

        
        public DateTime DateOfService
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

         
        public DateTime ReceiptDate
        { get; set; }


        public string Category
        { get; set; }
        public string ScanOperator
        { get; set; }

        public string ScanMachine
        { get; set; }

       
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
