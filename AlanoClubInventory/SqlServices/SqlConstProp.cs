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





        #endregion
    }
}


