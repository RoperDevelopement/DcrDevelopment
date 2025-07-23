using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class EditLabReqsReportModel: LabReqsModel, ILabReqsEditRep 
    {
       public string VersionNumber
        { get; set; }
       public DateTime DateAdded
        { get; set; }
    }
}
