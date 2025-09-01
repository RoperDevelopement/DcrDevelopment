using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
using SQLite;
namespace BMRMobileApp.Models
{
    public class PSChatGroups: IPSChatGroups
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public string PSGroupName { get; set; }
        [NotNull]
        public string PSGroupDescription { get; set; }
            
        public string PSGroupCreatedDate { get; set; }
        [NotNull]
        public int PSUserID { get; set; }
        public byte[] PSGroupAvatar { get; set; } // URL or path to the group's avatar image
        [NotNull]
        public int PSIsActive { get; set; } // Status of the group (active/inactive)
    }
}
