using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace BinMonitor.Common
{
    public static class UserManager
    {
        public static void AddNewUser(User addedBy, User user)
        {
            if (addedBy.UserProfile.IsAdmin == false)
            { throw new SecurityException(); }

            Users.Instance.Add(user.Id, user);
        }

        public static bool TryRemoveUser(User removedBy, User user)
        {
            if (removedBy.UserProfile.IsAdmin == false)
            { throw new SecurityException(); }

            return Users.Instance.TryRemove(user.Id);
        }

        public static void AssignProfileToUser(User assignedBy, User user, UserProfile profile)
        {
            if (assignedBy.UserProfile.IsAdmin == false)
            { throw new SecurityException(); }

            user.UserProfileId = profile.Id;
        }

    }
}
