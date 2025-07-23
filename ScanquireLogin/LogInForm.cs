using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScanQuireSqlCmds;
using System.Threading;
using System.Threading.Tasks;
using TL = EdocsUSA.Utilities.Logging;
namespace ScanquireLogin
{
    public partial class LogInForm : Form
    {
        public bool IsAdmin
        { get; set; }
        public string UserName
        { get; set; }
        
        public bool UserCanceled
        { get; set; }
        
        
        public LogInForm()
        {

            InitializeComponent();
            progressBar1.Maximum = 100;
            UserCanceled = false;



        }
        private void ShowHideControl(bool showHidCont)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"In method ShowHideControl for setting:{showHidCont.ToString()}");
            txtPassword.Visible = showHidCont;
            cmbBoxUserID.Visible = showHidCont;
            labId.Visible = showHidCont;
            labPW.Visible = showHidCont;
            cmbBoxUserID.Visible = showHidCont;
            chkBoxShowPW.Visible = showHidCont;
            btnCan.Visible = showHidCont;
            btnLogOn.Visible = showHidCont;
            


        }
        private void Button2_Click(object sender, EventArgs e)
        {
            // throw new Exception("User canceled");
            UserCanceled = true;
            this.Close();
        }
        //private void InitCmbBoxes()
        //{
        //    cmbBoxUserID.BeginUpdate();
        //    cmbBoxUserID.DataSource = DS.Tables[0].DefaultView;
        //    cmbBoxUserID.DisplayMember = "loginIDS";
        //    cmbBoxUserID.BindingContext = this.BindingContext;
        //    cmbBoxUserID.EndUpdate();
        //    cmbBoxUserID.SelectedItem = null;
        //}
        private bool TestSqlConnection()
        {
            
            try
            {
                using (SqlCmds sqlCmds = new SqlCmds())
                {
                    //SqlCmds sqlCmds = new SqlCmds();
                    sqlCmds.SqlConn = sqlCmds.OpenSqlConnection();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot get users id's error message:{ex.Message}", "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Close();
            }
            return false;

        }
        private DataSet GetLoginIdDataSet()
        //private void GetLoginId()
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting Scanquire log in id's");
            DataSet ds = null;
            try
            {

                using (SqlCmds sqlCmds = new SqlCmds())
                {
                    //SqlCmds sqlCmds = new SqlCmds();
                    sqlCmds.SqlConn = sqlCmds.OpenSqlConnection();
                    ds = sqlCmds.UserLoginDataTabe();
                    //cmbBoxUserID.BeginUpdate();
                    //cmbBoxUserID.DataSource = ds.Tables[0].DefaultView;
                    //cmbBoxUserID.DisplayMember = "loginIDS";
                    //cmbBoxUserID.BindingContext = this.BindingContext;
                    //cmbBoxUserID.EndUpdate();
                    //cmbBoxUserID.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot get users id's error message:{ex.Message}", "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Getting log in id's {ex.Message.Replace(","," ")}");
                //this.Close();
            }
            return ds;
        }
        private void GetLoginId(DataSet ds)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("Adding scanquire user id's to lsit");
            try

            {
                    cmbBoxUserID.BeginUpdate();
                    cmbBoxUserID.DataSource = ds.Tables[0].DefaultView;
                    cmbBoxUserID.DisplayMember = "loginIDS";
                    cmbBoxUserID.BindingContext = this.BindingContext;
                    cmbBoxUserID.EndUpdate();
                    cmbBoxUserID.SelectedItem = null;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot get users id's error message:{ex.Message}", "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Adding scanquire user id's to lsit {ex.Message.Replace(",","")}");
                //this.Close();
            }

        }
        private void LogIn()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbBoxUserID.Text))
                {
                    MessageBox.Show("Select loging id", "No Login id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TL.TraceLogger.TraceLoggerInstance.TraceError("No log in id selected");
                    return;
                }
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Logging on user {cmbBoxUserID.Text}");
                using (SqlCmds sqlCmds = new SqlCmds())
                {
                    sqlCmds.SqlConn = sqlCmds.OpenSqlConnection();
                    IsAdmin = sqlCmds.LogIn(cmbBoxUserID.Text.ToLower(), txtPassword.Text);
                    UserName = cmbBoxUserID.Text;
                }
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"User {cmbBoxUserID.Text} logged into Scanquire");
                this.Close();
            }
            catch (Exception ex)
            {
                DialogResult dr = MessageBox.Show($"Eror logging on {ex.Message}", "Error Loggin in", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                TL.TraceLogger.TraceLoggerInstance.TraceError($"{cmbBoxUserID.Text} logging into Scanquire {ex.Message.Replace(",","")}");
                if (dr == DialogResult.Cancel)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceWarning($"User {cmbBoxUserID.Text} cancled logging into Scanquire");
                    throw new Exception(string.Format("Error logging in {0}", ex.Message));
                }
            }
        }
        private void BtnLogOn_Click(object sender, EventArgs e)
        {
            
            LogIn();
        }

        private void ChkBoxShowPW_CheckStateChanged(object sender, EventArgs e)
        {
            
            if (chkBoxShowPW.Checked)
            {
                if (!(string.IsNullOrEmpty(txtPassword.Text)) && (txtPassword.Text == "EdocsAdminPassword"))
                    txtPassword.Text = string.Empty;
                txtPassword.PasswordChar = LoginUtilites.LoginUtilitesInstance.RemovePassowrdChar; //char.Parse("\0");
                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"User {cmbBoxUserID.Text} showing password");

            }
            else
                txtPassword.PasswordChar = '*';

            
        }


        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtPassword.Text)) && (txtPassword.Text == "EdocsAdminPassword"))
                txtPassword.Text = string.Empty;


        }

        private void LogInForm_Shown(object sender, EventArgs e)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opening widows form {this.Name}");
            ShowHideControl(false);
            labTitle.Text = "Getting Login Id's";
           
            timer1.Start();

        }
       private void RunThread()
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("Running thread to get users loging id's");
            DataSet ds = null;
            Thread TGetIds = new Thread(() => { ds = GetLoginIdDataSet(); });
            TGetIds.Start();
            while (TGetIds.IsAlive)
            {
                Thread.Sleep(1000);
                if (progressBar1.Value > (progressBar1.Maximum-10))
                    progressBar1.Value = progressBar1.Value - 20;
                else
                       progressBar1.Value += 1;
                
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Still getting users loging id's number of tries {progressBar1.Value.ToString()}");
                Application.DoEvents();
                
            }
            progressBar1.Value = progressBar1.Maximum;
            if (ds != null)
            {
                GetLoginId(ds);
                if (cmbBoxUserID.Items.Count == 0)
                    this.Close();
                progressBar1.Value = progressBar1.Maximum - 5;
                ShowHideControl(true);
                progressBar1.Value = progressBar1.Maximum;
                labTitle.Text = "Log On ScanQuire";
                progressBar1.Visible = false;
            }
            else
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Opening form {this.Name}");
                this.Close();
            }
                

        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            RunThread();
            
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"ENTER KEY PRESSED ON FORM {this.Name}");
                LogIn();
            }
        }
             
        }
  //  } 
}
