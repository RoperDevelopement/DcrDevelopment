using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using System.Data;
using Edocs.PSE.Api.Models;
using System.Data.SqlClient;
using Edocs.WebApi.ApiClasses;
using Microsoft.AspNetCore.Mvc;
namespace Edocs.PSE.Api.ApisConstants
{
    public sealed class PSEApis
    {
        static readonly PSEApis _instance = new PSEApis();
        public static PSEApis PSEApisInstance
        {
            get
            {
                return _instance;
            }
        }
        PSEApis()
        {
            // Initialize.
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task UploadPESRecords(PSEJsonUploadModel jsonUploadModel, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {

                UploadPESStudentRecords(jsonUploadModel, sqlConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }


        public async Task UploadBSBRecords(BSBUploadJsonModel jsonUploadModel, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {

                UploadBSBArchiveRecords(jsonUploadModel, sqlConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task UploadPESStudentRecords(PSEJsonUploadModel jsonUploadModel, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var js = JsonConvert.SerializeObject(jsonUploadModel);
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(jsonUploadModel.AzureSPName, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaJasonFile, js));
                    JsonBasicApis.JsonInstance.ExecSP(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
        }

        public async Task<JsonResult> GetBSBArchiveRecordByid(int archiveID, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(PSEConstants.SpGetBSBArchivedRecordByID, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaArchiveID,archiveID));
                    return JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
        }
        public async Task<JsonResult> BSBArchiveRecords(BSBLoockUpArchivesModel bSBLoockUpArchives, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(PSEConstants.SpGetBSBArchivedRecords, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    if(string.IsNullOrWhiteSpace(bSBLoockUpArchives.ArchiveCollection))
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaCollection,DBNull.Value));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaCollection, bSBLoockUpArchives.ArchiveCollection));

                    if (string.IsNullOrWhiteSpace(bSBLoockUpArchives.ArchiveDate))
                                                sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaArchiveDate, DBNull.Value));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaArchiveDate, bSBLoockUpArchives.ArchiveDate));

                   
                    if (string.IsNullOrWhiteSpace(bSBLoockUpArchives.KeyWord))
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaFullTextSearch, DBNull.Value));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaFullTextSearch, bSBLoockUpArchives.KeyWord));
                    if (string.IsNullOrWhiteSpace(bSBLoockUpArchives.ArchiveTitle))
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaTitle, DBNull.Value));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaTitle, bSBLoockUpArchives.ArchiveTitle));

                    return  JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task UploadBSBArchiveRecords(BSBUploadJsonModel jsonUploadModel, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var js = JsonConvert.SerializeObject(jsonUploadModel);
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(jsonUploadModel.AzureSPName, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaJasonFile, js));
                    JsonBasicApis.JsonInstance.ExecSP(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetStudentRecords(string sqlConnectionString, string studentFirstName, string studentLastName, DateTime studentDOB, string storedProcedure, DateTime scanStDate, DateTime scanEndDate)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    if (string.Compare(storedProcedure, PSEConstants.SpGetStudentRecordFirstLastName, true) == 0)
                    {
                        if (string.Compare(studentFirstName,"NA",true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStudentFirstName, DBNull.Value));
                        else
                                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStudentFirstName, studentFirstName));

                        if (string.Compare(studentLastName, "NA", true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStudentLastName, DBNull.Value));

                        else
                                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStudentLastName, studentLastName));
                        
                    }
                    else if (string.Compare(storedProcedure, PSEConstants.SpGetStudentRecordDOB, true) == 0)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStudentDateOfBirth, studentDOB));
                    }
                    else if ((string.Compare(storedProcedure, PSEConstants.SpGetStudentRecordScanDate, true) == 0) || ((string.Compare(storedProcedure, PSEConstants.SpGetStudentRecordDOBDateRange, true) == 0)))
                    {
                        if (scanEndDate.Date == DateTime.Now.Date)
                            scanEndDate = DateTime.Now.AddDays(1);
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaScanStDate,scanStDate));
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaScanEndDate, scanEndDate));
                    }
                    else
                        throw new Exception("Invalid args for method GetStudentRecords");
                    return JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetFinancialRecords(string sqlConnectionString, DateTime startFinRecYear, DateTime endFinRecYear, string finCat,
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
                    string spName, string scanStDate, string scanEndDate)
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    
                        if (startFinRecYear.Year < 1980)
                             sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStartFinYear, DBNull.Value));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaStartFinYear,startFinRecYear));

                        if (endFinRecYear.Year< 1980)
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaEndFinYear, DBNull.Value));

                        else
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaEndFinYear, endFinRecYear));

                    
                     if (string.Compare(finCat, PSEConstants.NA, true) == 0)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaFinCategory, DBNull.Value));
                    }
                     else
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaFinCategory, finCat));
                     if(string.Compare(scanStDate, PSEConstants.NA, true) == 0)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaScanStDate, DBNull.Value));
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaScanEndDate, DBNull.Value));
                    }
                    else 
                    {
                        
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaScanStDate, scanStDate));
                        sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaScanEndDate, scanEndDate));
                    }
                     
                    return JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetPSEFinancialCategories(string sqlConnectionString, string storedProcedure)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return JsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionString, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> CheckEmptyStr(string inStr)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if(string.IsNullOrWhiteSpace(inStr))
                return PSEConstants.NA;
            return inStr;
        }
    }
}
