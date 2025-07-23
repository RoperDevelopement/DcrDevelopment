using MergeDemographicsCloud.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeDemographicsCloud.Models
{
   public class LabReqsFullTextModel: ILabRecsFullText
    {
       public DateTime DateOfService { get; set; }
       public string ImageFullText { get; set; }
       public DateTime DateAdded { get; set; }
       public DateTime DateModified { get; set; }
       public DateTime ScanDate { get; set; }
       public int LabReqID
        { get; set; }
    }
}
