using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace DemoArchivers
{
    public partial class SharepointRestCredentials : UserControl
    {
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

        public NetworkCredential NetworkCredentials
        { get { return new NetworkCredential(UserName, Password, Domain); } }

        public string AuthorizationHeader
        { get { return SharepointRestConnector.GetAuthorizationHeader(Domain, UserName, Password); } }

        public SharepointRestCredentials()
        {
            InitializeComponent();
        }

        private void UnlockServerAddressButton_Click(object sender, EventArgs e)
        {
            ServerAddressTextBox.ReadOnly = !ServerAddressTextBox.ReadOnly;
        }
    }
}
