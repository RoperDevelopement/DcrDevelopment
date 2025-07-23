using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EdocsUSA.Utilities.Extensions;
using SP = Microsoft.SharePoint.Client;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Scanquire.Public.Sharepoint
{
    public partial class SharepointCredentials : UserControl
    {
        private Timeout _Timeout = new  Timeout();
        public Timeout Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        public SharepointCredentials()
        {
            InitializeComponent();
            TimeoutTimer.Tick += TimeoutTimer_Tick;
            ResetTimeoutTimer();
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

        public enum ValidationResult
        { }

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

        
    }
}
