using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.SqlServices
{
    public class SqlConstProp
    {
        #region sp
        public const string SPAddUpdateCatagories = "spAddEditDeleteCatagories";
        public const string SPGetCategories = "spGetCategories";
        public const string SPAddEditDeleteProducts = "spAddEditDeleteProducts";
        public const string SPGetInventory = "spGetInventory";
        public const string SPAddEditDeleteAlconCLubInventory = "spAddEditDeleteAlconCLubInventory";
        public const string SPGetCategoriesBarItems = "spGetCategoriesBarItems";
        public const string SPAddEditDeleteAlconCLubOtherInventory = "spAddEditDeleteAlconCLubOtherInventory";
        public const string SPGetCategoriesNotBarItems = "spGetCategoriesNotBarItems";
        public const string SPGetOtherInventoryProducts = "spGetOtherInventoryProducts";
        public const string SPAddEditDeleteAlconProducts = "spAddEditDeleteAlconProducts";
        public const string SPGetAlanoCLubProducts = "spGetAlanoCLubProducts";
        public const string SPAlanoClubTillPrices = "spAlanoClubTillPrices";
        public const string SPGetDailyTillReceipts = "spGetDailyTillReceipts";
        public const string SPDailyTellReceipts = "spDailyTellReceipts";
        public const string SPAlanoClubCheckTillDate = "spAlanoClubCheckTillDate";
        public const string SPAlanoCLubDailyTapeTill = "spAlanoCLubDailyTapeTill";
        public const string SPGetDateReoprtLastRan = "spGetDateReoprtLastRan";
        public const string SPGetAlanoCLubReport = "spGetAlanoCLubReport";
        public const string SpGetAlanoCLubTotalsTapeTill = "spGetAlanoCLubTotalsTapeTill";
        public const string SPGetAlanoCLubDailyTapeTill = "spGetAlanoCLubDailyTapeTill";
        public const string SPGetAlanoCLubReportItemsSoldByMonth = "[dbo].[spGetAlanoCLubReportItemsSoldByMonth]";
        public const string SPGetAlanClubInventoryYears = "spGetAlanClubInventoryYears";
        public const string SPGetAlanoClubDailyTillReceiptDate = "spGetAlanoClubDailyTillReceiptDate";
        public const string SPGetAlanoCLubCurrentInventory = "spGetAlanoCLubCurrentInventory";
        public const string SPAlanoCLubCurrentInventory = "spAlanoCLubCurrentInventory";
        public const string SPGetAlanoClubInventoryProfitLoss = "[dbo].[spGetAlanoClubInventoryProfitLoss]";
        public const string SPALanoClubInventroyCount = "spALanoClubInventroyCount";
        public const string SPGetInventoryDates = "spGetInventoryDates";
        public const string SPALanoClubRePrintInventory = "spALanoClubRePrintInventory";
        public const string SPlanoCLubOldDailyTapeTill = "spAlanoCLubOldDailyTapeTill";
        public const string SPGetDailyOldTillReceipts = "spGetDailyOldTillReceipts";
        public const string SPUpdateOldDailyTellReceipts = "spUpdateOldDailyTellReceipts";
        public const string SPUpDateAlanoClubOldTillDrop = "spUpDateAlanoClubOldTillDrop";
        public const string SPALanoClubGetDailySalesGraphChart = "spALanoClubGetDailySalesGraphChart";
        public const string SPCheckMemberIDNotUsed = "spCheckMemberIDNotUsed";
        public const string SPUpDateAddMemebers = "spUpDateAddMemebers";
        public const string SPALanoCLubGetMembers = "spALanoCLubGetMembers";
        public const string SPAddUpdateAlanoClubUsers = "spAddUpdateAlanoClubUsers";
        public const string SPGetAlanoClubUserInfo = "spGetAlanoClubUserInfo";
        public const string SPUpdateUserPassword = "spUpdateUserPassword";
        public const string SPGetALanoCLubUsers = "spGetALanoCLubUsers";
        public const string SPAlanoClubDeleByID = "spAlanoClubDeleByID";
        public const string SPGetNextReceiptNumber = "spGetNextReceiptNumber";
        public const string SPAlanoClubAddRecSig = "spAlanoClubAddRecSig";
        public const string SPAddPayDues = "spAddPayDues";
        public const string SPAlanoCLubAddRecNumber = "spAlanoCLubAddRecNumber";
        public const string SPAddAlanoClubReceipt = "spAddAlanoClubReceipt";
        public const string SPALanoCLubGetMemberByID = "spALanoCLubGetMemberByID";
        public const string SPAlanoClubClockInOutVolHours = "spAlanoClubClockInOutVolHours";
        public const string SPCheckDatabaseOptimize = "spCheckDatabaseOptimize";
        public const string SPGetAlanoClubTableNames = "spGetAlanoClubTableNames";
        public const string SPAddPurgeDataBaseName = "spAddPurgeDataBaseName";
        public const string SPPurgeAlacnoDataBaseData = "spPurgeAlacnoDataBaseData";
        public const string SPGetPrintVolunteerHours = "spGetPrintVolunteerHours";
        public const string SPGetVolunteers = "spGetVolunteers";

        


        #endregion
        #region sp parmas
        public const string SPParmaCategoryName = "@CategoryName";
        public const string SPParmaID = "@ID";
        public const string SPParmaMemberPrice = "@MemberPrice";
        public const string SPParmaNonMemberPrice = "@NonMemberPrice";
        public const string SPParmaDelete = "@Delete";
        public const string SPParmaBarItem = "@BarItem";

        public const string SPParmaCategoryID = "@CategoryID";
        public const string SPParmaProductName = "@ProductName";
        public const string SPParmaQuantity = "@Quantity";
        public const string SPParmaPrice = "@Price";
        public const string SPParmaDateTillReceipt = "@DateTillReceipt";
        public const string SPParmaMemberITems = "@MemberITems";
        public const string SPParmaNonMemberITems = "@NonMemberITems";
        public const string SPParmaDailyProductTotal = "@DailyProductTotal";
        public const string SPParmaDailyTotalSales = "@DailyTotalSales";
        public const string SPParmaItemsPerCase = "@ItemsPerCase";


        public const string SPParmaTapeTotal = "@TapeTotal";

        public const string SPParmaRepSDate = "@RepSDate";
        public const string SPParmaRepEDate = "@RepEDate";
        public const string SPParmaDepsoit = "@Depsoit";
        public const string SPParmaQuanitySold = "@QuanitySold";
        public const string SPParmaInventoryCount = "@InventoryCount";
        public const string SPParmaInventoryItem = "@InventoryItem";
        public const string SPParmaInStock = "@InStock";
        public const string SPParmaItemsSold = "@ItemsSold";
        public const string SPParmaInventoryCurrent = "@InventoryCurrent";
        public const string SPParmaDate = "@Date";
        public const string SPParmaInventorySales = "@InventorySales";

        public const string SPParmaYear = "@Year";
        public const string SPParmaStartMonthYear = "@StartMonthYear";
        public const string SPParmaEndMonthYear = "@EndMonthYear";

        public const string SPParmaMemberID = "@MemberID";

        public const string SPParmaMemberFirstName = "@MemberFirstName";
        public const string SPParmaMemberLastName = "@MemberLastName";
        public const string SPParmaMemberEmail = "@MemberEmail";
        public const string SPParmaMemberPhoneNumber = "@MemberPhoneNumber";
        public const string SPParmaSobrietyDate = "@SobrietyDate";
        public const string SPParmaMembershipEndDate = "@MembershipEndDate";
        public const string SPParmaIsActiveMember = "@IsActiveMember";
        public const string SPParmaIsAdmin = "@IsAdmin";
        public const string SPParmaSalt = "@Salt";
        public const string SPParmaPasswordHash = "@PasswordHash";
        public const string SPParmaUserName = "@UserName";
        public const string SPParmaUserPasswordReversed = "@UserPasswordReversed";
        public const string SPParmaIsBoardMember = "@IsBoardMember";
        public const string SPParmaNumberMonthsDues = "@NumberMonthsDues";
        public const string SPParmaReceiptSignature = "@ReceiptSignature";
        public const string SPParmaRecivedBY = "@RecivedBY";
        public const string SPParmaReceiptNumber = "@ReceiptNumber";
        public const string SPParmaReceiptDesc = "@ReceiptDesc";
        public const string SPParmaUserID = "@UserID";
        public const string SPPurgedDBName = "@PurgedDBName";
        public const string SPDateTimeVolClockedIn = "@DateTimeVolClockedIn";
        public const string SPDateTimeVolClockedOut = "@DateTimeVolClockedOut";
        public const string SPUpDatVolunteerHours = "spUpDatVolunteerHours";

        




        public const string AlanoClubInsertNewTableQuery = "INSERT INTO [{0}].[dbo].[{1}] select * from [AlanoClub].[dbo].[{2}]";
        public const string AlanoCheckPurgeTable = "select [PurgedDBName] from [dbo].[ALanoCLubPurgedDBNames] where [PurgedDBName] = '{0}'";
        //   public const string RestoreDB = @"RESTORE DATABASE [{0}] FROM DISK = N'{1}' WITH REPLACE, RECOVERY;";
        public const string RestoreDB = @"RESTORE DATABASE [{0}] FROM DISK = N'{1}' WITH REPLACE, RECOVERY;";
       public const string SetSingleUser = @"ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
        // Set database back to multi-user mode
       public const string SetMultiUser = @"ALTER DATABASE [{0}] SET MULTI_USER;";
        public const string QueryDataBaseSizeInGB = "SELECT CAST(SUM(size) * 8.0 / 1024 / 1024 AS DECIMAL(18, 2)) AS SizeGB FROM sys.master_files  WHERE database_id = DB_ID(@DbName);";
        public const string QueryMinYear = "SELECT min(Year([DateTillReceipt])) FROM [dbo].[AlanoCLubDailyTillReceipts]";
        public const string QueryGetDatabaseNames = "SELECT name AS DatabaseName FROM sys.databases ORDER BY name;";
        #endregion
    }
}   


