using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Models
{
    public class StateInfoModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Capital { get; set; }
        public string Region { get; set; }
        public int Population
        {
            get; set;
        }
    }
}
