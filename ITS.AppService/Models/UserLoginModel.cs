using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class UserLoginModel : IUserLoginInfo,IEdocsCustID
    {
        public string EmailAddress
        { get; set; }

        public string UserName
        { get; set; }
        public string Password
        { get; set; }
        public string CellPhoneNumber
        { get; set; }
        public DateTime LastMFLA
        { get; set; }
        public bool IsCustomerAdmin
        { get; set; }
        public bool IsEdocsAdmin
        { get; set; }
        public string EdocsCustomerName { get; set; }
        public string NewPassword
        { get; set; }
   public int EdocsCustomerID
    { get; set; }
}
}
