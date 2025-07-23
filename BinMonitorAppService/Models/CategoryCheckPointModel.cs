using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
using System.ComponentModel.DataAnnotations;
namespace BinMonitorAppService.Models
{
   public class CategoryCheckPointModel: ICategoryCheckPoints, IBinCategory
    {
       public int CategoryID
        { get; set; }
        public string CategoryName
        { get; set; }

        [Display(Name = "Check Point 1 Duration")]
        public string CategoryCheckPointOneDuration
        { get; set; }
        [Display(Name = "Category Color")]
        public string CategoryColorCheckPointOne
        { get; set; }
        [Display(Name = "Check Point 2 Duration")]
        public string CategoryCheckPointTwoDuration
        { get; set; }
        [Display(Name = "Category Color")]
        public string CategoryColorCheckPointTwo
        { get; set; }
        [Display(Name = "Check Point 3 Duration")]
        public string CategoryCheckPointThreeDuration
        { get; set; }
        [Display(Name = "Category Color")]
        public string CategoryColorCheckPointThree
        { get; set; }
        [Display(Name = "Check Point 4 Duration")]
        
        public string CategoryCheckPointFourDuration
        { get; set; }
        [Display(Name = "Category Color")]
        public string CategoryColorCheckPointFour
        { get; set; }

        public int TotalOpened
        { get; set; }

        public int TotalCompleted
        { get; set; }
        public string CategoryColor
        { get; set;        }
        public string Bins
        { get; set; }
    }
}
