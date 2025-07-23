using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Models
{
    public class CategoryColorModel
    {
       public string CategoryName
        { get; set; }

        public string CategorId
        { get; set; }
        public string CategoryColorHexValue
        { get; set; }

        public bool Selected
        { get; set; }
    }
}
