using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SP = Microsoft.SharePoint.Client;
using EdocsUSA.Utilities;
using EdocsUSA.Controls;

namespace Scanquire.Public.Sharepoint
{
    public partial class SharepointCredentialsDialog : Form
    {
        private Timeout _Timeout = new  Timeout();
        public Timeout Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        public string ServerAddress
        {
            get { return ServerAddressTextBox.Text; }
            set { ServerAddressTextBox.Text = value; }
        }

        public string UserName
        {
            get { return UserNameTextBox.Text; }
            set { UserNameTextBox.Text = value; }
        }

        public string Password
        {
            get { return PasswordTextBox.Text; }
            set { PasswordTextBox.Text = value; }
        }

        public string Domain
        {
            get { return DomainTextBox.Text; }
            set { DomainTextBox.Text = value; }
        }

        public bool ShowDomain
        {
            get { return DomainTextBox.Visible; }
            set 
            {
                DomainLabel.Visible = value;
                DomainTextBox.Visible = value; 
            }
        }

        public SharepointCredentialsDialog()
        {
            InitializeComponent();
        }

        public override bool ValidateChildren()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                Trace.TraceInformation("No username provided");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                Trace.TraceInformation("No password provided");
                return false;
            }
            return base.ValidateChildren();
        }

        public enum TestConnectionResult
        {
            SUCCESS,
            TIMEOUT,
            INSUFICIENT_DATA,
            CONNECTION_FAILED
        }

        public async Task<SP.ClientContext> _GetClientContext()
        {
            if (ValidateChildren() == false)
            { throw new Exception("Invalid or unspecified credentials"); }
            if (Timeout.TimedOut)
            { throw new Exception("Timed out"); }

            SP.ClientContext context;
            context = await Task.Factory.StartNew<SP.ClientContext>(() =>
            {
                context = new SP.ClientContext(ServerAddress);
                context.AuthenticationMode = SP.ClientAuthenticationMode.FormsAuthentication;
                context.FormsAuthenticationLoginInfo = new SP.FormsAuthenticationLoginInfo(UserName, Password);
                context.ExecuteQuery();
            });
            Timeout.Reset();
            return context;
        }

        public async Task<SP.ClientContext> GetClientContext()
        {
            DialogResult r = this.ShowDialog();
            {if (r != DialogResult.OK)
            { throw new OperationCanceledException(); }

            using (SP.ClientContext context = await GetClientContext())
            catch (Exception ex)
            {
                StatusLabel.Text = ex.Message;

                bool tryAgain;
                if (ex.Message == "Timed out")
                {
                    Trace.TraceError("Error establishing connection to sharepoint, " + ex.Message);
                    Trace.TraceError(ex.StackTrace);
                }

                
            }

            //Getting client context failed, so prompt the user and continue;
            string message = string.Concat(
                    "Failed to connect to the server, details:"
                    , Environment.NewLine, ex.Message
                    , Environment.NewLine, "Select OK to proceed anyway (records will need to be manually uploaded later)"
                    , Environment.NewLine, "Select Cancel to re-enter credentials or cancel");
            DialogResult r = MessageBox.Show(this, message, "Error Connecting", MessageBoxButtons.OKCancel);
            if (r == DialogResult.OK)
            { return null; }
            
            return await GetClientContext();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
