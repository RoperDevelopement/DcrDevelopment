using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.DOH.Upload.Report.Interfaces;
namespace Edocs.DOH.Upload.Report.Models
{
   public class DOHIDImagesScanned:IDOHIDImages
    {
       public int ID
        { get; set; }
       public int ImagesScanned
        { get; set; }
        public string PDFFIleName
        { get; set; }
       public DateTime DateAdded
        { get; set; }
    }
}
