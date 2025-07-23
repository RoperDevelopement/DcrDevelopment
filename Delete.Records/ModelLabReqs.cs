using Edocs.Delete.Records.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Delete.Records
{
  public  class ModelLabReqs:ILabReqs
    {
       public int ID
        { get; set; }
        public string FileUrl
        { get; set; }

        public Guid ScanBatch
        { get; set; }
        public DateTime ScanDate
        { get; set; }
    }
}
