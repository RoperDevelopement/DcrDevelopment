using AlanoClubInventory.Interfaces;
using AlanoClubInventory.SqlServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AlanoClubInventory.Models
{
  public  class LoginUserModel: IUserEmailAddress, IUserName, IUserID,IUserIntils, IIsUserAdmin
    {
        private static LoginUserModel loginInstance;
        private static readonly object _lock = new object();
        private LoginUserModel() { }
        public static LoginUserModel LoginInstance
        {
            get
            {
                // Double-checked locking for thread safety
                if (loginInstance == null)
                {
                    lock (_lock)
                    {
                        if (loginInstance == null)
                        {
                            loginInstance = new LoginUserModel();
                        }
                    }
                }
                return loginInstance;
            }
        }
        public int ID { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string UserIntils { get; set; }
    }
}
