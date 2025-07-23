using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.ITS.AppService.Models
{
    public class PSUSDRecordsModel
    {
        public string Department
        { get; set; }
        public string OrginationDepartment
        { get; set; }
        public string BoxID
        { get; set; }
        public string DateOfRecords
        { get; set; }

        public string DescriptionOfRecords
        { get; set; }
        public string MethOfFiling
        { get; set; }
        public string PDFFileName
        { get; set; }
        public string FirsName
        { get; set; }


        public string LastName
        { get; set; }
        public string DateOfBirth
        { get; set; }
    }
}
