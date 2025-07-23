using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.IO;
using Edocs.Service.BinMonitor.SendEmails.Models;
using Edocs.Service.BinMonitor.SendEmails.EmailSqlCommands;
using System.Xml;

namespace Edocs.Service.BinMonitor.SendEmails
{
    public class ServiceHelpers
    {

        static readonly ServiceHelpers ServiceHelpersInstance = new ServiceHelpers();
        static readonly int HrToMillSec = 3600000;
        static readonly int DefaultTSHr = 1;
        public static ServiceHelpers Instance
        { get { return ServiceHelpersInstance; } }

        private ServiceHelpers() { }
        public async Task<string> GetXmlValue(string xmlFile, string xPathQuery)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile);
            XmlNode xmlNode = xmlDocument.SelectSingleNode(xPathQuery);
            return xmlNode.InnerText;

        }
        public async Task<int> DefaultThreadSleep(string xmlFile)
        {
            //string defaultTs = GetXmlValue(xmlFile, $"//{BinMonitorConst.XmlRootNode}//{BinMonitorConst.BMEmailInfoXmlNodeBMEmailInfo}//{BinMonitorConst.BMEmailInfoXmlElementDefaultThreadSleepHr}").ConfigureAwait(false).GetAwaiter().GetResult();
            //if (!(string.IsNullOrWhiteSpace(defaultTs)))
            //{
            //    return ((int)StrToDouble(defaultTs).ConfigureAwait(false).GetAwaiter().GetResult());
            //}
            //return 1;
            return GetThreadSleepHrToMilliSec(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<double> StrToDouble(string intStr)
        {
            if (double.TryParse(intStr, out double result))
            {
                return result;
            }
            return (double)-1.0;
        }
        public async Task<int> GetThreadSleepHrToMilliSec(string xmlFile)
        {
            string defaultTs = GetXmlValue(xmlFile, $"//{BinMonitorConst.XmlRootNode}//{BinMonitorConst.BMEmailInfoXmlNodeBMEmailInfo}//{BinMonitorConst.BMEmailInfoXmlElementDefaultThreadSleepHr}").ConfigureAwait(false).GetAwaiter().GetResult();
            if (int.TryParse(defaultTs, out int result))
            {
                return result * HrToMillSec;
            }
            return DefaultTSHr * HrToMillSec;
        }
        public async Task<bool> SendEmail(DateTime lastTimeEmailSent, string emailFreq)
        {
            double freq = StrToDouble(emailFreq).ConfigureAwait(false).GetAwaiter().GetResult();
            if (freq > 0)
            {
                TimeSpan ts = DateTime.Now - lastTimeEmailSent;
                if (ts.TotalHours >= freq)
                    return true;
            }
            return false;
        }
        public async Task<SqlServerModel> GetSqlServerInfo(string xmlFileName)
        {
            SqlServerModel retInfo = new SqlServerModel();

            XDocument document = XDocument.Load(xmlFileName);
            var query = from useInfo in document.Descendants(BinMonitorConst.SqlXmlNodeSqlServerInfo)
                        select useInfo;
            foreach (var record in query)
            {
                retInfo.SqlServerName = record.Element(BinMonitorConst.SqlXmlElementSqlServerName).Value.ToString();
                retInfo.SqlDBName = record.Element(BinMonitorConst.SqlXmlElementDbName).Value.ToString();
                retInfo.SqlDBUserName = record.Element(BinMonitorConst.SqlXmlElementUserName).Value.ToString();
                retInfo.SqlDBPassWord = record.Element(BinMonitorConst.SqlXmlElementPassWord).Value.ToString();
            }
            return retInfo;
        }
        public async Task<SendEmailModel> GetEmailInfo(string xmlFileName)
        {
            SendEmailModel eModel = new SendEmailModel();

            XDocument document = XDocument.Load(xmlFileName);
            var query = from useInfo in document.Descendants(BinMonitorConst.EmailXmlNodeSqlServerInfo)
                        select useInfo;
            foreach (var record in query)
            {
                eModel.EmailCC = record.Element(BinMonitorConst.EmailXmlElementEmailCC).Value.ToString();
                eModel.EmailFrom = record.Element(BinMonitorConst.EmailXmlElementEmailFrom).Value.ToString();
                eModel.EmailPort = (int)StrToDouble(record.Element(BinMonitorConst.EmailXmlElementEmailPort).Value.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
                eModel.EmailPw = record.Element(BinMonitorConst.EmailXmlElementEmailPassword).Value.ToString();
                eModel.EmailServer = record.Element(BinMonitorConst.EmailXmlElementEmailServer).Value.ToString();
                eModel.EmailSubject = record.Element(BinMonitorConst.EmailXmlElementEmailSubject).Value.ToString();
                eModel.EmailTo = record.Element(BinMonitorConst.EmailXmlElementEmailTo).Value.ToString();
                eModel.TextCC = record.Element(BinMonitorConst.EmailXmlElementTextCC).Value.ToString();
                eModel.TextTO = record.Element(BinMonitorConst.EmailXmlElementTextTo).Value.ToString();
            }
            return eModel;
        }
        internal bool CheckFileExists(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                    return true;
            }
            catch { }
            return false;
        }
        internal async Task CreateDir(string dirName)
        {
            if (Path.HasExtension(dirName))
            {
                dirName = Path.GetDirectoryName(dirName);
            }
            if (!(Directory.Exists(dirName)))
                Directory.CreateDirectory(dirName);
        }
        internal async Task DelFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
        internal async Task WriteData(string fileName, string fileData, bool append)
        {
            CreateDir(fileName).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(append))
                DelFile(fileName).ConfigureAwait(false).GetAwaiter().GetResult();
            using (StreamWriter sw = new StreamWriter(fileName, append, System.Text.Encoding.UTF8))
            {
                sw.WriteAsync(fileData).ConfigureAwait(false).GetAwaiter().GetResult();
                sw.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        internal string GetApplicationDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        internal string GetApplicationName()
        {
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            return assembly.Name;
        }
        internal string GetApplicationSetting(string key)
        {

            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return ConfigurationManager.AppSettings.Get(key);
        }

        internal string StrReplace(string instr, string replaceValue, string newValue)
        {
            return instr.Replace(replaceValue, newValue);
        }

        internal async Task<bool> CheckRunService(string xmlFile)
        {
            string emailSt = GetXmlValue(xmlFile, $"//{BinMonitorConst.XmlRootNode}//{BinMonitorConst.BMEmailInfoXmlNodeBMEmailInfo}//{BinMonitorConst.BMEmailInfoXmlNodeEmailStartTime}").ConfigureAwait(false).GetAwaiter().GetResult();
            string emailEt = GetXmlValue(xmlFile, $"//{BinMonitorConst.XmlRootNode}//{BinMonitorConst.BMEmailInfoXmlNodeBMEmailInfo}//{BinMonitorConst.BMEmailInfoXmlNodeEmailEndTime}").ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(emailSt))
                throw new Exception("Email start time cannot be null");
            if (string.IsNullOrWhiteSpace(emailEt))
                throw new Exception("Email end time cannot be null");
            int stEmail = (int)StrToDouble(emailSt).ConfigureAwait(true).GetAwaiter().GetResult();
            int etEmail = (int)StrToDouble(emailEt).ConfigureAwait(true).GetAwaiter().GetResult();
            if ((stEmail < 0) || (etEmail < 0))
                throw new Exception("Could not get email start or end time");
            int currHr = (int)StrToDouble(DateTime.Now.ToString("HH")).ConfigureAwait(true).GetAwaiter().GetResult();
            if ((currHr >= stEmail) && (currHr <= etEmail))
                return true;
            return false;
        }
        internal async Task<string> ReadData(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
            {
                return sr.ReadToEndAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
