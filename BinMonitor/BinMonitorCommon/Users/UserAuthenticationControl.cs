using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class UserAuthenticationControl : FlowLayoutPanel
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IUserSource UserSource { get { return cmbSelectedUser; } }

        protected ErrorProvider ErrorProvider;

        protected FlowLayoutPanel pnlUserAuthentication;
        protected Button btnAuthenticateUser;
        protected UsersComboBox cmbSelectedUser;
        protected Button btnClearSelectedUser;

        protected FlowLayoutPanel pnlAdminOverride;
        protected Button btnEnableAdminOverride;
        protected CheckBox chkAdminOverrideStatus;
        protected Button btnDisableAdminOverride;

        public UserAuthenticationControl()
        { InitializeComponent(); }

        private void InitializeComponent()
        {
            this.Margin = Padding.Empty;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ErrorProvider = new ErrorProvider()
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };

            ErrorProvider = new ErrorProvider()
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };

            pnlUserAuthentication = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight
            };
            this.Controls.Add(pnlUserAuthentication);
            
            btnAuthenticateUser = new Button()
            {
                Text = "Authenticate",
                Margin = new Padding(0, 0, ErrorProvider.Icon.Width, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            btnAuthenticateUser.Click += btnAuthenticateUser_Click;
            pnlUserAuthentication.Controls.Add(btnAuthenticateUser);

            cmbSelectedUser = new UsersComboBox()
            {
                Margin = Padding.Empty,
                Enabled = false
            };
            pnlUserAuthentication.Controls.Add(cmbSelectedUser);

            btnClearSelectedUser = new Button()
            {
                Text = "Clear",
                Margin = Padding.Empty,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            btnClearSelectedUser.Click += btnClearSelectedUser_Click;
            pnlUserAuthentication.Controls.Add(btnClearSelectedUser);

            pnlAdminOverride = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight
            };
            this.Controls.Add(pnlAdminOverride);

            btnEnableAdminOverride = new Button()
            {
                Text = "Admin Override",
                Margin = new Padding(0, 0, ErrorProvider.Icon.Width, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };
            btnEnableAdminOverride.Click += btnEnableAdminOverride_Click;
            pnlAdminOverride.Controls.Add(btnEnableAdminOverride);

            chkAdminOverrideStatus = new CheckBox()
            {
                Margin = Padding.Empty,
                AutoSize = true,
                Enabled = false
            };
            pnlAdminOverride.Controls.Add(chkAdminOverrideStatus);

            btnDisableAdminOverride = new Button()
            {
                Text = "Disable",
                Margin = Padding.Empty,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };
            btnDisableAdminOverride.Click += btnDisableAdminOverride_Click;
            pnlAdminOverride.Controls.Add(btnDisableAdminOverride);

            UserAuthentication.AdminOverrideStatusChanged += UserAuthentication_AdminOverrideStatusChanged;
            cmbSelectedUser.SelectedUserChanged += cmbSelectedUser_SelectedUserChanged;
        }

        void cmbSelectedUser_SelectedUserChanged(object sender, SelectedUserChangedEventArgs e)
        {
            pnlUserAuthentication.BackColor =
                (e.SelectedUser == null)
                ? SystemColors.Control
                : Color.LightGreen;
        }

        void UserAuthentication_AdminOverrideStatusChanged(object sender, AdminOverrideStatusChangedEventArgs e)
        {
            chkAdminOverrideStatus.Checked = e.Value;
            pnlAdminOverride.BackColor =
                (e.Value == true)
                ? Color.DarkOrange
                : SystemColors.Control;
        }

        void btnDisableAdminOverride_Click(object sender, EventArgs e)
        {
            ErrorProvider.Clear();
            UserAuthentication.DisableAdminOverride();
        }

        void btnEnableAdminOverride_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorProvider.Clear();
                UserAuthentication.DisableAdminOverride();
                UserAuthentication.RequestAdminOverride(this);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(btnEnableAdminOverride, ex.Message);
            }
        }


        void btnClearSelectedUser_Click(object sender, EventArgs e)
        {
            ErrorProvider.Clear();
            UserSource.Clear();
        }

        void btnAuthenticateUser_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorProvider.Clear();
                UserSource.Clear();
                User user = UserAuthentication.EnsureRequestUser(this);
                UserSource.EnsureSelectUser(user);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                ErrorProvider.SetError(btnAuthenticateUser, ex.Message);
            }
        }

        public bool AdminOverrideVisible
        {
            get { return pnlAdminOverride.Visible; }
            set { pnlAdminOverride.Visible = value; }
        }

        public void RequestUserAuthentication()
        { btnAuthenticateUser.PerformClick(); }
    }
}
