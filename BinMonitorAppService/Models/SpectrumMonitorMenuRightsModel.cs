using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class SpectrumMonitorMenuRightsModel:IUserMenu

    {
       public bool RunReports
        { get; set; } = false;
        public bool TransFerBins
        { get; set; } = false;
       public bool TransFerCategories
        { get; set; } = false;

        public bool EmailReports
        { get; set; } = false;
        public bool Categories
        { get; set; } = false;
    }
}
