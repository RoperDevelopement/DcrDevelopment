using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using BMRMobileApp.InterFaces;
using SQLite;
namespace BMRMobileApp.Models
{
    public class PSChatMessages: IPSChatMessage
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public int   PSUserID { get; set; }
        [NotNull]
        public int PSGroupID { get; set; }
        // public int PSGroupChatMessageID { get; set; }
        //  public int PSMessageID { get; set; }
        [NotNull]
        public string PSMessage { get; set; }
         [NotNull]
        public string PSMessageTimestamp { get; set; }
        
        public int PSReplayto { get; set; }
        public bool IsMessageRead { get; set; }
        public byte[] MessageAttachment { get; set; }
    }
}
