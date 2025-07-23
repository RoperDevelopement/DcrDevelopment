using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class UpdateProfilesModel: IUserSqlPermission
    {
       readonly string NamesRights = "Create New Profiles,Change Users Passwords,Create Users,Delete Users,Edit Users,Manage User Profiles,Run Reports,TransFer Bins,TransFer Categories";
     
     public   int ChangeUsersPasswords
        { get; set; } = 6;
        public int CreateUsers
        { get; set; } = 4;
        public int DeleteUsers
        { get; set; } = 7;


        public int EditUsers
        { get; set; } = 5;
        public int ManageUserProfiles
        { get; set; } = 8;
        public int CreateNewProfiles
        { get; set; } = 9;
        public int RunReports
        { get; set; } = 1;

        public int TransFerBins
        { get; set; } = 2;
        public int TransFerCategories
        { get; set; } = 3;
       public string ProfileName
        { get; set; } = "N/A";

        public int EmailReports
        { get; set; } = 10;
        public int Categories
        { get; set; } = 11;

    }
}
