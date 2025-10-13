using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BMRMobileApp.InterFaces;

 

namespace BMRMobileApp.Models
{
    public class PSJJournalEntry : IPSJournalEntry, IPSTablesIndexID, IUserID, IPSEmotionTagID, IPSJournalGoalsID
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; } // Unique identifier for the feeling note
        [SQLite.NotNull]
        public int PSUserID { get; set; } // Foreign key referencing the user who added the feeling note
        [SQLite.NotNull]
        public string JournalEntry { get; set; } // Additional notes about the feeling
        [SQLite.NotNull]
        public string DateAdded { get; set; } // Date when the feeling was added

        [SQLite.NotNull]
        public int EmotionTagID { get; set; } // The feeling expressed by the user   
        [SQLite.NotNull]
      public  int JournalGoalsID { get; set; } // The feeling expressed by the user   
       
    }
}
