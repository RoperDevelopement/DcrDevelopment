using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Models
{
    public class EmotionsCountModel : IPickerChart
    {
        public string ValueLabel
        {
            get; set;
        }
        public string Label { get; set; }
        public string EmotionsColor { get; set; }
    }
}
