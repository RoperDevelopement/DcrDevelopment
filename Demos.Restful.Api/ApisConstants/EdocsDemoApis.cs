using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Demos.Restful.Api.Models;
using Newtonsoft.Json;
using Edocs.Demos.Restful.Api.ApisConstants;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;

namespace Edocs.Demos.Restful.Api.ApisConstants
{
    public class EdocsDemoApis
    {
        private static EdocsDemoApis instance = null;

        private EdocsDemoApis() { }

        public static EdocsDemoApis DemoInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EdocsDemoApis();
                }
                return instance;
            }
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> UploadBSBProdDepRecords(BSPProdDepUloadRecords jsonUploadModel, string sqlConnectionString, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var js = JsonConvert.SerializeObject(jsonUploadModel);
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmaJasonFile, js));
                        return (JsonBasicApis.JsonInstance.GetJsonResultsString(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult());

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {spName} {ex.Message}");

            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> UploadBSBPWDRec(BSBPWDRecords bSBPWD, string sqlConnectionString, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                //  var js = JsonConvert.SerializeObject(jsonUploadModel);
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectDepartment, bSBPWD.ProjectDepartment));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectYear, bSBPWD.ProjectYear));

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectName, bSBPWD.ProjectName));

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmFileName, bSBPWD.FileName));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmFileUrl, bSBPWD.FileUrl));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmScanOperator, bSBPWD.ScanOperator));
                        return (JsonBasicApis.JsonInstance.GetJsonResultsString(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult());


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {spName} {ex.Message}");

            }
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetBSBPWDProjectNameYears(string sqlConnectionString, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                //  var js = JsonConvert.SerializeObject(jsonUploadModel);
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        return (JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult());


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {spName} {ex.Message}");

            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task UploadBSBProdDepSearchText(BSPProdDeptUploadSearchTxt deptUploadSearchTxt, string sqlConnectionString, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                //  var js = JsonConvert.SerializeObject(deptUploadSearchTxt);
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmPermitNum, deptUploadSearchTxt.PermitNumber));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmSearchText, deptUploadSearchTxt.SearchStr));
                        JsonBasicApis.JsonInstance.GetJsonResultsString(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {spName} {ex.Message}");

            }
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetBSBProdDepPermitRecords(int permitNum, string address, bool addSW, int parcelNumber, int exePermitNumber, int zoneNumber, string goCode, string constCompany, string ownerLot, string sqlConnectionString, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmPermitNum, permitNum));
                        if (string.Compare(address, "na", true) != 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmAddress, address));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmAddressSW, addSW));

                        return (JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult());

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {spName} {ex.Message}");

            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetBSBPWDPNames(string pName, string sqlConnectionString, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectName, pName));
                        

                        return (JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult());

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {spName} {ex.Message}");

            }
        }

        public async Task<JsonResult> GetBSBProdDepPermitRecordsbyKeyWord(string keyWords, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                keyWords = keyWords.Replace(EdocsDemoConstants.DoubleQuotes, "");
                string wereCluse = string.Empty;
                string selStr = "SELECT bsp.[PermitNum] as PermitNum,bsp.[FileName] as [FileName],bsp.[FileUrl] as FileUrl,bsp.[OwnerLot] as OwnerLot,bsp.[ConstCompany] as ConstCompany,bsp.[ParcelNumber] as ParcelNumber,bsp.[ZoneNumber] as ZoneNumber,bsp.[GoCode] as GoCode,bsp.[ExePermitNumber] as ExePermitNumber,bsp.[Address] as [Address],bsp.[DateIssued] as DateIssued,bsp.[DateExperied] DateExperied  FROM [dbo].[ButteSilverBowPlanningDepFullText] ft join ButteSilverBowPlanningDep bsp on ft.PermitNumber = bsp.PermitNum";
                //where[PermitNum] = @PermitNum;"
                //     CONTAINS([PDFLabReqsFullText], @srcStr)) set @srcStr= '"'+@KeyWordSearch+'*"';
                foreach (string keyword in keyWords.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(keyword))
                        continue;
                    if (string.IsNullOrWhiteSpace(wereCluse))
                    {
                        //    wereCluse = $"'{EdocsDemoConstants.DoubleQuotes}{keyword}{EdocsDemoConstants.DoubleQuotes}'";
                        if (string.IsNullOrWhiteSpace(wereCluse))
                            wereCluse = $"CONTAINS(ft.[FullTextSearch],'{EdocsDemoConstants.DoubleQuotes}{keyword}*{EdocsDemoConstants.DoubleQuotes}')";

                    }
                    else
                        wereCluse = $"{wereCluse} or CONTAINS(ft.[FullTextSearch],'{EdocsDemoConstants.DoubleQuotes}{keyword}*{EdocsDemoConstants.DoubleQuotes}')";
                }
                // selStr = $"{selStr}  where CONTAINS(ft.[FullTextSearch],{wereCluse})";
                selStr = $"{selStr} where {wereCluse}";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(selStr, sqlConnection))
                    {

                        sqlCmd.CommandType = CommandType.Text;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                        var dt = new DataTable();
                        dt.BeginLoadData();
                        dt.Load(dr);
                        dt.EndLoadData();
                        dt.AcceptChanges();

                        JsonResult jsonResult = new JsonResult(dt);
                        return jsonResult;

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error looking up keywords  {keyWords} {ex.Message}");

            }
        }


        public async Task<JsonResult> GetBSBPWDRecordsbyKeyWord(string keyWords, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                keyWords = keyWords.Replace(EdocsDemoConstants.DoubleQuotes, "");
                string wereCluse = string.Empty;
                string selStr = "SELECT dep.[ProjectDepartment] as ProjectDepartment,[ProjectYear] as ProjectYear,[ProjectName] as ProjectName,[FileName] as ProdFileName,[FileUrl] as FileUrl    FROM[dbo].[BSBPlublicWorksDepartmentFullText] bsbpwft join [dbo].[BSBPlubicWorksDepartmentProjects] proj on bsbpwft.[ProjectNameID] = proj.[ID] join [dbo].BSBPublicWorksProjectDepartment dep on proj.ProjectDepartment=dep.ID";
                //where[PermitNum] = @PermitNum;"
                //     CONTAINS([PDFLabReqsFullText], @srcStr)) set @srcStr= '"'+@KeyWordSearch+'*"';
                foreach (string keyword in keyWords.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(keyword))
                        continue;
                    if (string.IsNullOrWhiteSpace(wereCluse))
                    {
                        //    wereCluse = $"'{EdocsDemoConstants.DoubleQuotes}{keyword}{EdocsDemoConstants.DoubleQuotes}'";
                        if (string.IsNullOrWhiteSpace(wereCluse))
                            wereCluse = $"CONTAINS(bsbpwft.[FullTextSearch],'{EdocsDemoConstants.DoubleQuotes}{keyword}*{EdocsDemoConstants.DoubleQuotes}')";

                    }
                    else
                        wereCluse = $"{wereCluse} or CONTAINS(bsbpwft.[FullTextSearch],'{EdocsDemoConstants.DoubleQuotes}{keyword}*{EdocsDemoConstants.DoubleQuotes}')";
                }
                // selStr = $"{selStr}  where CONTAINS(ft.[FullTextSearch],{wereCluse})";
                selStr = $"{selStr} where {wereCluse}";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(selStr, sqlConnection))
                    {

                        sqlCmd.CommandType = CommandType.Text;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                        var dt = new DataTable();
                        dt.BeginLoadData();
                        dt.Load(dr);
                        dt.EndLoadData();
                        dt.AcceptChanges();

                        JsonResult jsonResult = new JsonResult(dt);
                        return jsonResult;

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error looking up keywords  {keyWords} {ex.Message}");

            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetBSBPWDRecords(string projectDepartment, string projectYear, string projectName, string keyWords, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                if (string.Compare(keyWords, "NA", true) != 0)
                {
                    return GetBSBPWDRecordsbyKeyWord(keyWords, sqlConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsDemoConstants.SpGetBSBPWDRecords, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (string.Compare(projectDepartment, "NA", true) != 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectDepartment, projectDepartment));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectDepartment, DBNull.Value));
                        if (string.Compare(projectYear, "NA", true) != 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectYear, projectYear));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectYear, DBNull.Value));
                        if (string.Compare(projectName, "NA", true) != 0)
                        {
                            projectName = projectName.Replace(EdocsDemoConstants.DoubleQuotes, "");
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectName, projectName.Trim()));
                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsDemoConstants.SpParmProjectName, DBNull.Value));

                        return (JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult());

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Calling Sp {EdocsDemoConstants.SpGetBSBPWDRecords} {ex.Message}");

            }
        }

    }
}
