
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Edocs.WorkFlow.Archiver.InterFaces;
namespace Edocs.WorkFlow.Archiver.Models
{
  public  class WFUsersModel :IWFUsers
    {
      public  int ID
        { get; set; }
      public  string FName
        { get; set; }
       public string LName
        { get; set; }
    }
}