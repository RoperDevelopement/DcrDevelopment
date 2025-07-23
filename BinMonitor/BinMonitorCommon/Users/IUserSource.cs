using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BinMonitor.Common
{
    public class SelectedUserChangedEventArgs : EventArgs
    {
        public User SelectedUser { get; private set; }

        public SelectedUserChangedEventArgs(User selectedUser) 
            : base()
        { this.SelectedUser = selectedUser; }
    }

    public interface IUserSource
    {
        event EventHandler<SelectedUserChangedEventArgs> SelectedUserChanged;

        User SelectedUser { get; }

        User EnsureGetSelectedUser();

        void SelectUser(User user);

        void EnsureSelectUser(User user);

        void Clear();
    }
}
