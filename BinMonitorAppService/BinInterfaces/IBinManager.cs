using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitor.BinInterfaces
{
    public interface IBinRegistration
    {
        Guid BatchID
        { get; set; }
        string BinID
        { get; set; }
        string CategoryName
        { get; set; }
        string RegCreatedBy
        { get; set; }
        string RegAssignedBy
        { get; set; }
        string RegAssignedTo
        { get; set; }
        DateTime RegStartedAt
        { get; set; }
        string RegCompletedBy
        { get; set; }
        DateTime RegCompletedAt
        { get; set; }
        string RegDuration
        { get; set; }
        string LabRecNumber
        { get; set; }
    }
    public interface IBinClosed
    {

        string BinClosedBy
        { get; set; }
        DateTime BinCompletedAt
        { get; set; }
        DateTime ClosedCreatedAt
        { get; set; }
        string CompleteDuration
        { get; set; }

        DateTime DateBatchPickedUp
        { get; set; }
        string BatchPickedUpBy
        { get; set; }
    }

    public interface ICategoryCheckPoints
    {

        string CategoryCheckPointOneDuration
        { get; set; }
        string CategoryColorCheckPointOne
        { get; set; }
        string CategoryCheckPointTwoDuration
        { get; set; }
        string CategoryColorCheckPointTwo
        { get; set; }
        string CategoryCheckPointThreeDuration
        { get; set; }
        string CategoryColorCheckPointThree
        { get; set; }
        string CategoryCheckPointFourDuration
        { get; set; }

        string CategoryColorCheckPointFour
        { get; set; }
    }
    public interface IBinContentsComments
    {

        string BinComments
        { get; set; }
        string BinContents
        { get; set; }

    }

    public interface IBinsLabRecNum
    {
        string BinID
        { get; set; }

        string LabRecNumber
        { get; set; }
        bool AcvitveBin
        { get; set; }
        DateTime StartTime
        { get; set; }
        string Category
        { get; set; }

        string BinAssignedTo
        { get; set; }

        bool BinRegStarted
        { get; set; }
        bool BinRegCompleted
        { get; set; }
        bool BinProcessCompleted
        { get; set; }
        bool BinProcessStarted
        { get; set; }
    }

    public interface IBinUsers
    {
        string Cwid
        { get; set; }
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
        string UserPW
        { get; set; }
        string EmailAddress
        { get; set; }
        string UserProfile
        { get; set; }

        DateTime DateLastLogin
        { get; set; }

        DateTime DatePasswordLastChanged
        { get; set; }

        string UserProfileName
        { get; set; }
    }
    public interface IBinProcessBin
    {
        string ProcessAssignedBy
        { get; set; }
        string ProcessAssignedTo
        { get; set; }
        DateTime ProcessStartAt
        { get; set; }
        string ProcessCompletedBy
        { get; set; }
        DateTime ProcessCompletedAt
        { get; set; }
        string ProcessDuration
        { get; set; }

    }
    public interface IBinCategory
    {
        int CategoryID
        { get; set; }
        string CategoryName
        { get; set; }


    }
    public interface IBatchIdBinId
    {
        Guid BatchId
        { get; set; }
        string BinId
        { get; set; }

    }
    public interface ICategoriesName
    {
        string CategoryName
        { get; set; }


        string CategoryColor
        { get; set; }

        int CategoryDurationHrs
        { get; set; }
    }
    public interface IActiveBins : ICategoriesName
    {
        string BinID
        { get; set; }


        bool AcvitveBin
        { get; set; }
        DateTime StartTime
        { get; set; }


        string BinAssignedTo
        { get; set; }

        bool BinRegStarted
        { get; set; }
        bool BinRegCompleted
        { get; set; }
        bool BinProcessCompleted
        { get; set; }
        bool BinProcessStarted
        { get; set; }
    }
    public interface IUserProfRights
    {
        string BinUserights
        { get; set; }

        string ChangeUsersPasswords
        { get; set; }
        string CreateUsers
        { get; set; }
        string CreateNewProfiles
        { get; set; }
        string DeleteUsers
        { get; set; }


        string EditUsers
        { get; set; }
        string ManageUserProfiles
        { get; set; }
        string RunReports
        { get; set; }
        string TransFerBins
        { get; set; }
        string TransFerCategories
        { get; set; }

        string EmailReports
        { get; set; }
        string Categories
        { get; set; }


    }

    public interface IUserFormPermission : IUserMenu
    {
        string Cwid
        { get; set; }
        bool Admin
        { get; set; }
        bool ChangeUsersPasswords
        { get; set; }
        bool CreateUsers
        { get; set; }
        bool DeleteUsers
        { get; set; }


        bool EditUsers
        { get; set; }
        bool ManageUserProfiles
        { get; set; }
        bool CreateNewProfiles
        {
            get; set;
        }
        string ProfileName
        { get; set; }

    }

    public interface IUserMenu
    {
        bool RunReports
        { get; set; }
        bool TransFerBins
        { get; set; }
        bool TransFerCategories
        { get; set; }

        bool EmailReports
        { get; set; }
        bool Categories
        { get; set; }

    }
    public interface IUserSqlPermission
    {


        int ChangeUsersPasswords
        { get; set; }
        int CreateUsers
        { get; set; }
        int DeleteUsers
        { get; set; }


        int EditUsers
        { get; set; }
        int ManageUserProfiles
        { get; set; }
        int CreateNewProfiles
        {
            get; set;
        }
        int RunReports
        { get; set; }

        int TransFerBins
        { get; set; }
        int TransFerCategories
        { get; set; }
        string ProfileName
        { get; set; }
        int EmailReports
        { get; set; }
        int Categories
        { get; set; }
    }

    public interface ITransFer
    {
        Guid BatchId
        { get; set; }
        string BinID
        { get; set; }
        string CategoryName
        { get; set; }
        string LabReqNumber
        { get; set; }
        string Comments
        { get; set; }
        string OldBinId
        { get; set; }
        string Processing
        { get; set; }
         string TransFerType
        { get; set; }
        string TransferBy
        { get; set; }
    }

    public interface IEmailSettings
    {
        string EmailServer
        { get; set; }
        string EmailFrom
        { get; set; }
        string EmailPassWord
        { get; set; }
        int EmailPort
        { get; set; }
        string EmailTo
        { get; set; }
        string EmailCC
        { get; set; }
        string TextTo
        { get; set; }
        string TextCC
        { get; set; }
    }

    public interface IUsageReport
    {
        string BinID
        { get; set; }
        string CWID
        { get; set; }

        string CategoryName
        { get; set; }

        int TotalLabReqs
        { get; set; }
    }

    public interface IDelLabReqsReport : IBatchIdBinId
    {
        string LabRecNumber
        { get; set; }
          string CategoryName
        { get; set; }
          string RegCreatedBy
        { get; set; }
          string RegAssignedBy
        { get; set; }
          string RegAssignedTo
        { get; set; }
          DateTime RegStartedAt
        { get; set; }
          string RegCompletedBy
        { get; set; }
          DateTime RegCompletedAt
        { get; set; }
          string RegDuration
        { get; set; }
          DateTime DeletedAt
        { get; set; }
          string DeletedBy
        { get; set; }
    }
    public interface IUsageReportByVolDur
    {

        string CWID
        { get; set; }

        public int TotalVolume
        { get; set; }

        public string TotalDur
        { get; set; }
    }


    public interface ITransFerRep
    {
      int ID
        { get; set; }
        string OldCategoryName
        { get; set; }
        string OldLabReqNumber
        { get; set; }
        
         
        string OldProcAssignedTo
        { get; set; }
        string OldRegAssignedTo
        { get; set; }
        string RegAssignedTo
        { get; set; }
        DateTime TransferTime
        { get; set; }
    }
    public interface IEmailService
    {
        public void SendEmail(string message, string subject, IEmailSettings emailSettings);
        public void SendEmail(string message, string subject, bool sendText, IEmailSettings emailSettings);
        public void SendHtmlEmail(string htmlMessage, string subject, IEmailSettings emailSettings);
        public void SendHtmlEmail(string htmlMessage, string subject, IEmailSettings emailSettings, string cwid, string userEmailAddress);
    }
}
