using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    
    public class EdocsITSUsersModel : IEdocsITSUser
    {
        [Display(Name = "Customer Name:")]
        public string EdocsCustomerName { get; set; }
        [Display(Name = "First Name:")]
        public string FirstName
        { get; set; }
        [Display(Name = "First Last:")]
        public string LastName
        { get; set; }
        [Display(Name = "Date Last Logged IN:")]
        public DateTime DateLastLogin
        { get; set; }

        [Display(Name = "Date Password Last Changed:")]
        public DateTime DatePasswordLastChanged
        { get; set; }

        [Display(Name = "Date Last MFA:")]
        public DateTime LastMFLA
        { get; set; }
        [Display(Name = "Site Admin:")]
        public bool IsCustomerAdmin
        { get; set; }
        [Display(Name = "e-Docs Admin:")]
        public bool IsEdocsAdmin
        { get; set; }
        [Display(Name = "UserName:")]
        public string UserName
        { get; set; }

       // [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        
        public string Password
        { get; set; }
      //  [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address:")]
        
        public string EmailAddress
        { get; set; }
        [Display(Name = "Cell Phone Number:")]
        public string CellPhoneNumber
        { get; set; }
        public int UserID
        { get; set; }
     public   bool IsUserActive
        { get; set; }
        public string NewPassword
        { get; set; }
    }
}
