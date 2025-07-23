using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
   public class UploadBatchRecordsModel : JsonFileModel
    {
        //public string FileName { get; set; }

        private Dictionary<string, string> _Fields = new Dictionary<string, string>();
        public Dictionary<string, string> Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public UploadBatchRecordsModel() { }

        public UploadBatchRecordsModel(string fileName) : this()
        { this.FileName = fileName; }

        public UploadBatchRecordsModel(string fileName, Dictionary<string, string> fields) : this(fileName)
        { this.Fields = fields; }
    }
}
