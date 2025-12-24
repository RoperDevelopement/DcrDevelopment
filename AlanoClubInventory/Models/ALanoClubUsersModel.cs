using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
     
 public   class ALanoClubUsersModel: IUserCredentials, IUserID, IUserFullName, IUserEmailAddress, IUserPassWprdString, IUserAtive,IUserIntils
    {
        public int ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserPassword { get; set; } =string.Empty;
        public DateTime DateLastLoggedIn { get; set; }
        public string Salt { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmailAddress { get; set; } = string.Empty;
        public string UserPasswordString { get; set; } = string.Empty;
        public string UserPhoneNumber { get; set; } = string.Empty;
        public string UserPasswordReversed { get; set; }= string.Empty;
       public string UserIntils { get; set; }

    }
}
