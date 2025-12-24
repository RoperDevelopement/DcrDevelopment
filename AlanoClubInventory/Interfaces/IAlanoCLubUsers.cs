using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Interfaces
{
    public interface IUserName
    {
        string UserName { get; set; }
    }
    public interface IIsUserAdmin : IUserName
    {

        bool IsAdmin { get; set; }

    }
    public interface IUserCredentials : IUserName, IIsUserAdmin
    {



        string UserPassword { get; set; }
        DateTime DateLastLoggedIn { get; set; }
        string Salt { get; set; }

    }

    public interface IUserID
    {
        int ID { get; set; }
    }
    public interface IUserFullName
    {
        string UserFirstName { get; set; }
        string UserLastName { get; set; }
        string UserPhoneNumber { get; set; }
    }
    public interface IUserEmailAddress
    {
        string UserEmailAddress { get; set; }
    }
    public interface IUserPassWprdString
    {
        string UserPasswordString { get; set; }
        string UserPasswordReversed { get; set; }
    }
    public interface IUserAtive
    {
        bool IsActive { get; set; }

    }
    public interface IUserIntils
    {
        string UserIntils { get; set; }
    }
    public interface IUserClLockedInOut
    {
        DateTime DateTimeClockedIn { get; set; }
        DateTime DateTimeClockedOut { get; set; }
        double TotalHours { get; set; }
    }
}
