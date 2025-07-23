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
    public class SendOutPackingSlipsModel:ISendOutPackingSlips
    {
       
        public DateTime DateOfService
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
