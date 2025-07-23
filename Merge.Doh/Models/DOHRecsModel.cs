using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdocsUSA.Merge.Doh.Interfaces;
namespace EdocsUSA.Merge.Doh.Models
{
    class DOHRecsModel : IDOHRecs
    {
        public DateTime DateOfService { get; set; }

        
        public string AccessionNumber { get; set; }

        public string MRN { get; set; }
        public int DOHID
        { get; set; }
    }
}
