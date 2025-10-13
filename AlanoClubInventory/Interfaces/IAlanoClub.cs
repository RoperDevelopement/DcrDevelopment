using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Interfaces
{
    public interface IID
    {
        int ID { get; set; }
    }
    public interface IIDCategory
    {
        int CategoryID { get; set; }
    }
    
    public interface ICaegoryName
    {
        string CategoryName { get; set; }
    }
    public interface ICategories
    {
        
         string MemberPrice { get; set; }
         string NonMemberPrice { get; set; }
    }
    public interface IAlanoClubPrices
    {

        float ClubPrice { get; set; }
        float ClubNonMemberPrice { get; set; }
    }
    public interface IAlanoClubProductName
    {
        string ProductName { get; set; }
        //  int TotalSold { get; set; }
    }
    public interface IInventoryProductName: IAlanoClubProductName
    {
       
        float Price { get; set; }
     
        //  int TotalSold { get; set; }
    }
    public interface IInventoryQuanity
    {
     
      int Quantity {  get; set; }
    //  int TotalSold { get; set; }
    }
    public interface IBarItem
    {

        bool BarItem { get; set; }
        //  int TotalSold { get; set; }
    }
    public interface IDateCreated
    {
        DateTime DateCreated { get; set; }
    }
    public interface IDailyTapeTill
    {
        float DailyTillTotal { get; set; }
        float DailyTillTape { get; set; }
        float Depsoit { get; set; }
    }
    public interface IItesSold
    {
        int TotalMemberSold { get; set; }
        int TotalNonMemberSold { get; set; }
        
    }
    public interface IDailyTill
    {
        float DailyTotal { get; set; }
        float DailyTillTotal { get; set; }
        float DailyProductTotal { get; set; }
    }
    public interface IalanoClubProdID
    {
               int AlanoClubProductID { get; set; }
    }
    public interface IALanoCLubCurrentInventory: IID, IAlanoClubProductName
    {
        int Quantity { get; set; }
       int InStock { get; set; }
        int ItemsSold { get; set; }
        int InventoryCurrent { get; set; }
        int NewCount { get; set; }

    }
}
