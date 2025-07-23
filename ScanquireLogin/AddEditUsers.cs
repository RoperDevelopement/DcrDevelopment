using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScanQuire_SendEmails;
using ScanQuireSqlCmds;
namespace ScanquireLogin
{
    public partial class AddEditUsers : Form
    {
        private readonly string BtnAdd = "&Add User";
        private readonly string BtnUpDate = "&UpDate User";
       // private readonly string BtnDel = "&Delete User";

        public int TabIndexPage
        {
            get; set;
        }
        private Dictionary<string, ScanQuireUsers> DicScanUserInfor
        { get; set; }
        public AddEditUsers()
        {

            InitializeComponent();


        }
        private void UpdateTabPage()
        {
            if (TabIndexPage == 0)
                scanquireUsersControl1.txtBoxFName.Focus();
            else
            {
                tabCntAddEditUsers.SelectTab(TabIndexPage);
                UpdateTabForm();

            }

        }
        private void MakeTxtReadOnly(ScanquireUsersControl scanquireUsersControl, bool readOnly)
        {
            scanquireUsersControl.txtBoxEmailAddress.ReadOnly = readOnly;
            scanquireUsersControl.txtBoxFName.ReadOnly = readOnly;
            scanquireUsersControl.txtBoxLName.ReadOnly = readOnly;
            scanquireUsersControl.txtBoxLoginId.ReadOnly = readOnly;
            if (readOnly)
                readOnly = false;
            else
                readOnly = true;
            scanquireUsersControl.chkBoxShowPassWord.Visible = readOnly;
            scanquireUsersControl.chkBoxIsAdmin.Visible = readOnly;
            scanquireUsersControl.chkBoxShowPassWord.Checked = false;

        }
        private void ClearTxtBoxes(ScanquireUsersControl scanquireUsersControl)
        {
            if (TabIndexPage != 1)
            {
                scanquireUsersControl.txtBoxPassWord.PasswordChar = '*';
                scanquireUsersControl.txtBoxVerifyPassWord.PasswordChar = '*';
            }
            scanquireUsersControl.txtBoxPassWord.Text = string.Empty;

            scanquireUsersControl.txtBoxVerifyPassWord.Text = string.Empty;
            scanquireUsersControl.txtBoxFName.Text = string.Empty;
            scanquireUsersControl.txtBoxLName.Text = string.Empty;
            scanquireUsersControl.txtBoxLoginId.Text = string.Empty;
            scanquireUsersControl.txtBoxEmailAddress.Text = string.Empty;
            scanquireUsersControl.chkBoxIsAdmin.Checked = false;
            scanquireUsersControl.chkBoxShowPassWord.Checked = false;
            scanquireUsersControl.txtBoxFName.Focus();
        }



        private bool CheckPw(TextBox textBox)
        {
            

            if (!(LoginUtilites.LoginUtilitesInstance.CheckValidString(textBox.Text, 8,"Password")))
            {
                textBox.Text = string.Empty;
                textBox.Focus();
                return false;
            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(textBox.Text, LoginUtilites.LoginUtilitesInstance.RegXNonDigitChar, false)))
            {
                MessageBox.Show("Invalid password must contain one non digit ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Text = string.Empty;
                textBox.Focus();
                return false;
            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(textBox.Text, LoginUtilites.LoginUtilitesInstance.RegXLowewrCase, false)))
            {
                MessageBox.Show("Invalid password must contain one lowercase  letter ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Text = string.Empty;
                textBox.Focus();
                return false;
            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(textBox.Text, LoginUtilites.LoginUtilitesInstance.RegXLowewrUpperCase, false)))
            {
                MessageBox.Show("Invalid password must contain one upercase letter ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Text = string.Empty;
                textBox.Focus();
                return false;
            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(textBox.Text, LoginUtilites.LoginUtilitesInstance.RegXNumsLetters, false)))
            {
                MessageBox.Show("Invalid password must contain one digit ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Text = string.Empty;
                textBox.Focus();
                return false;
            }

            return true;
        }
        private void HideShowUserControl(ScanquireUsersControl scanquireUsersControl, bool showHide)
        {
            // scanquireUsersControl.chkBoxShowPassWord.Visible = showHide;
            if (showHide)
            {
                scanquireUsersControl.txtBoxPassWord.ReadOnly = false;
                scanquireUsersControl.txtBoxVerifyPassWord.ReadOnly = false;
                scanquireUsersControl.txtBoxPassWord.PasswordChar = '*';
                scanquireUsersControl.txtBoxVerifyPassWord.PasswordChar = '*';
                scanquireUsersControl.labPw.Text = "PassWord:";
                scanquireUsersControl.labVerPW.Text = "Verify PassWord:";
                scanquireUsersControl.chkBoxShowPassWord.Text = "Show Password:";

            }
            else
            {
                scanquireUsersControl.txtBoxPassWord.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar;
                scanquireUsersControl.txtBoxVerifyPassWord.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar;
                scanquireUsersControl.labPw.Text = "Last Login:";
                scanquireUsersControl.labVerPW.Text = "PW Last Changed:";
                scanquireUsersControl.txtBoxPassWord.ReadOnly = true;
                scanquireUsersControl.txtBoxVerifyPassWord.ReadOnly = true;
                scanquireUsersControl.chkBoxShowPassWord.Text = "Delete User:";
            }


        }
        private bool ChekPassword(ScanquireUsersControl scanquireUsersControl)
        {
            if (!(CheckPw(scanquireUsersControl.txtBoxPassWord)))
                return false;

            if (scanquireUsersControl.txtBoxPassWord.Text != scanquireUsersControl.txtBoxVerifyPassWord.Text)
            {
                MessageBox.Show("Passwords do not match", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void AddUpDateUsers(ScanquireUsersControl scanquireUsersControl)
        {


            if (!(LoginUtilites.LoginUtilitesInstance.CheckValidString(scanquireUsersControl.txtBoxFName.Text, 2,"User First Name ")))
            {
                // scanquireUsersControl1.txtBoxFName.Text = string.Empty;
                scanquireUsersControl.txtBoxFName.Focus();
                return;

            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckValidString(scanquireUsersControl.txtBoxLName.Text, 2,"User Last Name ")))
            {
                //  scanquireUsersControl1.txtBoxFName.Text = string.Empty;
                scanquireUsersControl.txtBoxLName.Focus();
                return;

            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckValidString(scanquireUsersControl.txtBoxLoginId.Text, 6,"Login ID ")))
            {
                // scanquireUsersControl1.txtBoxLoginId.Text = string.Empty;
                scanquireUsersControl.txtBoxLoginId.Focus();
                return;

            }
            try
            {
                ScanQuire_SendEmails.Send_Emails.EmailInstance.NewMailMessage(scanquireUsersControl.txtBoxEmailAddress.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid email address: {scanquireUsersControl.txtBoxEmailAddress.Text} {ex.Message}", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //   scanquireUsersControl1.txtBoxEmailAddress.Text = string.Empty;
                scanquireUsersControl.txtBoxEmailAddress.Focus();
                return;

            }

            if (TabIndexPage == 0)
            {
                if (ChekPassword(scanquireUsersControl))
                {
                    AddUserSql();
                    ClearTxtBoxes(scanquireUsersControl1);
                }
            }
            else
            {
                UpDateUsersSql(scanquireUsersControl2);
            }
            //^[a-zA-Z0-9_@.-]+$
        }
        private void UpDateUsersSql(ScanquireUsersControl scanquireUsersControl)
        {

            using (SqlCmds CmdSql = new SqlCmds())
            {
                CmdSql.SqlConn = CmdSql.OpenSqlConnection();
                Dictionary<string, string> dicUpDateUser = new Dictionary<string, string>();
                dicUpDateUser.Add(CmdSql.SqlSpParamLoginName, scanquireUsersControl.txtBoxLoginId.Text);
                dicUpDateUser.Add(CmdSql.SqlSpParamOldLoginName, cmbBoxUsers.Text);
                dicUpDateUser.Add(CmdSql.SqlSpParamUserFirstName, scanquireUsersControl.txtBoxFName.Text);
                dicUpDateUser.Add(CmdSql.SqlSpParamUserLastName, scanquireUsersControl.txtBoxLName.Text);
                dicUpDateUser.Add(CmdSql.SqlSpParamUserEmail, scanquireUsersControl.txtBoxEmailAddress.Text);
                if (scanquireUsersControl.chkBoxIsAdmin.Checked)
                    dicUpDateUser.Add(CmdSql.SqlSpParamIsAdmin, CmdSql.IsAdminYes);
                else
                    dicUpDateUser.Add(CmdSql.SqlSpParamIsAdmin, CmdSql.IsAdminNo);
                string retMess = CmdSql.UpdateUsers(dicUpDateUser);

                UpdateTabForm();
                MessageBox.Show($"{retMess}", "UpDateUsers", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }
        private void AddUserSql()
        {
            string retMess = string.Empty;
            try
            {
                using (SqlCmds CmdSql = new SqlCmds())
                {
                    CmdSql.SqlConn = CmdSql.OpenSqlConnection();
                    Dictionary<string, string> usersSq = new Dictionary<string, string>();
                    usersSq.Add(CmdSql.SqlSpParamLoginName, scanquireUsersControl1.txtBoxLoginId.Text);
                    usersSq.Add(CmdSql.SqlSpParamUserPassWord, scanquireUsersControl1.txtBoxPassWord.Text);
                    usersSq.Add(CmdSql.SqlSpParamUserFirstName, scanquireUsersControl1.txtBoxFName.Text);
                    usersSq.Add(CmdSql.SqlSpParamUserLastName, scanquireUsersControl1.txtBoxLName.Text);
                    usersSq.Add(CmdSql.SqlSpParamUserEmail, scanquireUsersControl1.txtBoxEmailAddress.Text);
                    if (scanquireUsersControl1.chkBoxIsAdmin.Checked)
                        usersSq.Add(CmdSql.SqlSpParamIsAdmin, CmdSql.IsAdminYes);
                    else
                        usersSq.Add(CmdSql.SqlSpParamIsAdmin, CmdSql.IsAdminNo);

                    retMess = CmdSql.AddUsers(usersSq);
                }
            }
            catch (Exception ex)
            {
                retMess = ex.Message;
            }
            MessageBox.Show($"{retMess}", "Added User Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void DeleteUserLogin(ScanquireUsersControl scanquireUsersControl)
        {
            string retMess = string.Empty;
            if (cmbBoxUsers.Items.Count == 1)
            {
                MessageBox.Show($"Cannot delete last user: {cmbBoxUsers.Text}", "Last User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (scanquireUsersControl.txtBoxLoginId.Text != cmbBoxUsers.Text)
            {
                MessageBox.Show($"User seleted user login id: {cmbBoxUsers.Text} not the same as login id: {scanquireUsersControl.txtBoxLoginId.Text}", "User Miss match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dr = MessageBox.Show($"Delete user login id: {scanquireUsersControl.txtBoxLoginId.Text}", "Confrim Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No)
                return;
            try
            {
                using (SqlCmds CmdSql = new SqlCmds())
                {
                    CmdSql.SqlConn = CmdSql.OpenSqlConnection();
                    retMess = CmdSql.DeleteUser(scanquireUsersControl.txtBoxLoginId.Text);
                }
                UpdateTabForm();
            }
            catch (Exception ex)
            {
                retMess = $"Error deleting user :{scanquireUsersControl.txtBoxLoginId} {ex.Message}";
            }
            MessageBox.Show($"{retMess}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {
            if (TabIndexPage == 0)
                AddUpDateUsers(scanquireUsersControl1);
            else
            {
                if (scanquireUsersControl2.chkBoxShowPassWord.Checked)
                {
                    DeleteUserLogin(scanquireUsersControl2);
                }
                else
                {
                    if ((CheckForChanges(scanquireUsersControl2)))
                        AddUpDateUsers(scanquireUsersControl2);
                }
            }

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (TabIndexPage == 0)
                ClearTxtBoxes(scanquireUsersControl1);
            else
                ClearTxtBoxes(scanquireUsersControl2);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpDateCmbBox()
        {

            cmbBoxUsers.BeginUpdate();
            cmbBoxUsers.Items.Clear();
            foreach (KeyValuePair<string, ScanQuireUsers> keyValuePair in DicScanUserInfor)
            {
                cmbBoxUsers.Items.Add(keyValuePair.Value.LoginId);
            }
            cmbBoxUsers.EndUpdate();

            cmbBoxUsers.SelectedItem = null;
        }
        private void GetScanUsers()
        {
            try
            {
                using (SqlCmds CmdSql = new SqlCmds())
                {
                    CmdSql.SqlConn = CmdSql.OpenSqlConnection();
                    DicScanUserInfor = CmdSql.GetScanQuireUserInfo();
                    UpDateCmbBox();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Getting Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void TrimTxtBox(ScanquireUsersControl scanquireUsersControl)
        {
            scanquireUsersControl.txtBoxEmailAddress.Text = scanquireUsersControl2.txtBoxEmailAddress.Text.Trim();
            scanquireUsersControl.txtBoxFName.Text = scanquireUsersControl2.txtBoxFName.Text.Trim();
            scanquireUsersControl.txtBoxLName.Text = scanquireUsersControl2.txtBoxLName.Text.Trim();
            scanquireUsersControl.txtBoxLoginId.Text = scanquireUsersControl2.txtBoxLoginId.Text.Trim();
        }
        private void UpdateTabForm()
        {
            GetScanUsers();
            MakeTxtReadOnly(scanquireUsersControl2, true);
            ClearTxtBoxes(scanquireUsersControl2);
            btnAddEditusers.Text = BtnUpDate;
            HideShowUserControl(scanquireUsersControl2, false);
            

        }
        private void TabCntAddEditUsers_Selected(object sender, TabControlEventArgs e)
        {
            TabIndexPage = e.TabPageIndex;
            if (e.TabPageIndex == 1)
            {
                UpdateTabForm();

            }
            else
            {

                btnAddEditusers.Text = BtnAdd;
                HideShowUserControl(scanquireUsersControl1, true);
            }
            Console.WriteLine();
        }

        private ScanQuireUsers GetCurrentUserInfo()
        {
            ScanQuireUsers scanQuireUsers = null;
            if (!(string.IsNullOrWhiteSpace(cmbBoxUsers.Text)))
            {
                if (DicScanUserInfor.TryGetValue(cmbBoxUsers.Text, out scanQuireUsers))
                {
                    return scanQuireUsers;
                }
            }
            return null;
        }
        private bool CheckForChanges(ScanquireUsersControl scanquireUsersControl)
        {

            ScanQuireUsers scanQuireUsers = GetCurrentUserInfo();
            if (scanQuireUsers != null)
            {
                if (scanquireUsersControl.txtBoxEmailAddress.Text != scanQuireUsers.UserEmailAddress)
                    return true;
                if (scanquireUsersControl.txtBoxFName.Text != scanQuireUsers.UserFName)
                    return true;
                if (scanquireUsersControl.txtBoxLName.Text != scanQuireUsers.UserLName)
                    return true;

                if (scanquireUsersControl.txtBoxLoginId.Text != scanQuireUsers.LoginId)
                    return true;
                if (scanQuireUsers.IsAdmin == "False")
                {
                    if (scanquireUsersControl.chkBoxIsAdmin.Checked)
                        return true;
                }
                if (scanQuireUsers.IsAdmin == "True")
                {
                    if (!(scanquireUsersControl.chkBoxIsAdmin.Checked))
                        return true;
                }

            }

            MessageBox.Show("No changes found", "No changes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private void CmbBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(cmbBoxUsers.Text)))
            {
                if (scanquireUsersControl2.txtBoxFName.ReadOnly)
                    MakeTxtReadOnly(scanquireUsersControl2, false);
                ScanQuireUsers scanQuireUsers = GetCurrentUserInfo();
                if (scanQuireUsers != null)
                {

                    scanquireUsersControl2.txtBoxEmailAddress.Text = scanQuireUsers.UserEmailAddress;
                    scanquireUsersControl2.txtBoxFName.Text = scanQuireUsers.UserFName;
                    scanquireUsersControl2.txtBoxLName.Text = scanQuireUsers.UserLName;
                    scanquireUsersControl2.txtBoxLoginId.Text = scanQuireUsers.LoginId;
                    if (!(string.IsNullOrEmpty(scanQuireUsers.LastLogin)))
                        scanquireUsersControl2.txtBoxPassWord.Text = scanQuireUsers.LastLogin;
                    else
                        scanquireUsersControl2.txtBoxPassWord.Text = DateTime.Now.ToString("MM-dd-yyyy");
                    scanquireUsersControl2.txtBoxVerifyPassWord.Text = scanQuireUsers.PWLastChanged;
                    if (scanQuireUsers.IsAdmin == "True")
                        scanquireUsersControl2.chkBoxIsAdmin.Checked = true;
                    TrimTxtBox(scanquireUsersControl2);
                }

            }
        }

        private void AddEditUsers_Shown(object sender, EventArgs e)
        {
            UpdateTabPage();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetPassWord resetPassWord = new ResetPassWord();
            resetPassWord.ShowDialog();
        }

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetPassWord resetPassWord = new ResetPassWord();
            if (!(string.IsNullOrWhiteSpace(cmbBoxUsers.Text)))
                resetPassWord.UserId = cmbBoxUsers.Text;
            resetPassWord.ShowDialog();
        }
    }
}

