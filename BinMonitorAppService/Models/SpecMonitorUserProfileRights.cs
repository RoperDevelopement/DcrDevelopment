using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class SpecMonitorUserProfileRights : IUserProfRights
    {

        public string BinUserights
        { get; set; }
        public string ChangeUsersPasswords
        { get; set; }
        public string CreateUsers
        { get; set; }
        public string DeleteUsers
        { get; set; }
        public string EditUsers
        { get; set; }
        public string ManageUserProfiles
        { get; set; }
        public string RunReports
        { get; set; }
        public string TransFerBins
        { get; set; }
        public string TransFerCategories
        { get; set; }

        public string CreateNewProfiles
        { get; set; }

        public string EmailReports
            { get; set; }
        public string Categories
        { get; set; }


    }

}

