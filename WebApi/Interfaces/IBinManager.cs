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
            {get;set;}
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
    public interface IBinReportModel
    {
        string BinID
        { get; set; }
        string LabReqNum
        { get; set; }
        string RegCreatedBY
        { get; set; }
        string ProcessCreatedBY
        { get; set; }
        string BinCLosedBY
        { get; set; }
        DateTime LabReqRegStDate
        { get; set; }

        DateTime LabReqRegEndDate
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

    public interface IActiveBins: ICategoriesName
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
        string DiplayName
        { get; set; }
        string Id
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
    public interface ICategories
    {
         string CategoryName
        { get; set; }
         int CategoryId
        { get; set; }

         string CategoryColor
        { get; set; }

        int CategoryDurationHrs
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
    public interface IBatchIdBinId
    {
        Guid BatchId
        { get; set; }
        string BinId
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

}
