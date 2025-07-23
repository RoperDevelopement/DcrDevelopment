using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSCustomersModel: IEdocsITSCustomers
    {
        [Display(Name = "Customer ID:")]
        public    int EdocsCustomerID
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
        [Display(Name = "First Name:")]
        public string EdosCustomerFirstName { get; set; }
        [Display(Name = "Last Name:")]
        public string EdocsCustomerLastName { get; set; }
        [Display(Name = "Email Address:")]
        public string EdocsCustomerEmailAddress { get; set; }
        [Display(Name = "Phone Number:")]
        public string EdocsCustomerPhoneNumber { get; set; }
        [Display(Name = "Cell Phone Number:")]
        public string EdocsCustomerCellPhoneNumber { get; set; }

        public string EdocsCustomerModifyBy { get; set; }
        public DateTime EdocsCustomerDateAdded { get; set; }
        public DateTime EdocsCustomerDateModify { get; set; }

        [Display(Name = "Notes:")]
        public string EdocsCustomerNotes { get; set; }
        public bool Active
        { get; set; }

        [Display(Name = "Cost to Store Documents Per Month:")]
        public  float PriceStoreByMonth
        { get; set; }
        [Display(Name = "Cost Per Standard Document to Scan:")]
        public float PricePerDocument
        { get; set; }
        [Display(Name = "Cost Per Large Document to Scan:")]
       public float PricePerLargeDocuments
        { get; set; }
        [Display(Name = "Store Documents:")]
        public bool StoreDocuments
        { get; set; }
        
       public int StorageYears
        { get; set; }
       public int StorageDays
        { get; set; }
       public int StorageMonths
        { get; set; }
        [Display(Name = "Setup Fee for Scanning:")]
        public float PriceSetUpFee
        { get; set; }
        [Display(Name = "Cost Per Document to OCR:")]
        public float PriceOcr
        { get; set; }
        [Display(Name = "Cost Per Typed Character:")]
      public  float PricePerChar
        { get; set; }
        public string ImgStr
        { get; set; }
    }
}
