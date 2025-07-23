using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
using Edocs.Service.BinMonitor.SendEmails.Models;
using System.Data.SqlClient;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using Edocs.Service.BinMonitor.SendEmails.SendEmail;

using Edocs.Service.BinMonitor.SendEmails.EmailSqlCommands;
using System.Net.Http.Headers;

namespace Edocs.Service.BinMonitor.SendEmails
{
    public class SendEmailsBinMonitor:IDisposable
    {
        private SqlCommands SqlCmd = null;
        private SqlConnection SqlConnection
        { get; set; }
        private string BinMonitorXmlFile
        {
            get { return $"{TL.TraceLoggerInstance.CheckPath(ServiceHelpers.Instance.GetApplicationDir())}{ServiceHelpers.Instance.GetApplicationSetting(BinMonitorConst.AppConfigKeyBinMonitorXmlFile)}"; }
        }
        private string AuditLogFolder
        { get { return $"{TL.TraceLoggerInstance.CheckPath(ServiceHelpers.Instance.GetApplicationDir())}{ServiceHelpers.Instance.GetApplicationSetting(BinMonitorConst.AppConfigKeyAuditLogFolder)}"; } }

        private string EmailTansFormXml
        {
            get { return ServiceHelpers.Instance.StrReplace(ServiceHelpers.Instance.GetApplicationSetting(BinMonitorConst.AppConfigKeyEmailTansFormXml), BinMonitorConst.RepStrApplicationDataFolder, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)); }
        }
        private string XsltStyleSHeet
        {
            get
            {
                return $"{ServiceHelpers.Instance.GetApplicationDir()}{ServiceHelpers.Instance.GetApplicationSetting(BinMonitorConst.AppConfigKeyXsltStyleSHeet)}";
            }
        }
        private StringBuilder Sb
        { get; set; }
        private string EmailHtmlFile
        {
            get { return ServiceHelpers.Instance.StrReplace(ServiceHelpers.Instance.GetApplicationSetting(BinMonitorConst.AppConfigKeyEmailHtmlFile), BinMonitorConst.RepStrApplicationDataFolder, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)); }
        }
        public int ThreadSleep
        { get; set; } = 0;

        public SendEmailsBinMonitor()
        {

            OpenAuditLog().Wait();
            SqlCmd = new SqlCommands();
            ThreadSleep = ServiceHelpers.Instance.DefaultThreadSleep(BinMonitorXmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
            Sb = new StringBuilder();
        }
        private async Task OpenAuditLog()
        {
            TL.TraceLoggerInstance.StartStopStopWatch();
            TL.TraceLoggerInstance.RunningAssembley = ServiceHelpers.Instance.GetApplicationName();
            string auditLogFileName = $"{TL.TraceLoggerInstance.RunningAssembley}_AuditLog_{DateTime.Now.ToString("MM_dd_yyyy")}.log";
            TL.TraceLoggerInstance.OpenTraceFile(AuditLogFolder, auditLogFileName, TL.TraceLoggerInstance.RunningAssembley, true);
            TL.TraceLoggerInstance.TraceInformation($"Checking to send emails : {DateTime.Now.ToString()}");
        }
        public async Task<int> GetThreadSleep()
        {
           return (int)ServiceHelpers.Instance.GetThreadSleepHrToMilliSec(BinMonitorXmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task CheckEmails()
        {
            try
            {
                
                TL.TraceLoggerInstance.TraceInformation($"In method CheckEmails getting sql server informaion using xml file {BinMonitorXmlFile}");
                if (ServiceHelpers.Instance.CheckRunService(BinMonitorXmlFile).ConfigureAwait(true).GetAwaiter().GetResult())
                {
                    SqlServerModel sqlServerModel = ServiceHelpers.Instance.GetSqlServerInfo(BinMonitorXmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                    SqlConnection = SqlCmd.SqlConnection(sqlServerModel).ConfigureAwait(true).GetAwaiter().GetResult();
                    IList<EmailCategoriesModel> emailCategories = SqlCmd.SqlDataReader<EmailCategoriesModel>(BinMonitorConst.SpEmailCwidCategorieIds, SqlConnection).ConfigureAwait(false).GetAwaiter().GetResult();
                    SendEmailsCategories(emailCategories).ConfigureAwait(false).GetAwaiter().GetResult();
                    IList<DailyEmailModel> dailyEmail = SqlCmd.SqlDataReader<DailyEmailModel>(BinMonitorConst.SpEmailReportsUsers, SqlConnection).ConfigureAwait(false).GetAwaiter().GetResult();
                    SendEmailsFreq(dailyEmail).ConfigureAwait(true).GetAwaiter().GetResult();

                    
                }
                else
                {
                   
                    TL.TraceLoggerInstance.TraceInformation($"Not time to run since datetime is {DateTime.Now.ToString()} thread sleeping for {ThreadSleep.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Sb.AppendLine($"Error getting email info message: {ex.Message}");
                TL.TraceLoggerInstance.TraceError($"{ex.Message}");
            }

        }

        public async Task SendEmailsCategories(IList<EmailCategoriesModel> emailCategories)
        {
            if (emailCategories == null)
            {
                TL.TraceLoggerInstance.TraceInformation("Method SendEmailsCategories not checking emails since no categoires found ");
                return;
            }
            foreach (var cat in emailCategories)
            {
                
                if (ServiceHelpers.Instance.SendEmail(cat.LastTimeEmailSent, cat.EmailDur).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    Dictionary<string, string> catID = new Dictionary<string, string>() {
                    { BinMonitorConst.SqlParmaCategoryID, cat.CategoryId.ToString() }
                };
                    using (SqlDataReader dr = SqlCmd.SqlDataReader(BinMonitorConst.SpGetActiveBinsXmlByCategoryId, SqlConnection, catID).ConfigureAwait(false).GetAwaiter().GetResult())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                            {
                                string xmldata = dr[0].ToString();
                                if (!(string.IsNullOrWhiteSpace(xmldata)))
                                {
                                    CreateReportBinMonitorCategories(xmldata, cat).ConfigureAwait(false).GetAwaiter().GetResult();
                                    UpdateEmailSentTime(catID, BinMonitorConst.SpUpdateCategoryLastEmailSent).ConfigureAwait(false).GetAwaiter().GetResult();
                                }
                            }
                        }
                        if (!(dr.IsClosed))
                            dr.Close();
                    }
                }
            }
        }
        public async Task SendEmailsFreq(IList<DailyEmailModel> dailyEmails)
        {
            if (dailyEmails == null)
            {
                TL.TraceLoggerInstance.TraceInformation("Method SendEmailsFreq not checking emails since no freq was found ");
                return;
            }
            Dictionary<string, string> dEmails = null;
            string reportType = "Daily Report";
            foreach (var de in dailyEmails)
            {


               
                if (ServiceHelpers.Instance.SendEmail(de.LastTimeEmailSent, de.EmailFrequency.ToString()).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    if (de.LastTimeEmailSent.Date != DateTime.Now.Date)
                    {
                        reportType = $"Daily Report Start Date {DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy")} End Date {DateTime.Now.ToString("MM-dd-yyyy")}";
                        dEmails = new Dictionary<string, string>()
                        {
                            {BinMonitorConst.SqlParmaReportStartDate,DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy")},
                            {BinMonitorConst.SqlParmaReportEndDate,DateTime.Now.ToString("MM-dd-yyyy")},
                            {BinMonitorConst.SqlParmaOpenBins,BinMonitorConst.SqlParmaValueDaily}
                        };
                    }
                    else
                    {
                        reportType = $" sent every {de.EmailFrequency} hrs";
                        dEmails = new Dictionary<string, string>()
                        {
                        { BinMonitorConst.SqlParmaReportStartDate,DateTime.Now.ToString("MM-dd-yyyy")},
                            { BinMonitorConst.SqlParmaReportEndDate,DateTime.Now.AddDays(1).ToString("MM-dd-yyyy")},
                            { BinMonitorConst.SqlParmaOpenBins,BinMonitorConst.SqlParmaValueDaily}
                    };
                    }
                    using (SqlDataReader dr = SqlCmd.SqlDataReader(BinMonitorConst.SpGetXmlAllBinsCategoriesReport, SqlConnection, dEmails).ConfigureAwait(false).GetAwaiter().GetResult())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                            {
                                string xmldata = dr[0].ToString();
                                if (!(string.IsNullOrWhiteSpace(xmldata)))
                                {
                                    CreateDailyBmReport(xmldata, de,reportType).ConfigureAwait(false).GetAwaiter().GetResult();
                                    dEmails.Clear();
                                    dEmails.Add(BinMonitorConst.SqlParmaEmailTo, BinMonitorConst.SqlParmaValueUpdateDate);
                                    dEmails.Add(BinMonitorConst.SqlParmaEmailCC, BinMonitorConst.SqlParmaValueUpdateDate);
                                    dEmails.Add(BinMonitorConst.SqlParmaEmailFrequency, BinMonitorConst.SqlParmaValueUpdateDate);
                                    UpdateEmailSentTime(dEmails,BinMonitorConst.SpEmailReports).ConfigureAwait(false).GetAwaiter().GetResult();
                                }
                            }
                        }
                        if (!(dr.IsClosed))
                            dr.Close();
                    }
                }
            }
        }
        private async Task UpdateEmailSentTime(Dictionary<string, string> dicUpCatTime, string spName)
        {
            using (SqlDataReader dr = SqlCmd.SqlDataReader(spName, SqlConnection, dicUpCatTime).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                if (!(dr.HasRows))
                    throw new Exception("no upate");
            }
        }
        public async Task CreateReportBinMonitorCategories(string xmlFile, EmailCategoriesModel emailCategories)
        {
            CreateReportSendEmail(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
            string emailSubject = ServiceHelpers.Instance.GetXmlValue(EmailTansFormXml, "//Bins//LabReqs//CategoryName").ConfigureAwait(true).GetAwaiter().GetResult();
            emailSubject = $"for Category {emailSubject}";
            SendEmailBM.SendEmailInstance.SendEmailMessage(BinMonitorXmlFile, string.Empty, EmailHtmlFile, emailCategories.EmailsTo, "noemailcc", true, emailSubject).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task CreateDailyBmReport(string xmlFile, DailyEmailModel daily,string reportType)
        {
            CreateReportSendEmail(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
           
          string  emailSubject = $"{reportType}";
            SendEmailBM.SendEmailInstance.SendEmailMessage(BinMonitorXmlFile, string.Empty, EmailHtmlFile, daily.EmailTo, daily.EmailCC, true, emailSubject).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task CreateReportSendEmail(string xmlFile)
        {
            ServiceHelpers.Instance.CreateDir(EmailHtmlFile).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            ServiceHelpers.Instance.WriteData(EmailTansFormXml, xmlFile, false).ConfigureAwait(false).GetAwaiter().GetResult();
            XslCompiledTransform xslt = new XslCompiledTransform();

            xslt.Load(XsltStyleSHeet);
            XPathDocument xmlDoc = new XPathDocument(EmailTansFormXml);
            using (XmlWriter xmlWriter = XmlWriter.Create(EmailHtmlFile, xslt.OutputSettings))
            {
                xslt.Transform(xmlDoc, xmlWriter);
                xmlWriter.Flush();
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProcessAuditLogs() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            try
            {
                TL.TraceLoggerInstance.CloseTraceFile();
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            catch { }
        }
        #endregion
    }


}

