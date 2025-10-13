using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.Models;
//https://outlook.live.com/mail/0/ dcrpsmobleapp@outlook.com
namespace BMRMobileApp.InterFaces
{
    public interface IPeerSupportUsers : IPSTablesIndexID
    {
        // Properties for Peer Support User details

        string PSUserCity { get; set; } // User's city

        string PSUserFirstName { get; set; }
        string PSUserLastName { get; set; }
        string PSUserPhoneNumber { get; set; }
        string PSUserState { get; set; }
        string PSUserCountry { get; set; }
        string PSUserAddress { get; set; } // e.g., Active, Inactive, Suspended
        string PSUserDateOfBirth { get; set; } // User's date of birth
        string PSUserZipCode { get; set; } // User's zip code   
        string PSUserNotes { get; set; } // Additional notes about the user
        string PSUserDateJoined { get; set; } // Date when the user joined the platform

        //  bool IsFeelingShared { get; set; }
        // bool IsFeelingPublic { get; set; }
        //bool IsFeelingPrivate { get; set; }
        //bool IsFeelingAnonymous { get; set; }
        //bool IsFeelingVisibleToPeerSupportUsers { get; set; }
    }
     
    public interface IPSTablesIndexID
    {
        int ID { get; set; }
    }
    public interface IUserID
    {
        int PSUserID { get; set; }
    }

    public interface IPSUserLogin : IPSTablesIndexID
    {
        //byte[] PSUserProfilePicture { get; set; } // URL or path to the profile picture
        string PSUserProfilePicture { get; set; } // URL or path to the profile picture
        string PSUserName { get; set; }
        string PSUserPassword { get; set; }
        string PSUserEmail { get; set; }
        byte[] PSUserAvatar { get; set; } // URL or path to the user's avatar image
        string PSUserDisplayName { get; set; } // Display name for the user
        string PSUserPWSalt { get; set; } // Salt for password hashing
        string PSUserLastLogin { get; set; } // Last login date and time
        string PSUserPassWordLastChanged { get; set; } // Last password change date and time
        string PSPasswordHash { get; set; } // Hashed password for security
        int PSUserSex { get; set; } // 0 = Not Specified



    }
    public interface IPSSUerLoginInformation : IPSTablesIndexID
    {
        string PSUserEmail { get; set; }
        string PSUserAvatar { get; set; } // URL or path to the user's avatar image
        string PSUserDisplayName { get; set; } // Display name for the user
        string PSUserName { get; set; }
        
        
    }
    public interface IAppExitService
    {
        Task RequestExitAsync();
    }
    public interface IPSFeeling
    {
        string Feeling { get; set; } // The feeling expressed by the user   
    }
    public interface IPSEmotionTagID
    {
        int EmotionTagID { get; set; } // The feeling expressed by the user   
    }
    public interface IPSJournalGoalsID
    {
        int JournalGoalsID { get; set; } // The feeling expressed by the user   
    }

    public interface IPSJournalEntry 
    {
        string DateAdded { get; set; } // Date when the feeling was added
        
        string JournalEntry { get; set; } // Additional notes about the feeling
    }
    public interface IPSUserToDOList : IPSTablesIndexID, IUserID
    {
        string TodoItem { get; set; } // The feeling expressed by the user   
        byte[] EmojiTag { get; set; } // Additional notes about the feeling
        string DateToDoItemAdded { get; set; } // The feeling expressed by the user   
    }
    public interface IPSChatGroup
    {
        int PSGroupID { get; set; }
    }
    public interface IPSChatMessage : IPSTablesIndexID, IUserID, IPSChatGroup
    {
        string PSMessage { get; set; }
        string PSMessageTimestamp { get; set; }
        int PSReplayto { get; set; }
        bool IsMessageRead { get; set; }

        byte[] MessageAttachment { get; set; } // Optional attachment (e.g., image, file)
    }


    public interface IPSGroupMembers : IPSTablesIndexID, IPSChatGroup
    {
        string PSUserDateJoined { get; set; }
        string PSUserRole { get; set; } // e.g., Admin, Member, Moderator
        bool PSIsActive { get; set; } // Membership status





    }
    public interface IPSChatUserRoles : IPSTablesIndexID
    {
        string PSUserRole { get; set; } // e.g., Admin, Member, Moderator
        string PSRoleDescription { get; set; } // Description of the role
        string PSRolePermissions { get; set; } // Permissions associated with the role (e.g., Read, Write, Delete)
    }
    public interface IPSChatGroups : IPSTablesIndexID, IUserID
    {
        string PSGroupName { get; set; }
        string PSGroupDescription { get; set; }
        string PSGroupCreatedDate { get; set; }
        int  PSIsActive { get; set; } // e.g., Active, Inactive, Archived
        public byte[] PSGroupAvatar { get; set; }
    }
    public interface  IChatMessage
    {

          string Sender { get; set; }
          string SenderMessage { get; set; }
          string TimesMessageSent { get; set; }
      //  Color BubbleColor { get; set; }
    }
    public interface IConnectionMonitorService
    {
        event Action<bool>? ConnectionStatusChanged;
        bool IsConnected { get; }
        Task StartMonitoringAsync();
    }
    public interface IChatRelayService
    {
        event Action<ChatMessageModel>? MessageReceived;
        event Action<bool>? ConnectionStatusChanged;

        Task SendMessageAsync(ChatMessageModel message);
        Task StartAsync();
        bool IsConnected { get; }
    }
}
