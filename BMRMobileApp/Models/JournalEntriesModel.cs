using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Models
{
    public class JournalEntriesModel: IPSJournalEntry, IPSTablesIndexID, IUserID, IPSJournalGoalsID, IEmotionsTag
    {
        public int ID { get; set; } // Unique identifier for the feeling note
        public int PSUserID { get; set; } // Foreign key referencing the user who added the feeling note
        public string DateAdded { get; set; } // Date when the feeling was added

       public string JournalEntry { get; set; } // Additional notes about the feeling
        public int JournalGoalsID { get; set; } // The feeling expressed by the user   
        public string Emotion { get; set; }
       public string EmotionIcon { get; set; }
       public string EmotionsColor { get; set; }
        public string JournalEntryGoals {  get; set; }
    }
}
