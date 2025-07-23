using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public class UsersComboBox : SerializedObjectDictionaryComboBox<User>, IUserSource
    { 
        [Browsable(false)]
        public User SelectedUser
        { get { return base.SelectedValue; } }

        public User EnsureGetSelectedUser()
        { return base.EnsureGetSelectedValue(); }

        public void SelectUser(User user)
        { base.TrySelectKey(user.DisplayName); }

        public void EnsureSelectUser(User user)
        { base.EnsureSelectKey(user.DisplayName); }

        public UsersComboBox() : base()
        { 
            base.Source = Users.Instance;
            base.SelectedKeyChanged += UsersComboBox_SelectedKeyChanged;
        }

        private void UsersComboBox_SelectedKeyChanged(object sender, EventArgs e)
        { OnSelectedUserChanged(this.SelectedUser); }

        public event EventHandler<SelectedUserChangedEventArgs> SelectedUserChanged;

        protected void OnSelectedUserChanged(User currentUser)
        { NotifySelectedUserChanged(currentUser); }

        protected void NotifySelectedUserChanged(User currentUser)
        {
            EventHandler<SelectedUserChangedEventArgs> handler = this.SelectedUserChanged;
            if (handler != null)
            { handler(this, new SelectedUserChangedEventArgs(currentUser)); }
        }

        public void Clear()
        { base.ClearSelection(); }
    }
}
