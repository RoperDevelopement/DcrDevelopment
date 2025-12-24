using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class AlanoClubIndexesModel : IAlanoClubIndexes
    {
    public   string Schema { get; set; }
       public string Table { get; set; }
       public string Index { get; set; }

       public int AvgFragmentationInPercent { get; set; }
       public int IndexPageCount { get; set; }

    }
}
