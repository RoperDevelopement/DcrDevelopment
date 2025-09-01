using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
    public class PSUsersLogin: IPSUserLogin
    {

        [NotNull,Unique]
        public int ID { get; set; } // Unique identifier for the user
        [PrimaryKey,NotNull, Unique]
        public string PSUserName { get; set; }
        [NotNull]
        public string PSUserPassword { get; set; }
        [NotNull]
        public string PSUserEmail { get; set; }
        [NotNull]
        public byte[] PSUserAvatar { get; set; } // URL or path to the user's avatar image
        [NotNull]
        public string PSUserDisplayName { get; set; } // Display name for the user
        [NotNull]
        public string PSUserPWSalt { get; set; } // Salt for password hashing
        [NotNull]
        public string PSUserLastLogin { get; set; } // Last login date and time
        [NotNull]
        public string PSUserPassWordLastChanged { get; set; } // Last password change date and time
        [NotNull]
        public string PSPasswordHash { get; set; } // Hashed password for security
        [SQLite.NotNull]
        public string PSUserProfilePicture { get; set; } // URL or path to the profile picture
        [SQLite.NotNull]
        public int PSUserSex { get; set; } // 0 = Not Specified
                                           //public byte[] PSUserProfilePicture { get; set; } // URL or path to the profile picture

    }
}
