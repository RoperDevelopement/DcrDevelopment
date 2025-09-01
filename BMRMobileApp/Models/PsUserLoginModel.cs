using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
    public class PsUserLoginModel : IPSSUerLoginInformation
    {

        public int ID { get; set; } // Unique identifier for the user
        public string PSUserEmail { get; set; }

        public string PSUserAvatar { get; set; } // URL or path to the user's avatar image
        public string PSUserDisplayName { get; set; } // Display name for the user
        public string PSUserName { get; set; }
      //  public byte[] PSUserProfilePicture { get; set; } // URL or path to the profile picture
      public string PSUserProfilePicture { get; set; } // URL or path to the profile picture
        public string PSUserSex { get; set; } // 0 = Not Specified
        public static readonly Lazy<PsUserLoginModel> instance = new Lazy<PsUserLoginModel>(() => new PsUserLoginModel());


    }
}
