using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using SP = Microsoft.SharePoint.Client;

namespace Scanquire.Public.Sharepoint
{
    public partial class SharepointCredentialsManager : Form
    {
        public SharepointCredentialsManager()
        {
            InitializeComponent();
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
            set { DomainTextBox.Visible = value; }
        }

        private bool _EnableTimeout = false;
        public bool EnableTimeout
        {
            get { return _EnableTimeout; }
            set { _EnableTimeout = value; }
        }

        private int _Timeout = 30;
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        protected DateTime LastActionTime = DateTime.MinValue;

        protected bool HasTimedOut()
        {
            TimeSpan timespan = DateTime.Now - LastActionTime;
            return (timespan.TotalMinutes > Timeout);
        }

        protected void SetLastActionTime()
        { LastActionTime = DateTime.Now; }


        private async Task<SP.ClientContext> GetClientContext(bool forceInput)
        {
            bool requiresInput = string.IsNullOrWhiteSpace(ServerAddress)
                || string.IsNullOrWhiteSpace(UserName)
                || string.IsNullOrWhiteSpace(Password)
                || HasTimedOut()
                || forceInput;

            if (requiresInput)
            { this.TryShowDialog(DialogResult.OK); }
            
            try
            {
                SP.ClientContext context = await LogIn();
                MessageLabel.Text = "...";
                return context;
            }
            catch (Exception ex)
            {
                MessageLabel.Text = ex.Message;
            }
            //If it failed to return above, try again forcing input.
            return await GetClientContext(true);
        }
        
        private async Task<SP.ClientContext> LogIn()
        {
            SP.ClientContext context;   
            context = await Task.Factory.StartNew<SP.ClientContext>(() =>
            {
                context = new SP.ClientContext(ServerAddress);
                context.AuthenticationMode = SP.ClientAuthenticationMode.FormsAuthentication;
                context.FormsAuthenticationLoginInfo = new SP.FormsAuthenticationLoginInfo(UserName, Password);
                return context;
            });
            return context;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
