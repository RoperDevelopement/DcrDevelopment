using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Models
{
    public class UserFeelingModel: IPeerSupportUsers
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime FeelingDateTime { get; set; }
        public string NotesFeeling { get; set; }
        public string FeelingType { get; set; }
        public string FeelingColor { get; set; }
        public string FeelingIcon { get; set; }
        public string FeelingEmoji { get; set; }
        // Uncomment if needed in the future
        // public bool IsFeelingShared { get; set; }
        // public bool IsFeelingPublic { get; set; }
        // public bool IsFeelingPrivate { get; set; }
        // public bool IsFeelingAnonymous { get; set; }
        // public bool IsFeelingVisibleToPeerSupportUsers { get; set; }
    }
     
     
}
