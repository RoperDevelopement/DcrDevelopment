using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.Dillion.VCC.Archiver
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
