using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class VolunteerModel : IID, IUserFullName, IUserName
    {
        public int ID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPhoneNumber  {  get; set; }
        public string UserName { get; set; }
    }
}
