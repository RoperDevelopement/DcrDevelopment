using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
using SQLite;

namespace BMRMobileApp.Models
{
   public class PSupportUsers: IPeerSupportUsers
    {
        
        [PrimaryKey, AutoIncrement,Unique]
        public int ID { get; set; } // Unique identifier for the user
       
        [NotNull]
        public string PSUserCity { get; set; } // User's city
        [NotNull]
        public string PSUserFirstName { get; set; }
        [NotNull]
        public string PSUserLastName { get; set; }
        [NotNull]
        public string PSUserPhoneNumber { get; set; }
        [NotNull]
        public string PSUserState { get; set; }
        [NotNull]
        public string PSUserCountry { get; set; }
        public string PSUserAddress { get; set; } // e.g., Active, Inactive, Suspended
        [NotNull]
        public string PSUserDateOfBirth { get; set; } // User's date of birth
        [NotNull]
        public string PSUserZipCode { get; set; } // User's zip code   

        public string PSUserNotes { get; set; } // Additional notes about the user
        [NotNull]
        public string PSUserDateJoined { get; set; } // Date when the user joined the platform


    }
}
