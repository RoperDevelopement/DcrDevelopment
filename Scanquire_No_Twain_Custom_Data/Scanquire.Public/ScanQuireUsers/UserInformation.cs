using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EDL = EdocsUSA.Utilities.Logging;
using ScanquireLogin;
namespace Scanquire.Public.ScanQuireUsers
{
    public class ScanQuireUser
    {
        public static ScanQuireUser ScanQuireInstance = null;

        public bool IsAdmin
        { get; set; }
        public string ScanQuireUserLoggedIn
        { get; set; }

        public int InActiveTimeout
        { get; set; }
        public DateTime TimeIdle
        { get; set; }
        protected ScanQuireUser()
        { }
        static ScanQuireUser()
        {

            if (ScanQuireInstance == null)
            {
                ScanQuireInstance = new ScanQuireUser();
            }

        }

        public void Login()
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Opening form Log into scanquire");
            IsAdmin = false;
            ScanQuireUserLoggedIn = string.Empty;
            LoginScanQuire Login = new LoginScanQuire();
            Login.ShowFormLogin();
            IsAdmin = Login.IsAdmin;
            ScanQuireUserLoggedIn = Login.UserName;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Close form Log into scanquire user is admin:{IsAdmin.ToString()} user logged in:{ScanQuireUserLoggedIn}");
            if (!(string.IsNullOrWhiteSpace(ScanQuireUserLoggedIn)))
                TimeIdle = DateTime.Now;
        }

        public void AddUsers(int tabIndex)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opening form add edit users for tab index:{tabIndex.ToString()}");
            LoginScanQuire UsersAdd = new LoginScanQuire();
            UsersAdd.AddEditUsersTabIndex = tabIndex;
            UsersAdd.ShowAddUser();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closing form add edit users");
            TimeIdle = DateTime.Now;
            //AddUsers.ShowAddUser();

        }
        public void RestPassword()
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Opeing form change password");
            LoginScanQuire ResetPW = new LoginScanQuire();
            ResetPW.UserName = ScanQuireUserLoggedIn;
            ResetPW.ShowResetPassword();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Closing form change password");
            TimeIdle = DateTime.Now;
        }
        public bool CheckInaviteTimeOut()
        {
            if(InActiveTimeout > 0 )
            {
                TimeSpan ts =  DateTime.Now - TimeIdle;
                
                if (ts.TotalMinutes > Convert.ToDouble(InActiveTimeout))
                {
                    MessageBox.Show("Session expired", "Loggin", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return true;
                }
            }
            TimeIdle = DateTime.Now;
            return false;
        }
    }
}
