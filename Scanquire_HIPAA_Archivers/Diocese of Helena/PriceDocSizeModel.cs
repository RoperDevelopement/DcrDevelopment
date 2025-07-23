using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.Diocese.Of.Helena.Archiver
{
    public class PriceDocSizeModel
    {
        public int EdocsCustomerID
        { get; set; }
        public float PricePerDocument
        { get; set; }
        public string DocumentSize
        { get; set; }

    }
}
