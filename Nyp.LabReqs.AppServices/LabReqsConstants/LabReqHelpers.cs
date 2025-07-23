using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDocs.Nyp.LabReqs.AppServices.LabReqsConstants
{
    public static class LabReqHelpers
    {
        private static System.Diagnostics.Stopwatch watch = null;
        public static void StartStopWatch()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
        }
        public static string StopStopWatch()
        {
            try
            { 
            watch.Stop();
         return ($"{watch.Elapsed}");
            }
            catch(Exception ex)
            {
                return $"Error getting end time {ex.Message}";
            }
        }


        public static string CheckDate(string sDate, string eDate)
        {
            try
            {
                if ((DateTime.TryParse(sDate, out DateTime sDateResults)) && (DateTime.TryParse(eDate, out DateTime eDateResults)))

                    if (((sDateResults.Year >= 1753) && (sDateResults.Year <= 9999)) && ((eDateResults.Year >= 1753) && (eDateResults.Year <= 9999)))
                    {
                        if (eDateResults.Year < sDateResults.Year)
                            throw new Exception($"End Scan Year {eDate} Cannot be less then Scan Year Start {sDate}");
                        else if ((eDateResults.Month < sDateResults.Month) && (eDateResults.Year == sDateResults.Year))
                            throw new Exception($"End Scan Month {eDate} Cannot be less then Scan Month Start {sDate}");
                        else
                            return string.Empty;
                    }

                    else
                        throw new Exception($"Invalid Scan Start Date {sDate} or Invalid Scan End Date {eDate}");
                else
                {
                    if (!(string.IsNullOrWhiteSpace(sDate)))
                        return $"Invalid search Start Date {sDate} or Invalid search End Date {eDate}";

                    return $"Invalid search Start Date or Invalid search End Date";
                }
            }
            catch (Exception ex)
            {
                return $"Invalid search Start Date: {sDate} or Invalid search End Date: {eDate} {ex.Message}";
            }
        }
        public static bool GetStartEndDate(string stdate,string enddate,ref DateTime dtSDate,ref DateTime dtEndDate)
        {
            if(!(string.IsNullOrWhiteSpace(stdate)))
            {
                dtSDate = GetLabRecDate(stdate);
                dtEndDate = GetLabRecDate(enddate);
                return true;
            }
            return false;
        }

        private static DateTime GetLabRecDate(string labRecDate)
        {

            if (DateTime.TryParse(labRecDate, out DateTime result))
            {
                return result;
            }
            return DateTime.Now;
        }

        public static DateTime AddDayToSearchEndDate(DateTime dateTime)
        {
            TimeSpan ts =  dateTime - DateTime.Now;
            if (ts.Days <= 0)
                return dateTime.AddDays(1);
            return dateTime;

        }
    }

    public class RejectionLogs
    {
        public string ReasonRej
        { get; set; }
    }

    public class NypInvoiceDepartment
    {
        public string Department
        { get; set; }
    }

    public class NypInvoiceAccount
    {
        public string Account
        { get; set; }
    }
    public class NypInvoiceCategory
    {
        public string Category
        { get; set; }
    }

    public class NypEmployeeComplianceLogs
    {
        public string NypEmployeeCompliance
        { get; set; }
    }
    public class NypPerformingLabCodeDOH
    {
        public string PerformingLabCodes
        { get; set; }
    }
    public class MaintenanceLogsLogStation
    {
        public string LogStation
        { get; set; }
    }
}
