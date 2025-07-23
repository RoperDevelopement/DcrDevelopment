using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Models
{
    public class CategoryFreqModel
    {
        public string CategoryName
        { get; set; }
        public string CategoryColor
        { get; set; }
        public string CategoryColorHexValue
        { get; set; }
        public TimeSpan CategoryDurationOne
        { get; set; }
        public bool CategoryFlashOne
        { get; set; }

        public TimeSpan CategoryDurationTwo
        { get; set; }
        public bool CategoryFlashTwo
        { get; set; }

        public TimeSpan CategoryDurationThree
        { get; set; }
        public bool CategoryFlashThree
        { get; set; }

        public TimeSpan CategoryDurationFour
        { get; set; }
        public bool CategoryFlashFour
        { get; set; }
    }
}
