using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class CategoriesModel
    {
        public int TotalCompleted
        { get; set; }
        public int TotalOpened
        { get; set; }

        

        public string CategoryName
        { get; set; }
        public int CategoryId
        { get; set; }

        public string Bins
        { get; set; }

        public string CategoryColor
        { get; set; }
            
    }
}
