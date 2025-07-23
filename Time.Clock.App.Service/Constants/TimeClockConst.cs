using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Employees.Time.Clock.App.Service.Constants
{
    public class TimeClockConst
    {
        public const string SessionEdocsAdmin = "EdocsAdmin";
        public const string DoubleQuotes = "\"";
        public const string SessionEmpID = "EdocsAdmin";
        public const string SessionKeyUserProfile = "UserProfile";
        public const string JsonKeyEdocsTimeClokResApi = "EdocsTimeClokResApi";
        public const string ControllerAddEditTimeClockUsers = "TimeCLockAddEditEmp/";
        public const string ControllerTimeClockInOut = "TimeClockInOut/";
        public const string ControllerTimeClockLogIn = "TimeClockLogIn/";
        public const string ControllerTimeClockReports = "TimeClockReports/";
        public const string SpsTimeClockTimeWorkedReports = "sp_TimeClockTimeWorkedReport";
        public const string SpTimeClockUdateHoursWorked = "sp_TimeClockUdateHoursWorked";
       
        public const string ErrorMessage = "Error:";
        public const string AdminMessage = "Admin";
 
        public static async Task<IList<string>> GetStatesABB()
        {
            IList<string> lStateABB = new List<string>();
            lStateABB.Add("AK");
            lStateABB.Add("AZ");
            lStateABB.Add("AR");
            lStateABB.Add("CA");
            lStateABB.Add("CO");
            lStateABB.Add("CT");
            lStateABB.Add("DE");
            lStateABB.Add("FL");
            lStateABB.Add("GA");
            lStateABB.Add("HI");
            lStateABB.Add("ID");
            lStateABB.Add("IL");
            lStateABB.Add("IN");
            lStateABB.Add("IA");
            lStateABB.Add("KS");
            lStateABB.Add("KY");
            lStateABB.Add("LA");
            lStateABB.Add("ME");
            lStateABB.Add("MD");
            lStateABB.Add("MA");
            lStateABB.Add("MI");
            lStateABB.Add("MN");
            lStateABB.Add("MS");
            lStateABB.Add("MO");
            lStateABB.Add("MT");
            lStateABB.Add("NE");
            lStateABB.Add("NV");
            lStateABB.Add("NH");
            lStateABB.Add("NJ");
            lStateABB.Add("NM");
            lStateABB.Add("NY");
            lStateABB.Add("NC");
            lStateABB.Add("ND");
            lStateABB.Add("OH");
            lStateABB.Add("OK");
            lStateABB.Add("OR");
            lStateABB.Add("PA");
            lStateABB.Add("RI");
            lStateABB.Add("SC");
            lStateABB.Add("SD");
            lStateABB.Add("TN");
            lStateABB.Add("TX");
            lStateABB.Add("UT");
            lStateABB.Add("VT");
            lStateABB.Add("VA");
            lStateABB.Add("WA");
            lStateABB.Add("WV");
            lStateABB.Add("WI");
            lStateABB.Add("WY");
            return lStateABB;
        }
    }
}
