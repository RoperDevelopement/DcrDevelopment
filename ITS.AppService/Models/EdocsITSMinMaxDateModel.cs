using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Interfaces;

namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSMinMaxDateModel:IMinMaxDate
    {
       public DateTime MinDate
        { get; set; }
       public DateTime MaxDate
        { get; set; }
    }
}
