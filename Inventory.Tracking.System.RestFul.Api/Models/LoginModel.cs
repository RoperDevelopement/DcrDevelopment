using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;

namespace Edocs.ITS.AppService.Models
{
    public class LoginModel : IUserLogin
    {
         
        public string UserName
        { get; set; }
        
        public string Password
        { get; set; }
    }
}
