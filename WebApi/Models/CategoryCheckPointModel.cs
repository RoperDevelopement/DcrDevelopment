using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Models
{
   public class CategoryCheckPointModel: ICategoryCheckPoints, IBinCategory
    {
       public int CategoryID
        { get; set; }
        public string CategoryName
        { get; set; }
        public string CategoryCheckPointOneDuration
        { get; set; }
        public string CategoryColorCheckPointOne
        { get; set; }
        public string CategoryCheckPointTwoDuration
        { get; set; }
        public string CategoryColorCheckPointTwo
        { get; set; }
        public string CategoryCheckPointThreeDuration
        { get; set; }
        public string CategoryColorCheckPointThree
        { get; set; }
        public string CategoryCheckPointFourDuration
        { get; set; }

        public string CategoryColorCheckPointFour
        { get; set; }
    }
}
