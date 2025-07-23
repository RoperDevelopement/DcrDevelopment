using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSCustomersModel : IEdocsITSCustomers
    {
        [Display(Name = "Customer ID:")]
        public int EdocsCustomerID
        { get; set; }

        [Display(Name = "Name:")]
        public string EdocsCustomerName { get; set; }

        [Display(Name = "Address:")]
        public string EdocsCustomerAddress { get; set; }

        [Display(Name = "City:")]
        public string EdocsCustomerCity { get; set; }

        [Display(Name = "State:")]
        public string EdosCustomerState { get; set; }

        [Display(Name = "Zip Code:")]
        public string EdocsCustomerZipCode { get; set; }
        [Display(Name = "Contact First Name:")]
        public string EdosCustomerFirstName { get; set; }
        [Display(Name = "Contact Last Name:")]
        public string EdocsCustomerLastName { get; set; }
        [Display(Name = "Contact Email Address:")]
        public string EdocsCustomerEmailAddress { get; set; }
        [Display(Name = "Contact Phone Number:")]
        public string EdocsCustomerPhoneNumber { get; set; }
        [Display(Name = "Contact Cell Phone Number:")]
        public string EdocsCustomerCellPhoneNumber { get; set; }

        public string EdocsCustomerModifyBy { get; set; }
        public DateTime EdocsCustomerDateAdded { get; set; }
        public DateTime EdocsCustomerDateModify { get; set; }

        [Display(Name = "Notes:")]
        public string EdocsCustomerNotes { get; set; }
        public bool Active
        { get; set; }
        public float PriceStoreByMonth
        { get; set; }
        public float PricePerDocument
        { get; set; }
        public bool StoreDocuments
        { get; set; }


        public int StorageYears
        { get; set; }
        public int StorageDays
        { get; set; }
        public int StorageMonths
        { get; set; }
        public float PriceSetUpFee
        { get; set; }
        public float PriceOcr
        { get; set; }
        public float PricePerChar
            
        { get; set; }
        public float PricePerLargeDocuments
        { get; set; }
        public string ImgStr
        { get; set; }
    }
}
