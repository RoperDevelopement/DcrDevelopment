using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using MergeDemographicsCloud.Models;
using TL = EdocsUSA.Utilities.Logging;
using Edocs.HelperUtilities;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MergeDemographicsCloud.Interfaces;

namespace MergeDemographicsCloud
{
    public class SqlCmds
    {
        private readonly string AppKeySqlServerName = "SqlServerName";
        private readonly string AppKeySqlServerLabRecsDbName = "SqlServerLabRecsDbName";
        private readonly string AppKeySqlServerHl7DbName = "SqlServerHl7DbName";
        private readonly string AppKeySqlServerUserName = "SqlServerUserName";
        private readonly string AppKeySqlServerUserPw = "SqlServerUserPw";
        private readonly string AppKeyThreadSleepSeconds = "ThreadSleepSeconds";
        private readonly string AppKeyRegxSkip = "RegxSkip";
        private readonly string AppKeyRegxDateOfServiceMMDDYY = "RegxDateOfServiceMMDDYY";
        private readonly string AppKeyRegxDateOfServiceMonth = "RegxDateOfServiceMonth";


        private readonly string AppKeyNumberTimesToTryUpdateLR = "NumberTimesToTryUpdateLR";
        #region sql sp
        private readonly string SpUpDateLabRecsHL7ByLabReqRowID = "sp_UpDateLabRecsHL7ByLabReqRowID";
        private readonly string SpCheckLabReqMerged = "sp_CheckLabReqMerged";


        private readonly string SpGetHL7FilesByRowID = "sp_GetHL7FilesByRowID";
        private readonly string SpMergeLabRecs = "sp_MergeLabRecs";
        private readonly string SpGetHL7Files = "sp_GetHL7Files";
        private readonly string SpUpDateLabRecsHL7 = "sp_UpDateLabRecsHL7";
        private readonly string SpMergeLabRecsByID = "sp_MergeLabRecsByID";
        private readonly string SpUpDateHL7MergedByHl7RowID = "sp_UpDateHL7MergedByHl7RowID";
        private readonly string SpGetFullTextByID = "sp_GetFullTextByID";


        private readonly string SPUpDateHL7Merged = "sp_UpDateHL7Merged";
        public const int MillSecond = 1000;
        private readonly string CheckSPMergeRunning = "SELECT count(*) as countNumRunning FROM sys.dm_exec_requests req where req.command ='Merge';";
        #endregion
        #region sql params
        private readonly string SpParmLabReqRowID = "@LabReqRowID";
        private readonly string SpParmMerged = "@Merged";
        private readonly string SpParmHL7RowID = "@HL7RowID";
        private readonly string SpParmDateofService = "@DateofService";
        private readonly string SpParmEndDateOfService = "@EndDateOfService";
        private readonly string SpParmClientCode = "@ClientCode";
        private readonly string SpParmRequisitionNumber = "@RequisitionNumber";
        private readonly string SpParmFinancialNumber = "@FinancialNumber";
        private readonly string SpParmPatientID = "@PatientID";
        private readonly string SpParmMRN = "@MRN";
        private readonly string SpParmFileName = "@FileName";
        private readonly string SpParmPatientFirstName = "@PatientFirstName";
        private readonly string SpParmPatientLastName = "@PatientLastName";
        private readonly string SpParmDrCode = "@DrCode";
        private readonly string SpParmDrFirstName = "@DrFirstName";
        private readonly string SpParmDrLastName = "@DrLastName";
        private readonly string SpParmUserModified = "@UserModified";
        private readonly string SpParmMergeLabReqs = "@MergeLabReqs";
        #endregion
        #region sql fields
        private readonly string FieldDateOfService = "DateOfService";
        private readonly string FieldPatientID = "PatientID";
        private readonly string FieldMRN = "MRN";
        private readonly string FieldLabReqID = "LabReqID";
        private readonly string FieldImageFullText = "ImageFullText";

        private readonly string FieldClientCode = "ClientCode";
        private readonly string FieldPatientFirstName = "PatientFirstName";
        private readonly string FieldPatientLastName = "PatientLastName";
        private readonly string FieldFinancialNumber = "FinancialNumber";
        private readonly string FieldDrCode = "DrCode";
        private readonly string FieldDrName = "DrName";
        private readonly string FieldRequisitionNumber = "RequisitionNumber";
        private readonly string FieldIndexNumber = "IndexNumber";
        private readonly string FieldFileName = "FileName";
        private readonly string FieldHL7RowID = "HL7RowID";

        private readonly string FieldDateAdded = " DateAdded";
        private readonly string FieldDateModified = "DateModified";
        private readonly string FieldScanDate = "ScanDate";

        private readonly string FieldIndexnumberNoZeros = "IndexnumberNoZeros";
        private readonly int MinPatId = 5;
        private readonly int MaxPatId = 13;
        private readonly string RegxMatch = @"\d+";

        #endregion





        private string RegxDateOfServiceMMDDYY
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyRegxDateOfServiceMMDDYY); }
        }
        private string RegxDateOfServiceMonth
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyRegxDateOfServiceMonth); }
        }

        private string ThreadSleepSeconds
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyThreadSleepSeconds); }
        }
        private string RegxSkip
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyRegxSkip); }
        }
        private string NumberTimesToTryUpdateLR
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyNumberTimesToTryUpdateLR); }
        }

        public int TotalSkipped
        { get; set; }

        private string SqlSever
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerName); } }

        private string DbUserName
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerUserName); } }

        private string DbPasswod
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerUserPw); } }

        private string SqlDataBaseName(string dbName)
        {
            return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(dbName);
        }
        private SqlConnection SqlConn
        { get; set; }

        private SqlConnection OpenSqlConnection(string dbName)
        {

            string connStr = GetConnectionString(dbName);
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opening sql connection connection string for database name {dbName}");
            SqlConnection sqlConnection = new SqlConnection(connStr);
            sqlConnection.Open();
            return sqlConnection;
        }
        private async Task CheckMergeRunning(int tSleep)
        {
            SqlConn = OpenSqlConnection(AppKeySqlServerLabRecsDbName);
            using (SqlCommand cmd = new SqlCommand(CheckSPMergeRunning, SqlConn))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                SqlDataReader dr = cmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                if (dr.HasRows)
                {
                    dr.Read();
                    int count = Utilities.ParseInt(dr[0].ToString());
                    if (count > 0)
                        Thread.Sleep(tSleep);

                }
            }
            if (SqlConn.State == ConnectionState.Open)
                SqlConn.Close();
        }
        public void CheckLabReqMerg(int LrNumber)
        {
            SqlConn = OpenSqlConnection(AppKeySqlServerLabRecsDbName);
            try
            {
                using (SqlCommand cmd = new SqlCommand(SpCheckLabReqMerged, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParmLabReqRowID, LrNumber));
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!(dr.HasRows))
                            throw new Exception($"Labreq ID {LrNumber} not merged");
                    }


                }

                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<HL7Model> GetFullTextDos(List<string> fNumbers, DateTime endDate, string fText, int merge)
        {
            //    MatchCollection match = Regex.Matches(fText, RegxDateOfService, RegexOptions.IgnoreCase);
            //  if (match.Count > 0)
            //  {
            //foreach (string str in fText.Split('\r'))
            //{
            //    string matchStr = str.Replace('\n', ' ');
            //    matchStr = matchStr.Replace("/", "-");
            //    matchStr = matchStr.Replace("\\", "-");
            fText = fText.Replace("/", "-");
            fText = fText.Replace("\\", "-");
            string month = fText;
            string regXMatch = $"{RegxDateOfServiceMMDDYY.Trim()}|{RegxDateOfServiceMonth.Trim()}";
            MatchCollection match = Regex.Matches(fText, regXMatch, RegexOptions.IgnoreCase);
            if (match.Count > 0)
            {
                foreach (Match m in match)
                {
                    if (Regex.IsMatch(m.Value, RegxDateOfServiceMMDDYY.Trim(), RegexOptions.IgnoreCase))
                    {
                        if (DateTime.TryParse(m.Value, out DateTime results))
                        {
                            if (results.Year > 2020)
                            {
                                foreach (string finNum in fNumbers)
                                {

                                    LabRecsModel labRecs = new LabRecsModel();
                                    labRecs.IndexnumberNoZeros = finNum;
                                    labRecs.RequisitionNumber = finNum;
                                    HL7Model hL7 = GetHl7(results, endDate, labRecs, merge);
                                    if (hL7 != null)
                                        return hL7;
                                }
                            }
                        }
                    }
                    else
                    {
                        int index = month.IndexOf(m.Value);
                        if(index > 0)
                        {
                             month = month.Substring(index);
                        }
                    }
                }
                //if (Regex.IsMatch(matchStr.Trim(), RegxDateOfService, RegexOptions.IgnoreCase))
                //{
                //    Console.WriteLine();
                //    // if (match.Count > 0)
                //    //    foreach(Match m in match)
                //    //  { 
                //    //if (DateTime.TryParse(m.Value, out DateTime results))
                //    //{
                //    //    if (results.Year > 2020)
                //    //    {
                //    //        foreach (string finNum in fNumbers)
                //    //        {

                //    //            LabRecsModel labRecs = new LabRecsModel();
                //    //            labRecs.IndexnumberNoZeros = finNum;
                //    //            labRecs.RequisitionNumber = finNum;
                //    //            HL7Model hL7 = GetHl7(results, endDate, labRecs, merge);
                //    //            if (hL7 != null)
                //    //                return hL7;
                //    //        }
                //    //    }
                //    //}
                //}
            }
            //  }
            //  }
            return null;
        }
        public async Task UpdateLRTable(HL7Model hL7, LabRecsModel labRecs, string spNameUpDate)
        {

            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating labreq {labRecs.IndexNumber} file name {labRecs.FileName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Updating labreq {labRecs.IndexNumber} file name {labRecs.FileName}");
            int numLoop = Utilities.ParseInt(NumberTimesToTryUpdateLR);
            int tSleep = Utilities.ParseInt(ThreadSleepSeconds) * MillSecond;
            string errmess = string.Empty;

            while (numLoop-- > 0)
            {
                try
                {
                    errmess = string.Empty;
                    UpdateLabRecs(hL7, labRecs, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                }
                catch (Exception ex)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Num Loop {numLoop}  {ex.Message}");
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Sql Server error Num Loop {numLoop}  {ex.Message}");
                    errmess = ex.Message;
                    Thread.Sleep(tSleep);
                    CheckMergeRunning(tSleep).ConfigureAwait(false).GetAwaiter().GetResult();
                }

            }


            if (!(string.IsNullOrWhiteSpace(errmess)))
                throw new Exception(errmess);
        }
        public async Task UpDateMergeHL7Files(IDictionary<string, HL7Model> hL7)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("In Method UpDateMergeHL7Files list");
            foreach (KeyValuePair<string, HL7Model> hL7Model in hL7)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method UpDateMergeHL7Files list setting merged to 1 for fin number {hL7Model.Key}");
                UpDateMergeHL7Files(hL7Model.Value).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        public async Task UpDateMergeHL7Files(HL7Model hL7)
        {
            try
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for DateOfService {hL7.DateOfService} FinancialNumber {hL7.FinancialNumber}");
                SqlConn = OpenSqlConnection(AppKeySqlServerHl7DbName);
                using (SqlCommand cmd = new SqlCommand(SPUpDateHL7Merged, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParmDateofService, hL7.DateOfService));
                    if (string.IsNullOrWhiteSpace(hL7.PatientID))
                    {

                        cmd.Parameters.Add(new SqlParameter(SpParmPatientID, DBNull.Value));
                    }

                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientID, hL7.PatientID));
                    if (string.IsNullOrWhiteSpace(hL7.FinancialNumber))
                        cmd.Parameters.Add(new SqlParameter(SpParmFinancialNumber, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmFinancialNumber, hL7.FinancialNumber));
                    if (string.IsNullOrWhiteSpace(hL7.RequisitionNumber))
                        cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, hL7.RequisitionNumber));
                    cmd.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                TL.TraceLogger.TraceLoggerInstance.TraceError($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for DateOfService {hL7.DateOfService} FinancialNumber {hL7.FinancialNumber} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for DateOfService {hL7.DateOfService} FinancialNumber {hL7.FinancialNumber} {ex.Message}");
                //throw new Exception($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for DateOfService {hL7.DateOfService} FinancialNumber {hL7.FinancialNumber} {ex.Message}");
            }

        }
        public async Task UpDateMergeHL7Files(int hl7RowID)
        {
            try
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for hl7 row id {hl7RowID}");
                SqlConn = OpenSqlConnection(AppKeySqlServerHl7DbName);
                using (SqlCommand cmd = new SqlCommand(SpUpDateHL7MergedByHl7RowID, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParmHL7RowID, hl7RowID));
                    cmd.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                TL.TraceLogger.TraceLoggerInstance.TraceError($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for hl7 row id {hl7RowID} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for hl7 row id {hl7RowID} {ex.Message}");
                //throw new Exception($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for DateOfService {hL7.DateOfService} FinancialNumber {hL7.FinancialNumber} {ex.Message}");
            }

        }
        public async Task UpDateMergeHL7Files(List<int> hl7RowID)
        {
            try
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}");
                foreach (var id in hl7RowID)
                {
                    UpDateMergeHL7Files(id).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {

                TL.TraceLogger.TraceLoggerInstance.TraceError($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for hl7 row id {hl7RowID} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for hl7 row id {hl7RowID} {ex.Message}");
                //throw new Exception($"Setting merged to 1 in database {Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName)}  for DateOfService {hL7.DateOfService} FinancialNumber {hL7.FinancialNumber} {ex.Message}");
            }

        }
        public HL7Model GetHl7MatchDefault(DateTime dateOfService, DateTime endDateOfService, LabRecsModel hLSeven, int merge, int ds)
        {
            return GetHl7(dateOfService, endDateOfService, hLSeven, merge);
        }

        public HL7Model GetFullTextHL7(DateTime dateOfService, DateTime endDateOfService, int labReqID, int merge, int ds)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"GetFullTextHL7 for start date {dateOfService} end date {endDateOfService} labreqid {labReqID} for merge {merge}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"GetFullTextHL7 for start date {dateOfService} end date {endDateOfService} for merge {merge}");
            LabReqsFullTextModel labReqsFullText = GetLRFullText(labReqID, SpGetFullTextByID).ConfigureAwait(false).GetAwaiter().GetResult();
            List<string> finNum = new List<string>();
            if (labReqsFullText != null)
            {
                MatchCollection match = Regex.Matches(labReqsFullText.ImageFullText, RegxMatch, RegexOptions.IgnoreCase);
                if (match.Count > 0)
                {
                    foreach (Match m in match)
                    {
                        if (m.Value.StartsWith("0"))
                        {
                            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Skipping {m.Value} since starts with 0");
                            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Skipping {m.Value} since starts with 0");
                            TotalSkipped++;
                            continue;
                        }
                        if ((m.Value.Length > 6) && (m.Value.Length < 11))
                        {
                            finNum.Add(m.Value);
                            LabRecsModel labRecs = new LabRecsModel();
                            labRecs.IndexnumberNoZeros = m.Value;
                            labRecs.RequisitionNumber = m.Value;
                            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for HL7 for using full text for finnumber {labRecs.IndexnumberNoZeros}");
                            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Looking for HL7 using full text for finnumber {labRecs.IndexnumberNoZeros}");
                            HL7Model hL7 = GetHl7(dateOfService, endDateOfService, labRecs, merge);
                            if (hL7 != null)
                                return hL7;
                        }
                    }
                }
            }
            if (finNum.Count() > 0)
            {
                HL7Model hL7 = GetFullTextDos(finNum, endDateOfService, labReqsFullText.ImageFullText, merge).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                if (hL7 != null)
                    return hL7;
            }
            return null;
        }
        public HL7Model GetHl7Match(DateTime dateOfService, DateTime endDateOfService, LabRecsModel hLSeven, int merge, int ds)
        {
            int startPatId = MinPatId;
            HL7Model model = null;
            string lrNumCurr = hLSeven.IndexNumber;
            if (hLSeven.IndexNumber.ToUpper().StartsWith("GRACIE"))
                return model;
            string currPatID = hLSeven.PatientID;
            string currLR = hLSeven.RequisitionNumber;
            string currFin = hLSeven.IndexnumberNoZeros;

            while (startPatId <= MaxPatId)
            {
                model = GetHl7(dateOfService, endDateOfService, hLSeven, merge);
                if (model != null)
                    return model;

                //if (ds < 2)
                //{
                //    return model;

                //}

                if ((lrNumCurr.Length > startPatId) && (startPatId < MaxPatId))
                {

                    hLSeven.PatientID = lrNumCurr.Substring(lrNumCurr.Length - startPatId);
                    hLSeven.RequisitionNumber = hLSeven.PatientID;
                    hLSeven.IndexnumberNoZeros = hLSeven.PatientID;
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Using patien id/lab req number {hLSeven.PatientID} Using for FIn ID {hLSeven.IndexNumber} number loop {startPatId}");
                }
                else
                {
                    hLSeven.PatientID = currPatID;
                    hLSeven.RequisitionNumber = currLR;
                    hLSeven.IndexnumberNoZeros = currFin;
                    return null;
                }

                startPatId++;
            }
            if (model == null)
            {
                hLSeven.PatientID = currPatID;
                hLSeven.RequisitionNumber = currLR;

            }

            return model;
        }
        public HL7Model GetHl7Match(int merge, int ds, DateTime dateOfService, DateTime endDateOfService, LabRecsModel hLSeven)
        {

            HL7Model model = null;
            if (hLSeven.IndexNumber.ToUpper().StartsWith("GRACIE"))
                return model;
            model = GetHl7(dateOfService, endDateOfService, hLSeven, merge);
            return model;

        }
        public HL7Model GetHl7(DateTime dateOfService, DateTime endDateOfService, LabRecsModel hLSeven, int merge)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting hl7 record for start date of service {dateOfService.ToString()} for end date of sevice {endDateOfService.ToString()} for index number {hLSeven.IndexNumber} for rec number {hLSeven.RequisitionNumber} for date of service {hLSeven.DateOfService.ToString()}");
            HL7Model hL7 = null;
            try
            {
                SqlConn = OpenSqlConnection(AppKeySqlServerHl7DbName);
                using (SqlCommand cmd = new SqlCommand(SpGetHL7FilesByRowID, SqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.Add(new SqlParameter(SpParmDateofService, dateOfService.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new SqlParameter(SpParmEndDateOfService, endDateOfService.ToString("yyyy-MM-dd")));
                 //   if (!(string.IsNullOrWhiteSpace(hLSeven.PatientID)))
                  //      cmd.Parameters.Add(new SqlParameter(SpParmPatientID, hLSeven.PatientID));
                 //   else
                 //   {
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientID, DBNull.Value));
                 //   }

                    // if (hLSeven.ClientCode == "000000")
                    cmd.Parameters.Add(new SqlParameter(SpParmClientCode, DBNull.Value));
                    //else if (!(string.IsNullOrWhiteSpace(hLSeven.ClientCode)))
                    //  cmd.Parameters.Add(new SqlParameter(SpParmClientCode, hLSeven.ClientCode));
                    //  else
                    //    cmd.Parameters.Add(new SqlParameter(SpParmClientCode, DBNull.Value));
                    if (!(string.IsNullOrWhiteSpace(hLSeven.RequisitionNumber)))
                        cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, hLSeven.RequisitionNumber));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, DBNull.Value));
                    if (!(string.IsNullOrWhiteSpace(hLSeven.IndexnumberNoZeros)))
                        cmd.Parameters.Add(new SqlParameter(SpParmFinancialNumber, hLSeven.IndexnumberNoZeros));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmFinancialNumber, DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter(SpParmMerged, merge));
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        IList<HL7Model> lHl7 = new List<HL7Model>();
                        int numRows = 0;
                        while (dr.Read())
                        {

                            hL7 = new HL7Model();
                            hL7.HL7RowID = int.Parse(dr[FieldHL7RowID].ToString());
                            hL7.ClientCode = dr[FieldClientCode].ToString();
                            hL7.DateOfService = DateTime.Parse(dr[FieldDateOfService].ToString());
                            hL7.DrCode = dr[FieldDrCode].ToString();
                            if (!(string.IsNullOrWhiteSpace(dr[FieldDrName].ToString())))
                            {
                                string[] drName = dr[FieldDrName].ToString().Split(',');
                                if (drName.Length == 2)
                                {
                                    hL7.DrFName = drName[1];
                                    hL7.DrLName = drName[0];
                                }
                                else
                                {
                                    hL7.DrFName = hL7.DrLName = drName[0];

                                }

                            }
                            else
                            {
                                hL7.DrFName = string.Empty;
                                hL7.DrLName = string.Empty;
                            }
                            if (!(string.IsNullOrWhiteSpace(dr[FieldMRN].ToString())))
                                hL7.PatientID = dr[FieldMRN].ToString();
                            else
                                hL7.PatientID = string.Empty;

                            hL7.FinancialNumber = dr[FieldFinancialNumber].ToString();

                            hL7.PatientFirstName = dr[FieldPatientFirstName].ToString();
                            hL7.PatientLastName = dr[FieldPatientLastName].ToString();
                            hL7.RequisitionNumber = dr[FieldRequisitionNumber].ToString();
                            numRows++;
                            lHl7.Add(hL7);
                        }



                        if (lHl7.Count > 1)
                        {
                            //L7 = FindBestMatchHL7(lHl7, hLSeven);
                            hL7 = lHl7[lHl7.Count - 1];
                        }

                    }


                }
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Getting hl7 record for start date of service {dateOfService.ToString()} for end date of sevice {endDateOfService.ToString()} for index number {hLSeven.IndexNumber} for rec number {hLSeven.RequisitionNumber} for date of service {hLSeven.DateOfService.ToString()} {ex.Message}");
                throw new Exception($"Getting hl7 record for start date of service {dateOfService.ToString()} for end date of sevice {endDateOfService.ToString()} for index number {hLSeven.IndexNumber} for rec number {hLSeven.RequisitionNumber} for date of service {hLSeven.DateOfService.ToString()} {ex.Message}");
            }
            return hL7;
        }
        private HL7Model FindBestMatchHL7(IList<HL7Model> hL7s, LabRecsModel hLSeven)
        {
            HL7Model model = null;
            foreach (HL7Model hL in hL7s)
            {
                TimeSpan ts = hL.DateOfService - hLSeven.DateOfService;
                int totalDays = Math.Abs(ts.Days);

                if ((string.Compare(hLSeven.RequisitionNumber, hL.RequisitionNumber, true) == 0) && (!(string.IsNullOrWhiteSpace(hL.RequisitionNumber))))
                {
                    model = hL;
                    if (totalDays <= 3)
                        return model;
                }
                else if ((string.Compare(hLSeven.PatientID, hL.PatientID, true) == 0) && (!(string.IsNullOrWhiteSpace(hL.PatientID))))
                {
                    model = hL;
                    if (totalDays <= 3)
                        return model;
                }
                else
                {
                    if (string.Compare(hLSeven.IndexnumberNoZeros, hL.FinancialNumber, true) == 0)
                    {
                        model = hL;
                        if (totalDays <= 3)
                            return model;
                    }
                    else if (string.Compare(hLSeven.FinancialNumber, hL.FinancialNumber, true) == 0)
                    {
                        model = hL;
                        if (totalDays <= 3)
                            return model;
                    }
                    else
                    {
                        if (string.Compare(hLSeven.IndexNumber, hL.FinancialNumber, true) == 0)
                        {
                            model = hL;
                            if (totalDays <= 3)
                                return model;
                        }
                    }
                }
            }
            return model;
        }
        private DateTime ConverToDateTime(string dateToConvert)
        {
            if (DateTime.TryParse(dateToConvert, out DateTime results))
            {
                return results;
            }
            return DateTime.Now;
        }
        private string GetConnectionString(string dbName)
        {
            dbName = SqlDataBaseName(dbName);
            //return string.Format("Server={0};Database={1};Trusted_Connection=True; User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=True;", SqlSever, dbName, DbUserName, DbPasswod, 1000);
            TL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Using Connectin Server={0};Database={1};Connect Timeout={2};MultipleActiveResultSets=true;", SqlSever, dbName, 180));
            return string.Format("Server={0};Database={1};User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=true;", SqlSever, dbName, DbUserName, DbPasswod, 180);
        }
        public async Task ReMerge(LabRecsModel labRecs, string spNameUpDate)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"ReMerging lab recs with hl7 informaion for sp name {spNameUpDate} for file name {labRecs.FileName}");
            HL7Model hL7 = new HL7Model();
            hL7.ClientCode = labRecs.ClientCode;
            hL7.DateOfService = labRecs.DateOfService;
            hL7.FinancialNumber = labRecs.IndexnumberNoZeros;
            if ((labRecs.MRN.StartsWith("5000")))
            {
                labRecs.PatientID = labRecs.MRN;
                hL7.PatientID = labRecs.MRN;
            }
            else
            {
                if (!(string.IsNullOrWhiteSpace(labRecs.MRN)))
                {
                    if (labRecs.MRN.Length != 9)
                    {
                        labRecs.PatientID = labRecs.MRN;
                        hL7.PatientID = labRecs.MRN;
                    }
                    else
                    {
                        labRecs.PatientID = string.Empty;
                        hL7.PatientID = string.Empty;
                        labRecs.MRN = string.Empty;
                    }
                }
            }
            // UpdateLabRecs(hL7, labRecs, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
            UpdateLRTable(hL7, labRecs, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();

        }
        public async Task UpdateLabRecs(HL7Model hL7, LabRecsModel labRecs, string spNameUpDate)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating lab recs with hl7 informaion for sp name {spNameUpDate} for file name {labRecs.FileName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating lab recs with hl7 informaion for sp name {spNameUpDate} for file name {labRecs.FileName}");
            try
            {
                SqlConn = OpenSqlConnection(AppKeySqlServerLabRecsDbName);
                using (SqlCommand cmd = new SqlCommand(spNameUpDate, SqlConn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParmLabReqRowID, labRecs.LabReqID));
                    cmd.Parameters.Add(new SqlParameter(SpParmFileName, labRecs.FileName));
                    cmd.CommandTimeout = 180;
                    if (string.Compare(hL7.DateOfService.ToString("MM-dd-yyyy"), labRecs.DateOfService.ToString("MM-dd-yyyy")) != 0)
                    {
                        TL.TraceLogger.TraceLoggerInstance.TraceWarning($"HL7 Date of Service different Lab recs date of service {labRecs.DateOfService.ToString()} and HL7 date of service:{hL7.DateOfService.ToString()}");
                        //    cmd.Parameters.Add(new SqlParameter(SpParmDateofService, hL7.DateOfService));
                    }
                    //else
                    cmd.Parameters.Add(new SqlParameter(SpParmDateofService, hL7.DateOfService));
                    if (string.Compare(hL7.PatientID, labRecs.PatientID, true) != 0)
                    {
                        TL.TraceLogger.TraceLoggerInstance.TraceWarning($"HL7 PatientID differnet then Lab recs PatientID {labRecs.PatientID} and HL7 PatientID {hL7.PatientID}");
                        //        cmd.Parameters.Add(new SqlParameter(SpParmPatientID, hL7.PatientID));
                    }
                    if ((!(string.IsNullOrWhiteSpace(hL7.PatientID))))
                    {
                        labRecs.PatientID = hL7.PatientID;
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientID, labRecs.PatientID));
                    }
                    else
                    {
                        if ((string.IsNullOrWhiteSpace(labRecs.PatientID)))
                        {
                            cmd.Parameters.Add(new SqlParameter(SpParmPatientID, DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter(SpParmPatientID, labRecs.PatientID));
                        }

                    }

                    //else
                    if (!(string.IsNullOrEmpty(hL7.PatientID)))
                        cmd.Parameters.Add(new SqlParameter(SpParmMRN, hL7.PatientID));
                    else
                    {
                        if (!(string.IsNullOrWhiteSpace(labRecs.PatientID)))
                            cmd.Parameters.Add(new SqlParameter(SpParmMRN, labRecs.PatientID));
                        else
                            cmd.Parameters.Add(new SqlParameter(SpParmMRN, DBNull.Value));
                    }

                    if (string.Compare(hL7.ClientCode, labRecs.ClientCode, true) != 0)
                    {
                        TL.TraceLogger.TraceLoggerInstance.TraceWarning($"HL7 ClientCode different then lab recs ClientCode {labRecs.ClientCode} and HL7 Client code {hL7.ClientCode}");
                        cmd.Parameters.Add(new SqlParameter(SpParmClientCode, hL7.ClientCode));
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(labRecs.ClientCode))
                            cmd.Parameters.Add(new SqlParameter(SpParmClientCode, DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter(SpParmClientCode, labRecs.ClientCode));
                    }
                    //else
                    if (!(string.IsNullOrWhiteSpace(hL7.PatientFirstName)))
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientFirstName, hL7.PatientFirstName));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientFirstName, DBNull.Value));
                    if (!(string.IsNullOrWhiteSpace(hL7.PatientLastName)))
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientLastName, hL7.PatientLastName));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmPatientLastName, DBNull.Value));
                    if (!(string.IsNullOrWhiteSpace(hL7.FinancialNumber)))
                        cmd.Parameters.Add(new SqlParameter(SpParmFinancialNumber, hL7.FinancialNumber));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmFinancialNumber, labRecs.IndexnumberNoZeros));
                    if (!(string.IsNullOrWhiteSpace(hL7.DrCode)))
                        cmd.Parameters.Add(new SqlParameter(SpParmDrCode, hL7.DrCode));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmDrCode, DBNull.Value));
                    if (!(string.IsNullOrWhiteSpace(hL7.DrFName)))
                        cmd.Parameters.Add(new SqlParameter(SpParmDrFirstName, hL7.DrFName));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmDrFirstName, DBNull.Value));
                    if (!(string.IsNullOrWhiteSpace(hL7.DrLName)))
                        cmd.Parameters.Add(new SqlParameter(SpParmDrLastName, hL7.DrLName));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmDrLastName, DBNull.Value));

                    if ((string.Compare(labRecs.RequisitionNumber, hL7.RequisitionNumber) != 0))
                    {

                        TL.TraceLogger.TraceLoggerInstance.TraceWarning($"HL7 RequisitionNumber {hL7.RequisitionNumber} does not match lab RequisitionNumber {labRecs.RequisitionNumber}");
                        //  cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, hL7.RequisitionNumber));
                        //cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, hL7.RequisitionNumber));
                    }

                    if (string.IsNullOrWhiteSpace(labRecs.RequisitionNumber))
                    {
                        if (!(string.IsNullOrWhiteSpace(hL7.RequisitionNumber)))
                            cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, hL7.RequisitionNumber));
                        else
                            cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, DBNull.Value));
                    }
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParmRequisitionNumber, labRecs.RequisitionNumber));




                    cmd.Parameters.Add(new SqlParameter(SpParmUserModified, Environment.UserName));
                    cmd.ExecuteNonQueryAsync().ConfigureAwait(true).GetAwaiter().GetResult();



                }
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                //  UpDateMergeHL7Files(hL7);
            }
            catch (Exception ex)
            {
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Updating lab recs with hl7 informaion hL7 FinancialNumber {hL7.FinancialNumber} PatientID {hL7.PatientID} hL7 DateOfService {hL7.DateOfService.ToString()} lab recs date of service {labRecs.DateOfService.ToString()} labreq 9 digit fin number {labRecs.IndexnumberNoZeros} labreq fin number {labRecs.IndexNumber} for file name {labRecs.FileName} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Updating lab recs with hl7 informaion hL7 FinancialNumber {hL7.FinancialNumber} PatientID {hL7.PatientID} hL7 DateOfService {hL7.DateOfService.ToString()} lab recs date of service {labRecs.DateOfService.ToString()} labreq 9 digit fin number {labRecs.IndexnumberNoZeros} labreq fin number {labRecs.IndexNumber} for file name {labRecs.FileName} {ex.Message}");
                throw new Exception($"Updating lab recs with hl7 informaion hL7 FinancialNumber {hL7.FinancialNumber} PatientID {hL7.PatientID} hL7 DateOfService {hL7.DateOfService.ToString()} lab recs date of service {labRecs.DateOfService.ToString()} labreq 9 digit fin number {labRecs.IndexnumberNoZeros} labreq fin number {labRecs.IndexNumber} for file name {labRecs.FileName} {ex.Message}");
            }
        }

        private async Task<LabReqsFullTextModel> GetLRFullText(int labReqID, string spName)
        {

            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab recs Full text for id {labReqID} for spname {spName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting lab recs Full text for id {labReqID} for spname {spName}");
            SqlConn = OpenSqlConnection(AppKeySqlServerLabRecsDbName);
            LabReqsFullTextModel labReqsFullText = null;
            using (SqlCommand cmd = new SqlCommand(spName, SqlConn))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter(SpParmLabReqRowID, labReqID));
                using (SqlDataReader reader = cmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    if (reader.HasRows)
                    {
                        labReqsFullText = new LabReqsFullTextModel();
                        while (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            labReqsFullText.LabReqID = int.Parse(reader[FieldLabReqID].ToString());
                            labReqsFullText.ImageFullText = reader[FieldImageFullText].ToString();
                            labReqsFullText.DateModified = DateTime.Parse(reader[FieldDateModified].ToString());
                            //  labReqsFullText.DateOfService = DateTime.Parse(reader[FieldDateAdded].ToString());
                            labReqsFullText.ScanDate = DateTime.Parse(reader[FieldScanDate].ToString());
                        }
                    }
                    else
                    {
                        TL.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"No rows found  for id {labReqID} for spname {spName}");
                        TL.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"No rows found  for id {labReqID} for spname {spName}");
                    }
                }
            }
            return labReqsFullText;
        }
        public IEnumerable<LabRecsModel> GetLabRecs(DateTime dateOfServices, string spNameMerge, int mergeLabReq)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab recs for date of service {dateOfServices.ToString()} for spname {spNameMerge}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting lab recs for date of service {dateOfServices.ToString()} for spname {spNameMerge}");
            SqlConn = OpenSqlConnection(AppKeySqlServerLabRecsDbName);
            TotalSkipped = 0;
            string rexMatch = RegxSkip;
            using (SqlCommand cmd = new SqlCommand(spNameMerge, SqlConn))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter(SpParmDateofService, dateOfServices));
                cmd.Parameters.Add(new SqlParameter(SpParmMergeLabReqs, mergeLabReq));


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LabRecsModel hLSevenLabRecs = new LabRecsModel();
                        hLSevenLabRecs.LabReqID = int.Parse(reader[FieldLabReqID].ToString());
                        hLSevenLabRecs.ClientCode = reader[FieldClientCode].ToString();
                        hLSevenLabRecs.DateOfService = DateTime.Parse(reader[FieldDateOfService].ToString());
                        hLSevenLabRecs.FinancialNumber = reader[FieldFinancialNumber].ToString();
                        hLSevenLabRecs.IndexNumber = reader[FieldIndexNumber].ToString();
                        // hLSevenLabRecs.IndexnumberNoZeros = reader[FieldIndexnumberNoZeros].ToString();
                        hLSevenLabRecs.PatientID = reader[FieldPatientID].ToString();
                        hLSevenLabRecs.RequisitionNumber = reader[FieldRequisitionNumber].ToString();
                        hLSevenLabRecs.FileName = reader[FieldFileName].ToString();
                        hLSevenLabRecs.MRN = reader[FieldMRN].ToString();
                        Match match = Regex.Match(hLSevenLabRecs.IndexNumber, rexMatch, RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            TotalSkipped++;
                            continue;
                        }


                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("ERROR"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("TSTLOG"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("CANCEL"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("PRBLOG"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("GRA"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("BMH"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("NYPHWCD"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("K0"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("PROLOG"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("ST"))
                        //    continue;
                        //if (hLSevenLabRecs.IndexNumber.ToUpper().StartsWith("DHK"))

                        //    continue;



                        hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.FinancialNumber;


                        if (hLSevenLabRecs.IndexNumber.Length > 9)
                        {
                            hLSevenLabRecs.ClientCode = hLSevenLabRecs.IndexNumber.Substring(0, 6);
                        }



                        if ((hLSevenLabRecs.IndexNumber.StartsWith("00000201")) || (hLSevenLabRecs.IndexNumber.StartsWith("0201")) || (hLSevenLabRecs.IndexNumber.StartsWith("201")))
                        {
                            //  if (hLSevenLabRecs.IndexNumber.Length > 9)
                            //  {
                            //       hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.IndexNumber.Substring(5).TrimStart('0');

                            //   }
                            //  else
                            if ((hLSevenLabRecs.IndexnumberNoZeros.Length > 10) && (hLSevenLabRecs.IndexNumber.Length > 9))
                                hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.IndexNumber.TrimStart('0');
                        }

                        if ((string.Compare(hLSevenLabRecs.IndexnumberNoZeros, hLSevenLabRecs.IndexNumber, true) == 0) && (hLSevenLabRecs.IndexnumberNoZeros.Length > 9))
                        {
                            hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.IndexnumberNoZeros.Substring(6).TrimStart('0');
                            if (hLSevenLabRecs.IndexnumberNoZeros.Length > 8)
                            {
                                int indexOne = hLSevenLabRecs.IndexnumberNoZeros.IndexOf("1");
                                if (indexOne > 0)
                                {
                                    string tempStr = hLSevenLabRecs.IndexnumberNoZeros.Substring(indexOne).TrimStart('0');
                                    if ((tempStr.Length == 10) || (tempStr.Length == 9))
                                        hLSevenLabRecs.IndexnumberNoZeros = tempStr;
                                }

                            }


                            //else
                            //    hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.IndexNumber.TrimStart('0');
                        }
                        if ((hLSevenLabRecs.IndexnumberNoZeros.Length > 10) && (hLSevenLabRecs.IndexNumber.Length > 9))
                        {
                            hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.IndexNumber.Substring(6).TrimStart('0');
                            if (hLSevenLabRecs.IndexnumberNoZeros.Length > 8)
                            {
                                int indexOne = hLSevenLabRecs.IndexNumber.IndexOf("1");
                                if (indexOne > 0)
                                {
                                    string tempStr = hLSevenLabRecs.IndexnumberNoZeros = hLSevenLabRecs.IndexNumber.Substring(indexOne).TrimStart('0');
                                    if ((tempStr.Length == 10) || (tempStr.Length == 9))
                                        hLSevenLabRecs.IndexnumberNoZeros = tempStr;
                                }

                            }


                        }
                        if ((string.IsNullOrWhiteSpace(hLSevenLabRecs.PatientID)) || (hLSevenLabRecs.PatientID.Length < 9))
                        {
                            if (hLSevenLabRecs.IndexNumber.Length > 12)
                                hLSevenLabRecs.PatientID = hLSevenLabRecs.IndexNumber.Substring(6).TrimStart('0');
                            else
                            {
                                if (hLSevenLabRecs.PatientID.Length < 8)
                                    hLSevenLabRecs.PatientID = string.Empty;
                            }

                        }
                        hLSevenLabRecs.MRN = hLSevenLabRecs.PatientID;
                        //if ((hLSevenLabRecs.MRN.StartsWith("5000")))
                        //    hLSevenLabRecs.PatientID = hLSevenLabRecs.MRN;
                        //else
                        //{
                        //    if (!(string.IsNullOrWhiteSpace(hLSevenLabRecs.PatientID)))
                        //    {
                        //        if (hLSevenLabRecs.PatientID.Length > 9)
                        //        {
                        //            if (hLSevenLabRecs.IndexNumber.Length == 15)
                        //            {


                        //                hLSevenLabRecs.PatientID = hLSevenLabRecs.IndexNumber.Substring(9).TrimStart('0');
                        //            }
                        //        }
                        //        if (hLSevenLabRecs.PatientID.Length >= 9)
                        //            hLSevenLabRecs.PatientID = string.Empty;


                        //    }
                        //    if (!(string.IsNullOrWhiteSpace(hLSevenLabRecs.MRN)))
                        //    {
                        //        if (hLSevenLabRecs.MRN.Length > 8)
                        //        {
                        //            if (hLSevenLabRecs.IndexNumber.Length == 15)
                        //            {
                        //                hLSevenLabRecs.MRN = hLSevenLabRecs.IndexNumber.Substring(9).TrimStart('0');

                        //            }
                        //        }

                        //        if (hLSevenLabRecs.MRN.Length >= 9)
                        //        {

                        //            hLSevenLabRecs.MRN = string.Empty;
                        //        }


                        //    }
                        //}
                        //if ((string.IsNullOrWhiteSpace(hLSevenLabRecs.RequisitionNumber)) && (hLSevenLabRecs.IndexnumberNoZeros.StartsWith("201")))
                        //    hLSevenLabRecs.RequisitionNumber = hLSevenLabRecs.IndexnumberNoZeros;


                        yield return hLSevenLabRecs;
                    }
                }
            }
        }
    }
}
