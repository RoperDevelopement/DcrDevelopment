using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.DOH.DL.Doc
{
    public class DOHDownLoadModel
    {
        public int ID
        { get; set; }
        public string FName
        { get; set; }
        public string City
        { get; set; }
        public string Church
        { get; set; }
        public string BookType
        { get; set; }

        public string SDate
        { get; set; }
        public string EDate
        { get; set; }
        public string DownLoadSubFolder
        { get; set; }
        public string Uri
        { get; set; }
        public int ImagesScanned
            {get;set;}
    }
}
