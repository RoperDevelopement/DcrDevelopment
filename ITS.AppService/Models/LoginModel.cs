using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
namespace Edocs.ITS.AppService.Models
{
    public class LoginModel : IUserLogin
    {
         [Display(Name = "UserName:")]
        public string UserName
        { get; set; }
         [DataType(DataType.Password)]
        [Display(Name = "PassWord:")]
        public string Password
        { get; set; }
    }
}
