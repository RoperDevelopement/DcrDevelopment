using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlCommands
{
  public  class SqlConstants
    {
        #region Sql StoredProcedure Names

        public const string SpGetUsersXmlFileByCwid = "sp_GetUsersXmlFileByCwid";
        public const string SpUpDateXmlFileBinCategorie = "sp_UpDateXmlFileBinCategorie";
        public const string SpXmlBinUserProfiles = "sp_XmlBinUserProfiles";
        public const string SpGetEdocsUpDateInfo = "sp_GetEdocsUpDateInfo";
        
        public const string SpEmailAddress = "sp_EmailAddress";
        public const string spEmailReports = "sp_EmailReports"; 
        public const string SpGetActiveBins = "sp_GetActiveBins";
        public const string SpUpLoadLogFiles = "sp_UpLoadLogFiles";
        public const string SpGetUnactiveBatches = "sp_GetUnactiveBatches";
        public const string SpAddBinRegistration = "sp_BinRegistration";
        public const string SpUploadXmlFileUsers = "sp_UploadXmlFileUsers";
        public const string SpGetXmlSpecimenBatches = "sp_GetXmlSpecimenBatches";
        public const string SpGetUsersXmlFile = "sp_GetUsersXmlFile";
        public const string SpGetXmlBinUserProfiles = "sp_GetXmlBinUserProfiles";
        public const string SpGetXmlFileBinCategories = "sp_GetXmlFileBinCategories";
        public const string SpGetXmlFileBIns = "sp_GetXmlFileBIns";
        public const string SpGetXmlFileBinsMasterCategories = "sp_GetXmlFileBinsMasterCategories";
        public const string SpBinCommentsContents = "sp_BinCommentsContents";
        public const string SpBinProcessing = "sp_BinProcessing";
        public const string SpBinClosed = "sp_BinClosed";
        public const string SpGetUserInfo = "sp_GetUserInfo";
        public const string SpGetBatchXmlFile = "sp_GetBatchXmlFile";
        public const string SpGetCatgories = "sp_GetCatgories";
        public const string SpUploadBinXmlFile = "sp_UploadBinXmlFile";
        public const string SpUploadSpecimenBatchesXmlFile = "sp_UploadSpecimenBatchesXmlFile";
        public const string SpUpdateXmlFileBIns = "sp_UpdateXmlFileBIns";
        public const string SpActiveBins = "sp_XmlActiveBins";
        public const string SpAddUpDateBinUsers = "sp_AddUpDateBinUsers";
        public const string SpDeleteBinUsers = "sp_DeleteBinUsers";
        public const string SpEmailReportsUsers = "sp_EmailReportsUsers";
        public const string SpSendSpecBatchsEmails = "sp_SendSpecBatchsEmails";
        
        #endregion

        #region Sql Stored Procedure Params
        public const string SpParmaBatchId = "@BatchId";
        public const string SpParmaBinId = "@BinId";
        public const string SpParmaCategorieId = "@Categorie";
        public const string SpParmaStartedAt = "@StartedAt";
        public const string SpParmaAssignedBy = "@AssignedBy";
        public const string SpParmaAssignedTo = "@AssignedTo";
        public const string SpParmaCompletedBy = "@CompletedBy";
        public const string SpParmaComments = "@Comments";
        public const string SpParmaContents = "@Contents";
        public const string SpParmaXmlFile = "@XmlFile";
        public const string SpParmaProcessing = "@Processing";
        public const string SpParmaUserName = "@UserName";
        public const string SpParmaCreatedBy = "@CreatedBy";
        public const string SpParmaCreatedAt = "@CreatedAt";
        public const string SpParmaCompletedAt = "@CompletedAt";
        public const string SpParmaCwid = "@Cwid";
        public const string SpParmaUserFirstName = "@UserFirstName";
        public const string SpParmaUserLastName = "@UserLastName";
        public const string SpParmaUserPassword = "@UserPassword";
        public const string SpParmaUserEmailAddress = "@UserEmailAddress";
        public const string SpParmaUserProfile = "@UserProfile";
        public const string SpParmaDisplayName = "@DisplayName";
        public const string SpParmaComputerName = "@ComputerName";
        public const string SpParmaLogFileName = "@LogFileName";
        public const string SpParmaLogFile = "@LogFile";
        public const string SpParmaDelete = "@Delete";
        
        public const string SpParmaEmailTo = "@EmailTo";
        public const string SpParmaEmailCC = "@EmailCC";
        public const string SpParmaEmailFrequency  = "@EmailFrequency";
        public const string SpParmaSendEmailStart = "@SendEmailStart";
        public const string SpParmaSendEmailComplete = "@SendEmailComplete";
        public const string SpParmaSendEmailContents = "@SendEmailContents";
        public const string SpParmaUpdateEmailTable = "@UpdateEmailTable";
        

        #endregion

        public const string SqlAliasBinId = "binId";
        public const string SqlAliasBinXmlFile = "binXmlFile";
        public const string SqlAliasBatchId = "batchId";
        public const string SqlAliasBatchXmlFile = "batchXmlFile";
        public const string SqlAliasRegStartedAt = "regStartedAt";
        public const string SqlAliasRegCompltedBy = "regCompltedBy";
        public const string SqlAliasRegCreatedB = "regCreatedBy";
        public const string SqlAliasRegAssginedBy = "regAssginedBy";
        public const string SqlAliasRegAssignedTo = "regAssignedTo";

        public const string SqlAliasRegCompletedAt = "regCompletedAt";
        public const string SqlAliasProcessAssignedBy = "processAssignedBy";
        public const string SqlAliasProcessAssignedTo = "processAssignedTo";
        public const string SqlAliasProcessCompletedBy = "processCompletedBy";
        public const string SqlAliasProcessCompletedAt = "processCompletedAt";
        public const string SqlAliasBinClosedBy = "binClosedBy";
        public const string SqlAliasBinCompletedAt = "binCompletedAt";
        public const string SqlAliasBinComments = "binComments";
        public const string SqlAliasBinContents = "binContents";


        public const string ProcessingBatchTrue = "True";
        public const string ProcessingBatchFalse = "False";
        public const string NoValue = "Not Assigned";
        public const string RepStrAt = "@";
        public const string EmptyBin = "000";
        public const string EdocsUserFN = "edocs";
        public const string EmailSpecimsCreate = "EmailSpecimsCreate";
        public const string EmailSpecimsProcess = "EmailSpecimsProcess";


        protected string cloudDbServerName;
        protected string cloudDbName;
        protected string cloudDbUserName;
        protected string cloudDbpassword;

    }
}
