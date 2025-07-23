using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Demos.Restful.Api.Interfaces
{
    interface IJsonBSBPropDepRecords  
    {
        string FileName
        { get; set; }
        string FileUrl
        { get; set; }
        int PermitNumber
        { get; set; }
        int ParcelNumber
        { get; set; }
        int ZoneNumber
        { get; set; }
        string GoCode
        { get; set; }
        int ExePermitNumber
        { get; set; }
        string Address
        { get; set; }
        string OwnerLot
        { get; set; }
        string ConstCompany
        { get; set; }
        DateTime DateIssued
        { get; set; }
        DateTime DateExpired
        { get; set; }
       
        Guid BatchID
        { get; set; }
        DateTime ScanDate
        { get; set; }
        string ScanOperator
        { get; set; }
    }
    interface IUploadSearchTxt
    {
        int PermitNumber
        { get; set; }
        string SearchStr
        { get; set; }
    }
    interface IJsonBSBPWDRecords
    {
        string FileName
        { get; set; }
        string FileUrl
        { get; set; }
        string ProjectDepartment
        { get; set; }
        int ProjectYear
        { get; set; }
        string ProjectName
        { get; set; }


    }
}
