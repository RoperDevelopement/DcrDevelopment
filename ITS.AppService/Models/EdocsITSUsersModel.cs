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
        [Display(Name = "Last Name:")]
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
      //  [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        
        public string Password
        { get; set; }
        [Display(Name = "Email Address:")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
          [EmailAddress(ErrorMessage = "Invalid Email Address")]
       // [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]


        public string EmailAddress
        { get; set; }
        [Display(Name = "Cell Phone Number:")]
        public string CellPhoneNumber
        { get; set; }
        public int UserID
        { get; set; }
     public   bool IsUserActive
        { get; set; }
        [Display(Name = "New Passwword:")]
        public string NewPassword
        { get; set; }
    }
}
