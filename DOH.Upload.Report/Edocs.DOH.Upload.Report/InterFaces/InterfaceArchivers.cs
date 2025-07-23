 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
  
namespace Edocs.DOH.Upload.Report.Interfaces
{
    interface IDOHIDImages
    {
        int ID
        { get; set; }
        int ImagesScanned
        { get; set; }
      

    }
    interface IDOHRecords
    {
      
          string FName
        { get; set; }
          string City
        { get; set; }
          string Church
        { get; set; }
          string BookType
        { get; set; }

          string SDate
        { get; set; }
          string EDate
        { get; set; }
          string DownLoadSubFolder
        { get; set; }
          string Uri
        { get; set; }
     
      
    }
   
    
    
    
    
    
    
}
