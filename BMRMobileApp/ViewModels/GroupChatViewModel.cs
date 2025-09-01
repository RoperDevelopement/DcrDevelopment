using BMRMobileApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace BMRMobileApp.ViewModels
{
    public class GroupChatViewModel : ObservableObject
    {
        public ObservableCollection<ChatMessageModel> Messages { get; } = new();
        public string OutgoingText { get; set; }
        public string CurrentUser { get; set; } = "DAN"; // Replace with dynamic user ID

        public ICommand SendCommand => new Command(SendMessage);

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(OutgoingText)) return;

            Messages.Add(new ChatMessageModel
            {
                Sender = CurrentUser,
                SenderMessage = OutgoingText,
               // BubbleColor = Colors.Blue
            });

            // Simulate group reply
            Messages.Add(new ChatMessageModel
            {
                Sender = "Alex",
                SenderMessage = "Got it!",
               // BubbleColor = Colors.Gray
            });

            OutgoingText = string.Empty;
            OnPropertyChanged(nameof(OutgoingText));
        }
    }

}
