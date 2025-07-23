using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class UserPickerDialog : Form
    {
        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }

        public string SelectedUserId
        {
            get { return (string)(lbEmployees.SelectedItem); }
            set { lbEmployees.SelectedValue = value; }
        }

        public User SelectedUser
        {
            get
            {
                string selectedUserId = this.SelectedUserId;
                User user;
                if (Users.Instance.TryGetValue(selectedUserId, out user) == false)
                { return null; }
                return user;
            }
        }

        public User EnsureGetSelectedUser()
        {
            User selectedUser = this.SelectedUser;
            if (selectedUser == null)
            { throw new InvalidOperationException("The requested operation requires a user to be selected"); }
            return selectedUser;
        }

        public UserPickerDialog()
        {
            InitializeComponent();

            string[] employees = Users.Instance.Keys.ToArray();
            lbEmployees.DataSource = employees;
        }

        private void lbEmployees_DoubleClick(object sender, EventArgs e)
        {
            btnOk.PerformClick();
        }

        public static User SelectEmployee(IWin32Window parent, string caption)
        {
            UserPickerDialog dlg = new UserPickerDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Caption = caption;
            
            if (dlg.ShowDialog(parent) != DialogResult.OK)
            { throw new OperationCanceledException(); }

            return dlg.EnsureGetSelectedUser();
        }

        public static User SelectEmployee(string caption)
        {
            return SelectEmployee(null, caption);
        }

        private void lbEmployees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { btnOk.PerformClick(); }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ErrorProvider.Clear();
            if (lbEmployees.SelectedValue == null)
            { 
                ErrorProvider.SetError(btnOk, "Selection is required");
                return;
            }
            
            this.DialogResult = DialogResult.OK;
        }
    }
}
