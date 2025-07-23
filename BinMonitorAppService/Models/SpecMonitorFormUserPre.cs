using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class SpecMonitorFormUserPre : IUserFormPermission
    {
       public string Cwid
        { get; set; }
        public bool Admin
        { get; set; }
        public bool ChangeUsersPasswords
        { get; set; }
        public bool CreateUsers
        { get; set; }
        public bool DeleteUsers
        { get; set; }
        public bool RunReports
        { get; set; }

        public bool EditUsers
        { get; set; }
        public bool ManageUserProfiles
        { get; set; }
        public bool CreateNewProfiles
        {
            get; set;
        }
         
        public bool TransFerBins
        { get; set; }
        public bool TransFerCategories
        { get; set; }

        public bool EmailReports
        { get; set; }
        public bool Categories
        { get; set; }
        public string ProfileName
        { get; set; }
    }
}
