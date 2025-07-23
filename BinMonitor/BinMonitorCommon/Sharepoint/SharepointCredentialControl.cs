using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common.Sharepoint
{
    public partial class SharepointCredentialControl : UserControl
    {
        public string Host { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool HostReadonly
        {
            get { return txtHost.ReadOnly; }
            set { txtHost.ReadOnly = value; }
        }

        public bool DomainReadOnly
        {
            get { return txtDomain.ReadOnly; }
            set
            { txtDomain.ReadOnly = value; }
        }

        public bool UserNameReadonly
        {
            get { return txtUserName.ReadOnly; }
            set { txtUserName.ReadOnly = value; }
        }

        public SharepointCredentialControl()
        {
            InitializeComponent();
            txtHost.DataBindings.Add("Text", this, "Host");
            txtHost.Validating += delegate(object sender, CancelEventArgs e)
            {
                if (e.Cancel)
                { ErrorProvider.SetError(txtHost, "Error"); }
                else
                {ErrorProvider.SetError(txtHost, txtHost.ValidationErrorMessage);}
            };
            txtDomain.DataBindings.Add("Text", this, "Domain");
            txtDomain.Validating += delegate(object sender, CancelEventArgs e)
            {
                if (e.Cancel)
                { ErrorProvider.SetError(txtDomain, "Error"); }
                else
                { ErrorProvider.SetError(txtDomain, txtDomain.ValidationErrorMessage); }
            };
            txtUserName.DataBindings.Add("Text", this, "Username");
            txtUserName.Validating += delegate(object sender, CancelEventArgs e)
            {
                if (e.Cancel)
                { ErrorProvider.SetError(txtUserName, "Error"); }
                else
                { ErrorProvider.SetError(txtUserName, txtUserName.ValidationErrorMessage); }
            };
            txtPassword.DataBindings.Add("Text", this, "Password");
            txtPassword.Validating += delegate(object sender, CancelEventArgs e)
            {
                if (e.Cancel)
                {ErrorProvider.SetError(txtPassword, "Error");}
                else
                {ErrorProvider.SetError(txtPassword, txtPassword.ValidationErrorMessage);}
            };
        }
    }
}
