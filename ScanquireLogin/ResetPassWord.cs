using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScanQuireSqlCmds;
using ScanQuire_SendEmails;
using System.Text.RegularExpressions;
using TL = EdocsUSA.Utilities.Logging;
namespace ScanquireLogin
{
    public partial class ResetPassWord : Form
    {
         
        readonly string AdminChanged = "AdminChanged";


        private readonly int MinPasswordLenth = 16;
        public string UserId
        { get; set; }

        public ResetPassWord()
        {
            UserId = string.Empty;
           
            InitializeComponent();
            
            //  if (string.Compare(TL.TraceLogger.TraceLoggerInstance.UserName, "edocsadmin", true) == 0)
            
        }

        private void ChkBoxGeneratePassWord_CheckStateChanged(object sender, EventArgs e)
        {


            if (chkBoxGeneratePassWord.Checked)
            {
                txtBoxNewPassword.Text = string.Empty;
               
                txtBoxVerifyPassword.Text = string.Empty;
                txtBoxNewPassword.ReadOnly = true;
                
                txtBoxVerifyPassword.ReadOnly = true;
                chkBoxShowPassWord.Checked = false;
                chkBoxShowPassWord.Enabled = false;
            }
            else
            {
                txtBoxNewPassword.Text = string.Empty;
               
                txtBoxVerifyPassword.ReadOnly = false;
                txtBoxNewPassword.ReadOnly = false;
                 
                chkBoxShowPassWord.Enabled = true;
            }

        }
        private void SendEmail(string emailTo)
        {
            string emailMessage = $"Your password has been rest for user login id:{txtBoxUserID.Text.Trim()} the new password is:{txtBoxNewPassword.Text.Trim()}";
            string emailSubjext = $"Password rest for user login id:{txtBoxUserID.Text.Trim()}";
            Send_Emails.EmailInstance.SendEmail(emailTo.Trim(), emailMessage, emailSubjext);

        }
        private void ChangePassword()
        {
            string retMess = string.Empty;
            try
            {
                using (SqlCmds CmdSql = new SqlCmds())
                {
                    CmdSql.SqlConn = CmdSql.OpenSqlConnection();
                    retMess = CmdSql.ResetPassword(txtBoxUserID.Text.Trim(), txtBoxNewPassword.Text.Trim(), AdminChanged);

                }
               
            }
            catch (Exception ex)
            {
                retMess = ex.Message;
            }
            
            MessageBox.Show($"{retMess}", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void ChkBoxShowPassWord_CheckStateChanged(object sender, EventArgs e)
        {
            txtBoxNewPassword = LoginUtilites.LoginUtilitesInstance.ChangePasWordChar(txtBoxNewPassword);
            
            txtBoxVerifyPassword = LoginUtilites.LoginUtilitesInstance.ChangePasWordChar(txtBoxVerifyPassword);

        }
        private void GeneeratePw()
        {
            try
            {
                txtBoxNewPassword.Text = GeneratePassWord.GeneratePassWordInstance.GeneratePw(MinPasswordLenth);

                ChangePassword();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cound not create password: {ex.Message}", "Generate Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void CheckPassWord()
        {
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(txtBoxNewPassword.Text, LoginUtilites.LoginUtilitesInstance.RegXNonDigitChar, false)))
            {
                MessageBox.Show("Invalid password must contain one non digit ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //textBox.Text = string.Empty;
                txtBoxNewPassword.Focus();
                return;
            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(txtBoxNewPassword.Text, LoginUtilites.LoginUtilitesInstance.RegXLowewrCase, false)))
            {
                MessageBox.Show("Invalid password must contain one lowercase  letter ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //textBox.Text = string.Empty;
                txtBoxNewPassword.Focus();
                return;
            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(txtBoxNewPassword.Text, LoginUtilites.LoginUtilitesInstance.RegXLowewrUpperCase, false)))
            {
                MessageBox.Show("Invalid password must contain one upercase letter ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //textBox.Text = string.Empty;
                txtBoxNewPassword.Focus();
                return;

            }
            if (!(LoginUtilites.LoginUtilitesInstance.CheckRegxString(txtBoxNewPassword.Text, LoginUtilites.LoginUtilitesInstance.RegXNumsLetters, false)))
            {
                MessageBox.Show("Invalid password must contain one digit ", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //txtBoxNewPassword.Text = string.Empty;
                txtBoxNewPassword.Focus();
                return;

            }
            ChangePassword();
        }
        private void BtnChangePw_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (string.IsNullOrWhiteSpace(txtBoxUserID.Text))
            {
                MessageBox.Show("User ID cannot be blank", "User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (chkBoxGeneratePassWord.Checked)
                {
                    GeneeratePw();
                }
                else
                {
                    if (txtBoxNewPassword.Text != txtBoxVerifyPassword.Text)
                    {
                        MessageBox.Show("Verify Password does not match Password", "PassWord MisMatch", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                        CheckPassWord();
                }
            }

            this.Enabled = true;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetPassWord_Shown(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(UserId)))
                txtBoxUserID.Text = UserId;
            txtBoxUserID.Text = txtBoxUserID.Text.Trim();
            txtBoxNewPassword.Focus();
        }

       
    }
}
