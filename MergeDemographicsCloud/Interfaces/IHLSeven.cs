using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeDemographicsCloud.Interfaces
{
    public interface IHlSeven
    {
        DateTime DateOfService { get; set; }
        string PatientID { get; set; }
        string ClientCode { get; set; }
        string FinancialNumber { get; set; }
        string PatientLastName { get; set; }
        string PatientFirstName { get; set; }
        string DrCode { get; set; }
        string DrFName { get; set; }
        string DrLName { get; set; }
        string RequisitionNumber { get; set; }
        int HL7RowID
        { get; set; }

    }

    public interface ILabRecs
    {
        DateTime DateOfService { get; set; }
        string PatientID { get; set; }
        string ClientCode { get; set; }
        string RequisitionNumber { get; set; }
        string IndexNumber { get; set; }
        string FileName { get; set; }
        string IndexnumberNoZeros { get; set; }
        string FinancialNumber { get; set; }
        string MRN { get; set; }
        int LabReqID
        { get; set; }

    }
    public interface ILabRecsFullText
    {
        DateTime DateOfService { get; set; }
        string ImageFullText { get; set; }
        DateTime DateAdded { get; set; }
        DateTime DateModified { get; set; }
        DateTime ScanDate { get; set; }
        int LabReqID
        { get; set; }

    }


}
