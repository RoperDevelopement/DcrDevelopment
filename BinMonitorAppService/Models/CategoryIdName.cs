using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class CategoryIdName:IBinCategory
    {
        public int CategoryID
        { get; set; }
       public string CategoryName
        { get; set; }
    }
}
