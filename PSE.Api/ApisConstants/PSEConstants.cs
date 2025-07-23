using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.PSE.Api.ApisConstants
{
    public class PSEConstants
    {
        #region  sql parms
        public const string SpParmaJasonFile = "@JasonFile";
        public const string SpParmaStudentFirstName = "@StudentFirstName";
        public const string SpParmaStudentLastName  = "@StudentLastName";
        public const string SpParmaStudentDateOfBirth = "@StudentDateOfBirth";
        public const string SpParmaStartFinYear = "@StartFinYear";
        public const string SpParmaEndFinYear = "@EndFinYear";
        public const string SpParmaFinCategory = "@FinCategory";
        public const string SpParmaScanStDate = "@ScanStDate";
        public const string SpParmaScanEndDate = "@ScanEndDate";
        public const string NA = "NA";
        public const string SpParmaLoginName = "@LoginName";
        public const string SpParmaUserPassWord = "@UserPassWord";
        public const string SpParmaNumDaysNextMFLA = "@NumDaysNextMFLA";
        public const string SpParmaTitle = "@Title";
        public const string SpParmaArchiveDate = "@ArchiveDate";
        public const string SpParmaCollection = "@Collection";
        public const string SpParmaDescription = "@Description";
        public const string SpParmaFullTextSearch = "@FullTextSearch";
        public const string SpParmaArchiveID = "@ArchiveID";










        #endregion
        public const string SqlSpUploadPSEFinancialRecordsJson = "sp_UploadPSEFinancialRecordsJson";
        public const string SqlSpUploadPSEStudentRecordJson = "sp_UploadPSEStudentRecordJson";
        public const string PSEConnectionStr = "PSECloudConnectionString";
        public const string SpGetStudentRecordFirstLastName = "sp_GetStudentRecordFirstLastName";
        public const string SpGetStudentRecordDOB = "sp_GetStudentRecordDOB";
        public const string SpGetStudentRecordScanDate = "sp_GetStudentRecordScanDate";
        public const string SpGetStudentRecordDOBDateRange = "sp_GetStudentRecordDOBDateRange";
        public const string SpGetBSBArchivedRecords = "sp_GetBSBArchivedRecords";
        public const string SpGetBSBArchivedRecordByID = "[dbo].[sp_GetBSBArchivedRecordByID]";
        


    }
}

