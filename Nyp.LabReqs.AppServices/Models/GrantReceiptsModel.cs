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
    public class GrantReceiptsModel:IGranitReceipts
    {
       public string FileUrl
        { get; set; }
        [Display(Name = "Document Client Code:")]
        public string ClientCode
        { get; set; }
        public string Comments
        { get; set; }
        public DateTime DocumentDate
        { get; set; }
        public DateTime ScanDate
        { get; set; }
        [Display(Name = "Scan Operator:")]
        public string ScanOperator
        { get; set; }
        public string ScanBatchId
        { get; set; }

        public   bool SearchPartial
        { get; set; }

        public string ScanByDate
        { get; set; }
    }
}
