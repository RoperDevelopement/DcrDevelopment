using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.ITS.AppService.Models
{
 public  class PSUSDUploadRecordsModel:PSUSDRecordsModel
    {


     public string TrackindID
        { get; set; }
     public int EdocsCustomerID
        { get; set; }
         
       
     public DateTime RecordSDate
        { get; set; }


        public DateTime RecordEndDate
        { get; set; }

    }
}
