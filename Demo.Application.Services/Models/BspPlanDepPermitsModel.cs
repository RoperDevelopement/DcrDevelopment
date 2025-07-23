using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
  using System.ComponentModel.DataAnnotations;
namespace Edocs.Demo.Application.Services.Models
{
    public class BspPlanDepPermitsModel
    {
        [Display(Name = "Permit Number:")]
        public int PermitNum
        { get; set; }
        public string FileName
        { get; set; }
        public string FileUrl
        { get; set; }
        [Display(Name = "Owner Lot:")]
        public string OwnerLot
        { get; set; }

        [Display(Name = "Const Company:")]
        public string ConstCompany
        { get; set; }
        [Display(Name = "Parcel Number:")]
        public int ParcelNumber
        { get; set; }
        [Display(Name = "Zone Number:")]
        public int ZoneNumber
        { get; set; }
        [Display(Name = "Go Code:")]
        public string GoCode
        { get; set; }
        [Display(Name = "Exe Permit Number:")]
        public int ExePermitNumber
        { get; set; }
        [Display(Name = "Address:")]
        public string Address
        { get; set; }
        public DateTime DateIssued
        { get; set; }
        public DateTime DateExperied
        { get; set; }
    }
}
