using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Models
{
    public class ChatMessageModel: IChatMessage
    {
       public string Sender { get; set; }
      public  string SenderMessage { get; set; }
        public string TimesMessageSent { get; set; } = DateTime.Now.ToString();
      ///  public Color BubbleColor { get; set; }
    }
}
