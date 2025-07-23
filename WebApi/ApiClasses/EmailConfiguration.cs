using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
    public class EmailConfiguration:IEmailSettings
    {
        public string EmailServer
        { get; set; }
        public string EmailFrom
        { get; set; }
        public string EmailPassWord
        { get; set; }
        public int EmailPort
        { get; set; }
        public string EmailTo
        { get; set; }
        public string EmailCC
        { get; set; }
        public string TextTo
        { get; set; }
        public string TextCC
        { get; set; }
    }
}
