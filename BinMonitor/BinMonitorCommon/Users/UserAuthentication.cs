using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class AdminOverrideStatusChangedEventArgs : EventArgs
    {
        public bool Value { get; protected set; }

        public AdminOverrideStatusChangedEventArgs(bool value)
        { this.Value = value; }

    }

    public static class UserAuthentication
    {
        public static event EventHandler<AdminOverrideStatusChangedEventArgs> AdminOverrideStatusChanged;

        private static bool _AdminOverrideStatus = false;
        public static bool AdminOverrideStatus
        {
            get { return _AdminOverrideStatus; }
            private set 
            { 
                _AdminOverrideStatus = value;
                OnAdminOverrideStatusChanged(value);
            }
        }

        public static void RequestAdminOverride(IWin32Window owner)
        {
            DisableAdminOverride();

            AdminOverrideDialog dlg = new AdminOverrideDialog()
            {StartPosition = FormStartPosition.CenterParent};
            if (dlg.ShowDialog(owner) != DialogResult.OK)
            { throw new OperationCanceledException(); }

            AdminOverrideStatus = true;
        }

        public static void DisableAdminOverride()
        { AdminOverrideStatus = false; }

        public static User RequestUser(IWin32Window parent)
        {
            if (AdminOverrideStatus)
            {
                string caption = "Select your name from the list";
                return UserPickerDialog.SelectEmployee(parent, caption);
            }
            else
            {
                /*
                string uid = SmartCardReaderDialog.PromptForUid(parent);
                string encodedUid = User.EncodeSmartCardUid(uid);
                return Users.Instance.EnsureGetByEncodedCardId(encodedUid);
                */
                string caption = "Select your name from the list";
                return UserPickerDialog.SelectEmployee(parent, caption);
            }
        }

        public static User EnsureRequestUser(IWin32Window parent)
        {
            User user = RequestUser(parent);
            if (user == null)
            { throw new InvalidOperationException("The requested operation requires an authenticated user"); }
            EnterPassword test = new EnterPassword();
            test.ShowDialog();
            String password = test.getData();
            if (password.Equals(user.CardId))
            {
                return user;
            }
            else
            {
                MessageBox.Show("Wrong password!");
                return null;
            }
        }

        private static void NotifyAdminOverrideStatusChanged(bool value)
        {
            EventHandler<AdminOverrideStatusChangedEventArgs> handler = AdminOverrideStatusChanged;
            if (handler != null)
            { handler(null, new AdminOverrideStatusChangedEventArgs(value)); }
        }

        private static void OnAdminOverrideStatusChanged(bool value)
        { NotifyAdminOverrideStatusChanged(value); }
    }
}