using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging;
using System.Threading;
using EdocsUSA.Merge.Doh.Models;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;

namespace EdocsUSA.Merge.Doh
{
    class SqlCmd
    {
        private readonly string AppKeySqlServerName = "SqlServerName";
        private readonly string AppKeySqlServerLabRecsDbName = "SqlServerLabRecsDbName";
        private readonly string AppKeySPMergeDOHRecs = "SPMergeDOHRecs";
        private readonly string AppKeySqlServerUserName = "SqlServerUserName";
        private readonly string AppKeySqlServerHl7DbName = "SqlServerHl7DbName";
        private readonly string AppKeyNumberTimesToTryUpdateDOH = "NumberTimesToTryUpdateDOH";


        private readonly string AppKeySqlServerUserPw = "SqlServerUserPw";
        private readonly string AppKeyThreadSleepSeconds = "ThreadSleepSeconds";
        private readonly string AppKeySPGetDOHHL7Files = "SPGetDOHHL7Files";
        private readonly string AppKeySPUpDateDOHRecsByID = "SPUpDateDOHRecsByID";

        private readonly string SpParamaStartDate = "@StartDate";
        private readonly string SpParamaEndDate = "@EndDate";
        private readonly string SpParamaMergeDOH = "@MergeDOH ";

        private readonly string SpParamaDateOfService = "@DateOfService";
        private readonly string SpParamaDOHReqID = "@DOHReqID";
        private readonly string SpParamaEndDateOfService = "@EndDateOfService";
        private readonly string SpParamaPatientID = "@PatientID";
        private readonly string SpParamaPatientFirstName = "@PatientFirstName";
        private readonly string SpParamaPatientLastName = "@PatientLastName";
        private readonly string SpParamaAccessionNumber = "@AccessionNumber";
        private readonly string SpParamaClientCode = "@ClientCode";
        private readonly string SpParamaDrCode = "@DrCode";

        private readonly string SpParamaDrFirstName = "@DrFirstName";
        private readonly string SpParamaDrLastName = "@DrLastName";
        private readonly string SpParamaUserModified = "@UserModified";

        private readonly string FieldNameDOHID = "DOHID";
        private readonly string FieldNameMRN = "MRN";
        private readonly string FieldNameClientCode = "ClientCode";

        private readonly string FieldNameAccessionNumber = "AccessionNumber";
        private readonly string FieldNameDateOfService = "DateOfService";

        private readonly string FieldNamePatientLastName = "PatientLastName";
        private readonly string FieldNamePatientFirstName = "PatientFirstName";
        private readonly string FieldNameDrCode = "DrCode";
        private readonly string FieldNameDrName = "DrName";






        private readonly int MillSecond = 1000;



        public string SPUpDateDOHRecsByID
        {
            get
            {
                return
              Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySPUpDateDOHRecsByID);
            }
        }
        private string ThreadSleepSeconds
        {
            get
            {
                return
              Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyThreadSleepSeconds);
            }
        }
        private string NumberTimesToTryUpdateDOH
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeyNumberTimesToTryUpdateDOH); }
        }
        private string SqlServerHl7DbName
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerHl7DbName); }
        }

        private string DBName
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerLabRecsDbName); }
        }
        private string SPGetDOHHL7Files
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySPGetDOHHL7Files); }
        }




        private string SqlSever
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerName); } }
        private string DbPasswod
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerUserPw); } }
        private string DbUserName
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySqlServerUserName); } }
        private string SqlDataBaseName(string dbName)
        {
            return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(dbName);
        }
        private string SpGetDOHRecsByIDToMerge
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppKeySPMergeDOHRecs); } }
        private SqlConnection SqlConn
        { get; set; }

        private SqlConnection OpenSqlConnection(string dbName)
        {

            string connStr = GetConnectionString(dbName);
            // TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opening sql connection connection string for database name {dbName}");
            SqlConnection sqlConnection = new SqlConnection(connStr);
            sqlConnection.Open();
            return sqlConnection;
        }
        private string GetConnectionString(string dbName)
        {
            // dbName = SqlDataBaseName(dbName);
            //return string.Format("Server={0};Database={1};Trusted_Connection=True; User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=True;", SqlSever, dbName, DbUserName, DbPasswod, 1000);
            //TL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Using Connectin Server={0};Database={1};Connect Timeout={2};MultipleActiveResultSets=true;", SqlSever, dbName, 180));
            return string.Format("Server={0};Database={1};User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=true;", SqlSever, dbName, DbUserName, DbPasswod, 180);
        }

        public async Task<HL7Model> GetHL7Records(DOHRecsModel dOHRecs, DateTime sDate)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"GetHL7Records for date range start date {sDate.ToString()} end date {dOHRecs.DateOfService.ToString()} doh rec id {dOHRecs.DOHID}");
            HL7Model model = new HL7Model();
            SqlConn = OpenSqlConnection(SqlServerHl7DbName);
            try
            {
                using (SqlCommand cmd = new SqlCommand(SPGetDOHHL7Files, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParamaAccessionNumber, dOHRecs.AccessionNumber));
                    cmd.Parameters.Add(new SqlParameter(SpParamaEndDateOfService, dOHRecs.DateOfService));
                    cmd.Parameters.Add(new SqlParameter(SpParamaDateOfService, sDate));
                    cmd.Parameters.Add(new SqlParameter(SpParamaPatientID, dOHRecs.MRN));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.AccessionNumber = (string)reader[FieldNameAccessionNumber];
                            model.DateOfService = (DateTime)reader[FieldNameDateOfService];
                            model.MRN = (string)reader[FieldNameMRN];
                            model.DrCode = (string)reader[FieldNameDrCode];
                            
                            model.ClientCode = (string)reader[FieldNameClientCode];
                            string drName = (string)reader[FieldNameDrName];
                            if (!(string.IsNullOrWhiteSpace(drName)))
                            {
                                string[] splitDrName = drName.Split(',');
                                if (splitDrName.Length > 0)
                                {
                                    model.DrFName = splitDrName[1];
                                }
                                model.DrLName = splitDrName[0];
                            }
                            model.PatientFirstName = (string)reader[FieldNamePatientFirstName];
                            model.PatientLastName = (string)reader[FieldNamePatientLastName];
                            model.PatientID = model.MRN;

                            return model;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"GetHL7Records for date range start date {sDate.ToString()} end date {dOHRecs.DateOfService.ToString()} doh rec id {dOHRecs.DOHID} {ex.Message}");
            }

            return null;
        }
        public async Task UpdateDOHTable(HL7Model hL7, DOHRecsModel dOHRecs, string spNameUpDate)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating doh AccessionNumber {dOHRecs.AccessionNumber} patid {dOHRecs.MRN} for doh id {dOHRecs.DOHID}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Updating doh AccessionNumber {dOHRecs.AccessionNumber} patid {dOHRecs.MRN}");
            int numLoop = Edocs.HelperUtilities.Utilities.ParseInt(NumberTimesToTryUpdateDOH);
            int tSleep = Edocs.HelperUtilities.Utilities.ParseInt(ThreadSleepSeconds) * MillSecond;
            string errmess = string.Empty;

            while (numLoop-- > 0)
            {
                try
                {

                    UpdateDOHRecs(hL7, dOHRecs, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                    errmess = string.Empty;
                    break;
                }
                catch (Exception ex)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Num Loop {numLoop}  {ex.Message}");
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Sql Server error Num Loop {numLoop}  {ex.Message}");
                    errmess = $"Updating doh AccessionNumber {dOHRecs.AccessionNumber} patid {dOHRecs.MRN} for doh id {dOHRecs.DOHID} {ex.Message}";
                    Thread.Sleep(tSleep);
                    // CheckMergeRunning(tSleep).ConfigureAwait(false).GetAwaiter().GetResult();
                }

            }


            if (!(string.IsNullOrWhiteSpace(errmess)))
                throw new Exception(errmess);

        }
        private async Task UpdateDOHRecs(HL7Model hL7, DOHRecsModel dOHRecs, string spNameUpDate)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"UpdateDOHRecs for doh id {dOHRecs.DOHID}");
            try
            {
                SqlConn = OpenSqlConnection(DBName);
                using (SqlCommand cmd = new SqlCommand(spNameUpDate, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParamaDOHReqID, dOHRecs.DOHID));
                    if (string.IsNullOrWhiteSpace(hL7.MRN))
                    {
                        if(!(string.IsNullOrWhiteSpace(dOHRecs.MRN)))
                            cmd.Parameters.Add(new SqlParameter(SpParamaPatientID, dOHRecs.MRN));
                        else
                        cmd.Parameters.Add(new SqlParameter(SpParamaPatientID, DBNull.Value));
                    }
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaPatientID, hL7.MRN));

                    if (string.IsNullOrWhiteSpace(hL7.PatientFirstName))
                        cmd.Parameters.Add(new SqlParameter(SpParamaPatientFirstName, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaPatientFirstName, hL7.PatientFirstName));

                    if (string.IsNullOrWhiteSpace(hL7.PatientLastName))
                        cmd.Parameters.Add(new SqlParameter(SpParamaPatientLastName, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaPatientLastName, hL7.PatientLastName));

                    if (string.IsNullOrWhiteSpace(hL7.ClientCode))
                        cmd.Parameters.Add(new SqlParameter(SpParamaClientCode, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaClientCode, hL7.ClientCode));


                    if (string.IsNullOrWhiteSpace(dOHRecs.AccessionNumber))
                        cmd.Parameters.Add(new SqlParameter(SpParamaAccessionNumber, DBNull.Value));
                    else
                    {
                        //if ((dOHRecs.AccessionNumber.StartsWith("0000")) && (!(string.IsNullOrWhiteSpace(hL7.AccessionNumber))))

                        //    cmd.Parameters.Add(new SqlParameter(SpParamaAccessionNumber, hL7.AccessionNumber));
                        //else
                        cmd.Parameters.Add(new SqlParameter(SpParamaAccessionNumber, dOHRecs.AccessionNumber));

                    }

                    if (string.IsNullOrWhiteSpace(hL7.DrCode))
                        cmd.Parameters.Add(new SqlParameter(SpParamaDrCode, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaDrCode, hL7.DrCode));

                    if (string.IsNullOrWhiteSpace(hL7.DrFName))
                        cmd.Parameters.Add(new SqlParameter(SpParamaDrFirstName, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaDrFirstName, hL7.DrFName));

                    if (string.IsNullOrWhiteSpace(hL7.DrLName))
                        cmd.Parameters.Add(new SqlParameter(SpParamaDrLastName, DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter(SpParamaDrLastName, hL7.DrLName));
                   
                    cmd.Parameters.Add(new SqlParameter(SpParamaUserModified, Environment.UserName));
                    cmd.Parameters.Add(new SqlParameter(SpParamaDateOfService,hL7.DateOfService));
                    cmd.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"UpdateDOHRecs for doh id {dOHRecs.DOHID} {ex.Message}");
            }
        }
        public IEnumerable<DOHRecsModel> GetMergeDOHRecs(DateTime startDate, DateTime endDate, int merge)
        {

            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"GetMergeDOHRecs for start date {startDate.ToString()} end date {endDate.ToString()} merge {merge}");

                SqlConn = OpenSqlConnection(DBName);
                using (SqlCommand cmd = new SqlCommand(SpGetDOHRecsByIDToMerge, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(SpParamaStartDate, startDate));
                    cmd.Parameters.Add(new SqlParameter(SpParamaEndDate, endDate));
                    cmd.Parameters.Add(new SqlParameter(SpParamaMergeDOH, merge));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DOHRecsModel recsModel = new DOHRecsModel();
                            recsModel.DOHID = (int)reader[FieldNameDOHID];
                            recsModel.AccessionNumber = (string)reader[FieldNameAccessionNumber];
                            // recsModel.FileName = (string)reader[FieldNameFileName];

                            recsModel.DateOfService = (DateTime)reader[FieldNameDateOfService];
                        if(!(reader.IsDBNull(reader.GetOrdinal(FieldNameMRN))))
                            recsModel.MRN = (string)reader[FieldNameMRN];
                            yield return recsModel;
                        }
                    }

                }
            }
            
        
    }
}
