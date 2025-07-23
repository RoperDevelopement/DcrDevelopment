using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string UserPW
        { get; set; }
        public string EmailAddress
        { get; set; }
        public string UserProfile
        { get; set; }
        public string DiplayName
        { get; set; }
        public string Id
        { get; set; }
    }
}
