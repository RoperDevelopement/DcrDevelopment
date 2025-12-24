using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class VolunteerHoursModel: IUserFullName, IUserID, IUserEmailAddress, IUserName,IUserClLockedInOut
    {
        public int ID { get; set; }
        public string UserName { get; set; } = string.Empty;
       public string UserFirstName { get; set; }= string.Empty;
      public  string UserLastName { get; set; }=string.Empty;
        public string UserPhoneNumber { get; set; } = string.Empty;
        public string UserEmailAddress { get; set; } = string.Empty;
       public DateTime DateTimeClockedIn { get; set; }
      public  DateTime DateTimeClockedOut { get; set; }
        public double TotalHours { get; set; } = 0.00;
    }
}
