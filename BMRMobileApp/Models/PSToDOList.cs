using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
    internal class PSToDOList : IPSUserToDOList
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        [SQLite.NotNull]
        public int PSUserID { get; set; }
        [SQLite.NotNull]
        public string TodoItem { get; set; } // The feeling expressed by the user   
        [SQLite.NotNull]
        public byte[] EmojiTag { get; set; } // Additional notes about the feeling
        [SQLite.NotNull]
        public string DateToDoItemAdded { get; set; } // The feeling expressed by the user   
    }
}
