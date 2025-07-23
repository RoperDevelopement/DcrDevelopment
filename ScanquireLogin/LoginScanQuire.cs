using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScanquireLogin
{
    public class LoginScanQuire
    {
        public bool IsAdmin
        { get; set; }

        public bool LoggedOn
        { get; set; }
        public bool Cancled
        { get; set; }

        public string UserName
        { get; set; }

        public int AddEditUsersTabIndex
        { get; set; }
        public void ShowFormLogin()
        {
            IsAdmin = false;
            LogInForm logInForm = new LogInForm();
            logInForm.ShowDialog();
            IsAdmin = logInForm.IsAdmin;
            UserName = logInForm.UserName;
            Cancled = logInForm.UserCanceled;
        }

        public void ShowAddUser()
        {
            AddEditUsers addEditUsers = new AddEditUsers();
            addEditUsers.TabIndexPage = AddEditUsersTabIndex;
            addEditUsers.ShowDialog();
        }
        public void ShowResetPassword()
        {
            ResetPassWord resetPassWord = new ResetPassWord();
            if (!(string.IsNullOrWhiteSpace(UserName)))
                resetPassWord.UserId = UserName;
            resetPassWord.ShowDialog();
        }
    }

}
