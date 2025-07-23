using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EHU = Edocs.HelperUtilities;

namespace Edocs.Check.HL7.Running.ConstProperties
{
    class PropertiesConst
    {
        #region application config keys
        readonly string AppConfigKeyHL7ListenerLogsFolder = "HL7ListenerLogsFolder";
        readonly string AppConfigKeyHL7ListenerLogsFileName = "HL7ListenerLogsFileName";
        readonly string AppConfigKeyHL7TrafficLogsFolder = "HL7TrafficLogsFolder";
        readonly string AppConfigKeyHL7TrafficLogsFileName = "HL7TrafficLogsFileName";
        readonly string AppConfigKeyHL7TimeDiffMinutes = "HL7TimeDiffMinutes";
        readonly string AppConfigKeyHL7ThreadSleepSecs = "HL7ThreadSleepSecs";
        readonly string AppConfigKeyHL7ReStartProcess = "HL7ReStartProcess";
        readonly string AppConfigKeyHL7ThredSleepAfterRestartServiceSecs = "ThredSleepAfterRestartServiceSecs";

        readonly string AppConfigKeyAuditLogsFolder = "AuditLogsFolder";

        readonly string AppConfigKeyHL7ServiceName = "HL7ServiceName";
        readonly string AppConfigKeyHL7RunningWorkingFolder = "HL7RunningWorkingFolder";
        readonly string AppConfigKeyHL7StartServiceWaitForExitSeconds = "HL7StartServiceWaitForExitSeconds";





        readonly string AppConfigKeyHL7EmailTo = "EmailTo";
        readonly string AppConfigKeyHL7EmailCC = "EmailCC";
        readonly string AppConfigKeyHL7EmailSubject = "EmailSubject";
         


        #endregion
        readonly int DEFAULTSLEEP = 3600;
        readonly string RepStringApplicationDir = "{ApplicationDir}";
        const string RepStrYYYYMMDD = "yyyymmdd";
        private readonly int MillSeconds = 1000;
        private static volatile PropertiesConst hl7instance;
        private static object syncRoot = new object();
        private PropertiesConst() { }

        public static PropertiesConst HL7Instance
        {
            get
            {
                if (hl7instance == null)
                {
                    lock (syncRoot)
                    {
                        if (hl7instance == null)
                            hl7instance = new PropertiesConst();
                    }
                }

                return hl7instance;
            }
        }


        public string NoLogFile
        { get { return Path.Combine(EHU.Utilities.GetApplicationDir(),"NoLogFile.txt"); } }
        public string AuditLogsFolder
        { get { return EHU.Utilities.GetAppConfigSetting(AppConfigKeyAuditLogsFolder).Replace(RepStringApplicationDir, EHU.Utilities.GetApplicationDir()); } }

        public string HL7ListenerLogsFolder
        { get { return Path.Combine(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7ListenerLogsFolder), HL7ListenerLogsFileName); } }

        public string HL7ListenerLogsFileName
        {
            get
            {


                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7ListenerLogsFileName).Replace(RepStrYYYYMMDD, FormatDateTime);
            }
        }

        public string HL7TrafficLogsFolder
        {
            get
            {

                return Path.Combine(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7TrafficLogsFolder), HL7TrafficLogsFileName);
            }
        }

        public string EmailTo
        {
            get
            {


                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7EmailTo).Replace(RepStrYYYYMMDD, FormatDateTime);
            }
        }
        public string EmailCC
        {
            get
            {


                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7EmailCC).Replace(RepStrYYYYMMDD, FormatDateTime);
            }
        }
        public string HL7ServiceName
        {
            get
            {


                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7ServiceName).Replace(RepStrYYYYMMDD, FormatDateTime);
            }
        }


        public string EmailSubject
        {
            get
            {


                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7EmailSubject).Replace(RepStrYYYYMMDD, FormatDateTime);
            }
        }

        public string HL7TrafficLogsFileName
        {
            get
            {



                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7TrafficLogsFileName).Replace(RepStrYYYYMMDD, FormatDateTime);
            }
        }


        public int HL7ThreadSleepSecs
        {
            get
            {
                int sleepMillSeconds = (EHU.Utilities.ParseInt32(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7ThreadSleepSecs))) * MillSeconds;
                if (sleepMillSeconds <= 0)
                    sleepMillSeconds = DEFAULTSLEEP;
                return sleepMillSeconds;
            }
        }

        public int StartServiceWaitForExitSeconds
        {
            get
            {
                int sleepMillSeconds = (EHU.Utilities.ParseInt32(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7StartServiceWaitForExitSeconds))) * MillSeconds;
                if (sleepMillSeconds <= 0)
                    sleepMillSeconds = DEFAULTSLEEP;
                return sleepMillSeconds;
            }
        }
        public int ThredSleepAfterRestartServiceSecs
        {
            get
            {
                int sleepMillSeconds = (EHU.Utilities.ParseInt32(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7ThredSleepAfterRestartServiceSecs))) * MillSeconds;
                if (sleepMillSeconds <= 0)
                    sleepMillSeconds = DEFAULTSLEEP;
                return sleepMillSeconds;
            }
        }


       
        public bool HL7ReStartProcess
        {
            get
            {
                return EHU.Utilities.GetBool(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7ReStartProcess));
            }
        }
        public string FormatDateTime
        {
            get { return DateTime.Now.ToString("yyyyMMdd"); }
        }




        public string HL7RunningWorkingFolder
        {
            get
            {
                return EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7RunningWorkingFolder);
            }

        }
        public int HL7TimeDiffMinutes
        {
            get
            {
                return EHU.Utilities.ParseInt(EHU.Utilities.GetAppConfigSetting(AppConfigKeyHL7TimeDiffMinutes));
            }

        }



        public bool GetTimeDifference(string HL7TimeHR, ref int durDiff)
        {
            if (durDiff <= 0)
                durDiff = HL7TimeDiffMinutes;
            string Format24HR = DateTime.Now.ToString("HH:mm:ss");
            TimeSpan duration = DateTime.Parse(Format24HR).Subtract(DateTime.Parse(HL7TimeHR));
            if (duration.Minutes < 0)
            {
                durDiff = duration.Minutes;
                return false;
            }
            if (durDiff < duration.Minutes)
            {
                durDiff = duration.Minutes;
                return false;
            }
            //   TimeSpan ts = new TimeSpan()
            //if(Format24HR > 0)
            //{
            //    Format24HR = Math.Abs(HL7TimeHR - Format24HR);
            //    if (durDiff > Format24HR)
            //        return true;
            //}
            durDiff = duration.Minutes;
            return true;
        }

        public async Task<string> GetHHMM(string HL7LastLine)
        {

            string[] format24HR = HL7LastLine.Split('\t');
            return format24HR[0];



        }
    }
}

