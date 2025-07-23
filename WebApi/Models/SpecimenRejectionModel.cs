using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class SpecimenRejectionModel:ISpecimenRejection
    {
     public   string FileUrl
        { get; set; }

        public string ScanBatch
        { get; set; }


        public DateTime DateUpload
        { get; set; }

        [Display(Name = "Scan Operator Name: ")]
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

        [Display(Name = "Scan Batch ID contains: ")]
        
        public  bool SearchPartial
        { get; set; }

        public DateTime LogDate
        { get; set; }

        [Display(Name = "From Year: ")]
        public string FromYear
        { get; set; }
        [Display(Name = "From Number: ")]
        public string FromNumber
        { get; set; }

        public string FromReason
        { get; set; }

    }
}
