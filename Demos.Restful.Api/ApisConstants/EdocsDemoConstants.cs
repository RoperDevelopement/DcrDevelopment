using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Edocs.Demos.Restful.Api.ApisConstants
{
    public class EdocsDemoConstants
    {
        public const string SpParmaJasonFile = "@JasonFile";
        public const string EdocsDemoCloudConnectionString = "EdocsDemoCloudConnectionString";
        public const string SpUploadBSBProdDepJson = "sp_UploadBSBProdDepJson";
        public const string SpBSBGetByPermit = "sp_BSBGetByPermit";
        public const string SUpateBSBProdDepFullTextSearch = "[dbo].[spUpateBSBProdDepFullTextSearch]";
        public const string SpGetBSBPWDRecords = "sp_GetBSBPWDRecords";
        public const string SpGetBSBPWDProjectNames = "sp_GetBSBPWDProjectNames";


        public const string SpBSBPWDUpload = "sp_BSBPWDUpload";
        public const string SpParmPermitNum = "@PermitNum";
        public const string SpParmAddress = "@Address";
        public const string SpParmAddressSW = "@AddressSW";
        public const string SpParmSearchText = "@SearchText";

        public const string SpParmProjectDepartment ="@ProjectDepartment";
        public const string SpParmProjectYear = "@ProjectYear";
        public const string SpParmProjectName = "@ProjectName";
        public const string SpParmFileName = "@FileName";
        public const string SpParmFileUrl = "@FileUrl";
        public const string SpParmScanOperator = "@ScanOperator";
       

	
        public const string SpParmOwnerLot = "@OwnerLot";
        public const string SpParmConstCompany = "@ConstCompany";
        public const string SpParmParcelNumber = "@ParcelNumber";
        public const string SpParmZoneNumber = "@ZoneNumber";
        public const string SpParmGoCode = "@GoCode";
        public const string SpParmExePermitNumber = "@ExePermitNumber";
        public const string DoubleQuotes = "\"";
    }
}
