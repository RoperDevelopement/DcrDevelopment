using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Models
{
    public class UsersModel: IBinUsers
    {
        public string Cwid
        { get; set; }
        public string FirstName
        { get; set; }
        public string LastName
        { get; set; }
        [DataType(DataType.Password)]
        public string UserPW
        { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
               public string EmailAddress
        { get; set; }
        public string UserProfile
        { get; set; }

       public DateTime DateLastLogin
        { get; set; }
       public DateTime DatePasswordLastChanged
        { get; set; }

        public string UserProfileName
        { get; set; }
    }
}
