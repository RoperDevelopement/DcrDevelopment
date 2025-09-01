using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.Models;
namespace BMRMobileApp.Utilites
{
    public class ChatServiceEvent
    {
        public event Action<ChatHubModel> MessageReceived;

        public void ReceiveMessage(ChatHubModel msg)
        {
            MessageReceived?.Invoke(msg);
        }
         
    }

}
