using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    class BSBPWDRecords: IJsonBSBPWDRecords
    {
       public string FileName
        { get; set; }
        public string FileUrl
        { get; set; }
        public string ProjectDepartment
        { get; set; }
     public   int ProjectYear
        { get; set; }
      public  string ProjectName
        { get; set; }
        public string ScanOperator
        { get; set; }
       
    }
}
