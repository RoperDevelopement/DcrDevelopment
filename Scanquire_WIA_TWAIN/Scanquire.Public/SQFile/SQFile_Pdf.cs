using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scanquire.Public
{
    /// <summary>Specialized type of SQFile for PDF files.</summary>
    public class SQFile_Pdf : SQFile
    {
        public string OwnerPassword { get; set; }
        public string UserPassword { get; set; }
    }
}
