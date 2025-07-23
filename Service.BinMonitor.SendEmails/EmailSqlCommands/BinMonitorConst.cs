using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Service.BinMonitor.SendEmails.EmailSqlCommands
{
    public class BinMonitorConst
    {
        internal const string AppConfigKeyBinMonitorXmlFile = "BinMonitorXmlFile";
        internal const double HoursToSeconds = 3600.00;
        internal const double ThreadSleepNight = 10.00;



        public const string RepStrApplicationDir = "{ApplicationDir}";
        public const string AppConfigKeyAuditLogFolder = "AuditLogFolder";
        internal const string AppConFigKeyEmailXmlFile = "EmailXmlFile";
        internal const string AppConFigKeyServiceSleep = "ServiceSleep";
        internal const string AppConFigKeyEmailToServiceStatus = "EmailToServiceStatus";
        internal const string AppConFigKeyEmailServiceStatusHr = "EmailServiceStatusHr";
        internal const double SecondsToMillinSeconds = 1000.00;
        internal const string AppConFigKeyNumberOfDaysToKeepLogFIles = "NumberOfDaysToKeepLogFIles";
        #region sql info xml file
        internal const string XmlRootNode = "BMEmailServiceSettings";
        internal const string SqlXmlNodeSqlServerInfo = "SqlServerInfo";
        internal const string SqlXmlElementSqlServerName = "SqlServerName";
        internal const string SqlXmlElementDbName = "DbName";
        internal const string SqlXmlElementUserName = "UserName";
        internal const string SqlXmlElementPassWord = "PassWord";

        internal const string EmailXmlNodeSqlServerInfo = "Email";
        internal const string EmailXmlElementEmailServer = "EmailServer";
        internal const string EmailXmlElementTextTo = "TextTo";
        internal const string EmailXmlElementTextCC = "TextCC";

        internal const string EmailXmlElementEmailFrom = "EmailFrom";
        internal const string EmailXmlElementEmailPassword = "EmailPassword";
        internal const string EmailXmlElementEmailPort = "EmailPort";
        internal const string EmailXmlElementEmailSubject = "EmailSubject";
        internal const string EmailXmlElementEmailCC = "EmailCC";
        internal const string EmailXmlElementEmailTo = "EmailTo";
        internal const string BMEmailInfoXmlElementDefaultThreadSleepHr = "DefaultThreadSleepHr";
        internal const string BMEmailInfoXmlNodeBMEmailInfo = "BMEmailInfo";
        internal const string BMEmailInfoXmlNodeEmailStartTime = "EmailStartTime";
        internal const string BMEmailInfoXmlNodeEmailEndTime = "EmailEndTime";


        #endregion

        #region stored procedures
        internal const string SpEmailReportsUsers = "sp_EmailReportsUsers";
        internal const string SpEmailReports = "[dbo].[sp_EmailReports]";
        
        const string SpSendSpecBatchsEmails = "sp_SendSpecBatchsEmails";
        internal const string SpGetActiveBins = "sp_GetActiveBins";
        internal const string SpUpdateEmailSentCategorieIds = "dbo].[sp_UpdateEmailSentCategorieIds]";
        internal const string SpEmailCwidCategorieIds = "[dbo].[sp_EmailCwidCategorieIds]";
        internal const string SpEmailCwidCategorieIdsJson = "[dbo].[sp_EmailCwidCategorieIdsJson]";
        internal const string SpGetActiveBinsXmlByCategoryId = "[dbo].[sp_GetActiveBinsXmlByCategoryId]";
        internal const string SpUpdateCategoryLastEmailSent = "[dbo].[sp_UpdateCategoryLastEmailSent]";
        internal const string SpGetXmlAllBinsCategoriesReport = "[dbo].[sp_GetXmlAllBinsCategoriesReport]";
        
        #endregion
        #region stored Procedure params
        internal const string SqlParmaCategoryID = "@CategoryID";
        internal const string SqlParmaEmailTo = "@EmailTo";
        internal const string SqlParmaEmailCC = "@EmailCC";
        internal const string SqlParmaEmailFrequency = "@EmailFrequency";
        internal const string SqlParmaReportStartDate = "@ReportStartDate";
        internal const string SqlParmaOpenBins = "@OpenBins";
        
        internal const string SqlParmaReportEndDate = "@ReportEndDate";
        internal const string SqlParmaValueUpdateDate = "UpdateDate";
        internal const string SqlParmaValueOpen = "Open";
        internal const string SqlParmaValueDaily = "Daily";
         

        #endregion
        internal const string AppConfigKeySqlServerNameNyp = "SqlServerNameNyp";
        internal const string AppConfigKeySqlServerNameQueens = "SqlServerNameQueens";
        internal const string AppConfigKeyDataBaseNameNyp = "DataBaseNameNyp";
        internal const string AppConfigKeyDataBaseName = "DataBaseName";
        internal const string AppConfigKeyEmailHtmlFile = "EmailHtmlFile";
        internal const string AppConfigKeyXsltStyleSHeet = "XsltStyleSHeet";
        
        internal const string AppConfigKeyEmailTansFormXml = "EmailTansFormXml";
        internal const string RepStrApplicationDataFolder = "{ApplicationDataFolder}";
        internal const string RepStrEmailSubject = "{repStr}";
        internal const string ReplaceStrDateTime = "{dateTime}";

        internal const string SpEmailAddress = "sp_EmailAddress";
        internal const string spEmailReports = "sp_EmailReports";
        internal const string DllName = "SendEmails.dll.config";

        internal const string SpGetEmailFreq = "sp_GetEmailFreq";
    }
}
 
