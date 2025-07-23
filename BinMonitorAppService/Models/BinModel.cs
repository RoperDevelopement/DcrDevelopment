using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BinMonitorAppService.Models
{
    public class BinModel
    {
        [Display(Name = "Bin ID")]
        public string BinID

        { get; set; }
    }
}
