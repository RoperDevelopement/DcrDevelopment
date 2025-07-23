using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Upload.Azure.Blob.Storage
{
   public class BlockBlobModel
    {
        public string Name
        { get; set; }
        public Uri StorageUri
        { get; set; }
      public  DateTimeOffset? Create
        { get; set; }
    }
}
