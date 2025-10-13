using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.InterFaces
{
    interface IPSID
    {
        int ID { get; set; }
    }
    interface IPSIndexID
    {
        int IndexID { get; set; }
    }
    interface IPSSettings: IPSID
    {
        
        int ShowFeelingsPage { get; set; }
        int AutoScroll { get; set; }
        string ScrollWaits { get; set; }
         
    }
    interface IPSUserMoodTag
    {
        string MoodTag { get; set; }
    }
    interface IPSUserMood: IPSID, IPSUserMoodTag
    {
     
      
        double SentimentScore { get; set; }

        public string TimeMood { get; set; }

        public string Mood { get; set; }

        public string BackgroundColor { get; set; }
        
        }
    interface IPSUserMoodTags: IPSID
    {
        string PSMood { get;set; }
        string PSMoodColor { get; set; }

    }
}
 
