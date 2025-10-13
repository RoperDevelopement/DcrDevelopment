using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Models
{
 public   class PSJounalEntryGoals: IPSJournalEntry, IPSTablesIndexID, IUserID
    {
        [PrimaryKey,Unique]
        public int ID { get; set; } // Unique identifier for the feeling note
        [SQLite.NotNull]
        public int PSUserID { get; set; } // Foreign key referencing the user who added the feeling note
        [SQLite.NotNull]
        public string DateAdded { get; set; } // Date when the feeling was added

        
        [SQLite.NotNull]
        public string JournalEntry { get; set; } // Additional notes about the feeling
    }
}
