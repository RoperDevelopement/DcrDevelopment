using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Constants
{
    //public enum EnumCategorieNames
    //{
    //    PROBLEM,
    //    READY,
    //    CHECKIN,
    //    STAT,
    //    ORDERED,
    //    QA,
    //    TIMED
    //}
    public class SqlConstants
    {
        #region Sql StoredProcedure Names

        public const string SpGetSpecUserRights = "sp_GetSpecUserRights";
        public const string SpGetSPCategoryPermissions = "sp_GetSPCategoryPermissions";
        public const string SpGetSpUserRightsModel = "sp_GetSpUserRightsModel";
        public const string SpAddUpdateUserProfiles = "[dbo].[sp_AddUpdateUserProfiles]";
        public const string SpGetDeletedLabReqs = "sp_GetDeletedLabReqs";
        public const string SpGetReportVolumeTotalDurByCWID = "[dbo].[sp_GetReportVolumeTotalDurByCWID]";
        public const string SpGetSpectrumMonitorUserCwid = "sp_GetSpectrumMonitorUserCwid";
        public const string SpCategoryCheckPointModel = "sp_CategoryCheckPointModel";
        public const string SPGetSpecSettings = "sp_GetBinMonitorSettings";
        public const string SpGetBinsStatusStartDateEndDateModel = "sp_GetBinsStatusStartDateEndDateModel";

        public const string SpGetActiveBinsModelByBatchID = "sp_GetActiveBinsModelByBatchID";
        public const string SpGetActiveBinsModelByBinId = "sp_GetActiveBinsModelByBinId";
        public const string SpGetUnRegBinsModel = "sp_GetUnRegBinsModel";
        public const string SpCompleteRegBinsModel = "sp_CompleteRegBinsModel";
        public const string SpBeginProcessModel = "sp_BeginProcessModel";
        public const string SpCompleteProcessModel = "sp_CompleteProcessModel";
        public const string SpNotClosedModel = "sp_NotClosedModel";
        public const string SpGetCategoryColors = "sp_GetCategoryColors";
        public const string SpColorCodesModel = "sp_ColorCodesModel";
        public const string SpUpDateCategoryColors = "sp_UpDateCategoryColors";
        public const string SpGetTotalOpenByCategory = "sp_GetTotalOpenByCategory";
        public const string SpGetTotalClosedByCategory = "sp_GetTotalClosedByCategory";
        public const string SpCompleteRegStartProcessingBinsModel = "sp_CompleteRegStartProcessingBinsModel";
        public const string SpCategoryDurationsModel = "sp_CategoryDurationsModel";
        public const string SpUpDateCategoryDurationsModel = "sp_UpDateCategoryDurationsModel";
        public const string SpTransFerWFSendEMail = "sp_TransFerWFSendEMail";
        public const string SpUsageReportByCWID = "sp_UsageReportByCWID";
        public const string SpGetActiveBinsModel = "sp_GetActiveBinsModel";
        public const string SpGetActiveBinsModelByBInId = "sp_GetActiveBinsIDModel";
        public const string SpGetActiveBinsModelTop1 = "sp_GetActiveBinsModelTop1";
        public const string SpGetLabReq = "sp_GetLabReq";

        public const string SpGetUsersXmlFileByCwid = "sp_GetUsersXmlFileByCwid";
        public const string SpBins = "sp_Bins";
        public const string SpGetCategoryDurations = "sp_GetCategoryDurations";
        public const string SpGetBMTotalCategories = "sp_GetBMTotalCategories";
        public const string SpBinsOpened = "sp_OpenBins";
        public const string SpAllBinsModel = "sp_AllBinsModel";
        public const string SpModelBinCreatedBY = "sp_ModelBinCreatedBY";


        public const string SpCategoryColors = "sp_CategoryColors";
        public const string SpCategoryDurations = "sp_CategoryDurations";
        public const string SpUpDateXmlFileBinCategorie = "sp_UpDateXmlFileBinCategorie";
        public const string SpXmlBinUserProfiles = "sp_XmlBinUserProfiles";
        public const string SpGetEdocsUpDateInfo = "sp_GetEdocsUpDateInfo";
        public const string SpBinMonitorChanges = "sp_BinMonitorChanges";
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
        public const string SpXmlActiveBins = "sp_XmlActiveBins";
        public const string SpAddUpDateBinUsers = "sp_AddUpDateBinUsers";
        public const string SpDeleteBinUsers = "sp_DeleteBinUsers";
        public const string SpEmailReportsUsers = "sp_EmailReportsUsers";
        public const string SpSendSpecBatchsEmails = "sp_SendSpecBatchsEmails";
        public const string SpActiveBins = "sp_ActiveBins";
        public const string SpGetRegisteredCategories = "sp_GetRegisteredCategories";
        public const string SpComputersRunningBM = "sp_ComputersRunningEdocsApps";
        public const string SpUpdateSpectrumMonitorSettings = "sp_UpdateSpectrumMonitorSettings";
        public const string SpGetBinsReportsByCwid = "sp_GetBinsReportsByCwid";
        public const string SpBinsProcessingStarted = "sp_BinsProcessingStarted";
        

        #endregion
        #region blobstorageconst
        public const string JsonKeyAzureStorageBlobSettings = "AzureStorageBlobSettings";
        public const string JsonKeyAzureStorageBlobAccountKey = "AzureBlobAccountKey";
        public const string JsonKeyAzureStorageBlobAccountName = "AzureBlobAccountName";


        public const string JsonKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        public const string JsonKeyAzureBlobShareName = "AzureBlobShareLabRecs";
        public const string JsonKeyAzureBlobShareAuditLogsUpLoad = "AzureBlobShareAuditLogsUpLoad";
        public const string AppSettingsJsonFileName = "AppSettings.json";
        public const string JsonKeyAzureBlobShareAuditLogs = "AzureBlobShareAuditLogs";
        public const string JsonKeyAzureBlobShareManual = "AzureBlobShareManual";
        
        #endregion

        #region Sql Stored Procedure Params

        public const string SpParmaOldBatchId = "@OldBatchId";
        public const string SpParmaBinStatus = "@BinStatus";
        public const string SpParmaBinStatusStartDate = "@BinStatusStartDate";
        public const string SpParmaBinStatusEndDate = "@BinStatusEndDate";
        public const string SpParmaBatchId = "@BatchId";
        public const string SpParmaBinId = "@BinId";
        public const string SpParmaCategorieId = "@Categorie";
        public const string SpParmaCategorieColor = "@CategorieColor";
        public const string SpParmaCategorieColorHex = "@CategorieColorHex";

        public const string SpParmaCategorieDuration = "@CategorieDuration";
        public const string SpParmaFlash = "@Flash";
        public const string SpParmaCategorieDurOne = "@CategorieDurOne";
        public const string SpParmaCategorieFlashOne = "@CategorieFlashOne";
        public const string SpParmaCategorieDurTwo = "@CategorieDurTwo";
        public const string SpParmaCategorieFlashTwo = "@CategorieFlashTwo";
        public const string SpParmaCategorieDurThree = "@CategorieDurThree";
        public const string SpParmaCategorieFlashThree = "@CategorieFlashThree";
        public const string SpParmaCategorieDurFour = "@CategorieDurFour";
        public const string SpParmaCategorieFlashFour = "@CategorieFlashFour";
        public const string SpParmaEmailAlerts = "@EmailAlerts";


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
        public const string SpParmaEmailSubject = "@EmailSubject";
        public const string SpParmaEmailCC = "@EmailCC";
        public const string SpParmaEmailFrequency = "@EmailFrequency";
        public const string SpParmaSendEmailStart = "@SendEmailStart";
        public const string SpParmaSendEmailComplete = "@SendEmailComplete";
        public const string SpParmaSendEmailContents = "@SendEmailContents";
        public const string SpParmaUpdateEmailTable = "@UpdateEmailTable";
        public const string SpParmaRegProcess = "@RegProcess";
        public const string SpAppName = "@AppName";
        public const string ApiCategoryDurations = "CategoryDurations/{0}/{1}";
        public const string SpParmaProfileName = "@ProfileName";
        public const string SpParmaRunReports = "@RunReports";
        public const string SpParmaTransFerBins = "@TransFerBins";
        public const string SpParmaTransFerCategories = "@TransFerCategories";
        public const string SpParmaCreateUsers = "@CreateUsers";
        public const string SpParmaEditUsers = "@EditUsers";

        public const string SpParmaChangeUsersPasswords = "@ChangeUsersPasswords";
        public const string SpParmaDeleteUsers = "@DeleteUsers";
        public const string SpParmaManageUserProfiles = "@ManageUserProfiles";
        public const string SpParmaCreateNewProfiles = "@CreateNewProfiles";


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
        public const string SqlAliasCatName = "catName";
        public const string SqlAliasCatId = "catId";
        public const string SqlAliasAssignedTo = "assignedTo";
        public const string SqlAliasStarted = "started";
        public const string SqlAliasRegCompletedAt = "regCompletedAt";
        public const string SqlAliasProcessAssignedBy = "processAssignedBy";
        public const string SqlAliasProcessAssignedTo = "processAssignedTo";
        public const string SqlAliasProcessCompletedBy = "processCompletedBy";
        public const string SqlAliasProcessCompletedAt = "processCompletedAt";
        public const string SqlAliasBinClosedBy = "binClosedBy";
        public const string SqlAliasBinCompletedAt = "binCompletedAt";
        public const string SqlAliasBinComments = "binComments";
        public const string SqlAliasBinContents = "binContents";
        public const string SqlAliasTotalCat = "totalCat";
        public const string BinMonitorCloudConnectionString = "BinMonitorCloudConnectionString";
        public const string UserUnassigned = "UNASSIGNED";
        public const string ProcessingBatchTrue = "True";
        public const string ProcessingBatchFalse = "False";
        public const string NoValue = "Not Assigned";
        public const string RepStrAt = "@";
        public const string EmptyBin = "000";
        public const string EdocsUserFN = "edocs";
        public const string EmailSpecimsCreate = "EmailSpecimsCreate";
        public const string EmailSpecimsProcess = "EmailSpecimsProcess";
        public const string DefaultFreq = "00:00:00";
        public const string CatNotReg = "NotReg";
        public const string CatReg = "Reg";

        public const string ApiBinsModel = "BinMonitor?spName=sp_BinsModel&modelName=BinsModel";
        public const string ApiUrlBins = "WebApiBinMonitor/sp_BinsModel";
        public const string ApiUrlActiveBins = "WebApiBinMonitor/sp_ActiveBinsModel";
        public const string ApiCategoriesModel = "BinMonitor?spName=sp_GetCatgories&modelName=CategoriesModel";
        public const string ApiCategories = "BinMonitor?spName=sp_GetCatgoriesModel";
        public const string ApiOpenBins = "WebApiBinMonitor/sp_OpenBins";
        public const string ApiUserInfo = "WebApiBinMonitor/sp_GetUserInfo";
        public const string ApiCatNameID = "WebApiBinMonitor/sp_GetCatgoriesModelIdName";
        public const string ApiActiveBinsModel = "WebApiBinMonitor/sp_GetActiveBinsModel";
        public const string ApiGetBinByBinId = "WebApiBinMonitor/sp_GetActiveBinsModelByBinId/";
        public const string ApiGetBinByBatchId = "WebApiBinMonitor/sp_GetActiveBinsModelByBatchID/";
        public const string CLoudConfigSqlConnectionString = "BinMonitorCloudConnectionString";
        public const string ApiGetAllBinIds = "WebApiBinMonitor/sp_AllBinsModel";
        public const string ApiUpdateEmailReports = "EmailReports";
        public const string ApiUpDateCategoryColors = "UpDateCategoryColors";
        public const string ApiUpDateCategoryCheckPoints = "CategoryCheckPoints/";
        public const string ApiWorkFlowEmailController = "WorkFlowEmail/";
        public const string ApiSpecMonitorReportsByCwidController = "SpecMonitorReportsByCwid/";
        public const string ApiSpecMonitorLabReqsController = "SpecMonitorLabReqs/";
        //public const string ApiCategoryDurations = "CategoryDurations?categoryName={0}";

        public const string ApiUpDateCategoryDurations = "CategoryDurations";
        //  public const string ApiCategoryCheckPoints = "CategoryCheckPoints?categoryName={0}";
        // public const string ApiCategoryCheckPoints = "CategoryCheckPoints?{0}";
        public const string ApiUpdateBinsByBInID = "UpdateBinsByBInID/";

        public const string RepStrWF = "{Working}";
        public const string ApiCreateBatch = "CreateBatches";
        public const string RegAllBatch = "registerAll";
        public const string RegBatch = "regbatch";
        public const string WebApiBinMonitor = "WebApiBinMonitor/";
        public const string UpdateRegBatches = "updateregisterbatch";
        public const string RegProcessBatches = "regProcessBatches";
        public const string ClosegRegBatch = "closeRegBatch";
        public const string ApiGetBinStatus = "GetBinStatus?spName=sp_GetBinsStatusStartDateEndDateModel&categoryName={0}&stDate={1}&endDate={2}";
        public const string ApiUpdateEmailInforTF = " WorkFlowEmail?oldBatchId={0}&newBatchId={1}";
        public const string ApiGetUseNamePw = "GetUseNamePw?cwid={0}";
        public const string ApiSpecMonitorUserInfo = "SpecMonitorUser/";
        public const string RepDoubleQuots = @"""";
        public const string RepDoubleSingleQuots = @"\";
        public const string DoubleQuotes = "\"";
        public const string ApiSpecMonitorUserController = "SpecMonitorUser/";
        public const string ApiUpDateAddUersPermissionsController = "UpDateAddUersPermissions/";
        public const string ApiUserInformation = "{0}/{1}";
        public const string ApiUserCwid = "{0}";
        protected string cloudDbServerName;
        protected string cloudDbName;
        protected string cloudDbUserName;
        public const string ApiTransFer = "TransFer";
        protected string cloudDbpassword;
       // public const string ApiSpecMonitorUserUpdateInfo = "SpecMonitorUser/userPassWord={0}&pwModify={1}";
       public const string ApiSpecMonitorUserUpdateInfo = "SpecMonitorUser/{0}&{1}";
        public const string ApiSpecMonitorUpdateSettings = "SpecMonitorUser/{0}";
        public const string RepStrCwid = "{CWID}";
        public const string RepStrEmailAddress = "{EmailAdddressNewUser}";
        public const string NypDefaultBMPW = "Nyp123456@";
        public const string BinRegProcessMondelIndexLabReq = "IndexLabReq";
        public const string BinRegProcessMondelIndexBatchId = "IndexBatchID";
        public const string BinRegProcessMondelIndexBinID = "IndexBinID";
        public const string CachBinRegProcessModel = "CachBinRegProcessModel";
        public const string CachMenuRights = "CachMenuRights";
        public const string ViewDataEmailReports = "ViewDataEmailReports";
        public const string ViewDataChangeCategories = "ViewDataChangeCategories";
        public const string ViewDataRunReports = "ViewDataRunReports";
        public const string CacheSecurityTokenValidTo = "CacheSecurityTokenValidTo";
        public const string CacheSecurityTokenValidFrom = "CacheSecurityTokenValidFrom";
        public const string CookieReturnUrl = "CookieReturnUrl";
        public const string CachMaxPerBin = "CookieReturnUrl";
        public const string CachAddTime = "CachAddTime";
    }
}
