using Edocs.Employees.Time.Clock.App.Service.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Edocs.Employee.Time.Clock.RestFul.Api.Constants
{
    public class EmpConsts
    {
        public const string SessionEdocsAdmin = "EdocsAdmin";
        public const string SessionKeyUserProfile = "UserProfile";
        public const string JsonKeyEdocsTimeClockCS = "EdocsTimeClockCS";

        #region sp psarams
        public const string SpParmaEmpID = "@EmpID";
        public const string SpParmaEmpFirstName = "@EmpFirstName";
        public const string SpParmaEmpLastName = "@EmpLastName";
        public const string SpParmaEmpAddress = "@EmpAddress";
        public const string SpParmaEmpCity = "@EmpCity";
        public const string SpParmaEmpState = "@EmpState";
        public const string SpParmaEmpEmailAddress = "@EmpEmailAddress";
        public const string SpParmaEmpCellPhone = "@EmpCellPhone";
        public const string SpParmaEmpHomePhone = "@EmpHomePhone";
        public const string SpParmaEmpPayRate = "@EmpPayRate";
        public const string SpParmaEmpHolidayPayRate = "@EmpHolidayPayRate";
        public const string SpParmaFedTaxesPercent = "@FedTaxesPercent";
        public const string SpParmaStateTaxPercent = "@StateTaxPercent";
        public const string SpParmaEmpStartDate = "@EmpStartDate";
        public const string SpParmaEmpTerminationDate = "@EmpTerminationDate";
        public const string SpParmaComments = "@Comments";
        public const string SpParmaEmpActive = "@EmpActive";
        public const string SpParmaEmpSiteAdmin = "@EmpSiteAdmin";
        public const string SpParmaLoginName = "@LoginName";
        public const string SpParmaPassWord = "@PassWord";
        public const string SpParmaClockInAdmin = "@ClockInAdmin";
        public const string SpParmaTimeClockWorkWeekStartDate  = "@TimeClockWorkWeekStartDate";
        public const string SpParmaTimeClockWorkWeekEndDate = "@TimeClockWorkWeekEndDate";
        public const string SpParmaClockInTime = "@ClockInTime";
        public const string SpParmaClockOutTime = "@ClockOutTime";
        public const string SpParmaID = "@ID";
        
        // pu//blic const string SpParmaTimeClockWorkWeekEndDate = "@TimeClockWorkWeekEndDate";


        #endregion
        #region store proc
        public const string SpAddUpdateTimeClockUsers = "sp_AddUpdateTimeClockUsers";
        public const string SpClockInOutTimeClock = "[dbo].[sp_ClockInOutTimeClock]";
        public const string SpTimeClockUserLogIn = "[dbo].[sp_TimeClockUserLogIn]";
        public const string SpTimeClockEmps = "sp_TimeClockEmps";
        public const string SpsTimeClockTimeWorkedReports = "sp_TimeClockTimeWorkedReport";
        public const string SpAddEmpTimeClockEntry = "[dbo].[sp_AddEmpTimeClockEntry]";
        public const string SpDelTimeClockEntry = "sp_DelTimeClockEntry";


        #endregion

    }
}
