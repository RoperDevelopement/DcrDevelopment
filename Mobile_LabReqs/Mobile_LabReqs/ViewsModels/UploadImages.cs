using System;
using System.Collections.Generic;
using System.Text;
using Mobile_LabReqs.InterFaces;
namespace Mobile_LabReqs.ViewsModels
{
  public  class UploadImages :IArchiver
    {
       public string ArchiverName
        { get; set; }
      public  string ArchiverFileName
        { get; set; }

     public   byte[] ArchiverImage
        { get; set; }
    }
}
