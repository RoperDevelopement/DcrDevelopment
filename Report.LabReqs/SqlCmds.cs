using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Edocs.Report.LabReqs.Models;
using Edocs.HelperUtilities;
using System.Runtime.Remoting.Messaging;

namespace Edocs.Report.LabReqs
{
    class SqlCmds
    {
        private string SqlSever
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(LabReqRepConst.AppKeySqlServerName); } }

        private string DbUserName
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(LabReqRepConst.AppKeySqlServerUserName); } }

        private string DbPasswod
        { get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(LabReqRepConst.AppKeySqlServerUserPw); } }

        private string SqlDataBaseName
        {
            get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(LabReqRepConst.AppKeySqlDBName); }
        }
        private SqlConnection SqlConn
        { get; set; }
        private async Task<SqlConnection> OpenSqlConnection()
        {

            string connStr = GetConnectionString().ConfigureAwait(false).GetAwaiter().GetResult(); ;

            SqlConnection sqlConnection = new SqlConnection(connStr);
            sqlConnection.OpenAsync().ConfigureAwait(true).GetAwaiter().GetResult();
            return sqlConnection;
        }
        private async Task<string> GetConnectionString()
        {

            //return string.Format("Server={0};Database={1};Trusted_Connection=True; User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=True;", SqlSever, dbName, DbUserName, DbPasswod, 1000);

            return string.Format("Server={0};Database={1};User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=true;", SqlSever, SqlDataBaseName, DbUserName, DbPasswod, 180);
        }
        public IEnumerable<LabReqModel> GetLabRecs(DateTime scanDateStart, DateTime scanDateEnd, string spName)
        {
            SqlConn = OpenSqlConnection().ConfigureAwait(false).GetAwaiter().GetResult();
          


                using (SqlCommand cmd = new SqlCommand(spName, SqlConn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(LabReqRepConst.SpParamScanStartDate, scanDateStart));
                    cmd.Parameters.Add(new SqlParameter(LabReqRepConst.SpParmScanEndDate, scanDateEnd));
                    SqlDataReader dr = cmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    if (dr.HasRows)
                    {
                        while (dr.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {

                            LabReqModel reqModel = new LabReqModel();
                            reqModel.BatchID = dr[LabReqRepConst.FieldBatchID].ToString();
                            reqModel.FinNumber = dr[LabReqRepConst.FieldFinNumber].ToString();
                            reqModel.DateOfServices = DateTime.Parse(dr[LabReqRepConst.FieldDateOfServices].ToString());
                            reqModel.ScanDate = DateTime.Parse(dr[LabReqRepConst.FieldScanDate].ToString());
                            reqModel.IndexNumber = dr[LabReqRepConst.FieldIndexNumber].ToString();
                            reqModel.Merged = bool.Parse(dr[LabReqRepConst.FieldMerged].ToString());
                            if (!DBNull.Value.Equals(dr[LabReqRepConst.FieldMRN]))
                            {
                                reqModel.MRN = dr[LabReqRepConst.FieldMRN].ToString();
                            }
                            else
                                reqModel.MRN = "N/A";
                            if (!DBNull.Value.Equals(dr[LabReqRepConst.FieldPatID]))
                            {
                                reqModel.PatID = dr[LabReqRepConst.FieldPatID].ToString();
                            }
                            else
                                reqModel.PatID = "N/A";
                            if (!DBNull.Value.Equals(dr[LabReqRepConst.FieldReqNum]))
                            {
                                reqModel.ReqNum = dr[LabReqRepConst.FieldReqNum].ToString();
                            }
                            else
                                reqModel.ReqNum = "N/A";

                            if (reqModel.IndexNumber.Length == 16)
                            {
                                reqModel.ClientCode = reqModel.IndexNumber.Substring(0, 6);
                            }
                            else
                                reqModel.ClientCode = "N/A";

                            yield return reqModel;
                        }
                    }
                }
           
        }
    }
}
