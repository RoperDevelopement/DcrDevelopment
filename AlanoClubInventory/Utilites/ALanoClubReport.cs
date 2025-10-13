using AlanoClubInventory.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Navigation;

namespace AlanoClubInventory.Utilites
{
    public class ALanoClubReport
    {
        public async Task<List<AlanoCLubMontlyProductsSoldModel>> GenerateMonthlyInventorySalesAsync(IList<AlanoClubReportModel> alanoClubReports)
        {

            var grouped = alanoClubReports
//.GroupBy(r => r.DateCreated,r.BarItem) // Group by date only (ignore time)
.GroupBy(p => new { p.DateCreated.Month, p.DateCreated.Year, p.ProductName })
.Select(g => new
{

    Date = g.Key,
    Month = g.Key.Month,
    ProductName=g.Key.ProductName,
    // ID = g.First().ID,
    //  BarITtemsTotal = g.Sum(item => item.TotalMemberSold) + g.Sum(item => item.TotalNonMemberSold),
    //  BarITtemsCost = g.Sum(item => item.TotalMemberSold * item.ClubPrice) + g.Sum(item => item.TotalNonMemberSold * item.ClubNonMemberPrice)
    TotalMemBerProductsSold = g.Sum(item => item.TotalMemberSold),
    TotalNonMemBerProductsSold = g.Sum(item => item.TotalNonMemberSold),
    TotalMemBerPrice = g.Sum(item => item.MemBerPriceTotal),
    TotalNonMemPrice = g.Sum(item => item.NonMemBerPriceTotal),



})
.OrderBy(x => x.Date.Month+x.ProductName)
.ToList();
            return grouped.Select(x => new AlanoCLubMontlyProductsSoldModel { 
                Month = x.Month,
                ProductName= x.ProductName,
                TotalMemBerProductsSold= x.TotalMemBerProductsSold,
                TotalNonMemBerProductsSold= x.TotalNonMemBerProductsSold,
                TotalMemBerPrice= x.TotalMemBerPrice,
                TotalNonMemBerPrice= x.TotalNonMemPrice


            }).ToList();
        }

        public async Task<List<AlanoCLubMontlySalesDepositsModel>> GenerateMonthlySalesAsync(IList<AlanoCLubDailyTillTapeModel> alanoClubReports)
        {
            
                      var grouped = alanoClubReports
       //.GroupBy(r => r.DateCreated,r.BarItem) // Group by date only (ignore time)
       .GroupBy(p => new { p.DateCreated.Month,p.DateCreated.Year })
        .Select(g => new
        {

            Date = g.Key,
            Month = g.Key.Month,

            // ID = g.First().ID,
            //  BarITtemsTotal = g.Sum(item => item.TotalMemberSold) + g.Sum(item => item.TotalNonMemberSold),
            //  BarITtemsCost = g.Sum(item => item.TotalMemberSold * item.ClubPrice) + g.Sum(item => item.TotalNonMemberSold * item.ClubNonMemberPrice)
            TotalTillSale = g.Sum(item => item.DailyTillTotal),
            TotalTape = g.Sum(item => item.DailyTillTape),
            TotalDeposit = g.Sum(item => item.Depsoit),



        })
        .OrderBy(x => x.Date.Month)
         .ToList();
            return grouped.Select(x => new AlanoCLubMontlySalesDepositsModel
            {
               Month= x.Month,
               TotalDeposit= x.TotalDeposit,
                TotalTape= x.TotalTape,
                TotalTillSale= x.TotalTillSale

            }).ToList();
        }
        
        public async Task<List<BarItemsSoldModel>> GenerateReportAsync(IList<AlanoClubReportModel> alanoClubReports)
        {
            var grouped = alanoClubReports
       //.GroupBy(r => r.DateCreated,r.BarItem) // Group by date only (ignore time)
       .GroupBy(p => new { p.DateCreated.Date })
        .Select(g => new
        {

            Date = g.Key,
            BarItem = g.First().BarItem,

            // ID = g.First().ID,
            //  BarITtemsTotal = g.Sum(item => item.TotalMemberSold) + g.Sum(item => item.TotalNonMemberSold),
            //  BarITtemsCost = g.Sum(item => item.TotalMemberSold * item.ClubPrice) + g.Sum(item => item.TotalNonMemberSold * item.ClubNonMemberPrice)
            BarITtemsCost = g.Sum(item => item.DailyProductTotal),


        })
        .OrderBy(x => x.Date.Date)
         .ToList();
            return grouped.Select(x => new BarItemsSoldModel
            {
                Date = x.Date.Date,

                BarItem = x.BarItem,
                //   ID = x.ID,
                //BarITtemsTotal = x.BarITtemsTotal,
                BarITtemsCost = x.BarITtemsCost
            }).ToList();
        }

        public async Task<List<CategorySoldModel>> GenerateReportCatAsync(IList<AlanoClubReportModel> alanoClubReports)
        {
            var grouped = alanoClubReports
       //.GroupBy(r => r.DateCreated,r.BarItem) // Group by date only (ignore time)
       .GroupBy(p => new { p.DateCreated.Date, p.CategoryName })
        .Select(g => new
        {

            Date = g.Key.Date,
            CategoryName = g.Key.CategoryName,
            Baritem = g.First().BarItem,
            // CategoryName = g.First().CategoryName,
            ID = g.First().CategoryID,
            //  BarITtemsTotal = g.Sum(item => item.TotalMemberSold) + g.Sum(item => item.TotalNonMemberSold),
            //  BarITtemsCost = g.Sum(item => item.TotalMemberSold * item.ClubPrice) + g.Sum(item => item.TotalNonMemberSold * item.ClubNonMemberPrice)
            CateogoryITtemsCost = g.Sum(item => item.DailyProductTotal),


        })
        .OrderBy(x => x.Date.Date)
              .ToList();
            var catItem = grouped.Select(x => new CategorySoldModel
            {
                Date = x.Date.Date,
                CategoryName = x.CategoryName,
                BarItem = x.Baritem,
                ID = x.ID,
                //BarITtemsTotal = x.BarITtemsTotal,
                CateogoryITtemsCost = x.CateogoryITtemsCost
            }).ToList();
            return catItem.Where(t => !t.BarItem).ToList();
        }



        public async Task<List<CategorySoldModel>> GetCatAsyncNoBarItems(IList<CategorySoldModel> categorySolds)
        {
            return categorySolds.Where(t => !t.BarItem).ToList();

        }
        public async Task<IList<AlanoClubReportModel>> GetAsyncBarItems(IList<AlanoClubReportModel> barItemsSoldModel)
        {
            return barItemsSoldModel.Where(t => t.BarItem).ToList();

        }
        public async Task<IList<AlanoClubReportModel>> GetAsyncNonBarItems(IList<AlanoClubReportModel> barItemsSoldModel)
        {
            return barItemsSoldModel.Where(t => !t.BarItem).ToList();

        }


        public async Task<IList<AlanClubPrintReportModel>> AddBarItems(IList<BarItemsSoldModel> barItemsSoldModel, IList<AlanClubPrintReportModel>? clubPrintReportModels)
        {
            // if (clubPrintReportModels == null)
            // {
            //     clubPrintReportModels = new List<AlanClubPrintReportModel>();
            // }
            // if (clubPrintReportModels.Count > 0)
            // {
            //     clubPrintReportModels.Clear();
            // }

            foreach (var item in barItemsSoldModel)
            {
                var report = clubPrintReportModels?.FirstOrDefault(d => d.TillDate.Date == item.Date.Date);
                if (report != null)
                {
                    report.TillDate = item.Date;
                    report.BarItemsTotal = item.BarITtemsCost;




                }
            }

            return clubPrintReportModels;
        }
        public async Task<IList<AlanClubPrintReportModel>> InitReportWithDates(DateTime sDate, DateTime eDate)
        {
            IList<AlanClubPrintReportModel> clubPrintReportModels = new List<AlanClubPrintReportModel>();

            for (DateTime date = sDate.Date; date <= eDate; date = date.AddDays(1))
            {
                clubPrintReportModels.Add(new AlanClubPrintReportModel
                {
                    TillDate = date.Date,
                    BarItemsTotal = 0.00f,
                    NonBarItemsTotal = 0.00f,
                    TotalSales = 0.0f,

                    Dues = 0.00f,
                    CoffeeClub = 0.00f,
                    Coins = 0.00f,
                    Donations = 0.00f,
                    DailyTotalOther = 0.00f,
                    TotalTotal = 0.00f,
                    Deposit = 0.00f,
                    Tape = 0.00f,
                    OverShort = 0.00f,
                    MiscItems = 0.00f
                });

            }
            await Task.CompletedTask;
            return clubPrintReportModels;

        }
        public async Task<IList<AlanClubPrintReportModel>> AddOtherItems(IList<CategorySoldModel> categorySolds, IList<AlanClubPrintReportModel>? clubPrintReportModels)
        {
            var distinctDates = categorySolds
                                .Select(dt => dt.Date.Date)
                                .Distinct()
                                .ToList();
            foreach (var item in distinctDates)
            {
                var report = clubPrintReportModels?.FirstOrDefault(d => d.TillDate.Date == item.Date.Date);
                var catItems = categorySolds.Where(c => c.Date.Date == item.Date.Date).ToList();
                if ((catItems != null) && (catItems.Count > 0))
                {
                    foreach (var cat in catItems)
                    {
                        if (cat.CategoryName.Equals(Utilites.AlanoCLubConstProp.Dues, StringComparison.OrdinalIgnoreCase))
                        {
                            report.Dues += cat.CateogoryITtemsCost;
                        }
                        else if (cat.CategoryName.StartsWith(Utilites.AlanoCLubConstProp.CoffeeClub, StringComparison.OrdinalIgnoreCase))
                        {
                            report.CoffeeClub += cat.CateogoryITtemsCost;
                        }
                        else if (cat.CategoryName.Equals(Utilites.AlanoCLubConstProp.Donations, StringComparison.OrdinalIgnoreCase))
                        {
                            report.Donations += cat.CateogoryITtemsCost;
                        }
                        else if (cat.CategoryName.Equals(Utilites.AlanoCLubConstProp.Rent, StringComparison.OrdinalIgnoreCase))
                        {
                            report.Rent += cat.CateogoryITtemsCost;
                        }
                        else
                        {
                            report.MiscItems += cat.CateogoryITtemsCost;
                        }
                    }

                    // report.TillDate = item.Date;
                    // report.BarItemsTotal = item.BarITtemsCost;




                }
            }

            return clubPrintReportModels;
        }
        public async Task<IList<AlanClubPrintReportModel>> GetTapeTotals(IList<AlanoCLubDailyTillTapeModel> alanoCLubDailyTillTapes, IList<AlanClubPrintReportModel>? clubPrintReportModels)
        {
            foreach (var tape in alanoCLubDailyTillTapes)
            {
                var report = clubPrintReportModels?.FirstOrDefault(d => d.TillDate.Date == tape.DateCreated.Date);
                // int index = clubPrintReportModels?.FindIndex(d => d.TillDate.Date == tape.DateCreated.Date);
                if (report != null)
                {
                    report.Tape = tape.DailyTillTape;
                    report.TotalSales = tape.DailyTillTotal;
                    report.OverShort =   tape.DailyTillTape- tape.DailyTillTotal;
                    // report.OverShort = 10 - 5;
                    report.Deposit = tape.Depsoit;
                }

            }

            return clubPrintReportModels;

        }
        public async Task<IList<AlanoCLubDailyTillTapeModel>> GetMonthlySales(string sqlConnectionStr, string storedProcedueName)
        {
            return await SqlServices.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoCLubDailyTillTapeModel>(sqlConnectionStr, storedProcedueName);
        }
    }
}

